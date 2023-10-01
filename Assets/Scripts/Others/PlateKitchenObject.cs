using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIncrediantAddedEventArgs> OnIncrediantAdded;
    public event EventHandler<OnIncrediantAddedFromGhostPlateEventArgs> OnIncrediantAddedFromGhostPlate;

    public class OnIncrediantAddedFromGhostPlateEventArgs : EventArgs
    {
        public List<KitchenObjectSO> kitchenObjectSOs;
    }
    public class OnIncrediantAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIncrediantToPlate(KitchenObjectSO kitchenObjectSO)   // Test 
    {
      
        if (!validKitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (!kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIncrediantAdded?.Invoke(this, new OnIncrediantAddedEventArgs
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
    public List<KitchenObjectSO> GetKitchenObjectsList() 
    {
        return kitchenObjectSOList;
    }
    

    public bool SetGhostKitchenObjToPlate(GhostPlateKitchenObject ghostPlateKitchenObject)
    {
        bool objectExist = false;
        for(int i = 0; i < kitchenObjectSOList.Count; i++)
        {
            KitchenObjectSO kitchenObjectSO = kitchenObjectSOList[i];

            foreach(KitchenObjectSO kitchenObjectSOGhost in ghostPlateKitchenObject.GetKitchenObjectsList())
            {
               
                if(kitchenObjectSOGhost == kitchenObjectSO)
                {
                    Debug.Log("Already Exist");
                    objectExist = true;
                }
     
            }
        }

        if(!objectExist)
        {
            foreach (KitchenObjectSO kitchenObjectSO in ghostPlateKitchenObject.GetKitchenObjectsList())
            {
                kitchenObjectSOList.Add(kitchenObjectSO);

                OnIncrediantAddedFromGhostPlate?.Invoke(this, new OnIncrediantAddedFromGhostPlateEventArgs
                {
                    kitchenObjectSOs = GetKitchenObjectsList()
                });

                Debug.Log(kitchenObjectSO.ObjectName);
            }
            return true;
        }
        else
        {
          return false;
        }
    }

}
