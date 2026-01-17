using UnityEngine;
using GameSystem;
using SpinSystem;
using ZoneSystem;

namespace Testing
{
    public class GameTester : MonoBehaviour
    {
        [ContextMenu("1. Start Game")]
        public void TestStartGame()
        {
            Debug.Log("=== TESTING: Start Game ===");
            GameStateManager.Instance.StartGame();
        }

        [ContextMenu("2. Spin Wheel")]
        public void TestSpin()
        {
            Debug.Log("=== TESTING: Spin Wheel ===");
            SpinController.Instance.Spin();
        }

        [ContextMenu("3. Leave Game")]
        public void TestLeave()
        {
            Debug.Log("=== TESTING: Leave Game ===");
            GameStateManager.Instance.LeaveGame();
        }

        [ContextMenu("4. Jump to Zone 5 (Safe Zone)")]
        public void TestJumpToSafeZone()
        {
            Debug.Log("=== TESTING: Jump to Zone 5 ===");
            for (int i = ZoneManager.Instance.CurrentZone; i < 5; i++)
            {
                ZoneManager.Instance.NextZone();
            }
        }

        [ContextMenu("5. Jump to Zone 30 (Super Zone)")]
        public void TestJumpToSuperZone()
        {
            Debug.Log("=== TESTING: Jump to Zone 30 ===");
            for (int i = ZoneManager.Instance.CurrentZone; i < 30; i++)
            {
                ZoneManager.Instance.NextZone();
            }
        }

        [ContextMenu("6. Show Current State")]
        public void ShowCurrentState()
        {
            Debug.Log("=== CURRENT STATE ===");
            Debug.Log($"Game State: {GameStateManager.Instance.CurrentState}");
            Debug.Log($"Current Zone: {ZoneManager.Instance.CurrentZone}");
            Debug.Log($"Is Safe Zone: {ZoneManager.Instance.IsSafeZone()}");
            Debug.Log($"Is Super Zone: {ZoneManager.Instance.IsSuperZone()}");
            Debug.Log($"Wheel Type: {ZoneManager.Instance.GetCurrentWheelType()}");
            Debug.Log($"Spin State: {SpinController.Instance.State}");
            Debug.Log($"Collected Rewards: {GameStateManager.Instance.GetCollectedRewards().Count}");
        }

        [ContextMenu("7. Restart Game")]
        public void TestRestart()
        {
            Debug.Log("=== TESTING: Restart Game ===");
            GameStateManager.Instance.RestartGame();
        }
    }
}
