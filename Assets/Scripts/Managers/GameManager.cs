using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentState;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetGameState(GameState.GenerateGrid);
    }

    public void SetGameState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.GenerateGrid:
                GridManager.instance.GenerateGrid();
                break;
            case GameState.SpawnEnemies:
                while (SpawnEnemyManager.instance.enemyRiflemansSpawnsLeft > 0)
                {
                    SpawnEnemyManager.instance.SpawnEnemyRifleman();
                }
                while (SpawnEnemyManager.instance.enemyMeleeSpawnsLeft > 0)
                {
                    SpawnEnemyManager.instance.SpawnEnemyMelee();
                }
                while (TerrainManager.instance.terrainToSpawn > 0)
                {
                    TerrainManager.instance.SpawnTerrain();
                }
                SetGameState(GameState.SpawnAllys);
                break;
            case GameState.SpawnAllys:
                UnitSpawnManager.instance.ActivateButtons();
                break;
            case GameState.PlayerTurn:
                for(int i = 0; i < 10; i++)
                {
                    if(GridManager.instance.playerUnits[i] != null)
                    {
                        if (GridManager.instance.playerUnits[i].alive)
                        {
                            GridManager.instance.playerUnits[i].NewTurnAbilities();
                        }
                    }
                }
                break;
            case GameState.EnemyTurn:
                EnemyMovementManager.instance.EnemyActions();
                break;
        }
    }
}
