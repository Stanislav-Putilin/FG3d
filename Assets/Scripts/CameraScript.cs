using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private GameObject character;
    private Vector3 s;
	private InputAction lookAction;

	private float angH, angH0;
	private float angV, angV0;

	void Start()
    {
        character = GameObject.Find("Character");
		lookAction = InputSystem.actions.FindAction("Look");
		s = this.transform.position - character.transform.position;

		angH0 = angH = transform.rotation.eulerAngles.y;
		angV0 = angV = transform.rotation.eulerAngles.x;
	}   
    void Update()
    {
		Vector2 lookValue = lookAction.ReadValue<Vector2>();

		angH += lookValue.x * 0.05f;

		angV -= lookValue.y * 0.05f;

		angV = Mathf.Clamp(angV, -30.0f,30.0f);

		this.transform.eulerAngles = new Vector3(angV, angH,  0.0f);

		this.transform.position = character.transform.position + 
		  Quaternion.Euler(angV - angV0, angH - angH0,  0.0f) * s;
	}
}
