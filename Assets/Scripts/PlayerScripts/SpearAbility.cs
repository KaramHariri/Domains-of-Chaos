using UnityEngine;

public class SpearAbility
{
    private float FireRate = 1.0f;
    private Vector2 DirectionToClosestEnemy = Vector2.zero;
    private float CurrentSpearCooldown = 0f;
    private float NextThrowingTime = 0f;
    private GameObject ClosestEnemy;
    private float ThrowRange = 25f;

    public GameObject GetClosestEnemy(Vector3 playerPosition)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        ClosestEnemy = null;
        foreach (GameObject currentEnemy in allEnemies)
        {
            float distanceToCurrentEnemy = (currentEnemy.transform.position - playerPosition).magnitude;
            if (distanceToCurrentEnemy < distanceToClosestEnemy && distanceToCurrentEnemy < ThrowRange)
            {
                distanceToClosestEnemy = distanceToCurrentEnemy;
                ClosestEnemy = currentEnemy;
                SetDirectionToClosestEnemy(playerPosition);
            }
        }
        return ClosestEnemy;
    }

    public void SetDirectionToClosestEnemy(Vector3 playerPosition)
    {
        DirectionToClosestEnemy = ClosestEnemy.transform.position - playerPosition;
        DirectionToClosestEnemy.Normalize();
    }

    public Vector2 GetDirectionToClosestEnemy()
    {
        return DirectionToClosestEnemy;
    }

    public void InstantiateSpear(GameObject spearPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject spear = ObjectPooler.Instance.SpawnFromPool("Spear", position, rotation);
        spear.GetComponent<Spear>().SetThrowDirection(DirectionToClosestEnemy);
    }

    public bool CanThrowSpear()
    {
        CurrentSpearCooldown += Time.deltaTime;
        NextThrowingTime = 1f / (FireRate * ( 1 + AbilityController.Instance.SpearFireRateMultiplier));
        if (CurrentSpearCooldown >= NextThrowingTime)
        {
            CurrentSpearCooldown = 0;
            return true;
        }
        return false;
    }
}
