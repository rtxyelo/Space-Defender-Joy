using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    private int pointsByKill = 50;

    [SerializeField]
    private LayerMask bottomWallLayer;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask playerShotLayer;

    [SerializeField]
    private float fallingSpeed = 1.0f;

    private UnityEvent destroyedHandler;

    private ShotEventHandler shotHandler;

    private void Awake()
    {
        destroyedHandler = new UnityEvent();
        shotHandler = new ShotEventHandler();
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
                destroyedHandler.Invoke();
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
                destroyedHandler.Invoke();
                Destroy(gameObject);
            }
            else if (playerShotLayer == (1 << collisionLayer))
            {
                // SCORE INCREASE FOR THAT !!!
                shotHandler.Invoke(pointsByKill);
                Destroy(gameObject);
            }
        }
    }

    public void AddListener(UnityAction<int> action)
    {
        shotHandler.AddListener(action);
    }

    public void RemoveListener(UnityAction<int> action)
    {
        shotHandler.RemoveListener(action);
    }

    public void AddListener(UnityAction action)
    {
        destroyedHandler.AddListener(action);
    }

    public void RemoveListener(UnityAction action)
    {
        destroyedHandler.RemoveListener(action);
    }
}