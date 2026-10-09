using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Put this on an empty GameObject in a scene called "Shop".
// It builds the whole UI by itself, so no Canvas setup is needed.
public class ShopManager : MonoBehaviour
{
	public string fallbackDungeonScene = "SampleScene";
	public int baseCost = 20;
	public int costStep = 15;

	class Upgrade
	{
		public string name;
		public string effect;
		public Func<int> getLevel;
		public Action levelUp;
		public TMP_Text label;
	}

	readonly List<Upgrade> upgrades = new List<Upgrade>();
	TMP_Text goldText;

	void Start()
	{
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		upgrades.Add(new Upgrade
		{
			name = "Damage",
			effect = "+" + PlayerProgress.DamagePerLevel + " damage",
			getLevel = () => PlayerProgress.DamageLevel,
			levelUp = () => PlayerProgress.DamageLevel++
		});
		upgrades.Add(new Upgrade
		{
			name = "Fire Rate",
			effect = "10% faster",
			getLevel = () => PlayerProgress.FireRateLevel,
			levelUp = () => PlayerProgress.FireRateLevel++
		});
		upgrades.Add(new Upgrade
		{
			name = "Range",
			effect = "+" + PlayerProgress.RangePerLevel + " range",
			getLevel = () => PlayerProgress.RangeLevel,
			levelUp = () => PlayerProgress.RangeLevel++
		});

		BuildUI();
		RefreshAll();
	}

	int CostOf(Upgrade u) { return baseCost + u.getLevel() * costStep; }

	void Buy(Upgrade u)
	{
		int cost = CostOf(u);
		if (PlayerProgress.Gold < cost) return;
		PlayerProgress.Gold -= cost;
		u.levelUp();
		RefreshAll();
	}

	void RefreshAll()
	{
		goldText.text = "GOLD: " + PlayerProgress.Gold;
		foreach (Upgrade u in upgrades)
		{
			u.label.text = u.name + "  Lv " + u.getLevel() + "  (" + u.effect + ")\nCost: " + CostOf(u);
		}
	}

	void StartDungeon()
	{
		string scene = string.IsNullOrEmpty(PlayerProgress.DungeonScene) ? fallbackDungeonScene : PlayerProgress.DungeonScene;
		SceneManager.LoadScene(scene);
	}

	// ---------- UI building ----------

	void BuildUI()
	{
		if (FindFirstObjectByType<EventSystem>() == null)
		{
			GameObject es = new GameObject("EventSystem");
			es.AddComponent<EventSystem>();
			es.AddComponent<InputSystemUIInputModule>();
		}

		GameObject canvasGO = new GameObject("ShopCanvas");
		Canvas canvas = canvasGO.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1920, 1080);
		canvasGO.AddComponent<GraphicRaycaster>();
		Transform root = canvasGO.transform;

		// Background
		GameObject bg = new GameObject("Background", typeof(RectTransform), typeof(Image));
		bg.transform.SetParent(root, false);
		bg.GetComponent<Image>().color = new Color(0.08f, 0.07f, 0.12f, 1f);
		Stretch(bg.GetComponent<RectTransform>());

		// Title
		TMP_Text title = CreateText(root, "Title", "SHOP", 80, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -80), new Vector2(800, 120));
		title.alignment = TextAlignmentOptions.Center;

		// Gold
		goldText = CreateText(root, "Gold", "GOLD: 0", 56, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -190), new Vector2(800, 80));
		goldText.alignment = TextAlignmentOptions.Center;
		goldText.color = new Color(1f, 0.85f, 0.2f);

		// Upgrade buttons
		float y = 120f;
		foreach (Upgrade u in upgrades)
		{
			Upgrade captured = u;
			TMP_Text label;
			CreateButton(root, u.name + "Button", "", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
				new Vector2(0, y), new Vector2(900, 130), () => Buy(captured), out label, 44);
			u.label = label;
			y -= 160f;
		}

		// Start Dungeon (bottom right)
		TMP_Text startLabel;
		CreateButton(root, "StartDungeonButton", "Start Dungeon", new Vector2(1f, 0f), new Vector2(1f, 0f),
			new Vector2(-40, 40), new Vector2(380, 110), StartDungeon, out startLabel, 48);
	}

	static void Stretch(RectTransform rt)
	{
		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.one;
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;
	}

	static TMP_Text CreateText(Transform parent, string name, string text, float size, Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 dim)
	{
		GameObject go = new GameObject(name, typeof(RectTransform));
		go.transform.SetParent(parent, false);
		TextMeshProUGUI t = go.AddComponent<TextMeshProUGUI>();
		t.text = text;
		t.fontSize = size;
		t.color = Color.white;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.anchorMin = anchor;
		rt.anchorMax = anchor;
		rt.pivot = pivot;
		rt.anchoredPosition = pos;
		rt.sizeDelta = dim;
		return t;
	}

	static void CreateButton(Transform parent, string name, string text, Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 dim,
		Action onClick, out TMP_Text label, float fontSize)
	{
		GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
		go.transform.SetParent(parent, false);
		go.GetComponent<Image>().color = new Color(0.25f, 0.2f, 0.45f, 1f);
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.anchorMin = anchor;
		rt.anchorMax = anchor;
		rt.pivot = pivot;
		rt.anchoredPosition = pos;
		rt.sizeDelta = dim;
		go.GetComponent<Button>().onClick.AddListener(() => onClick());

		label = CreateText(go.transform, "Label", text, fontSize, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, dim);
		label.alignment = TextAlignmentOptions.Center;
		label.raycastTarget = false;
		Stretch(label.GetComponent<RectTransform>());
	}
}
