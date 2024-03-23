using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private List<Transform> spawnPositionsList = new List<Transform>(2);

    [SerializeField]
    private int m_numberOfEnemies = 2;

    [SerializeField]
    private float m_spawnTime = 0.5f;

    [SerializeField]
    private AudioBehaviour audioBehaviour;

    private int enemyIndex = 0;

    private List<GameObject> spawnList = new List<GameObject>(2);

    private bool isSpawning = false;

    public bool IsSpawnAllow { get; set; } = false;

    private void Update()
    {
        if (m_numberOfEnemies > 0 && IsSpawnAllow)
        {
            StartCoroutine(SpawnRoutine());
        }
        if (m_numberOfEnemies == 0)
        {
            IsSpawnAllow = false;
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

        while (m_numberOfEnemies > 0)
        {
            EnemyController newEnemy = Spawn();
            if (newEnemy != null)
            {
                yield return new WaitForSeconds(m_spawnTime);
                StopEnemy(newEnemy);
            }
        }

        isSpawning = false;
    }

    public EnemyController Spawn()
    {
        Debug.Log("enemyIndex " + enemyIndex);
        if (enemyIndex < 2)
        {
            Vector3 spawnPosition = spawnPositionsList[enemyIndex].position;

            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);

            spawnList.Add(spawnedEnemy);

            EnemyController controller = spawnedEnemy.GetComponent<EnemyController>();

            //controller.audioBehaviour = audioBehaviour;
            controller.IsCanShooting = false;

            m_numberOfEnemies--;

            enemyIndex++;

            return controller;
        }
        return null;
    }

    public void StopEnemy(EnemyController enemy)
    {
        enemy.IsCanMoving = false;
    }

    public void DeactivateEnemies()
    {
        foreach (var enemy in spawnList)
        {
            enemy.SetActive(false);
        }
    }

    public void ActivateEnemies()
    {
        foreach (var enemy in spawnList)
        {
            enemy.SetActive(true);
        }
    }
}