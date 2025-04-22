using UnityEngine;
namespace TV {
    public class PlayerManager : CharacterManager
    {
       public PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
        {
            
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }
        protected override void Update()
        {
            
            base.Update();
            // neu khong phai la owner thi khong cho di chuyen
            if (!IsOwner)
                return;
            playerLocomotionManager.HandleAllMovement();

        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();
           
            PlayerCamera.instance.HandleAllCameraActions();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
            }
        }

    }
}
