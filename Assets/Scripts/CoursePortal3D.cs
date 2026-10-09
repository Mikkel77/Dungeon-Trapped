using UnityEngine;

public class CoursePortal3D : MonoBehaviour
{
	public CourseGame3D game;

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<CoursePlayer3D>() != null && game != null)
			game.EnterPortal();
	}
}