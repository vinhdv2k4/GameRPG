using UnityEngine;
namespace TV
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControlls playerControlls;
        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreen.instance.AttemptToDeleteCharacterSlot();
            }
        }
        private void OnEnable()
        {
            if(playerControlls == null)
            {
                playerControlls = new PlayerControlls();
                playerControlls.UI.X.performed += i => deleteCharacterSlot = true;
            }
            playerControlls.Enable();
        }
        private void OnDisable()
        {
            playerControlls.Disable();
        }

    }
}
