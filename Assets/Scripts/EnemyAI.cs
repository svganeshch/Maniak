using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float speed = 5;
    public float targetDistance = 10f;

    private NavMeshAgent enemy;
    private Animator enemyAnimator;

    public Transform player;

    private float velocity;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        enemy.SetDestination(player.position);

        if (enemy.remainingDistance > targetDistance)
        {
            enemy.speed = speed * 2;
            velocity = 0.5f;

            enemyAnimator.SetBool("throw", false);
        }
        else if (enemy.remainingDistance < targetDistance && enemy.remainingDistance >= enemy.stoppingDistance)
        {
            enemy.speed = speed;
            velocity = 0.25f;

            enemyAnimator.SetBool("throw", false);
        }
        else if (enemy.remainingDistance <= enemy.stoppingDistance)
        {
            velocity = 0;
            Attack();
        }

        enemyAnimator.SetFloat("Velocity", velocity);

        //Debug.Log("Enemy distance : " + enemy.remainingDistance);

        //Debug.Log("Enemy accel : " + enemy.acceleration);
    }

    public void Attack()
    {
        //if (enemyAnimator.GetLayerWeight(1) != 1)
        //    enemyAnimator.SetLayerWeight(1, Mathf.Lerp(0, 1, 1));

        //enemyAnimator.SetFloat("ThrowForce",  0.5f);
        
        if (enemyAnimator.GetBool("throw") != true)
            enemyAnimator.SetBool("throw", true);

    }
}
