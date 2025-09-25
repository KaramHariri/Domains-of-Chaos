using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("References")]
    private Transform PlayerTransform;          // Center to orbit
    public GameObject Prefab;         // Orbiting object prefab

    [Header("Settings")]
    public float Radius = 3f;         // Distance from player
    public float OrbitSpeedDegPerSec = 90f;


    public void RotateAroundPlayer(Transform playerTransform, float angle)
    {
        transform.RotateAround(playerTransform.position, Vector3.forward, angle);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    


}
