using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(AudioSource))]
public class CharacterSoundController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip jumpLandingClip;
    [SerializeField] private AudioClip poofClip;

    private CharacterController2D character;
    private AudioSource audioSource;

    private bool wasGrounded;

    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        audioSource = GetComponent<AudioSource>();
        
        // Subscribe to events
        character.OnJumped += PlayJumpSound;
        PlayerController.OnSwitch += PlayPoofSound;
    }

    private void OnDestroy()
    {
        if (character != null)
        {
            character.OnJumped -= PlayJumpSound;
        }
        PlayerController.OnSwitch -= PlayPoofSound;
    }

    private void LateUpdate()
    {
        // Check for landing
        if (!wasGrounded && character.IsGrounded)
        {
            PlayLandingSound();
        }
        wasGrounded = character.IsGrounded;
    }

    private void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private void PlayLandingSound()
    {
        if (jumpLandingClip != null)
        {
            audioSource.PlayOneShot(jumpLandingClip);
        }
    }

    private void PlayPoofSound(bool isAwake)
    {
        if (poofClip != null)
        {
            audioSource.PlayOneShot(poofClip);
        }
    }
}
