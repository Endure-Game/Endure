using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance = null;
	public int size = 3;

	// TODO: the RoomManager should probably hold this
	public GameObject[] rooms;

	private RoomManager roomScript;

	public GameObject[] roomTypes;

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

	private float playerWidth;
	private float playerHeight;

	private GameObject[,] rooms;
	private int roomX = 0;
	private int roomY = 0;

	// animation data
	public float transitionDuration = 1;

	private bool animating = false;
	private Vector3 oldPosition;
	private Vector3 newPosition;
	private float animationTime;

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

		// TODO: view note about this.rooms at top
		this.rooms = new GameObject[this.size, this.size];
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.animating) {
			float playerX = this.player.transform.position.x;
			float playerY = this.player.transform.position.y;

			this.TrackPlayer (playerX, playerY);
			this.CheckForRoomChange (playerX, playerY);
		} else {
			this.animationTime += Time.deltaTime;
			if (this.animationTime > this.transitionDuration) {
				this.animationTime = this.transitionDuration;
				this.animating = false;
			}

			var completed = this.animationTime / this.transitionDuration;
			this.camera.transform.position = Vector3.Lerp (this.oldPosition, this.newPosition, completed);
		}
	}

	void TrackPlayer (float playerX, float playerY) {
		this.camera.transform.position = this.CameraPosition(playerX, playerY);
	}

	Vector3 CameraPosition (float playerX, float playerY)
	{
		float inRoomX = playerX - this.CurrentRoom ().transform.position.x;
		float inRoomY = playerY - this.CurrentRoom ().transform.position.y;

		float newCameraX = playerX;
		float newCameraY = playerY;

		// only move camera to match player if the camera isn't at room edges

		newCameraX = this.CurrentRoom ().transform.position.x + this.Restrain (inRoomX, this.camera.getWidth (), this.roomWidth);
		newCameraY = this.CurrentRoom ().transform.position.y + this.Restrain (inRoomY, this.camera.getHeight (), this.roomHeight);

		return new Vector3 (newCameraX, newCameraY, this.camera.transform.position.z);
	}

	void CheckForRoomChange (float playerX, float playerY) {
		float inRoomX = playerX - this.CurrentRoom ().transform.position.x;
		float inRoomY = playerY - this.CurrentRoom ().transform.position.y;

		this.playerWidth = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * this.player.transform.localScale.x;
		this.playerHeight = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.y * this.player.transform.localScale.y;

		// detect when player hits boundary
		if (this.roomWidth / 2 - inRoomX <= this.playerWidth / 2) {
			// east
			this.AnimateCamera(1, 0, playerX, playerY, inRoomX, inRoomY);
		} else if (this.roomWidth / 2 + inRoomX <= this.playerWidth / 2) {
			// west
			this.AnimateCamera(-1, 0, playerX, playerY, inRoomX, inRoomY);
		} else if (this.roomHeight / 2 - inRoomY <= this.playerHeight / 2) {
			// north
			this.AnimateCamera(0, 1, playerX, playerY, inRoomX, inRoomY);
		} else if (this.roomHeight / 2 + inRoomY <= this.playerHeight / 2) {
			// south
			this.AnimateCamera(0, -1, playerX, playerY, inRoomX, inRoomY);
		}
	}

	void AnimateCamera (int x, int y, float playerX, float playerY, float inRoomX, float inRoomY) {
		var distance = new Vector3 (playerWidth * 1.5f, playerWidth * 1.5f, 1);
		this.player.transform.Translate (Vector3.Scale (distance, new Vector3(x, y, 0)));

		this.oldPosition = this.camera.transform.position;
		this.roomX += x;
		this.roomY += y;
		this.newPosition = this.CameraPosition (this.player.transform.position.x, this.player.transform.position.y);

		this.animationTime = 0;
		this.animating = true;
	}

	bool WithinOffset (float coordinate, float offset) {
		return coordinate <= offset - (offset / 2) && coordinate >= -offset / 2;
	}

	float Restrain (float coordinate, float cameraWidth, float roomWidth)
	{
		if (coordinate + cameraWidth / 2 > roomWidth / 2) {
			return roomWidth / 2 - cameraWidth / 2;
		} else if (coordinate - cameraWidth / 2 < -roomWidth / 2) {
			return -roomWidth / 2 + cameraWidth / 2;
		}

		return coordinate;
	}

	GameObject CurrentRoom () {
		return this.rooms [this.roomX, this.roomY];
	}
}
