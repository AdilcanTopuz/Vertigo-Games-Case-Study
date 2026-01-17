using UnityEngine;

namespace RewardSystem
{

    [CreateAssetMenu(fileName = "CashReward", menuName = "Rewards/Cash Reward")]
    public class CashRewardConfig : RewardConfig
    {
        [Header("Cash Amount Settings")]
        [SerializeField] private int amount = 50;

        public override Reward CreateReward()
        {
            return new CashReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }

}