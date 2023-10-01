using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{
    public event EventHandler OnInteractionButtonClicked;

    [SerializeField] private GameObject buttonGameObject;

    private Button interactButton;

    //Player player; // SomeTime Bit Dangerous it cause null ref.

    private void Awake()
    {
        interactButton = buttonGameObject.GetComponent<Button>();
        //player = Player.Instance; // SomeTime Bit Dangerous it cause null ref.
    }
    private void Start()
    {
        if (TouchManager.Instance.interactionMode == TouchManager.InteractionMode.TapInteraction)
        {
            gameObject.SetActive(false);
            return;
        }
        Player.Instance.OnPlayerInteract += Player_OnPlayerInteract;
        Player.Instance.OnPlayerInteractCanceled += Player_OnPlayerInteractCanceled;

        buttonGameObject.SetActive(false);

        interactButton.onClick.AddListener(() =>
        {
            OnInteractionButtonClicked?.Invoke(this, EventArgs.Empty);
            
        });
    }
    private void OnDisable()
    {
        Player.Instance.OnPlayerInteract -= Player_OnPlayerInteract;
        Player.Instance.OnPlayerInteractCanceled -= Player_OnPlayerInteractCanceled;
    }
    private void Player_OnPlayerInteractCanceled(object sender, EventArgs e)
    {
        buttonGameObject.SetActive(false);
    }

    private void Player_OnPlayerInteract(object sender, EventArgs e)
    {
        buttonGameObject.SetActive(true);
    }

    public bool GetButtonStatus()
    {
        return buttonGameObject.activeInHierarchy;
    }
}
