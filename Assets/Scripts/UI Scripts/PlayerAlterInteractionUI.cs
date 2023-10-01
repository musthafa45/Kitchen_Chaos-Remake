using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAlterInteractionUI : MonoBehaviour
{
    public event EventHandler OnAlterInteractionButtonClicked;

    [SerializeField] private GameObject altrButtonGameObject;

    private Button altrInteractButton;

   // Player player;

    private void Awake()
    {
        altrInteractButton = altrButtonGameObject.GetComponent<Button>();
        //player = Player.Instance;     // SomeTime Bit Dangerous it cause null ref.
    }
    private void Start()
    {
        if(TouchManager.Instance.interactionMode == TouchManager.InteractionMode.TapInteraction)
        {
            gameObject.SetActive(false);
            return;
        }
         Player.Instance.OnPlayerAlterInteract += Player_OnPlayerAlterInteract;
         Player.Instance.OnPlayerAlterInteractCanceled += Player_OnPlayerAlterInteractCanceled;

        altrButtonGameObject.SetActive(false);

        altrInteractButton.onClick.AddListener(() =>
        {
            OnAlterInteractionButtonClicked?.Invoke(this, EventArgs.Empty);
        });
    }
    private void OnDisable()
    {
        Player.Instance.OnPlayerAlterInteract -= Player_OnPlayerAlterInteract;
        Player.Instance.OnPlayerAlterInteractCanceled -= Player_OnPlayerAlterInteractCanceled;
    }

    private void Player_OnPlayerAlterInteractCanceled(object sender, EventArgs e)
    {
        altrButtonGameObject.SetActive(false);
    }

    private void Player_OnPlayerAlterInteract(object sender, EventArgs e)
    { 
        altrButtonGameObject.SetActive(true);
    }

    public bool GetButtonStatus()
    {
        return altrButtonGameObject.activeInHierarchy;
    }
}
