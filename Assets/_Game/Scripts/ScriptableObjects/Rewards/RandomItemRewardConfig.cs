using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "RandomItemReward", menuName = "Rewards/Random Item Reward")]
    public class RandomItemRewardConfig : RewardConfig
    {
        [Header("Item Pool Settings")]
        [SerializeField] private List<ItemData> possibleItems = new List<ItemData>();

        public override Reward CreateReward()
        {
            return new RandomItemReward(rewardName, icon, description, new List<ItemData>(possibleItems));
        }

        private void OnValidate()
        {
            if (possibleItems != null)
            {
                possibleItems.RemoveAll(item => item == null);
            }
        }

        public bool HasValidItems()
        {
            return possibleItems != null && possibleItems.Count > 0;
        }

        public void AddItemToPool(ItemData item)
        {
            if (item != null && !possibleItems.Contains(item))
            {
                possibleItems.Add(item);
            }
        }

        public void RemoveItemFromPool(ItemData item)
        {
            if (possibleItems.Contains(item))
            {
                possibleItems.Remove(item);
            }
        }
    }
}
