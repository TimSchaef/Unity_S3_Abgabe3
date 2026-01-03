// SaveManager.cs (GEÄNDERT)
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public GlobalSaveState SaveState;

    private string SavePath => Application.persistentDataPath + "/SaveData.json";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        LoadGame();
    }

    public void SaveGame()
    {
        var hp = FindPlayerHitpoints();
        if (hp != null)
            SaveState.saveHP = hp.CurrentHP;

        string json = JsonUtility.ToJson(SaveState);
        System.IO.File.WriteAllText(SavePath, json);

        Debug.Log("Save completed");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(SavePath))
        {
            string json = System.IO.File.ReadAllText(SavePath);
            SaveState = JsonUtility.FromJson<GlobalSaveState>(json);
            Debug.Log("Load completed");
        }
        else
        {
            SaveState = new GlobalSaveState();
            SaveGame();
        }
    }

    public void DeleteSaveFile()
    {
        // 1) Datei löschen (falls vorhanden)
        if (System.IO.File.Exists(SavePath))
            System.IO.File.Delete(SavePath);

        // 2) In-Memory SaveState zurücksetzen
        SaveState = new GlobalSaveState();

        // 3) Frischen Save anlegen (damit nächste Loads stabil sind)
        SaveGame();

        Debug.Log("Save deleted and reset completed");
    }

    private Hitpoints FindPlayerHitpoints()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.GetComponent<Hitpoints>() : null;
    }
}


