using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private Transform lookRoot;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;

    private PlayerMovement playerMovement;

    private bool isCrouching;

    private PlayerFootStep playerFootStep;

    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f;
    private float walkVolumeMax = 0.6f;

    private float walkStepDistance = 0.4f;
    private float crouchStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;

    private PlayerStats playerStats;
    private float sprintValue = 100f;          // витривалість Player
    public float sprintTreshold = 10f;        // споживання енергії за одиницю часу

    private void Start()
    {
        playerFootStep.volumeMin = walkVolumeMin;
        playerFootStep.volumeMax = walkVolumeMax;
        playerFootStep.stepDistance = walkStepDistance;
    }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);

        playerFootStep = GetComponentInChildren<PlayerFootStep>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (sprintValue > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
            {
                playerMovement.speed = sprintSpeed;

                playerFootStep.volumeMin = sprintVolume;
                playerFootStep.volumeMax = sprintVolume;
                playerFootStep.stepDistance = sprintStepDistance;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;

            playerFootStep.volumeMin = walkVolumeMin;
            playerFootStep.volumeMax = walkVolumeMax;
            playerFootStep.stepDistance = walkStepDistance;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            // зменшуємо к-сть енергії
            sprintValue -= sprintTreshold * Time.deltaTime;

            // якщо закінчилася енергія
            if (sprintValue <= 0f)
            {
                // енергія нуль
                sprintValue = 0f;

                // скинути швидкість і звук
                playerMovement.speed = moveSpeed;
                playerFootStep.stepDistance = walkStepDistance;
                playerFootStep.volumeMin = walkVolumeMin;
                playerFootStep.volumeMax = walkVolumeMax;
            }
            // змінити значення Fill amount
            playerStats.DisplayStaminaStats(sprintValue);
        }
        else
        {
            if (sprintValue != 100f)
            {
                // відновлюємо енергію
                sprintValue += (sprintTreshold / 2f) * Time.deltaTime;

                // змінити значення Fill amount
                playerStats.DisplayStaminaStats(sprintValue);

                // якщо відновили повністю енергію, то зупинити її
                if (sprintValue > 100f)
                {
                    sprintValue = 100f;
                }
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                lookRoot.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovement.speed = moveSpeed;
                isCrouching = false;

                playerFootStep.volumeMin = walkVolumeMin;
                playerFootStep.volumeMax = walkVolumeMax;
                playerFootStep.stepDistance = walkStepDistance;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovement.speed = crouchSpeed;
                isCrouching = true;

                playerFootStep.volumeMin = crouchVolume;
                playerFootStep.volumeMax = crouchVolume;
                playerFootStep.stepDistance = crouchStepDistance;
            }
        }
    }
}
