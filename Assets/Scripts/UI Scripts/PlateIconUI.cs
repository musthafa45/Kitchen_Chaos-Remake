using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform iconTemplate;

    private void OnEnable()
    {
        plateKitchenObject.OnIncrediantAdded += PlateKitchenObject_OnIncrediantAdded;
        plateKitchenObject.OnIncrediantAddedFromGhostPlate += PlateKitchenObject_OnIncrediantAddedFromGhostPlate;

        iconTemplate.gameObject.SetActive(false);
        
    }

    private void PlateKitchenObject_OnIncrediantAddedFromGhostPlate(object sender, PlateKitchenObject.OnIncrediantAddedFromGhostPlateEventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;       //Delete Before Icon Except iconTemplate
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in e.kitchenObjectSOs)
        {
            Debug.Log(""+ e.kitchenObjectSOs.Count);
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<IconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

    //private void Awake()
    //{
    //    iconTemplate.gameObject.SetActive(false);
    //}
    private void PlateKitchenObject_OnIncrediantAdded(object sender, PlateKitchenObject.OnIncrediantAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if(child == iconTemplate) continue;       //Delete Before Icon Except iconTemplate
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectsList())
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<IconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
