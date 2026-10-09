using UnityEngine;

public class CourseRune3D : MonoBehaviour
{
	public CourseGame3D game;
	private bool collected;

	private void OnTriggerEnter(Collider other)
	{
		if (collected || other.GetComponent<CoursePlayer3D>() == null) return;
		if (game == null || game.Finished) return;
		collected = true;
		game.Collect();
		gameObject.SetActive(false);
	}
}