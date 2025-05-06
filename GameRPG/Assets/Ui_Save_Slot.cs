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
            saveFileDataWirte = new SaveFileDataWirte();
            saveFileDataWirte.saveDataDirectionPath = Application.persistentDataPath;
            if (characterSlot == CharacterSlot.CharacterSlot01)
            {
                saveFileDataWirte.saveDataFileName = WorldGameSave.instance.DecideCharacterFileOnBasedOnCharacterSlotBeingUsed(characterSlot);
            }

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
    }
}

