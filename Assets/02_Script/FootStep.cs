using System.Collections;
using UnityEngine;

public class Footstep : MonoBehaviour
{

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip footstepSound;
    [SerializeField]
    Transform footTransform;
    [SerializeField]
    float raycastDistance = 0.1f;
    [SerializeField]
    LayerMask groundLayers = 0;
    [SerializeField]
    float minFootstepInterval = 0.5f;

    bool timerIsActive = false;
    WaitForSeconds footstepWait;

    void Start()
    {
        footstepWait = new WaitForSeconds(minFootstepInterval);
    }

    void Update()
    {
        CheckGroundStatus();
    }

    void CheckGroundStatus()
    {
        if (timerIsActive)
        {
            return;
        }

        bool isGrounded = Physics.Raycast(footTransform.position, Vector3.down, raycastDistance, groundLayers, QueryTriggerInteraction.Ignore);

        if (isGrounded)
        {
            PlayFootstepSound();
        }

        StartCoroutine(nameof(FootstepTimer));
    }

    void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }

    IEnumerator FootstepTimer()
    {
        timerIsActive = true;

        yield return footstepWait;

        timerIsActive = false;
    }

}