using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Walk
    }
    private GameObject player;
    public float arcAngle;
    public float turnBackAngle;
    public float idleTimer;
    protected Vector3 walkTarget;
    public EnemyStates currentState = EnemyStates.Idle;
    public float pointDistanceMultiplier;
    public SkinnedMeshRenderer[] rendererr;
    private MaterialPropertyBlock mpb;
    float t = 0;
    bool animate = false;
    bool revert = false;

    public NavMeshAgent agent;
    private void Start()
    {
        mpb = new MaterialPropertyBlock();

        player = GameObject.Find("Player");
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
    void Update()
    {
        if (animate)
        {

            t += Time.deltaTime;
            t = Mathf.Clamp01(t);

            Vector2 value = Vector2.Lerp(
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0f),
                t
            );

            for (int i = 0; i < rendererr.Length; i++)
            {
                rendererr[i].GetPropertyBlock(mpb);
                mpb.SetVector("_Vector2", value);
                rendererr[i].SetPropertyBlock(mpb);
            }

            if (t >= 1f)
                animate = false;
        }
        if (revert)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);

            Vector2 value = Vector2.Lerp(
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0.5f),
                t
            );

            for (int i = 0; i < rendererr.Length; i++)
            {
                rendererr[i].GetPropertyBlock(mpb);
                mpb.SetVector("_Vector2", value);
                rendererr[i].SetPropertyBlock(mpb);
            }

            if (t >= 1f)
                revert = false;
        }
        if (currentState == EnemyStates.Idle)
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
    
    protected void setDestination()
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
    protected Vector3 GetPointInArc()
    {
        float angle = Random.Range(-arcAngle, arcAngle);
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
        Vector3 direction = rotation * transform.forward;
        return transform.position + (direction * pointDistanceMultiplier);
    }
    protected Vector3 GetPointFromPlayer(GameObject player)
    {
        Vector3 directionFromPlayer = (transform.position - player.transform.position).normalized;
        float angle = Mathf.Atan2(directionFromPlayer.x, directionFromPlayer.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
        Vector3 direction = rotation * transform.forward;
        return transform.position + (direction * pointDistanceMultiplier);
    }
    protected Vector3 GetPointBehind()
    {
        Vector3 directionToCenter = (Vector3.zero - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCenter.x, directionToCenter.z) * Mathf.Rad2Deg;
        angle += Random.Range(-turnBackAngle, turnBackAngle);
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);

        Vector3 direction = rotation * Vector3.forward;
        return transform.position + (direction * pointDistanceMultiplier);
    }
    protected void calculatePathAwayFromPlayer(GameObject player)
    {
        for (int i = 0; i < 10; i++)
        {
            walkTarget = GetPointFromPlayer(player);
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Scan"))
        {
            t = 0;

            revert = false;
            animate = true;
            agent.speed *= 2;
            Invoke("revertForm", 3f);
            calculatePathAwayFromPlayer(player);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
   
    private void revertForm()
    {
        revert = true;
        animate = false;
        t = 0;
        agent.speed /= 2;
        gameObject.GetComponent<Collider>().enabled = true;

    }
}


  