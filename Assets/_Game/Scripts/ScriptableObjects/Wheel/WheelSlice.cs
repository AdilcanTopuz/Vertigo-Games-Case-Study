using System;
using UnityEngine;
using RewardSystem;

namespace WheelSystem
{
    [Serializable]
    public class WheelSlice
    {
        [Header("Reward Configuration")]
        public RewardConfig rewardConfig;

        [Header("Wheel Slice Settings")]
        [Range(0f, 1f)]
        public float weight = 1f;

        public string SliceName => rewardConfig != null ? rewardConfig.RewardName : "Empty";
        public Sprite SliceIcon => rewardConfig != null ? rewardConfig.Icon : null;
        public bool IsBomb => rewardConfig != null && rewardConfig.IsBomb;
    }
}
