using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DashAbilityStats", menuName = "Scriptable Objects/AbilitiesStats/DashAbilityStats")]
public class DashAbilityStats : ScriptableObject
{
    public List<DashAbilityLevelInfo> DashInfo;
    public int CurrentAbilityLevel = 0;
    public int MaxAbilityLevel = 8;

    public void ResetAbilityLevel()
    {
        CurrentAbilityLevel = 0;
    }
}

[Serializable]
public class DashAbilityLevelInfo
{
    [Header("Ability Name info")]
    public string Level;
    public new string Name;
    public string Description;
    public Sprite Sprite;
    public AbilityType AbilityType;

    [Header("Dash Variables")]
    public int DashCooldownReductionPercentage;
    public int DashDistanceMultiplierPercentage;
    public int MovementSpeedMultiplierPercentage;
    public bool MaxAbility;
}
