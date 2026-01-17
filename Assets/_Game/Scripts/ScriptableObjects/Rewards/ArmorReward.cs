using UnityEngine;

namespace RewardSystem
{
    public class ArmorReward : Reward
    {
        public int Amount { get; private set; }

        public ArmorReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[ArmorReward] Claimed {Amount} armor piece(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Armor";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }
}