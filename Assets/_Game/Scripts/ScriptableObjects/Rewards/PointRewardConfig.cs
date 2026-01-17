using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "PointReward", menuName = "Rewards/Point Reward")]
    public class PointRewardConfig : RewardConfig
    {
        [Header("Point Amount Settings")]
        [SerializeField] private int amount = 100;

        public override Reward CreateReward()
        {
            return new PointReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }

}