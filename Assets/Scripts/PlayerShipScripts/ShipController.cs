using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipController : PlayerShipBase
{
    private Rigidbody2D rb;

    private Animator animator;

    private TouchDetection touchDetection;

    [Header("Misc")]
    [Space]
    [SerializeField]
    private GameInfoDisplay gameInfoDisplay;

    [SerializeField]
    private AudioBehaviour audioBehaviour;

    [SerializeField]
    private GameObject shotPrefab;

    [SerializeField]
    private int shotParentIndex;

    protected override void Shoot()
    {
        audioBehaviour.PlayShotSound();
        Instantiate(shotPrefab, gameObject.transform.GetChild(shotParentIndex));
    }

    protected override IEnumerator ShootRoutine()
    {
        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        while (IsShootingAllow && touchDetection.TouchDetected)
        {
            Shoot();
            yield return new WaitForSeconds(1.8f / ShootingSpeed);
        }

        isShooting = false;
    }

    protected override void DamageHit(DamageType damageType)
    {
        //Debug.Log($"Minus durability by {damageType}! Current durability index: " + Durability);

        Durability -= (int)damageType;
        audioBehaviour.PlayDamageSound();
        if (Durability > 0)
        {
            SetIsHit(1);
        }
        else if (Durability <= 0)
        {
            SetIsHeal(1);
            Durability = durabilityOrigin;
            HealthPoints--;
            gameInfoDisplay.HealthCounterChange(HealthPoints);
            if (HealthPoints <= 0)
            {
                HealthPoints = 0;
                ShipDestroyed(damageType);
            }
            else
                ShotEventHandler?.Invoke();
        }
    }

    protected override void ShipDestroyed(DamageType damageType)
    {
        //Debug.Log($"Ship destroyed by {damageType}!");
        DeadEventHandler?.Invoke();
        isDead = true;
        animator.SetBool("IsDead", isDead);
        Invoke(nameof(OnDestroy), 1f);
    }

    protected override void OnDestroy()
    {
        DeadEventHandler?.RemoveAllListeners();
        ShotEventHandler?.RemoveAllListeners();
        Destroy(gameObject);
    }


    #region Animation Flag Setters
    protected override void SetIsHit(int _isHit)
    {
        isHit = Convert.ToBoolean(_isHit);
        animator.SetBool("IsHit", isHit);
    }

    protected override void SetIsHeal(int _isHeal)
    {
        isHeal = Convert.ToBoolean(_isHeal);
        animator.SetBool("IsHeal", isHeal);
        HealIgnoreCollision(isHeal);
    }
    #endregion

    #region Tutorial Scene Methods

    public override void SetIsDead(DamageType damageType)
    {
        ShipDestroyed(damageType);
    }

    protected override void TutorialDamageHit()
    {
        SetIsHit(1);
        audioBehaviour.PlayDamageSound();
    }

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        touchDetection = rb.GetComponent<TouchDetection>();

        animator = GetComponent<Animator>();
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsHit", isHit);
        animator.SetBool("IsHeal", isHeal);

        gameInfoDisplay.HealthCounterChange(HealthPoints);
    }

    private void Update()
    {
        directionX = MoveDirection();

        if (touchDetection.TouchDetected)
        {
            StartCoroutine(ShootRoutine());
        }

        ShipInfo shipInfo = new ShipInfo(Maneuverability, Damage, Durability, ShootingSpeed, HealthPoints, IsDead, IsMoveAllow, IsShootingAllow);
        Debug.Log("Ship info " + shipInfo.SerializeShipInfo());
    }

    private void FixedUpdate()
    {
        if (isMoveAllow && rb != null)
        {
            if (Mathf.Abs(directionX) < 1E-2)
                rb.velocity = new Vector2(0f, 0f);
            else
                rb.velocity = new Vector2(directionX * Time.deltaTime * 100f, 0f);
        }
        else
            rb.velocity = new Vector2(0f, 0f);
    }
}
