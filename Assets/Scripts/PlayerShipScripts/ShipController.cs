using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : PlayerShipBase
{
    private Rigidbody2D rb;

    private Animator animator;

    private TouchDetection touchDetection;

    [Header("Misc")]
    [Space]
    [SerializeField]
    private AudioBehaviour audioBehaviour;

    [SerializeField]
    private GameObject shotPrefab;

    [SerializeField]
    private int shotParentIndex;

    protected override void Shoot()
    {
        audioBehaviour.PlayShotSound();
        var shot = Instantiate(shotPrefab, gameObject.transform.GetChild(shotParentIndex));

        // подписатьс€ на событи€ shot!!! (дл€ детекта попаданий)
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

    // todo Add animations and stuff
    protected override void DamageHit(DamageType damageType)
    {
        base.DamageHit(damageType);
        audioBehaviour.PlayDamageSound();
        if (Durability > 0)
        {
            SetIsHit(1);
        }
        else if (Durability <= 0)
        {
            Durability = 0;
            HealthPoints--;
            if (HealthPoints <= 0)
            {
                HealthPoints = 0;
                ShipDestroyed(damageType);
            }
        }
    }

    // todo Add animations and stuff
    protected override void ShipDestroyed(DamageType damageType)
    {
        base.ShipDestroyed(damageType);
        Debug.Log($"Ship destroyed by {damageType}!");
        isDead = true;
        animator.SetBool("IsDead", isDead);
    }

    protected override void TutorialDamageHit()
    {
        SetIsHit(1);
        audioBehaviour.PlayDamageSound();
    }

    protected override void SetIsHit(int _isHit)
    {
        base.SetIsHit(_isHit);
        animator.SetBool("IsHit", isHit);
    }

    public override void SetIsDead(DamageType damageType)
    {
        ShipDestroyed(damageType);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        touchDetection = rb.GetComponent<TouchDetection>();

        animator = GetComponent<Animator>();
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsHit", isHit);

        ShipInfo shipInfo = new ShipInfo(Maneuverability, Damage, Durability, ShootingSpeed, HealthPoints, IsDead, IsMoveAllow, IsShootingAllow);
        Debug.Log("Ship info " + shipInfo.SerializeShipInfo());
    }

    private void Update()
    {
        directionX = MoveDirection();

        if (touchDetection.TouchDetected)
        {
            StartCoroutine(ShootRoutine());
        }
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
