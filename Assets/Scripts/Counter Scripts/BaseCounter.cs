using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent 
{

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform playerStandPoint;

    private KitchenObject kitchenObject;


    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter.Interact()");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter.InteractAlternate()");
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public Transform GetPlayerStandPos()
    {
        return playerStandPoint;
    }

    public void AdjustPlayerPosInteract(Player player)
    {
        if(player.GetInteractCounter() != null)
        {
           // StartCoroutine(StartPositioning(player));
        }
        
    }
    //IEnumerator StartPositioning(Player player)
    //{
    //    while(true)
    //    {
    //        Debug.Log(this.name + "interacted");

    //        float rotationSpeed = 10f;
    //        Vector3 dir = (player.transform.position - transform.position);

    //        player.transform.forward = Vector3.Slerp(-player.transform.forward, dir, Time.deltaTime * rotationSpeed);
    //        yield return null;
    //    }
        
    //}
}
