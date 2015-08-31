using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject inventory;
	public GameObject map;

	private Health playerHealth;
	private int startingMaxHealth;

	// Use this for initialization
	void Start () {
		this.playerHealth = PlayerController.instance.Health;
		startingMaxHealth = this.playerHealth.maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.transform.localScale = new Vector3(1f * this.playerHealth.CurrentHealth / this.startingMaxHealth, 1f, 1f);
	}
}
