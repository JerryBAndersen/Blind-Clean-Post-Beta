using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour
{
    public float minimumX = -45F;
    public float maximumX = 45F;

    public float minimumY = -360F;
    public float maximumY = 360F;

    public float minimumZ = -45F;
    public float maximumZ = 45F;

    public float speed = 25;

    private Vector3 angleVector = Vector3.zero;

    private Quaternion originalRotation;

    private SixenseInput.Controller linkerController;
    private SixenseInput.Controller rechterController;

    private Rigidbody rb_main;

    // Unity - Start
    void Start()
    {
        rb_main = GetComponent<Rigidbody>();
        originalRotation = transform.localRotation;
    }

    // Unity - Update
    void Update()
    {
        if (Manager.IsHydraConnected())
        {
            linkerController = SixenseInput.Controllers[(int)SixenseHands.LEFT];
            rechterController = SixenseInput.Controllers[(int)SixenseHands.RIGHT];
        }
    }

    // Unity - FixedUpdate
    void FixedUpdate()
    {
        angleVector = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);

        if (Manager.MANAGERINSTANCE.MovementMode == Manager.mode.FLYING || Manager.MANAGERINSTANCE.MovementMode == Manager.mode.GLIDING)
        {
            MoveToRotation(angleVector);
        }
    }

    // Get the middel rotation of both controller-hands
    private Vector3 GetRotationFromHydra()
    {
        Quaternion middleRotation = Quaternion.identity;
        Vector3 eulerRotation = Vector3.zero;

        if (Manager.IsHydraConnected())
        {
            if ((linkerController != null) && (rechterController != null))
            {
                middleRotation = linkerController.Rotation * rechterController.Rotation * originalRotation;
                eulerRotation = middleRotation.eulerAngles;
            }
        }

        return eulerRotation;
    }

    // Rotates the local transform step by step to the target
    private void MoveToRotation(Vector3 target)
    {
        target = new Vector3(
            ClampAngle(target.x, minimumX, maximumX),
            target.y,
            ClampAngle(target.z, minimumZ, maximumZ)
            );
        Quaternion targetQuat = Quaternion.Euler(target);
        targetQuat = Quaternion.RotateTowards(transform.localRotation, targetQuat, speed * Time.fixedDeltaTime);
        rb_main.MoveRotation(targetQuat);
    }

    // Clamp the given angle into the given min and max
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}