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
	public int[] end = new int[2] {31, 31};

	public int roomSide = 3;
	public int biomeNumber = 4;
	public Tile[,] tileMap;
	public List<Tile>[] regions;
	public List<Vector4> randomPoints;

	public GameObject[] outerWallTiles;

	//need game objects for collectible items obstacles etc
	public GameObject[] coins;
	public GameObject[] blocks;

	private GameObject[,] rooms;

	void Awake() {
		this.tileMap = new Tile[this.roomSide * this.columns, this.roomSide * this.rows];
	}

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

				GameObject toInstantiate;
				if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 0) {
					toInstantiate = this.ForestTile.getGroundTile();
				} else if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 1) {
					toInstantiate = this.DesertTile.getGroundTile();
				} else if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 2) {
					toInstantiate = this.PlainsTile.getGroundTile();
				} else if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 3) {
					toInstantiate = this.MountainTile.getGroundTile();
				} else if (tileMap[x + gridX * this.columns, y + gridY * this.rows].biome == 4) {
					toInstantiate = this.SnowTile.getGroundTile();
				} else {
					toInstantiate = this.BeachTile.getGroundTile();
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

	// Using Voronoi Generation to make a fill tileMap with tile numbers
	public void TileMapGeneration () {

		// Initialize tileMap and regions
		// ToDo: create private width and height variables
		int size = this.rows * this.roomSide;
		this.regions = new List<Tile>[this.biomeNumber];
		for (int i = 0; i < this.regions.Length; i++) {
			this.regions[i] = new List<Tile>();
		}

		// Generate list of points with a biome number associated
		this.randomPoints = new List<Vector4>();
		int randomPointsRegion = (int)(size / Mathf.Sqrt(this.biomeNumber));
		for (int i = 0; i < size; i += randomPointsRegion) {
			for (int j = 0; j < size; j += randomPointsRegion) {
				Vector4 newPoint = new Vector4 (Random.Range (0, randomPointsRegion) + j,
				                                Random.Range (0, randomPointsRegion) + i,
				                                Random.Range (0, 6), // biome
				                                Random.Range (0, 2)); // altitude

				randomPoints.Add(newPoint);
			}
		}

		int skip = 4;

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

				tileMap[i, j] = new Tile (i, j, closestIndex, (int)randomPoints[closestIndex].z, false, (int)randomPoints[closestIndex].w);
			}
		}

		for (int i = 0; i < size; i+=skip) {
			for (int j = 0; j < size; j+=skip) {

				Tile topLeft = tileMap[i, j];
				Tile topRight = (i + skip < size) ? tileMap[i + skip, j] : topLeft;
				Tile bottemLeft = (j + skip < size) ? tileMap[i, j + skip] : topLeft;
				Tile bottemRight = (j + skip < size && i + skip < size) ? tileMap[i + skip, j + skip] : topLeft;

				if (topLeft.biome != topRight.biome ||
				    topLeft.biome != bottemLeft.biome ||
				    topLeft.biome != bottemRight.biome ||
				    topLeft.elevation != topRight.elevation ||
				    topLeft.elevation != bottemLeft.elevation ||
				    topLeft.elevation != bottemRight.elevation) {

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

							tileMap[x, y] = new Tile (i, j, closestIndex, (int)randomPoints[closestIndex].z, false, (int)randomPoints[closestIndex].w);
							regions[closestIndex].Add(tileMap[x, y]);
						}
					}

				} else {

					for (int x = i; x < Mathf.Min(i + skip, size); x++) {
						for (int y = j; y < Mathf.Min(j + skip, size); y++) {
							tileMap[x, y] = new Tile (topLeft);
							regions[tileMap[x, y].regionIndex].Add(tileMap[x, y]);
						}
					}
				}
			}
		}

	}

	public void SetupRooms () {

		this.rooms = new GameObject[roomSide, roomSide];

		TileMapGeneration();

		// Create rooms
		for (int i = 0; i < roomSide; i++) {
			for (int j = 0; j < roomSide; j++) {
				this.rooms [i, j] = RoomSetup (i, j);
//				List<Vector3> gridPositions = InitializeList (i, j);

				//this is where we would call LayoutObjectAtRandom, we don't have any health, items, weapons, etc. yet
				//we do have an item though so...
//				LayoutObjectAtRandom (coins, coinCount.minimum, coinCount.maximum, gridPositions);
//				LayoutObjectAtRandom (blocks, blockingCount.minimum, blockingCount.maximum, gridPositions);
			}
		}

		// Create outer rock wall
		for (int x = 0; x < this.roomSide * this.columns; x++) {
			for (int y = 0; y < this.roomSide * this.rows; y++) {
				if (x == 0 || x == this.roomSide * this.rows - 1 || y == 0 || y == this.roomSide * this.columns - 1) {
					this.PlaceItem(outerWallTiles[Random.Range(0, outerWallTiles.Length)], x, y);
//					GameObject wallTile = Instantiate (outerWallTiles[Random.Range(0, outerWallTiles.Length)],
//					                                   new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f),
//					                                   Quaternion.identity) as GameObject;
//					wallTile.transform.SetParent(this.rooms[0,0].transform);
				}
			}
		}

		// Create altitude sprites
		this.ElevationTile.placeCliffTiles();

		// Create climb points
		for (int i = 0; i < this.randomPoints.Count; i++) {
			Vector4 point = this.randomPoints[i];

			if (i < this.roomSide) {
				int x = 1;
				Vector4 rightPoint = this.randomPoints[i + 1];
				while (point.x + x < rightPoint.x) {
					Destroy(this.tileMap[(int)point.x + x, (int)point.y].item);
					x++;
				}
			}

			if (i < this.randomPoints.Count - this.roomSide) {
				int y = 1;
				Vector4 lowerPoint = this.randomPoints[i + this.roomSide];
				while (point.y + y < lowerPoint.y) {
					Tile tile = this.tileMap[(int)point.x, (int)point.y + y];
					if (tile.item != null) {
						Destroy(tile.item);
						this.PlaceItem(this.ElevationTile.tiles[0], (int)point.x, (int)point.y + y);
//						tile.item = Instantiate (this.ElevationTile.tiles[0],
//						                         new Vector3((int)point.x - this.columns / 2 + .5f, (int)point.y + y - this.rows / 2 + .5f, 1f),
//						                         Quaternion.identity) as GameObject;
//						tile.item.transform.SetParent(this.rooms[0,0].transform);
					}
					y++;
				}
			}
		}

		// Create blocking tiles
		foreach (List<Tile> tiles in this.regions) {
			if (tiles[0].biome == 0) {
				this.ForestTile.RandomBlocking(tiles);
			} else if (tiles[0].biome == 1) {
				this.DesertTile.RandomBlocking(tiles);
			} else if (tiles[0].biome == 2) {
				this.PlainsTile.RandomBlocking(tiles);
			} else if (tiles[0].biome == 3) {
				this.MountainTile.RandomBlocking(tiles);
			} else if (tiles[0].biome == 4) {
				this.SnowTile.RandomBlocking(tiles);
			} else {
				this.BeachTile.RandomBlocking(tiles);
			}
		}

		//create game path
		//TODO fix sorting algo for randomPoints
		List <int[]> pointsDist = new List<int[]>();

		for (int i = 0; i < randomPoints.Count; i++) {
			int pointX = (int)randomPoints[i].x;
			int pointY = (int)randomPoints[i].y;
			float dist = Math.Abs(pointX - end[0]) + Math.Abs(pointY - end[1]);
			int[] tuple = new int[2] {i, (int)dist};
			pointsDist.Add(tuple);
		}
		//first item is index of randomPoints the second is the distance
		pointsDist.Sort ((a, b) => a [1].CompareTo (b [1]));

		for (int i = 0; i < pointsDist.Count - 1 ; i++) {
			int index1 = pointsDist[i][0];
			int index2 = pointsDist[i + 1][0];
		    float[] current = new float[2] {randomPoints[index1].x, randomPoints[index1].y};
		    float[] next = new float[2] {randomPoints[index2].x, randomPoints[index2].y};
			float xDiff = next[0] - current[0];
			float yDiff = next[1] - current[1];
			float moveX;
			float moveY;

			if(Mathf.Abs(xDiff) > Mathf.Abs(yDiff)){
				moveX = Mathf.Sign(xDiff);
				moveY = yDiff/Math.Abs(xDiff);
			}else{
				moveY = Mathf.Sign(yDiff);
				moveX = xDiff/Math.Abs (yDiff);
			}
			print("placing placePath");
			placePath(current, next, moveX, moveY);

		}



	}

	void placePath (float[] current, float[] next, float moveX, float moveY){

		print (moveY);
		print (moveX);
		float currentX = current [0];
		float currentY = current [1];
		float nextX = next [0];
		float nextY = next [1];

		while ((int)Mathf.Floor(currentX) != (int)Mathf.Floor(nextX) &&
		        (int)Mathf.Floor(currentY) != (int)Mathf.Floor(nextY)) //||
//		       ((int)Mathf.Floor(currentX) == (int)Mathf.Floor(nextX) &&
//				 (int)Mathf.Floor(currentY) != (int)Mathf.Floor(nextY)) ||
//		       ((int)Mathf.Floor(currentX) != (int)Mathf.Floor(nextX) &&
//				 (int)Mathf.Floor(currentY) == (int)Mathf.Floor(nextY)))
		{

			Tile tile = this.tileMap [(int)Mathf.Floor(currentX), (int)Mathf.Floor(currentY)];
			if(tile.item != null){
				Destroy (tile.item);
			}
			tile.item = Instantiate (this.elavationTiles[0],
			                         new Vector3((int)Mathf.Floor(currentX) - this.columns / 2 + .5f, (int)Mathf.Floor(currentY) - this.rows / 2 + .5f, 1f),
			                         Quaternion.identity) as GameObject;
			tile.item.transform.SetParent(this.rooms[0,0].transform);
			tile.path = true;
			currentX = currentX + moveX;

			tile = this.tileMap [(int)Mathf.Round(currentX), (int)Mathf.Round(currentY)];
			if(tile.item != null){
				Destroy (tile.item);
			}
			tile.item = Instantiate (this.elavationTiles[0],
			                         new Vector3((int)Mathf.Floor(currentX) - this.columns / 2 + .5f, (int)Mathf.Floor(currentY) - this.rows / 2 + .5f, 1f),
			                         Quaternion.identity) as GameObject;
			tile.item.transform.SetParent(this.rooms[0,0].transform);
			tile.path = true;
			currentY = currentY + moveY;

		}
	}
		// Fill all Tiles with blocking tiles
		for (int x = 1; x < this.roomSide * this.columns - 1; x++) {
			for (int y = 1; y < this.roomSide * this.rows - 1; y++) {
				Tile tile = tileMap[x, y];
				if (tile.blocking == true) {
					GameObject toInstantiate;
					if (tileMap[x, y].biome == 0) {
						toInstantiate = this.ForestTile.getBlockingTile();
					} else if (tileMap[x, y].biome == 1) {
						toInstantiate = this.DesertTile.getBlockingTile();
					} else if (tileMap[x, y].biome == 2) {
						toInstantiate = this.PlainsTile.getBlockingTile();
					} else if (tileMap[x, y].biome == 3) {
						toInstantiate = this.MountainTile.getBlockingTile();
					} else if (tileMap[x, y].biome == 4) {
						toInstantiate = this.SnowTile.getBlockingTile();
					} else {
						toInstantiate = this.BeachTile.getBlockingTile();
					}
					tile.item = Instantiate (toInstantiate,
					                         new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 1f),
					                         Quaternion.identity) as GameObject;
					tile.item.transform.SetParent(this.rooms[0,0].transform);
				}
			}
		}

		// Randomly distribute items throughout the game
		LayoutObjectAtRandom (coins, coinCount.minimum, coinCount.maximum);
		LayoutObjectAtRandom (blocks, blockingCount.minimum, blockingCount.maximum);

	}

	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			int x = Random.Range (0, this.roomSide * this.columns);
			int y = Random.Range (0, this.roomSide * this.rows);
			Tile tile = this.tileMap[x, y];
			if (tile.item == null) {
				this.PlaceItem(tileArray[Random.Range(0, tileArray.Length)], x, y);
				print ("placed");
			} else {
				i--;
			}
		}

	}

	public GameObject GetRoom (int x, int y) {
		return this.rooms [x, y];
	}

	public ForestTile ForestTile {
		get {
			return this.GetComponent<ForestTile> ();
		}
	}

	public DesertTile DesertTile {
		get {
			return this.GetComponent<DesertTile> ();
		}
	}

	public PlainsTile PlainsTile {
		get {
			return this.GetComponent<PlainsTile> ();
		}
	}

	public MountainTile MountainTile {
		get {
			return this.GetComponent<MountainTile> ();
		}
	}

	public SnowTile SnowTile {
		get {
			return this.GetComponent<SnowTile> ();
		}
	}

	public BeachTile BeachTile {
		get {
			return this.GetComponent<BeachTile> ();
		}
	}

	public ElevationTile ElevationTile {
		get {
			return this.GetComponent<ElevationTile> ();
		}
	}

	public void PlaceItem(GameObject sprite, int x, int y) {
		this.tileMap[x, y].item = Instantiate (sprite,
		                                       new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f),
		                                       Quaternion.identity) as GameObject;
		this.tileMap[x, y].item.transform.SetParent(this.rooms[0,0].transform);
	}
}
