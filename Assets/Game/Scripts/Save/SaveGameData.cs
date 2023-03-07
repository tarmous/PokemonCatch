using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveGameData
{
    public SaveGameData()
    {
        currentTeam = new List<Pokemon>();
    }

    public SaveGameData(List<Pokemon> p)
    {
        currentTeam = p;
    }
    public List<Pokemon> currentTeam;
}
