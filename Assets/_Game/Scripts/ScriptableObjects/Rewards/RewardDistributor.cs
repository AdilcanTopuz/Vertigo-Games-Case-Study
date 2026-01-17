using System;
using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    public class RewardDistributor : MonoBehaviour
    {
        public static RewardDistributor Instance { get; private set; }

        public event Action<Reward> OnRewardClaimed;
        public event Action<List<Reward>> OnMultipleRewardsClaimed;
        public event Action<Reward, string> OnRewardFailed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void GiveReward(Reward reward)
        {
            if (reward == null)
            {
                Debug.LogWarning("[RewardDistributor] Attempted to give null reward!");
                return;
            }

            if (!reward.Validate())
            {
                string errorMsg = $"Reward validation failed: {reward.RewardName}";
                Debug.LogWarning($"[RewardDistributor] {errorMsg}");
                OnRewardFailed?.Invoke(reward, errorMsg);
                return;
            }

            reward.Claim();
            OnRewardClaimed?.Invoke(reward);

            Debug.Log($"[RewardDistributor] Successfully gave reward: {reward.GetDisplayText()}");
        }

        public void GiveRewards(List<Reward> rewards)
        {
            if (rewards == null || rewards.Count == 0)
            {
                Debug.LogWarning("[RewardDistributor] Attempted to give null or empty reward list!");
                return;
            }

            List<Reward> claimedRewards = new List<Reward>();

            foreach (var reward in rewards)
            {
                if (reward == null)
                {
                    Debug.LogWarning("[RewardDistributor] Skipping null reward in list");
                    continue;
                }

                if (!reward.Validate())
                {
                    string errorMsg = $"Reward validation failed: {reward.RewardName}";
                    Debug.LogWarning($"[RewardDistributor] {errorMsg}");
                    OnRewardFailed?.Invoke(reward, errorMsg);
                    continue;
                }

                reward.Claim();
                claimedRewards.Add(reward);
            }

            if (claimedRewards.Count > 0)
            {
                OnMultipleRewardsClaimed?.Invoke(claimedRewards);
                Debug.Log($"[RewardDistributor] Successfully gave {claimedRewards.Count} rewards");
            }
        }

        public void GiveRewardFromConfig(RewardConfig config)
        {
            if (config == null)
            {
                Debug.LogWarning("[RewardDistributor] Attempted to give reward from null config!");
                return;
            }

            Reward reward = config.CreateReward();
            GiveReward(reward);
        }

        public void GiveRewardsFromConfigs(List<RewardConfig> configs)
        {
            if (configs == null || configs.Count == 0)
            {
                Debug.LogWarning("[RewardDistributor] Attempted to give rewards from null or empty config list!");
                return;
            }

            List<Reward> rewards = new List<Reward>();

            foreach (var config in configs)
            {
                if (config != null)
                {
                    rewards.Add(config.CreateReward());
                }
            }

            GiveRewards(rewards);
        }

        public Reward PreviewReward(RewardConfig config)
        {
            if (config == null) return null;
            return config.CreateReward();
        }

        public bool ValidateReward(Reward reward)
        {
            return reward != null && reward.Validate();
        }
    }
}
