using UnityEngine;
using System.Collections.Generic;

public class RandomAcornSpawner : MonoBehaviour
{
    public GameObject acornPrefab;
    public List<Transform> spawnPoints;
    public int minAcorns = 3;
    public int maxAcorns = 7;

    void Start()
    {
        SpawnAcorns();
    }

    void SpawnAcorns()
    {
        List<Transform> shuffledPoints = new List<Transform>(spawnPoints);
        for (int i = 0; i < shuffledPoints.Count; i++)
        {
            Transform temp = shuffledPoints[i];
            int rand = Random.Range(i, shuffledPoints.Count);
            shuffledPoints[i] = shuffledPoints[rand];
            shuffledPoints[rand] = temp;
        }

        int acornsToSpawn = Random.Range(minAcorns, Mathf.Min(maxAcorns + 1, shuffledPoints.Count));
        for (int i = 0; i < acornsToSpawn; i++)
        {
            Instantiate(acornPrefab, shuffledPoints[i].position, Quaternion.identity);
        }
    }
}
