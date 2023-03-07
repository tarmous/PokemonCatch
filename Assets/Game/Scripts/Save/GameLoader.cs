using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameLoader
{

    public GameLoader(){}
    ~GameLoader(){}
    public const string saveDirectory = "Saves";
    public const string saveName = "savedGame";

    public SaveGameData LoadFromFile()
    {
        if (!File.Exists(Application.persistentDataPath + saveDirectory + "/" + saveName + ".bin")) return new SaveGameData();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + saveDirectory + "/" + saveName + ".bin", FileMode.Open);
        SaveGameData loadData = (SaveGameData)formatter.Deserialize(saveFile);

        saveFile.Close();
        return loadData;
    }
}
