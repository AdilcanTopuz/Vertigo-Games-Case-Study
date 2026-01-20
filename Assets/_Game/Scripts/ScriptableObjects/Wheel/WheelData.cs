using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZoneSystem;

namespace WheelSystem
{
    [CreateAssetMenu(fileName = "WheelData", menuName = "Game/Wheel Data")]
    public class WheelData : ScriptableObject
    {
        [Header("Wheel Info")]
        public WheelType wheelType;
        public string wheelName;
        public Sprite wheelSprite;

        [Header("Slices")]
        public List<WheelSlice> slices = new List<WheelSlice>();

        private void OnValidate()
        {
            if (wheelType == WheelType.Bronze)
            {
                bool hasBomb = slices.Any(s => s.IsBomb);
                if (!hasBomb)
                {
                    Debug.LogWarning($"[{name}] Bronze wheel must have at least one bomb!");
                }
            }

            if (wheelType != WheelType.Bronze)
            {
                bool hasBomb = slices.Any(s => s.IsBomb);
                if (hasBomb)
                {
                    Debug.LogWarning($"[{name}] Safe/Super wheels should not have bombs!");
                }
            }
        }
    }
}
