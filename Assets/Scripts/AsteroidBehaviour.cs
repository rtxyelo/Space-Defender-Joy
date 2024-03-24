using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    private int moneyByKill = 50;

    [SerializeField]
    private LayerMask bottomWallLayer;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask playerShotLayer;

    [SerializeField]
    private float fallingSpeed = 5.0f;

    [HideInInspector]
    public AudioBehaviour AudioBehaviour;

    public UnityEvent DestroyedHandler;

    public UnityEvent<int> ShotHandler;

    private void Awake()
    {
        DestroyedHandler = new UnityEvent();
        ShotHandler = new UnityEvent<int>();
    }

    private void Update()
    {
        transform.localPosition += fallingSpeed * Time.deltaTime * Vector3.down;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

            //else if (collision.gameObject.CompareTag("DeathRay"))
            //{
            //    // SCORE INCREASE FOR THAT !!!
            //    shotHandler.Invoke(pointsByKill);
            //    Destroy(gameObject);
            //}
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
                ShotHandler?.Invoke(moneyByKill);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        AudioBehaviour.PlayeAsteroidDestroyedSound();
        ShotHandler.RemoveAllListeners();
        DestroyedHandler.RemoveAllListeners();
    }
}