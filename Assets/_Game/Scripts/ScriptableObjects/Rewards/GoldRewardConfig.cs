using UnityEngine;

namespace RewardSystem
{

    [CreateAssetMenu(fileName = "GoldReward", menuName = "Rewards/Gold Reward")]
    public class GoldRewardConfig : RewardConfig
    {
        [Header("Gold Amount Settings")]
        [SerializeField] private int amount = 100;

        public override Reward CreateReward()
        {
            return new GoldReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }

}