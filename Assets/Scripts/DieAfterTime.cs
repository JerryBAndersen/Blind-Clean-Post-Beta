using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {


    public float countDown = 10;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, countDown);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
