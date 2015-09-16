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

}