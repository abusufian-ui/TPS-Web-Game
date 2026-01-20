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
}
