using UnityEngine;
namespace TV
{
    public class PlayerEquitmentManager : CharacterEquitMentManager
    {
        PlayerManager player;
        public WeaponModelInstantiationSlot rightHandSLot;
        public WeaponModelInstantiationSlot leftHandSLot;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            IntializeWeaponSlots();
        }
        protected override void Start()
        {
            base.Start();
            LoadWeaponOnBothHands();
        }
        private void IntializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponModelSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSLot = weaponSlot;
                }
                else if (weaponSlot.weaponModelSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSLot = weaponSlot;
                }
            }
        }
        public void LoadWeaponOnBothHands()
        {
            LoadLeftWeapon();
            LoadRightWeapon();
        }
        public void LoadRightWeapon() {
        if(player.playerInventoryManager.currentLeftWeapon != null)
            {
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightWeapon.weaponModel);
                rightHandSLot.LoadWeapon(rightHandWeaponModel);
            }
        }

        public void LoadLeftWeapon() {
            if (player.playerInventoryManager.currentLeftWeapon != null)
            {
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftWeapon.weaponModel);
                leftHandSLot.LoadWeapon(leftHandWeaponModel);
            }
        }
    }
}
