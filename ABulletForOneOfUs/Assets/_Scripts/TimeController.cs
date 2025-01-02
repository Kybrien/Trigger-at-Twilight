using UnityEngine;
using TMPro; // Utiliser TextMeshPro pour l'UI

public class TimerController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timerText; // R�f�rence au texte de l'UI qui affichera le timer (TextMeshPro)

    private float elapsedTime = 0f; // Temps �coul� en secondes

    void Update()
    {
        // Incr�menter le temps �coul�
        elapsedTime += Time.deltaTime;

        // Convertir le temps �coul� en minutes et secondes
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Mettre � jour le texte du timer
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        Update(); // Mettre � jour l'affichage imm�diatement
    }
}
