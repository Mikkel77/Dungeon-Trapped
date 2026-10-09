using UnityEngine;
using TMPro;

public class CourseHealth3D : MonoBehaviour
{
	public CourseGame3D game;
	public TMP_Text healthText;
	public int maxHealth = 3;
	public float invincibleSeconds = 1f;

	public int CurrentHealth { get; private set; }
	private float safeUntil;

	private void Start()
	{
		maxHealth = Mathf.Max(1, maxHealth);
		CurrentHealth = maxHealth;
		RefreshUI();
	}

	private void Update()
	{
		RefreshUI();
	}

	public void TakeDamage(int amount)
	{
		if (game == null || game.Finished || CurrentHealth <= 0) return;
		if (amount <= 0 || Time.time < safeUntil) return;

		CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
		safeUntil = Time.time + Mathf.Max(0.1f, invincibleSeconds);

		if (CurrentHealth == 0) game.Lose();
		RefreshUI();
	}

	private void RefreshUI()
	{
		if (healthText == null) return;
		bool protectedNow = CurrentHealth > 0 && Time.time < safeUntil
			&& game != null && !game.Finished;

		healthText.text = "LIV: " + CurrentHealth + " / " + maxHealth;
		if (protectedNow) healthText.text += " - BESKYTTET";
		healthText.color = CurrentHealth == 0 ? Color.red
			: protectedNow ? Color.yellow : Color.white;
	}
}