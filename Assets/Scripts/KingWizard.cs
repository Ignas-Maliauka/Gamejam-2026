using UnityEngine;

public class KingWizard : EnemyController
{
    private Material kingMaterial;
    bool firstScan = true;
    private GameObject playerK;

    private void Start()
    {
        kingMaterial = GetComponent<Renderer>().material;
        playerK = GameObject.Find("Player");

    }
    void Update()
    {
        if (firstScan) { return; }
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
            if(firstScan)
            {
                firstScan = false;
                GameManager.successfulScan = true;
                agent.SetDestination(Vector3.zero);
                currentState = EnemyStates.Walk;
            }
            agent.speed *= 2;
            kingMaterial.color = Color.yellow;
            Invoke("revertForm", 3f);
            calculatePathAwayFromPlayer(playerK);
        }
    }
    private void revertForm()
    {
        agent.speed /= 2;
        kingMaterial.color = Color.red;
    }
}

