using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public Transform grid;
    public TileLogic tile;
    float centerShift = -2f;
    public int xTilesNumber = 20;
    public int yTilesNumber = 7;
    int randomDetales;
    public LayerMask maskToAvoid;

    public TileLogic[,] tiles;

    public BaseUnit[] playerUnits;
    public BaseUnit[] enemyUnits;
    public int playerUnitIndex = 0;
    public int enemyUnitIndex = 0;


    public static GridManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void GenerateGrid()
    {
        tiles = new TileLogic[xTilesNumber, yTilesNumber];
        playerUnits = new BaseUnit[10];
        enemyUnits = new BaseUnit[10];

        for(int i = 0; i < xTilesNumber; i++)
        {
            for(int j = 0; j < yTilesNumber; j++)
            {
                TileLogic temp = Instantiate(tile, transform.position + new Vector3(i + centerShift, j + centerShift, 0), Quaternion.identity);
                temp.Init(i, j);
                temp.transform.SetParent(grid);
                temp.transform.name = $"Tile {i} {j}";
                tiles[i,j] = temp;
            }
        }
        GameManager.instance.SetGameState(GameState.SpawnEnemies);
    }

    
}