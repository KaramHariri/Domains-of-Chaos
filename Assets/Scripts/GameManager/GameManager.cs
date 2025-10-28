using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    GameStateMachine GameStateMachine;
    MainMenuState MainMenuState;
    [HideInInspector] public GameplayState GameplayState;
    LevelUpSelectionState LevelUpSelectionState; 
    PauseMenuState PauseMenuState;
    GameOverState GameOverState;

    public InputActionReference PauseAction;
    private bool GameIsPaused = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);

        GameStateMachine = new GameStateMachine();
        MainMenuState = new MainMenuState();
        GameplayState = new GameplayState();
        LevelUpSelectionState = new LevelUpSelectionState();
        PauseMenuState = new PauseMenuState();
        GameOverState = new GameOverState();

        MainMenuState.Awake();
        GameplayState.Awake();
        LevelUpSelectionState.Awake();
        PauseMenuState.Awake();
        GameOverState.Awake();
    }

    private void PauseCallback(InputAction.CallbackContext context)
    {
        if(GameIsPaused != true)
        {
            SwitchToNextState(GameState.Pause);
            GameIsPaused = true;
        }
        else
        {
            SwitchToNextState(GameState.Gameplay);
            GameIsPaused = false;
        }
    }

    private void OnEnable()
    {
        PauseAction.action.started += PauseCallback; 
    }

    private void OnDisable()
    {
        PauseAction.action.started -= PauseCallback;
    }

    private void Start()
    {
        SwitchToNextState(GameState.MainMenu);
    }

    private void Update()
    {
        GameStateMachine.CurrentState.Execute();
    }

    private void LateUpdate()
    {
        GameStateMachine.CurrentState.LateExectue();
    }

    public void SwitchToNextState(GameState nextState)
    {
        switch (nextState)
        {
            case GameState.MainMenu:
                GameStateMachine.ChangeState(MainMenuState);
                break;
            case GameState.Gameplay:
                GameStateMachine.ChangeState(GameplayState);
                break;
            case GameState.Pause:
                GameStateMachine.ChangeState(PauseMenuState);
                break;
            case GameState.LevelUpSelection:
                GameStateMachine.ChangeState(LevelUpSelectionState);
                break;
            case GameState.GameOver:
                Debug.Log("Changing to Gameover state");
                break;
            default:
                Debug.Log("Problem occured when switching to other states");
                break;
        }
    }

    public IState GetCurrentState()
    {
        return GameStateMachine.CurrentState;
    }   
}



public enum GameState
{
    MainMenu,
    Gameplay,
    Pause,
    LevelUpSelection,
    GameOver
}
