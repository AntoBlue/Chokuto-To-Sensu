using System.Collections.Generic;
using System.IO;
using Interfaces;
using UnityEngine;


[System.Serializable]
public class SaveEntry
{
    public string key;
    public string value;

    public SaveEntry(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
}

[System.Serializable]
public class SaveWrapper
{
    public List<SaveEntry> entries = new List<SaveEntry>();
}

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] entities;
    [SerializeField] private string fileName = "/save.json";
    

    public void SaveFile()
    {
        SaveWrapper saved = new SaveWrapper();

        foreach (GameObject entity in entities)
        {
            foreach (var savableScript in entity.GetComponents<ISavable>())
            {
                saved.entries.Add(new SaveEntry(savableScript.SaveId, savableScript.Save()));
            }
        }
        
        string path = Application.persistentDataPath + fileName;
        File.WriteAllText(path, JsonUtility.ToJson(saved));
        Debug.Log("saved at:" + path);
    }
    
    public void LoadFile()
    {
        string path = Application.persistentDataPath + fileName;

        if (File.Exists(path))
        {
            string saveData = File.ReadAllText(path);
            
            SaveWrapper saved = JsonUtility.FromJson<SaveWrapper>(saveData);

            foreach (GameObject entity in entities)
            {
                foreach (var savableScript in entity.GetComponents<ISavable>())
                {
                    if (saved.entries.Find(e => e.key == savableScript.SaveId) is { } entry)
                    {
                        savableScript.Load(entry.value);
                    }
                }
            }

        }
        
    }
}