using Unity.VisualScripting;
using UnityEngine;

public class LevelUpSelectionState : IState
{
    private GameObject LevelUpSelectionPanel;
    private LevelUpCardSelectionHandler LevelUpSelectionHandler;
    private AbilityType CurrentSelectedAbility;

    public void Awake()
    {
        LevelUpSelectionPanel = GameObject.Find("LevelUpSelectionPanel");
        LevelUpSelectionHandler = LevelUpSelectionPanel.GetComponent<LevelUpCardSelectionHandler>();
        LevelUpSelectionHandler.ResetAbilityLevel();
        Card.OnAbilitySelected += OnAbilitySelected;
        LevelUpSelectionPanel.SetActive(false);
    }
    
    public void Enter()
    {
        LevelUpSelectionPanel.SetActive(true);
        LevelUpSelectionHandler.RandomizeCards();
        Card.OnAbilitySelected += OnAbilitySelected;
    }

    public void Execute()
    {
    }

    public void LateExectue()
    {
    }

    public void Exit()
    {
        // Invoke an event to the player controller and weapon controller to listen to for Updating the values.
        //LevelUpSelectionHandler.UpdateSelectedAbilityLevel(CurrentSelectedAbility);
        LevelUpSelectionHandler.DestroyAllCards();
        Card.OnAbilitySelected -= OnAbilitySelected;
        LevelUpSelectionPanel.SetActive(false);
    }

    void OnAbilitySelected(AbilityType currentSelectedAbility)
    {
        CurrentSelectedAbility = currentSelectedAbility;
        GameManager.Instance.SwitchToNextState(GameState.Gameplay);
    }
}
