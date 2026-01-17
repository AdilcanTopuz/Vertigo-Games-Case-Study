using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{

    public class RandomItemReward : Reward
    {
        public ItemData SelectedItem { get; private set; }
        private List<ItemData> itemPool;

        public RandomItemReward(string name, Sprite icon, string desc, List<ItemData> pool)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            itemPool = pool;
            SelectedItem = SelectRandomItem();

            if (SelectedItem != null && SelectedItem.ItemIcon != null)
            {
                Icon = SelectedItem.ItemIcon;
            }
        }

        private ItemData SelectRandomItem()
        {
            if (itemPool == null || itemPool.Count == 0)
            {
                Debug.LogWarning("[RandomItemReward] Item pool is empty!");
                return null;
            }

            itemPool.RemoveAll(item => item == null);

            if (itemPool.Count == 0)
            {
                Debug.LogWarning("[RandomItemReward] All items in pool are null!");
                return null;
            }

            return itemPool[Random.Range(0, itemPool.Count)];
        }

        public override void Claim()
        {
            if (SelectedItem == null)
            {
                Debug.LogWarning("[RandomItemReward] Cannot claim - no item selected!");
                return;
            }


            Debug.Log($"[RandomItemReward] Claimed item: {SelectedItem.ItemName}");

        }

        public override string GetDisplayText()
        {
            if (SelectedItem != null)
            {
                return $"Item: {SelectedItem.ItemName}";
            }

            return "Random Item";
        }

        public override bool Validate()
        {
            return SelectedItem != null;
        }
    }

}