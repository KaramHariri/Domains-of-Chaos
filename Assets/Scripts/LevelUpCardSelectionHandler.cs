using System.Collections.Generic;
using UnityEngine;

public class LevelUpCardSelectionHandler : MonoBehaviour
{
    [SerializeField] GameObject CardPrefab;

    [SerializeField] DashAbilityStats DashAbilityStats;
    [SerializeField] ShieldAbilityStats ShieldAbilityStats;
    [SerializeField] SpearAbilityStats SpearAbilityStats;
    [SerializeField] HealthAbilityStats HealthAbilityStats;

    private int MaxNumberOfAbilityOption = 3;
    private int CurrentNumberOfOptions = 0;

    List<int> AvailableOptions;

    public void ResetAbilityLevel()
    {
        DashAbilityStats.ResetAbilityLevel();
        ShieldAbilityStats.ResetAbilityLevel();
        SpearAbilityStats.ResetAbilityLevel();
        HealthAbilityStats.ResetAbilityLevel();
    }

    public void RandomizeCards()
    {
        DestroyAllCards();
        CurrentNumberOfOptions = 0;
        CreateRandomCards();
    }

    void CreateRandomCards()
    {
        AvailableOptions = new List<int> { 0, 1, 2, 3 };
        while (CurrentNumberOfOptions < MaxNumberOfAbilityOption && AvailableOptions.Count > 0)
        {

            int randomNumber = GetRandomNumber();
            
            switch (randomNumber)
            {
                case (int)AbilityType.DASH:
                    if (DashAbilityStats.CurrentAbilityLevel < DashAbilityStats.MaxAbilityLevel)
                    {
                        GameObject dashCard = Instantiate(CardPrefab);
                        dashCard.transform.SetParent(this.transform);
                        Card dashCardScript = dashCard.GetComponent<Card>();
                        dashCardScript.SetCardInfo(DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].Sprite, DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].Name,
                                                                  DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].Description);
                        dashCardScript.AbilityType = AbilityType.DASH;

                        CurrentNumberOfOptions++;
                        AvailableOptions.Remove(randomNumber);
                    }
                    break;
                case (int)AbilityType.SPEAR:
                    if (SpearAbilityStats.CurrentAbilityLevel < SpearAbilityStats.MaxAbilityLevel)
                    {
                        GameObject spearCard = Instantiate(CardPrefab);
                        spearCard.transform.SetParent(this.transform);
                        Card spearCardScript = spearCard.GetComponent<Card>();
                        spearCardScript.SetCardInfo(SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].Sprite, SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].Name,
                                                                  SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].Description);
                        spearCardScript.AbilityType = AbilityType.SPEAR;
                        CurrentNumberOfOptions++;
                        AvailableOptions.Remove(randomNumber);
                    }

                    break;
                case (int)AbilityType.SHIELD:
                    if (ShieldAbilityStats.CurrentAbilityLevel < ShieldAbilityStats.MaxAbilityLevel)
                    {
                        GameObject shieldCard = Instantiate(CardPrefab);
                        shieldCard.transform.SetParent(this.transform);
                        Card shieldCardScript = shieldCard.GetComponent<Card>();
                        shieldCardScript.SetCardInfo(ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].Sprite,
                                                                    ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].Name,
                                                                    ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].Description);
                        shieldCardScript.AbilityType = AbilityType.SHIELD;
                        CurrentNumberOfOptions++;
                        AvailableOptions.Remove(randomNumber);
                    }

                    break;
                case (int)AbilityType.HEALTH:
                    if (HealthAbilityStats.CurrentAbilityLevel < HealthAbilityStats.MaxAbilityLevel)
                    {
                        GameObject healthCard = Instantiate(CardPrefab);
                        healthCard.transform.SetParent(this.transform);
                        Card healthCardScript = healthCard.GetComponent<Card>();
                        healthCardScript.SetCardInfo(HealthAbilityStats.HealthInfo[HealthAbilityStats.CurrentAbilityLevel].Sprite,
                                                                    HealthAbilityStats.HealthInfo[HealthAbilityStats.CurrentAbilityLevel].Name,
                                                                    HealthAbilityStats.HealthInfo[HealthAbilityStats.CurrentAbilityLevel].Description);
                        healthCardScript.AbilityType = AbilityType.HEALTH;
                        CurrentNumberOfOptions++;
                        AvailableOptions.Remove(randomNumber);
                    }

                    break;
                default:
                    Debug.Log("An error occured while creating a card");
                    break;
            }

        }
    }

    //public void UpdateSelectedAbilityLevel(AbilityType selectedAbility)
    //{
    //    switch(selectedAbility)
    //    {
    //        case AbilityType.DASH:
    //            DashAbilityStats.CurrentAbilityLevel++;
    //            break;
    //        case AbilityType.SPEAR:
    //            SpearAbilityStats.CurrentAbilityLevel++;
    //            break;
    //        case AbilityType.SHIELD:
    //            ShieldAbilityStats.CurrentAbilityLevel++;
    //            break;
    //        case AbilityType.HEALTH:
    //            HealthAbilityStats.CurrentAbilityLevel++;
    //            break;
    //    }
    //}

    public void DestroyAllCards()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    int GetRandomNumber()
    {
        int index;
        int value;
        index = Random.Range(0,AvailableOptions.Count);

        value = AvailableOptions[index];

        AvailableOptions.RemoveAt(index);
        return value;
    }
}
