// handles movement, jumping, running, gravity, stamina, and attack behavior

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool ignoreGroundSnap = false;

    [Header("Stamina")]
    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float RunCost;
    public float ChargeRate;

    private Coroutine recharge;
    private bool isShifting = false;
    public bool isGrounded;
    private Vector3 velocity;

    // handles player input, movement, stamina usage and recovery
    void Update()
    {
        // ground check for ability too
        if (!ignoreGroundSnap)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0 && !ignoreGroundSnap)
            velocity.y = -2f;

        // movemnet input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        isShifting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if ((x != 0 || z != 0) && speed > 6)
        {
            Stamina -= RunCost * Time.deltaTime;
            Stamina = Mathf.Max(Stamina, 0);
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // speed control for shifting and 0 stamina
        if (isShifting)
        {
            speed = 5f;
        }
        else if (Stamina >= MaxStamina * 0.1f && speed == 5f)
        {
            speed = 30f;
        }
        else if (Stamina == 0)
        {
            speed = 5f;
        }
    }

    // gradually restores stamina over time
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (Stamina < MaxStamina)
        {
            float rechargeRate = (speed == 5f) ? ChargeRate / 2 : ChargeRate;

            Stamina = Mathf.MoveTowards(Stamina, MaxStamina, rechargeRate * Time.deltaTime);
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if (Stamina >= 10f && speed == 5f && !isShifting)
                speed = 30f;

            yield return null;
        }

        Stamina = MaxStamina;
    }

    // immediately refills stamina to full for ult and developer mode
    public void RefillStamina()
    {
        Stamina = MaxStamina;
        StaminaBar.fillAmount = Stamina / MaxStamina;

        if (recharge != null) StopCoroutine(recharge);
    }
}
