using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZoneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject myZone;

    [SerializeField] private GameObject[] zonePrefabs;
    [SerializeField] private GameObject[] stairsPrefabs;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject finalZone;
    [SerializeField] private GameObject deadEnd;
    [SerializeField] private Transform[] exitPoints;
    private ZoneCounter zoneCounter;

    private void Awake()
    {
        zoneCounter = GameObject.FindGameObjectWithTag("startRoom").GetComponent<ZoneCounter>();
        SpawnNewZone(exitPoints[Random.Range(0, exitPoints.Length)]);
    }

    private void SpawnNewZone(Transform exitPoint)
    {
        Transform temp = exitPoint;
        exitPoint.tag = "used";
        exitPoint.gameObject.SetActive(false);
        Destroy(exitPoint.gameObject);
        if (zoneCounter.currentZones >= zoneCounter.maxZones - 1)
        {
            if (zoneCounter.currentFloor < zoneCounter.maxFloors)
            {
                zoneCounter.currentFloor++;
                zoneCounter.currentZones = 1;
                zoneCounter.currentTreasures = 1;
                CloseExits();
                Instantiate(stairsPrefabs[Random.Range(0, stairsPrefabs.Length)], temp.position, temp.rotation);
            }
            else
            {
                zoneCounter.currentZones++;
                Instantiate(finalZone, temp.position, temp.rotation);
                CloseExits();
            }
        }
        else
        {
            zoneCounter.currentZones++;
            Instantiate(zonePrefabs[Random.Range(0, zonePrefabs.Length)], temp.position, temp.rotation);
        }
        
    }

    private void CloseExits()
    {
        GameObject[] exits = GameObject.FindGameObjectsWithTag("exitPoint");
        foreach (GameObject point in exits)
        {
            point.tag = "used";
            if (Random.Range(0, 100) <= 10 && zoneCounter.currentTreasures < zoneCounter.maxTreasures)
            {
                zoneCounter.currentTreasures++;
                Instantiate(treasurePrefab, point.transform.position, point.transform.rotation);
            }
            else
                Instantiate(deadEnd, point.transform.position, point.transform.rotation);
            point.SetActive(false);
            Destroy(point);
        }
    }
}
