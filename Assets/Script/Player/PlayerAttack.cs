using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCoooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }

        if (!TryGetComponent<PlayerMovement>(out playerMovement))
        {
            Debug.LogError("PlayerMovement component not found on this GameObject.");
        }
    }

    public void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        animator.SetTrigger("Attack");
        cooldownTimer = 0;

        fireballs[FindFireBall()].transform.position = firePoint.position;
        fireballs[FindFireBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    public bool CanAttack()
    {
        return cooldownTimer > attackCoooldown;
    }

    private int FindFireBall()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
}
