using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnContainerCounterInteracted;
    [SerializeField] private KitchenObjectSO kithenObjectSO;

    public override void Interact(Player player)
    {

        if (!player.HasKitchenObject())
        {
            Transform kitchenObjectTransform = Instantiate(kithenObjectSO.Prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnContainerCounterInteracted?.Invoke(this, EventArgs.Empty);
        }


    }
}
