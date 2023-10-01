using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlateIconUI : MonoBehaviour
{
    [SerializeField] GhostPlateKitchenObject ghostPlateKitchenObject;
    [SerializeField] Transform iconTemplate;

    private void Start()
    {
        ghostPlateKitchenObject.OnIncrediantAddedOnGhostPlateKO += GhostPlateKitchenObject_OnIncrediantAddedOnGhostPlateKO;
        ghostPlateKitchenObject.OnKitchenObjectsSetToPlate += GhostPlateKitchenObject_OnKitchenObjectsSetToPlate;

        UpdateVisual();
    }

    private void GhostPlateKitchenObject_OnKitchenObjectsSetToPlate(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void GhostPlateKitchenObject_OnIncrediantAddedOnGhostPlateKO(object sender, GhostPlateKitchenObject.OnIncrediantAddedOnGhostPlateKOEventArgs e)
    {
        UpdateVisual();
    }

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;       //Delete Before Icon Except iconTemplate
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in ghostPlateKitchenObject.GetKitchenObjectsList())
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<IconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
