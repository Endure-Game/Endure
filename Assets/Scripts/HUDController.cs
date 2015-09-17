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

	public Sprite arrow;
	public Sprite bullet;

	public GameObject world;
	private RoomManager roomManager;

	public GameObject map;
	public GameObject mapCover;
	private GameObject inventory;
	private int oldInventorySize = 0;
	private GameObject border;
	private GameObject control;
	private GameObject count;

	private Health playerHealth;
	private int startingMaxHealth;

	public Text pauseText;

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

		this.CreateMapTexture();
	}

	// Update is called once per frame
	void Update () {


		this.UpdateMapTexture();

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
				this.pauseText.text = "PAUSED [ESC]";

			} else {
				Time.timeScale = 1;
				this.pauseText.text = "";
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
				float x = this.GetComponent<RectTransform>().rect.width * this.GetComponent<Canvas>().scaleFactor - barWidth + i * rectTransform.rect.width + 40;
				float y = this.GetComponent<RectTransform>().rect.height * this.GetComponent<Canvas>().scaleFactor - rectTransform.rect.width / 2;
				icon.transform.position = new Vector3 (x, y, 0);

				i++;

				// Give a number to each icon, indicating it's button
				var number = new GameObject ();
				var text = number.AddComponent<Text> ();
				text.text = "" + i;
				text.font = this.font;
				text.fontSize = 32;
				number.AddComponent<Outline>();

				number.transform.SetParent (icon.transform);
				number.transform.position = new Vector3 (x + 4, y - 4, 0);
			}
		}

		if (newInventorySize > 0) {
			Destroy (this.border);
			this.border = new GameObject ();
			border.AddComponent<Image> ().sprite = this.selected;
			border.transform.SetParent (this.inventory.transform);
			var rt = border.GetComponent<RectTransform> ();

			var selX = this.GetComponent<RectTransform>().rect.width * this.GetComponent<Canvas>().scaleFactor - barWidth + PlayerController.instance.InventoryIndex * rt.rect.width  + 40;
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

			control.transform.SetParent (this.inventory.transform);
			control.transform.position = new Vector3 (selX, selY, 0);
		}

		for (int i = 0; i < PlayerController.instance.inventory.Count - 1; i++) {
			if(PlayerController.instance.inventory[i].name == "BowAndArrow"){
				//show the ammount of arrows the player currently has
			}
			if(PlayerController.instance.inventory[i].name == "Rifle"){
				//show the ammount of bullets the player currently has
			}
		}

		// Create ammo count objects
		Destroy (this.count);
		this.count = new GameObject ();

		float xCount = this.GetComponent<RectTransform>().rect.width * this.GetComponent<Canvas>().scaleFactor - 20;
		float yCount = this.GetComponent<RectTransform>().rect.height * this.GetComponent<Canvas>().scaleFactor - iconWidth * 2 + 40;

		var arrowCount = new GameObject();

		var arrowNumber = arrowCount.AddComponent<Text> ();
		arrowNumber.text = "x" + PlayerController.instance.arrows;
		arrowNumber.font = this.font;
		arrowNumber.fontSize = 32;
		arrowCount.AddComponent<Outline>();
		var arrowSprite = new GameObject();
		arrowSprite.AddComponent<Image> ().sprite = this.arrow;
		arrowSprite.transform.localScale = new Vector3 (.4f, .4f, 1f);
		arrowSprite.transform.SetParent (arrowCount.transform);
		arrowSprite.transform.position = new Vector3 (-70f, 35f, 0f);
		arrowCount.transform.SetParent (this.count.transform);
		arrowCount.transform.position = new Vector3 (xCount, yCount, 0f);

		var rifleCount = new GameObject ();
		var rifleNumber = rifleCount.AddComponent<Text> ();
		rifleNumber.text = "x" + PlayerController.instance.bullets;
		rifleNumber.font = this.font;
		rifleNumber.fontSize = 32;
		rifleCount.AddComponent<Outline>();
		var rifleSprite = new GameObject();
		rifleSprite.AddComponent<Image> ().sprite = this.bullet;
		rifleSprite.transform.localScale = new Vector3 (.4f, .4f, 1f);
		rifleSprite.transform.SetParent (rifleCount.transform);
		rifleSprite.transform.position = new Vector3 (-70f, 35f, 0f);
		rifleCount.transform.SetParent (this.count.transform);
		rifleCount.transform.position = new Vector3 (xCount, yCount - 35, 0f);

		this.count.transform.SetParent (this.inventory.transform);
		this.count.transform.position = new Vector3 (0, 0, 0);
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

	// Instantiates mapTexture to clear except for starting area around the player
	private void CreateMapTexture () {
		this.mapTexture = new Texture2D (this.roomManager.rows * this.roomManager.roomSide,
		                         				 this.roomManager.columns * this.roomManager.roomSide);
		for (var x = 0; x < mapTexture.width; x++) {
			for (var y = 0; y < mapTexture.height; y++) {
				mapTexture.SetPixel(x, y, Color.clear);
			}
		}
		int playerX = (int)(PlayerController.instance.transform.position.x + 15.5f);
		int playerY = (int)(PlayerController.instance.transform.position.y + 15.5f);

		int range = 6;
		for (var x = playerX - range; x < playerX + range; x++) {
			for (var y = playerY - range; y < playerY + range; y++) {
				mapTexture.SetPixel(x, y, this.colorForTile (roomManager.tileMap [x, y]));
			}
		}
		mapTexture.Apply();
	}

	// Only updates the pixels on the rim of the player view
	private void UpdateMapTexture () {

		int playerX = (int)(PlayerController.instance.transform.position.x + 15.5f);
		int playerY = (int)(PlayerController.instance.transform.position.y + 15.5f);

		int range = 6;
		for (var x = playerX - range; x < playerX + range; x++) {
			mapTexture.SetPixel(x, playerY - range, this.colorForTile (roomManager.tileMap [x, playerY - range]));
			mapTexture.SetPixel(x, playerY + range, this.colorForTile (roomManager.tileMap [x, playerY + range]));
		}
		for (var y = playerY - range; y < playerY + range; y++) {
			mapTexture.SetPixel(playerX - range, y, this.colorForTile (roomManager.tileMap [playerX - range, y]));
			mapTexture.SetPixel(playerX + range, y, this.colorForTile (roomManager.tileMap [playerX + range, y]));
		}
		mapTexture.Apply();

		this.mapCover.GetComponent<Image> ().sprite = Sprite.Create(this.mapTexture, new Rect(0, 0, this.mapTexture.width, this.mapTexture.height), new Vector2 ());
	}

	// Updates entire mapTexture
	public void FillMapTexture () {

		for (var x = 0; x < mapTexture.width; x++) {
			for (var y = 0; y < mapTexture.height; y++) {
				mapTexture.SetPixel(x, y, this.colorForTile (roomManager.tileMap [x, y]));
			}
		}
		mapTexture.Apply();

		this.mapCover.GetComponent<Image> ().sprite = Sprite.Create(this.mapTexture, new Rect(0, 0, this.mapTexture.width, this.mapTexture.height), new Vector2 ());
	}

}
