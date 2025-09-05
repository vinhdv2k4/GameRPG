
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV
{
    public class WorldAiManager : MonoBehaviour
    {
        public  static WorldAiManager instance;
        [Header("Characters")]
        [SerializeField] List<AiCharacterSpawner> aiCharacterSpawners;
        [SerializeField] List<GameObject> spawnedCharacters;
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
        }
    
        public void SpawnAiCharacter(AiCharacterSpawner aiCharacterSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(aiCharacterSpawner);
                aiCharacterSpawner.AttemptToSpawnCharacter();
            }
         
        }

        private void DeSpawnAllCharacter()
        {
            foreach (var character in spawnedCharacters)
            {
                    character.GetComponent<NetworkObject>().Despawn();
            }
        }
        private void DisableAllCharacters()
        {

        }

    }
}
