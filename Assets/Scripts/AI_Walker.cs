using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Walker : MonoBehaviour
{
    public float walkPointRange;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private bool canWalk;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        goToPoint();
        animator.SetFloat("Blend", agent.velocity.magnitude / agent.speed);
    }
    void findPoint()
    {
        //print("Getting point");
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        canWalk = Physics.Raycast(walkPoint, -transform.up);
    }
    void goToPoint()
    {
        if (!canWalk)
        {
            findPoint();
        }
        agent.SetDestination(walkPoint);
        if (Vector3.Distance(transform.position, walkPoint) < 3)
        {
            canWalk = false;
        }
    }
}
