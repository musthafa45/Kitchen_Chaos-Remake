using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRecipeCallSingleUI : MonoBehaviour
{
    [SerializeField] Transform containerIcon;
    [SerializeField] Transform templateImage;


    private void Awake()
    {
        
        templateImage.gameObject.SetActive(false);
    }
    public void SetTRecipe(RecipeSO recipeSO)
    {
       
        foreach (Transform child in containerIcon)
        {
            if (child == templateImage) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjects)
        {
            Transform templateTransform = Instantiate(templateImage, containerIcon);
            templateTransform.gameObject.SetActive(true);
            templateTransform.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
        }
    }
}
