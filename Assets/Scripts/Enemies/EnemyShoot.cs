using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour, IDamagable
{
    #region Enemy properties
    private float Health = 100f;
    private float MovementSpeed = 2.5f;
    private float Damage = 10f;
    private float ProjectileDamage = 15f;
    private float Experience = 25f;
    #endregion

    #region Shoot Variables and Timers
    private float ShootTriggerDistance = 10f;
    private bool CanShoot = true;
    private float ShootCooldown = 3f;
    #endregion

    #region References
    private Rigidbody2D Rigidbody;
    private GameObject Player;
    private SpriteRenderer SpriteRenderer;
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject ExperiencePrefab;
    private ObjectPooler ObjectPoolerInstance;
    #endregion

    private Vector2 PlayerPosition;
    private void Start()
    {
        ObjectPoolerInstance = ObjectPooler.Instance;
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        Health = Health * WaveSpawner.WaveDifficulty;
        Damage = Damage * WaveSpawner.WaveDifficulty;
        ProjectileDamage = ProjectileDamage * WaveSpawner.WaveDifficulty;
        MovementSpeed = MovementSpeed * WaveSpawner.WaveDifficulty;
    }

    private void Update()
    {
        if (IsDead())
        {
            InstantiateExperienceDrop();
            ObjectPoolerInstance.ReturnToPool(gameObject.name, this.gameObject);
        }
        FlipSprite();
        if (GetVetorToPlayer().magnitude <= ShootTriggerDistance)
        {
            if (CanShoot)
                StartCoroutine(Shoot());
        }
        else
        {
            MoveTowardsPlayer();
            PlayerPosition = Player.transform.position;
        }
    }

    private void FlipSprite()
    {
        if (GetVetorToPlayer().normalized.x > -0.1f)
        {
            SpriteRenderer.flipX = false;
        }
        else
        {
            SpriteRenderer.flipX = true;
        }
    }

    private void InstantiateExperienceDrop()
    {
        GameObject experience = ObjectPoolerInstance.SpawnFromPool("Experience", transform.position, Quaternion.identity);
        Experience experienceScript = ExperiencePrefab.GetComponent<Experience>();
        experienceScript.SetExperienceAmount(Experience);
    }

    private bool IsDead()
    {
        if(Health <= 0)
        {
            return true;
        }
        return false;
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, MovementSpeed * Time.deltaTime);
    }

    private Vector2 GetVetorToPlayer()
    {
        return Player.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamagable>().TakeDamage(Damage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
    }

    private IEnumerator Shoot()
    {
        CanShoot = false;

        Vector2 direction = GetVetorToPlayer().normalized;
        InstantiateProjectile();

        yield return new WaitForSeconds(ShootCooldown);

        CanShoot = true;
    }

    public void InstantiateProjectile()
    {
        GameObject projectile = ObjectPoolerInstance.SpawnFromPool("EnemyProjectile", transform.position, Quaternion.identity);
        EnemyProjectile enemyProjectileScript = projectile.GetComponent<EnemyProjectile>();
        enemyProjectileScript.SetThrowDirection(GetVetorToPlayer().normalized);
        enemyProjectileScript.SetProjectileDamage(ProjectileDamage);
    }
}
