    using Unity.Entities.UniversalDelegates;
    using UnityEngine;
    namespace TV
    {
        public class PlayerEquitmentManager : CharacterEquitMentManager
        {
            PlayerManager player;
            public WeaponModelInstantiationSlot rightHandSLot;
            public WeaponModelInstantiationSlot leftHandSLot;

            [SerializeField] WeaponManager rightWeaponManager;
            [SerializeField] WeaponManager leftWeaponManager;

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
                    if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                    {
                        rightHandSLot = weaponSlot;
                    }
                    else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
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

            public void SwitchRightWeapon()
            {
                if (!player.IsOwner) return;

                player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

                WeaponItem selectedWeapon = null;
                // Tăng chỉ số để chuyển sang vũ khí tiếp theo
                player.playerInventoryManager.rightHandWeaponIndex++;

                // Nếu vượt giới hạn slot, reset về 0
                if (player.playerInventoryManager.rightHandWeaponIndex > 2 || player.playerInventoryManager.rightHandWeaponIndex < 0)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = 0;
                float weaponCount = 0;
                WeaponItem firstweapon = null;
                int firstWeaponPosition = 0;
                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDataBase.instance.unWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstweapon == null)
                        {
                            firstweapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;

                        }
                    }
                    return;
                }


                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDataBase.instance.unWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstweapon.itemID;
                }
                return;
            }


                foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDataBase.instance.unWeapon.itemID)
                    {
                        selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                        player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                    }
                }
                if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
                {
                    SwitchRightWeapon();
                }
                
        }




            public void LoadRightWeapon()
            {
               if(player.playerInventoryManager.currentRightWeapon  != null)
                {

                rightHandSLot.UnloadWeapon();   
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightWeapon.weaponModel );
                    rightHandSLot.LoadWeapon(rightHandWeaponModel);
                    rightWeaponManager =rightHandWeaponModel.GetComponent<WeaponManager>();
                    rightWeaponManager.SetWeaponDamage(player,player.playerInventoryManager.currentRightWeapon);
                }
            
            }

            public void SwitchLeftWeapon()
            {
           
            }
            public void LoadLeftWeapon()
            {
                if (player.playerInventoryManager.currentLeftWeapon != null)
                {
                    leftHandSLot.UnloadWeapon();
                    leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftWeapon.weaponModel);
                    leftHandSLot.LoadWeapon(leftHandWeaponModel);
                    leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                    leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftWeapon);
            }
        }

        public void OpenDamageCollier()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollier()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
    } 
}


