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

            meleeDamageCollider.lightAttackModifier = weapon.lightAttackModifier;
        }
    
    }
}
