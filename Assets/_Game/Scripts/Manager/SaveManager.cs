using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoInstance<SaveManager>
{
    public void SaveGameData(GameData gameData)
    {
        var json = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString("GameData", json);
    }
    public GameData LoadData()
    {
        GameData gameData = new GameData();
        if(PlayerPrefs.HasKey("GameData"))
        {
            var json = PlayerPrefs.GetString("GameData");
            gameData = JsonConvert.DeserializeObject<GameData>(json);
        }
        return gameData;
    }
    public bool HasSaveGameData()
    {
        if(PlayerPrefs.HasKey("GameData"))
        {
            var json = PlayerPrefs.GetString("GameData");
            var gameData = JsonConvert.DeserializeObject<GameData>(json);
            return !gameData.NumberList.All(num => num == -1);
        }
        else
        {
            return false;
        }
    }
}
