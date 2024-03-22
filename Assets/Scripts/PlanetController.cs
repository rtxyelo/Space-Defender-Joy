using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    private LayerMask bottomWallLayer;   
    
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask playerShotLayer;

    [SerializeField]
    private float fallingSpeed = 1.0f;

    private DestroyEventHandler destroyedHandler;

    private void Awake()
    {
        destroyedHandler = new DestroyEventHandler();
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
                destroyedHandler.Invoke(gameObject);
                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Shield"))
            {
                destroyedHandler.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            int collisionLayer = collision.gameObject.layer;
            if (playerLayer == (1 << collisionLayer))
            {
                Debug.Log("Collide with player!");
                destroyedHandler.Invoke(gameObject);
                Destroy(gameObject);
            }
            else if (playerShotLayer == (1 << collisionLayer))
            {
                destroyedHandler.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void AddListener(UnityAction<GameObject> action)
    {
        destroyedHandler.AddListener(action);
    }

    public void RemoveListener(UnityAction<GameObject> action)
    {
        destroyedHandler.RemoveListener(action);
    }
}

[System.Serializable]
public class DestroyEventHandler : UnityEvent<GameObject> { };