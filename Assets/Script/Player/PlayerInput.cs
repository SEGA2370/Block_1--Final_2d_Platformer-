using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerInput : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Application.isMobilePlatform)
        {
            return;
        }
        else
        {
            HandleMovementInput();
            HandleJumpInput();
            Attack();
        }
    }

    private void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerMovement.SetHorizontalInput(horizontalInput);
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerMovement.AdjustableJump();
        }
    }

    private void Attack()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(0) && playerAttack.CanAttack() && playerMovement.CanAttack())
        {
            playerAttack.Attack();
        }
    }
}
