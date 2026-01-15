using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public int health = 2;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 10;
    public float activationDelay = 2f; // Delay in seconds

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;
    private bool isActive = false;

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Grab the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.autoBraking = false;

        // Setup rigidbody if present
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Delay zombie activation to allow player time to move
        Invoke("ActivateZombie", activationDelay);
    }

    void ActivateZombie()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            FacePlayer();
            TryAttack();
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }

    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
