using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using WheelSystem;
using ZoneSystem;
using TMPro;
using SpinSystem;

namespace UI
{
    public class WheelViewController : MonoBehaviour
    {
        [Header("Wheel Components")]
        [SerializeField] private Image wheelBase;
        [SerializeField] private Image wheelIndicator;
        [SerializeField] private Transform wheelContainer;
        [SerializeField] private TextMeshProUGUI wheelNameText;

        [Header("Slots")]
        [SerializeField] private Image[] rewardSlots = new Image[8];
        [SerializeField] private TextMeshProUGUI[] slotAmountTexts = new TextMeshProUGUI[8];

        [Header("Animation Settings")]
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private int minSpins = 3;
        [SerializeField] private int maxSpins = 5;

        private bool isSpinning;
        private RewardSystem.Reward lastReward;
        private bool lastResultWasBomb;

        private const int SLOT_COUNT = 8;

        #region Init

        public void Initialize()
        {
            ValidateReferences();
            
            if (ZoneManager.Instance != null)
            {
                UpdateWheelForZone(ZoneManager.Instance.CurrentZone);
            }
            
            Debug.Log("[WheelViewController] Initialized");
        }

        private void ValidateReferences()
        {
            if (!wheelContainer) Debug.LogError("WheelContainer missing");
            if (!wheelIndicator) Debug.LogError("Indicator missing");

            for (int i = 0; i < rewardSlots.Length; i++)
            {
                if (!rewardSlots[i])
                    Debug.LogError($"RewardSlot {i} missing");
            }
        }

        #endregion

        #region Wheel Visuals

        public void UpdateWheelForZone(int zone)
        {
            if (WheelManager.Instance == null)
                return;

            WheelManager.Instance.LoadWheelForZone(zone);

            if (WheelManager.Instance.CurrentWheel == null)
                return;

            UpdateWheelVisuals();
            UpdateSlotIcons();
        }

        private void UpdateWheelVisuals()
        {
            var currentWheel = WheelManager.Instance.CurrentWheel;
            
            if (wheelBase != null && currentWheel.wheelSprite != null)
            {
                wheelBase.sprite = currentWheel.wheelSprite;
                Debug.Log($"[WheelViewController] Updated wheel visual to: {currentWheel.wheelName}");
            }
            
            if (wheelNameText != null && !string.IsNullOrEmpty(currentWheel.wheelName))
            {
                wheelNameText.text = currentWheel.wheelName;
            }
        }

        private void UpdateSlotIcons()
        {
            var slices = WheelManager.Instance.CurrentWheel.slices;

            for (int i = 0; i < rewardSlots.Length && i < slices.Count; i++)
            {
                rewardSlots[i].sprite = slices[i].SliceIcon;
                rewardSlots[i].enabled = true;

                slotAmountTexts[i].text = GetSlotAmountText(slices[i]);
            }
        }

        private string GetSlotAmountText(WheelSlice slice)
        {
            var reward = slice.rewardConfig.CreateReward();

            return reward switch
            {
                RewardSystem.CashReward r => $"x{r.Amount}",
                RewardSystem.PointReward r => $"x{r.Amount}",
                RewardSystem.GoldReward r => $"x{r.Amount}",
                RewardSystem.ArmorReward r => $"x{r.Amount}",
                RewardSystem.WeaponReward r => $"x{r.Amount}",
                RewardSystem.ChestReward r => $"x{r.Amount}",
                RewardSystem.ConsumableReward r => $"x{r.Amount}",
                _ => "x1"
            };
        }

        #endregion

        #region Spin Flow

        public void StartSpin()
        {
            if (isSpinning) return;
            isSpinning = true;
        }

        public void CompleteSpin(WheelSlice result)
        {
            if (WheelManager.Instance == null || WheelManager.Instance.CurrentWheel == null)
            {
                isSpinning = false;
                return;
            }

            int selectedIndex = WheelManager.Instance.CurrentWheel.slices.IndexOf(result);
            if (selectedIndex < 0)
            {
                Debug.LogError("Selected slice not found!");
                isSpinning = false;
                return;
            }

            lastResultWasBomb = result.IsBomb;
            lastReward = result.IsBomb ? null : result.rewardConfig.CreateReward();

            SpinToSlot(selectedIndex);
        }

        private void SpinToSlot(int selectedIndex)
        {
            float slotLocalAngle =
                rewardSlots[selectedIndex].transform.localEulerAngles.z;

            if (slotLocalAngle > 180f)
                slotLocalAngle -= 360f;

            float targetAngle = -slotLocalAngle;

            int extraSpins = Random.Range(minSpins, maxSpins + 1);

            float finalAngle = targetAngle - (360f * extraSpins);

            Debug.Log($"[Spin CW] SlotAngle={slotLocalAngle}, target={targetAngle}, final={finalAngle}");

            wheelContainer.DORotate(
                new Vector3(0, 0, finalAngle),
                spinDuration,
                RotateMode.FastBeyond360
            )
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                wheelContainer.localEulerAngles = new Vector3(0, 0, targetAngle);
                HighlightSlot(selectedIndex);
                isSpinning = false;
            });
        }




        private void OnSpinComplete(int slotIndex, float targetAngle)
        {
            wheelContainer.localEulerAngles = new Vector3(0, 0, targetAngle);

            HighlightSlot(slotIndex);

            isSpinning = false;
        }

        #endregion

        #region Highlight & Reward

        private void HighlightSlot(int slotIndex)
        {
            Image slot = rewardSlots[slotIndex];

            slot.transform.DOScale(1.3f, 0.25f)
                .SetLoops(3, LoopType.Yoyo);

            Color originalColor = slot.color;
            DOVirtual.Color(originalColor, Color.yellow, 0.25f, (c) => slot.color = c)
                .SetLoops(3, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    slot.color = originalColor;
                    ProcessResult();
                });
        }

        public void PlayBombExplosion()
        {
            if (wheelContainer == null || wheelBase == null) return;

            wheelContainer.DOShakePosition(0.5f, strength: 20f, vibrato: 10);

            Color originalColor = wheelBase.color;
            DOVirtual.Color(originalColor, Color.red, 0.2f, (c) => wheelBase.color = c)
                .SetLoops(3, LoopType.Yoyo)
                .OnComplete(() => wheelBase.color = originalColor);

            Debug.Log("[WheelViewController] Bomb explosion effect played");
        }

        private void ProcessResult()
        {
            if (SpinController.Instance == null) return;

            if (lastResultWasBomb)
            {
                SpinController.Instance.ProcessBomb();
            }
            else if (lastReward != null)
            {
                SpinController.Instance.ProcessReward(lastReward);
                
                if (ZoneManager.Instance != null)
                {
                    ZoneManager.Instance.NextZone();
                }
            }

            lastReward = null;
            lastResultWasBomb = false;
        }

        #endregion
    }
}

   
