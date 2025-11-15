using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject stovetop;
    public GameObject knife;
    public GameObject fork;
    // public GameObject shelf;
    GameObject[] obstacles;
    public Vector3[] spawnLocations;

    private float secondsBetweenSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secondsBetweenSpawn = 1;
        obstacles = new GameObject[] {stovetop, knife, fork};
        
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

            newObstacle.transform.Translate(Vector2.left * Time.deltaTime);

            yield return new WaitForSeconds(secondsBetweenSpawn);
        }
    }
}
