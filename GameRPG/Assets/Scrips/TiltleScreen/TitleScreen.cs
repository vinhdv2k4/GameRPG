using Unity.Netcode;
using UnityEngine;
namespace TV
{
    public class TitleScreen : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void StartNewGame()
        {
            WorldGameSave.instance.CreateNewGame();
            StartCoroutine(WorldGameSave.instance.loadWorldScence());

        }
    }
}