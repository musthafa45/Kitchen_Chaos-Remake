using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlateCompleteVisual : MonoBehaviour
{
    [SerializeField] GhostPlateKitchenObject ghostPlateKitchenObject;


    [System.Serializable]
    public struct KitchenObjectSO_GameObjects
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject kitchenObjectVisual;
    }

    [SerializeField] private List<KitchenObjectSO_GameObjects> kitchenObjectsSO_GameObjectsList;


    private void OnEnable()
    {
        ghostPlateKitchenObject.OnIncrediantAddedOnGhostPlateKO += GhostPlateKitchenObject_OnIncrediantAddedOnGhostPlateKO1;
        ghostPlateKitchenObject.OnKitchenObjectsSetToPlate += GhostPlateKitchenObject_OnKitchenObjectsSetToPlate;

        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {
            kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(false);
        }
    }

    private void GhostPlateKitchenObject_OnIncrediantAddedOnGhostPlateKO1(object sender, GhostPlateKitchenObject.OnIncrediantAddedOnGhostPlateKOEventArgs e)
    {
        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {
            if (kitchenObjectSO_GameObjects.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(true);

            }
        }
    }


    private void GhostPlateKitchenObject_OnKitchenObjectsSetToPlate(object sender, System.EventArgs e)
    {
        foreach (KitchenObjectSO_GameObjects kitchenObjectSO_GameObjects in kitchenObjectsSO_GameObjectsList)
        {
            kitchenObjectSO_GameObjects.kitchenObjectVisual.SetActive(false);
        }
    }




}
