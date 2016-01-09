using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    public static Manager MANAGERINSTANCE = null;

    public enum mode { FLYING, GLIDING, GROUND, HANGING };
    public mode actMode;

    public mode MovementMode
    {
        get { return actMode; }
        set { actMode = value; }
    }

    void Awake()
    {
        Debug.Log("Manager awaked");
        // Prüfen ob es bereits eine Instanz gibt
        if (MANAGERINSTANCE == null)
        {
            MANAGERINSTANCE = this;
        }
        else if (MANAGERINSTANCE != this)
        {
            Destroy(gameObject);
        }

        // Objekt nicht neuinstanziieren
        DontDestroyOnLoad(gameObject);
    }

    private Manager()
    { }

    // Use this for initialization
    void Start ()
    {
        actMode = mode.GROUND;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Checks if the Razer Hydra is as joystick connected
    public static bool IsHydraConnected()
    {
        bool isConnected = false;

        foreach (string joystick in Input.GetJoystickNames())
        {
            if (joystick == "Razer Hydra")
            {
                isConnected = true;
                break;
            }
        }

        return isConnected;
    }
}
