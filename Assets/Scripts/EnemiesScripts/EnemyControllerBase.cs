using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyControllerBase : MonoBehaviour
{
    [Header("Properties")]
    [Space]
    [SerializeField]
    protected int pointsByKill;

    [SerializeField]
    protected float movingSpeed;

    [SerializeField]
    protected float shootingDelay;

    [SerializeField]
    protected int healthPoints;

    [SerializeField]
    protected bool isCanShooting;

    [SerializeField]
    protected bool isCanMoving;

    protected bool isShooting = false;

    protected bool isHit = false;

    [Header("Misc")]
    [Space]
    [SerializeField]
    protected LayerMask bottomWallLayer;

    [SerializeField]
    protected LayerMask playerLayer;

    [SerializeField]
    protected LayerMask playerShotLayer;

    protected EnemyControllerBase() 
    {
        MovingSpeed = movingSpeed;
        ShootingDelay = shootingDelay;
        HealthPoints = healthPoints;
        IsCanMoving = isCanMoving;
        IsCanShooting = isCanShooting;
    }

    public UnityEvent DestroyedHandler;

    public UnityEvent<int> ShotHandler;

    public virtual float MovingSpeed { get => movingSpeed; set => movingSpeed = value; }
    public virtual float ShootingDelay { get => shootingDelay; set => shootingDelay = value; }
    public virtual int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public virtual bool IsCanMoving { get => isCanMoving; set => isCanMoving = value; }
    public virtual bool IsCanShooting { get => isCanShooting; set => isCanShooting = value; }

    protected virtual void Awake()
    {
        DestroyedHandler = new UnityEvent();
        ShotHandler = new UnityEvent<int>();
    }

    protected virtual void Update()
    {
        if (IsCanMoving)
            transform.localPosition += movingSpeed * Time.deltaTime * Vector3.down;

        if (IsCanShooting)
            StartCoroutine(EnemyShootRoutine());
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            int collisionLayer = collision.gameObject.layer;
            if (bottomWallLayer == (1 << collisionLayer))
            {
                DestroyedHandler?.Invoke();
                Destroy(gameObject);
            }

            //if (collision.gameObject.CompareTag("Shield"))
            //{
            //    destroyedHandler.Invoke();
            //    Destroy(gameObject);
            //}

            else if (collision.gameObject.CompareTag("DeathRay"))
            {
                // SCORE INCREASE FOR THAT !!!
                ShotHandler?.Invoke(pointsByKill);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            int collisionLayer = collision.gameObject.layer;

            if (playerLayer == (1 << collisionLayer))
            {
                DestroyedHandler?.Invoke();
                Destroy(gameObject);
            }
            else if (playerShotLayer == (1 << collisionLayer))
            {
                // SCORE INCREASE FOR THAT !!!
                var damage = 0;
                if (collision.gameObject.TryGetComponent(out SingleShotBehaviour shot))
                {
                    damage = shot.Damage;
                }

                EnemyDamageHit(damage);
            }
        }
    }

    protected virtual void EnemyDamageHit(int takenDamage)
    {
        HealthPoints -= takenDamage;

        if (HealthPoints <= 0)
        {
            HealthPoints = 0;
            ShotHandler?.Invoke(pointsByKill);
            Destroy(gameObject);
        }
    }

    protected abstract IEnumerator EnemyShootRoutine();

    protected abstract void EnemyShoot();

    protected abstract void OnDestroy();

    protected virtual void SetIsHit(int _isHit)
    {
        isHit = Convert.ToBoolean(_isHit);
    }
}
