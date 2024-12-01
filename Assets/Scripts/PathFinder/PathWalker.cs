using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWalker : MonoBehaviour
{

    private const float speed = 20;
    int currentPathIndex;
    private List<Vector3> pathVectorList;

    public PathFinder pathFinder;


    private void Update()
    {
        if (Input.GetKey(KeyCode.P)) SetTargetPosition();

        HandleMovement();
    }

    public void SetTargetPosition()
    {
        currentPathIndex = 0;
        pathVectorList = pathFinder.FindPath(3, 0, 28, 25);
        transform.position = pathFinder.GetWorldPosition(pathVectorList[0]);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    public void HandleMovement()
    {
        if (pathVectorList == null) return;

        Vector3 targetPosition = pathFinder.GetWorldPosition(pathVectorList[currentPathIndex]);
        if (Vector3.Distance(transform.position, targetPosition) > 0.1)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
        }
        else
        {
            currentPathIndex++;
            if (currentPathIndex >= pathVectorList.Count)
            {
                StopMoving();
            }
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }
}
