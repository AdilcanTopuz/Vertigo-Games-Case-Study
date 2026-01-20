using UnityEngine;

namespace RewardSystem
{
    public abstract class RewardConfig : ScriptableObject
    {
        [Header("Basic Reward Info")]
        [SerializeField] protected string rewardName;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool isBomb;
        [TextArea(2, 4)]
        [SerializeField] protected string description;

        public string RewardName => rewardName;
        public Sprite Icon => icon;
        public bool IsBomb => isBomb;

        public abstract Reward CreateReward();
    }
}
