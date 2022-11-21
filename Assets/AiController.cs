using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    [SerializeField]
    private float speedWalk = 1;
    [SerializeField]
    private float speedRun = 2;

    private bool isStunned = false;

    public static event Action<AiController> OnEnemyKilled;
    public float health;
    public float maxHealth = 100;
    public float attack = 5;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public float edgeIterations = 4;
    public float edgeDistance = 0.5f;
    public bool dead = false;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    public Rigidbody m_Rigidbody;
    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
        print("enemy");
        health = maxHealth;
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;
        m_Rigidbody = GetComponent<Rigidbody>();
       

        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        print("tessa");
        print(health);
        if ( health <= 0)
        {
            Stop();
            dead = true;
            isStunned = true;
            print("death");
            animator.SetBool("Stunned Loop", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("Run Forward", false);
            animator.SetBool("Death", true);
           
           
            
        }
    }

    public void death()
    {
        Destroy(gameObject);
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isStunned == false && dead == false)
        {
            EnviromentView();
            if (dead == true)
            {
                speedRun = 0;
                speedWalk = 0;
            }
            else if (!m_IsPatrol)
            {
              
                animator.SetBool("Run Forward", true);
                animator.SetBool("WalkForward", false);
                Chasing();
            }
            else
            {
               
                animator.SetBool("WalkForward", true);
                animator.SetBool("Run Forward", false);
                Patroling();
            }
        }
    }
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isStunned == false)
        {
            print("Attack");
            animator.SetTrigger("Attack3");
        }
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller) && isStunned == false)
        {
            controller.takeDamage(attack);
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            stunned();
        }

    }
    
    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if(!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);

        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if(m_TimeToRotate <= 0)
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
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_TimeToRotate -= Time.deltaTime;
                }
            }
        }
    }
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer=true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime > 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    public void stunned()
    {
        if (dead == false)
        {
            Stop();
            isStunned = true;
            print("stun");
            animator.SetBool("Stunned Loop", true);
            animator.SetBool("WalkForward", false);
            animator.SetBool("Run Forward", false);
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        
    }

    public void unStun()
    {
        isStunned = false;
        print("unstun");
        animator.SetBool("Stunned Loop", false);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = 3.5f;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }

  
   void EnviromentView()
   {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2) ;
            {
                float dstToPlayer = Vector3.Distance(transform.position,player.position);
                if(!Physics.Raycast(transform.position,dirToPlayer,dstToPlayer,obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;

                }
                else
                {
                    m_PlayerInRange =false;
                }
            }
            if(Vector3.Distance(transform.position, player.position)> viewRadius)
            {
                m_PlayerInRange=false;
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
        
   }
}
