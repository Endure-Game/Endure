using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BiomeTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	public Spawn[] enemies;

	private Tile[,] myTileMap = null;
	protected Tile[,] tileMap {
		get {
			if (this.myTileMap == null) {
				this.myTileMap = this.GetComponent<RoomManager>().tileMap;
			}
			return this.myTileMap;
		}
	}

	private int myWidth = 0;
	protected int width {
		get {
			if (this.myWidth == 0) {
				this.myWidth = this.tileMap.GetLength(1);
			}
			return this.myWidth;
		}
	}

	private int myHeight = 0;
	protected int height {
		get {
			if (this.myHeight == 0) {
				this.myHeight = this.tileMap.GetLength(0);
			}
			return this.myHeight;
		}
	}

	//protected Tile[,] tileMap;
	//protected int width;
	//protected int height;

	public RoomManager.Count treasureCount = new RoomManager.Count(1, 2);
	public RoomManager.Count chestCount = new RoomManager.Count(1, 2);
	public RoomManager.Count tentCount = new RoomManager.Count(1, 2);

	[System.Serializable]
	public class Spawn {
		public float chance;
		public GameObject enemy;
	}

	/*
	void Start() {
		this.tileMap = this.GetComponent<RoomManager>().tileMap;
		this.height = this.tileMap.GetLength(0);
		this.width = this.tileMap.GetLength(1);
	}*/

	public delegate void TilePlacer(int x, int y);
	// Blooms are great for making irregular, but roundish shapes like bodies of water
	public void BlockingExplosion(int x, int y, int level, TilePlacer spritePlacer) {

		if (Random.Range (0, level) < 1 || x < 0 || y < 0 || x >= this.width || y >= this.height) {
			return;
		}

		Tile tile = this.tileMap[x, y];

		if (tile.biome != this.getBiomeNumber() || tile.blocking || tile.path) {
			return;
		}

		if (tile.item == null) {
			spritePlacer(x, y);
			tile.blocking = true;
		}

		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (Random.Range(0, 10) > 3 && (j != 0 || i == 1)) {
					BlockingExplosion(x + i, y + j, level - 1, spritePlacer);
				}
			}
		}
	}

	// Perlin is faster than blooms and creates a maze-like structure, which is great for forests
	// blockingRatio: float between 0f and 1f; 1f is no blocking and 0f is all blocking
	// blockingSize: how large thick the walls of the maze are, recommended .2f
	public void PerlinGenerator(List<Tile> tiles, TilePlacer spritePlacer, float blockingRatio, float blockingSize) {

		foreach (Tile tile in tiles) {
			float noise = Mathf.PerlinNoise((float)tile.x * blockingSize, (float)tile.y * blockingSize);
			if (noise > blockingRatio && !tile.path && tile.item == null) {
				tile.blocking = true;
				spritePlacer(tile.x, tile.y);
			}
		}
	}

	public virtual GameObject getGroundTile() {
		return this.groundTiles[Random.Range(0, this.groundTiles.Length)];
	}

	public GameObject getBlockingTile() {
		return this.blockingTiles[Random.Range(0, this.blockingTiles.Length)];
	}

	public void placeBlockingTile(int x, int y) {
		this.GetComponent<RoomManager>().PlaceItem(this.getBlockingTile(), x, y);
	}

	public void placeTreasureTiles(List<Tile> tiles) {

		int treasureNum = Random.Range(this.treasureCount.minimum, this.treasureCount.maximum);
		for (int i = 0; i < treasureNum; i++) {
			Tile treasureTile = tiles[Random.Range(0, tiles.Count)];
			while (treasureTile.item != null) {
				treasureTile = tiles[Random.Range(0, tiles.Count)];
			}

			this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().xMarksTheSpot,
																								 treasureTile.x,
																								 treasureTile.y);
		}
	}

	public void placeChestTiles(List<Tile> tiles) {

		int chestNum = Random.Range(this.chestCount.minimum, this.chestCount.maximum);
		for (int i = 0; i < chestNum; i++) {
			Tile chestTile = tiles[Random.Range(0, tiles.Count)];
			while (chestTile.item != null) {
				chestTile = tiles[Random.Range(0, tiles.Count)];
			}

			this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().chest,
																								 chestTile.x,
																								 chestTile.y);
		}
	}

	public void placeCamps(List<Tile> tiles) {

		int tentNum = Random.Range(this.tentCount.minimum, this.tentCount.maximum + 1);
		for (int i = 0; i < tentNum; i++) {
			Tile tentTile = tiles[Random.Range(0, tiles.Count)];
			while (tentTile.item != null) {
				tentTile = tiles[Random.Range(0, tiles.Count)];
			}

			this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().tent,
																								 tentTile.x,
																								 tentTile.y);

			// place enemies and item nearby if there is room
			int enemyMax = Random.Range(3, 5);
			int enemies = 0;
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {

					int enemyX = x + tentTile.x;
					int enemyY = y + tentTile.y;

					if (this.inMap(enemyX, enemyY) &&
							!tileMap[enemyX, enemyY].blocking &&
							enemies < enemyMax) {

						// place item for first open spot, then place enemies
						if (enemies == 0) {
							this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().randomItem,
																												 enemyX,
																												 enemyY);
						} else {
							this.GetComponent<RoomManager>().PlaceItem(this.getEnemy(),
																											 enemyX,
																											 enemyY);
						}
						enemies++;
					}
				}
			}



			// place reward item in front of tent


		}
	}

	public Tile GetOpenArea(List<Tile> tiles) {

		int biome = tiles[0].biome;
		foreach (Tile tile in tiles) {

			bool open = true;
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {

					int xPos = tile.x + x;
					int yPos = tile.y + y;

					if (xPos < 0 || yPos < 0 || xPos >= width || yPos >= height ||
							this.tileMap[xPos, yPos].blocking || biome != tile.biome) {
						open = false;
					}
				}
			}

			if (open) {
				return tile;
			}
		}

		return null;
	}

	public void RandomFlip(GameObject item) {
		if (Random.Range(0, 2) == 1) {
	    item.transform.localScale = new Vector3(-1f, 1f, 1f);
	  }
	}

	public GameObject makeEnemy(List<Tile> tiles) {

		GameObject enemy = this.getEnemy();

		Tile enemyTile = tiles[Random.Range(0, tiles.Count)];
		while (enemyTile.item != null || enemyTile.blocking ||
					 this.GetComponent<RoomManager>().PlayerIsNear(enemyTile.x, enemyTile.y)) {
			enemyTile = tiles[Random.Range(0, tiles.Count)];
		}

		// TODO: change 32 to be whatever the column, row amount is.

		return GameObject.Instantiate (enemy,
		                               new Vector3(enemyTile.x - 32 / 2 + .5f, enemyTile.y - 32 / 2 + .5f, 0f),
		                               Quaternion.identity) as GameObject;
	}

	public GameObject getEnemy() {

		float total = 0f;
		foreach (Spawn enemy in this.enemies) {
			total += enemy.chance;
		}

		float selected = Random.Range (0f, total);

		total = 0f;
		foreach (Spawn enemy in this.enemies) {
			total += enemy.chance;
			if (selected < total) {
				return enemy.enemy;
			}
		}

		// this line should never run if this function works correctly
		return this.enemies[Random.Range(0, this.enemies.Length)].enemy;
	}

	public virtual void RandomBlocking(List<Tile> tiles) {

		this.GetComponent<ElevationTile>().placeCliffTiles(tiles);
		this.placeTreasureTiles(tiles);
		this.placeChestTiles(tiles);
		this.placeCamps(tiles);
	}

	public abstract int getBiomeNumber();

	public bool inMap(int x, int y) {
		return x >= 0 && y >= 0 && x < this.width && y < this.height;
	}
}

