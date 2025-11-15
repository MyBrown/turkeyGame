using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject stovetop;
    public GameObject knife;
    public GameObject fork;
    public GameObject shelf;
    // public GameObject shelf;
    GameObject[] obstacles;
    public Vector3[] spawnLocations;

    private float secondsBetweenSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secondsBetweenSpawn = 3;
        obstacles = new GameObject[] {stovetop, knife, fork, shelf};
        
        StartCoroutine(SpawnObjects());
        

        
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            int obstacleIndex = Random.Range(0, 3);
            GameObject currentOb = obstacles[obstacleIndex];
            Vector3 spawnLoc = spawnLocations[obstacleIndex];
            GameObject newObstacle = Instantiate(currentOb, transform);
            newObstacle.transform.position = spawnLoc;

            if (obstacleIndex == 3)
            {
                GameObject stovetop = Instantiate(obstacles[0], transform);
                Vector3 stoveSpawn = spawnLocations[0];
                stovetop.transform.position = stoveSpawn;
            }
            else if (obstacleIndex == 0)
            {
                GameObject stovetop = Instantiate(obstacles[3], transform);
                Vector3 stoveSpawn = spawnLocations[3];
                stovetop.transform.position = stoveSpawn;
            }


            yield return new WaitForSeconds(secondsBetweenSpawn);
        }
    }
}
