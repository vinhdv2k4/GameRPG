using Unity.Netcode;
using UnityEngine;

namespace TV
{
    public class PlayerUIManger : MonoBehaviour
    {
        public static PlayerUIManger instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUiPopUpManager playerUiPopUpManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUiPopUpManager = GetComponentInChildren<PlayerUiPopUpManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                NetworkManager.Singleton.Shutdown();// Shutdown bc hvae start host during title screen
                NetworkManager.Singleton.StartClient();

            }
        }
    }
}
