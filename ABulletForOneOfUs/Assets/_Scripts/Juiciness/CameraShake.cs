using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Durée totale
    public float shakeMagnitude = 0.1f; // Amplitude du tremblement

    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        shakeTimeRemaining = shakeDuration;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
}
