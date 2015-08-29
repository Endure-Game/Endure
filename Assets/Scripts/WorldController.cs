using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance = null;
	public int size = 3;

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
	
	private float roomWidth;
	private float roomHeight;

	private float playerWidth;
	private float playerHeight;

	private int roomX = 0;
	private int roomY = 0;

	// animation data
	public float transitionDuration = 0.8f;

	private bool animating = false;
	private Vector3 oldCameraPosition;
	private Vector3 newCameraPosition;
	private Vector3 oldPlayerPosition;
	private Vector3 newPlayerPosition;
	private float animationTime;

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.camera = CameraController.instance;

		this.roomWidth = this.roomScript.columns;
		this.roomHeight = this.roomScript.rows;
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.animating) {
			this.TrackPlayer ();
			this.CheckForRoomChange ();
		} else {
			Animate ();
		}
	}

	void TrackPlayer () {
		this.camera.transform.position = this.CameraPosition(PlayerX (), PlayerY ());
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

	void CheckForRoomChange () {
		float inRoomX = PlayerX () - this.CurrentRoom ().transform.position.x;
		float inRoomY = PlayerY () - this.CurrentRoom ().transform.position.y;

		this.playerWidth = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * this.player.transform.localScale.x;
		this.playerHeight = this.player.GetComponent<SpriteRenderer>().sprite.bounds.size.y * this.player.transform.localScale.y;

		// detect when player hits boundary
		if (this.roomWidth / 2 - inRoomX <= this.playerWidth / 2) {
			// east
			this.AnimateCamera(1, 0, inRoomX, inRoomY);
		} else if (this.roomWidth / 2 + inRoomX <= this.playerWidth / 2) {
			// west
			this.AnimateCamera(-1, 0, inRoomX, inRoomY);
		} else if (this.roomHeight / 2 - inRoomY <= this.playerHeight / 2) {
			// north
			this.AnimateCamera(0, 1, inRoomX, inRoomY);
		} else if (this.roomHeight / 2 + inRoomY <= this.playerHeight / 2) {
			// south
			this.AnimateCamera(0, -1, inRoomX, inRoomY);
		}
	}

	void AnimateCamera (int x, int y, float inRoomX, float inRoomY) {
		this.oldPlayerPosition = this.player.transform.position;

		Vector3 distance = new Vector3 (playerWidth * 1.5f, playerWidth * 1.5f, 1);
		this.newPlayerPosition = this.player.transform.position + Vector3.Scale (distance, new Vector3(x, y, 0));

		this.oldCameraPosition = this.camera.transform.position;
		this.roomX += x;
		this.roomY += y;
		this.newCameraPosition = this.CameraPosition (this.player.transform.position.x, this.player.transform.position.y);

		this.animationTime = 0;
		this.animating = true;
	}

	void Animate ()
	{
		this.animationTime += Time.deltaTime;
		if (this.animationTime > this.transitionDuration) {
			this.animationTime = this.transitionDuration;
			this.animating = false;
		}

		float completed = this.animationTime / this.transitionDuration;
		this.camera.transform.position = Vector3.Lerp (this.oldCameraPosition, this.newCameraPosition, completed);
		this.player.transform.position = Vector3.Lerp (this.oldPlayerPosition, this.newPlayerPosition, completed);
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
		return this.roomScript.GetRoom(this.roomX, this.roomY);
	}

	float PlayerX ()
	{
		return this.player.transform.position.x;
	}

	float PlayerY ()
	{
		return this.player.transform.position.y;
	}
}
