using UnityEngine;

namespace RewardSystem
{

    public class ChestReward : Reward
    {
        public int Amount { get; private set; }

        public ChestReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[ChestReward] Claimed {Amount} chest(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Chest{(Amount > 1 ? "s" : "")}";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }

}