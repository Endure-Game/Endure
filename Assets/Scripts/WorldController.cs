using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance = null;
	public int size = 3;

	private RoomManager roomScript;

	private PlayerController player;
	private CameraController camera;

	void InitGame () {
		roomScript.SetupRooms ();
	}
	
	private float roomWidth;
	private float roomHeight;

	private float playerWidth;
	private float playerHeight;

	private int numRooms;

	// Use this for initialization
	void Start () {

		roomScript = GetComponent<RoomManager>();
		InitGame();

		this.player = PlayerController.instance;
		this.camera = CameraController.instance;

		this.roomWidth = this.roomScript.columns;
		this.roomHeight = this.roomScript.rows;

		this.numRooms = this.roomScript.roomSide;
	}
	
	// Update is called once per frame
	void Update () {
		this.TrackPlayer ();
	}

	void TrackPlayer () {
		this.camera.transform.position = this.CameraPosition(PlayerX (), PlayerY ());
	}

	Vector3 CameraPosition (float playerX, float playerY)
	{

		float newCameraX = playerX;
		float newCameraY = playerY;

		// only move camera to match player if the camera isn't at room edges

		newCameraX = this.Restrain (playerX, this.camera.getWidth (), this.roomWidth);
		newCameraY = this.Restrain (playerY, this.camera.getHeight (), this.roomHeight);

		return new Vector3 (newCameraX, newCameraY, this.camera.transform.position.z);
	}

	float Restrain (float coordinate, float cameraWidth, float roomWidth)
	{
		if (coordinate < cameraWidth / 2 - roomWidth / 2) {
			return cameraWidth / 2 - roomWidth / 2;
		}

		if (coordinate > roomWidth * (this.numRooms - 0.5f) - cameraWidth / 2) {
			return roomWidth * (this.numRooms - 0.5f) - cameraWidth / 2;
		}

		return coordinate;
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
