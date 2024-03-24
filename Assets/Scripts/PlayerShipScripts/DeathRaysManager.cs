using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathRaysManager : MonoBehaviour
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

    [SerializeField]
    private GameInfoDisplay gameInfoDisplay;

    private HashSet<GameObject> _rayOneSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayTwoSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayThreeSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayFourSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayFiveSet = new HashSet<GameObject>();

    private void Awake()
    {
        if (_rayOne != null)
            _rayOne.RayCastDetectEvent.AddListener(RayCastOneHandler);

        if (_rayTwo != null)
            _rayTwo.RayCastDetectEvent.AddListener(RayCastTwoHandler);

        if (_rayThree != null)
            _rayThree.RayCastDetectEvent.AddListener(RayCastThreeHandler);

        if (_rayFour != null)
            _rayFour.RayCastDetectEvent.AddListener(RayCastFourHandler);

        if (_rayFive != null)
            _rayFive.RayCastDetectEvent.AddListener(RayCastFiveHandler);
    }

    private void Update()
    {
        HashSet<GameObject> _raysSet = new HashSet<GameObject>();

        if (_rayOneSet != null)
            _raysSet.UnionWith(_rayOneSet);

        if (_rayTwoSet != null)
            _raysSet.UnionWith(_rayTwoSet);

        if (_rayThreeSet != null)
            _raysSet.UnionWith(_rayThreeSet);

        if (_rayFourSet != null)
            _raysSet.UnionWith(_rayFourSet);

        if (_rayFiveSet != null)
            _raysSet.UnionWith(_rayFiveSet);

        if (_raysSet.Count > 0 )
        {
            foreach (var enemy in _raysSet)
            {
                if (enemy != null)
                {
                    int money = EnemyTagToMoneyCount(enemy.tag);
                    if (gameInfoDisplay != null)
                    {
                        gameInfoDisplay.IncreaseMoneyCount(money);
                    }
                    Destroy(enemy);
                }
            }
            _raysSet.Clear();

            if (_rayOneSet != null)
                _rayOneSet.Clear();

            if (_rayTwoSet != null)
                _rayTwoSet.Clear();

            if (_rayThreeSet != null)
                _rayThreeSet.Clear();

            if (_rayFourSet != null)
                _rayFourSet.Clear();

            if (_rayFiveSet != null)
                _rayFiveSet.Clear();
        }
    }

    private void RayCastOneHandler(GameObject enemy)
    {
        _rayOneSet.Add(enemy);
    }
    private void RayCastTwoHandler(GameObject enemy)
    {
        _rayTwoSet.Add(enemy);
    }
    private void RayCastThreeHandler(GameObject enemy)
    {
        _rayThreeSet.Add(enemy);
    }
    private void RayCastFourHandler(GameObject enemy)
    {
        _rayFourSet.Add(enemy);
    }

    private void RayCastFiveHandler(GameObject enemy)
    {
        _rayFiveSet.Add(enemy);
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
