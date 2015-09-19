using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Region
{
	public int focusX;
	public int focusY;
	public BiomeTile biome;
	public int altitude;

	public List<Tile> tiles;
	private List<GameObject> enemies;
	private int enemyLimit = 40;

	public Region(int focusX, int focusY, BiomeTile biome, int altitude) {

		this.focusX = focusX;
		this.focusY = focusY;
		this.biome = biome;
		this.altitude = altitude;

		this.tiles = new List<Tile>();
		this.enemies = new List<GameObject>();
	}

	public void makeBlocking() {
		this.biome.RandomBlocking(this.tiles);
	}

	public void spawnEnemy() {

		if (this.enemies.Count >= this.enemyLimit) {
			return;
		}

		this.enemies.Add(this.biome.makeEnemy(this.tiles));
	}

	// Shuffles all tiles inside of the tiles list
	public void ShuffleRegion() {
		int n = this.tiles.Count;
    while (n > 1) {
      n--;
      int k = Random.Range(0, n + 1);
      Tile value = this.tiles[k];
      this.tiles[k] = this.tiles[n];
      this.tiles[n] = value;
    }
	}

}