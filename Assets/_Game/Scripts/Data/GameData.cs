using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int Width;
    public int Height;
    public List<int> NumberList;
    public int Score;
    public int Combo;
    public int Turn;
    public GameData()
    {
        Width = 2;
        Height =2;
        NumberList = new List<int>();
        Score = 0;
        Combo = 1;
        Turn = 1;
    }
}
