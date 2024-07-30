using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Point")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;
    
    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initialScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header ("Enemy Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        initialScale = enemy.localScale;
    }

    private void OnDisable()
    {
        animator.SetBool("Moving", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }

        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        animator.SetBool("Moving", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        animator.SetBool("Moving", true);

        // Make Enemy face direction
        enemy.localScale = new Vector3 (Mathf.Abs(initialScale.x) * _direction, initialScale.y, initialScale.z);

        // Move In Direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}
