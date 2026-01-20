using UnityEngine;

namespace ConfigSystem
{
    [CreateAssetMenu(fileName = "ReviveConfig", menuName = "Game/Revive Config")]
    public class ReviveConfig : ScriptableObject
    {
        [Header("Revive Settings")]
        [Tooltip("Base cost in cash to revive after hitting a bomb")]
        public int reviveCost = 1000;

        [Tooltip("Multiplier for each subsequent revive (e.g., 2.0 means cost doubles each time)")]
        [Range(1.0f, 5.0f)]
        public float reviveCostMultiplier = 2.0f;

        [Header("Ads Settings")]
        [Tooltip("Simulated ad watch duration in seconds")]
        public float adWatchDuration = 3f;
    }
}
