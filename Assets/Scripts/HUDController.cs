using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject healthBar;
	public GameObject maxHealthBar;

	public Image lowHealth;
	public Sprite selected;
	public Sprite mouse;
	public Sprite space;
	public Font font;

	public GameObject world;
	private RoomManager roomManager;

	public GameObject map;
	private GameObject inventory;
	private int oldInventorySize = 0;
	private GameObject border;
	private GameObject control;
	private GameObject count;

	private Health playerHealth;
	private int startingMaxHealth;

	private Vector2 resolution;

	private bool paused = false;

	private Texture2D mapTexture;

	// Use this for initialization
	void Start () {
		this.playerHealth = PlayerController.instance.Health;
		startingMaxHealth = this.playerHealth.maxHealth;

		this.inventory = new GameObject ();
		this.inventory.transform.parent = this.gameObject.transform;

		this.resolution = this.GetComponent<CanvasScaler> ().referenceResolution;
		this.resolution.y -= 3;

		if (this.font == null) {
			this.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		}

		this.roomManager = this.world.GetComponent<RoomManager> ();

		this.mapTexture = this.CreateMap ();
		//var s = this.map.GetComponent<Image> ().sprite;
		this.map.GetComponent<Image> ().sprite = Sprite.Create(this.mapTexture, new Rect(0, 0, this.mapTexture.width, this.mapTexture.height), new Vector2 ());

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

		var sample = new GameObject ();
		sample.AddComponent<Image> ().sprite = this.selected;
		float iconWidth = sample.GetComponent<RectTransform> ().rect.width;
		Destroy (sample);

		float barWidth = iconWidth * PlayerController.instance.inventory.Count;

		int newInventorySize = PlayerController.instance.inventory.Count;

		if (PlayerController.instance.inventory.Count != oldInventorySize) {
			oldInventorySize = newInventorySize;

			// inventory
			Destroy (this.inventory);
			this.inventory = new GameObject ();
			this.inventory.transform.parent = this.gameObject.transform;

			int i = 0;

			foreach (var item in PlayerController.instance.inventory) {
				var icon = new GameObject ();
				icon.AddComponent<Image> ().sprite = item.sprite;
				var rectTransform = icon.GetComponent<RectTransform> ();
				icon.transform.SetParent(this.inventory.transform);
				float x = this.GetComponent<RectTransform>().rect.width * this.GetComponent<Canvas>().scaleFactor - barWidth + i * rectTransform.rect.width;
				float y = this.GetComponent<RectTransform>().rect.height * this.GetComponent<Canvas>().scaleFactor - rectTransform.rect.width / 2;
				icon.transform.position = new Vector3 (x, y, 0);

				i++;
			}
		}

		if (newInventorySize > 0) {
			Destroy (this.border);
			this.border = new GameObject ();
			border.AddComponent<Image> ().sprite = this.selected;
			border.transform.SetParent (this.inventory.transform);
			var rt = border.GetComponent<RectTransform> ();

			var selX = this.GetComponent<RectTransform>().rect.width * this.GetComponent<Canvas>().scaleFactor - barWidth + PlayerController.instance.InventoryIndex * rt.rect.width;
			var selY = this.GetComponent<RectTransform>().rect.height * this.GetComponent<Canvas>().scaleFactor - rt.rect.height / 2;

			border.transform.position = new Vector3 (selX, selY, 0);

			Destroy (this.control);
			this.control = new GameObject ();

			switch (PlayerController.instance.inventory[PlayerController.instance.InventoryIndex].control) {
				case PlayerController.Control.MOUSE:
					control.AddComponent<Image> ().sprite = this.mouse;
				break;
				case PlayerController.Control.SPACE:
					control.AddComponent<Image> ().sprite = this.space;
				break;
			}

			Destroy (this.count);
			this.count = new GameObject ();

			switch(PlayerController.instance.inventory[PlayerController.instance.InventoryIndex].name){
				case "BowAndArrow":
					var arrowCount = count.AddComponent<Text> ();
					arrowCount.text = "" + PlayerController.instance.arrows;
					arrowCount.font = this.font;
					arrowCount.fontSize = 32;
				break;
				case "Rifle":
					var rifleCount = count.AddComponent<Text> ();
					rifleCount.text = "" + PlayerController.instance.bullets;
					rifleCount.font = this.font;
					rifleCount.fontSize = 32;
				break;
			}

			var outline = count.AddComponent<Outline>();
			control.transform.SetParent (this.inventory.transform);
			control.transform.position = new Vector3 (selX, selY, 0);

			count.transform.SetParent (this.inventory.transform);
			count.transform.position = new Vector3 (selX, selY, 0);
		}

		for (int i = 0; i < PlayerController.instance.inventory.Count - 1; i++) {
			if(PlayerController.instance.inventory[i].name == "BowAndArrow"){
				//show the ammount of arrows the player currently has

			}
			if(PlayerController.instance.inventory[i].name == "Rifle"){
				//show the amount of bullets the play currently has
			}
		}
	}

	Color colorForTile (Tile t) {
		switch (t.biome) {
		case 0:
			return new Color (66f / 255f, 86f / 255f, 0f / 255f);
			break;
		case 1:
			return new Color (208f / 255f, 207f / 255f, 108f / 255f);
			break;
		case 2:
			return new Color (166f / 255f, 167f / 255f, 55f / 255f);
			break;
		case 3:
			return new Color (167f / 255f, 97f / 255f, 63f / 255f);
			break;
		case 4:
			return new Color (231f / 255f, 255f / 255f, 255f / 255f);
			break;
		case 5:
			return new Color (225f / 255f, 236f / 255f, 143f / 255f);
			break;
		default:
			return Color.black;
			break;
		}
	}

	Texture2D CreateMap () {
		var map = new Texture2D (this.roomManager.rows * this.roomManager.roomSide,
		                         this.roomManager.columns * this.roomManager.roomSide);

		//print ("hahableh " + roomManager.tileMap [0, 0].biome);

		for (var x = 0; x < map.width; x++) {
			for (var y = 0; y < map.height; y++) {
				map.SetPixel(x, y, this.colorForTile (roomManager.tileMap [x, y]));
			}
		}

		map.Apply ();

		return map;
	}
}
