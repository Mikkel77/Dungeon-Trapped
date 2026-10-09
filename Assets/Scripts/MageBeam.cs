using UnityEngine;
using UnityEngine.InputSystem;
public class MageBeam : MonoBehaviour
{
	[Header("Beam Stats (base values, shop upgrades are added on top)")]
	public int damage = 10;
	public float range = 100f;
	public float fireRate = 0.5f;      // time between beams
	public float beamDuration = 0.25f; // how long the beam lingers
	public float beamWidth = 0.05f;

	[Header("References")]
	public Camera fpsCam;
	public Transform attackPoint;      // where the beam visually starts (wand/hand)
	public LayerMask hitLayers;        // include Enemy + environment layers

	LineRenderer line;
	float nextFireTime;

	void Awake()
	{
		// Apply the upgrades bought in the shop
		damage += PlayerProgress.DamageBonus;
		fireRate *= PlayerProgress.FireRateMultiplier;
		range += PlayerProgress.RangeBonus;

		// Create the beam visuals automatically
		line = gameObject.AddComponent<LineRenderer>();
		line.positionCount = 2;
		line.startWidth = beamWidth;
		line.endWidth = beamWidth;
		line.material = new Material(Shader.Find("Sprites/Default"));
		line.startColor = Color.white;
		line.endColor = Color.white;
		line.enabled = false;
	}

	void Update()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + fireRate;
			Shoot();
		}
	}

	void Shoot()
	{
		Vector3 origin = fpsCam.transform.position;
		Vector3 direction = fpsCam.transform.forward;
		Vector3 endPoint = origin + direction * range;

		// Hitscan raycast from camera center (no spread)
		if (Physics.Raycast(origin, direction, out RaycastHit hit, range, hitLayers))
		{
			endPoint = hit.point;

			if (hit.collider.CompareTag("Enemy"))
			{
				hit.collider.GetComponent<AIController>().TakeDamage(damage);
			}
		}

		// Show the beam
		line.SetPosition(0, attackPoint.position);
		line.SetPosition(1, endPoint);
		line.enabled = true;
		CancelInvoke(nameof(HideBeam));
		Invoke(nameof(HideBeam), beamDuration);
	}

	void HideBeam()
	{
		line.enabled = false;
	}
}
