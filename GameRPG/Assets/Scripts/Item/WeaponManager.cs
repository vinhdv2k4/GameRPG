using UnityEngine;
namespace TV
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWithWeapon, WeaponItem weapon)
        {

            meleeDamageCollider.characterCasingDamage = characterWithWeapon;
            meleeDamageCollider.physicDamage = weapon.physicalDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.holyDamage = weapon.HolyDamage;
            meleeDamageCollider.poisonDamage = weapon.poisonDamage;

            meleeDamageCollider.lightAttackModifier_01 = weapon.lightAttackModifier_01;
            meleeDamageCollider.lightAttackModifier_02 = weapon.lightAttackModifier_02;

            meleeDamageCollider.heavyAttackModifier_01 = weapon.heavyAttackModifier_01;
            meleeDamageCollider.heavyAttackModifier_02 = weapon.heavyAttackModifier_02;

            meleeDamageCollider.chargeAttackModifier_01 = weapon.chargeAttackModifier_01;
            meleeDamageCollider.chargeAttackModifier_02 = weapon.chargeAttackModifier_02;
        }
    
    }
}
