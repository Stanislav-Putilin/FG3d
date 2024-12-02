using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private InputAction moveAction;
	private InputAction jumpAction;

	private CharacterController characterController;

    private float speedFactor = 4f;

	public Transform cameraTransform;


	void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");
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

		if (jumpAction.IsPressed())
		{			
			Vector3 forwardDirection = cameraTransform.forward;
			forwardDirection.y = 0;
			forwardDirection.Normalize();
			moveDirection += forwardDirection;
		}

		if (terrainH < 20f)
		{
			characterController.Move(speedFactor * Time.deltaTime * moveDirection);
		}
		else
		{
			Vector3 newPosition = transform.position;
			newPosition.y -= 0.1f;
			transform.position = newPosition;
		}	

		Debug.Log(this.transform.position.y
				- Terrain.activeTerrain.SampleHeight(this.transform.position));
	}
}