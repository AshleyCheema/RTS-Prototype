using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class Unit : MonoBehaviour
{

    public float speed = 20;
    public float turnDst = 5;
    public float turnSpeed = 3;
    public float stoppingDst = 10;

    private NavMeshAgent agent;
    [SerializeField]
    private float reachThreshold = 1.0f;

    [SerializeField]
    private GameObject selectedHighlight;

    Path path;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SelectionManager.instance.allUnits.Add(this);
    }

    private void OnDestroy()
    {
        SelectionManager.instance.allUnits.Remove(this);
    }

    public void UnitSelected(bool isSelected)
    {
        selectedHighlight.SetActive(isSelected);
    }

    public void GetPath(Transform targetLocation)
    {
        Pathfinder.instance.CreatePath(targetLocation);
    }

    public async void MoveUnit(Transform targetLocation)
    {
        agent.destination = targetLocation.position;

        await FirstToDestination();
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos(); 
        }
    }

    private async Task FirstToDestination()
    {
        //Bullied by the Y axis being to high on the capsule. Can be fixed with a lower down axis
        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(agent.destination.x, 0f, agent.destination.z)) > reachThreshold)
        {
            await Task.Yield();
        }

        Pathfinder.instance.FindNewPaths();
    }

}