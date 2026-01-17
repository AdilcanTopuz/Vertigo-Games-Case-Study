using UnityEngine;

namespace RewardSystem
{

    [CreateAssetMenu(fileName = "ChestReward", menuName = "Rewards/Chest Reward")]
    public class ChestRewardConfig : RewardConfig
    {
        [Header("Chest Amount Settings")]
        [SerializeField] private int amount = 1;

        public override Reward CreateReward()
        {
            return new ChestReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }

}