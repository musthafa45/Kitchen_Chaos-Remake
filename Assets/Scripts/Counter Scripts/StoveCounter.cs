using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler OnProgressCanceled;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float fryingTimer;
    private float burningTimer;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    private State state;
    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                  
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        ProgressNormalized = fryingTimer / fryingRecipeSO.ProgressTimeMax
                    });

                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > fryingRecipeSO.ProgressTimeMax)
                    {
                        fryingTimer = 0;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.Output, this);

                        burningRecipeSO = GetBurningRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                        state = State.Fried;
                    }
                    break;
                case State.Fried:
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        ProgressNormalized = burningTimer / burningRecipeSO.BurningTimeMax
                    });

                    burningTimer += Time.deltaTime;
                    if (burningTimer > burningRecipeSO.BurningTimeMax)
                    {
                        burningTimer = 0;

                        burningRecipeSO = GetBurningRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.Output, this);

                        state = State.Burned;
                    }
                    break;
                case State.Burned:
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        ProgressNormalized = 0
                    }) ;
                    break;

            }
           
        }
    }
    public override void Interact(Player player)
    {
        
        if (!HasKitchenObject())            // cutting Counter is Empty 
        {
            if (!player.HasKitchenObject())  // player Not Carrying Anything 
            {
               
            }
            else   // Player Carrying Somthing
            {
                if (HasKithchenObjectForInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    state = State.Frying;
                    fryingTimer = 0;
                }

            }

        }
        else // Counter Have KitchenObject
        {
            if (!player.HasKitchenObject())
            {
                //OnCutCanceled?.Invoke(this, EventArgs.Empty);
               
                GetKitchenObject().SetKitchenObjectParent(player); // Give It To The Player

                state = State.Idle;
                fryingTimer = 0;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressCanceled?.Invoke(this, EventArgs.Empty);

            }
            else // Player Have Plate 
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))  // Try To Get Plate
                {
                    if (plateKitchenObject.TryAddIncrediantToPlate(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        fryingTimer = 0;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressCanceled?.Invoke(this, EventArgs.Empty);
                    }
                }
                else // Player Have valid Object to Ad to the Plate And Conter Has Plate
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if(plateKitchenObject.TryAddIncrediantToPlate(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }

                    }
                }
            }


        }
    }

    private bool HasKithchenObjectForInput(KitchenObjectSO kitchenObjectInput)
    {
        fryingRecipeSO = GetFryingRecipeSOForInput(kitchenObjectInput);
        return fryingRecipeSO != null;

    }

    private KitchenObjectSO GetoutputForInput(KitchenObjectSO kitchenObjectInput)
    {

        fryingRecipeSO = GetFryingRecipeSOForInput(kitchenObjectInput);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.Output;
        }
        else
            return null;
    }

    private FryingRecipeSO GetFryingRecipeSOForInput(KitchenObjectSO kitchenObjectSOInput)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.Input == kitchenObjectSOInput)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOForInput(KitchenObjectSO kitchenObjectSOInput)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.Input == kitchenObjectSOInput)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
