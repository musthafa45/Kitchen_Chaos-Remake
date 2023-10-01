using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    #region Events
    public event EventHandler OnDoubleTapOnCounter;
    public event EventHandler<OnPlayerMovingToTargetEventArgs> OnPlayerMovingToTarget;
    public class OnPlayerMovingToTargetEventArgs
    {
        public bool IsWalking;
    }
    #endregion

    public static TouchManager Instance { get; private set; }

    
    private PlayerInput playerInput;
    public InputAction touchPressInputAction;
    private InputAction touchPositionInputAction;
    private InputAction multiTouchInputAction;

    [SerializeField] private ClickToMove player;
    [SerializeField] private DynamicZoomAndPan zoomAndPan;
    [SerializeField] private Transform interactButtonTranform;
    [SerializeField] private Transform altrInteractButtonTranform;

    [SerializeField] private PlayerInteractionUI playerInteractionUIScript;
    [SerializeField] private PlayerAlterInteractionUI playerAlterInteractionUIScript;


    private readonly float buttonCircleRadiusForInteractionBtn = 85;
    private readonly float buttonCircleRadiusForAlterInteractionBtn = 75;

    private Coroutine coroutine;
    public bool IsMoving;
    private Transform baseCounterTransform;

   //[SerializeField] private bool isUsingButtonInteract;
   //[SerializeField] private bool isUsingTapInteraction;
    public enum InteractionMode
    {
        ButtonInteract,
        TapInteraction
    }

    public InteractionMode interactionMode;

    #region DoubleTapDetection
    //private float lastTapTime;
    private Vector2 lastTapPosition;

    //[SerializeField] private float lastTapTimeThreshold = 0.5f;
    //[SerializeField] private float lastTapDistanceThreshold = 50f;
    #endregion
    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        touchPositionInputAction = playerInput.actions["TouchPosition"];
        touchPressInputAction = playerInput.actions["TouchPress"]; // for interactions / player moves
        multiTouchInputAction = playerInput.actions["DoubleTap"];
    }

    private void Start()
    {
        interactionMode = InteractionMode.TapInteraction;   
    }
    private void OnEnable()
    {
        touchPositionInputAction.Enable();
        touchPressInputAction.Enable(); 
        multiTouchInputAction.Enable();

        touchPressInputAction.performed += TouchPressed;
        touchPositionInputAction.performed += TouchPositionInputAction_performed;
        multiTouchInputAction.performed += MultiTouchInputAction_performed;
        
    }
    private void OnDisable()
    {
        touchPressInputAction.performed -= TouchPressed;
        touchPositionInputAction.performed -= TouchPositionInputAction_performed;
        multiTouchInputAction.performed -= MultiTouchInputAction_performed;

        touchPositionInputAction.Disable();
        touchPressInputAction.Disable();
        multiTouchInputAction.Disable();
    }
    private void MultiTouchInputAction_performed(InputAction.CallbackContext context)
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if(interactionMode == InteractionMode.ButtonInteract) return;

        Ray ray = Camera.main.ScreenPointToRay(lastTapPosition);

        Vector3 alterInteractButtonPos = new(altrInteractButtonTranform.position.x, altrInteractButtonTranform.position.y);
        float distanceAltrInteractBtnToTouchPos = Vector3.Distance(lastTapPosition, alterInteractButtonPos);


        if (distanceAltrInteractBtnToTouchPos < buttonCircleRadiusForInteractionBtn)
        {
            Debug.Log("Alter Interaction Clicked");
        }
        else if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.TryGetComponent(out BaseCounter baseCounter))
            {
                OnDoubleTapOnCounter?.Invoke(this, EventArgs.Empty);
            }
        }


    }


    private void TouchPressed(InputAction.CallbackContext context)
    {
        lastTapPosition = Touchscreen.current.primaryTouch.position.ReadValue();
      

        if(interactionMode == InteractionMode.ButtonInteract)  // Player Choosing (Button) Interaction.
        {
            Vector3 interactButtonPos = new(interactButtonTranform.position.x, interactButtonTranform.position.y);
            Vector3 alterInteractButtonPos = new(altrInteractButtonTranform.position.x, altrInteractButtonTranform.position.y);
            float distanceInteractBtnToTouchPos = Vector3.Distance(lastTapPosition, interactButtonPos);
            float distanceAltrInteractBtnToTouchPos = Vector3.Distance(lastTapPosition, alterInteractButtonPos);
            if (distanceInteractBtnToTouchPos < buttonCircleRadiusForInteractionBtn)
            {
                if (playerInteractionUIScript.GetButtonStatus())                 // Check For The Interaction Button postion
                {
                    // Let The Button Do His Job.
                    return;
                }
                else
                {
                    StartRayToWorldPositionForMovement(lastTapPosition);
                }

            }
            else if (distanceAltrInteractBtnToTouchPos < buttonCircleRadiusForAlterInteractionBtn)
            {
                if (playerAlterInteractionUIScript.GetButtonStatus())                 // Check For The AlterInteraction Button postion
                {
                    // Let The Button Do His Job.
                    return;
                }
                else
                {
                    StartRayToWorldPositionForMovement(lastTapPosition);
                }
            }
            else
            {
                StartRayToWorldPositionForMovement(lastTapPosition);
            }
        }
        else if(interactionMode == InteractionMode.TapInteraction)     // Player Choosing (Tap) Interaction.
        {
            Vector3 alterInteractButtonPos = new(altrInteractButtonTranform.position.x, altrInteractButtonTranform.position.y);
            float distanceAltrInteractBtnToTouchPos = Vector3.Distance(lastTapPosition, alterInteractButtonPos);
            if (distanceAltrInteractBtnToTouchPos < buttonCircleRadiusForAlterInteractionBtn)
            {
                if (playerAlterInteractionUIScript.GetButtonStatus())                 // Check For The Interaction Button postion
                {
                    // Let The Button Do His Job.
                    return;
                }
                else
                {
                    StartRayToWorldPositionForMovement(lastTapPosition);
                }
            }
            else
            {
                StartRayToWorldPositionForMovement(lastTapPosition);
            }


        }
       

    }

  
    private void StartRayToWorldPositionForMovement(Vector2 lastTouchPos) 
    {
        Ray ray = Camera.main.ScreenPointToRay(lastTouchPos);

        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) /*&&*/
           //hit.collider.gameObject.layer.CompareTo(layerMaskIndexFloor) == 0 &&
           /*!hit.collider.TryGetComponent(out BaseCounter _)*/)
        {

            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
        }
        if (hit.collider != null) // Has Hitted On Counters
        {
            if (hit.collider.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounterTransform = baseCounter.GetComponent<BaseCounter>().GetPlayerStandPos();
                if(coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(PlayerMoveTowardsToCounter(baseCounterTransform.transform.position));
            }

        }
    }
    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        bool isWalking;
        float floorToPlayerOffset = player.transform.position.y - target.y;
        target.y += floorToPlayerOffset;

        while (Vector3.Distance(Player.Instance.transform.position, target) > .3f)
        {
            isWalking = true;
            OnPlayerMovingToTarget?.Invoke(this, new OnPlayerMovingToTargetEventArgs
            {
                IsWalking = isWalking
            });

            player.MovePlayer(target);

            yield return null;
        }
        // Player Reached His Target
        isWalking = false;

        OnPlayerMovingToTarget?.Invoke(this, new OnPlayerMovingToTargetEventArgs
        {
            IsWalking = isWalking
        });
    }

    private IEnumerator PlayerMoveTowardsToCounter(Vector3 target)
    {
        bool isWalking;
        float playerToCounterOffset = player.transform.position.y - target.y;
        target.y += playerToCounterOffset;
        Vector3 direction = target - player.transform.position;

        while (Vector3.Distance(player.transform.position, target) > 0.25f)
        {
            isWalking = true;

            OnPlayerMovingToTarget?.Invoke(this, new OnPlayerMovingToTargetEventArgs
            {
                IsWalking = isWalking
            });

            player.MovePlayer(target);

            yield return null;
        }
       
        // Player Reached His Target
        isWalking = false;


        //float rotationSpeed = 10f;
        OnPlayerMovingToTarget?.Invoke(this, new OnPlayerMovingToTargetEventArgs
        {
            IsWalking = isWalking
        });


    }
    private void TouchPositionInputAction_performed(InputAction.CallbackContext context)
    {
        
    }

   
}
