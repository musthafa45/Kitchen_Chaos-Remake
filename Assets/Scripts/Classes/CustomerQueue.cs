using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class CustomerQueue 
{
    private List<Vector3> customerWaitingPosList;
    private List<BaseCustomer> customerList;
    private Vector3 enterPos;
    public CustomerQueue(List<Vector3> waitingPositionList,List<BaseCustomer> waitingCustomersList = null)
    {
        this.customerWaitingPosList = waitingPositionList;
        this.customerList = waitingCustomersList;

        enterPos = waitingPositionList[waitingPositionList.Count - 1] + new Vector3(0, 0, 1f); 
        foreach(Vector3 position in waitingPositionList)
        {
            World_Sprite.Create(position, new Vector3(.3f, .3f, .3f),Color.green);
        }
        World_Sprite.Create(enterPos, new Vector3(.3f, .3f, .3f), Color.magenta);

        customerList = new List<BaseCustomer>();
       
    }
    public List<BaseCustomer> GetCustomerList()
    {
        return customerList;
    }

    public bool CanAddCustomer()
    {
        return customerList.Count < customerWaitingPosList.Count;
    }
    
    public void AddCustomer(BaseCustomer customer)
    {
        customerList.Add(customer);
        int index = customerList.IndexOf(customer);
        Vector3 targetPos = customerWaitingPosList[index];
        customer.MoveTo(enterPos,() =>
        {
            customer.MoveTo(targetPos);
        });
    }

    public BaseCustomer GetFirstCustomerInQueue()
    {
        if(customerList.Count == 0)
        {
            return null;
        }
        else
        {
            BaseCustomer customer = customerList[0];
            customerList.RemoveAt(0);
            //RePositionAllCustomers();
            return customer;
        }
    }

    public void RePositionAllCustomers()
    {
         // Give A time For Front Customer move
        for (int i = 0; i < customerList.Count; i++)
        {
            customerList[i].MoveTo(customerWaitingPosList[i]);
        }

    }
}
