using UnityEngine;
using TMPro;
using DG.Tweening;

namespace UI
{
    public class CashDisplayController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI cashText;

        [Header("Settings")]
        [SerializeField] private float counterDuration = 0.5f;

        private int currentCash = 0;

        public void Initialize()
        {
            ValidateReferences();
            ResetCash();
            Debug.Log("[CashDisplayController] Initialized");
        }

        private void ValidateReferences()
        {
            if (cashText == null)
                Debug.LogError("[CashDisplayController] CashText is not assigned!");
        }

        public void AddCash(int amount)
        {
            if (amount == 0) return;

            int oldAmount = currentCash;
            currentCash += amount;
            
            if (currentCash < 0)
                currentCash = 0;

            AnimateCashChange(oldAmount, currentCash);
            Debug.Log($"[CashDisplayController] Changed by ${amount} (Total: ${currentCash})");
        }

        public void SetCash(int amount)
        {
            currentCash = amount;
            UpdateCashDisplay(currentCash);
        }

        public void ResetCash()
        {
            currentCash = 0;
            UpdateCashDisplay(0);
        }

        private void UpdateCashDisplay(int amount)
        {
            if (cashText == null) return;
            cashText.text = amount.ToString();
        }

        private void AnimateCashChange(int from, int to)
        {
            if (cashText == null) return;

            DOTween.To(
                () => from,
                x => UpdateCashDisplay(x),
                to,
                counterDuration
            ).SetEase(Ease.OutQuad);

            cashText.transform.DOScale(1.2f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }

        public int GetCurrentCash()
        {
            return currentCash;
        }

        #region Test Functions (Inspector Context Menu)

        [ContextMenu("Add $500")]
        private void TestAdd500()
        {
            AddCash(500);
            Debug.Log("[CashDisplayController] TEST: Added $500");
        }

        [ContextMenu("Add $1000")]
        private void TestAdd1000()
        {
            AddCash(1000);
            Debug.Log("[CashDisplayController] TEST: Added $1000");
        }

        [ContextMenu("Add $5000")]
        private void TestAdd5000()
        {
            AddCash(5000);
            Debug.Log("[CashDisplayController] TEST: Added $5000");
        }

        #endregion
    }
}
