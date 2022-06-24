using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimFollow
{
    public class EnemySpawnpoint : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemy;
        [SerializeField] public List<Transform> waypoints;
        private GameObject newEnemy;

        private void Awake()
        {
            if (Random.Range(0, 2) == 1)
                newEnemy = Instantiate(enemy[Random.Range(0, enemy.Length)], gameObject.transform);
        }
    }
}