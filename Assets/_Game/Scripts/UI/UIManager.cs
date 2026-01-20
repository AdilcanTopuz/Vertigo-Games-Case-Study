using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameSystem;
using SpinSystem;
using ZoneSystem;
using WheelSystem;
using BombSystem;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Panels")]
        [SerializeField] private GameObject topRightPanel;
        [SerializeField] private GameObject leftPanel;
        [SerializeField] private GameObject centerPanel;
        [SerializeField] private GameObject rightPanel;
        [SerializeField] private GameObject exitPanel;
        [SerializeField] private GameObject bombPanel;

        [Header("UI Controllers")]
        [SerializeField] private WheelViewController wheelViewController;
        [SerializeField] private RewardDisplayController rewardDisplayController;
        [SerializeField] private CashDisplayController cashDisplayController;
        [SerializeField] private ZoneButtonController zoneButtonController;
        [SerializeField] private ExitPanelController exitPanelController;
        [SerializeField] private BombPanelController bombPanelController;

        [Header("Buttons")]
        [SerializeField] private Button spinButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            ValidateReferences();
        }

        private void Start()
        {
            SubscribeToEvents();
            InitializeUI();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void ValidateReferences()
        {
            if (wheelViewController == null)
                Debug.LogError("[UIManager] WheelViewController is not assigned!");
            
            if (rewardDisplayController == null)
                Debug.LogError("[UIManager] RewardDisplayController is not assigned!");
            
            if (cashDisplayController == null)
                Debug.LogError("[UIManager] CashDisplayController is not assigned!");
            
            if (zoneButtonController == null)
                Debug.LogError("[UIManager] ZoneButtonController is not assigned!");
            
            if (exitPanelController == null)
                Debug.LogError("[UIManager] ExitPanelController is not assigned!");
            
            if (bombPanelController == null)
                Debug.LogError("[UIManager] BombPanelController is not assigned!");
            
            if (spinButton == null)
                Debug.LogError("[UIManager] SpinButton is not assigned!");
            
            if (exitButton == null)
                Debug.LogError("[UIManager] ExitButton is not assigned!");
        }

        private void SubscribeToEvents()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
                GameStateManager.Instance.OnRewardAdded += OnRewardAdded;
                GameStateManager.Instance.OnRewardsCleared += OnRewardsCleared;
            }

            if (ZoneManager.Instance != null)
            {
                ZoneManager.Instance.OnZoneChanged += OnZoneChanged;
            }

            if (SpinController.Instance != null)
            {
                SpinController.Instance.OnSpinStarted += OnSpinStarted;
                SpinController.Instance.OnSpinCompleted += OnSpinCompleted;
                SpinController.Instance.OnSpinResultProcessed += OnSpinResultProcessed;
            }

            if (BombHandler.Instance != null)
            {
                BombHandler.Instance.OnBombHit += OnBombHit;
            }

            if (exitPanelController != null)
            {
                exitPanelController.OnCollectAndExit += OnCollectAndExit;
                exitPanelController.OnBackToSpin += OnBackToSpin;
            }

            if (bombPanelController != null)
            {
                bombPanelController.OnGiveUp += OnBombGiveUp;
                bombPanelController.OnMoneyRevive += OnBombMoneyRevive;
                bombPanelController.OnAdsRevive += OnBombAdsRevive;
            }

            if (spinButton != null)
                spinButton.onClick.AddListener(OnSpinButtonClicked);
            
            if (exitButton != null)
                exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void UnsubscribeFromEvents()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
                GameStateManager.Instance.OnRewardAdded -= OnRewardAdded;
                GameStateManager.Instance.OnRewardsCleared -= OnRewardsCleared;
            }

            if (ZoneManager.Instance != null)
            {
                ZoneManager.Instance.OnZoneChanged -= OnZoneChanged;
            }

            if (SpinController.Instance != null)
            {
                SpinController.Instance.OnSpinStarted -= OnSpinStarted;
                SpinController.Instance.OnSpinCompleted -= OnSpinCompleted;
                SpinController.Instance.OnSpinResultProcessed -= OnSpinResultProcessed;
            }

            if (BombHandler.Instance != null)
            {
                BombHandler.Instance.OnBombHit -= OnBombHit;
            }

            if (exitPanelController != null)
            {
                exitPanelController.OnCollectAndExit -= OnCollectAndExit;
                exitPanelController.OnBackToSpin -= OnBackToSpin;
            }

            if (bombPanelController != null)
            {
                bombPanelController.OnGiveUp -= OnBombGiveUp;
                bombPanelController.OnMoneyRevive -= OnBombMoneyRevive;
                bombPanelController.OnAdsRevive -= OnBombAdsRevive;
            }

            if (spinButton != null)
                spinButton.onClick.RemoveListener(OnSpinButtonClicked);
            
            if (exitButton != null)
                exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void InitializeUI()
        {
            if (wheelViewController != null)
                wheelViewController.Initialize();
            
            if (rewardDisplayController != null)
                rewardDisplayController.Initialize();
            
            if (cashDisplayController != null)
                cashDisplayController.Initialize();
            
            if (zoneButtonController != null)
                zoneButtonController.Initialize();
            
            if (exitPanelController != null)
                exitPanelController.Initialize();
            
            if (bombPanelController != null)
                bombPanelController.Initialize();

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.StartGame();
            }

            UpdateSpinButtonState();
            UpdateExitButtonState();

            Debug.Log("[UIManager] UI initialized");
        }

        #region Event Handlers

        private void OnGameStateChanged(GameState newState)
        {
            Debug.Log($"[UIManager] Game state changed to: {newState}");

            switch (newState)
            {
                case GameState.Playing:
                    ShowGameUI();
                    break;
                case GameState.GameOver:
                    ShowGameOverUI();
                    break;
                case GameState.Victory:
                    ShowVictoryUI();
                    break;
            }

            UpdateSpinButtonState();
            UpdateExitButtonState();
        }

        private void OnRewardAdded(RewardSystem.Reward reward)
        {
            if (rewardDisplayController != null)
                rewardDisplayController.AddReward(reward);
            
        }

        private void OnRewardsCleared()
        {
            if (rewardDisplayController != null)
                rewardDisplayController.ClearRewards();
            
            if (cashDisplayController != null)
                cashDisplayController.ResetCash();
        }

        private void OnZoneChanged(int newZone)
        {
            if (zoneButtonController != null)
                zoneButtonController.UpdateZone(newZone);
            
            if (wheelViewController != null)
                wheelViewController.UpdateWheelForZone(newZone);

            UpdateExitButtonState();
        }

        private void OnSpinStarted()
        {
            UpdateSpinButtonState();
            
            if (wheelViewController != null)
                wheelViewController.StartSpin();
        }

        private void OnSpinCompleted(WheelSlice result)
        {
            if (wheelViewController != null)
                wheelViewController.CompleteSpin(result);
            
        }

        private void OnSpinResultProcessed()
        {
            UpdateSpinButtonState();
        }

        private void OnBombHit()
        {
            if (wheelViewController != null)
                wheelViewController.PlayBombExplosion();
            
            if (bombPanelController != null)
                bombPanelController.ShowPanel();
        }

        #endregion

        #region Button Handlers

        private void OnSpinButtonClicked()
        {
            if (SpinController.Instance != null && SpinController.Instance.CanSpin())
            {
                SpinController.Instance.Spin();
            }
            else
            {
                Debug.LogWarning("[UIManager] Cannot spin right now!");
            }
        }

        private void OnExitButtonClicked()
        {
            if (exitPanelController != null)
            {
                exitPanelController.ShowPanel();
            }
            else
            {
                Debug.LogWarning("[UIManager] ExitPanelController is null, leaving immediately");
                if (GameStateManager.Instance != null && GameStateManager.Instance.CanLeave())
                {
                    GameStateManager.Instance.LeaveGame();
                }
            }
        }

        #endregion

        #region Panel Event Handlers

        private void OnCollectAndExit()
        {
            Debug.Log("[UIManager] Collect and Exit");
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.LeaveGame();
            }
        }

        private void OnBackToSpin()
        {
            Debug.Log("[UIManager] Back to Spin");
        }

        private void OnBombGiveUp()
        {
            Debug.Log("[UIManager] Bomb - Give Up");
            if (BombHandler.Instance != null)
            {
                BombHandler.Instance.GiveUp();
            }
        }

        private void OnBombMoneyRevive()
        {
            Debug.Log("[UIManager] Bomb - Money Revive");
            if (BombHandler.Instance != null && bombPanelController != null)
            {
                int cost = bombPanelController.GetReviveCost();
                bool success = BombHandler.Instance.MoneyRevive(cost);
                
                if (success)
                {
                    bombPanelController.HidePanel();
                }
                else
                {
                    Debug.LogWarning("[UIManager] Money revive failed - panel stays open");
                }
            }
        }

        private void OnBombAdsRevive()
        {
            Debug.Log("[UIManager] Bomb - Ads Revive");
            if (BombHandler.Instance != null)
            {
                BombHandler.Instance.AdsRevive();
            }
        }

        #endregion

        #region UI State Updates

        private void UpdateSpinButtonState()
        {
            if (spinButton == null) return;

            bool canSpin = SpinController.Instance != null && SpinController.Instance.CanSpin();
            spinButton.interactable = canSpin;
        }

        private void UpdateExitButtonState()
        {
            if (exitButton == null) return;

            bool canLeave = GameStateManager.Instance != null && GameStateManager.Instance.CanLeave();
            exitButton.interactable = canLeave;
        }

        private void ShowGameUI()
        {
            if (topRightPanel != null) topRightPanel.SetActive(true);
            if (leftPanel != null) leftPanel.SetActive(true);
            if (centerPanel != null) centerPanel.SetActive(true);
            if (rightPanel != null) rightPanel.SetActive(true);
        }

        private void ShowGameOverUI()
        {
            Debug.Log("[UIManager] Game Over!");
        }

        private void ShowVictoryUI()
        {
            Debug.Log("[UIManager] Victory!");
        }

        #endregion

        #region Public Methods

        public void ShowPanel(GameObject panel)
        {
            if (panel != null)
                panel.SetActive(true);
        }

        public void HidePanel(GameObject panel)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        #endregion
    }
}
