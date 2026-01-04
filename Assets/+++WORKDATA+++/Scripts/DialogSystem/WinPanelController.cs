using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Name der Ziel-Scene

    public void LoadScene()
    {
        Time.timeScale = 1f; // falls Spiel pausiert wurde
        SceneManager.LoadScene(sceneToLoad);
    }
}