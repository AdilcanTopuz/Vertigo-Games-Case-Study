using UnityEngine;

namespace RewardSystem
{
    public class WeaponReward : Reward
    {
        public int Amount { get; private set; }

        public WeaponReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[WeaponReward] Claimed {Amount} weapon(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Weapon{(Amount > 1 ? "s" : "")}";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }
}
