using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Walk
    }
    public float idleTimer;
    private Vector3 walkTarget;
    public EnemyStates currentState = EnemyStates.Idle;

    public NavMeshAgent agent;
    void Update()
    {
        if(currentState == EnemyStates.Idle)
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer <= 0)
            {
                setDestination();
                currentState = EnemyStates.Walk;
            }
        }
        else if(currentState == EnemyStates.Walk)
        {
            if(Vector3.Distance(transform.position, walkTarget) < 1f)
            {
                currentState = EnemyStates.Idle;
                idleTimer = Random.Range(0, 0.2f);
            }
        }
    }
    
    private void setDestination()
    {
        for(int i = 0; i < 1000; i++){
            walkTarget = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            if (agent.SetDestination(walkTarget))
            {
                break;
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.greenYellow;
        Gizmos.DrawSphere(walkTarget, 1f);
    }
}
