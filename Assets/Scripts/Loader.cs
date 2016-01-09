using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject manager;

    void Awake()
    {
        Debug.Log("GameLoader awaked");

        // Check if a manager has already been assigned to static variable SoundManager.instance or if it's still null
        if (Manager.MANAGERINSTANCE == null)
        {
            Instantiate(manager);
        }
    }

}
