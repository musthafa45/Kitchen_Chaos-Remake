using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractor : MonoBehaviour
{
    private BaseCustomer _baseCustomer;

    private void Start()
    {
        _baseCustomer = GetComponentInParent<BaseCustomer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out DeliveryCounter deliveryCounter))
        {
            deliveryCounter.OnCustomerInteractToCounter(this);
        }
    }

    public BaseCustomer GetCustomer()
    {
        if (_baseCustomer == null) Debug.LogError("There is No _baseCounter On InteractorScript");
        return _baseCustomer;
    }
}
