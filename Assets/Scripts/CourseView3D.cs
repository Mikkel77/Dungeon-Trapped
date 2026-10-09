using UnityEngine;
using UnityEngine.InputSystem;

public class CourseView3D : MonoBehaviour
{
	public Rigidbody playerBody;
	public CourseGame3D game;
	public Renderer playerVisual;
	public LayerMask cameraWalls;
	public bool firstPerson;
	public float speed = 4f;
	public float sensitivity = 0.12f;
	public float eyeHeight = 0.65f;
	public float distance = 3f;

	private Vector3 direction;
	private float yaw;
	private float pitch;

	private void Start()
	{
		if (playerBody == null || game == null || playerVisual == null)
		{
			Debug.LogError("Forbind Player Body, Game og Player Visual.", this);
			enabled = false;
			return;
		}
		CoursePlayer3D original = playerBody.GetComponent<CoursePlayer3D>();
		if (original != null) original.enabled = false;
		yaw = playerBody.rotation.eulerAngles.y;
		pitch = firstPerson ? 0f : 20f;
		ReleaseMouse();
	}

	private void Update()
	{
		direction = Vector3.zero;
		Keyboard keys = Keyboard.current;
		Mouse mouse = Mouse.current;
		if (game.Finished || keys == null || mouse == null)
		{
			ReleaseMouse();
			return;
		}
		if (keys.escapeKey.wasPressedThisFrame)
		{
			ReleaseMouse();
			return;
		}
		if (Cursor.lockState != CursorLockMode.Locked)
		{
			if (mouse.leftButton.wasPressedThisFrame)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			return;
		}

		Vector2 look = mouse.delta.ReadValue();
		yaw += look.x * sensitivity;
		pitch = Mathf.Clamp(pitch - look.y * sensitivity, -60f, 70f);
		float x = 0f, z = 0f;
		if (keys.aKey.isPressed || keys.leftArrowKey.isPressed) x -= 1f;
		if (keys.dKey.isPressed || keys.rightArrowKey.isPressed) x += 1f;
		if (keys.sKey.isPressed || keys.downArrowKey.isPressed) z -= 1f;
		if (keys.wKey.isPressed || keys.upArrowKey.isPressed) z += 1f;
		direction = Quaternion.Euler(0f, yaw, 0f)
			* new Vector3(x, 0f, z).normalized;
	}

	private void FixedUpdate()
	{
		bool canMove = !game.Finished && Cursor.lockState == CursorLockMode.Locked;
		playerBody.linearVelocity = canMove ? direction * speed : Vector3.zero;
	}

	private void LateUpdate()
	{
		Vector3 eye = playerBody.transform.position + Vector3.up * eyeHeight;
		Quaternion view = Quaternion.Euler(pitch, yaw, 0f);
		float cameraDistance = firstPerson ? 0f : distance;
		Vector3 backwards = view * Vector3.back;
		if (cameraDistance > 0f && Physics.SphereCast(eye, 0.2f, backwards,
			out RaycastHit hit, cameraDistance, cameraWalls, QueryTriggerInteraction.Ignore))
		{
			cameraDistance = Mathf.Max(0f, hit.distance - 0.05f);
		}
		transform.SetPositionAndRotation(eye + backwards * cameraDistance, view);
		playerVisual.enabled = !firstPerson && cameraDistance > 0.7f;
	}

	private void ReleaseMouse()
	{
		direction = Vector3.zero;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void OnApplicationFocus(bool focused)
	{
		if (!focused) ReleaseMouse();
	}

	private void OnDisable()
	{
		ReleaseMouse();
		if (playerBody != null) playerBody.linearVelocity = Vector3.zero;
		if (playerVisual != null) playerVisual.enabled = true;
	}
}