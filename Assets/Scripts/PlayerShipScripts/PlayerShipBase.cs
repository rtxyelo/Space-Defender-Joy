using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class PlayerShipBase : MonoBehaviour
{
    [Header("Debug")]
    [Space]
    [SerializeField]
    [Tooltip("Keyboard motion control.")]
    protected bool isDebugMode;

    [Header("Ship stats")]
    [Space]
    [Tooltip("Player maneuverability.")]
    [Range(1f, 7f)]
    [SerializeField]
    protected float maneuverability;

    [SerializeField]
    [Tooltip("Player damage.")]
    [Range(1, 7)]
    protected int damage;

    [SerializeField]
    [Tooltip("Player durability.")]
    [Range(1, 7)]
    protected int durability;

    [SerializeField]
    [Tooltip("Player shooting speed.")]
    [Range(1f, 7f)]
    protected float shootingSpeed;

    [SerializeField]
    [Tooltip("Player health points count.")]
    [Range(1, 4)]
    protected int healthPoints;

    protected int durabilityOrigin;

    protected bool isHit = false;

    protected bool isHeal = false;

    protected bool isDead = false;

    protected bool isMoveAllow = true;
    
    protected bool isShootingAllow = true;

    protected bool isShooting = false;

    protected float directionX;

    public bool isTutorial;

    protected TutorialSceneController tutorialSceneController;

    public UnityEvent DeadEventHandler;

    public UnityEvent ShotEventHandler;

    protected PlayerShipBase()
    {
        Maneuverability = maneuverability;
        Damage = damage;
        Durability = durability;
        ShootingSpeed = shootingSpeed;
        HealthPoints = healthPoints;
        IsDead = isDead;
        IsMoveAllow = isMoveAllow;
        IsShootingAllow = isShootingAllow;
    }

    public virtual float Maneuverability { get => maneuverability; set => maneuverability = value; }
    public virtual int Damage { get => damage; set => damage = value; }
    public virtual int Durability { get => durability; set => durability = value; }
    public virtual float ShootingSpeed { get => shootingSpeed; set => shootingSpeed = value; }
    public virtual int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public virtual bool IsDead { get => isDead; set => isDead = value; }
    public virtual bool IsMoveAllow { get => isMoveAllow; set => isMoveAllow = value; }
    public virtual bool IsShootingAllow { get => isShootingAllow; set => isShootingAllow = value; }

    protected virtual private void Awake()
    {
        durabilityOrigin = Durability;

        DeadEventHandler = new UnityEvent();
        ShotEventHandler = new UnityEvent();

        var obj = FindAnyObjectByType<TutorialSceneController>();

        if (obj != null)
        {
            isTutorial = true;
        }
        else
        {
            isTutorial = false;
            IsShootingAllow = true;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if ((1 << LayerMask.NameToLayer("Enemy") & 1 << collision.gameObject.layer) != 0)
            {
                //Debug.Log("Player collide with enemy!");

                if (!isTutorial)
                    DamageHit(DamageType.Enemy);
                else
                    TutorialDamageHit();
            }

            if ((1 << LayerMask.NameToLayer("EnemyShot") & 1 << collision.gameObject.layer) != 0)
            {
                //Debug.Log("Player shot!");

                if (!isTutorial)
                    DamageHit(EnemyTagToDamageType(collision.gameObject.tag));
                else
                    TutorialDamageHit();
            }
        }
    }

    /// <summary>
    /// Ignores collision of ship when it heal.
    /// </summary>
    protected virtual void HealIgnoreCollision(bool ignore)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyShot"), ignore);
    }



    /// <summary>
    /// Convert enemy tag to enum DamageType.
    /// </summary>
    /// <param name="tag">Object tag</param>
    /// <returns></returns>
    protected virtual DamageType EnemyTagToDamageType(string tag)
    {
        return tag switch
        {
            "Enemy" => DamageType.Enemy,
            "Scout" => DamageType.Scout,
            "Raider" => DamageType.Raider,
            "Destroyer" => DamageType.Destroyer,
            "Dreadnought" => DamageType.Dreadnought,
            _ => DamageType.None,
        };
    }

    /// <summary>
    /// Direction magnitude by acceleration.
    /// </summary>
    protected virtual float MoveDirection()
    {
        if (!isDebugMode)
        {
            return Input.acceleration.x * maneuverability;
        }
        else
        {
            return Input.GetAxis("Horizontal") * maneuverability;
        }
    }

    /// <summary>
    /// Decrement durability damage.
    /// </summary>
    /// <param name="damageType">Which kind of damage taken</param>
    protected abstract void DamageHit(DamageType damageType);

    /// <summary>
    /// Sets IsHit animation flag.
    /// </summary>
    /// <param name="_isHit">Value to set</param>
    protected abstract void SetIsHit(int _isHit);

    /// <summary>
    /// Sets IsHeal animation flag.
    /// </summary>
    /// <param name="_isHeal">Value to set</param>
    protected abstract void SetIsHeal(int _isHeal);

    /// <summary>
    /// Sets IsDead animation flag.
    /// </summary>
    /// <param name="damageType">Dead by..</param>
    public abstract void SetIsDead(DamageType damageType);

    protected abstract IEnumerator ShootRoutine();

    /// <summary>
    /// HealthPoints equal zero.
    /// </summary>
    /// <param name="damageType">Which kind of damage taken</param>
    protected abstract void ShipDestroyed(DamageType damageType);

    protected abstract void OnDestroy();

    /// <summary>
    /// Just player shooting function)
    /// </summary>
    protected abstract void Shoot();

    protected abstract void TutorialDamageHit();

}

public enum DamageType
{
    None,
    Enemy,
    Scout,
    Raider,
    Destroyer,
    Dreadnought,
}

[System.Serializable]
public struct ShipInfo
{
    public float maneuverability;
    public int damage;
    public int durability;
    public float shootingSpeed;
    public int healthPoints;
    public bool isDead;
    public bool isMoveAllow;
    public bool isShootingAllow;

    public ShipInfo(float _maneuverability, int _damage, int _durability, float _shootingSpeed, int _healthPoints, bool _isDead, bool _isMoveAllow, bool _isShootingAllow)
    {
        maneuverability = _maneuverability;
        damage = _damage;
        durability = _durability;
        shootingSpeed = _shootingSpeed;
        healthPoints = _healthPoints;
        isDead = _isDead;
        isMoveAllow = _isMoveAllow;
        isShootingAllow = _isShootingAllow;
    }

    public string SerializeShipInfo()
    {
        return JsonUtility.ToJson(this);
    }

    public static ShipInfo DeserializeShipInfo(string jsonString)
    {
        return JsonUtility.FromJson<ShipInfo>(jsonString);
    }
}

