using System.Collections;
using UnityEngine;

/// <summary>
/// Simple camera shake component. Call `Shake(duration, magnitude)` to start a shake.
/// </summary>
public class CameraShake : MonoBehaviour
{
    private Coroutine currentShake;

    public void Shake(float duration, float magnitude)
    {
        if (currentShake != null)
        {
            StopCoroutine(currentShake);
            currentShake = null;
        }
        currentShake = StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        currentShake = null;
    }
}
