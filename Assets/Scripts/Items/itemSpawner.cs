using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public int chance;

    private void Awake()
    {
        if (Random.Range(0, 101) <= chance)
            Instantiate(items[Random.Range(0,items.Length)], gameObject.transform.position, Quaternion.identity);
    }
}
