using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAsteroidSpawner : MonoBehaviour, ISpawner
{
    [SerializeField]
    private List<GameObject> asteroidsList = new List<GameObject>(2);

    [SerializeField]
    private List<Transform> spawnPositionsList = new List<Transform>(10);

    [SerializeField]
    private int m_numberOfAsteroids = 7;

    [SerializeField]
    private float m_spawnTime = 1.6f;

    private List<GameObject> spawnedAsteroidsList = new List<GameObject>();

    private bool isSpawning = false;

    public bool IsSpawnAllow { get; set; } = false;

    private int numberOfDestroyedAsteroids;

    private int numberOfAsteroidsOrigin;

    private void Start()
    {
        numberOfAsteroidsOrigin = m_numberOfAsteroids;
        numberOfDestroyedAsteroids = m_numberOfAsteroids;
    }

    private void Update()
    {
        if (m_numberOfAsteroids > 0 && IsSpawnAllow)
        {
            StartCoroutine(SpawnRoutine());
        }
        if (numberOfDestroyedAsteroids == 0)
        {
            IsSpawnAllow = false;
            m_numberOfAsteroids = numberOfAsteroidsOrigin;
            numberOfDestroyedAsteroids = numberOfAsteroidsOrigin;
        }
    }

    /// <summary>
    /// Spawns asteroids at regular intervals.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    public IEnumerator SpawnRoutine()
    {
        if (isSpawning)
        {
            yield break;
        }

        isSpawning = true;

        while (m_numberOfAsteroids > 0 && IsSpawnAllow)
        {
            yield return new WaitForSeconds(m_spawnTime);
            Spawn();
        }

        isSpawning = false;
    }

    public void Spawn()
    {
        if (m_numberOfAsteroids > 0)
        {
            int randomAsteroidIndex = Random.Range(0, asteroidsList.Count);
            GameObject planetPrefab = asteroidsList[randomAsteroidIndex];

            Vector3 randomSpawnPosition = spawnPositionsList[Random.Range(0, spawnPositionsList.Count)].position;

            GameObject newAsteroid = Instantiate(planetPrefab, randomSpawnPosition, Quaternion.identity, transform);

            spawnedAsteroidsList.Add(newAsteroid);

            // In titorial scene asteroid prefabs use background planets script as their own!
            if (newAsteroid.TryGetComponent(out PlanetController controller))
            {
                controller.AddListener(Destroyed);
            }

            float randomScale = Random.Range(0.85f, 1.35f);
            newAsteroid.transform.localScale = new Vector3(randomScale, randomScale, 1);

            m_numberOfAsteroids--;
        }
    }

    public void SetAsteroidsCount(int count)
    {
        m_numberOfAsteroids = count;
        numberOfAsteroidsOrigin = m_numberOfAsteroids;
        numberOfDestroyedAsteroids = m_numberOfAsteroids;
    }

    public void AsteroidDestroyHandler()
    {
        foreach (var item in spawnedAsteroidsList)
        {
            Destroy(item);
        }

        spawnedAsteroidsList.Clear();
    }

    public void Destroyed() { }

    public void Destroyed(GameObject asteroid)
    {
        numberOfDestroyedAsteroids--;
        spawnedAsteroidsList.Remove(asteroid);
        //Debug.Log("Destroyed is invoked! Number of asteroids: " + numberOfDestroyedAsteroids);
    }
}
