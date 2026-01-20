using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using RewardSystem;

namespace UI
{
    public class RewardDisplayController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform rewardContainer;
        [SerializeField] private GameObject rewardItemPrefab;

        [Header("Settings")]
        [SerializeField] private float itemSpawnDelay = 0.1f;

        private List<GameObject> rewardItems = new List<GameObject>();
        
        private Dictionary<string, RewardItemData> rewardItemsDict = new Dictionary<string, RewardItemData>();

        private class RewardItemData
        {
            public GameObject itemObject;
            public Reward reward;
            public int totalAmount;
        }

        public void Initialize()
        {
            ValidateReferences();
            ClearRewards();
            Debug.Log("[RewardDisplayController] Initialized");
        }

        private void ValidateReferences()
        {
            if (rewardContainer == null)
                Debug.LogError("[RewardDisplayController] RewardContainer is not assigned!");
            
            if (rewardItemPrefab == null)
                Debug.LogError("[RewardDisplayController] RewardItemPrefab is not assigned!");
        }

        public void AddReward(Reward reward)
        {
            if (rewardContainer == null)
            {
                Debug.LogError("[RewardDisplayController] rewardContainer is NULL!");
                return;
            }
            
            if (rewardItemPrefab == null)
            {
                Debug.LogError("[RewardDisplayController] rewardItemPrefab is NULL!");
                return;
            }
            
            if (reward == null)
            {
                Debug.LogError("[RewardDisplayController] reward is NULL!");
                return;
            }

            string rewardKey = GetRewardKey(reward);
            
            if (rewardItemsDict.ContainsKey(rewardKey))
            {
                UpdateExistingReward(rewardKey, reward);
            }
            else
            {
                CreateNewRewardItem(rewardKey, reward);
            }
        }

        private string GetRewardKey(Reward reward)
        {
            string iconName = reward.Icon != null ? reward.Icon.name : "no_icon";
            return $"{reward.GetType().Name}_{iconName}";
        }

        private void CreateNewRewardItem(string key, Reward reward)
        {
            GameObject item = Instantiate(rewardItemPrefab, rewardContainer);
            Debug.Log($"[RewardDisplayController] Instantiated new item: {item.name}");
            
            int amount = GetRewardAmount(reward);
            
            rewardItemsDict[key] = new RewardItemData
            {
                itemObject = item,
                reward = reward,
                totalAmount = amount
            };
            
            UpdateRewardItem(item, reward, amount);

            rewardItems.Add(item);

            item.transform.localScale = Vector3.zero;
            item.transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(itemSpawnDelay);

            Debug.Log($"[RewardDisplayController] Created new reward: {reward.GetDisplayText()} (Total items: {rewardItems.Count})");
        }

        private void UpdateExistingReward(string key, Reward reward)
        {
            RewardItemData itemData = rewardItemsDict[key];
            int newAmount = GetRewardAmount(reward);
            itemData.totalAmount += newAmount;
            
            UpdateRewardItem(itemData.itemObject, reward, itemData.totalAmount);
            
            itemData.itemObject.transform.DOScale(1.2f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack);
            
            Debug.Log($"[RewardDisplayController] Updated existing reward: {reward.GetDisplayText()} -> Total: {itemData.totalAmount}");
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

        private void UpdateRewardItem(GameObject item, Reward reward, int totalAmount)
        {
            Debug.Log($"[RewardDisplayController] UpdateRewardItem - Item: {item.name}, Reward: {reward.GetDisplayText()}, Amount: {totalAmount}");
            
            Image iconImage = item.transform.Find("ui_image_collected_reward_icon")?.GetComponent<Image>();
            if (iconImage != null && reward.Icon != null)
            {
                iconImage.sprite = reward.Icon;
                Debug.Log($"[RewardDisplayController] Icon updated: {reward.Icon.name}");
            }
            else
            {
                Debug.LogWarning($"[RewardDisplayController] Icon not found or null! iconImage: {iconImage != null}, reward.Icon: {reward.Icon != null}");
            }

            TextMeshProUGUI multiplierText = item.transform.Find("ui_text_collected_reward_multiplier_value")?.GetComponent<TextMeshProUGUI>();
            if (multiplierText != null)
            {
                multiplierText.text = GetRewardMultiplierText(reward, totalAmount);
                Debug.Log($"[RewardDisplayController] MultiplierText updated: {multiplierText.text}");
            }
            else
            {
                Debug.LogWarning("[RewardDisplayController] MultiplierText not found!");
            }
        }

        private string GetRewardMultiplierText(Reward reward, int totalAmount)
        {
            if (reward is CashReward)
            {
                return $"${totalAmount}";
            }
            
            return $"x{totalAmount}";
        }

        public void ClearRewards()
        {
            foreach (var item in rewardItems)
            {
                if (item != null)
                    Destroy(item);
            }

            rewardItems.Clear();
            rewardItemsDict.Clear();
            Debug.Log("[RewardDisplayController] Rewards cleared");
        }

        public int GetRewardCount()
        {
            return rewardItems.Count;
        }
    }
}
