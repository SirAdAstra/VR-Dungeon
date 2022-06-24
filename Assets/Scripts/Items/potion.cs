using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potion : MonoBehaviour
{
    [SerializeField] private GameObject healDrip;
    [SerializeField] private Transform dropPoint;

    private void Update()
    {
        if (transform.eulerAngles.x >= 15f && transform.eulerAngles.x <= 90f)
            Instantiate(healDrip, dropPoint.position, Quaternion.identity);
    }
}
