using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject GameOverText;
    public GameObject playAgainButton;

    private void Start()
    {
        Instance = this;
        GameOverText.SetActive(false);
        playAgainButton.SetActive(false);
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
        playAgainButton.SetActive(true);
        Debug.Log("Game Over sequence finished.");
        // Add logic here to restart the level or return to menu
    }
}
