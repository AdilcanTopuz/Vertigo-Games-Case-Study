using UnityEngine;

namespace RewardSystem
{
    public abstract class RewardConfig : ScriptableObject
    {
        [Header("Basic Reward Info")]
        [SerializeField] protected string rewardName;
        [SerializeField] protected Sprite icon;
        [TextArea(2, 4)]
        [SerializeField] protected string description;

        public abstract Reward CreateReward();
    }
}
