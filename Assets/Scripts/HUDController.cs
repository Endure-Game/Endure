using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject inventory;
	public GameObject map;

	private int maxHealth;

	// Use this for initialization
	void Start () {

		this.maxHealth = PlayerController.instance.health;
	}
	
	// Update is called once per frame
	void Update () {

		print(1f * PlayerController.instance.health / this.maxHealth);
		healthBar.transform.localScale = new Vector3(1f * PlayerController.instance.health / this.maxHealth, 1f, 1f);
	}
}
