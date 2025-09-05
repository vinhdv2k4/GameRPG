using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private UIDocument root;

    private VisualElement startGameScreen;
    private VisualElement loadGameScreen;

    [SerializeField] private Button startButton;
    private Button newGame;
    private Button loadGame;

    private void Awake()
    {
 
        root = GetComponent<UIDocument>();

        startButton = root.rootVisualElement.Q<Button>("StartButton");
        newGame = root.rootVisualElement.Q<Button>("NewGame");
        loadGame = root.rootVisualElement.Q<Button>("LoadGame");

        startGameScreen = root.rootVisualElement.Q<VisualElement>("StartScreen");
        loadGameScreen = root.rootVisualElement.Q<VisualElement>("LoadScreen");



        startButton.RegisterCallback<ClickEvent>(OnStartNetworkAsHost);
        startGameScreen.style.display = DisplayStyle.None;
        loadGameScreen.style.display = DisplayStyle.None;


    }
    public void OnStartNetworkAsHost(ClickEvent clickEvent)
    {
        NetworkManager.Singleton.StartHost();
        startGameScreen.style.display = DisplayStyle.Flex;
  
    }



   
}
