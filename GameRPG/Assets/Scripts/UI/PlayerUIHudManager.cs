using UnityEngine;
using UnityEngine.UI;
namespace TV
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] UI_Sta_Bars healthBar;
        [SerializeField] UI_Sta_Bars staminaBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;
        public void RefeshHud()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetSta(newValue);
        }
        public void SetMaxHealthValue(int maxhealth)
        {
            healthBar.SetMaxSta(maxhealth);
        }
        public void SetNewStaminaValue(float  oldValue ,float  newValue)
        {
            staminaBar.SetSta(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxSta(maxStamina);
        }

       
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDataBase.instance.GetWeaponByID(weaponID);
            if (WorldItemDataBase.instance.GetWeaponByID(weaponID) == null)
            {
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if(weapon.itemIcon == null)
            {
                Debug.LogError("item has no Icon");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
            }

            rightWeaponQuickSlotIcon.enabled = true;
            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        }


        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDataBase.instance.GetWeaponByID(weaponID);
            if (WorldItemDataBase.instance.GetWeaponByID(weaponID) == null)
            {
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.LogError("item has no Icon");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
            }

            leftWeaponQuickSlotIcon.enabled = true;
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        }


    }
}
