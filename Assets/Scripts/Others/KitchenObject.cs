using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlateKitchenObject;

public class KitchenObject : MonoBehaviour
{
 

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

  
    public virtual void SetKitchenObjectParent(IKitchenObjectParent iKitchenObjectParent)
    {
        this.kitchenObjectParent?.ClearKitchenObject();            //if(this.kitchenObjectParent != null) {  ClearKitchenObject(); }
                                                                       
        this.kitchenObjectParent = iKitchenObjectParent;

        if (iKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("----");
        }
        iKitchenObjectParent.SetKitchenObject(this);

        transform.parent = iKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;


    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return this.kitchenObjectParent;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
   
    public virtual void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent)
    {

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
       
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
    public bool TryGetGhostPlate(out GhostPlateKitchenObject ghostPlateKitchenObject)
    {
        if (this is GhostPlateKitchenObject)
        {
            ghostPlateKitchenObject = this as GhostPlateKitchenObject;
            return true;
        }
        else
        {
            ghostPlateKitchenObject = null;
            return false;
        }
    }

    
    
}
