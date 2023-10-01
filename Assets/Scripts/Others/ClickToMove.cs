using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class ClickToMove : MonoBehaviour
{
    private CharacterController characterController;
    private Player player;

    [SerializeField]private float speed = 2f;
    private float roationSpeed = 10;


    private void Awake()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
       
    }

   


    public void MovePlayer(Vector3 target)
    {
        Vector3 direction = target  - Player.Instance.transform.position;
        Vector3 movement = speed * Time.deltaTime * direction.normalized;
        characterController.Move(movement);
     
        // rotation
        player.HandleInteraction(direction);
        //player.HandleMovement(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                             Quaternion.LookRotation(direction.normalized), roationSpeed * Time.deltaTime);


    }

}
