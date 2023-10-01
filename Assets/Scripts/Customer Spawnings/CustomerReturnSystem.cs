using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerReturnSystem : MonoBehaviour
{
    public static CustomerReturnSystem Instance { get; private set; }


    [SerializeField] private Transform[] returnPoints;
    [SerializeField] private Transform[] thiefReturnPoints;

    private void Awake()
    {
        Instance = this;    
    }
    public Transform GetRandomReturnPoint()
    {
        int randTranformIndex = Random.Range(0, returnPoints.Length);
        Transform selectedTransform = returnPoints[randTranformIndex];

        return selectedTransform;
    }

    public Transform GetRandomTheifReturnPoint()
    {
        int randTheifTranformIndex = Random.Range(0, thiefReturnPoints.Length);
        Transform selectedTheifTransform = thiefReturnPoints[randTheifTranformIndex];

        return selectedTheifTransform;
    }
}
