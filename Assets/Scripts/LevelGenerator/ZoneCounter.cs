using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCounter : MonoBehaviour
{
    public int maxZones;
    public int maxStairs;
    public int maxTreasures;
    public int maxFloors;
    public int currentZones = 1;
    public int currentStairs = 0;
    public int currentTreasures = 0;
    public int currentFloor = 1;

    private void Start()
    {
        maxZones = PlayerPrefs.GetInt("room");
        maxFloors = PlayerPrefs.GetInt("floor");
    }
}
