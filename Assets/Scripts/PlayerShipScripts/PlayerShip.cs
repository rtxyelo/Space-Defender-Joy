using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerShip : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Keyboard motion control.")]
    protected bool isDebugMode;

    [SerializeField]
    [Tooltip("Player maneuverability.")]
    [Range(1f, 7f)]
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

    protected bool isDead = false;

    protected bool isMoveAllowed = true;

    protected float directionX;

    protected PlayerShip()
    {
        Maneuverability = maneuverability;
        Damage = damage;
        Durability = durability;
        ShootingSpeed = shootingSpeed;
        IsDead = isDead;
        IsMoveAllowed = isMoveAllowed;
    }

    public virtual float Maneuverability { get => maneuverability; set => maneuverability = value; }
    public virtual int Damage { get => damage; set => damage = value; }
    public virtual int Durability { get => durability; set => durability = value; }
    public virtual float ShootingSpeed { get => shootingSpeed; set => shootingSpeed = value; }
    public virtual bool IsDead { get => isDead; set => isDead = value; }
    public virtual bool IsMoveAllowed { get => isMoveAllowed; set => isMoveAllowed = value; }

    /// <summary>
    /// Direction magnitude by acceleration.
    /// </summary>
    public virtual float MoveDirection()
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
    
    public abstract void Shoot();

    [System.Serializable]
    public struct ShipInfo
    {
        public float maneuverability;
        public int damage;
        public int durability;
        public float shootingSpeed;
        public bool isDead;
        public bool isMoveAllowed;

        public ShipInfo(float _maneuverability, int _damage, int _durability, float _shootingSpeed, bool _isDead, bool _isMoveAllowed)
        {
            maneuverability = _maneuverability;
            damage = _damage;
            durability = _durability;
            shootingSpeed = _shootingSpeed;
            isDead = _isDead;
            isMoveAllowed = _isMoveAllowed;
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
}

