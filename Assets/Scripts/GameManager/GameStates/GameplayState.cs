using UnityEngine;

public class GameplayState : IState
{
    GameObject GameplayPanel;
    WaveSpawner WaveSpawner;

    public void Awake()
    {
        GameplayPanel = GameObject.Find("GamePlayPanel");
        GameplayPanel.SetActive(false);
        WaveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
    }

    public void Enter()
    {
        GameplayPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Execute()
    {
        PlayerController.Instance.UpdatePlayer();
        WaveSpawner.StartSpawningWaves();
    }

    public void LateExectue()
    {
        PlayerController.Instance.LatePlayerUpdate();
    }

    public void Exit()
    {
        GameplayPanel.SetActive(false);
        Time.timeScale = 0f;
    }

}
