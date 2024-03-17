using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of the sixth ship.
/// </summary>
public class KennysShipController : PlayerShip
{
    private Rigidbody2D rb;

    private Animator animator;

    public override void Shoot()
    {
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        animator.SetBool("IsDead", isDead);

        ShipInfo shipInfo = new ShipInfo(Maneuverability, Damage, Durability, ShootingSpeed, IsDead, IsMoveAllowed);
        Debug.Log("Ship info " + shipInfo.SerializeShipInfo());
    }

    private void Update()
    {
        directionX = MoveDirection();
    }

    private void FixedUpdate()
    {
        if (isMoveAllowed && rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x + directionX, 0f);
        }
    }
}
