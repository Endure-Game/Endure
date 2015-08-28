using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance = null;
	public int size = 3;
	public GameObject[] rooms;

	private RoomManager roomScript;
	private PlayerController player;
	private CameraController camera;

	void Awake () {
//		if (instance == null) {
//			instance = this;
//		} else if (instance != this) {
//			Destroy(gameObject);
//			DontDestroyOnLoad(gameObject);
			roomScript = GetComponent<RoomManager>();
			InitGame();
//		}
	}

	void InitGame () {
		roomScript.SetupRoom ();
	}

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.camera = CameraController.instance;

//		GameObject nextRoom = rooms[0];
//		float width = nextRoom.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
//		float scale = (float) nextRoom.transform.localScale.x;		
//		for (int i = 0; i < this.size; i++) {
//			for (int j = 0; j < this.size; j++){
//				Instantiate (rooms[0], new Vector3(i*width*scale, j*width*scale, 0), Quaternion.identity);
//			}
//		}
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: separate parts out into helper functions to make it less messy and monolithic
		float playerX = this.player.transform.position.x;
		float playerY = this.player.transform.position.y;

		float playerWidth = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * this.player.transform.localScale.x;
		float playerHeight = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.y * this.player.transform.localScale.y;

		float cameraX = this.camera.transform.position.x;
		float cameraY = this.camera.transform.position.y;

		GameObject nextRoom = rooms[0];
		float width = nextRoom.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		float height = nextRoom.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		float scale = (float) nextRoom.transform.localScale.x;

		float scaledWidth = width * scale;
		float verticalCameraPadding = scaledWidth - this.camera.getWidth ();

		float scaledHeight = height * scale;
		float horizontalCameraPadding = scaledWidth - this.camera.getHeight ();

		// make camera track player

		float newCameraX = cameraX;
		float newCameraY = cameraY;

		// only move camera to match player if the camera isn't at room edges
		if (playerX <= verticalCameraPadding - (verticalCameraPadding / 2) &&
			playerX >= -verticalCameraPadding / 2) {
			newCameraX = playerX;
		}

		if (playerY <= horizontalCameraPadding - (horizontalCameraPadding / 2) &&
		//if (playerY <= (scaledWidth - this.camera.getHeight ()) / 2 &&
		    playerY >= -horizontalCameraPadding / 2) {
			newCameraY = playerY;
		}

		this.camera.transform.position = new Vector3 (newCameraX,
		                                              newCameraY,
		                                   			  this.camera.transform.position.z);

		// detect when player hits boundary
		if (scaledWidth / 2 - playerX < playerWidth / 2) {
			// east
			this.ChangeRoom(new Vector2(-1, 0));
		} else if (scaledWidth / 2 + playerX < playerWidth / 2) {
			// west
			this.ChangeRoom(new Vector2(1, 0));
		} else if (scaledHeight / 2 - playerY < playerHeight / 2) {
			// north
			this.ChangeRoom(new Vector2(0, 1));
		} else if (scaledHeight / 2 + playerY < playerHeight / 2) {
			// south
			this.ChangeRoom(new Vector2(0, -1));
		}
	}

	void ChangeRoom (Vector2 direction)
	{
		this.camera.transform.Translate (0.1f * direction);
		this.player.transform.position = new Vector2 (0, 0);
	}
}
