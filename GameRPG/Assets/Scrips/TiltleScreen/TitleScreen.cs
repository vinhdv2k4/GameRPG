using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
namespace TV
{
    public class TitleScreen : MonoBehaviour
    {
        public static TitleScreen instance;

        [Header("Menu")]
        [SerializeField] GameObject tileScreenMenu;
        [SerializeField] GameObject loadScreenMenu;

        [Header("Buttons")]
        [SerializeField] Button newGameButton;
        [SerializeField] Button loadMenureturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button deleteCharacterPopupConfirm;

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterOkButton;
        [SerializeField] GameObject deleteCharacterPopup;

        [Header("Character Slots")]
       
        public CharacterSlot currentSelectedSlots = CharacterSlot.NO_SLOT;

       
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
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void StartNewGame()
        {
            WorldGameSave.instance.CreateNewGame();
        

        }
        
        public void OpenLoadGame()
        {
            tileScreenMenu.SetActive(false);
            loadScreenMenu.SetActive(true);

            loadMenureturnButton.Select();
        }
        public void CloseLoadGameMenu()
        {
            loadScreenMenu.SetActive(false);
            tileScreenMenu.SetActive(true);
          

           mainMenuLoadGameButton.Select();
        }
        public void DisplayeNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterOkButton.Select();
        }
        public void CloseNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            newGameButton.Select();
        }
        public void SelecteCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlots = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentSelectedSlots = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlots != CharacterSlot.NO_SLOT)
            deleteCharacterPopup.SetActive(true);
            deleteCharacterPopupConfirm.Select();
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterPopup.SetActive(false);
           
            WorldGameSave.instance.DeleteGame(currentSelectedSlots);
            loadScreenMenu.SetActive(false);
            loadScreenMenu.SetActive(true);
            loadMenureturnButton.Select();
        }
        public void CloseDeleteCharacterPopup()
        {
            deleteCharacterPopup.SetActive(false);
            loadMenureturnButton.Select();
        }
    }
}