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

    public void RayCastToggleOnAnim(int toggle)
    {
        _rayOne.RayCastToggle(toggle);

        _rayTwo.RayCastToggle(toggle);

        _rayThree.RayCastToggle(toggle);
    }
}
