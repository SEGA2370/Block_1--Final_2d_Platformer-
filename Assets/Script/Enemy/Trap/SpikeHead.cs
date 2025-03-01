using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header ("SpikeHead Attributes")]

    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;

    private float checkTimer;
    private Vector3 destination;

    private bool attacking;

    private Vector3[] directions = new Vector3[4];

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        // Move SpikeHead To Destination only if attacking
        if (attacking)
        {
            transform.Translate(destination * speed * Time.deltaTime);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        // checking if spikehead sees Player in all 4 directions
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }

    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range; // Right Direction for spike
        directions[1] = -transform.right * range; // Left Direction for spike
        directions[2] = transform.up * range; // Up Direction for Spike
        directions[3] = -transform.up * range; // Down Direction for Spike
    }

    private void Stop()
    {
        destination = transform.position; // Set destionation as current position so it wont move
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        Stop(); // stop spikehead once hits player
    }

}
