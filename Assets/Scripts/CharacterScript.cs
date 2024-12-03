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

	public Transform cameraTransform;

	//public float burst => burstLeft / burstPeriod;


	void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");

		sprintAction = InputSystem.actions.FindAction("Sprint");

		sprintAction.started += _ => isSprinting = true;
		sprintAction.canceled += _ => isSprinting = false;

		characterController = GetComponent<CharacterController>();
		cameraTransform = Camera.main.transform;
	}   
    void Update()
    {	
		Vector2 moveValue = moveAction.ReadValue<Vector2>();
	
		Vector3 moveDirection = new Vector3(moveValue.x, moveValue.y, 0);

		moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;

		Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

		float terrainH = this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position);

		//if (jumpAction.IsPressed())
		//{			
			Vector3 forwardDirection = cameraTransform.forward;
			forwardDirection.y = 0;
			forwardDirection.Normalize();
			moveDirection += forwardDirection;
		//}

		
		if (terrainH < 20f)
		{
			float currentSpeedFactor;

			if(terrainH < 1.15f)
			{
				currentSpeedFactor = speedFactor * slowDownFactor;
			}
			else if(isSprinting)
			{
				currentSpeedFactor = sprintSpeed;
			}
			else
			{
				currentSpeedFactor = speedFactor;
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

		Debug.Log(this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position));
	}	

	private void LateUpdate()
	{
		//if(burstLeft > 0f)
	}
}