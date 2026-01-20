using System;
using System.Collections.Generic;
using UnityEngine;
using RewardSystem;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SAVE_KEY_PREFIX = "PlayerInventory_";
        
        private Dictionary<string, int> inventory = new Dictionary<string, int>();

        public event Action<string, int> OnInventoryUpdated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadInventory();
        }

        public void SaveRewards(List<Reward> rewards)
        {
            if (rewards == null || rewards.Count == 0)
            {
                Debug.Log("[SaveManager] No rewards to save");
                return;
            }

            Debug.Log($"[SaveManager] Saving {rewards.Count} rewards to inventory...");

            foreach (var reward in rewards)
            {
                AddToInventory(reward);
            }

            SaveInventory();
            Debug.Log("[SaveManager] All rewards saved successfully!");
        }

        private void AddToInventory(Reward reward)
        {
            string key = GetInventoryKey(reward);
            int amount = GetRewardAmount(reward);

            if (inventory.ContainsKey(key))
            {
                inventory[key] += amount;
            }
            else
            {
                inventory[key] = amount;
            }

            Debug.Log($"[SaveManager] Added to inventory: {key} x{amount} (Total: {inventory[key]})");
            OnInventoryUpdated?.Invoke(key, inventory[key]);
        }

        private string GetInventoryKey(Reward reward)
        {
            return reward switch
            {
                CashReward _ => "Cash",
                PointReward r => GetPointType(r),
                GoldReward _ => "Gold",
                ArmorReward _ => "Armor",
                WeaponReward _ => "Weapon",
                ChestReward _ => "Chest",
                ConsumableReward _ => "Consumable",
                _ => "Unknown"
            };
        }

        private string GetPointType(PointReward reward)
        {
            if (reward.Icon != null)
            {
                string iconName = reward.Icon.name.ToLower();
                Debug.Log($"[SaveManager] Checking point type for icon: {reward.Icon.name}");
                
                if (iconName.Contains("pistol"))
                {
                    Debug.Log("[SaveManager] Detected: Pistol Points");
                    return "Pistol Points";
                }
                else if (iconName.Contains("rifle"))
                {
                    Debug.Log("[SaveManager] Detected: Rifle Points");
                    return "Rifle Points";
                }
                else if (iconName.Contains("shotgun"))
                {
                    Debug.Log("[SaveManager] Detected: Shotgun Points");
                    return "Shotgun Points";
                }
            }
            else
            {
                Debug.LogWarning("[SaveManager] PointReward has no icon!");
            }
            
            Debug.Log("[SaveManager] Defaulting to generic Points");
            return "Points";
        }

        private int GetRewardAmount(Reward reward)
        {
            return reward switch
            {
                CashReward r => r.Amount,
                PointReward r => r.Amount,
                GoldReward r => r.Amount,
                ArmorReward r => r.Amount,
                WeaponReward r => r.Amount,
                ChestReward r => r.Amount,
                ConsumableReward r => r.Amount,
                _ => 1
            };
        }

        private void SaveInventory()
        {
            foreach (var kvp in inventory)
            {
                PlayerPrefs.SetInt(SAVE_KEY_PREFIX + kvp.Key, kvp.Value);
            }
            
            PlayerPrefs.Save();
            Debug.Log("[SaveManager] Inventory saved to PlayerPrefs");
        }

        private void LoadInventory()
        {
            inventory.Clear();

            string[] inventoryTypes = new string[]
            {
                "Cash", "Pistol Points", "Rifle Points", "Shotgun Points", "Points",
                "Gold", "Armor", "Weapon", "Chest", "Consumable"
            };

            foreach (string type in inventoryTypes)
            {
                int amount = PlayerPrefs.GetInt(SAVE_KEY_PREFIX + type, 0);
                if (amount > 0)
                {
                    inventory[type] = amount;
                    Debug.Log($"[SaveManager] Loaded from save: {type} x{amount}");
                }
            }

            Debug.Log($"[SaveManager] Inventory loaded: {inventory.Count} item types");
        }

        public Dictionary<string, int> GetInventory()
        {
            return new Dictionary<string, int>(inventory);
        }

        public int GetInventoryAmount(string itemType)
        {
            return inventory.ContainsKey(itemType) ? inventory[itemType] : 0;
        }

        [ContextMenu("Clear All Inventory Data")]
        public void ClearInventory()
        {
            inventory.Clear();
            
            string[] inventoryTypes = new string[]
            {
                "Cash", "Pistol Points", "Rifle Points", "Shotgun Points", "Points",
                "Gold", "Armor", "Weapon", "Chest", "Consumable"
            };

            foreach (string type in inventoryTypes)
            {
                PlayerPrefs.DeleteKey(SAVE_KEY_PREFIX + type);
            }
            
            PlayerPrefs.Save();
            Debug.Log("[SaveManager] All inventory data cleared!");
        }

        [ContextMenu("Print Current Inventory")]
        public void PrintInventory()
        {
            Debug.Log("=== CURRENT INVENTORY ===");
            
            if (inventory.Count == 0)
            {
                Debug.Log("Inventory is empty");
            }
            else
            {
                foreach (var kvp in inventory)
                {
                    Debug.Log($"{kvp.Key}: {kvp.Value}");
                }
            }
            
            Debug.Log("========================");
        }
    }
}
