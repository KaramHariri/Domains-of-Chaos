using UnityEngine;
using UnityEngine.UI;

public enum PauseMenuButton
{
    StartPausedGame,
    ExitGame
}

public class PauseMenuState : IState
{
    private GameObject PauseMenuPanel;
    private Button ExitButton;
    private Button StartGameButton;

    public void Awake()
    {
        PauseMenuPanel = InitialzePanel();

        if (StartGameButton == null)
            StartGameButton = InitializeButton(PauseMenuButton.StartPausedGame);
        if (ExitButton == null)
            ExitButton = InitializeButton(PauseMenuButton.ExitGame);

        PauseMenuPanel.SetActive(false);

    }

    public void Enter()
    {
        PauseMenuPanel.SetActive(true);
    }

    public void Execute()
    {
    }

    public void LateExectue()
    {
    }

    public void Exit()
    {
        PauseMenuPanel.SetActive(false);
    }

    Button InitializeButton(PauseMenuButton buttonName)
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
        GameObject mainMenuPanel = GameObject.Find("PauseMenuPanel");
        return mainMenuPanel;
    }
}
