using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using ZoneSystem;

namespace UI
{
    public class ZoneButtonController : MonoBehaviour
    {
        [Header("Current Zone")]
        [SerializeField] private TextMeshProUGUI currentZoneValueText;

        [Header("Zone Buttons")]
        [SerializeField] private GameObject safeZoneButton;
        [SerializeField] private GameObject superZoneButton;
        [SerializeField] private TextMeshProUGUI safeZoneValueText;
        [SerializeField] private TextMeshProUGUI superZoneValueText;

        [Header("Settings")]
        [SerializeField] private int safeZoneInterval = 5;
        [SerializeField] private int superZoneInterval = 30;

        private int currentZone = 1;

        public void Initialize()
        {
            ValidateReferences();
            UpdateZone(1);
            Debug.Log("[ZoneButtonController] Initialized");
        }

        private void ValidateReferences()
        {
            if (currentZoneValueText == null)
                Debug.LogError("[ZoneButtonController] CurrentZoneValueText is not assigned!");
            
            if (safeZoneButton == null)
                Debug.LogError("[ZoneButtonController] SafeZoneButton is not assigned!");
            
            if (superZoneButton == null)
                Debug.LogError("[ZoneButtonController] SuperZoneButton is not assigned!");
            
            if (safeZoneValueText == null)
                Debug.LogError("[ZoneButtonController] SafeZoneValueText is not assigned!");
            
            if (superZoneValueText == null)
                Debug.LogError("[ZoneButtonController] SuperZoneValueText is not assigned!");
        }

        public void UpdateZone(int zone)
        {
            currentZone = zone;
            UpdateCurrentZoneDisplay();
            UpdateZoneButtonStates();
            AnimateZoneChange();
        }

        private void UpdateCurrentZoneDisplay()
        {
            if (currentZoneValueText == null) return;
            currentZoneValueText.text = currentZone.ToString();
        }

        private void UpdateZoneButtonStates()
        {
            int nextSafeZone = CalculateNextZone(currentZone, safeZoneInterval);
            int nextSuperZone = CalculateNextZone(currentZone, superZoneInterval);

            if (safeZoneValueText != null)
                safeZoneValueText.text = nextSafeZone.ToString();

            if (superZoneValueText != null)
                superZoneValueText.text = nextSuperZone.ToString();

            HighlightButton(safeZoneButton, currentZone % safeZoneInterval == 0 && currentZone % superZoneInterval != 0);
            HighlightButton(superZoneButton, currentZone % superZoneInterval == 0);
        }

        private int CalculateNextZone(int current, int interval)
        {
            if (current % interval == 0)
            {
                return current;
            }
            
            return ((current / interval) + 1) * interval;
        }

        private void HighlightButton(GameObject buttonObj, bool highlight)
        {
            if (buttonObj == null) return;

            if (highlight)
            {
                buttonObj.transform.DOScale(1.1f, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            }
            else
            {
                buttonObj.transform.DOKill();
                buttonObj.transform.localScale = Vector3.one;
            }
        }

        private void AnimateZoneChange()
        {
            if (currentZoneValueText == null) return;

            currentZoneValueText.transform.DOScale(1.3f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack);
        }

        private void OnDestroy()
        {
            if (safeZoneButton != null)
                safeZoneButton.transform.DOKill();
            
            if (superZoneButton != null)
                superZoneButton.transform.DOKill();
            
            if (currentZoneValueText != null)
                currentZoneValueText.transform.DOKill();
        }
    }
}
