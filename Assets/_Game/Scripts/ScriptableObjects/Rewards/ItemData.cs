using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Item Information")]
        public string ItemName;
        public Sprite ItemIcon;

        [TextArea(2, 4)]
        public string ItemDescription;

        public int ItemID;
    }

}