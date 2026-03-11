using UnityEngine;

/// <summary>
/// Spawner_Script is responsible for creating the player (turkey) when the game starts.
/// </summary>
public class Spawner_Script : MonoBehaviour
{
    [Header("Prefab & Spawn")]
    [Tooltip("Assign the turkey prefab that will be instantiated at game start.")]
    [SerializeField] private GameObject turkeyPrefab;

    [Tooltip("Optional: a Transform that marks where the turkey should spawn. If null, uses this GameObject's position.")]
    [SerializeField] private Transform spawnPoint;

    // Holds a reference to the instantiated turkey (the player instance)
    private GameObject turkeyInstance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Use Awake to validate required references early.
    /// </summary>
    private void Awake()
    {
        if (turkeyPrefab == null)
        {
            Debug.LogWarning("Spawner_Script: 'turkeyPrefab' is not assigned in the Inspector.");
        }
    }

    /// <summary>
    /// Start is called before the first frame update. We spawn the turkey here.
    /// </summary>
    private void Start()
    {
        SpawnTurkey();
    }

    /// <summary>
    /// Instantiates the turkey prefab at the chosen spawn position.
    /// This is public so other systems (respawn manager, scene loader) can call it.
    /// </summary>
    public void SpawnTurkey()
    {
        if (turkeyPrefab == null)
        {
            Debug.LogError("Spawner_Script: Cannot spawn turkey because 'turkeyPrefab' is null.");
            return;
        }

        Vector3 spawnPosition = (spawnPoint != null) ? spawnPoint.position : transform.position;
        Quaternion spawnRotation = (spawnPoint != null) ? spawnPoint.rotation : Quaternion.identity;

        // If there's already an instance, optionally destroy it first
        if (turkeyInstance != null)
        {
            Destroy(turkeyInstance);
        }

        turkeyInstance = Instantiate(turkeyPrefab, spawnPosition, spawnRotation);
        turkeyInstance.name = "Player_Turkey"; // set a predictable name

        // Ensure the spawned turkey has a GameOver component so collisions cause a restart.
        var goComp = turkeyInstance.GetComponent<GameOver>();
        if (goComp == null)
        {
            turkeyInstance.AddComponent<GameOver>();
        }
    }

    /// <summary>
    /// (Optional) Expose the spawned turkey instance to other scripts.
    /// </summary>
    public GameObject GetTurkeyInstance()
    {
        return turkeyInstance;
    }
}
