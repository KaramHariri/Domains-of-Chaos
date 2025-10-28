using System.Collections;
using UnityEngine;

// level 2 increase movement.
// level 3 cooldown reduction.
// level 4 extra dash.
// level 5 distance.
// level 6 increase movement.
// level 7 cooldown reduction.
// level 8 elemental dash.

public class Dash : MonoBehaviour
{
    private bool IsDashing = false;
    private bool DashOnCooldown = false;
    [HideInInspector] public bool HasDashAbility = false;
    [HideInInspector] public bool MaxDashAbility = false;
    [HideInInspector] public float DashCooldownReductionMultiplier = 0f;
    [HideInInspector] public float DashDistanceMultiplier = 0f;
    private float DashCooldown = 4f;
    private float DashBaseDistance = 25f;
    private float DashDuration = 0.25f;

    private float DashDamage = 30f;
    Vector2 DashDirection = Vector2.zero;
    private PlayerController PlayerController;
    private Rigidbody2D RigidBody;


    private void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        PlayerController = GetComponent<PlayerController>();
        DashDistanceMultiplier = 0f;
    }

    public void DashCallback()
    {
        if (CanDash() && HasDashAbility)
            StartCoroutine(DashEnumerator());
    }

    private bool CanDash()
    {
        if(DashOnCooldown == false)
            return true;

        return false;
    }

    private IEnumerator DashEnumerator()
    {
        IsDashing = true;
        DashOnCooldown = true;
        DashDirection = PlayerController.GetInputDirection();
        RigidBody.linearVelocity = new Vector2(DashDirection.x * DashBaseDistance * (1 + DashDistanceMultiplier), DashDirection.y * DashBaseDistance * (1 + DashDistanceMultiplier));

        yield return new WaitForSeconds(DashDuration);
        IsDashing = false;
        if (DashCooldownReductionMultiplier > 0)
        {
    
            yield return new WaitForSeconds(DashCooldown * (1 - DashCooldownReductionMultiplier));
            DashOnCooldown = false;
        }
        else
        {
            yield return new WaitForSeconds(DashCooldown);
            DashOnCooldown = false;
        }
    }

    public bool IsCurrentlyDashing()
    {
        if (IsDashing == true)
            return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(MaxDashAbility && other.CompareTag("Enemy") && IsCurrentlyDashing())
        {
            other.GetComponent<IDamagable>().TakeDamage(DashDamage);
            Debug.Log("Dash damage " +  DashDamage);
        }
    }
}
