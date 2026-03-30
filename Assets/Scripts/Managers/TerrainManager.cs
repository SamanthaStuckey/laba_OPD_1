using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    public int terrainPosX;
    public int terrainPosY;
    public GameObject terrainObject;

    public int terrainToSpawn = 7;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnTerrain()
    {
        terrainPosX = Random.Range(1, 20);
        terrainPosY = Random.Range(1, 7);
        GridManager.instance.tiles[terrainPosX, terrainPosY].SetTerrain();
    }
}
