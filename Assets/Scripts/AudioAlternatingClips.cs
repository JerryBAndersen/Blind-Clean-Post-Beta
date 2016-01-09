using UnityEngine;
using System.Collections;

public class AudioAlternatingClips : MonoBehaviour {

    public string Button;
    public string Axis;
    public AudioClip[] audioclips;
	public AudioSource source;
    bool passed = false;

	// Use this for initialization
	void Start () {
		source.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Manager.IsHydraConnected() && (SixenseInput.GetController(SixenseHands.RIGHT) != null))
        {
            if (Button != "")
            {
                if (!source.isPlaying && (Input.GetButton(Button) || SixenseInput.GetController(SixenseHands.RIGHT).GetButton(SixenseButtons.BUMPER)))
                {
                    source.clip = audioclips[(int)(Random.value * audioclips.Length)];
                    source.Play();
                }
            }
            if (Axis != "")
            {

                if (!source.isPlaying && (Input.GetAxis(Axis) < 0.8f && !passed))
                {
                    source.clip = audioclips[(int)(Random.value * audioclips.Length)];
                    source.Play();
                    passed = !passed;
                }
                if (Input.GetAxis(Axis) > 0.8f && passed)
                {
                    passed = !passed;
                }
            }
        }
	}
}
