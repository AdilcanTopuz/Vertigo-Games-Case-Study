using UnityEngine;

namespace RewardSystem
{

    public class GoldReward : Reward
    {
        public int Amount { get; private set; }

        public GoldReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[GoldReward] Claimed {Amount} gold!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Gold";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }

}