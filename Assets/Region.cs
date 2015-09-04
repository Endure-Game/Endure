using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Region
{
	public int focusX;
	public int focusY;
	public BiomeTile biome;
	public int altitude;

	public List<Tile> tiles;
	private List<GameObject> enemies;
	private int enemyLimit = 10;

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

		if (this.enemies.Count == this.enemyLimit) {
			return;
		}

		GameObject enemy = this.biome.getEnemy();

		Tile enemyTile = this.tiles[Random.Range(0, this.tiles.Count)];
		while (enemyTile.item != null) {
			enemyTile = this.tiles[Random.Range(0, this.tiles.Count)];
		}
		// TODO: change 16 to be whatever the column, row amount is.
		this.enemies.Add(GameObject.Instantiate (enemy, 
		                              new Vector3(enemyTile.x - 16 / 2 + .5f, enemyTile.y - 16 / 2 + .5f, 0f),
		                              Quaternion.identity) as GameObject);
	}
}