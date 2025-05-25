using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    PlayerMovement movement;
    PlayerAttack attack;

    void Awake()
    {
        movement = FindObjectOfType<PlayerMovement>();
        attack = FindObjectOfType<PlayerAttack>();
    }

    // called by UI button PointerDown
    public void OnLeftDown() => movement.SetHorizontalInput(-1f);
    public void OnRightDown() => movement.SetHorizontalInput(1f);
    // called by UI button PointerUp
    public void OnLeftUp() => movement.SetHorizontalInput(0f);
    public void OnRightUp() => movement.SetHorizontalInput(0f);

    // simple tap actions
    public void OnJump() => movement.Jump();
    public void OnFire() => attack.Attack();
}
