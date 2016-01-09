using UnityEngine;
using System.Collections;

public class VelocityWind : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject w in GameObject.FindGameObjectsWithTag("Wind")) {
            w.GetComponent<AudioSource>().volume = w.GetComponent<AudioSource>().volume * .9f + .1f * Mathf.Min(Mathf.Max(Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude / 50f, 3f), 0f), 0.8f);
        }
    }
}
