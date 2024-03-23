using System;
using UnityEngine;
using UnityEngine.Events;

public class SingleShotBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 shotDirection;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float shotSpeed = 20f;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private LayerMask enemyLayer;

    public int Damage { get => damage; private set => damage = value; }

    private Rigidbody2D rb;

    private UnityEvent shotIsDestroyedHandler;

    private UnityEvent<int> enemyHitHandler;

    private void Start()
    {
        if (TryGetComponent(out rb))
        {
            rb.AddForce(shotDirection * shotSpeed, ForceMode2D.Impulse);
        }
        else
            throw new InvalidOperationException("Cannot add force because Rigidbody2D component is missing.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            int collisionLayer = collision.gameObject.layer;
            if (wallLayer == (1 << collisionLayer))
            {
                shotIsDestroyedHandler?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            int collisionLayer = collision.gameObject.layer;
            if (enemyLayer == (1 << collisionLayer))
            {
                enemyHitHandler?.Invoke(damage);
                Destroy(gameObject);
            }
        }
    }
}