using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastToggle : MonoBehaviour
{
    [SerializeField]
    private DeathRayBehaviour _rayOne;

    [SerializeField]
    private DeathRayBehaviour _rayTwo;

    [SerializeField]
    private DeathRayBehaviour _rayThree;

    [SerializeField]
    private DeathRayBehaviour _rayFour;

    [SerializeField]
    private DeathRayBehaviour _rayFive;

    public void RayCastToggleOnAnim(int toggle)
    {
        if (_rayOne != null)
            _rayOne.RayCastToggle(toggle);

        if (_rayTwo != null)
            _rayTwo.RayCastToggle(toggle);

        if (_rayThree != null)
            _rayThree.RayCastToggle(toggle);

        if (_rayFour != null)
            _rayFour.RayCastToggle(toggle);

        if (_rayFive != null)
            _rayFive.RayCastToggle(toggle);
    }
}
