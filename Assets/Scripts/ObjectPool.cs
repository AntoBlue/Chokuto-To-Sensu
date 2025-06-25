using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private bool sharedInstance = true;

    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> pooledObjects2;
    public GameObject objectToPool;
    public GameObject objectToPool2;
    public int amountToPool;

    void Awake()
    {
        if (sharedInstance)
        {
            SharedInstance = this;
        }
    }

    //spawn inactive objects
    void Start()
    {
        pooledObjects = new List<GameObject>();
        pooledObjects2 = new List<GameObject>();
        GameObject tmp;
        GameObject tmp2;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            if (objectToPool2)
            {
                tmp2 = Instantiate(objectToPool2);
                tmp2.SetActive(false);
                pooledObjects2.Add(tmp2);
            }
        }
    }

    //give object type to scripts that need it
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject2()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects2[i].activeInHierarchy)
            {
                return pooledObjects2[i];
            }
        }

        return null;
    }
}