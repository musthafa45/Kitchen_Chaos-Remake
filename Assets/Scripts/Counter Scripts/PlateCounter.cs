using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField]private KitchenObjectSO plateKitchenObjectSO;
    private float plateSpawnTimer;
    private float plateSpawnTimerMax = 4;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax=4;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;
        if(plateSpawnTimer > plateSpawnTimerMax) //Spawn Plates
        {
            plateSpawnTimer = 0;

            if(plateSpawnAmount < plateSpawnAmountMax)
            {
                plateSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(plateSpawnAmount > 0)
            {
                plateSpawnTimer = 0;
                plateSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
            
            //GetKitchenObject().SetKitchenObjectParent(player);
        }
    }
}
