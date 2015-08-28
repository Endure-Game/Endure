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

	// TODO: make better
	private bool cameraLock = false;

	private float roomWidth;
	private float roomHeight;

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

		// TODO: actually calculate this
		this.roomWidth = 32;
		this.roomHeight = 32;

	}
	
	// Update is called once per frame
	void Update () {
		if (!this.cameraLock) {
			TrackPlayer ();
			CheckForRoomChange ();
		}
	}

	void TrackPlayer () {
		float playerX = this.player.transform.position.x;
		float playerY = this.player.transform.position.y;

		float cameraX = this.camera.transform.position.x;
		float cameraY = this.camera.transform.position.y;

		float verticalCameraPadding = this.roomWidth - this.camera.getWidth ();
		float horizontalCameraPadding = this.roomHeight - this.camera.getHeight ();

		// make camera track player

		float newCameraX = cameraX;
		float newCameraY = cameraY;

		// only move camera to match player if the camera isn't at room edges
		if (playerX <= verticalCameraPadding - (verticalCameraPadding / 2) &&
		    playerX >= -verticalCameraPadding / 2) {
			newCameraX = playerX;
		}

		if (playerY <= horizontalCameraPadding - (horizontalCameraPadding / 2) &&
		    playerY >= -horizontalCameraPadding / 2) {
			newCameraY = playerY;
		}

		this.camera.transform.position = new Vector3 (newCameraX,
		                                              newCameraY,
		                                              this.camera.transform.position.z);
	}

	void CheckForRoomChange() {
		float playerX = this.player.transform.position.x;
		float playerY = this.player.transform.position.y;

		float playerWidth = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * this.player.transform.localScale.x;
		float playerHeight = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.y * this.player.transform.localScale.y;

		// detect when player hits boundary
		if (this.roomWidth / 2 - playerX < playerWidth / 2) {
			// east
			this.ChangeRoom (new Vector2 (-1, 0));
		} else if (this.roomWidth / 2 + playerX < playerWidth / 2) {
			// west
			this.ChangeRoom (new Vector2 (1, 0));
		} else if (this.roomHeight / 2 - playerY < playerHeight / 2) {
			// north
			this.ChangeRoom (new Vector2 (0, 1));
		} else if (this.roomHeight / 2 + playerY < playerHeight / 2) {
			// south
			this.ChangeRoom (new Vector2 (0, -1));
		}
	}

	void ChangeRoom (Vector2 direction)
	{
		this.cameraLock = true;
		this.camera.transform.Translate (this.roomHeight * direction);
		this.player.transform.position = new Vector2 (0, 0);
	}
}
