using UnityEngine;
using TMPro;
using SaveSystem;
using System.Collections.Generic;

namespace UI
{
    public class InventoryDisplayController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private GameObject inventoryPanel;

        [Header("Settings")]
        [SerializeField] private bool showOnStart = true;

        private void Start()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.OnInventoryUpdated += OnInventoryChanged;
            }

            if (showOnStart)
            {
                UpdateDisplay();
            }
        }

        private void OnDestroy()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.OnInventoryUpdated -= OnInventoryChanged;
            }
        }

        private void OnInventoryChanged(string itemType, int amount)
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (SaveManager.Instance == null)
            {
                if (inventoryText != null)
                    inventoryText.text = "SaveManager not found!";
                return;
            }

            var inventory = SaveManager.Instance.GetInventory();

            if (inventory.Count == 0)
            {
                if (inventoryText != null)
                    inventoryText.text = "No saved items";
                return;
            }

            string displayText = "=== SAVED INVENTORY ===\n\n";

            foreach (var kvp in inventory)
            {
                displayText += $"{kvp.Key}: {kvp.Value}\n";
            }

            if (inventoryText != null)
                inventoryText.text = displayText;
        }

        [ContextMenu("Show Inventory")]
        public void ShowInventory()
        {
            if (inventoryPanel != null)
                inventoryPanel.SetActive(true);

            UpdateDisplay();
        }

        [ContextMenu("Hide Inventory")]
        public void HideInventory()
        {
            if (inventoryPanel != null)
                inventoryPanel.SetActive(false);
        }

        public void ToggleInventory()
        {
            if (inventoryPanel != null)
            {
                bool isActive = inventoryPanel.activeSelf;
                inventoryPanel.SetActive(!isActive);

                if (!isActive)
                    UpdateDisplay();
            }
        }
    }
}
