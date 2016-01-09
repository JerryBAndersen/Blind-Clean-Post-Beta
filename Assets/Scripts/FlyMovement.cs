using UnityEngine;
using System.Collections;

public class FlyMovement : MonoBehaviour {

    public Transform goal;
    public GameObject target;
    private Mesh cave;
    private NavMeshAgent agent;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        cave = target.GetComponent<Mesh>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    
    void OnTriggerEnter(Collider col)
    {
        print("Trigger!");
        if (col.gameObject.tag == "Finish")
        {
            print("Destination reached!");
            float r = Random.value;

            if(r <= 0.25f)
            {
                goal.transform.position = new Vector3(56, 129, -198);//cave.vertices[(int)Random.value * cave.vertices.Length];
            }
            else if (r >0.25f && r <=0.5f)
            {
                goal.transform.position = new Vector3(31, 55, -198);//cave.vertices[(int)Random.value * cave.vertices.Length];
            }
            else if(r > 0.5f && r <=0.75f)
            {
                goal.transform.position = new Vector3(-112, 90, -95);//cave.vertices[(int)Random.value * cave.vertices.Length];
            }
            else if(r > 0.75f)
            {
                goal.transform.position = new Vector3(188, 90, -268);//cave.vertices[(int)Random.value * cave.vertices.Length];
            }
            agent.destination = goal.transform.position;
            
        }
    }
}
