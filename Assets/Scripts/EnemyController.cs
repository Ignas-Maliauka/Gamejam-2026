using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Walk
    }
    public float arcAngle;
    public float turnBackAngle;
    public float idleTimer;
    private Vector3 walkTarget;
    public EnemyStates currentState = EnemyStates.Idle;
    public float pointDistanceMultiplier;
    Material material;

    public NavMeshAgent agent;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        material = GetComponent<Renderer>().material;
    }
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
            if(Vector3.Distance(transform.position, walkTarget) < 3f)
            {
                currentState = EnemyStates.Idle;
                idleTimer = Random.Range(0, 0.2f);
            }
        }
    }
    
    private void setDestination()
    {
        for(int i = 0; i < 10; i++){
            walkTarget = GetPointInArc();
            walkTarget.y = 0;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(walkTarget, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(walkTarget);

                return;
            }

        }

        for (int i = 0; i < 100; i++)
        {
            walkTarget = GetPointBehind();
            walkTarget.y = 0;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(walkTarget, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(walkTarget);

                return;
            }
        }
        Debug.Log("Failed to find path for enemy");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.greenYellow;
        Gizmos.DrawSphere(walkTarget, 1f);
    }
    private Vector3 GetPointInArc()
    {
        float angle = Random.Range(-arcAngle, arcAngle);
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
        Vector3 direction = rotation * transform.forward;
        return transform.position + (direction * pointDistanceMultiplier);
    }
    private Vector3 GetPointBehind()
    {
        Vector3 directionToCenter = (Vector3.zero - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCenter.x, directionToCenter.z) * Mathf.Rad2Deg;
        angle += Random.Range(-turnBackAngle, turnBackAngle);
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);

        Vector3 direction = rotation * Vector3.forward;
        return transform.position + (direction * pointDistanceMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Scan"))
        {
            material.color = Color.gray;
            Invoke("revertForm", 3f);
        }
    }
   
    private void revertForm()
    {
        material.color = Color.red;
    }
}


  