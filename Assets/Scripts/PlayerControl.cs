using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    
	void Start () {
	
	}
	
	void Update () {
        if (SixenseInput.GetController(SixenseHands.RIGHT) != null) {
            if (SixenseInput.GetController(SixenseHands.RIGHT).GetButtonDown(SixenseButtons.TWO)) {
                GetComponent<FlyingController>().enabled = !GetComponent<FlyingController>().enabled;
                GetComponent<RotationController>().enabled = !GetComponent<RotationController>().enabled;
                GetComponent<LiftingBody>().enabled = !GetComponent<LiftingBody>().enabled;
            }
        }
    }
}
