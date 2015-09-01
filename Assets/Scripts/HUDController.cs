using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject maxHealthBar;

	public Image lowHealth;

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
		maxHealthBar.transform.localScale = new Vector3(1f * this.playerHealth.maxHealth / this.startingMaxHealth, 1f, 1f);

		if (this.playerHealth.CurrentHealth <= this.playerHealth.maxHealth / 10) {
			lowHealth.color = new Color(255, 0, 0, 0.3f);
		} else {
			lowHealth.color = new Color(0, 0, 0, 0);
		}
	}
}
