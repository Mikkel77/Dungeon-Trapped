using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CourseEnemyPatrol3D : MonoBehaviour
{
	public CourseGame3D game;
	public Transform pointA;
	public Transform pointB;
	public float speed = 2f;
	public int damage = 1;

	private Rigidbody body;
	private bool goingToB = true;

	private void Awake()
	{
		body = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (game == null || game.Finished || pointA == null || pointB == null) return;

		Transform target = goingToB ? pointB : pointA;
		Vector3 next = Vector3.MoveTowards(body.position, target.position,
			Mathf.Max(0f, speed) * Time.fixedDeltaTime);
		body.MovePosition(next);

		if (Vector3.Distance(next, target.position) < 0.01f)
			goingToB = !goingToB;
	}

	private void OnTriggerEnter(Collider other)
	{
		TryDamage(other);
	}

	private void OnTriggerStay(Collider other)
	{
		TryDamage(other);
	}

	private void TryDamage(Collider other)
	{
		if (game == null || game.Finished) return;
		CourseHealth3D health = other.GetComponent<CourseHealth3D>();
		if (health != null) health.TakeDamage(damage);
	}
}