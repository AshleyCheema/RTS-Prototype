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
    public bool unitAtDestination = false;

    [SerializeField]
    private float reachThreshold = 1.0f;
    [SerializeField]
    private GameObject selectedHighlight;

    private bool destinationReached = false;
    private NavMeshAgent agent;


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

    public async void MoveUnit(Vector3 targetLocation)
    {
        agent.destination = targetLocation;

        FirstToDestination();
    }

    private void FirstToDestination()
    {
        //Bullied by the Y axis being to high on the capsule. Can be fixed with a lower down axis
        if (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(agent.destination.x, 0f, agent.destination.z)) < reachThreshold)
        {
            unitAtDestination = true;

            Pathfinder.instance.FindNewPaths(this, agent.destination);
        }

    }

}