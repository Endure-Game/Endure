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
	private List<Enemy1AI> enemies;

	public Region(int focusX, int focusY, BiomeTile biome, int altitude) {
		this.focusX = focusX;
		this.focusY = focusY;
		this.biome = biome;
		this.altitude = altitude;

		this.tiles = new List<Tile>();
		this.enemies = new List<Enemy1AI>();
	}
}