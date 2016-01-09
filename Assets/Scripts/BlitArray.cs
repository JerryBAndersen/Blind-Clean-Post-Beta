using UnityEngine;
using System.Collections;

public class BlitArray : MonoBehaviour
{
    Texture2D tex;
    public float[] vals;
    public RenderTexture rtex;
    [Range(0f, 1f)]
    public float input;
    public float maxdistance = 256f;
    float time = 0f;
    int oldtime = 0;
    
    void Start()
    {
        input = 0f;
        gameObject.GetComponent<Renderer>().material.SetFloat("_VisibleDistance", maxdistance);
        if (vals.Length != rtex.height)
        {
            vals = new float[rtex.height];
        }

        tex = new Texture2D(1, rtex.height);
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_VisibleDistance", maxdistance);

        for (int i = 0; i < tex.height; i++)
        {
            // assign the next value to this one
            if (i < vals.Length - 1)
            {
                vals[i] = vals[i + 1];
            }
            else
            {
                vals[i] = input;
            }
            tex.SetPixel(0, tex.height - i, new Color(vals[i], vals[i], vals[i]));
        }
        tex.Apply();
        Graphics.Blit(tex, rtex);
    }

    public float GetIfLit(float distance)
    {
        distance = Mathf.Max(Mathf.Min(distance,maxdistance),0f);
        int pointinarray = 255 - (int)(distance / maxdistance * (vals.Length-1));
        if (vals[pointinarray] > 0.5f)
        {

            return vals[pointinarray];
        }
        else
        {
            return 0f;
        }


    }

}