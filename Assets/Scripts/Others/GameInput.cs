using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteraction;
    public event EventHandler OnAlternateInteraction;
    private PlayerInputAction playerInputActions;
    
   
    private void Awake() {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAlternateInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteraction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {

        //Vector2 inputVector = new Vector2(0, 0);

        //if (Input.GetKey(KeyCode.W)) {
        //    inputVector.y = +1;
        //}
        //if (Input.GetKey(KeyCode.S)) {                       // OLD LEGACY MOVEMENT INPUT
        //    inputVector.y = -1;
        //}
        //if (Input.GetKey(KeyCode.D)) {
        //    inputVector.x = +1;
        //}
        //if (Input.GetKey(KeyCode.A)) {
        //    inputVector.x = -1;
        //}
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
