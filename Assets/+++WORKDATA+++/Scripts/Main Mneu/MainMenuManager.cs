using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Game";

    [Header("UI")]
    [SerializeField] private Button continueButton;

    private string SavePath => Path.Combine(Application.persistentDataPath, "SaveData.json");

    private void Start()
    {
        bool hasSave = File.Exists(SavePath) && new FileInfo(SavePath).Length > 0;

        if (continueButton != null)
            continueButton.interactable = hasSave;
    }

    // ===============================
    // START NEW GAME
    // ===============================
    public void StartNewGame()
    {
        // 1) Save-Datei löschen
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted");
        }

        // 2) SaveState im Speicher zurücksetzen (wichtig bei DontDestroyOnLoad)
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveState = new GlobalSaveState();
        }

        // 3) Spielszene laden
        SceneManager.LoadScene(gameSceneName);
    }

    // ===============================
    // CONTINUE
    // ===============================
    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


