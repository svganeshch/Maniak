using UnityEngine;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    EnemyBaseState enemyController;
    HealthController healthController;
    NavMeshAgent navMeshAgent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyBaseState>();
        healthController = GetComponent<HealthController>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //foreach (var rigidbody in rigidbodies)
        //{
        //    var rbhitbox = rigidbody.gameObject.AddComponent<HitBox>();
        //    rbhitbox.healthController = healthController;

        //}

        DeActivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        healthController.enabled = false;
        animator.enabled = false;
        enemyController.enabled = false;
        navMeshAgent.enabled = false;
        enabled = false;
    }

    public void DeActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector3.zero;
            Vector3 wCOM = rigidbody.worldCenterOfMass;
            wCOM = Vector3.zero;

        }
        animator.enabled = true;
    }

    public void ApplyForce(Vector3 force)
    {
        var rb = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}