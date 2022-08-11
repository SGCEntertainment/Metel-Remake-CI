using UnityEngine;

public class PlayerController : MonoBehaviour
{
	float cameraPitch;

	float velocityY;

	Vector2 currentDir = Vector2.zero;

	Vector2 currentDirVelocity = Vector2.zero;

	Vector2 currentMouseDelta = Vector2.zero;

	Vector2 currentMouseDeltaVelocity = Vector2.zero;

	CharacterController characterController;

	[SerializeField] bool lockCursor;
	[SerializeField] AudioSource audioSource;

	[SerializeField] Transform playerCamera;

	public float gravity;

	[Range(0.0f, 0.5f)] public float moveSmoothTime;

	[Range(0.0f, 0.5f)] public float mouseSmoothTime;

	public float mouseSensitivity;

	public float walkSpeed;

	public LayerMask targetMask;

	public AudioClip[] walkSoundsGround;

	public AudioClip[] walkSoundsSnow;

	private void Start()
	{
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		UpdateMouseLook();

		UpdateMovement();

		PlayWalkSound();
	}

	AudioClip GetRandomWalkClipSound(AudioClip[] clips) => clips[Random.Range(0, clips.Length)];

	void PlayWalkSound()
	{
		if (!characterController.isGrounded || characterController.velocity.magnitude < 2 || audioSource.isPlaying) return;

		if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, 3.0f, targetMask))
		{
			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("wood"))
				{
					audioSource.PlayOneShot(GetRandomWalkClipSound(walkSoundsGround));
				}
				else if (hit.collider.CompareTag("snow"))
				{
					audioSource.PlayOneShot(GetRandomWalkClipSound(walkSoundsSnow));
				}
			}
		}
	}

	void UpdateMouseLook()
	{
		Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime); 

		cameraPitch -= currentMouseDelta.y * mouseSensitivity;

		cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

		playerCamera.localEulerAngles = Vector3.right * cameraPitch;

		transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
	}

	void UpdateMovement()
	{
		if (characterController.isGrounded)
		{
			velocityY = 0;
		}

		velocityY += gravity * Time.deltaTime;

		Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		targetDir.Normalize();

		currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

		Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY; ;

		characterController.Move(Vector3.ClampMagnitude(velocity * Time.deltaTime, 1.0f));
	}
}
