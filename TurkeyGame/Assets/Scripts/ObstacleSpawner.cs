using System.Collections;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.AI;
>>>>>>> parent of a1093c2 (obstacle spawner and obstacle sprite fixes)

public class Spawner : MonoBehaviour
{
    public GameObject stovetop;
    public GameObject knife;
    public GameObject fork;
    public GameObject shelf;
    public float secondsBetweenSpawn;

    public float utensilOffset;
    GameObject[] obstacles;
    public Vector3[] spawnLocations;

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
            GameObject currentOb = obstacles[obstacleIndex];
            // Vector2 spawnLoc = spawnLocations[obstacleIndex];
            utensilOffset = Random.Range(-1, 2);
            GameObject newObstacle = Instantiate(currentOb, transform);
            // newObstacle.transform.position = spawnLoc;
            currentOb.transform.Translate(0, utensilOffset, 0);

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
