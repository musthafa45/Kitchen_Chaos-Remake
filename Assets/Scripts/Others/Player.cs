using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent {
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerInteract;
    public event EventHandler OnPlayerInteractCanceled;

    public event EventHandler OnPlayerAlterInteract;
    public event EventHandler OnPlayerAlterInteractCanceled;


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerInteractionUI playerInteractionUI;
    [SerializeField] private PlayerAlterInteractionUI playerAlterInteractionUI;


    [SerializeField] private LayerMask countersLayer;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    private Vector3 lastInteractionDir;
    private bool isMoving;
    private BaseCounter selectedCounter;
    
    [SerializeField]private ProccessedKitchenObjectSOList proccessedKitchenObjectSOList;

    private void Awake()
    {

        if(Instance != null)
        {
            Debug.LogError("There Is One More Instance There");
        }
        Instance = this;

    }


    private void Start() 
    {
        gameInput.OnInteraction += GameInput_OnInteraction;
        gameInput.OnAlternateInteraction += GameInput_OnAlternateInteraction;

        playerInteractionUI.OnInteractionButtonClicked += PlayerInteractionUI_OnInteractionButtonClicked;
        playerAlterInteractionUI.OnAlterInteractionButtonClicked += PlayerAlterInteractionUI_OnAlterInteractionButtonClicked;

        TouchManager.Instance.OnDoubleTapOnCounter += TouchManager_OnDoubleTapOnCounter;

    }
    private void OnDisable()
    {
        gameInput.OnInteraction -= GameInput_OnInteraction;
        gameInput.OnAlternateInteraction -= GameInput_OnAlternateInteraction;

        playerInteractionUI.OnInteractionButtonClicked -= PlayerInteractionUI_OnInteractionButtonClicked;
        playerAlterInteractionUI.OnAlterInteractionButtonClicked -= PlayerAlterInteractionUI_OnAlterInteractionButtonClicked;

        TouchManager.Instance.OnDoubleTapOnCounter -= TouchManager_OnDoubleTapOnCounter;
    }
    private void TouchManager_OnDoubleTapOnCounter(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
   
    private void PlayerAlterInteractionUI_OnAlterInteractionButtonClicked(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void PlayerInteractionUI_OnInteractionButtonClicked(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnAlternateInteraction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }
  
    private void Update() {
        //HandleMovement();
        //HandleInteraction();
    }

    private void GameInput_OnInteraction(object sender, EventArgs e) {

        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }

    }

    public void HandleInteraction(Vector3 moveDir) {

        //Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 1.5f;
        bool rayCast = Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, countersLayer);
        if (rayCast)
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has Base Counter
                if(baseCounter.TryGetComponent(out CuttingCounter cuttingCounter))
                {
                    OnPlayerAlterInteract?.Invoke(this, EventArgs.Empty);    // Check For Cutting Counter To Enable BTN.
                }
                if (baseCounter != selectedCounter)
                {
                    OnPlayerInteract?.Invoke(this, EventArgs.Empty);
                    SetSelectedCounter(baseCounter);
                    selectedCounter.AdjustPlayerPosInteract(this);
                }
            }
            else
            {
                OnPlayerInteractCanceled?.Invoke(this, EventArgs.Empty);    
                OnPlayerAlterInteractCanceled?.Invoke(this, EventArgs.Empty);
                SetSelectedCounter(null);
            }
        }
        else
        {
            OnPlayerInteractCanceled?.Invoke(this, EventArgs.Empty);
            OnPlayerAlterInteractCanceled?.Invoke(this, EventArgs.Empty);
            SetSelectedCounter(null);
        }
    }




    public void HandleMovement(Vector3 moveDir) {

        //Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
       
        if(!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if(canMove) {
                moveDir = moveDirX;
            }
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if(canMove) {
                    moveDir = moveDirZ;
                } else {
                    // cannot move AnyWhere.
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isMoving = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        
    }
    public bool IsMoving() {
        return isMoving;
    }

    public void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
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
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public BaseCounter GetInteractCounter()
    {
        return selectedCounter; 
    }
}
