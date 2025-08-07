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

            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath  = Application.persistentDataPath;


            // CHECK TO SEE IF WE CAN MAKE A NEW SAVE FILE (CHECK FOR OTHER EXISTING FILES FIRST)
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot01);

            if (!saveFileDataWirte.CheckToSeeFileExits())
            {
                // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
                currentCharacterSlotSavedUsed = CharacterSlot.CharacterSlot01;
                currentCharacterData = new CharacterSaveData();

                NewGame();
                return;
            }

            // CHECK TO SEE IF WE CAN MAKE A NEW SAVE FILE (CHECK FOR OTHER EXISTING FILES FIRST)
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot02);

            if (!saveFileDataWirte.CheckToSeeFileExits())
            {
                // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
                currentCharacterSlotSavedUsed = CharacterSlot.CharacterSlot02;
                currentCharacterData = new CharacterSaveData();

                NewGame();
                return;
            }
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot03);

            if (!saveFileDataWirte.CheckToSeeFileExits())
            {
                // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
                currentCharacterSlotSavedUsed = CharacterSlot.CharacterSlot03;
                currentCharacterData = new CharacterSaveData();

                NewGame();
                return;
            }
            saveFileDataWirte.saveFileName = DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot04);

            if (!saveFileDataWirte.CheckToSeeFileExits())
            {
                // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
                currentCharacterSlotSavedUsed = CharacterSlot.CharacterSlot04;
                currentCharacterData = new CharacterSaveData();

                NewGame();
                return;
            }

            TitleScreen.instance.DisplayeNoFreeCharacterSlotPopUp();
        }

        private void NewGame(){
            player.playerNetworkManager.vitality.Value = 10;
            player.playerNetworkManager.endurance.Value = 10;


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