using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon_particles : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private int minAttackSpeed;
    [SerializeField] private GameObject particles;
    private bool attack_cd = true;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_rb.velocity.magnitude >= minAttackSpeed && attack_cd == true)
        {
            StartCoroutine(AttackCooldown());
            ContactPoint contact = collision.contacts[0];
            Instantiate(particles, contact.point, Quaternion.identity);

        }
    }

    private IEnumerator AttackCooldown()
        {
            attack_cd = false;
            yield return new WaitForSeconds(1f);
            attack_cd = true;
        }
}
