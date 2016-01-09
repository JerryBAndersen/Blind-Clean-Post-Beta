using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour
{
    public GameObject cave;
    public GameObject player;
    public GameObject fly;
    private bool maySpawn = false;
    private float lastSpawn;
    public float spawnThres = 0.0f;

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("castFly", 2, 2); //Alte regelmäßige Methode

    }

    // Update is called once per frame
    void Update()
    {
      

        //Spawn wenn von Schall getroffen
        BlitArray scr = cave. GetComponent < BlitArray > ();
        
        if (scr.GetIfLit(Vector3.Distance(this.gameObject.transform.position, player.transform.position))>=0.5f && maySpawn)
        {
            
            castFly();
            maySpawn = false;
            lastSpawn = Time.time;
        }

        if (Time.time - lastSpawn >= spawnThres)
        {
            maySpawn = true;
        }

    }


    void castFly()
    {
        Object nFly = Instantiate(fly, this.transform.position, Quaternion.identity);
        //print("Fliege gespawned"+Time.time);
    }
}