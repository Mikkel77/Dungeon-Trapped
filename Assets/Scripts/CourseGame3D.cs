using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CourseGame3D : MonoBehaviour
{
	public CoursePlayer3D player;
	public TMP_Text statusText;
	public int totalRunes = 3;
	public string shopSceneName = "Shop";       // must match the shop scene's name
	public float shopDelay = 1.5f;              // seconds to show the win message first
	public bool Finished { get; private set; }
	private int collected;
	private string lastMessage = "";

	private void Start() { Refresh("Saml runerne. WASD/pile. R: genstart."); }

	private void Update()
	{
		if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Collect()
	{
		if (Finished) return;
		collected++;
		Refresh(collected >= totalRunes ? "Portalen er klar!" : "Find de sidste runer.");
	}

	public void AddGold(int amount)
	{
		PlayerProgress.Gold += amount;
		Refresh(lastMessage);
	}

	public void EnterPortal()
	{
		if (Finished) return;
		if (collected < totalRunes) { Refresh("Portalen er laast."); return; }
		Finished = true;
		if (player != null) player.StopMoving();
		Refresh("Portalen aabner... til butikken!");
		Invoke(nameof(GoToShop), shopDelay);
	}

	private void GoToShop()
	{
		PlayerProgress.DungeonScene = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(shopSceneName);
	}

	public void Lose()
	{
		if (Finished) return;
		Finished = true;
		if (player != null) player.StopMoving();
		Refresh("DU TABTE! R: genstart.");
	}

	private void Refresh(string message)
	{
		lastMessage = message;
		if (statusText != null)
			statusText.text = "RUNER: " + collected + " / " + totalRunes + "   GULD: " + PlayerProgress.Gold + "\n" + message;
	}
}
