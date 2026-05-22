using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject stovetop;
    public GameObject knife;
    public GameObject fork;
    public GameObject shelf;
    public float secondsBetweenSpawn;
    GameObject[] obstacles;
    public Vector3[] spawnLocations;
    private float utensilOffset;
    private GameObject currentOb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles = new GameObject[] {stovetop, knife, fork, shelf};
        
        StartCoroutine(SpawnObjects());
        
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            int obstacleIndex = Random.Range(0, 4);
            utensilOffset = Random.Range(0,2);
            currentOb = obstacles[obstacleIndex];
            
            if (obstacleIndex == 1 || obstacleIndex == 2)
            {
                GameObject newObstacle = Instantiate(currentOb, transform);
                currentOb.transform.Translate(0, utensilOffset, 0);
            }
            if (obstacleIndex == 0){
                Vector3 spawnLoc = spawnLocations[obstacleIndex];
                GameObject newObstacle = Instantiate(currentOb, transform);
                newObstacle.transform.position = spawnLoc;
            }
            else if (obstacleIndex == 3)
            {
                GameObject shelf = Instantiate(obstacles[3], transform);
                Vector3 stoveSpawn = spawnLocations[3];
                shelf.transform.position = stoveSpawn;
            }

            yield return new WaitForSeconds(secondsBetweenSpawn);
        }
    }
}
