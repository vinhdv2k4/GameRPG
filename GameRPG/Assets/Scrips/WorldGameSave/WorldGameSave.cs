using System;
using System.Collections;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV
{
    public class WorldGameSave : MonoBehaviour
    {
        public static WorldGameSave instance;
        public PlayerManager player;
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
        public CharacterSaveData characterSlot2;
        public CharacterSaveData characterSlot3;
        public CharacterSaveData characterSlot4;
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
            LoadAllCharacterProfiles();
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
                    filename = "CharacterSlot01.json";
                    break;
                case CharacterSlot.CharacterSlot02:
                    filename = "CharacterSlot02.json";
                    break;
                case CharacterSlot.CharacterSlot03:
                    filename = "CharacterSlot03.json";
                    break;
                case CharacterSlot.CharacterSlot04:
                    filename = "CharacterSlot04.json";
                    break;
            }
            return filename;
        }

        public void CreateNewGame()
        {
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            // check to see if create a new save fiile (check other exits file)
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;

            foreach (CharacterSlot slot in Enum.GetValues(typeof(CharacterSlot)))
            {
                saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(slot);

                if (!saveFileDataWirte.CheckToSeeFileExits())
                {
                    currentCharacterSlotSavedUsed = slot;
                    currentCharacterData = new CharacterSaveData();
                    NewGame();
                    return;
                }
            }

            TitleScreen.instance.DisplayeNoFreeCharacterSlotPopUp();
        }

        private void NewGame(){
            player.playerNetworkManager.vitality.Value = 15;
            player.playerNetworkManager.endurance.Value = 10;
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);

            SaveGame();
            StartCoroutine(loadWorldScence());
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);
           
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath =Application.persistentDataPath;
            saveFileDataWirte.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWirte.LoadSaveFile();

            StartCoroutine(loadWorldScence());
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(currentCharacterSlotSavedUsed);
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            saveFileDataWirte.saveFileName = saveFileName;

            player.SaveGameCurrentCharacterData(ref currentCharacterData);

            saveFileDataWirte.CreateNewCharacterSaveFile(currentCharacterData);

        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
          
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);
            saveFileDataWirte.DeleteSaveFile();
        }
        private void LoadAllCharacterProfiles()
        {

            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot01);
            characterSlot1 = saveFileDataWirte.LoadSaveFile();

            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot02);
            characterSlot2 = saveFileDataWirte.LoadSaveFile();

            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot03);
            characterSlot3 = saveFileDataWirte.LoadSaveFile();

            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot04);
            characterSlot4 = saveFileDataWirte.LoadSaveFile();

        }
        public IEnumerator loadWorldScence()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }


        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }



    }
}