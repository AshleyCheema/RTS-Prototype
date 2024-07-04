using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder instance { get; set; }
    private float unitSpacing = 0f;

    private float min = 10f;
    private float max = 15f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void BoxFormation(Vector3 origin, List<Unit> units)
    {
        List<Vector3> endpoints = new List<Vector3>();

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(units.Count));

        for (int i = 0; i < units.Count; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            unitSpacing = Random.Range(min, max);
            Vector3 unitOffset = new Vector3(col * unitSpacing, 0, row * unitSpacing);
            Vector3 endpoint = origin + unitOffset;

            //Debugging purposes
            Vector3 finalPoint = CheckPositionIsAvaliable(endpoint);
            DebugPositions(finalPoint);

            units[i].MoveUnit(CheckPositionIsAvaliable(endpoint));
        }
    }

    public Vector3 CheckPositionIsAvaliable(Vector3 destination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        Debug.Log(hit.position);
        return Vector3.zero;
    }

    private void DebugPositions(Vector3 endpoint)
    {
        List<GameObject> spheres = new List<GameObject>();

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spheres.Add(sphere);
        sphere.transform.position = endpoint;

    }

}
