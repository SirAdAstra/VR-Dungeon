using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimFollow
{
    [RequireComponent(typeof(AudioSource))]
    public class weapon_script : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Rigidbody _rb;
        [SerializeField] private int minAttackSpeed;
        [SerializeField] private float damage;
        [SerializeField] private AudioClip[] hitSound;
        private bool attack_cd = true;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _rb = GetComponentInParent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (_rb.velocity.magnitude >= minAttackSpeed && attack_cd == true && other.tag != "IgnoreMe")
            {
                StartCoroutine(AttackCooldown());
                _audioSource.PlayOneShot(hitSound[Random.Range(0, 2)]);
                if (other.tag == "enemy")
                {
                    GameObject parent = other.gameObject;
                    while (parent.tag == "enemy")
                        parent = parent.transform.parent.gameObject;

                    IAController enemyController = parent.GetComponentInChildren<IAController>();
                    enemyController.GetDamage(damage * _rb.velocity.magnitude);
                    if (enemyController.canAttack == false)
                        enemyController.blocked = true;
                }

                if (other.tag == "enemy_weapon")
                {
                    GameObject parent = other.gameObject;
                    while (parent.name != "Skeleton(Clone)")
                        parent = parent.transform.parent.gameObject;
                    
                    parent.GetComponentInChildren<IAController>().blocked = true;
                }
            }
        }

        private IEnumerator AttackCooldown()
        {
            attack_cd = false;
            yield return new WaitForSeconds(1f);
            attack_cd = true;
        }
    }
}
