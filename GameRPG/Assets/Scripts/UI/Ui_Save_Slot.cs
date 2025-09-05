using JetBrains.Annotations;
using TMPro;
using UnityEngine;
namespace TV
{
    public class Ui_Save_Slot : MonoBehaviour
    {
        SaveFileDataWirter saveFileDataWirte;

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
            saveFileDataWirte = new SaveFileDataWirter();
            saveFileDataWirte.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWirte.saveFileName = WorldGameSaveManager.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileDataWirte.CheckToSeeFileExits())
            {
                switch (characterSlot)
                {
                    case CharacterSlot.CharacterSlot01:
                        characterName.text = WorldGameSaveManager.instance.characterSlot1.characterName;
                        break;
                    case CharacterSlot.CharacterSlot02:
                        characterName.text = WorldGameSaveManager.instance.characterSlot2.characterName;
                        break;
                    case CharacterSlot.CharacterSlot03:
                        characterName.text = WorldGameSaveManager.instance.characterSlot3.characterName;
                        break;
                    case CharacterSlot.CharacterSlot04:
                        characterName.text = WorldGameSaveManager.instance.characterSlot4.characterName;
                        break;
                }
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
        public void LoadGameFromCharacter()
        {
            WorldGameSaveManager.instance.currentCharacterSlotSavedUsed = characterSlot;
            WorldGameSaveManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreen.instance.SelecteCharacterSlot(characterSlot);
        }
      
        }
    }


