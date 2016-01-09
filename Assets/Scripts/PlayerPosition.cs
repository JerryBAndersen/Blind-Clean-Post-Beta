using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {
        
	void Start () {
	}

	void FixedUpdate () {
        foreach (BlitArray b in GameObject.FindObjectsOfType<BlitArray>()) {       
            // Pass position Vector to all Shaders
            foreach (Renderer r in GameObject.FindObjectsOfType<Renderer>()) {
                if (r.gameObject.GetComponent<BlitArray>()) {
                    r.material.SetVector("_Origin", GameObject.Find("Main Camera").transform.position);
                }
            }
        }
    }
}
