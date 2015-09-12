using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;
	public AudioClip deathSound;

	private GameObject healthChangeDisplay;
	private AudioSource death;

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
		this.death = gameObject.AddComponent<AudioSource> ();
		this.death.clip = this.deathSound;
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
				this.Die ();
			}
		}
	}

	void Die ()
	{
		this.death.Play ();
		if (this.gameObject == PlayerController.instance.gameObject) {
			Application.LoadLevel (2);
		}

		var position = this.transform.position;
		if (this.GetComponent<Drops>() != null) {
			this.GetComponent<Drops>().DropItem();
		}
		//this.GetComponent<Renderer>().enabled = false;
		if (this.tag != "Player") {
			this.transform.position = Vector3.one * 9999999f;
		}
		Destroy(this.gameObject, this.death.clip.length);
		//Destroy (this.gameObject);

	}
}
