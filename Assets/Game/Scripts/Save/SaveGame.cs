using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame
{
    public SaveGame()
    {
        this.saveGameData = new SaveGameData();
    }
    public SaveGame(SaveGameData sgd)
    {
        this.saveGameData = sgd;
    }
    ~SaveGame() { }

    public const string directoryName = "Saves";
    public const string saveName = "savedGame";
    public SaveGameData saveGameData;

    public void SaveToFile()
    {
        if (!Directory.Exists(Application.persistentDataPath + directoryName))
            Directory.CreateDirectory(Application.persistentDataPath + directoryName);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + directoryName + "/" + saveName + ".bin");
        formatter.Serialize(saveFile, saveGameData);

        saveFile.Close();

        // Debug.Log("Game Saved to " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + saveName + ".bin");
    }
}
