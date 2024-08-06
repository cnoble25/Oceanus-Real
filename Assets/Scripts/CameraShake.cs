using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float Magnitude, float Duration)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            float x = Random.Range(-1f, 1f) * Magnitude;
            float y = Random.Range(-1f, 1f);
            transform.position = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;
        yield return null;
        }
        transform.position = originalPosition;
    }
}
