using System;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { private set; get; }

    public GlobalSaveState SaveState;

    private string SavePath => Application.persistentDataPath + "/SaveData.json";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        LoadGame();
    }

    public void SaveGame()
    {
        //umwandeln von SaveState in einen Text
        string json = JsonUtility.ToJson(SaveState);
        //speichern des Texts auf der Festplatte
        System.IO.File.WriteAllText(SavePath,json);
        
        Debug.Log("Save completed");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(SavePath))
        {
            //datei lesen von festplatte
            string json = System.IO.File.ReadAllText(SavePath);
            //Text in GlobalSaveData-Objekt umwandeln
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
        System.IO.File.Delete(SavePath);
    }
}
