using UnityEngine;

namespace RewardSystem
{
    public abstract class Reward
    {
        public string RewardName { get; protected set; }
        public Sprite Icon { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Claim();
        public abstract string GetDisplayText();

        public virtual bool Validate()
        {
            return true;
        }
    }
}
