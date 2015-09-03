using UnityEngine;
using System.Collections.Generic;

public class Tile
{
	public int x;
	public int y;

	public int regionIndex;
	public int biome;
	public int elevation;

	public bool blocking;
	public GameObject item;
	public bool path = false;

	public Tile (int x, int y, int regionIndex, int biome, bool blocking, int elevation) {
		this.x = x;
		this.y = y;
		this.regionIndex = regionIndex;
		this.biome = biome;
		this.blocking = blocking;
		this.elevation = elevation;
	}

	public Tile (Tile tile) {
		this.x = tile.x;
		this.y = tile.y;
		this.regionIndex = tile.regionIndex;
		this.biome = tile.biome;
		this.blocking = tile.blocking;
		this.elevation = tile.elevation;
	}
}

