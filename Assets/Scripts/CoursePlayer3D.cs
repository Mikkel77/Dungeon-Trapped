using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CoursePlayer3D : MonoBehaviour
{
	public float speed = 5f;
	private Rigidbody body;
	private Vector3 direction;

	private void Awake() { body = GetComponent<Rigidbody>(); }

	private void Update()
	{
		Keyboard keys = Keyboard.current;
		if (keys == null) { direction = Vector3.zero; return; }
		float x = 0f, z = 0f;
		if (keys.aKey.isPressed || keys.leftArrowKey.isPressed) x -= 1f;
		if (keys.dKey.isPressed || keys.rightArrowKey.isPressed) x += 1f;
		if (keys.sKey.isPressed || keys.downArrowKey.isPressed) z -= 1f;
		if (keys.wKey.isPressed || keys.upArrowKey.isPressed) z += 1f;
		direction = new Vector3(x, 0f, z).normalized;
	}

	private void FixedUpdate() { body.linearVelocity = direction * speed; }

	public void StopMoving()
	{
		direction = Vector3.zero;
		body.linearVelocity = Vector3.zero;
		enabled = false;
	}
}