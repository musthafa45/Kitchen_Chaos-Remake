using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    //public class OnProgressChangedEventArgs
    //{
    //    public float ProgressNormalized;
    //}
    public event EventHandler OnCut;
    public event EventHandler OnProgressCanceled;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())            // cutting Counter is Empty 
        {
            if (!player.HasKitchenObject())  // player Not Carrying Anything 
            {
                //Transform kitchenObjectTransform = Instantiate(kithenObjectSO.Prefab);
                //kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            }
            else   // Player Carrying Somthing
            {
                if(HasKithchenObjectForInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.ProgressAmountMax
                    });
                }
              
            }

        }
        else // ClearCounter Have KitchenObject
        {
            if (!player.HasKitchenObject())
            {
                OnProgressCanceled?.Invoke(this, EventArgs.Empty);
                GetKitchenObject().SetKitchenObjectParent(player); // Give It To The Player
    
            }
            else //Player Have Plate
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))  // Try To Get Plate
                {
                    if(plateKitchenObject.TryAddIncrediantToPlate(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else // Player Have valid Object to Ad to the Plate And Counter Has Plate
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIncrediantToPlate(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }


        }
    }


    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasKithchenObjectForInput(GetKitchenObject().GetKitchenObjectSO()))            // Cutting Counter Has KitchenObject 
        {
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);   // for The Animation

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.ProgressAmountMax                // For The Progress Data
            });

            if (cuttingProgress >= cuttingRecipeSO.ProgressAmountMax)
            {
                KitchenObjectSO kitchenObjectSO = GetoutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
            }
            
            
        }
    }
    
    private bool HasKithchenObjectForInput(KitchenObjectSO kitchenObjectInput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(kitchenObjectInput);
        return cuttingRecipeSO != null;

    }

    private KitchenObjectSO GetoutputForInput(KitchenObjectSO kitchenObjectInput)
    {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(kitchenObjectInput);

        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.Output;
        }
        else
             return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO kitchenObjectSOInput)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.Input == kitchenObjectSOInput)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

  
}
