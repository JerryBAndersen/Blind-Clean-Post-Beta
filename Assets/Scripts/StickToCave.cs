using UnityEngine;
using System.Collections;

public class StickToCave : MonoBehaviour
{

    Rigidbody rigid;
    FlyingController flyCtrl;

    // Use this for initialization
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        flyCtrl = gameObject.GetComponentInParent<FlyingController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Verhalten an der Wand
        if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.GROUND)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Brauchen wir nur, wenn die Rift nicht angeschlossen ist
            MovementManagement(h, v);
            if (IsControllerActive())
            {
                if ((Manager.MANAGERINSTANCE.MovementMode == Manager.mode.GROUND) && !SixenseInput.GetController(SixenseHands.RIGHT).GetButton(SixenseButtons.TRIGGER))
                {
                    rigid.useGravity = true;
                    // flyCtrl.IsPlayerGrounded    = false;
                    Manager.MANAGERINSTANCE.MovementMode = Manager.mode.FLYING;
                }
            }
        }
    }

    void MovementManagement(float horizontal, float vertical)
    {
        if (horizontal != 0f || vertical != 0f)
        {
            Rotating(horizontal, vertical);

        }
    }

    void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

        Quaternion newRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        rigid.MoveRotation(newRotation);
    }

    void OnTriggerStay(Collider other)
    {
        if (IsControllerActive())
        {
            if (other.gameObject.tag == "Cave" && SixenseInput.GetController(SixenseHands.RIGHT).GetButton(SixenseButtons.TRIGGER))
            {
                // flight-script deaktivieren
                //flyCtrl.IsPlayerGrounded = true;
                Manager.MANAGERINSTANCE.MovementMode = Manager.mode.GROUND;

                // Alle Achsenmächte auf 0 setzen
                rigid.velocity = Vector3.zero;

                // an der Wand "festhalten";
                rigid.useGravity = false;
            }
        }
    }

    private bool IsControllerActive()
    {
        return Manager.IsHydraConnected() && SixenseInput.GetController(SixenseHands.RIGHT) != null && SixenseInput.GetController(SixenseHands.LEFT) != null;
    }
}