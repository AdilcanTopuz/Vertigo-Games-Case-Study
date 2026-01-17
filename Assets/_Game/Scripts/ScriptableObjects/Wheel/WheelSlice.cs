using System;
using UnityEngine;
using RewardSystem;

namespace WheelSystem
{
    [Serializable]
    public class WheelSlice
    {
        public string sliceName;
        public Sprite sliceIcon;

        [Header("Reward or Bomb")]
        public bool isBomb;
        public RewardConfig rewardConfig;

        [Header("Visual")]
        public Color sliceColor = Color.white;

        [Range(0f, 1f)]
        public float weight = 1f;
    }
}
