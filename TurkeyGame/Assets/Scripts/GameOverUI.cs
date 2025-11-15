using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameOverUI creates a simple full-screen overlay with a "Game Over" label.
/// It exposes a static helper `ShowOnce()` so other scripts can request the overlay.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    private static GameOverUI instance;

    private Canvas canvas;

    private void Awake()
    {
        instance = this;
        BuildCanvas();
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    private void BuildCanvas()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        // Background panel
        var bgGO = new GameObject("GameOverBackground");
        bgGO.transform.SetParent(transform, false);
        var img = bgGO.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.6f);
        var rect = img.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        // Text
        var textGO = new GameObject("GameOverText");
        textGO.transform.SetParent(transform, false);
        var text = textGO.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 72;
        text.color = Color.white;
        // Use default Arial font if available
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = "Game Over";

        var tRect = text.rectTransform;
        tRect.anchorMin = new Vector2(0.1f, 0.4f);
        tRect.anchorMax = new Vector2(0.9f, 0.6f);
        tRect.offsetMin = tRect.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// Ensure an instance exists, create one if needed, and show the overlay once.
    /// </summary>
    public static void ShowOnce()
    {
        if (instance == null)
        {
            var go = new GameObject("GameOverUI");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<GameOverUI>();
        }
        instance.StartCoroutine(instance.ShowAndAutoHide());
    }

    private IEnumerator ShowAndAutoHide()
    {
        // Ensure canvas is active
        gameObject.SetActive(true);
        // Wait a short time; the scene restart will usually happen soon enough
        yield return new WaitForSeconds(0.9f);
        // Keep the overlay until scene reload; disable if still present
        gameObject.SetActive(false);
    }
}
