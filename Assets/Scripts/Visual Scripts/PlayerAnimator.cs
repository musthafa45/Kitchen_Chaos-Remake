using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   
    private const string IS_WALKING = "IsWalking";
    private Player player;
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
       TouchManager.Instance.OnPlayerMovingToTarget += Instance_OnPlayerMovingToTarget;
    }

    private void Instance_OnPlayerMovingToTarget(object sender, TouchManager.OnPlayerMovingToTargetEventArgs e)
    {
        animator.SetBool(IS_WALKING, e.IsWalking);
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsMoving());
        
    }
}
