using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] shelfTargets;
    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find shelf targets in the scene by name
        GameObject[] shelfObjects = new GameObject[]
        {
            GameObject.Find("shelf 1 product"),
            GameObject.Find("shelf 2 product"),
            GameObject.Find("shelf 3 product"),
            GameObject.Find("shelf 4 product")
        };

        // Extract transforms and pick a random one
        shelfTargets = new Transform[shelfObjects.Length];
        for (int i = 0; i < shelfObjects.Length; i++)
        {
            if (shelfObjects[i] != null)
                shelfTargets[i] = shelfObjects[i].transform;
        }

        // Choose a random shelf and move to it
        MoveToRandomShelf();
    }

    void MoveToRandomShelf()
    {
        if (shelfTargets.Length == 0 || agent == null) return;

        Transform target = shelfTargets[Random.Range(0, shelfTargets.Length)];
        if (target != null)
        {
            agent.SetDestination(target.position);
            isMoving = true;
        }
    }

    void Update()
    {
        // Stop movement when destination is reached
        if (isMoving && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoving = false;
            }
        }
    }
}
