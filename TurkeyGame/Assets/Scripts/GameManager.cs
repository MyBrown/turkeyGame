using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject GameOverText;

    private void Awake()
    {
        Instance = this;
        GameOverText.SetActive(false);
        Debug.Log("GameManager ready. GameOverText hidden.");
    }

    public void TriggerGameOver()
    {
        Debug.Log("TriggerGameOver called!");
        StartCoroutine(HandleGameOver());
    }

    public IEnumerator HandleGameOver()
    {
        Debug.Log("Game Over sequence started...");
        yield return new WaitForSecondsRealtime(2f);
        GameOverText.SetActive(true);
        Debug.Log("Game Over sequence finished.");
        // Add logic here to restart the level or return to menu
    }
}
