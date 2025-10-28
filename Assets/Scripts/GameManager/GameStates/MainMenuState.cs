using UnityEngine;
using UnityEngine.UI;

public enum MainMenuButton
{
    StartGame,
    Exit,

}

public class MainMenuState : IState
{
    private GameObject MainMenuPanel;
    private Button StartGameButton;
    private Button ExitGameButton;

    public void Awake()
    {
        if (MainMenuPanel == null)
            MainMenuPanel = InitialzePanel();
        if (StartGameButton == null)
            StartGameButton = InitializeButton(MainMenuButton.StartGame);
        if (ExitGameButton == null)
            ExitGameButton = InitializeButton(MainMenuButton.Exit);
    }

    public void Enter()
    {
        MainMenuPanel.SetActive(true);
        StartGameButton.onClick.AddListener(OnStartGameButtonClicked);
        ExitGameButton.onClick.AddListener(OnExitButtonClicked);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log("Exit Menu State");
        MainMenuPanel.SetActive(false);
    }

    Button InitializeButton(MainMenuButton buttonName)
    {
        Button button = GameObject.Find(buttonName.ToString()).GetComponent<Button>();
        if (button == null)
        {
            Debug.Log("Could not locate a button with this name" + buttonName.ToString());
        }
        return button;
    }

    GameObject InitialzePanel()
    {
        GameObject mainMenuPanel = GameObject.Find("MainMenuPanel");
        return mainMenuPanel;
    }

    private void OnStartGameButtonClicked()
    {
        Debug.Log("Start Button clicked");
        GameManager.Instance.SwitchToNextState(GameState.Gameplay);
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit ButtonClicked");
    }

    public void LateExectue()
    {
    }

    
}
