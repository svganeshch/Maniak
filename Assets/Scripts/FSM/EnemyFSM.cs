using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public interface IEnemyState
{
    void EnterState(EnemyBaseState EnemyBase);
    void UpateState(EnemyBaseState EnemyBase);
    void ExitState(EnemyBaseState EnemyBase);
}

public class EnemyFSM : MonoBehaviour
{

}

public class Idle : IEnemyState
{
    public void EnterState(EnemyBaseState EnemyBase)
    {
        EnemyBase.SetVelocity(0.25f);
    }

    public void ExitState(EnemyBaseState EnemyBase)
    {
       
    }

    public void UpateState(EnemyBaseState EnemyBase)
    {
        if (EnemyBase.GetRemainingDistance() > EnemyBase.targetDistance)
        {
            EnemyBase.TransitionToState(EnemyBase.ChaseState);
        }
        else if (EnemyBase.enemy.remainingDistance <= EnemyBase.attackDistance)
        {
            EnemyBase.enemy.stoppingDistance = EnemyBase.attackDistance / 2;
            EnemyBase.SetVelocity(0.15f);

            EnemyBase.TransitionToState(EnemyBase.AttackState);
        }
    }
}

class Chase : IEnemyState
{
    public void EnterState(EnemyBaseState EnemyBase)
    {
        EnemyBase.SetVelocity(0.5f);
    }

    public void ExitState(EnemyBaseState EnemyBase)
    {

    }

    public void UpateState(EnemyBaseState EnemyBase)
    {
        if (EnemyBase.enemy.remainingDistance > EnemyBase.targetDistance)
        {
            //enemy.speed = speed * 2;
            EnemyBase.SetVelocity(0.5f);
        }
        else if (EnemyBase.enemy.remainingDistance < (EnemyBase.targetDistance / 1.4f) && EnemyBase.enemy.remainingDistance >= EnemyBase.attackDistance)
        {
            //enemy.speed = speed;
            EnemyBase.SetVelocity(0.25f);
        }
        else if (EnemyBase.enemy.remainingDistance <= EnemyBase.attackDistance)
        {
            EnemyBase.enemy.stoppingDistance = EnemyBase.attackDistance / 2;
            EnemyBase.SetVelocity(0.15f);

            EnemyBase.TransitionToState(EnemyBase.AttackState);
        }
    }
}

class Attack : IEnemyState
{
    public void EnterState(EnemyBaseState EnemyBase)
    {        
        EnemyBase.animator.SetLayerWeight(1, 1);
        EnemyBase.animator.SetBool("throw", true);
    }

    public void ExitState(EnemyBaseState EnemyBase)
    {
        
    }

    public void UpateState(EnemyBaseState EnemyBase)
    {
        if (EnemyBaseState.isThrown)
        {
            EnemyBaseState.isThrown = false;
            EnemyBase.animator.SetBool("throw", false);
            EnemyBase.animator.SetLayerWeight(1, 0);
            
            EnemyBase.TransitionToState(EnemyBase.ChaseState);
        }
    }
}
