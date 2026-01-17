using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "BombReward", menuName = "Rewards/Bomb Reward")]
    public class BombRewardConfig : RewardConfig
    {
        [Header("Bomb Amount Settings")]
        [SerializeField] private int amount = 1;

        public override Reward CreateReward()
        {
            return new BombReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    } 
}
