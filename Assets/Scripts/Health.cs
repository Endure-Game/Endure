using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;

	[System.Serializable]
	public class Drop {
		public int chance;
		public GameObject item;
	}

	public Drop[] drops;

	private GameObject healthChangeDisplay;

	private int currentHealth;
	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

	private bool blocking = false;

	public bool Block {
		get {
			return this.blocking;
		}
		set {
			this.blocking = value;
		}
	}

	// Use this for initialization
	void Start () {
		this.currentHealth = this.maxHealth;
		this.healthChangeDisplay = Resources.Load ("HealthChange", typeof(GameObject)) as GameObject;
	}

	public void ChangeHealth (int delta) {
		if (this.currentHealth + delta < 0) {
			delta = -this.currentHealth;
		} else if (this.currentHealth + delta > this.maxHealth) {
			delta = this.maxHealth - this.currentHealth;
		}

		if (!this.Block || delta >= 0) {
			Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y + 0.75f, 0);

			GameObject change = Instantiate (healthChangeDisplay, startPos, Quaternion.identity) as GameObject;
			change.GetComponent<HealthAnimation> ().healthChange = delta;

			this.currentHealth += delta;

			if (this.currentHealth <= 0) {
				this.DropAndDestroy ();
			}
		}
	}

	void DropAndDestroy ()
	{
		var position = this.transform.position;
		Destroy (this.gameObject);

		var max = 0;
		foreach (var drop in this.drops) {
			max += drop.chance;
		}

		var current = 0;
		var selected = Random.Range (0, max + 1);
		foreach (var drop in this.drops) {
			current += drop.chance;
			if (current >= selected) {
				Instantiate (drop.item, position, Quaternion.identity);
				break;
			}
		}
	}
}
