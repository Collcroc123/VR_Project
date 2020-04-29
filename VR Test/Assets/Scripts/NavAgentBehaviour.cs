using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 location;
    public Transform player;
    public float speed = 2f;
    private Transform currentDestination;
    private bool isInTrigger = false;
    private int i = 0;
    public GameObject indicatorRed;
    public GameObject indicatorGreen;

    public List<Transform> patrolPoints;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //currentDestination = transform;
        currentDestination = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentDestination = player;
        isInTrigger = true;
        //speed = 7f;
    }

    private void OnTriggerExit(Collider other)
    {
        currentDestination = transform;
        isInTrigger = false;
        //speed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = speed;
        agent.destination = currentDestination.position;
        /*
        if(isInTrigger)
        {
            agent.destination = currentDestination.position;
            return;
        }
        
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.destination = patrolPoints[i].position;
            i = (i + 1) % patrolPoints.Count;
        }
        */
        //transform.LookAt(agent.destination);
        if (GetComponent<Renderer>().IsVisibleFrom(Camera.main))
        {
            speed = 0f;
            indicatorGreen.SetActive(false);
            indicatorRed.SetActive(true);
        }
        else
        {
            speed = 2f;
            indicatorRed.SetActive(false);
            indicatorGreen.SetActive(true);
        }
    }
    /*
    void OnBecameVisible()
    {
        speed = 0f;
        indicatorGreen.SetActive(false);
        indicatorRed.SetActive(true);
    }

    void OnBecameInvisible()
    {
        speed = 1f;
        indicatorRed.SetActive(false);
        indicatorGreen.SetActive(true);
    }
    */
}
