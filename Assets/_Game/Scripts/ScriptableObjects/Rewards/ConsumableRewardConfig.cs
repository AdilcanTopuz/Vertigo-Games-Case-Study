using UnityEngine;

namespace RewardSystem
{

    [CreateAssetMenu(fileName = "ConsumableReward", menuName = "Rewards/Consumable Reward")]
    public class ConsumableRewardConfig : RewardConfig
    {
        [Header("Consumable Amount Settings")]
        [SerializeField] private int amount = 1;

        public override Reward CreateReward()
        {
            return new ConsumableReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }

}