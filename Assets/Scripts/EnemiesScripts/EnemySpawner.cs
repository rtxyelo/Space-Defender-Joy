using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemiesList = new(6);

    private List<GameObject> spawnList = new List<GameObject>();

    [SerializeField]
    private List<Transform> spawnPositionsList = new List<Transform>(15);

    [SerializeField]
    private float spawnTime = 0.5f;

    private bool isSpawning = false;

    public bool IsSpawnAllow { get; set; } = false;

    public void Spawn()
    {
        int randomAsteroidIndex = Random.Range(0, enemiesList.Count);
        GameObject planetPrefab = enemiesList[randomAsteroidIndex];

        Vector3 randomSpawnPosition = spawnPositionsList[Random.Range(0, spawnPositionsList.Count)].position;

        GameObject newAsteroid = Instantiate(planetPrefab, randomSpawnPosition, Quaternion.identity, transform);

        spawnList.Add(newAsteroid);

        if (newAsteroid.TryGetComponent(out PlanetController controller))
        {
            controller.AddListener(Destroyed);
        }

    }

    /// <summary>
    /// Spawns enemies at regular intervals.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    public IEnumerator SpawnRoutine()
    {
        if (isSpawning)
        {
            yield break;
        }

        isSpawning = true;

        while (IsSpawnAllow)
        {
            yield return new WaitForSeconds(spawnTime);
            Spawn();
        }

        isSpawning = false;
    }

    private void Destroyed(GameObject enemy)
    {

    }
}
