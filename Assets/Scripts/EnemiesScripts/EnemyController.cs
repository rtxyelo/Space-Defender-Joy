using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyControllerBase
{
    private Animator animator;

    [HideInInspector]
    public AudioBehaviour AudioBehaviour;

    [Header("Misc")]
    [Space]
    [SerializeField]
    private GameObject shotPrefab;

    protected override void EnemyShoot()
    {
        AudioBehaviour.PlayEnemyShootingSound();
        Instantiate(shotPrefab, gameObject.transform.GetChild(1));

    }

    protected override IEnumerator EnemyShootRoutine()
    {
        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        while (IsCanShooting)
        {
            yield return new WaitForSeconds(ShootingDelay);
            EnemyShoot();
        }

        isShooting = false;
    }

    protected override void EnemyDamageHit(int takenDamage)
    {
        base.EnemyDamageHit(takenDamage);
        AudioBehaviour.PlayEnemyShotSound();
        if (HealthPoints > 0)
        {
            SetIsHit(1);
        }
    }

    protected override void SetIsHit(int _isHit)
    {
        base.SetIsHit(_isHit);
        animator.SetBool("IsHit", isHit);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsHit", isHit);

    }

    protected override void OnDestroy()
    {
        if (AudioBehaviour != null)
            AudioBehaviour.PlayEnemyDeathSound();

        DestroyedHandler.RemoveAllListeners();
        ShotHandler.RemoveAllListeners();
    }
}
