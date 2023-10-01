using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public event Action OnResourceUpdate;
    public int Coins {  get; private set; }

    private List<int> starRatings;

    public void AddResources(int coinAmount = 0, int StartRating = 0 )
    {
        this.Coins += coinAmount;
        starRatings.Add(StartRating);
        
        OnResourceUpdate?.Invoke();
    }
    private void Awake()
    {
        Instance = this;
        starRatings = new List<int>();
    }
}
