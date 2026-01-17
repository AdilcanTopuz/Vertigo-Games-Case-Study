using System;
using System.Collections;
using UnityEngine;
using WheelSystem;
using ZoneSystem;
using GameSystem;
using RewardSystem;

namespace SpinSystem
{
    public enum SpinState
    {
        Idle,
        Spinning,
        Stopped
    }

    public class SpinController : MonoBehaviour
    {
        public static SpinController Instance { get; private set; }

        public SpinState State { get; private set; } = SpinState.Idle;

        public event Action OnSpinStarted;
        public event Action<WheelSlice> OnSpinCompleted;

        [Header("Spin Settings")]
        [SerializeField] private float spinDuration = 2f;

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

        public bool CanSpin()
        {
            return State == SpinState.Idle &&
                   GameStateManager.Instance.CurrentState == GameState.Playing;
        }

        public void Spin()
        {
            if (!CanSpin())
            {
                Debug.LogWarning("[SpinController] Cannot spin right now!");
                return;
            }

            State = SpinState.Spinning;
            OnSpinStarted?.Invoke();

            Debug.Log("[SpinController] Spin started!");

            StartCoroutine(SimulateSpin());
        }

        private IEnumerator SimulateSpin()
        {
            yield return new WaitForSeconds(spinDuration);

            WheelSlice result = WheelManager.Instance.GetRandomSlice();

            State = SpinState.Stopped;
            OnSpinCompleted?.Invoke(result);

            Debug.Log($"[SpinController] Spin result: {result.sliceName} (Bomb: {result.isBomb})");

            ProcessSpinResult(result);
        }

        private void ProcessSpinResult(WheelSlice result)
        {
            if (result.isBomb)
            {
                BombHandler.Instance.HandleBombHit();
            }
            else
            {
                Reward reward = result.rewardConfig.CreateReward();
                GameStateManager.Instance.AddReward(reward);

                ZoneManager.Instance.NextZone();
                WheelManager.Instance.LoadWheelForZone(ZoneManager.Instance.CurrentZone);
            }

            State = SpinState.Idle;
        }
    }
}
