using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZoneSystem;

namespace WheelSystem
{
    public class WheelManager : MonoBehaviour
    {
        public static WheelManager Instance { get; private set; }

        [Header("Wheel Configurations")]
        [SerializeField] private List<WheelData> bronzeWheels;
        [SerializeField] private List<WheelData> silverWheels;
        [SerializeField] private List<WheelData> goldenWheels;

        public WheelData CurrentWheel { get; private set; }

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

        public void LoadWheelForZone(int zone)
        {
            WheelType type = ZoneManager.Instance.GetCurrentWheelType();

            CurrentWheel = type switch
            {
                WheelType.Golden => GetGoldenWheel(zone),
                WheelType.Silver => GetSilverWheel(zone),
                _ => GetBronzeWheel(zone)
            };

            Debug.Log($"[WheelManager] Loaded {type} wheel for zone {zone}");
        }

        private WheelData GetBronzeWheel(int zone)
        {
            if (bronzeWheels == null || bronzeWheels.Count == 0)
            {
                Debug.LogError("[WheelManager] No bronze wheels configured!");
                return null;
            }

            int index = Mathf.Clamp(zone - 1, 0, bronzeWheels.Count - 1);
            Debug.Log($"[WheelManager] GetBronzeWheel: zone={zone}, index={index}, wheel={bronzeWheels[index].name}");
            return bronzeWheels[index];
        }

        private WheelData GetSilverWheel(int zone)
        {
            if (silverWheels == null || silverWheels.Count == 0)
            {
                Debug.LogError("[WheelManager] No silver wheels configured!");
                return null;
            }

            int index = Mathf.Min((zone / 5) - 1, silverWheels.Count - 1);
            return silverWheels[index];
        }

        private WheelData GetGoldenWheel(int zone)
        {
            if (goldenWheels == null || goldenWheels.Count == 0)
            {
                Debug.LogError("[WheelManager] No golden wheels configured!");
                return null;
            }

            int index = Mathf.Min((zone / 30) - 1, goldenWheels.Count - 1);
            return goldenWheels[index];
        }

        public WheelSlice GetRandomSlice()
        {
            if (CurrentWheel == null || CurrentWheel.slices.Count == 0)
            {
                Debug.LogError("[WheelManager] No slices available!");
                return null;
            }

            int randomIndex = Random.Range(0, CurrentWheel.slices.Count);
            return CurrentWheel.slices[randomIndex];
        }

        public WheelSlice GetWeightedRandomSlice()
        {
            if (CurrentWheel == null || CurrentWheel.slices.Count == 0)
            {
                Debug.LogError("[WheelManager] No slices available!");
                return null;
            }

            float totalWeight = CurrentWheel.slices.Sum(s => s.weight);
            float randomValue = Random.Range(0f, totalWeight);

            float currentWeight = 0f;
            foreach (var slice in CurrentWheel.slices)
            {
                currentWeight += slice.weight;
                if (randomValue <= currentWeight)
                    return slice;
            }

            return CurrentWheel.slices[0];
        }
    }
}
