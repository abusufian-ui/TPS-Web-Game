using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;

    [Header("Attack")]
    public float attackDistance = 2f;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    //Heatlh and damage logic would go here 
    float health = 100f;
    public float damageAmount = 20f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("Player not found. Tag your player as 'Player'");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // --- CHASE ---
        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.ResetTrigger("Attacking");
            animator.SetBool("Walking", true);
        }
        // --- ATTACK ---
        else
        {
            agent.isStopped = true;

            FacePlayer();

            animator.SetBool("Walking", false);

            if (canAttack)
            {
                animator.SetTrigger("Attacking");
                canAttack = false;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= damageAmount;
            if (health <= 0)
            {
                Die();

            }
        }
    }

     void Die()
    {
        // 1. Trigger the death animation
        animator.SetTrigger("Die");

        // 2. Stop the enemy from moving or attacking further
        agent.isStopped = true;
        agent.enabled = false; // Optional: Disables navigation entirely

        // 3. Disable the script so it stops running Update logic
        this.enabled = false;

        // 4. Destroy the object after a delay (enough time for animation to play)
        Destroy(gameObject, 3f);
    }
}
