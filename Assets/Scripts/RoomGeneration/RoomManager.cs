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
	public int[] end = new int[2] {255, 255};
	public int enemySpawnInterval = 100;

	public int roomSide = 3;
	public int biomeNumber = 4;
	public Tile[,] tileMap;
	public List<Region> regions;

	public GameObject[] outerWallTiles;

	//need game objects for collectible items obstacles etc
	public GameObject[] coins;
	public GameObject[] blocks;

	private GameObject[,] rooms;

	void Awake() {
		this.tileMap = new Tile[this.roomSide * this.columns, this.roomSide * this.rows];
	}

	private int timer = 0;
	void Update() {
		// spawn enemies
		timer++;
		if (timer % this.enemySpawnInterval == 0) {
			this.regions[Random.Range(0, this.regions.Count)].spawnEnemy();
			print ("enemySpawned");
		}
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

				Tile tile = tileMap[x + gridX * this.columns, y + gridY * this.rows];
				GameObject toInstantiate;
				if (tile.biome == ForestTile.BiomeNumber) {
					toInstantiate = this.ForestTile.getGroundTile();
				} else if (tile.biome == DesertTile.BiomeNumber) {
					toInstantiate = this.DesertTile.getGroundTile();
				} else if (tile.biome == PlainsTile.BiomeNumber) {
					toInstantiate = this.PlainsTile.getGroundTile();
				} else if (tile.biome == MountainTile.BiomeNumber) {
					toInstantiate = this.MountainTile.getGroundTile();
				} else if (tile.biome == SnowTile.BiomeNumber) {
					toInstantiate = this.SnowTile.getGroundTile();
				} else {
					toInstantiate = this.BeachTile.getGroundTile();
				}

				SetGroundTile(toInstantiate, x + gridX * 32, y + gridY * 32);
				tileMap[x, y].ground.transform.SetParent (roomHolder);
			}
		}

		return room;
	}

	// Using Voronoi Generation to make a fill tileMap with tile numbers
	public void TileMapGeneration () {

		int size = this.rows * this.roomSide;

		// Generate list of points with a biome number associated
		this.regions = new List<Region>();
		int randomPointsRegion = (int)(size / Mathf.Sqrt(this.biomeNumber));
		for (int i = 0; i < size; i += randomPointsRegion) {
			for (int j = 0; j < size; j += randomPointsRegion) {

				int randomX = Random.Range (0, randomPointsRegion) + j;
				int randomY = Random.Range (0, randomPointsRegion) + i;
				int biomeIndex = Random.Range (0, 6);
				int altitude = Random.Range (0, 2);

				BiomeTile biome;
				if (biomeIndex == 0) {
					biome = this.ForestTile;
				} else if (biomeIndex == 1) {
					biome = this.DesertTile;
				} else if (biomeIndex == 2) {
					biome = this.PlainsTile;
				} else if (biomeIndex == 3) {
					biome = this.MountainTile;
				} else if (biomeIndex == 4) {
					biome = this.SnowTile;
				} else {
					biome = this.BeachTile;
				}
				this.regions.Add(new Region(randomX, randomY, biome, altitude));
			}
		}

//		For each Tile, check for closest biome point
		int skip = 15;
		for (int i = 0; i < size; i+=skip) {
			for (int j = 0; j < size; j+=skip) {

				this.getBiome(i, j);
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

					for (int y = j; y < Mathf.Min(j + skip, size); y++) {
						this.getBiome(i, y);
					}

					for (int y = j; y < Mathf.Min(j + skip, size); y++) {

						this.getBiome(i, y);
						if (i + skip < size) {
							this.getBiome(i + skip, y);
						}
						Tile left = tileMap[i, y];
						Tile right = (i + skip < size) ? tileMap[i + skip, y] : left;

						if (left.biome != right.biome ||
						    left.elevation != right.elevation) {

							for (int x = i + 1; x < Mathf.Min(i + skip, size); x++) {
								this.getBiome(x, y);
							}
						} else {

							for (int x = i + 1; x < Mathf.Min(i + skip, size); x++) {
								tileMap[x, y] = new Tile (left, x, y);
								this.regions[tileMap[x, y].regionIndex].tiles.Add(tileMap[x, y]);
							}
						}
					}

				} else {

					for (int x = i; x < Mathf.Min(i + skip, size); x++) {
						for (int y = j; y < Mathf.Min(j + skip, size); y++) {
							tileMap[x, y] = new Tile (topLeft, x, y);
							this.regions[tileMap[x, y].regionIndex].tiles.Add(tileMap[x, y]);
						}
					}
				}
			}
		}

	}

	public void SetupRooms () {

		float startTime = Time.realtimeSinceStartup;
		this.rooms = new GameObject[roomSide, roomSide];

		TileMapGeneration();
		print (Time.realtimeSinceStartup - startTime);

		// Create rooms
		for (int i = 0; i < roomSide; i++) {
			for (int j = 0; j < roomSide; j++) {
				this.rooms [i, j] = RoomSetup (i, j);
			}
		}
		print (Time.realtimeSinceStartup - startTime);

		// Create outer rock wall
		for (int x = 0; x < this.roomSide * this.columns; x++) {
			for (int y = 0; y < this.roomSide * this.rows; y++) {
				if (x == 0 || x == this.roomSide * this.rows - 1 || y == 0 || y == this.roomSide * this.columns - 1) {
					this.PlaceItem(outerWallTiles[Random.Range(0, outerWallTiles.Length)], x, y);
				}
			}
		}
		print (Time.realtimeSinceStartup - startTime);

		//create game path
		//TODO fix sorting algo for randomPoints
		List<Vector2> randomPoints = new List<Vector2>();
		foreach (Region region in this.regions) {
			randomPoints.Add(new Vector2(region.focusX, region.focusY));
		}

		List <int[]> pointsDist = new List<int[]>();
		Vector2 start = new Vector2 (16f, 16f);
		Vector2 exit = new Vector2 (255f, 255f);
		randomPoints.Insert (0, start);
		randomPoints.Add (exit);


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
			placePath(current, next, moveX, moveY);

		}
		print (Time.realtimeSinceStartup - startTime);

		// Create altitude sprites
		this.ElevationTile.placeCliffTiles();
		print (Time.realtimeSinceStartup - startTime);
		
		// Create climb points
		for (int i = 0; i < this.regions.Count; i++) {
			Region region = this.regions[i];
			
			if (i < this.roomSide) {
				int x = 1;
				Region rightRegion = this.regions[i + 1];
				while (region.focusX + x < rightRegion.focusX) {
					Destroy(this.tileMap[region.focusX + x, region.focusY].item);
					x++;
				}
			}
			
			if (i < this.regions.Count - this.roomSide) {
				int y = 1;
				Region upperRegion = this.regions[i + this.roomSide];
				while (region.focusY + y < upperRegion.focusY) {
					Tile tile = this.tileMap[region.focusX, region.focusY + y];
					if (tile.item != null) {
						Destroy(tile.item);
						this.SetGroundTile(this.ElevationTile.tiles[0], region.focusX, region.focusY + y);
						this.tileMap[region.focusX, region.focusY].blocking = true;
					}
					y++;
				}
			}
		}
		print (Time.realtimeSinceStartup - startTime);
		
		// Create blocking tiles
		foreach (Region region in this.regions) {
			region.makeBlocking();
		}
		print (Time.realtimeSinceStartup - startTime);

		// Randomly distribute items throughout the game
		LayoutObjectAtRandom (coins, coinCount.minimum, coinCount.maximum);
		LayoutObjectAtRandom (blocks, blockingCount.minimum, blockingCount.maximum);
		print (Time.realtimeSinceStartup - startTime);

		// Spawn starting enemies
		for (int i = 0; i < 100; i++) {
			Region region = this.regions[Random.Range(0, this.regions.Count)];
			region.spawnEnemy();
		}
		print (Time.realtimeSinceStartup - startTime);

	}

	void placePath (float[] current, float[] next, float moveX, float moveY){

		float currentX = current [0];
		float currentY = current [1];
		float nextX = next [0];
		float nextY = next [1];

		if (currentX > 1 && currentX < this.roomSide * this.rows - 1 && currentY > 1 && currentY < this.roomSide * this.columns - 1) {

			while (((int)Mathf.Floor(currentX) != (int)Mathf.Floor(nextX) &&
		       (int)Mathf.Floor(currentY) != (int)Mathf.Floor(nextY)) ||

			    ((int)Mathf.Floor(currentX) == (int)Mathf.Floor(nextX) &&
			    (int)Mathf.Floor(currentY) != (int)Mathf.Floor(nextY))) {


				Tile tile = this.tileMap [(int)Mathf.Floor (currentX), (int)Mathf.Floor (currentY)];
				//DONT TOUCH MY MAGIC IF BLOCK - IAN
				if (currentX - 1 > 3) {
					Tile westTile = this.tileMap [(int)Mathf.Floor (currentX) - 1, (int)Mathf.Floor (currentY)];
					westTile.path = true;
				}
				if (currentX + 1 < (this.roomSide * this.rows) - 3) {
					Tile eastTile = this.tileMap [(int)Mathf.Floor (currentX) + 1, (int)Mathf.Floor (currentY)];
					eastTile.path = true;
				}
				if (currentY + 1 < (this.roomSide * this.columns) - 3) {
					Tile northTile = this.tileMap [(int)Mathf.Floor (currentX), (int)Mathf.Floor (currentY) + 1];
					northTile.path = true;
				}
				if (currentY - 1 > 3) {
					Tile southTile = this.tileMap [(int)Mathf.Floor (currentX), (int)Mathf.Floor (currentY) - 1];
					southTile.path = true;
				}

				tile.path = true;
				currentX = currentX + moveX;
				if (currentX > 1 && currentX < this.roomSide * this.rows - 1 && currentY > 1 && currentY < this.roomSide * this.columns - 1) {
					tile = this.tileMap [(int)Mathf.Floor (currentX), (int)Mathf.Floor (currentY)];
					tile.path = true;
				}
				currentY = currentY + moveY;

			}
		}

	}

	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {

		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			int x = Random.Range (0, this.roomSide * this.columns);
			int y = Random.Range (0, this.roomSide * this.rows);
			Tile tile = this.tileMap[x, y];
			if (tile.item == null && tile.blocking == false) {
				this.PlaceItem(tileArray[Random.Range(0, tileArray.Length)], x, y);
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

		Tile tile = this.tileMap[x, y];
		tile.item = Instantiate (sprite,
			                         new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f),
			                         Quaternion.identity) as GameObject;
		tile.item.transform.SetParent(this.rooms[0,0].transform);
		tile.blocking = true;

		// Make higher tiles apear behind lower tiles
		tile.item.transform.Translate(new Vector3(0, 0, y));
	}

	public void SetGroundTile(GameObject sprite, int x, int y) {
		Destroy (this.tileMap[x, y].ground);

		this.tileMap[x, y].ground = Instantiate (sprite,
		                                       new Vector3(x - this.columns / 2 + .5f, y - this.rows / 2 + .5f, 0f),
		                                       Quaternion.identity) as GameObject;
	}

	private void getBiome(int x, int y) {
		if (this.tileMap[x, y] != null) {
			return;
		}

		int distance = 1000;
		int closestIndex = 0;
		for (int regionI = 0; regionI < this.regions.Count; regionI++) {
			Region region = this.regions[regionI];
			
			int dist = (int) (Mathf.Abs(region.focusX - x) + Mathf.Abs(region.focusY - y));
			if (dist < distance) {
				closestIndex = regionI;
				distance = dist;
			}
		}

		Region closest = this.regions[closestIndex];
		tileMap[x, y] = new Tile (x, y, closestIndex, closest.biome.getBiomeNumber(), false, closest.altitude);
		this.regions[closestIndex].tiles.Add(tileMap[x, y]);
	}
}
