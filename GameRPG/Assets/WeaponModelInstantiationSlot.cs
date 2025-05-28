using UnityEngine;
namespace TV
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponModelSlot;
        public GameObject currentWeaponModle;

        public void UnloadWeapon()
        {
            if (currentWeaponModle != null)
            {
                Destroy(currentWeaponModle);
            }
        }
        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModle = weaponModel;
            weaponModel.transform.parent = transform;
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}
