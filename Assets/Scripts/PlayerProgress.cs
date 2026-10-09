using UnityEngine;

// Static data that survives scene loads (dungeon -> shop -> dungeon).
public static class PlayerProgress
{
	public static int Gold;
	public static int DamageLevel;
	public static int FireRateLevel;
	public static int RangeLevel;

	// The scene the player came from, so the shop knows where "Start Dungeon" goes.
	public static string DungeonScene = "";

	public const int DamagePerLevel = 5;
	public const float FireRateFactorPerLevel = 0.9f;   // each level = 10% faster
	public const float RangePerLevel = 20f;

	public static int DamageBonus { get { return DamageLevel * DamagePerLevel; } }
	public static float FireRateMultiplier { get { return Mathf.Pow(FireRateFactorPerLevel, FireRateLevel); } }
	public static float RangeBonus { get { return RangeLevel * RangePerLevel; } }

	// Makes sure a fresh Play session starts from zero (static values can otherwise
	// survive if Domain Reload is disabled in the editor).
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetAll()
	{
		Gold = 0;
		DamageLevel = 0;
		FireRateLevel = 0;
		RangeLevel = 0;
		DungeonScene = "";
	}
}
