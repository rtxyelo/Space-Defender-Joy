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
    private GameInfoDisplay gameInfoDisplay;

    private HashSet<GameObject> _rayOneSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayTwoSet = new HashSet<GameObject>();

    private HashSet<GameObject> _rayThreeSet = new HashSet<GameObject>();

    private void Start()
    {
        _rayOne.RayCastDetectEvent.AddListener(RayCastOneHandler);

        _rayTwo.RayCastDetectEvent.AddListener(RayCastTwoHandler);

        _rayThree.RayCastDetectEvent.AddListener(RayCastThreeHandler);
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
