using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    public float speed = 20;
    public float turnDst = 5;
    public float turnSpeed = 3;
    public float stoppingDst = 10;

    private NavMeshAgent agent;

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

    public void MoveUnit(Transform targetLocation)
    {
        agent.destination = targetLocation.position;
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos(); 
        }
    }

}