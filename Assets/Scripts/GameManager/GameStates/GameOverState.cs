using UnityEngine;

public class GameOverState : IState
{
    private GameObject GameOverPanel;

    public void Awake()
    {
        GameOverPanel = GameObject.Find("GameOverPanel");
        GameOverPanel.SetActive(false);
    }

    public void Enter()
    {
        GameOverPanel.SetActive(true);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        GameOverPanel.SetActive(false);
    }

    public void LateExectue()
    {
    }
}
