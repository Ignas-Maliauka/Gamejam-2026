using UnityEngine;
using UnityEngine.UIElements;

public class KingWizard : EnemyController
{
    bool firstScan = true;
    private GameObject playerK;
    private MaterialPropertyBlock mpbb;

    public bool show = false;
    public bool hide = false;
    float time = 0;

    private void Start()
    {
        mpbb = new MaterialPropertyBlock();

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
    private void LateUpdate()
    {
        if (show)
        {

            time += Time.deltaTime;
            time = Mathf.Clamp01(time);
            float easedT = Mathf.SmoothStep(0f, 1f, time);
            float value = Mathf.Lerp(1f, 0f, easedT);


            for (int i = 0; i < rendererr.Length; i++)
            {

                rendererr[i].GetPropertyBlock(mpbb);
                mpbb.SetFloat("_wizard_coat", value);
                rendererr[i].SetPropertyBlock(mpbb);
            }

            if (time >= 3f)
                show = false;
        }
        if (hide)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp01(time);

            float easedT = Mathf.SmoothStep(0f, 1f, time);
            float value = Mathf.Lerp(0f, 1f, easedT);

            for (int i = 0; i < rendererr.Length; i++)
            {

                rendererr[i].GetPropertyBlock(mpbb);
                mpbb.SetFloat("_wizard_coat", value);
                rendererr[i].SetPropertyBlock(mpbb);
            }

            if (time >= 2.5f)
                hide = false;
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
            startReveal();
            
            agent.speed *= 2;
            Invoke("revertForm", 3f);
            calculatePathAwayFromPlayer(playerK);
        }
    }
    void startReveal()
    {
        hide = false;
        show = true;
        time = 0f;

        for (int i = 0; i < rendererr.Length; i++)
        {
            rendererr[i].GetPropertyBlock(mpbb);
            mpbb.SetFloat("_wizard_coat", 1f); // start value
            rendererr[i].SetPropertyBlock(mpbb);
        }
    }
    void endReveal()
    {
        hide = true;
        show = false;
        time = 0f;

        for (int i = 0; i < rendererr.Length; i++)
        {
            rendererr[i].GetPropertyBlock(mpbb);
            mpbb.SetFloat("_wizard_coat", 0f); // start value
            rendererr[i].SetPropertyBlock(mpbb);
        }
    }
    private void revertForm()
    {
        endReveal();
        time = 0;
        agent.speed /= 2;
    }
}

