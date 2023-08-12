using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseState : MonoBehaviour
{
    public float speed = 5f;
    public float targetDistance = 10f;
    public float attackDistance = 5f;
    public Transform player;

    public static bool isThrown = false;
    public float throwForce;
    private Injection injection;
    public GameObject InjPrefab;
    public Transform InjHoldPos;
    public GameObject InjectionObj;

    [HideInInspector]
    public NavMeshAgent enemy;
    [HideInInspector]
    public Animator animator;

    public IEnemyState currentState;
    public IEnemyState Idlestate = new Idle();
    public IEnemyState ChaseState = new Chase();
    public IEnemyState AttackState = new Attack();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        injection = FindAnyObjectByType<Injection>();

        AddTagRecursively(transform, "Enemy");
        TransitionToState(Idlestate);
        InstantiateInj();
    }

    void Update()
    {
        currentState.UpateState(this);

        enemy.SetDestination(player.position);
        //transform.LookAt(player.position);
    }

    public void TransitionToState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public float GetRemainingDistance()
    {
        return enemy.remainingDistance;
    }
    public void SetVelocity(float velocity)
    {
        animator.SetFloat("Velocity", velocity);
    }

    public IEnumerator IncrementValue()
    {
        float throwForceIncrement = 10f;
        float throwForceMax = 1000f;

        float elapsedTime = 0f;
        float timeInterval = 0.8f;

        while (elapsedTime < timeInterval)
        {
            throwForce = Mathf.Lerp(0, throwForceMax, elapsedTime / timeInterval);
            //Debug.Log("Current value: " + throwForce);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        throwForce = throwForceMax;
        //Debug.Log("Final value: " + throwForce);
    }

    public void InstantiateInj()
    {
        InjectionObj = injection.InstantiateInj(InjPrefab, InjHoldPos, player.transform);
    }

    void AddTagRecursively(Transform trans, string tag)
    {
        trans.gameObject.tag = tag;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }

    public void ThrowInj()
    {
        InjectionObj.TryGetComponent<Injection>(out injection);
        injection.Throw(player.transform, 1000);
        InjectionObj.transform.parent = null;
        InstantiateInj();
        isThrown = true;

        Debug.Log("Inj throw done");
    }
}
