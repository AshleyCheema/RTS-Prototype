using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class BoidsManager : MonoBehaviour
{
    public List<Boids> boidList;
    public GameObject boidPrefab;
    public int numOfBoids;
    
	// Use this for initialization
	void Start ()
    {
        boidList = new List<Boids>();
        Collider collider = GetComponent<Collider>();

        for (int i = 0; i < numOfBoids; ++i)
        {
            Vector3 position = new Vector3(
                    Random.value * collider.bounds.size.x,
                    Random.value * collider.bounds.size.y,
                    Random.value * collider.bounds.size.z
                ) - collider.bounds.extents;

            GameObject boids = Instantiate(boidPrefab, transform.position, transform.rotation);
            boids.transform.parent = transform;
            boids.transform.localPosition = position;
            boids.GetComponent<Boids>().SetBoidManager(this);

            boidList.Add(boids.GetComponent<Boids>());
        }
    }

    // Update is called once per frame
    void Update ()
    {
        for(int i = 0; i > boidList.Count; ++i)
        {
           // boidList[i].BoidUpdate();
        }
	}
}
