using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float ExperienceAmount = 0;


    public void SetExperienceAmount(float amount)
    {
        ExperienceAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            ExperienceManager.Instance.AddExperience(ExperienceAmount);
            Destroy(gameObject);
        }
    }
}
