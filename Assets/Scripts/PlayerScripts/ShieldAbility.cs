using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility
{
    public List<Shield> Shields = new List<Shield>();

    private void ClearShieldList()
    {
        foreach (var shield in Shields)
        {
            if (shield != null)
            {
                UnityEngine.Object.Destroy(shield.gameObject);
            }
        }
        Shields.Clear();
    }

    public void CreateShields(GameObject shieldPrefab, Transform playerTransform, float distanceToPlayer, int maxShieldCount)
    {
        ClearShieldList();
        float step = 360f / maxShieldCount;

        for (int i = 0; i < maxShieldCount; i++)
        {
            float angleDeg = i * step;
            float rad = angleDeg * Mathf.Deg2Rad;


            Vector3 offsetBetweenShield = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * distanceToPlayer;
            Vector3 pos = playerTransform.position + offsetBetweenShield;

            GameObject shieldObject = GameObject.Instantiate(shieldPrefab, pos, Quaternion.Euler(Vector3.zero));
            Shield shieldScripts = shieldObject.GetComponent<Shield>();
            Shields.Add(shieldScripts);
            shieldScripts.transform.SetParent(playerTransform);
        }
    }
    
    public void RotateShields(Transform playerTransform,float orbitSpeed)
    {
        foreach (var shield in Shields)
        {
            shield.RotateAroundPlayer(playerTransform, orbitSpeed);
        }
    }
}
