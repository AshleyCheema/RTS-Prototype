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
    GameObject goDes;
    NavMeshAgent xNacMeshAgent;

    private void Start()
    {
        //_agent = GetComponent<NavMeshAgent>();

        goDes = GameObject.FindGameObjectWithTag("Player");

        xNacMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(IsSelected == true)
        {
            //MoveToTarget();
        }
    }

    /// <summary> Move the loal Vector3 dst position and move 
    /// the unit towards it
    /// </summary>
    public void MoveToTarget()
    {
        Vector3 v3TargetPosition = goDes.transform.position;

        xNacMeshAgent.SetDestination(v3TargetPosition);
    }
}
