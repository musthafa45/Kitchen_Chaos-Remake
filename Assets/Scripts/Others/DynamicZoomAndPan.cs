using System;
using System.Collections;
using UnityEngine;

public class DynamicZoomAndPan : MonoBehaviour
{

    #region CameraZoom
    public float camZoomSpeed = 1f;
    private Coroutine coroutine;
    private readonly float minZoom = 12;
    private readonly float maxZoom = 5;
    #endregion

    [SerializeField]
    [Range(0.01f, 2)] private float panSpeed;
    private bool isPanning = false;
    private bool isZooming = false;

    private PlayerInputAction playerInputAction;
    private Transform camTransform;

 

    private void Awake()
    {
        camTransform = Camera.main.transform;
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
       playerInputAction.Enable();
    }
    private void OnDisable()
    {
        playerInputAction.Disable();
    }
    private void Start()
    {
        playerInputAction.Touch.SecondaryTouchContact.started += _ => StartZoom();
        playerInputAction.Touch.SecondaryTouchContact.canceled += _ => EndZoom();

    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (isZooming) return;
            isPanning = true;
            Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
            //transform.Translate(-touchDeltaPos.x * panSpeed * Time.deltaTime, -touchDeltaPos.y * panSpeed * Time.deltaTime, 0);
            //transform.position += new Vector3(-touchDeltaPos.x * panSpeed * Time.deltaTime, 0 , -touchDeltaPos.y * panSpeed * Time.deltaTime);
            float touchDeltaX = touchDeltaPos.x * panSpeed * Time.deltaTime;
            float touchDeltaY = touchDeltaPos.y * panSpeed * Time.deltaTime;

            Vector3 targetPos = transform.position + new Vector3(-touchDeltaX, 0, -touchDeltaY);

            targetPos.x = Mathf.Clamp(targetPos.x, -5.12f, 8f); // Adding Limits Min Max
            targetPos.z = Mathf.Clamp(targetPos.z, -10f, -3f);

            float smoothness = 10f;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);

        }
        else
        {
            isPanning = false;
        }
    }

    private void StartZoom()
    {
        if(isPanning) return;
        coroutine = StartCoroutine(ZoomDetection());
        isZooming = true;
    }
    private void EndZoom()
    {
        StopCoroutine(coroutine);
        isZooming=false;
    }
    private IEnumerator ZoomDetection()
    {
       
        float previousDis = 0;
        while (true)
        {
            float currentDis = Vector2.Distance(playerInputAction.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                                                playerInputAction.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            if (currentDis > previousDis)     //Zoom In
            {
                Vector3 targetPos = camTransform.position;
                targetPos.y -= 1f;
                if(targetPos.y > maxZoom)
                {
                   camTransform.position = Vector3.Slerp(camTransform.position, targetPos, Time.deltaTime * camZoomSpeed);
                }
            }
            else if(currentDis < previousDis) // Zoom Out 
            {
                Vector3 targetPos = camTransform.position;
                targetPos.y += 1f;
                if(targetPos.y < minZoom)
                {
                   camTransform.position = Vector3.Slerp(camTransform.position, targetPos, Time.deltaTime * camZoomSpeed);
                }
            }

            previousDis = currentDis;
            yield return null;
        }
    }


}
