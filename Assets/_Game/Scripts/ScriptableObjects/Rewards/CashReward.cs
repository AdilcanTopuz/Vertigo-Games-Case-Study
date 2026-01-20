using UnityEngine;


namespace RewardSystem
{
    public class CashReward : Reward
    {
        public int Amount { get; private set; }

        public CashReward(string name, Sprite icon, string desc, int amount)
        {
            RewardName = name;
            Icon = icon;
            Description = desc;
            Amount = amount;
        }

        public override void Claim()
        {

            Debug.Log($"[CashReward] Claimed {Amount} cash!");
        }

        public override string GetDisplayText()
        {
            return $"{Amount} Cash";
        }

        public override bool Validate()
        {
            return Amount > 0;
        }
    } 
}
