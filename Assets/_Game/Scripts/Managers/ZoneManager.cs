using System;
using UnityEngine;

namespace ZoneSystem
{
    public class ZoneManager : MonoBehaviour
    {
        public static ZoneManager Instance { get; private set; }

        public int CurrentZone { get; private set; } = 1;

        public event Action<int> OnZoneChanged;
        public event Action OnSafeZoneReached;
        public event Action OnSuperZoneReached;

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

        public bool IsSafeZone()
        {
            return CurrentZone % 5 == 0 && CurrentZone % 30 != 0;
        }

        public bool IsSuperZone()
        {
            return CurrentZone % 30 == 0;
        }

        public void NextZone()
        {
            CurrentZone++;
            OnZoneChanged?.Invoke(CurrentZone);

            if (IsSuperZone())
            {
                OnSuperZoneReached?.Invoke();
                Debug.Log($"[ZoneManager] SUPER ZONE reached! Zone {CurrentZone}");
            }
            else if (IsSafeZone())
            {
                OnSafeZoneReached?.Invoke();
                Debug.Log($"[ZoneManager] Safe zone reached! Zone {CurrentZone}");
            }
            else
            {
                Debug.Log($"[ZoneManager] Bronze zone: {CurrentZone}");
            }
        }

        public void ResetZone()
        {
            CurrentZone = 1;
            OnZoneChanged?.Invoke(CurrentZone);
            Debug.Log("[ZoneManager] Zone reset to 1");
        }

        public WheelType GetCurrentWheelType()
        {
            if (IsSuperZone()) return WheelType.Golden;
            if (IsSafeZone()) return WheelType.Silver;
            return WheelType.Bronze;
        }
    }

    public enum WheelType
    {
        Bronze,
        Silver,
        Golden
    }
}
