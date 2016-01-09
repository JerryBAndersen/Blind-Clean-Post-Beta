using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour
{
    /*
        ACHTUNG: Diese Script ist an alle Children manuell angehängt worden.
        Optimierung für das Parentobject kommt später
    */
    public float fade = 0.5f;
    float cuR;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 10); //Aufräumen
    }

    // Update is called once per frame
    void Update()
    {
        //Nach Sichtbarkeit handeln
        if (GetComponent<Renderer>()) {
            cuR = GetComponent<Renderer>().material.GetFloat("_Fade");


            if (cuR <= 0) {

                Destroy(GetComponent<Renderer>());
                Destroy(this);
            }
            else {
                GetComponent<Renderer>().material.SetFloat("_Fade", cuR -= fade * Time.deltaTime);
            }
        }     

    }
}