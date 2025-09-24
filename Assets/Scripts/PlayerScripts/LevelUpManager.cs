using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public int currentExp = 0;
    public int maxLevelExp = 100;
    public int currentLevel = 1;


    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }


    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleExperienceChange(int newExperienceAmount)
    {
        currentExp += newExperienceAmount;
        if (currentExp > maxLevelExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel += 1;
        currentExp = 0;
        maxLevelExp += 100;
    }
}
