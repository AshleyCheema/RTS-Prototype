using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {

    GameObject goDes;
    NavMeshAgent xNacMeshAgent;
	// Use this for initialization
	void Start ()
    {
        goDes = GameObject.FindGameObjectWithTag("Player");

        xNacMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void MoveToTarget()
    {
        Vector3 v3TargetPosition = goDes.transform.position;

        xNacMeshAgent.SetDestination(v3TargetPosition);
    }

}
