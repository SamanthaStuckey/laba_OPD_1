using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour
{
    public static SpawnEnemyManager instance;
    public int enemyRiflemansSpawnsLeft = 3;
    public int enemyMeleeSpawnsLeft = 2;
    public int enemyPosX;
    public int enemyPosY;
    public BaseUnit enemyRiflemanUnit;
    public BaseUnit enemyMeleeUnit;
    
    private void Awake()
    {
        instance = this;
    }
    

    public void SpawnEnemyRifleman()
    {
        enemyPosX = Random.Range(15, 20);
        enemyPosY = Random.Range(1, 7);
        GridManager.instance.tiles[enemyPosX, enemyPosY].SetEnemyRiflemanUnit();
    }

    public void SpawnEnemyMelee()
    {
        enemyPosX = Random.Range(15, 20);
        enemyPosY = Random.Range(1, 7);
        GridManager.instance.tiles[enemyPosX, enemyPosY].SetEnemymeMeleeUnit();
    }
}
