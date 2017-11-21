using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    public bool selected = false;
    public bool isLeader = false;
    public int offSet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(selected == true)
        {
            isLeader = true;

            if (isLeader == true)
            {
                
            }
        }
	}
}
