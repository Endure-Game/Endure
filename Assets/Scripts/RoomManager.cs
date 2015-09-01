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

	public int roomSide = 3;
	public Tile[,] tileMap;
	public Vector4[] randomPoints;
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
				if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 0) {
					tileSet = this.forestTiles;
				} else {
					tileSet = this.canyonTiles;
				}
		
				GameObject toInstantiate = tileSet[Random.Range(0, tileSet.Length)];

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
		tileMap = new Tile[size, size];

		// Generate list of points with a biome number associated
		this.randomPoints = new List<Vector4>();
		int randomPointsRegion = (int)(size / Mathf.Sqrt(this.biomeNumber));
		for (int i = 0; i < size; i += randomPointsRegion) {
			for (int j = 0; j < size; j += randomPointsRegion) {
				Vector4 newPoint = new Vector4 (Random.Range (0, randomPointsRegion) + j,
				                                Random.Range (0, randomPointsRegion) + i,
				                                Random.Range (0, 2),
				                                Random.Range (0, 2));
				
				randomPoints.Add(newPoint);
			}
		}

		int skip = 5;

		// For each Tile, check for closest biome point
		for (int i = 0; i < size; i+=skip) {
			for (int j = 0; j < size; j+=skip) {

				int distance = 1000;
				int closestIndex = 0;
				for (int pointI = 0; pointI < randomPoints.Count; pointI++) {
					Vector4 point = randomPoints[pointI];

					int dist = (int) (Mathf.Abs(point[0] - i) + Mathf.Abs(point[1] - j));
					if (dist < distance) {
						closestIndex = pointI;
						distance = dist;
					}
				}

				tileMap[i, j] = new Tile ((int)randomPoints[closestIndex].z, false, (int)randomPoints[closestIndex].w);
			}
		}

		for (int i = 0; i < size; i+=skip) {
			for (int j = 0; j < size; j+=skip) {

//				if (i >= size - skip || j >= size -skip) {
//					for (int x = i; x < Mathf.Min(size, i + skip); x++) {
//						for (int y = j; y < Mathf.Min(size, j + skip); y++) {
//							int distance = 1000;
//							int closestIndex = 0;
//							for (int pointI = 0; pointI < randomPoints.Count; pointI++) {
//								Vector4 point = randomPoints[pointI];
//								
//								int dist = (int) (Mathf.Abs(point[0] - x) + Mathf.Abs(point[1] - y));
//								if (dist < distance) {
//									closestIndex = pointI;
//									distance = dist;
//								}
//							}
//							
//							tileMap[x, y] = new Tile ((int)randomPoints[closestIndex].z, false, (int)randomPoints[closestIndex].w);;
//						}
//					}
//				
//				} else 

				Tile topLeft = tileMap[i, j];
				Tile topRight = (i >= size) ? tileMap[i + skip, j] : topLeft;
				Tile bottemLeft = (j >= size) ? tileMap[i, j + skip] : topLeft;
				Tile bottemRight = (j >= size) ? tileMap[i + skip, j + skip] : topLeft;

				if (topLeft.biome != topRight.biome ||
				    topLeft.biome != bottemLeft.biome ||
				    topLeft.biome != bottemRight.biome) {

					for (int x = i; x < Mathf.Min(i + skip, size); x++) {
						for (int y = j; y < Mathf.Min(j + skip, size); y++) {
							
							int distance = 1000;
							int closestIndex = 0;
							for (int pointI = 0; pointI < randomPoints.Count; pointI++) {
								Vector4 point = randomPoints[pointI];
								
								int dist = (int) (Mathf.Abs(point[0] - x) + Mathf.Abs(point[1] - y));
								if (dist < distance) {
									closestIndex = pointI;
									distance = dist;
								}
							}
							
							tileMap[x, y] = new Tile ((int)randomPoints[closestIndex].z, false, (int)randomPoints[closestIndex].w);
						}
					}
				
				} else {

					for (int x = i; x < Mathf.Min(i + skip, size); x++) {
						for (int y = j; y < Mathf.Min(j + skip, size); y++) {
							tileMap[x, y] = new Tile (topLeft);
						}
					}
				}
			}
		}

	}

	public void SetupRooms () {

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

		// Create outer rock wall
		for (int x = 0; x < this.roomSide * this.columns; x++) {
			for (int y = 0; y < this.roomSide * this.rows; y++) {
				if (x == 0 || x == this.roomSide * this.rows - 1 || y == 0 || y == this.roomSide * this.columns - 1) {
					GameObject wallTile = Instantiate (outerWallTiles[Random.Range(0, outerWallTiles.Length)], 
					                                   new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f), 
					                                   Quaternion.identity) as GameObject;
					wallTile.transform.SetParent(this.rooms[0,0].transform);
				}
			}
		}

		// Create altitude sprites
		for (int x = 1; x < this.roomSide * this.columns - 1; x++) {
			for (int y = 1; y < this.roomSide * this.rows - 1; y++) {

				bool lower = false;
				for (int xDelta = -1; xDelta <= 1; xDelta++) {
					for (int yDelta = -1; yDelta <= 1; yDelta++) {
						if (this.tileMap[x + xDelta, y + yDelta].elevation > this.tileMap[x, y].elevation) {
							lower = true;
						}
					}
				}

				if (lower) {
					this.tileMap[x, y].item = Instantiate (outerWallTiles[Random.Range(0, outerWallTiles.Length)], 
					                                   new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f), 
					                                   Quaternion.identity) as GameObject;
					this.tileMap[x, y].item.transform.SetParent(this.rooms[0,0].transform);
				}
			}
		}

		// Create 
	}

	public GameObject GetRoom (int x, int y)
	{
		return this.rooms [x, y];
	}
	
}
