using UnityEngine;

namespace RewardSystem
{
    public class ConsumableReward : Reward
    {
        public int Amount { get; private set; }

        public ConsumableReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[ConsumableReward] Claimed {Amount} consumable(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Consumable{(Amount > 1 ? "s" : "")}";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }

}