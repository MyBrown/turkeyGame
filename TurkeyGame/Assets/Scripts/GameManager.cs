using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject GameOverText;

    private void Awake()
    {
        // Singleton pattern: Ensure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); 
    }

    public void TriggerGameOver()
    {
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        Debug.Log("Game Over sequence started...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Game Over sequence finished.");
        // Add logic here to restart the level or return to menu
        GameOverText.SetActive(true);
    }
}
