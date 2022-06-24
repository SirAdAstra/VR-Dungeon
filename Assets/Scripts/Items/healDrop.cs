using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healDrop : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Lifetime());
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.root.GetComponentInChildren<PlayerController>().Heal();
            Destroy(gameObject);
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
