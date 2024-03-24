using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathRayBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private GameObject _topBorder;

    [SerializeField]
    private ShipController _shipController;

    private Vector2 _direction = Vector2.up;

    private bool _isRayCastOn = false;

    public UnityEvent<GameObject> RayCastDetectEvent;

    private void Awake()
    {
        RayCastDetectEvent = new UnityEvent<GameObject>();
    }

    public void RayCastToggle(int toggle)
    {
        _isRayCastOn = Convert.ToBoolean(toggle);
    }

    private void Update()
    {
        if (_isRayCastOn)
        {
            if (_shipController != null)
                _shipController.IsShootingAllow = false;

            float distance = Vector3.Distance(_topBorder.transform.position, transform.position);

            RaycastHit2D[] _hits = Physics2D.RaycastAll(transform.position, _direction, distance, _layerMask);

            if (_hits != null)
            {
                foreach (var hit in _hits)
                {
                    Debug.Log("Detected enemy " + hit.collider.gameObject.name);
                    RayCastDetectEvent?.Invoke(hit.collider.gameObject);
                }
            }

        }
        else if (_shipController != null && !_shipController.isTutorial)
            _shipController.IsShootingAllow = true;
    }

    private void OnDestroy()
    {
        RayCastDetectEvent.RemoveAllListeners();
    }
}


