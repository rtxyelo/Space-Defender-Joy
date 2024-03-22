using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlanetSpawner : MonoBehaviour, ISpawner
{
    [SerializeField]
    private List<GameObject> planetsList = new List<GameObject>(10);
    
    [SerializeField]
    private List<Transform> spawnPositionsList = new List<Transform>(15);

    [SerializeField]
    private float m_spawnTime = 3f;

    private bool isSpawning = false;

    public bool IsSpawnAllow { get; set; } = true;

    private void Update()
    {
        StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Spawns background planets at regular intervals.
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
            Spawn();
            yield return new WaitForSeconds(m_spawnTime);
        }

        isSpawning = false;
    }

    public void Spawn()
    {
        int randomPlanetIndex = Random.Range(0, planetsList.Count);
        GameObject planetPrefab = planetsList[randomPlanetIndex];

        Vector3 randomSpawnPosition = spawnPositionsList[Random.Range(0, spawnPositionsList.Count)].position;

        GameObject newPlanet = Instantiate(planetPrefab, randomSpawnPosition, Quaternion.identity, transform);

        if (newPlanet.TryGetComponent(out PlanetController controller))
        {
            controller.AddListener(Destroyed);
        }

        float randomScale = Random.Range(0.4f, 1.0f);
        newPlanet.transform.localScale = new Vector3(randomScale, randomScale, 1);
    }

    public void Destroyed() { }

    public void Destroyed(GameObject item) { }

    public void DestroyedByPlayer(DamageType dmgType) { }
}
