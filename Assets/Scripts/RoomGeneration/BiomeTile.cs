using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BiomeTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	public Spawn[] enemies;
	public RoomManager.Count treasureCount = new RoomManager.Count(1, 2);

	protected Tile[,] tileMap;
	protected int width;
	protected int height;

	[System.Serializable]
	public class Spawn {
		public float chance;
		public GameObject enemy;
	}

	void Start() {
		this.tileMap = this.GetComponent<RoomManager>().tileMap;
		this.height = this.tileMap.GetLength(0);
		this.width = this.tileMap.GetLength(1);
	}

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
			tile.blocking = true;
			spritePlacer(x, y);
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

	public void CreateHill(Tile centerTile) {
		int x = centerTile.x;
		int y = centerTile.y;
		for (int i = -2; i <= 2; i++) {
			for (int j = -2; j <= 2; j++) {
				if (Mathf.Abs(i) != 2 || Mathf.Abs(j) != 2) {
					tileMap[x + i, y + j].elevation++;
				}
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

	public virtual void RandomBlocking(List<Tile> region) {
		this.GetComponent<ElevationTile>().placeCliffTiles(region);
		this.placeTreasureTiles(region);
	}

	public abstract int getBiomeNumber();
}

