using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private GameObject[] customerPrefabs;
    private readonly int poolSize = 18;

    private List<GameObject> objectPool;

    private void Awake()
    {
        Instance = this;
        objectPool = new List<GameObject>();
    }
    private void Start()
    {
        for(int i = 0; i < poolSize; i++) 
        {
            GameObject go = Instantiate(customerPrefabs[Random.Range(0,customerPrefabs.Length)],transform);
            go.SetActive(false);
            objectPool.Add(go); 
        }
    }

    public GameObject GetCustomerGameObjectFromPool()
    {
        foreach(GameObject obj in objectPool)
        {
            if(!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // if There is no InActiveGameObjects or No GameObject On Pool
        GameObject newObj = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length)], transform);
        newObj.SetActive(true);
        objectPool.Add(newObj);
        Debug.Log("Created And Added New Obj On Pool");
        return newObj;
    }

    public void ReturnCustomerGameObjectToPool(GameObject customerGameObject)
    {
        customerGameObject.SetActive(false);
    }
}
