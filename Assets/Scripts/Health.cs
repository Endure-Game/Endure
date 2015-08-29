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
}
