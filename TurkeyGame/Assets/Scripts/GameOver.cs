using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// UI and camera shake helpers are used when the player dies

/// <summary>
/// GameOver handles player death and restarts the current scene when the player
/// collides with configured obstacles (by tag or by component type).
/// Attach this to the player turkey GameObject (or the prefab). The spawner will
/// automatically add it to spawned turkeys if it's missing.
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("Death Settings")]
    [Tooltip("Any GameObject with one of these tags will cause an instant game over on collision.")]
    [SerializeField] private string[] deathTags = new string[] { "Obstacle", "Knife", "Fire", "Enemy" };

    [Tooltip("Optional: if a collided GameObject has one of these component type names, it will also count as death.")]
    [SerializeField] private string[] deathComponentTypeNames = new string[] { "Knife", "Fire" };

    [Header("Restart")]
    [Tooltip("Delay in seconds between death and scene restart (allows effects/animations).")]
    [SerializeField] private float restartDelay = 1.0f;

    [Header("Death Feedback")]
    [Tooltip("Optional sound played when the player dies.")]
    [SerializeField] private AudioClip deathSfx;

    [Tooltip("Camera shake duration in seconds when death occurs.")]
    [SerializeField] private float shakeDuration = 0.35f;

    [Tooltip("Camera shake magnitude when death occurs.")]
    [SerializeField] private float shakeMagnitude = 0.12f;

    private bool isDead;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collided with: {collision.gameObject.name} tag={collision.gameObject.tag}");
        if (isDead) return;
        if (IsDeathCollision(collision.gameObject))
        {
            StartCoroutine(HandleGameOver());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collided with: {other.gameObject.name} tag={other.gameObject.tag}");
        if (isDead) return;
        if (IsDeathCollision(other.gameObject))
        {
            StartCoroutine(HandleGameOver());
        }
    }

    private bool IsDeathCollision(GameObject other)
    {
        if (other == null) return false;

        // Check tags first
        foreach (var t in deathTags)
        {
            if (!string.IsNullOrEmpty(t) && other.CompareTag(t))
                return true;
        }

        // Check by component type name (non-ideal but flexible for arbitrary obstacle scripts)
        foreach (var typeName in deathComponentTypeNames)
        {
            if (string.IsNullOrEmpty(typeName)) continue;
            var comp = other.GetComponent(typeName);
            if (comp != null) return true;
        }

        return false;
    }

    private IEnumerator HandleGameOver()
    {
        isDead = true;
        // Play death sound (if assigned)
        if (deathSfx != null)
        {
            AudioSource.PlayClipAtPoint(deathSfx, transform.position);
        }

        // Trigger camera shake (if there's a main camera)
        var cam = Camera.main;
        if (cam != null)
        {
            var shaker = cam.GetComponent<CameraShake>();
            if (shaker == null)
            {
                shaker = cam.gameObject.AddComponent<CameraShake>();
            }
            shaker.Shake(shakeDuration, shakeMagnitude);
        }

        // Show Game Over UI (will create one if none exists)
        GameOverUI.ShowOnce();

        yield return new WaitForSeconds(restartDelay);
        RestartScene();
    }

    /// <summary>
    /// Public API to trigger a game over from other systems (e.g., traps or timers).
    /// </summary>
    public void TriggerGameOver()
    {
        if (isDead) return;
        StartCoroutine(HandleGameOver());
    }

    private void RestartScene()
    {
        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(active.buildIndex);
    }
}
