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


        [Header("Weapon Attack Modifiers")]
        public float lightAttackModifier_01 = 1.1f;
        public float lightAttackModifier_02 = 1.2f;
        public float heavyAttackModifier_01 = 1.5f;
        public float heavyAttackModifier_02 = 1.8f;
        public float chargeAttackModifier_01 = 2.0f;
        public float chargeAttackModifier_02 = 2.5f;

        [Header("Stamina Costs Modifier")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;


        [Header("Actions")]
        public WeaponItemAction oh_RB_Action;
        public WeaponItemAction oh_RT_Action;

        [Header("WWhooshes")]
        public AudioClip[] whooshes;
    }
}
