using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This scripts handles all logic related to spaceship movement and physics
/// Temporary Manual: The mouse handles the majority of the direction.
/// Q to move right, E to move left, W to move forwards, D to move backwards. A and D to roll. Left ctrl move down, space move up. Shift to boost.
/// </summary>
[RequireComponent(typeof(Rigidbody))] //All additional ships will require a rigidbody
public class SpaceShipController : MonoBehaviour
{
    [System.Serializable]
    public class OnBoostAmountChangedEvent : UnityEvent<float, float> {}

    [Header("Ship Movement Settings")]
    [SerializeField]
    private float yawTorque = 500f; //yaw = rotate left and right
    [SerializeField]
    private float pitchTorque = 1000f; //pitch = look up and down movement (nose up and down)
    [SerializeField]
    private float rollTorque = 1000f; //roll = ship roll on its own z axis

    //Thrusts handle the speed of movements
    [SerializeField]
    private float thrust = 100f; //fowards movement
    [SerializeField]
    private float upThrust = 1000f; //up and down movements
    [SerializeField]
    private float strafeThrust = 1000f; //left to right movements

    [Header("Ship Boosting Settings")]
    [SerializeField]
    private float maxBoostAmount = 2f;
    [SerializeField]
    private float boostDeprecationRate = 0.25f; //how fast we ran out of boost
    [SerializeField]
    private float boostRechargeRate = 0.5f;
    [SerializeField]
    private float boostMultiplier = 5f;
    [SerializeField]
    private bool boosting = false; //determine if we are boosting
    [SerializeField]
    private OnBoostAmountChangedEvent onBoostAmountChanged;

    [Header("Ship Gliding Settings")]
    //Glide physics
    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 1000f;
    float glide, verticalGlide, horizontalGlide = 0f;

    [Header("Debug")]
    [SerializeField]
    [Tooltip("Do NOT directly change this value. Use `CurrentBoostAmount` property instead.")]
    private float currentBoostAmount;

    public float CurrentBoostAmount
    {
        get => currentBoostAmount;
        private set
        {
            if (Mathf.Abs(currentBoostAmount - value) < 0.001f) return;
            currentBoostAmount = value;
            onBoostAmountChanged?.Invoke(currentBoostAmount, maxBoostAmount);
        }
    }

    Rigidbody rb;

    //Input Values
    private float thrust1D;
    private float strafe1D;
    private float upDown1D;
    private float roll1D;
    private Vector2 pitchYaw;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CurrentBoostAmount = maxBoostAmount; //start will full boost
    }

    //We want this to de independent of the framerate, therefore we use FixedUpdate
    void FixedUpdate()
    {
        HandleMovement();
        HandleBoosting();
    }

    /// <summary>
    /// Handles movement for the ship by manipulation torque and thrust
    /// </summary>
    void HandleMovement()
    {
        /*We use torque to handle movements across all axes*/

        //Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);

        //Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime); //make sure the yaw movement does not exceed 1 or -1

        //Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        //Thrust
        if(thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust;

            if (boosting)
            {
                currentThrust = thrust * boostMultiplier;
            }
            else
            {
                currentThrust = thrust;
            }

            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;
        }
        else //if we are not pressing anything
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction; //the ship will lose force over time
        }

        //up / down
        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.deltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else //if we are not pressing anything
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.deltaTime);
            verticalGlide *= upDownGlideReduction; //the ship will lose force over time
        }

        //strafing
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.deltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else //if we are not pressing anything
        {
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.deltaTime);
            horizontalGlide *= leftRightGlideReduction; //the ship will lose force over time
        }

    }

    /// <summary>
    /// This method handles the boost logic, based on the ship boosting settings
    /// </summary>
    void HandleBoosting()
    {
        if (boosting && CurrentBoostAmount > 0f) //if we are boosting and we have enough boost
        {
            CurrentBoostAmount = Mathf.Max(0.0f, CurrentBoostAmount - boostDeprecationRate); //lose boost gradually
            if (CurrentBoostAmount <= 0f) //check again
            {
                boosting = false;
            }
        }
        else if (CurrentBoostAmount < maxBoostAmount)
        {
            CurrentBoostAmount = Mathf.Min(maxBoostAmount, CurrentBoostAmount + boostRechargeRate); //recharge boost
        }
    }

    /// <summary>
    /// When we press buttons, we set up so it access the correct methods and read the values
    /// This is going to be used in the Ship gameObject, under the Player Input controller under: Events -> ShipControls
    /// </summary>
    #region Input Methods
    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
    }
    #endregion
}
