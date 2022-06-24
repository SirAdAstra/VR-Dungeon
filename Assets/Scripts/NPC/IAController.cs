using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AnimFollow
{
    public class IAController : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public float startWaitTime = 4;
        public float timeToRotate = 2;
        public float speedWalk = 6;
        public float speedRun = 9;
        public float rotationSpeed = 10f;

        public float health = 250;
        public int damage = 10;
        public GameObject weapon;
        public Animator anim;
        public PlayerController playerController;
        public RagdollControl_AF ragdollControl;
        public bool blocked = false;
        public bool canAttack = true;
        private bool dead = false;
        
        public AudioSource audioSource;
        public AudioClip attackSound;
        public AudioClip[] footstepSouds;
        public AudioClip gotHitSound;
        public AudioClip deathSound;
        [SerializeField] private float baseStepSpeed = 0.5f;
        private float footstepTimer = 0;
    
        public float vievRadius = 15;
        public float vievAngle = 90;
        public LayerMask playerMask;
        public LayerMask obstacleMask;
        public float meshResolution = 1f;
        public int edgeIterations = 4;
        public float edgeDistance = 0.5f;
    
        public List<Transform> waypointsList;
        private int m_currentWaypointIndex;
    
        private Vector3 playerLastPosition = Vector3.zero;
        private Vector3 m_PlayerPosition;
    
        private float m_WaitTime;
        private float m_TimeToRotate;
        private bool m_PlayerInRange;
        private bool m_PlayerNear;
        private bool m_IsPatrol;
        private bool m_CaughtPlayer;
    
        private PlayerMovement_AF movement_AF;
    
        private void Start()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            movement_AF = GetComponent<PlayerMovement_AF>();
            audioSource = GetComponent<AudioSource>();
            m_PlayerPosition = Vector3.zero;
            m_IsPatrol = true;
            m_CaughtPlayer = false;
            m_PlayerInRange = false;
            m_WaitTime = startWaitTime;
            m_TimeToRotate = timeToRotate;
    
            waypointsList = GetComponentInParent<EnemySpawnpoint>().waypoints;
            m_currentWaypointIndex = 0;
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            navMeshAgent.isStopped = false;
            //navMeshAgent.speed = speedWalk;
            movement_AF.speed = speedWalk;
            navMeshAgent.SetDestination(waypointsList.ToArray()[m_currentWaypointIndex].position);
            FaceTarget(waypointsList.ToArray()[m_currentWaypointIndex].position);
        }
    
        private void Update()
        {
            EnviromentViev();
    
            if (!m_IsPatrol)
            {
                Chasing();
            }
            else
            {
                Patroling();
            }

            HandleFootsteps();
        }
    
        private void Chasing()
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
    
            if (!m_CaughtPlayer)
            {
                Move(speedRun);
                navMeshAgent.SetDestination(m_PlayerPosition);
                FaceTarget(m_PlayerPosition);
            }
    
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;
                    anim.ResetTrigger("battle");
                    Move(speedWalk);
                    m_TimeToRotate = timeToRotate;
                    m_WaitTime = startWaitTime;
                    navMeshAgent.SetDestination(waypointsList.ToArray()[m_currentWaypointIndex].position);
                    FaceTarget(waypointsList.ToArray()[m_currentWaypointIndex].position);
                }
                else
                {
                    if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).position) <= 2f)
                    {
                        //Debug.Log(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).position));
                        anim.SetTrigger("battle");
                        Stop();
                        m_WaitTime -= Time.deltaTime;
                        if (canAttack == true && ragdollControl.falling == false && ragdollControl.gettingUp == false)
                            StartCoroutine(Attack());
                    }
                }
            }
        }
    
        private void Patroling()
        {
            if (m_PlayerNear)
            {
                if (m_TimeToRotate <= 0)
                {
                    Move(speedWalk);
                    LookingPlayer(playerLastPosition);
                }
                else
                {
                    Stop();
                    m_TimeToRotate -= Time.deltaTime;
                }
            }
            else
            {
                m_PlayerNear = false;
                playerLastPosition = Vector3.zero;
                navMeshAgent.SetDestination(waypointsList.ToArray()[m_currentWaypointIndex].position);
                FaceTarget(waypointsList.ToArray()[m_currentWaypointIndex].position);

                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (m_WaitTime <= 0)
                    {
                        NextPoint();
                        Move(speedWalk);
                        m_WaitTime = startWaitTime;
                    }
                    else
                    {
                        Stop();
                        m_WaitTime -= Time.deltaTime;
                    }
                }
            }
        }
    
        public void NextPoint()
        {
            m_currentWaypointIndex = (m_currentWaypointIndex + 1) % waypointsList.ToArray().Length;
            navMeshAgent.SetDestination(waypointsList.ToArray()[m_currentWaypointIndex].position);
            FaceTarget(waypointsList.ToArray()[m_currentWaypointIndex].position);
        }
    
        private void Move(float speed)
        {
            navMeshAgent.isStopped = false;
            //navMeshAgent.speed = speed;
            movement_AF.speed = speed;
        }
    
        private void Stop()
        {
            navMeshAgent.isStopped = true;
            //navMeshAgent.speed = 0;
            movement_AF.speed = 0;
        }
    
        private void CaughtPlayer()
        {
            m_CaughtPlayer = true;
        }
    
        private void LookingPlayer(Vector3 player)
        {
            navMeshAgent.SetDestination(player);
            
            if (Vector3.Distance(transform.position, player) >= 2)
            {
                if (m_WaitTime <= 0)
                {
                    m_PlayerNear = false;
                    Move(speedWalk);
                    navMeshAgent.SetDestination(waypointsList.ToArray()[m_currentWaypointIndex].position);
                    FaceTarget(waypointsList.ToArray()[m_currentWaypointIndex].position);
                    m_WaitTime = startWaitTime;
                    m_TimeToRotate = timeToRotate;
                }
                else
                {
                    Stop();
                    //FaceTarget(player);
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    
        private void EnviromentViev()
        {
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, vievRadius, playerMask);
    
            for (int i = 0; i < playerInRange.Length; i++)
            {
                Transform player = playerInRange[i].transform;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                
                if ((Vector3.Angle(transform.forward, dirToPlayer) < vievAngle / 2) || Vector3.Distance(transform.position, player.position) <= 3)
                {
                    float dstToPlayer = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    {
                        m_PlayerInRange = true;
                        m_IsPatrol = false;
                    }
                    else
                    {
                        m_PlayerInRange = false;
                    }
                }
    
                if (Vector3.Distance(transform.position, player.position) > vievRadius)
                {
                    m_PlayerInRange = false;
                }
    
                if (m_PlayerInRange)
                {
                    m_PlayerPosition = player.transform.position;
                }
            }
        }

        private void FaceTarget(Vector3 destination)
        {
            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);  
        }

        public void GetDamage(float incomigDamage)
        {
            StopAllCoroutines();
            StartCoroutine(GetDamageCor(incomigDamage));
        }

        private IEnumerator GetDamageCor(float incomigDamage)
        {
            canAttack = false;
            if (!dead)
            {
                health -= incomigDamage;
                audioSource.PlayOneShot(gotHitSound);
                anim.SetTrigger("Hit");
                if (health <= 0)
                {
                    dead = true;
                    ragdollControl.shotInHead = true;
                    audioSource.PlayOneShot(deathSound);
                    Instantiate(weapon, gameObject.transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(3f);
                anim.ResetTrigger("Hit");
            }
            canAttack = true;
        }

        private IEnumerator Attack()
        {
            canAttack = false;
            blocked = false;
            int attackNumber = Random.Range(1, 4);
            anim.SetInteger("attack", attackNumber);
            yield return new WaitForSeconds(2f);
            if (!blocked && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).position) <= 2f)
            {
                playerController.RecieveDamage(damage);
                audioSource.PlayOneShot(attackSound);
            }
            anim.SetInteger("attack", 0);
            yield return new WaitForSeconds(2f);
            blocked = false;
            canAttack = true;
        }

        private void HandleFootsteps()
        {
            if (movement_AF.inhibitMove == true) return;
            if (movement_AF.speed == 0) return;

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                audioSource.PlayOneShot(footstepSouds[Random.Range(0, footstepSouds.Length)]);
                footstepTimer = baseStepSpeed;
            }
        }
    }
}
