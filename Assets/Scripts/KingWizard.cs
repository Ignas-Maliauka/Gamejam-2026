using UnityEngine;

public class KingWizard : EnemyController
{
    private Material kingMaterial;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        kingMaterial = GetComponent<Renderer>().material;
    }
    void Update()
    {
        if (currentState == EnemyStates.Idle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                setDestination();
                currentState = EnemyStates.Walk;
            }
        }
        else if (currentState == EnemyStates.Walk)
        {
            if (Vector3.Distance(transform.position, walkTarget) < 3f)
            {
                currentState = EnemyStates.Idle;
                idleTimer = Random.Range(0, 0.2f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Scan"))
        {
            agent.speed *= 2;
            kingMaterial.color = Color.yellow;
            Invoke("revertForm", 3f);
            calculatePathAwayFromPlayer();
        }
    }
    private void revertForm()
    {
        agent.speed /= 2;
        kingMaterial.color = Color.red;
    }
}

