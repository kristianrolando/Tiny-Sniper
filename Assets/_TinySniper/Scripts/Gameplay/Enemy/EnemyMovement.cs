using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement 
{
    private GameObject _enemyObj;
    private Transform[] _wayPoints;

    private NavMeshAgent _agent;
    private int currentWayId;

    internal void Initial(NavMeshAgent agent, Transform[] wayPoints, GameObject enemyObj)
    {
        _agent = agent;
        _wayPoints = wayPoints;
        _enemyObj = enemyObj;

        currentWayId = 0;
    }

    internal void PatrolMovement()
    {
        if (Vector3.Distance(_wayPoints[currentWayId].position, _enemyObj.transform.position) <= 1f)
        {
            currentWayId++;
            if (currentWayId >= _wayPoints.Length)
            {
                currentWayId = 0;
            }
        }
        //transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentWayId].position, Time.deltaTime * speed);
        //transform.LookAt(wayPoints[currentWayId].position);
        _agent.SetDestination(_wayPoints[currentWayId].position);
    }
}
