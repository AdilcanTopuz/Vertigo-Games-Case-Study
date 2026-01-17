using UnityEngine;

namespace RewardSystem
{

    public class PointReward : Reward
    {
        public int Amount { get; private set; }

        public PointReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {
            Debug.Log($"[PointReward] Claimed {Amount} point(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Point{(Amount > 1 ? "s" : "")}";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }

}