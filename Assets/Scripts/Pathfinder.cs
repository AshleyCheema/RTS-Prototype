using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder instance { get; set; }

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

    public Vector3 CreatePath(Transform targetPos)
    {


        return Vector3.zero;
    }

    public void FindNewPaths(Unit unit, Vector3 destination)
    {
        if(!unit.unitAtDestination)
        {
            NavMeshHit hit;
            if(NavMesh.SamplePosition(destination, out hit, 1.0f, NavMesh.AllAreas))
            {
                unit.MoveUnit(hit.position);
            }
        }

        Debug.Log("You have reached your Destination");
    }

}
