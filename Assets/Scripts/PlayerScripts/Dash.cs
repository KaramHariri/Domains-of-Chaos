using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    //Serialized fields
    [Header("Dash Settings")]
    [SerializeField] private float DashDuration = 0.25f;
    [SerializeField] private float DashCooldown = 1f;
    [SerializeField] private float DashMultiplierPercentage = 0f;

    // Timers for tracking the reset dash ability
    private float CurrentDashcooldownResetTimer = 2f;
    private float DashCooldownResetTimer = 2f;

    private float DashMultiplier = 1f;
    private float DashDistance = 25f;
    private float CooldownBetweenMultipleDashes = 0.15f;

    private int NumberOfDashesLeft = 1;
    private int MaxNumberOfDashes = 1;

    private bool IsDashing = false;
    private bool CanDash = true;

    Vector2 DashDirection = Vector2.zero;

    public void DashCallback()
    {
        if (CanDash)
            StartCoroutine(DashEnumerator());
    }

    public void DecreaseDashAbilityResetTimer()
    {
        CurrentDashcooldownResetTimer -= Time.deltaTime;
    }

    public void ResetDashCooldowns()
    {
        if (IsAbilityResetTime())
        {
            NumberOfDashesLeft = MaxNumberOfDashes;
            CurrentDashcooldownResetTimer = DashCooldownResetTimer;
        }
    }

    private float GetDashIncrementValueFromPercentage()
    {
        return 1f + (DashMultiplierPercentage / 100);
    }

    private bool IsAbilityResetTime()
    {
        if (CurrentDashcooldownResetTimer <= 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DashEnumerator()
    {
        CurrentDashcooldownResetTimer = DashCooldownResetTimer;
        NumberOfDashesLeft--;

        CanDash = false;
        IsDashing = true;

        DashMultiplier = GetDashIncrementValueFromPercentage();
        DashDirection = PlayerController.Instance.GetInputDirection();
        PlayerController.Instance.Rb.linearVelocity = new Vector2(DashDirection.x * DashDistance * DashMultiplier, DashDirection.y * DashDistance * DashMultiplier);

        yield return new WaitForSeconds(DashDuration);
        IsDashing = false;

        if (NumberOfDashesLeft > 0)
        {
            CanDash = true;
            yield return new WaitForSeconds(CooldownBetweenMultipleDashes);
        }
        else
        {
            yield return new WaitForSeconds(DashCooldown);
            CanDash = true;
            NumberOfDashesLeft = MaxNumberOfDashes;
        }
    }

    public bool IsCurrentlyDashing()
    {
        if (IsDashing == true)
            return true;
        return false;
    }
}
