using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
	public int damage = 3;
	public float range = 0.6f;
	public float delay = 0.5f;

	private GameObject weapon;
	private float elapsed;

	private float untilUnlocked;
	private bool locked = false;

	public bool Locked {
		get {
			return locked;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.untilUnlocked -= Time.deltaTime;

		if (this.weapon != null) {
			this.elapsed += Time.deltaTime;
			if (this.elapsed > 0.1f) {
				Destroy (this.weapon);
				this.weapon = null;
			}
		}

		if (this.untilUnlocked <= 0) {
			this.locked = false;
		}
	}

	private void CreateOuch (bool horizontal, int direction) {
		if (!this.locked) {
			Destroy (this.weapon);
			this.elapsed = 0;
			this.untilUnlocked = this.delay;
			this.locked = true;

			this.weapon = new GameObject ();
			this.weapon.name = "meleeWeapon";
			this.weapon.transform.parent = this.gameObject.transform;
			this.weapon.transform.position = this.gameObject.transform.position;

			BoxCollider2D collider = this.weapon.AddComponent<BoxCollider2D> ();
			collider.isTrigger = true;

			Vector2 playerSize = this.Size;
			collider.size = playerSize;

			if (horizontal) {
				collider.size = new Vector2 (collider.size.x, this.range);
				collider.transform.Translate (0, (playerSize.y / 2 + this.range / 2) * direction, 0);
			} else {
				collider.size = new Vector2 (this.range, collider.size.y);
				collider.transform.Translate ((playerSize.x / 2 + this.range / 2) * direction, 0, 0);
			}

			Ouch ouch = this.weapon.AddComponent<Ouch> ();
			ouch.damage = this.damage;
		}
	}

	public void AttackNorth () {
		CreateOuch (true, 1);
		print ("Attacking north");
	}
	
	public void AttackEast () {
		CreateOuch (false, 1);
		print ("Attacking east");
	}
	
	public void AttackWest () {
		CreateOuch (false, -1);
		print ("Attacking west");
	}
	
	public void AttackSouth () {
		CreateOuch (true, -1);
		print ("Attacking south");
	}

	public Vector2 Size {
		get {
			return this.GetComponent<BoxCollider2D> ().size;
		}
	}
}
