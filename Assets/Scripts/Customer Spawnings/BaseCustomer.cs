using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseCustomer : MonoBehaviour,IKitchenObjectParent,IInteractable
{
    public event EventHandler OnCustomerGetRecipe;
    public event Action<float, float> OnTimerStarted;

    private NavMeshAgent navMeshAgent;
    #region Customer States
    private enum CustomerState
    {
        WaitingOnQueueState,
        GotCounterState,
        ReturnToReturnPointState,
        ThiefState
    }
    private CustomerState state;
    #endregion

    public DeliveryCounter AttendedCounter { get; private set; }
    public RecipeSO AskedRecipeSO { get; private set; }

    #region Interaction Related Props
    [SerializeField] private Transform recipeHoldPosition;
    [SerializeField] private LayerMask counterLayerMask;
    private KitchenObject kitchenObject;
    private CustomerRecipeCallUI customerRecipeCallUI;
    #endregion


    #region Move Based Vector3
    private Vector3 currentStandingPosition;
    private Action OnTargetReached;
    private readonly float movementSpeed = 1f;
    public bool isMoving = false;
    #endregion

    #region Wainting Time
    public float WaitingTime { get; private set; } = 0;
    private readonly float waitingTimeMin = 10f;
    private float waitingTimeMax = 40f;
    #endregion

    #region Star Giving
    private int maxRating = 5;
    private int minRating = 1;
    #endregion

    private int recipePrice;

    private void Start()
    {
        WaitingTime = UnityEngine.Random.Range(waitingTimeMin, waitingTimeMax);
        waitingTimeMax = WaitingTime;
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();    
        customerRecipeCallUI = transform.GetChild(1).GetComponent<CustomerRecipeCallUI>();
    }
    private void Update()
    {
        UpdateCustomerState(state);

       
        if (isMoving)
        {
            float distance = Vector3.Distance(transform.position, currentStandingPosition);
            if (distance > 0.1f)
            {
                Vector3 direction = (currentStandingPosition - transform.position).normalized;
                transform.position += direction * movementSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                isMoving = false;
                OnTargetReached?.Invoke();
            }
        }
       
       
    }
   

    public void MoveTo(Vector3 target, Action OnTargetReached = null)
    {
        isMoving = true;
        this.OnTargetReached = OnTargetReached;
        this.currentStandingPosition = target;
    }


    private void UpdateCustomerState(CustomerState customerState)
    {
        
        switch (customerState)
        {
            case CustomerState.GotCounterState:
                
                    WaitingTime -= Time.deltaTime;

                    OnTimerStarted?.Invoke(WaitingTime, waitingTimeMax);   //For the visual Of Time Bar

                    if (WaitingTime < 0)
                    {
                        WaitingTime = UnityEngine.Random.Range(waitingTimeMin, waitingTimeMax);
                        waitingTimeMax = WaitingTime;

                        OnTimerStarted?.Invoke(WaitingTime, waitingTimeMax);
                        //Leave the Counter
                        AttendedCounter.RemoveCustomer();

                        RecipeFailedToDeliver();

                        state = CustomerState.ReturnToReturnPointState;

                        Debug.Log("Burn out called from " + gameObject.name); 
                    }
            break;
            case CustomerState.ReturnToReturnPointState:

            break;
            case CustomerState.ThiefState:
               
            break;

        }
    }

    private void CheckInteraction(Collision _)
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, .7f/*,counterLayerMask*/);

        foreach(Collider customerCollider in collider)
        {
            if(customerCollider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                //isMoving = false;
                interactable.OnCustomerInteract(this as Customer);
                break;
            }
         
        }
      
    }
    
    public void TakeDelivery(DeliveryCounter attendedDeliveryCounter)
    {
        OnCustomerGetRecipe?.Invoke(this,EventArgs.Empty); // For The Anim

        GameObject gameObject = attendedDeliveryCounter.GetKitchenObject().gameObject;
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale,new Vector3(0.6f, 0.6f, 0.6f),.1f);
        attendedDeliveryCounter.GetKitchenObject().SetKitchenObjectParent(this);
        // Give Money
        ResourceManager.Instance.AddResources(recipePrice, ValidateStarRatingByTime());
        StartCoroutine(SayThanks());
    }

    private int ValidateStarRatingByTime()
    {
        float ratingPercentage = WaitingTime / waitingTimeMax;
        int ratingAmount = Mathf.RoundToInt(Mathf.Lerp(minRating,maxRating,ratingPercentage));
        Debug.Log("Rating " + " " +  ratingAmount); 
        return ratingAmount;
    }
    public void DelayWithMoveTo(Vector3 target,Action OnTargetReached = null,float timeToMoveDelay = 0)
    {
        StartCoroutine(StartTime(target,OnTargetReached,timeToMoveDelay));
    }

    private IEnumerator StartTime(Vector3 target, Action OnTargetReached = null, float timeDelay = 0)
    {
        yield return new WaitForSeconds(timeDelay);
        MoveTo(target,OnTargetReached);
    }

    IEnumerator SayThanks()
    {
        // Call Thanks UI
        customerRecipeCallUI.EnableThanksUI();
        yield return null;

    }
    public void SetAttendedCounter(DeliveryCounter deliveryCounter)
    {
        this.AttendedCounter = deliveryCounter;
    }
    public void ShowRecipeAskingUI()
    {
        transform.GetChild(1).gameObject.SetActive(true);  // which is UI Pop Up Order

        SetRecipe(DeliveryManager.Instance.GetRandomRecipeSO(out int recipePrice));
        this.recipePrice = recipePrice;                    // Set Price For the Recipe

        state = CustomerState.GotCounterState;             // So the Waiting Timer Starts
    }

    private void SetRecipe(RecipeSO recipeSO)
    {
        if (recipeSO == null)
        {
            Debug.LogError("There is No Recipe On ShowRecipeAskingUI");
        }
        else
        {
            AskedRecipeSO = recipeSO;
            Debug.Log(AskedRecipeSO.RecipeName);
        }
        
    }

    public void RecipeFailedToDeliver()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        TakeDecisionThiefOrLeave();
    }

    private void TakeDecisionThiefOrLeave()
    {
        Debug.Log("Take desition Called by" + gameObject.name);
        NavMeshManager.Instance.SetAgent(navMeshAgent, CustomerReturnSystem.Instance.GetRandomTheifReturnPoint().position, () =>
        {
            state = CustomerState.ThiefState;
            Debug.Log("Thief State");
        });

        //MoveTo(CustomerReturnSystem.Instance.GetRandomTheifReturnPoint().position, () =>
        //{
        //    //Destroy(gameObject);  //Return To Pool
        //    //ObjectPoolManager.Instance.ReturnCustomerGameObjectToPool(this.gameObject);

        //});

    }

    public void StopMove()
    {
        StopAllCoroutines();
        isMoving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckInteraction(collision);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return recipeHoldPosition;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject; 
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;  
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public void OnCustomerInteract(Customer customer)
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("IsHit");

    }
 
    public void OnCustomerInteractToCounter(CustomerInteractor customer)
    {
        
    }
}
