using UnityEngine;
namespace TV
{
    public class WeaponItem : Item
    {
        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int requiredStrength = 0;
        public int requiredDexterity = 0;
        public int requiredIntelligence = 0;
        public int requiredfaith = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int magicDamage = 0;
        public int HolyDamage = 0;
        public int poisonDamage = 0;

        [Header("Weapon Poise")]
        public int poiseDamage = 10;

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;



    }
}
