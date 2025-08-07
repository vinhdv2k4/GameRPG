using UnityEngine;
namespace TV
{
    public class Item : ScriptableObject
    {
        [Header("Iitem Information")]
        public string itemName;
        public Sprite itemIcon;

        [TextArea ] public string itemDescription;
        public int itemID;

    }
}