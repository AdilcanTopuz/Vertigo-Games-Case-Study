using System;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

namespace UI
{
    public class ExitPanelController : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panel;

        [Header("Buttons")]
        [SerializeField] private Button collectAndExitButton;
        [SerializeField] private Button backToSpinButton;

        public event Action OnCollectAndExit;
        public event Action OnBackToSpin;

        public void Initialize()
        {
            ValidateReferences();
            
            if (collectAndExitButton != null)
                collectAndExitButton.onClick.AddListener(HandleCollectAndExit);
            
            if (backToSpinButton != null)
                backToSpinButton.onClick.AddListener(HandleBackToSpin);

            HidePanel();
            
            Debug.Log("[ExitPanelController] Initialized");
        }

        private void OnDestroy()
        {
            if (collectAndExitButton != null)
                collectAndExitButton.onClick.RemoveListener(HandleCollectAndExit);
            
            if (backToSpinButton != null)
                backToSpinButton.onClick.RemoveListener(HandleBackToSpin);
        }

        private void ValidateReferences()
        {
            if (panel == null)
                Debug.LogError("[ExitPanelController] Panel is not assigned!");
            
            if (collectAndExitButton == null)
                Debug.LogError("[ExitPanelController] CollectAndExitButton is not assigned!");
            
            if (backToSpinButton == null)
                Debug.LogError("[ExitPanelController] BackToSpinButton is not assigned!");
        }

        public void ShowPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true);
                Debug.Log("[ExitPanelController] Panel shown");
            }
        }

        public void HidePanel()
        {
            if (panel != null)
            {
                panel.SetActive(false);
                Debug.Log("[ExitPanelController] Panel hidden");
            }
        }

        private void HandleCollectAndExit()
        {
            Debug.Log("[ExitPanelController] Collect and Exit clicked");
            OnCollectAndExit?.Invoke();
            HidePanel();
        }

        private void HandleBackToSpin()
        {
            Debug.Log("[ExitPanelController] Back to Spin clicked");
            OnBackToSpin?.Invoke();
            HidePanel();
        }
    }
}
