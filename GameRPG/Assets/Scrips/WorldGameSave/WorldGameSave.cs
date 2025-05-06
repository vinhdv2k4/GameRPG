using System.Collections;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV
{
    public class WorldGameSave : MonoBehaviour
    {
        public static WorldGameSave instance;
        [SerializeField] PlayerManager player;
        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;
        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer ")]
        public SaveFileDataWirte saveFileDataWirte;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotSavedUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Charater Slots")]
        public CharacterSaveData characterSlot1;
        //public CharacterSaveData characterSlot2;
        //public CharacterSaveData characterSlot3;
        //public CharacterSaveData characterSlot4;
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
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (saveGame)
            {
                    saveGame = false;
                SaveGame();
            }
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }

        }

        public string DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string filename = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot01:
                    filename = "CharacterSlot01";
                    break;
                case CharacterSlot.CharacterSlot02:
                    filename = "CharacterSlot02";
                    break;
                case CharacterSlot.CharacterSlot03:
                    filename = "CharacterSlot03";
                    break;
                case CharacterSlot.CharacterSlot04:
                    filename = "CharacterSlot04";
                    break;
            }
            return filename;
        }

        public void CreateNewGame()
        {
            saveFileName= DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);
            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);
           
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath =Application.persistentDataPath;
            saveFileDataWirte.saveDataFileName = saveFileName;
            currentCharacterData = saveFileDataWirte.LoadSaveFile();

            StartCoroutine(loadWorldScence());
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            saveFileDataWirte.saveDataFileName = saveFileName;

            player.SaveGameCurrentCharacterData(ref currentCharacterData);

            saveFileDataWirte.CreateNewCharacterSaveFile(currentCharacterData);

        }
        public IEnumerator loadWorldScence()
        {

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }


        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }



    }
}