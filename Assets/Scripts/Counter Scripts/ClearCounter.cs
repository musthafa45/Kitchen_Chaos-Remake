using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    private enum CounterState
    {
        TableMode,
        GhostTableMode
    }

    private CounterState counterState;

    [SerializeField] private KitchenObjectSO ghostPlateKitchenObject;

    [SerializeField] private List<KitchenObjectSO> validProccesedKitchenObjects;

    [SerializeField] private ProccessedKitchenObjectSOList proccessedKitchenObjectSOList;
    private void Start()
    {
        counterState = CounterState.TableMode;
    }
  

    private void InitializeTable(Player player)
    {
 
        if(player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                counterState = CounterState.GhostTableMode;
                return;
            }

            if (proccessedKitchenObjectSOList.kitchenObjectSO.Contains(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                counterState = CounterState.GhostTableMode;
            }
            else
            {
                counterState = CounterState.TableMode;
            }
        }
        else 
        {
            counterState = CounterState.TableMode; 
        }
       
        
    }
    public override void Interact(Player player) {

        InitializeTable(player);

        if (counterState == CounterState.TableMode)
        {
            // Behaviour For Table Mode
            if (!HasKitchenObject())            // clear Counter is Empty 
            {
                if (!player.HasKitchenObject())  // player Not Carrying Anything 
                {

                }
                else                            // Player Carrying Somthing Basic Incrediant.
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }

            }
            else // Counter Has Kitchen Object Basic
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player); // Give It To The Player
                }
                else // Player Have SomeThing In Hand.
                {
                   if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))// Player Holding a Plate
                   {

                   }
                }
            }

        }
        else
        {
            // behaviour For GhostTableMode
       
            if(!HasKitchenObject())
            {
              KitchenObject.SpawnKitchenObject(ghostPlateKitchenObject, this); // create Ghost Plate.
            }
            
            GetGhostPlate(out bool HasGhostPlate, out GhostPlateKitchenObject _ghostPlateKitchenObject);


            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //bool kitchenObjectSOs = GetKitchenObject().TryGetGhostPlate(out GhostPlateKitchenObject ghostPlateKitchenObject);
                bool hasvalidObject = false;

                foreach (KitchenObjectSO kitchenObjectSO in _ghostPlateKitchenObject.GetKitchenObjectsList())
                {
                    if (validProccesedKitchenObjects.Contains(kitchenObjectSO))
                    {
                        hasvalidObject = true;
                    }
                    
                }
                if (HasGhostPlate && hasvalidObject)
                {
                    if (plateKitchenObject.SetGhostKitchenObjToPlate(_ghostPlateKitchenObject))
                    {
                        _ghostPlateKitchenObject.GetKitchenObjectsList().RemoveRange(0, _ghostPlateKitchenObject.GetKitchenObjectsList().Count);

                        _ghostPlateKitchenObject.GotTheKitchenObjectFromGhostPlate();

                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    // Player Has Non Valid Obj counter has empty 
                    if (GetKitchenObject().TryGetGhostPlate(out _))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                   
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                }
            }
            else
            {
                //Counter Has GhostPlate And Player Have Plate or Somthing
          
                if (HasGhostPlate)
                {
                    if(_ghostPlateKitchenObject.TryAddIncrediantToGhostPlate(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if(plateKitchenObject.TryAddIncrediantToPlate(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
 
                    }
                }


            }

        }


        //if (!HasKitchenObject())            // clear Counter is Empty 
        //{
        //    if(!player.HasKitchenObject())  // player Not Carrying Anything 
        //    {

        //    }
        //    else                            // Player Carrying Somthing Basic Incrediant.
        //    {        
        //        player.GetKitchenObject().SetKitchenObjectParent(this);    
        //    }

        //}
        //else // ClearCounter Have KitchenObject
        //{
        //    if(!player.HasKitchenObject())
        //    {
        //       GetKitchenObject().SetKitchenObjectParent(player); // Give It To The Player
        //       KitchenObject.SpawnKitchenObject(ghostPlateKitchenObject, this);
        //    }
        //    else                                                   //Player  AlreadyHave Somthing
        //    { // Check Player Holding What 

        //        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))// Player Holding a Plate
        //        {
        //            //if (plateKitchenObject.TryAddIncrediantToPlate(GetKitchenObject().GetKitchenObjectSO()))
        //            //{
        //            //    GetKitchenObject().DestroySelf();
        //            //}
        //            bool kitchenObjectSOs = GetKitchenObject().TryGetGhostPlate(out GhostPlateKitchenObject ghostPlateKitchenObject);
        //            bool hasvalidObject = false;
        //            Debug.Log("From GhostPlate KitchenObjects"+ ghostPlateKitchenObject.GetKitchenObjectsList().Count);
        //            foreach(KitchenObjectSO kitchenObjectSO in ghostPlateKitchenObject.GetKitchenObjectsList())
        //            {
        //                if (validProccesedKitchenObjects.Contains(kitchenObjectSO))
        //                {
        //                    hasvalidObject = true;  
        //                } 
        //            }
        //            if(kitchenObjectSOs && hasvalidObject)
        //            {
        //                if(plateKitchenObject.SetGhostKitchenObjToPlate(ghostPlateKitchenObject))
        //                {
        //                    ghostPlateKitchenObject.GetKitchenObjectsList().RemoveRange(0,ghostPlateKitchenObject.GetKitchenObjectsList().Count);

        //                    ghostPlateKitchenObject.GotTheKitchenObjectFromGhostPlate();
        //                }

        //            }
        //            else // Has Non Valid Obj counter has empty 
        //            {
        //                GetKitchenObject().DestroySelf();
        //                player.GetKitchenObject().SetKitchenObjectParent(this);
        //            }

        //        }
        //        else // Player Have valid Object to Ad to the Plate And Conter Has Plate
        //        {
        //            if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
        //            {
        //                if (plateKitchenObject.TryAddIncrediantToPlate(player.GetKitchenObject().GetKitchenObjectSO()))
        //                {
        //                    player.GetKitchenObject().DestroySelf();
        //                }

        //            }
        //            else
        //            {

        //                // Player Have valid Object to Ad to the Plate OR GhostPlate And Conter Has GhostPlate
        //                if (GetKitchenObject().TryGetGhostPlate(out GhostPlateKitchenObject ghostPlateKitchenObjectPlate))
        //                {
        //                    if (validProccesedKitchenObjects.Contains(player.GetKitchenObject().GetKitchenObjectSO()))
        //                    {
        //                        if(!addedKitchenObjects.Contains(player.GetKitchenObject().GetKitchenObjectSO()))
        //                        {
        //                            ghostPlateKitchenObjectPlate.TryAddIncrediantToGhostPlate(player.GetKitchenObject().GetKitchenObjectSO());
        //                            player.GetKitchenObject().DestroySelf();
        //                        }

        //                    }
        //                    else
        //                    {
        //                        // Counter Has GhostPlate And Player Have Plate or Somthing
        //                        //if (ghostPlateKitchenObjectPlate.TryAddIncrediantToGhostPlate(player.GetKitchenObject().GetKitchenObjectSO()))
        //                        //ghostPlateKitchenObjectPlate.SetCounter(this);
        //                        //OnNonValidObjectPlaced?.Invoke(this, EventArgs.Empty);
        //                        GetKitchenObject().DestroySelf();
        //                        player.GetKitchenObject().SetKitchenObjectParent(this);

        //                    }
        //                }

        //            }     


        //        }
        //    }


        //}


    }


    private void GetGhostPlate(out bool HasGhostPlate,out GhostPlateKitchenObject ghostPlateKitchenObject)
    {
        if(GetKitchenObject().TryGetGhostPlate(out GhostPlateKitchenObject _ghostPlateKitchenObject))
        {
            ghostPlateKitchenObject = _ghostPlateKitchenObject;
            HasGhostPlate = true;
        }
        else 
        {
            ghostPlateKitchenObject = null;
            HasGhostPlate = false;
        }

    }
   
}
