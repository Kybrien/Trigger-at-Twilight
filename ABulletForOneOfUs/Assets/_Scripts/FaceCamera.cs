using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        // R�cup�re la cam�ra principale au d�marrage
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Aligner la rotation du Canvas sur la cam�ra
            transform.LookAt(transform.position + mainCamera.forward);
        }
    }
}
