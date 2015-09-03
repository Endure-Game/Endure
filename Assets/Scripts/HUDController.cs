using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject maxHealthBar;

	public Image lowHealth;
	public Sprite selected;

	public GameObject map;
	private GameObject inventory;

	private Health playerHealth;
	private int startingMaxHealth;

	private RectTransform rectTransform;

	private bool paused = false;

	// Use this for initialization
	void Start () {
		this.playerHealth = PlayerController.instance.Health;
		startingMaxHealth = this.playerHealth.maxHealth;

		this.inventory = new GameObject ();
		this.inventory.transform.parent = this.gameObject.transform;

		this.rectTransform = this.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Health bar
		healthBar.transform.localScale = new Vector3(1f * this.playerHealth.CurrentHealth / this.startingMaxHealth, 1f, 1f);
		maxHealthBar.transform.localScale = new Vector3(1f * this.playerHealth.maxHealth / this.startingMaxHealth, 1f, 1f);

		// low health warning
		if (this.playerHealth.CurrentHealth <= this.playerHealth.maxHealth / 10) {
			lowHealth.color = new Color(255, 0, 0, 0.3f);
		} else {
			lowHealth.color = new Color(0, 0, 0, 0);
		}

		// pause
		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.paused = !this.paused;

			if (this.paused) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}
		}

		if (this.paused) {
			lowHealth.color = new Color(0, 0, 0, 0.8f);
		}

		// inventory
		Destroy (this.inventory);
		this.inventory = new GameObject ();
		this.inventory.transform.parent = this.gameObject.transform;

		float iconWidth = 100;
		float barWidth = iconWidth * PlayerController.instance.inventory.Count;

		int i = 0;

		foreach (var item in PlayerController.instance.inventory) {
			var icon = new GameObject ();
			icon.AddComponent<Image> ().sprite = item.sprite;
			icon.transform.SetParent(this.inventory.transform);
			float x = this.rectTransform.rect.width / 2 - barWidth / 2 + i * iconWidth;
			icon.transform.position = new Vector3 (this.rectTransform.rect.width / 2 - barWidth / 2 + i * iconWidth, 100, 0);

			i++;
		}

		if (PlayerController.instance.inventory.Count > 0) {
			var border = new GameObject ();
			border.AddComponent<Image> ().sprite = this.selected;
			border.transform.SetParent (this.inventory.transform);
			border.transform.position = new Vector3 (this.rectTransform.rect.width / 2 - barWidth / 2 + PlayerController.instance.InventoryIndex * iconWidth, 100, 0);
		}
	}
}
