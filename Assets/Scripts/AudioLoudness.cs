using UnityEngine;
using System.Collections;

public class AudioLoudness : MonoBehaviour {

	public AudioSource source;
	public float thresh = 0.01f;
    public float contrast = 30f;
	int avglength = 4;
	AudioClip clip;
	AudioListener listener;
	float[] samples;
	Queue lastrms;
	float avg;


	// Use this for initialization
	void Start () {
		clip = source.clip;
		listener = GameObject.FindObjectOfType<AudioListener> ();
		lastrms = new Queue ();
	}
	
	// Update is called once per frame
	void Update () {
		float rms = 0f;
		samples = new float[64];
		source.GetOutputData(samples, 0);
		for (int i = 0; i < samples.Length; i++) {
			rms += samples[i];
		}
		rms = Mathf.Sqrt (rms / samples.Length);


		if(!float.IsNaN(rms)){		
			
			avg = 0f;
			lastrms.Enqueue (rms>thresh?rms:0f);
			if (lastrms.Count == avglength) {
				lastrms.Dequeue();
				foreach(float f in lastrms){
					avg += f;
				}
				avg /= avglength;
			}
            foreach (BlitArray b in GameObject.FindObjectsOfType<BlitArray>()) {
                b.input = Mathf.Max(0f, Mathf.Min(1f, avg * contrast));
            }
		}

	}
}
