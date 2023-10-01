using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;


    [System.Serializable]
    public struct KitchenObjectSO_GameObjects
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject kitchenObjectVisual;
    }

    [SerializeField]private List<KitchenObjectSO_GameObjects> kitchenObjectsSO_GameObjectsList; 
    private void Start()
    {
        plateKitchenObject.OnIncrediantAdded += PlateKitchenObject_OnIncrediantAdded;
        plateKitchenObject.OnIncrediantAddedFromGhostPlate += PlateKitchenObject_OnIncrediantAddedFromGhostPlate;

        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {  
           kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIncrediantAddedFromGhostPlate(object sender, PlateKitchenObject.OnIncrediantAddedFromGhostPlateEventArgs e)
    {
        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {
            foreach(KitchenObjectSO kitchenObjectSO in e.kitchenObjectSOs)
            {
                if (kitchenObjectSO_GameObjects.kitchenObjectSO == kitchenObjectSO)
                {
                    kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(true);
                }
            }
            
        }
    }

    private void PlateKitchenObject_OnIncrediantAdded(object sender, PlateKitchenObject.OnIncrediantAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {
            if (kitchenObjectSO_GameObjects.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(true);
            }
        }
    }


    private void OnDestroy()
    {
        plateKitchenObject.OnIncrediantAdded -= PlateKitchenObject_OnIncrediantAdded;
    }
}
