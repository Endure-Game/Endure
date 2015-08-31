using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int rows = 32;
	public int columns = 32;
	public Count coinCount = new Count (4, 10);
	public Count blockingCount = new Count (5, 20);

	public int[,] tileMap;
	public int biomeNumber = 4;

	public GameObject[] forestTiles;
	public GameObject[] canyonTiles;
	public GameObject[] outerWallTiles;
	//need game objects for collectible items obstacles etc
	public GameObject[] coins;
	public GameObject[] blocks;

	private GameObject[,] rooms;

	List<Vector3> InitializeList (float gridX, float gridY) {
		// TODO: duplicated in RoomSetup, refactor
		float centerX = gridX * this.columns;
		float centerY = gridY * this.rows;

		List<Vector3> gridPositions = new List<Vector3> ();
		gridPositions.Clear ();
		for (int x = 1; x < columns - 1; x ++) {
			for (int y = 1; y < rows - 1; y ++) {
				float width = this.columns;
				float height = this.rows;

				float tileWidth = 1;
				float tileHeight = 1;

				float tileX = x + tileWidth / 2 - width / 2 + centerX;
				float tileY = y + tileHeight / 2 - height / 2 + centerY;

				gridPositions.Add(new Vector3 (tileX, tileY, 0f));
			}
		}

		return gridPositions;
	}

	GameObject RoomSetup (int gridX, int gridY) {

		GameObject room = new GameObject ("Room");
		Transform roomHolder = room.transform;

		float centerX = gridX * this.columns;
		float centerY = gridY * this.rows;

		room.transform.position = new Vector3 (centerX, centerY, room.transform.position.z);
		
		for (int x = 0; x < columns; x ++) {
			for(int y = 0; y < rows; y ++){

				// pick tileSet based on tile type
				GameObject[] tileSet;
				print (tileMap[x + gridX * this.columns, y + gridY * this.rows]);
				if (tileMap[x + gridX * this.columns, y + gridY * this.rows] == 0) {
					tileSet = this.forestTiles;
				} else {
					tileSet = this.canyonTiles;
				}
		
				GameObject toInstantiate = tileSet[Random.Range(0, tileSet.Length)];

				if ((x == 0 || x == columns - 1 || y == 0 || y == rows - 1) && x != 15 && y != 15) {
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				float width = this.columns;
				float height = this.rows;

				// TODO: probably don't hardcode, but definitely don't duplicate this in InitializeList
				float tileWidth = 1;
				float tileHeight = 1;

				float tileX = x + tileWidth / 2 - width / 2 + centerX;
				float tileY = y + tileHeight / 2 - height / 2 + centerY;

				GameObject instance = Instantiate (toInstantiate, new Vector3(tileX, tileY, 0f),Quaternion.identity) as GameObject;
				instance.transform.SetParent (roomHolder);
			}
		}

		return room;
	}

	Vector3 RandomPosition (List <Vector3> gridPositions) {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum, List<Vector3> gridPositions) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition(gridPositions);
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}

	}

	// Using Voronoi Generation to make a fill tileMap with tile numbers
	public void TileMapGeneration (int size) {
		tileMap = new int[size, size];

		// Generate list of points with a biome number associated
		List<Vector3> randomPoints = new List<Vector3>();
		int randomPointsRegion = (int)(size / Mathf.Sqrt(this.biomeNumber));
		for (int i = 0; i < size; i += randomPointsRegion) {
			for (int j = 0; j < size; j += randomPointsRegion) {
				Vector3 newPoint = new Vector3 (Random.Range (0, randomPointsRegion) + j,
				                                Random.Range (0, randomPointsRegion) + i,
				                                Random.Range (0, 2));
				
				randomPoints.Add(newPoint);
			}
		}

		// For each Tile, check for closest biome point
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {

				int distance = 1000;
				int closestIndex = 0;
				for (int pointI = 0; pointI < randomPoints.Count; pointI++) {
					Vector3 point = randomPoints[pointI];

					int dist = (int) (Mathf.Abs(point[0] - i) + Mathf.Abs(point[1] - j));
					if (dist < distance) {
						closestIndex = pointI;
						distance = dist;
					}
				}

				tileMap[i, j] = (int)randomPoints[closestIndex].z;
			}
		}

	}

	public void SetupRooms () {

		int roomSide = 3;
		this.rooms = new GameObject[roomSide, roomSide];

		TileMapGeneration(roomSide * this.rows);

		for (int i = 0; i < roomSide; i++) {
			for (int j = 0; j < roomSide; j++) {
				this.rooms [i, j] = RoomSetup (i, j);
				List<Vector3> gridPositions = InitializeList (i, j);
				
				//this is where we would call LayoutObjectAtRandom, we don't have any health, items, weapons, etc. yet
				//we do have an item though so...
				
				LayoutObjectAtRandom (coins, coinCount.minimum, coinCount.maximum, gridPositions);
				LayoutObjectAtRandom (blocks, blockingCount.minimum, blockingCount.maximum, gridPositions);
			}
		}
	}

	public GameObject GetRoom (int x, int y)
	{
		return this.rooms [x, y];
	}
	
}
