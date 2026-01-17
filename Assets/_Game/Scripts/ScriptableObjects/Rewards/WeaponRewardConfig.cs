using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "WeaponReward", menuName = "Rewards/Weapon Reward")]
    public class WeaponRewardConfig : RewardConfig
    {
        [Header("Weapon Amount Settings")]
        [SerializeField] private int amount = 1;

        public override Reward CreateReward()
        {
            return new WeaponReward(rewardName, icon, description, amount);
        }

        private void OnValidate()
        {
            if (amount < 0) amount = 0;
        }
    }
}
