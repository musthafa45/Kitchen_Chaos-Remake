using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResouceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;


    private void Start()
    {
        UpdateVisual();
        ResourceManager.Instance.OnResourceUpdate += Instance_OnResourceUpdate;
    }
    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceUpdate -= Instance_OnResourceUpdate;
    }
    private void UpdateVisual()
    {
        coinText.text = ResourceManager.Instance.Coins.ToString("D");
    }
    private void Instance_OnResourceUpdate()
    {
        UpdateVisual();
    }
}
