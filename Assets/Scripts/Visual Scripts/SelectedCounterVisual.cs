using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter selectedCounter;
    [SerializeField] private GameObject[] visual;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == selectedCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
        
    }

    private void Hide()
    {
        foreach(GameObject go in visual)
        {
            go.SetActive(false);
        }
       
    }
    private void Show()
    {
        foreach (GameObject go in visual)
        {
            go.SetActive(true);
        }
    }
}
