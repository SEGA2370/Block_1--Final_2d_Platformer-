using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage // Will Damage Player Upon Collision
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;

    private Animator animator;
    private BoxCollider2D boxCollider; 

    private bool hit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();  
    }

    public void ActivateProjectile()
    {
        hit = false;

        lifeTime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    private void Update()
    {
        if (hit)
        {
            return;
        }

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0 ,0);

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        // Activate Parent Script Logic First  Parent is(EnemyDamage) 
        base.OnTriggerEnter2D(collision); 
        boxCollider.enabled = false;
        
        if(animator != null)
        {
            // When Object is Fireball explode it
            animator.SetTrigger("Explode"); 
        }
        else
        {
            // Deactivates when ever hits anything
            gameObject.SetActive(false); 

        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
