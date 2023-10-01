using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnCustomerInteract(Customer customer);
    void OnCustomerInteractToCounter(CustomerInteractor customer);
}
