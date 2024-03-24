using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemies = new(6);

    [SerializeField]
    private List<Transform> _spawnPositions = new(15);

    [SerializeField]
    private float _spawnTime;

    [SerializeField]
    private GameInfoDisplay gameInfoDisplay;

    [SerializeField]
    private AudioBehaviour audioBehaviour;

    private bool _isSpawning = false;

    public bool IsSpawnAllow { get; set; } = false;
    public float SpawnTime { get => _spawnTime; set => _spawnTime = value; }

    private List<GameObject> _spawnedEnemies = new();

    private List<GameObject> _enemiesToSpawn = new();

    private void Update()
    {
        if (IsSpawnAllow)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    public void CollectEnemyListByLevelComplexity(Dictionary<EnemyType, int> listOfEnemies)
    {
        var indexCounter = 0;
        foreach (var enemy in listOfEnemies)
        {
            for (int i = 0; i < enemy.Value; i++)
            {
                _enemiesToSpawn.Add(_enemies[indexCounter]);
            }
            indexCounter++;
        }
    }

    public void Spawn()
    {
        int randomIndex = Random.Range(0, _enemiesToSpawn.Count);
        GameObject enemyPrefab = _enemiesToSpawn[randomIndex];

        Vector3 randomSpawnPosition = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;

        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity, transform);
        
        //_spawnedEnemies.Add(newEnemy);

        if (newEnemy.TryGetComponent(out EnemyController controller))
        {
            controller.AudioBehaviour = audioBehaviour;
            controller.ShotHandler.AddListener(Destroyed);
            controller.IsCanShooting = true;
        }
        else if (newEnemy.TryGetComponent(out AsteroidBehaviour component))
        {
            component.AudioBehaviour = audioBehaviour;
            component.ShotHandler.AddListener(Destroyed);
        }
    }

    /// <summary>
    /// Spawns enemies at regular intervals.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    public IEnumerator SpawnRoutine()
    {
        if (_isSpawning)
        {
            yield break;
        }

        _isSpawning = true;

        while (IsSpawnAllow)
        {
            yield return new WaitForSeconds(_spawnTime);
            Spawn();
        }

        _isSpawning = false;
    }

    private void Destroyed(int money)
    {
        gameInfoDisplay.IncreaseMoneyCount(money);
    }
}
