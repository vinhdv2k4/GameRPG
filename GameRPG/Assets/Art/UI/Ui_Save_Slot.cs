using JetBrains.Annotations;
using TMPro;
using UnityEngine;
namespace TV
{
    public class Ui_Save_Slot : MonoBehaviour
    {
        SaveFileDataWirte saveFileDataWirte;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot01:
                    {

                        saveFileDataWirte = new SaveFileDataWirte();
                        saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;

                        if (characterSlot == CharacterSlot.CharacterSlot01)

                            saveFileDataWirte.saveFileName = WorldGameSave.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);


                        // if file exits, get information from the file
                        if (saveFileDataWirte.CheckToSeeFileExits())
                        {
                            characterName.text = WorldGameSave.instance.characterSlot1.characterName;
                        }
                        else
                        {
                            gameObject.SetActive(false);
                        }
                    }
                    break;
                case CharacterSlot.CharacterSlot02:
                    saveFileDataWirte = new SaveFileDataWirte();
                    saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;

                    if (characterSlot == CharacterSlot.CharacterSlot02)

                        saveFileDataWirte.saveFileName = WorldGameSave.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);


                    // if file exits, get information from the file
                    if (saveFileDataWirte.CheckToSeeFileExits())
                    {
                        characterName.text = WorldGameSave.instance.characterSlot2.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot03:
                    saveFileDataWirte = new SaveFileDataWirte();
                    saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;

                    if (characterSlot == CharacterSlot.CharacterSlot03)

                        saveFileDataWirte.saveFileName = WorldGameSave.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);


                    // if file exits, get information from the file
                    if (saveFileDataWirte.CheckToSeeFileExits())
                    {
                        characterName.text = WorldGameSave.instance.characterSlot3.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot04:
                    saveFileDataWirte = new SaveFileDataWirte();
                    saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;

                    if (characterSlot == CharacterSlot.CharacterSlot04)

                        saveFileDataWirte.saveFileName = WorldGameSave.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);


                    // if file exits, get information from the file
                    if (saveFileDataWirte.CheckToSeeFileExits())
                    {
                        characterName.text = WorldGameSave.instance.characterSlot4.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

            }
        }
        public void LoadGameFromCharacter()
        {
            WorldGameSave.instance.currentCharacterSlotSavedUsed = characterSlot;
            WorldGameSave.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreen.instance.SelecteCharacterSlot(characterSlot);
        }
      
        }
    }


