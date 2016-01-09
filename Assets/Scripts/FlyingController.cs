using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlyingController : MonoBehaviour
{
    public static float THRESHOLDSPRAWL = 300f;

    private SixenseInput.Controller linkerController;
    private SixenseInput.Controller rechterController;

    private GameObject linkeHand;
    private GameObject rechteHand;

    private GameObject linkerFluegel;
    private GameObject rechterFluegel;

    private Rigidbody rb_main;

    private Vector3 lastPosition_links, actPosition_links;
    private Vector3 lastPosition_rechts, actPosition_rechts;

    public Vector3 geschwindigkeit_links, geschwindigkeit_rechts;

    public bool areWingsSprawled = false;
    public bool isPlayerGrounded = false;

    [Header("Flügelkraft-Level")]
    [Range(0.0f, 10.0f)]
    public float forceLevel = 2f;
    [Header("Seite <-> Mitte Kraftverteilung")]
    [Range(0.0f, 100.0f)]
    public float forceDivision = 30f;
    [Header("Dreh-Kraft-Level")]
    [Range(0.0f, 10.0f)]
    public float torqueForce = 0.1f;

    [Header("Sams Scaler")]
    [Range(0.0f, 100.0f)]
    public float factor = 8.0f;
    [Range(0.0f, 100.0f)]
    public float scale = 0.4f;

    private enum Wing { LINKS, RECHTS, BEIDE }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // initialisation and object-finding
    private void Init()
    {
        linkerFluegel = GameObject.Find("Wing-Left");
        rechterFluegel = GameObject.Find("Wing-Right");
        rb_main = gameObject.GetComponent<Rigidbody>();

        if (Manager.IsHydraConnected())
        {
            linkerController = SixenseInput.GetController(SixenseHands.LEFT);
            rechterController = SixenseInput.GetController(SixenseHands.RIGHT);
            linkeHand = GameObject.Find("Hand - Left");
            rechteHand = GameObject.Find("Hand - Right");
        }

        lastPosition_links = actPosition_links = lastPosition_rechts = actPosition_rechts = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn die Hydra angeschlossen und initialisiert ist
        if (Manager.IsHydraConnected() && IsControllerActive())
        {
            // .. wird geprüft, ob die Flügel ausgebreitet sind
            areWingsSprawled = CheckSprawl();
        }
        // Wenn nur die Hydra angeschlossen ist (nicht initialisiert)
        else if (Manager.IsHydraConnected())
        {
            // manchmal verliert sich die Initialisierung, daher hier nochmal..
            linkerController = SixenseInput.GetController(SixenseHands.LEFT);
            rechterController = SixenseInput.GetController(SixenseHands.RIGHT);
            linkeHand = GameObject.Find("Hand - Left");
            rechteHand = GameObject.Find("Hand - Right");
        }

        // Setzen des aktuelles Modis
        SetMode();
    }

    void FixedUpdate()
    {
        // Sind die Flügel ausgebreitet?
        // Wird per Variable geprüft, damit das auch ohne RazerHydra funktioniert
        if (areWingsSprawled)
        {
            // Gleiten
            if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.GLIDING)
            {
                GlideForward();
            }

            // Wenn die Hydra angeschlossen und initialisiert ist
            if (Manager.IsHydraConnected() && IsControllerActive())
            {
                // und der linke Bumper gedrückt ist
                if (linkerController.GetButton(SixenseButtons.BUMPER))
                {
                    // Die Geschwindigkeit berechnen
                    CalcSpeed();
                }
            }
            // Hier werden digitale Werte (Tastatur) abgefragt
            // wird nicht weiter erklärt, da nur für Debug-Zwecke
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    DoWingStroke(Wing.BEIDE);
                    Manager.MANAGERINSTANCE.MovementMode = Manager.mode.FLYING;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        DoWingStroke(Wing.LINKS);
                        Manager.MANAGERINSTANCE.MovementMode = Manager.mode.FLYING;
                    }
                    else
                    {
                        Manager.MANAGERINSTANCE.MovementMode = Manager.mode.GLIDING;
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        DoWingStroke(Wing.RECHTS);
                        Manager.MANAGERINSTANCE.MovementMode = Manager.mode.FLYING;
                    }
                    else
                    {
                        Manager.MANAGERINSTANCE.MovementMode = Manager.mode.GLIDING;
                    }
                }
            }
        }
    }

    // Calculate the actual movement mode. 
    // don't like it here ... 
    private void SetMode()
    {
        if (Manager.MANAGERINSTANCE.MovementMode != Manager.mode.GROUND)
        {
            if (((geschwindigkeit_rechts.y < -.1f) || (geschwindigkeit_links.y < -.1f)) && this.areWingsSprawled)
            {
                Manager.MANAGERINSTANCE.MovementMode = Manager.mode.FLYING;
            }
            if ((geschwindigkeit_rechts.y >= -.1f) && (geschwindigkeit_links.y >= -.1f) && this.areWingsSprawled)
            {
                Manager.MANAGERINSTANCE.MovementMode = Manager.mode.GLIDING;
            }
        }
    }

    // Calculates the Wing-stroke
    // When there is no strokePower given (e.g. per ArrowKeys) the power is 5
    private void DoWingStroke(Wing welcheSeite, float strokePower = 5)
    {
        // Berechnung der Kräfte
        float calcedForceWing = (forceDivision / 100f);
        float calcedForceMid = (1 - calcedForceWing);

        // Beide Flügel.. 
        if (welcheSeite == Wing.BEIDE)
        {
            // Wirken der Kräfte
            rb_main.AddRelativeForce(rb_main.transform.up * (forceLevel * strokePower), ForceMode.Impulse);
        }
        else // .. oder .. 
        {
            // Bestimmen der Seite
            Vector3 sideVector = welcheSeite == Wing.RECHTS ? -Vector3.up : Vector3.up;
            Vector3 wingPositionVector = welcheSeite == Wing.RECHTS ? rechterFluegel.transform.position : linkerFluegel.transform.position;
            Transform wingTransform = welcheSeite == Wing.RECHTS ? rechterFluegel.transform : linkerFluegel.transform;
            // Wirken der Kräfte
            rb_main.AddForceAtPosition(wingTransform.up * ((forceLevel * strokePower / 5) * calcedForceWing), wingPositionVector, ForceMode.Impulse);
            rb_main.AddRelativeForce(rb_main.transform.up * ((forceLevel * strokePower / 10) * (calcedForceMid)), ForceMode.Impulse);
            /// Leichte Drehung hervorrufen
            rb_main.AddRelativeTorque(sideVector * torqueForce, ForceMode.Impulse);
        }
    }

    private void GlideForward()
    {

        rb_main.velocity = Vector3.Lerp(
                rb_main.velocity,
                (rb_main.velocity.magnitude * rb_main.transform.forward * 0.5f) + (0.5f * rb_main.transform.forward * Vector3.Project(rb_main.velocity, rb_main.transform.forward).magnitude),
                2f * Time.fixedDeltaTime);

        //////
        // Neue Methode zum Gleiten von Sam, funktioniert aber nicht, ohne Neigung.
        // @ToDo -> Aerodynamics.cs einbinden
        //////
        /*   float wings = 1f; // - (Input.GetAxis("Left Shoulder") + Input.GetAxis("Right Shoulder")) / 2f;
           rb_main.velocity = rb_main.velocity * (1f - wings) + wings * Vector3.Slerp(rb_main.velocity,
               // this is the projection
               Vector3.Project(rb_main.velocity, rb_main.transform.forward),
               // this value alternates between 0 and 1, depending on the speed of the object
               Mathf.Min(
                   1f, Mathf.Max(0f, Mathf.Pow(scale * rb_main.velocity.magnitude, factor * 1 + wings))
                   )
               );*/
    }

    // Die Geschwindigkeit der Flügelschläge wird immer berechnet.
    // wenn die Kraft eine Schwelle unterschreitet (nach unten - Bewegung), wird die Kraft (absolut) als Flügelschlag ausgelöst
    private void CalcSpeed()
    {
        // Geschwindigkeit linke Seite
        lastPosition_links = actPosition_links;
        actPosition_links = linkeHand.transform.localPosition;
        geschwindigkeit_links = ((actPosition_links - lastPosition_links) / Time.fixedDeltaTime);
        // Geschwindigkeit rechte Seite
        lastPosition_rechts = actPosition_rechts;
        actPosition_rechts = rechteHand.transform.localPosition;
        geschwindigkeit_rechts = ((actPosition_rechts - lastPosition_rechts) / Time.fixedDeltaTime);

        // wenn die Geschwindigkeiten beider Seiten kleiner der Schwelle sind,
        // wird der Wert gemittelt und gleichmäßig auf den Körper übertragen
        if ((geschwindigkeit_links.y < -0.1f) && (geschwindigkeit_rechts.y < -0.1f))
        {
            float middle = Mathf.Abs((geschwindigkeit_links.y + geschwindigkeit_rechts.y) / 2);

            if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.FLYING)
            {
                DoWingStroke(Wing.BEIDE, middle);
            }
        }
        // wenn die Geschwindigkeiten nur einer Seite kleiner einer (anderen!) Schwelle sind, wird die Kraft nur auf eine Seite ausgeübt
        // zusätzlich wird eine leichte Drehung erzeugt
        else
        {
            if (geschwindigkeit_links.y < -0.2f)
            {
                if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.FLYING)
                {
                    DoWingStroke(Wing.LINKS, Mathf.Abs(geschwindigkeit_links.y));
                }
            }
            if (geschwindigkeit_rechts.y < -0.2f)
            {
                if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.FLYING)
                {
                    DoWingStroke(Wing.RECHTS, Mathf.Abs(geschwindigkeit_rechts.y));
                }
            }
        }
    }

    // Diese Funktion gibt nur zurück, ob die Controller weiter als der Threshold auseinander gehalten werden
    private bool CheckSprawl()
    {
        return (Vector3.Distance(linkerController.Position, rechterController.Position) > THRESHOLDSPRAWL);
    }
    // Diese Funktion prüft, ob die Controller auch initialisiert sind
    private bool IsControllerActive()
    {
        return (linkerController != null) && (rechterController != null);
    }
}