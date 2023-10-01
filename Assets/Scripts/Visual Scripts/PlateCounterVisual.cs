using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] PlateCounter plateCounter;
    [SerializeField] Transform plateVisual;
    [SerializeField] Transform counterPoint;
    private List<GameObject> plateGameObjects;


    private void Awake()
    {
        plateGameObjects = new List<GameObject>();
    }
    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject removedPlate = plateGameObjects[^1]; // GameObject removedPlate = plateGameObjects[plateGameObjects.Count - 1];
        plateGameObjects.Remove(removedPlate);
        Destroy(removedPlate);
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateTransform = Instantiate(plateVisual,counterPoint);
        float plateYOffset = .1f;
        plateTransform.localPosition = new Vector3(0 , plateYOffset * plateGameObjects.Count , 0);
        plateGameObjects.Add(plateTransform.gameObject);
       
    }
}
