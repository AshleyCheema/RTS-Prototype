using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitS : MonoBehaviour
{
    //NavMeshAgent _agent;
    //float maxSpeed = 30;
    //float minSpeed = 3.5f;
    public bool IsSelected = false;
    private GameObject goDes;
    private NavMeshAgent xNacMeshAgent;
    private Color isSelectedColor = Color.red;
    private Color notSelectedColor = Color.white;
    private Renderer rend;


    private void Start()
    {
        goDes = GameObject.FindGameObjectWithTag("Player");
        xNacMeshAgent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(IsSelected == true)
        {
            rend.material.color = isSelectedColor;
        }
        else
        {
            rend.material.color = notSelectedColor;
        }
    }

    /// <summary> Move the local Vector3 dst position and move 
    /// the unit towards it
    /// </summary>
    public void MoveToTarget()
    {
        Vector3 v3TargetPosition = goDes.transform.position;

        xNacMeshAgent.SetDestination(v3TargetPosition);
    }
}
