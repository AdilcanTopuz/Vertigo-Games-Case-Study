using UnityEngine;

namespace RewardSystem
{
    public class BombReward : Reward
    {
        public int Amount { get; private set; }

        public BombReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {

            Debug.Log($"[BombReward] Claimed {Amount} bomb(s)!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Bomb{(Amount > 1 ? "s" : "")}";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    }

}
