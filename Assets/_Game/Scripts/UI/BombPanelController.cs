using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BombSystem;
using ConfigSystem;

namespace UI
{
    public class BombPanelController : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panel;

        [Header("Buttons")]
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button moneyReviveButton;
        [SerializeField] private Button adsReviveButton;

        [Header("Revive Config")]
        [SerializeField] private ReviveConfig reviveConfig;

        [Header("UI Text")]
        [SerializeField] private TextMeshProUGUI moneyReviveCostText;

        public event Action OnGiveUp;
        public event Action OnMoneyRevive;
        public event Action OnAdsRevive;

        public void Initialize()
        {
            ValidateReferences();
            
            if (giveUpButton != null)
                giveUpButton.onClick.AddListener(HandleGiveUp);
            
            if (moneyReviveButton != null)
                moneyReviveButton.onClick.AddListener(HandleMoneyRevive);
            
            if (adsReviveButton != null)
                adsReviveButton.onClick.AddListener(HandleAdsRevive);

            UpdateReviveCostDisplay();
            HidePanel();
            
            Debug.Log("[BombPanelController] Initialized");
        }

        private void OnDestroy()
        {
            if (giveUpButton != null)
                giveUpButton.onClick.RemoveListener(HandleGiveUp);
            
            if (moneyReviveButton != null)
                moneyReviveButton.onClick.RemoveListener(HandleMoneyRevive);
            
            if (adsReviveButton != null)
                adsReviveButton.onClick.RemoveListener(HandleAdsRevive);
        }

        private void ValidateReferences()
        {
            if (panel == null)
                Debug.LogError("[BombPanelController] Panel is not assigned!");
            
            if (giveUpButton == null)
                Debug.LogError("[BombPanelController] GiveUpButton is not assigned!");
            
            if (moneyReviveButton == null)
                Debug.LogError("[BombPanelController] MoneyReviveButton is not assigned!");
            
            if (adsReviveButton == null)
                Debug.LogError("[BombPanelController] AdsReviveButton is not assigned!");

            if (reviveConfig == null)
                Debug.LogWarning("[BombPanelController] ReviveConfig is not assigned! Using default cost.");
        }

        private void UpdateReviveCostDisplay()
        {
            if (moneyReviveCostText != null)
            {
                int currentCost = BombHandler.Instance != null 
                    ? BombHandler.Instance.GetCurrentReviveCost() 
                    : (reviveConfig != null ? reviveConfig.reviveCost : 1000);
                    
                moneyReviveCostText.text = $"${currentCost}";
            }
        }

        public void ShowPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true);
                UpdateReviveCostDisplay();
                Debug.Log("[BombPanelController] Panel shown");
            }
        }

        public void HidePanel()
        {
            if (panel != null)
            {
                panel.SetActive(false);
                Debug.Log("[BombPanelController] Panel hidden");
            }
        }

        private void HandleGiveUp()
        {
            Debug.Log("[BombPanelController] Give Up clicked");
            OnGiveUp?.Invoke();
            HidePanel();
        }

        private void HandleMoneyRevive()
        {
            Debug.Log("[BombPanelController] Money Revive clicked");
            OnMoneyRevive?.Invoke();
        }

        private void HandleAdsRevive()
        {
            Debug.Log("[BombPanelController] Ads Revive clicked");
            OnAdsRevive?.Invoke();
            HidePanel();
        }

        public int GetReviveCost()
        {
            return BombHandler.Instance != null 
                ? BombHandler.Instance.GetCurrentReviveCost() 
                : (reviveConfig != null ? reviveConfig.reviveCost : 1000);
        }
    }
}
