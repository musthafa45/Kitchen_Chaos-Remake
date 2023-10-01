using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerRecipeCheck : MonoBehaviour
{
    public event Action<DeliveryCounter> OnPlacedCorrectRecipe;

    private RecipeSO customerAskedRecipeSO;

    public void SetCustomerAskedRecipe(BaseCustomer baseCustomer)
    {
        if(baseCustomer == null || baseCustomer.AskedRecipeSO == null)
        {
            Debug.LogError("No customer Or He Has No Recipe Ask " 
                + baseCustomer.name + baseCustomer.AskedRecipeSO);
            
        }
        else if(baseCustomer != null)
        {
            customerAskedRecipeSO = baseCustomer.AskedRecipeSO;
            Debug.Log("SuccesFully setted Customer Asked Recipe");
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject,DeliveryCounter attendDeliveryCounter)
    {
      
        bool allRecipeCorrect = false;

        //allRecipeCorrect = customerAskedRecipeSO.KitchenObjects.All(recipe =>
        //plateKitchenObject.GetKitchenObjectsList().Contains(recipe));
        if(customerAskedRecipeSO == null) { Debug.LogError("No customer Recipe"); }

        if (plateKitchenObject.GetKitchenObjectsList().Count != customerAskedRecipeSO.KitchenObjects.Count) // First Check
        {
            Debug.Log($" You deliverd Wrong");
            return;
        }
            

        foreach (KitchenObjectSO kitchenObjectSO in customerAskedRecipeSO.KitchenObjects)  // Second Check
        {
            if (plateKitchenObject.GetKitchenObjectsList().Contains(kitchenObjectSO))
            {
                allRecipeCorrect = true;
            }
            else
            {
                allRecipeCorrect = false;
            }
        }


        if (allRecipeCorrect)    // Verified
        {
            Debug.Log($" You deliverd Currect");
            OnPlacedCorrectRecipe?.Invoke(attendDeliveryCounter);
        }
        else
        {
            Debug.Log($" You deliverd Wrong");
        }

    }


}
