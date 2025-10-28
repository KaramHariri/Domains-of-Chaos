using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpearAbilityStats", menuName = "Scriptable Objects/AbilitiesStats/SpearAbilityStats")]
public class SpearAbilityStats : ScriptableObject
{
    public List<SpearAbilityLevelInfo> SpearInfo;
    public int CurrentAbilityLevel = 0;
    public int MaxAbilityLevel = 8;

    internal void ResetAbilityLevel()
    {
        CurrentAbilityLevel = 0;
    }
}

[Serializable]
public class SpearAbilityLevelInfo
{
    [Header("Ability Name info")]
    public string Level;
    public new string Name;
    public string Description;
    public Sprite Sprite;
    public AbilityType AbilityType ;


    [Header("Shield Variables")]
    public int SpearPiercingCount;
    public float FireRateMultiplierPercentage;
    public float SpearDamageMultiplierPercentage;
    public bool MaxAbility;
}