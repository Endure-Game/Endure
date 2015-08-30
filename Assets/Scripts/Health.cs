using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;

	private GameObject healthChangeDisplay;

	private int currentHealth;
	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

	// Use this for initialization
	void Start () {
		this.currentHealth = this.maxHealth;
		this.healthChangeDisplay = Resources.Load ("HealthChange", typeof(GameObject)) as GameObject;
		print (this.healthChangeDisplay);
	}

	public void ChangeHealth (int delta) {
		if (this.currentHealth + delta < 0) {
			delta = -this.currentHealth;
		} else if (this.currentHealth + delta > this.maxHealth) {
			delta = this.maxHealth - this.currentHealth;
		}

		Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y + 0.75f, 0);

		// TODO: add health change animation
		GameObject change = Instantiate (healthChangeDisplay, startPos, Quaternion.identity) as GameObject;
		change.GetComponent<HealthAnimation> ().healthChange = delta;

		this.currentHealth += delta;

		if (this.currentHealth <= 0) {
			Destroy (this.gameObject);
		}
	}
}
