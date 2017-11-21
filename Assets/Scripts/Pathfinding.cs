using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;

public class Pathfinding : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    Debug.Log("Path " + sw.ElapsedMilliseconds);

                    pathSuccess = true;
                    break;
                }

                /*
                RaycastHit hit;
                if(Physics.Raycast(currentNode.worldPosition, targetNode.worldPosition - currentNode.worldPosition, out hit, 1000))
                {
                    float disToTarget = Vector3.Distance(currentNode.worldPosition, targetNode.worldPosition);
                    float pointDis = Vector3.Distance(currentNode.worldPosition, hit.point);
                    if(disToTarget < pointDis)
                    {
                        Debug.Break();
                    }
                }
                else
                {
                    currentNode.parent = openSet.GetLast();
                    openSet.Add(currentNode);

                    currentNode = targetNode;
                    currentNode.parent = openSet.GetLast();
                    openSet.Add(targetNode);

                    sw.Stop();
                    Debug.Log("Path " + sw.ElapsedMilliseconds);

                    pathSuccess = true;
                    break;
                }
                */

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }

            }
        }
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (disX > disY)
            return 14 * disY + 10 * (disX - disY);
            return 14 * disX + 10 * (disY - disX);
    }
}
