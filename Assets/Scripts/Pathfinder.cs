using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void FindNewPaths()
    {
        Debug.Log("You have reached your Destination");
    }

}
