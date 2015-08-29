using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject inventory;
	public GameObject map;

	private Health playerHealth;

	// Use this for initialization
	void Start () {
		this.playerHealth = PlayerController.instance.Health;
	}
	
	// Update is called once per frame
	void Update () {

		print(1f * this.playerHealth.CurrentHealth / this.playerHealth.maxHealth);
		healthBar.transform.localScale = new Vector3(1f * this.playerHealth.CurrentHealth / this.playerHealth.maxHealth, 1f, 1f);
	}
}
