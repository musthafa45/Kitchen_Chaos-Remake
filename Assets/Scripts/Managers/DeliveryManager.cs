using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; } 

    //public event EventHandler OnRecipeSpawned;
    //public event EventHandler OnRecipeSpawnComplete;

    [SerializeField]private RecipeSOListSO recipeSOListSO;


    //private float recipeSpawnTime;
    //private float recipeSpawnTimeMax = 4;
    //private int recipeSpawnCountMax = 3;

    //private List<RecipeSO> waitingRecipeSOList;
    private List<RecipeSO> customerOrderedRecipes;


    private void Awake()
    {
        if(Instance != null)
        {
           Debug.LogError("There Is One More Delivery Manager On The Hierarchy");
        }
        Instance = this;
        
        //waitingRecipeSOList = new List<RecipeSO>();
        customerOrderedRecipes = new List<RecipeSO>();
    }
    private void Update()
    {
        //recipeSpawnTime -= Time.deltaTime;
        //if(recipeSpawnTime <= 0)
        //{
        //    recipeSpawnTime = recipeSpawnTimeMax;

        //    if(waitingRecipeSOList.Count < recipeSpawnCountMax) 
        //    {
        //        SpawnRecipe();
        //    }
        //}

    }

    //private void SpawnRecipe()
    //{
    //    RecipeSO waitingRecipeSO = recipeSOListSO.recipeSO[UnityEngine.Random.Range(0, recipeSOListSO.recipeSO.Count)];
    //    Debug.Log(waitingRecipeSO.RecipeName);
    //    waitingRecipeSOList.Add(waitingRecipeSO);

    //    OnRecipeSpawned?.Invoke(this,EventArgs.Empty); 
    //}

    //public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    //{
    //    for(int i = 0; i < waitingRecipeSOList.Count; i++)
    //    {
    //        // Cycle Through All Waiting Recipeies
    //        RecipeSO waitingRecipe = waitingRecipeSOList[i];
    //        if(waitingRecipe.KitchenObjects.Count == plateKitchenObject.GetKitchenObjectsList().Count)
    //        {
    //            bool plateContentMatched = true;
    //            // Plate has Same Number of Incrediants
    //            foreach(KitchenObjectSO kitchenObjectSO in waitingRecipe.KitchenObjects)
    //            {
    //                bool incrediantFound = false;
    //                foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectsList())
    //                {
    //                    if(kitchenObjectSO == plateKitchenObjectSO)
    //                    {
    //                        //incrediant Found
    //                        incrediantFound = true; 
    //                        break;
    //                    }
    //                }

    //                if(!incrediantFound)
    //                {
    //                    plateContentMatched = false;
    //                    Debug.Log("Incrediant Not Found");
    //                }
    //            }

    //            if(plateContentMatched)
    //            {
    //                Debug.Log("Correct Recipe");
    //                waitingRecipeSOList.RemoveAt(i);

    //                OnRecipeSpawnComplete?.Invoke(this, EventArgs.Empty);
    //                return;
    //            }
    //        }
    //    }


    //    Debug.Log("Wrong Recipe");
    //}

    public RecipeSO GetRandomRecipeSO(out int priceForTheRecipe)            // For Individual Customer Mode 
    {
        RecipeSO waitingRecipeSO = recipeSOListSO.recipeSO[UnityEngine.Random.Range(0, recipeSOListSO.recipeSO.Count)];
        Debug.Log(waitingRecipeSO.RecipeName);

        //int recipeIndex = UnityEngine.Random.Range(0, waitingRecipeSOList.Count);
        //RecipeSO selectedRecipe = waitingRecipeSOList[recipeIndex];

        customerOrderedRecipes.Add(waitingRecipeSO);
        //waitingRecipeSOList.Remove(waitingRecipeSO);
        priceForTheRecipe = waitingRecipeSO.KitchenObjects.Count * 10;

        return waitingRecipeSO;
    }

  

    //public List<RecipeSO> GetRecipeSOs()
    //{
    //    return waitingRecipeSOList;
    //}
}
