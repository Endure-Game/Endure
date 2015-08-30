using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;

	private int currentHealth;
	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

	// Use this for initialization
	void Start () {
		this.currentHealth = this.maxHealth;
	}

	public void ChangeHealth (int delta) {
		if (this.currentHealth + delta < 0) {
			delta = -this.currentHealth;
		} else if (this.currentHealth + delta > this.maxHealth) {
			delta = this.maxHealth - this.currentHealth;
		}

		// TODO: add health change animation

		this.currentHealth += delta;
	}
}
