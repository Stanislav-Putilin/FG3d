using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction sprintAction;

	private CharacterController characterController;

	private float speedFactor = 6f;
	private float slowDownFactor = 0.25f;

	private float sprintSpeed = 12f;

	private bool isSprinting = false;
	private bool isMoving = false;

	public Transform cameraTransform;

	private float burstPeriod = 10f;
	private float burstLeft;

	private Animator animator;

	public float burstLevel => burstLeft / burstPeriod;


	void Start()
	{
		moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");

		sprintAction = InputSystem.actions.FindAction("Sprint");

		jumpAction.started += _ => isMoving = true;
		jumpAction.canceled += _ => isMoving = false;

		sprintAction.started += _ => isSprinting = true;
		sprintAction.canceled += _ => isSprinting = false;

		characterController = GetComponent<CharacterController>();
		cameraTransform = Camera.main.transform;

		animator = GetComponent<Animator>();

		GameState.AddListener(nameof(GameState.isBurst), OnBurstChanged);
	}
	void Update()
	{
		Vector2 moveValue = moveAction.ReadValue<Vector2>();

		Vector3 moveDirection = new Vector3(moveValue.x, moveValue.y, 0);

		moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;

		Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

		float terrainH = this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position);

		if (isMoving || GameState.isBurst)
		{
			Vector3 forwardDirection = cameraTransform.forward;
			forwardDirection.y = 0;
			forwardDirection.Normalize();
			moveDirection += forwardDirection;

			if (terrainH < 20f)
			{
				float currentSpeedFactor;
				if (GameState.isBurst)
				{
					currentSpeedFactor = sprintSpeed;
				}
				else if (terrainH < 1.15f)
				{
					currentSpeedFactor = speedFactor * slowDownFactor;
					animator.SetInteger("MoveState", 1);
				}
				else if (isSprinting)
				{
					currentSpeedFactor = sprintSpeed;
				}
				else
				{
					currentSpeedFactor = speedFactor;
					animator.SetInteger("MoveState", 2);
				}

				characterController.Move(currentSpeedFactor * Time.deltaTime * moveDirection);
			}
			else
			{
				Vector3 newPosition = transform.position;
				float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
				newPosition.y = terrainHeight + 19.99f;
				transform.position = newPosition;
			}
		}
		else
		{
			if(animator.GetInteger("MoveState") != 0)
			{
				animator.SetInteger("MoveState", 0);
			}			
		}

		//Debug.Log(this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position));
	}

	private void OnBurstChanged(string ignored)
	{
		if (GameState.isBurst)
		{
			Debug.Log("Burst ++");
			burstLeft = burstPeriod;
		}
	}
	private void OnDestroy()
	{
		GameState.RemoveListener(nameof(GameState.isBurst), OnBurstChanged);
	}

	private void LateUpdate()
	{
		if (burstLeft > 0f)
		{
			burstLeft -= Time.deltaTime;
			if (burstLeft <= 0f)
			{
				burstLeft = 0f;
				GameState.isBurst = false;
			}
		}
	}
}