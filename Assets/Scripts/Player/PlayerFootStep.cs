using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    private AudioSource footstepSound;

    [SerializeField]
    private AudioClip[] footstepClip;

    private CharacterController characterController;

    [HideInInspector]
    public float volumeMin, volumeMax;

    private float accumulatedDistance;

    [HideInInspector]
    public float stepDistance;

    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        CheckToPlayFootStepSound();
    }

    void CheckToPlayFootStepSound()
    {
        if(!characterController.isGrounded)
        {
            return;
        }

        if(characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;

            if(accumulatedDistance > stepDistance)
            {
                footstepSound.volume = Random.Range(volumeMin, volumeMax);
                footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                footstepSound.Play();
                accumulatedDistance = 0;
            }
        }
        else
        {
            accumulatedDistance = 0;
        }
    }
}
