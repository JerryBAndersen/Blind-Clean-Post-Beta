using UnityEngine;
using System.Collections;

public class FixedFollow : MonoBehaviour {

    public GameObject target;

	void Update () {
        transform.position = target.transform.position;
	}
}
