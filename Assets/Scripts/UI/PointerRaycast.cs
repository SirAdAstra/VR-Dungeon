using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerRaycast : MonoBehaviour
{
    public class UnityEvent : UnityEvent<bool> { }

    private RaycastHit hit;
    public bool hitUI;

    private void Update()
    {
        Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, 5f);
        if (hit.transform.tag == "UI")
        {
            hitUI = true;
        }
        else
        {
            hitUI = false;
        }
    }
}
