using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    Image AbilitySprite;
    TextMeshProUGUI AbilityName;
    TextMeshProUGUI AbilityDescription;

    [SerializeField] Button button;
    public AbilityType AbilityType;
    LevelUpCardSelectionHandler LevelUpSelectionHandler;
    public static event Action<AbilityType> OnAbilitySelected;

    private void Awake()
    {
        LevelUpSelectionHandler = GameObject.FindFirstObjectByType<LevelUpCardSelectionHandler>().GetComponent<LevelUpCardSelectionHandler>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void SetCardInfo(Sprite abilitySprite, string abilityName, string abilityDescription)
    {
        AbilitySprite = transform.GetChild(0).GetComponent<Image>();
        AbilityName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        AbilityDescription = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        AbilitySprite.sprite = abilitySprite;
        AbilityName.text = abilityName;
        AbilityDescription.text = abilityDescription;
    }

    public void OnButtonClicked()
    {
        //LevelUpSelectionHandler.UpdateSelectedAbilityLevel(AbilityType);
        OnAbilitySelected?.Invoke(AbilityType);
    }
}
