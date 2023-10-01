using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter,IInteractable
{
    //public event Action<float, float> OnTimerStarted;

    private List<Vector3> waitingQueuePosList;
    private CustomerQueue customerQueue;
    [SerializeField] private Transform attendPosition;
    private readonly int maxCustomerStanPoint = 5;
    private CustomerRecipeCheck customerRecipeCheck;
    private BaseCustomer _baseCustomer;
    private bool isCounterFull;

    //private bool isCustomerAttended;

    //public float WaitingTime { get; private set; } = 0;
    //private readonly float waitingTimeMin = 60f;
    //private float waitingTimeMax = 120f;

    private readonly float timeBtwSpawnMax = 5;
    private float timeBtwSpawn;

    private void Awake()
    {
        customerRecipeCheck = GetComponent<CustomerRecipeCheck>();
    }
    private void Start()
    {
        //WaitingTime = UnityEngine.Random.Range(waitingTimeMin, waitingTimeMax);
        //waitingTimeMax = WaitingTime;

        waitingQueuePosList = new List<Vector3>();

        float positionSize = 1.8f;
        for (int i = 0; i < maxCustomerStanPoint; i++)
        {
            waitingQueuePosList.Add(attendPosition.position + new Vector3(0, 0, 1) * positionSize * i);
        }
        customerQueue = new(waitingQueuePosList);

        customerRecipeCheck.OnPlacedCorrectRecipe += GetRecipeCallToCustomer;

    }

   
    private void Update()
    {
        //if (isCustomerAttended)
        //{
        //    WaitingTime -= Time.deltaTime;
            
        //    OnTimerStarted?.Invoke(WaitingTime, waitingTimeMax);   //For the visual Of Time Bar

        //    if (WaitingTime < 0)
        //    {
        //        WaitingTime = UnityEngine.Random.Range(waitingTimeMin, waitingTimeMax);
        //        waitingTimeMax = WaitingTime;

        //        OnTimerStarted?.Invoke(WaitingTime, waitingTimeMax);
        //        //Remove First Customer
        //        RemoveCustomer();
        //        Debug.Log("Burn out called");
        //        isCustomerAttended = false;
        //    }
        //}

        if (isCounterFull) return;

        timeBtwSpawn -= Time.deltaTime;

        if(timeBtwSpawn < 0)
        {
            timeBtwSpawn = timeBtwSpawnMax;

            if(customerQueue.CanAddCustomer())
            {
                BaseCustomer customer = CustomerSpawner.Instance.GetCustomer();
                if (customer == null)
                {
                    Debug.LogError("There is NO Customer Founded");
                }
                else
                  Debug.Log("Customer Counts" + " " + customerQueue.GetCustomerList().Count);
                  customerQueue.AddCustomer(customer);
            }
            else
            {
                Debug.Log("Cannot Ad Customer");
                isCounterFull = true;
            }
               
        }
        
    }
   
    public void RemoveCustomer()
    {
         BaseCustomer customer = customerQueue.GetFirstCustomerInQueue();
         
        if(customer != null)
        {
            //customer.RecipeFailedToDeliver();
            //customer.MoveTo(CustomerReturnSystem.Instance.GetRandomReturnPoint().position, () =>
            //{
            //    //Destroy(customer.gameObject);  //Return To Pool
            //    ObjectPoolManager.Instance.ReturnCustomerGameObjectToPool(customer.gameObject);
            //});
            StartCoroutine(StartRePositioning());
            isCounterFull = false;
        }
    }

    private void GetRecipeCallToCustomer(DeliveryCounter counter)
    {
         BaseCustomer customer = customerQueue.GetFirstCustomerInQueue();

        if(customer != null)
        {
            float leaveCounterDelay = 3;
            customer.DelayWithMoveTo(CustomerReturnSystem.Instance.GetRandomReturnPoint().position, () =>
            {
              Destroy(customer.gameObject);
            },leaveCounterDelay);
            _baseCustomer.TakeDelivery(counter);
            //customerQueue.RePositionAllCustomers();
            StartCoroutine(StartRePositioning());
            isCounterFull = false;
        }
         
    }

    IEnumerator StartRePositioning()
    {
        yield return new WaitForSeconds(4);

        customerQueue.RePositionAllCustomers();
    }

    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                //player.GetKitchenObject().DestroySelf();
                player.GetKitchenObject().SetKitchenObjectParent(this);
                customerRecipeCheck.DeliverRecipe(plateKitchenObject,this);
            }
        }
    }
  
    public void OnCustomerInteract(Customer customer)
    {
       
    }

    public void OnCustomerInteractToCounter(CustomerInteractor customerInteractor)
    {
        Debug.Log("Interaction Customer With this Counter new ");
        //BaseCustomer customer = customerQueue.GetFirstCustomerInQueue();
        //_baseCustomer = customer;
        _baseCustomer = customerInteractor.GetCustomer();
        _baseCustomer.StopMove();
        _baseCustomer.ShowRecipeAskingUI();
        _baseCustomer.SetAttendedCounter(this);
        customerRecipeCheck.SetCustomerAskedRecipe(_baseCustomer);
        //isCustomerAttended = true;
    }

  
}
