using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    [SerializeField] private Transform container;
    [SerializeField] private Transform template;



    //private void Awake()
    //{
    //    template.gameObject.SetActive(false);
    //}
    //private void Start()
    //{
    //    DeliveryManager.Instance.OnRecipeSpawned += Delivery_Manager_OnRecipeSpawned;
    //    DeliveryManager.Instance.OnRecipeSpawnComplete += Delivery_Manager_OnRecipeSpawnComplete;

    //    UpdateVisual();
    //}

    //private void Delivery_Manager_OnRecipeSpawnComplete(object sender, System.EventArgs e)
    //{
    //    UpdateVisual();
    //}

    //private void Delivery_Manager_OnRecipeSpawned(object sender, System.EventArgs e)
    //{
    //    UpdateVisual();
    //}

    //private void UpdateVisual()
    //{
    //    foreach(Transform child in container)  //Destroying container's Childs
    //    {
    //        if(child == template) continue;    // Clean up Templates Before Maded.
    //        Destroy(child.gameObject);
    //    }

    //    foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetRecipeSOs())
    //    {
    //        Transform templateTransform = Instantiate(template, container);
    //        templateTransform.gameObject.SetActive(true);
    //        //templateTransform.GetComponent<DeliveryManagerSingleUI>().SetTRecipe(recipeSO);
    //        templateTransform.TryGetComponent(out DeliveryManagerSingleUI deliveryManager);
    //        deliveryManager.SetTRecipe(recipeSO);
    //    }

    //}
}
