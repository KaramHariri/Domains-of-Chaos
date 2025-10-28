using System.Collections;
using UnityEngine;

public class EnemyDash : MonoBehaviour, IDamagable
{
    #region Enemy properties
    private float Health = 100f;
    private float MovementSpeed = 3f;
    private float Damage = 10f;
    private float DashDamage = 20f;
    private float Experience = 20f;
    #endregion

    #region Dash Variables and Timers
    private float DashTriggerDistance = 10f;
    private bool IsDashing = false;
    private float DashWindUpTime = 0.35f;
    private float DashDuration = 0.3f;
    private float DashCooldown = 3f;
    #endregion

    #region References
    private Rigidbody2D Rigidbody;
    private GameObject Player;
    private SpriteRenderer SpriteRenderer;
    [SerializeField] GameObject ExperiencePrefab;
    private ObjectPooler ObjectPoolerInstance = ObjectPooler.Instance;
    #endregion
    
    private Vector2 PlayerPosition;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        Health = Health * WaveSpawner.WaveDifficulty;
        Damage = Damage * WaveSpawner.WaveDifficulty;
        DashDamage = DashDamage * WaveSpawner.WaveDifficulty;
        MovementSpeed = MovementSpeed * WaveSpawner.WaveDifficulty;
    }

    private void Update()
    {
        if (IsDead())
        {
            InstantiateExperienceDrop();
            ObjectPoolerInstance.ReturnToPool(gameObject.name, this.gameObject);
        }
        if (IsDashing) return;

        FlipSprite();
        if (GetVetorToPlayer().magnitude <= DashTriggerDistance)
        {
            StartCoroutine(Dash());
        }
        else
        {
            MoveTowardsPlayer();
            PlayerPosition = Player.transform.position;
        }
    }

    private void InstantiateExperienceDrop()
    {
        GameObject experience = ObjectPoolerInstance.SpawnFromPool("Experience", transform.position, Quaternion.identity);
        Experience experienceScript = ExperiencePrefab.GetComponent<Experience>();
        experienceScript.SetExperienceAmount(Experience);
    }

    private void FlipSprite()
    {
        if (GetVetorToPlayer().normalized.x < -0.1f)
        {
            SpriteRenderer.flipX = false;
        }
        else
        {
            SpriteRenderer.flipX = true;
        }
    }

    private bool IsDead()
    {
        if (Health <= 0)
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
            if (IsDashing)
                other.GetComponent<IDamagable>().TakeDamage(DashDamage);
            else
                other.GetComponent<IDamagable>().TakeDamage(Damage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
    }

    private IEnumerator Dash()
    {
        IsDashing = true;

        yield return new WaitForSeconds(DashWindUpTime);

        Vector2 startPosition = transform.position;
        Vector2 direction = (PlayerPosition - startPosition).normalized;
        float distanceToPlayer = Vector2.Distance(startPosition, PlayerPosition);
        Vector2 dashTarget = startPosition + (direction * (distanceToPlayer * 2f));

        float elapsed = 0f;
        while (elapsed < DashDuration)
        {
            Rigidbody.MovePosition(Vector2.Lerp(startPosition, dashTarget, elapsed / DashDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        Rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(DashCooldown);

        IsDashing = false;
    }
}
