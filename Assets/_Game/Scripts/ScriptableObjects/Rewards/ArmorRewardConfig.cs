using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "ArmorReward", menuName = "Rewards/Armor Reward")]
    public class ArmorRewardConfig : RewardConfig
    {
        [Header("Armor Amount Settings")]
        [SerializeField] private int amount = 1;

        public override Reward CreateReward()
        {
            return new ArmorReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    } 
}
