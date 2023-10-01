using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;
    //private List<BaseCustomer> spawnedCustomers;
    //[SerializeField] List<BaseCustomer> spawnObjectPrefab;
    [SerializeField] Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
        //spawnedCustomers = new List<BaseCustomer>();
    }

    public BaseCustomer GetCustomer()
    {
        //BaseCustomer customer = CreateCustomer();
        var customerGameObj = ObjectPoolManager.Instance.GetCustomerGameObjectFromPool();
        int randIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randIndex];
        Vector3 newPos = selectedSpawnPoint.position;
        customerGameObj.transform.position = newPos;

        if (!customerGameObj.TryGetComponent(out BaseCustomer baseCustomer))
        {
            Debug.LogError("There Is NO Customer Dude");
           return null;
        }
        else
        {
            return baseCustomer;
        }
       
    }
    //private BaseCustomer CreateCustomer()
    //{
        
    //    if (spawnObjectPrefab == null || spawnPoints.Length == 0) 
    //    {
    //        Debug.LogError("Please Assign Object To Spawn & Set Spawn Points");
    //        return null;
    //    }
    //    else
    //    {
            
    //        int randIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
    //        Transform selectedSpawnPoint = spawnPoints[randIndex];
    //        Vector3 newPos = selectedSpawnPoint.position;
    //        var selectedCustomer = Instantiate(spawnObjectPrefab[UnityEngine.Random.Range(0, spawnObjectPrefab.Count)], newPos,
    //            Quaternion.identity);
       
    //        return selectedCustomer;
    //    }
    //}


}
