    using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TV
{

    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase instance;

        public WeaponItem unWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Items")]
        private List<Item> items = new List<Item>();
            

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var weapon in weapons)
            {   
                items.Add(weapon);
            }
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }
    
}
