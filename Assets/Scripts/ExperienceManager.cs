using System;
using TMPro;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve ExperienceCurve;

    private int CurrentLevel = 0;
    private int MaxLevel = 32;
    private float CurrentExperienceAmount = 0f;
    private float PreviousLevelExperience = 0f;
    private float NextLevelExperience = 0f;
    private float StartingNextLevelExperience = 100f;

    public static ExperienceManager Instance;
    public delegate void ExperienceChangeHandler(float amount);
    public event ExperienceChangeHandler OnExperienceChange;
    public delegate void LevelUpHandler(int newLevelCount , float newExperienceValue);
    public event LevelUpHandler OnLevelChange;
    public delegate void OnLevelUpMenuHandler();
    public event OnLevelUpMenuHandler OnLevelUpMenu;

    // Singleton Check to make sure that there is only one instance at a time.
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
    }

    private void Start()
    {
        NextLevelExperience = StartingNextLevelExperience;
        OnExperienceChange?.Invoke(CurrentExperienceAmount);
    }

    public void AddExperience(float amount)
    {
        CurrentExperienceAmount += amount;

        float normalizedExperienceAmount = CurrentExperienceAmount / NextLevelExperience;
        OnExperienceChange?.Invoke(normalizedExperienceAmount);
        CheckForLevelUp();
    }

    void CheckForLevelUp()
    {
        if(CurrentExperienceAmount >= NextLevelExperience && CurrentLevel < MaxLevel)
        {
            UpdateLevel();
            GameManager.Instance.SwitchToNextState(GameState.LevelUpSelection);
        }

    }

    void UpdateLevel()
    {
        CurrentLevel++;
        
        PreviousLevelExperience = ExperienceCurve.Evaluate(CurrentLevel);
        NextLevelExperience = ExperienceCurve.Evaluate(CurrentLevel + 1);
        CurrentExperienceAmount -= PreviousLevelExperience;
        float normalizedExperienceAmount = CurrentExperienceAmount / NextLevelExperience;
        OnLevelChange?.Invoke(CurrentLevel, normalizedExperienceAmount);
        
    }
}
