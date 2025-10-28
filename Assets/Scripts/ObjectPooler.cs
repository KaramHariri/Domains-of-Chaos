using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPool> ObjectPoolList;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public static ObjectPooler Instance;
    private int PoolResizeAmount = 10;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (ObjectPool objPool in ObjectPoolList)
        {
            GameObject poolParent = new GameObject();
            poolParent.name = objPool.PoolTag;
            poolParent.transform.SetParent(this.transform);
            Queue<GameObject> pooledObjects = new Queue<GameObject>();
            for (int i = 0; i < objPool.PoolInitialSize; i++)
            {
                GameObject obj = Instantiate(objPool.PooledObjectPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(poolParent.transform);
                obj.name = objPool.PoolTag;
                pooledObjects.Enqueue(obj);
            }
            PoolDictionary.Add(objPool.PoolTag, pooledObjects);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Tag Check
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with the tag " + tag + " does not exist");
            return null;
        }

        Queue<GameObject> objectPool = PoolDictionary[tag];

        // Expand the pool if it is empty
        if (objectPool.Count == 0)
        {
            ObjectPool pool = null;
            for (int i = 0; i < ObjectPoolList.Count; i++)
            {
                if (ObjectPoolList[i].PoolTag == tag)
                {
                    pool = ObjectPoolList[i];
                    break;
                }
            }

            if (pool != null)
            {
                for (int i = 0; i < PoolResizeAmount; i++)
                {
                    GameObject poolParent = transform.Find(pool.PoolTag).gameObject;
                    GameObject obj = Instantiate(pool.PooledObjectPrefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(poolParent.transform);
                    obj.name = pool.PoolTag;
                    objectPool.Enqueue(obj);
                }
            }
            else
            {
                Debug.LogWarning("No Object pool found with the tag " + tag);
            }
        }

        GameObject objectToSpawn = objectPool.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject pooledObject)
    {
        pooledObject.SetActive(false);
        if (PoolDictionary.ContainsKey(tag))
        {
            PoolDictionary[tag].Enqueue(pooledObject);
        }
        else
        {
            Destroy(pooledObject);
            Debug.LogWarning("Could not return the object to the Pool with the tag " + tag + " because the tag does not exist so the object is destroyed");
        }
    }
}

#region Object Pool class
[System.Serializable]
public class ObjectPool
{
    public string PoolTag;
    public GameObject PooledObjectPrefab;
    public int PoolInitialSize;
}
#endregion


