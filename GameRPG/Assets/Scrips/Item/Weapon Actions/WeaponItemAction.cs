using UnityEngine;
namespace TV
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAcrion(PlayerManager playerperformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerperformingAction.IsOwner)
            {
                playerperformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }
            Debug.Log("The Action has fired");
        }
    }
}