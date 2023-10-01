using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlateKitchenObject : KitchenObject 
{
    public event EventHandler OnKitchenObjectsSetToPlate;

    public event EventHandler<OnIncrediantAddedOnGhostPlateKOEventArgs> OnIncrediantAddedOnGhostPlateKO;

    public class OnIncrediantAddedOnGhostPlateKOEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIncrediantToGhostPlate(KitchenObjectSO kitchenObjectSO)   // Test 
    {
    
        if (!validKitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        if(!kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            
            OnIncrediantAddedOnGhostPlateKO?.Invoke(this, new OnIncrediantAddedOnGhostPlateKOEventArgs
            {
                kitchenObjectSO = kitchenObjectSO                            // This Event For Adding Complete Plate Visual
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    public  List<KitchenObjectSO> GetKitchenObjectsList()
    {
        return kitchenObjectSOList;
    }

    public void GotTheKitchenObjectFromGhostPlate()
    {
        if(kitchenObjectSOList.Count == 0)
        {
            OnKitchenObjectsSetToPlate?.Invoke(this, EventArgs.Empty);
        }
    }

  

   
}
