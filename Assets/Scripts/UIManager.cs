using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CurrentLevelText;
    [SerializeField] Image ExperienceFill;
    [SerializeField] ExperienceManager ExperienceManagerInstance;

    private void Start()
    {
        ExperienceFill.fillAmount = 0;
    }

    private void OnEnable()
    {
        ExperienceManagerInstance.OnExperienceChange += UpdateExperienceBar;
        ExperienceManagerInstance.OnLevelChange += UpdateExperienceUI;
    }

    private void OnDisable()
    {
        ExperienceManagerInstance.OnExperienceChange -= UpdateExperienceBar;
        ExperienceManagerInstance.OnLevelChange -= UpdateExperienceUI;

    }

    private void UpdateExperienceBar(float amount)
    {
        ExperienceFill.fillAmount = amount;
    }

    private void UpdateExperienceUI(int newLevelCount, float newCurrentExperience)
    {
        CurrentLevelText.text = newLevelCount.ToString();
        ExperienceFill.fillAmount = newCurrentExperience;
    }
}
