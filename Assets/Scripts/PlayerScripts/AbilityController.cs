using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour, ILevelUP
{
    [Header("ThrowingSpearSettings")]
    [SerializeField] private float FireRate = 1.0f;
    [SerializeField] private GameObject SpearPrefab;
    private Vector2 DirectionToClosestEnemy = Vector2.zero;
    private float CurrentSpearCooldown = 0f;
    private float NextThrowingTime = 0f;

    [Header("ShieldSettings")]
    public int CurrentShieldCount = 0;      // Active orbiters (driven by ability level)
    public int MaxShieldCount = 0;
    public float Radius = 3f;
    public GameObject ShieldPrefab;
    public List<Shield> Shields = new List<Shield>();

    float CurrentAngle = 0f;
    public float OrbitSpeedDegPerSec = 90f;


    private void Start()
    {

    }

    private void Update()
    {
        CurrentAngle = OrbitSpeedDegPerSec * Time.deltaTime;

        FindClosestEnemy();
        if (CanThrowSpear())
        {
            InstantiateSpear();
        }

        if (CurrentShieldCount != MaxShieldCount)
        {
            Debug.Log("Shields Created");
            CreateShields();
            CurrentShieldCount = MaxShieldCount;
        }
    }

    private void LateUpdate()
    {
        CurrentAngle = OrbitSpeedDegPerSec * Time.deltaTime;
        RotateShields();
    }

    private bool CanThrowSpear()
    {
        CurrentSpearCooldown += Time.deltaTime;
        NextThrowingTime = 1f / FireRate;
        if (CurrentSpearCooldown >= NextThrowingTime)
        {
            CurrentSpearCooldown = 0;
            return true;
        }
        return false;
    }

    private void InstantiateSpear()
    {
        GameObject spear = Instantiate(SpearPrefab, transform.position, Quaternion.identity);
        spear.GetComponent<Spear>().SetThrowDirection(DirectionToClosestEnemy);
    }

    private void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        //Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy[] allEnemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToCurrentEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToCurrentEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToCurrentEnemy;
                closestEnemy = currentEnemy;
            }
        }
        DirectionToClosestEnemy = closestEnemy.transform.position - this.transform.position;
        DirectionToClosestEnemy.Normalize();
    }

    private void ClearShieldList()
    {
        foreach (var shield in Shields)
        {
            if (shield != null)
            {
                Destroy(shield.gameObject);
            }
        }
        Shields.Clear();
    }

    private void CreateShields()
    {
        ClearShieldList();
        float step = 360f / MaxShieldCount;

        for (int i = 0; i < MaxShieldCount; i++)
        {
            float angleDeg = i * step;
            float rad = angleDeg * Mathf.Deg2Rad;


            Vector3 offsetBetweenShield = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * Radius;
            Vector3 pos = transform.position + offsetBetweenShield;

            GameObject shieldObject = Instantiate(ShieldPrefab, pos, Quaternion.Euler(Vector3.zero));
            Shield shieldScripts = shieldObject.GetComponent<Shield>();
            Shields.Add(shieldScripts);
            shieldScripts.transform.SetParent(this.transform);
        }
    }

    private void RotateShields()
    {

        foreach (var shield in Shields)
        {
            shield.RotateAroundPlayer(this.transform, CurrentAngle);
        }
    }

    public void LevelUpAbility()
    {
        Debug.Log("Leveled Up");
    }
}
