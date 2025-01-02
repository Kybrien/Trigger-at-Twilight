using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        // Récupère la caméra principale au démarrage
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Aligner la rotation du Canvas sur la caméra
            transform.LookAt(transform.position + mainCamera.forward);
        }
    }
}
