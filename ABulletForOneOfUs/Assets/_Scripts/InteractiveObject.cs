using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public enum ActionType { Play, Leave, ExternLink }
    public ActionType actionType;
    public string externalLink; // Lien pour ExternLink

    public void TriggerAction()
    {
        switch (actionType)
        {
            case ActionType.Play:
                StartGame();
                break;
            case ActionType.Leave:
                QuitGame();
                break;
            case ActionType.ExternLink:
                OpenLink();
                break;
        }
    }

    private void StartGame()
    {
        Debug.Log("Starting the game...");
        // Remplacez par le chargement de la scène principale
        int rand;
        rand = Random.Range(0, 3);

        if (rand == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Halloween");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Base");
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    private void OpenLink()
    {
        if (!string.IsNullOrEmpty(externalLink))
        {
            Debug.Log($"Opening link: {externalLink}");
            Application.OpenURL(externalLink);
        }
        else
        {
            Debug.LogWarning("External link is empty or not assigned.");
        }
    }
}
