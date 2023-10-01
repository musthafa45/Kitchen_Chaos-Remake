using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingBarCanvas : MonoBehaviour
{
    [SerializeField] private Image fillAreaImage;
    [SerializeField] private BaseCustomer parentCustomer;



    private void Start()
    {
        if (parentCustomer.AttendedCounter == null) Debug.LogError("No Interact Counter");

        parentCustomer.OnTimerStarted += AttendedCounter_OnTimerStarted;

    }

    private void AttendedCounter_OnTimerStarted(float _waitingTime, float _waitingTimeMax)
    {
        fillAreaImage.fillAmount = _waitingTime / _waitingTimeMax;
    }


    private void OnDisable()
    {
        parentCustomer.OnTimerStarted -= AttendedCounter_OnTimerStarted;
    }

   
}
