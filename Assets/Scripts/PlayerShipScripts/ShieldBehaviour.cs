using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private float radiusOfOverlapcircle;

    [SerializeField]
    private GameInfoDisplay gameInfoDisplay;

    private Vector2 _direction = Vector2.up;

    private bool _isShieldOn = false;

    public UnityEvent<GameObject> ShieldDetectEvent;

    private void Awake()
    {
        ShieldDetectEvent = new UnityEvent<GameObject>();
    }

    public void ShieldToggle(int toggle)
    {
        _isShieldOn = Convert.ToBoolean(toggle);
    }

    private void Update()
    {
        if (_isShieldOn)
        {
            Debug.Log("Test");
            Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, radiusOfOverlapcircle, _layerMask);

            if (_hits != null)
            {
                foreach (var hit in _hits)
                {
                    Debug.Log("Detected enemy " + hit.gameObject.name);
                    ShieldDetectEvent.Invoke(hit.gameObject);

                    if (hit != null)
                    {
                        int money = EnemyTagToMoneyCount(hit.gameObject.tag);
                        if (gameInfoDisplay != null)
                        {
                            gameInfoDisplay.IncreaseMoneyCount(money);
                        }
                        Destroy(hit.gameObject);
                    }
                }
            }

        }
        
    }

    private void OnDestroy()
    {
        ShieldDetectEvent.RemoveAllListeners();
    }

    /// <summary>
    /// Convert enemy tag to money reward.
    /// </summary>
    /// <param name="tag">Object tag</param>
    /// <returns></returns>
    public int EnemyTagToMoneyCount(string tag)
    {
        return tag switch
        {
            "AsteroidOne" => 50,
            "AsteroidTwo" => 50,
            "EnemyOne" => 200,
            "EnemyTwo" => 400,
            "EnemyThree" => 600,
            "EnemyFour" => 1000,
            _ => 0,
        };
    }
}
