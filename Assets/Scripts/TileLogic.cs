using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    Vector2 position = Vector2.zero;
    public Color baseColor;
    public Color offSetColor;

    public Color highlighted;
    public Color tileColor;
    public Color selectedColor;
    public Color ableToMoveColor;

    public Color outOfRangeAimingColor;
    public Color normalAimingColor;

    public SpriteRenderer tileSprite;
    public BaseUnit unit = null;

    public ButtonSpawnUnit spawnsRemain;
    public bool isButtonActive = false;
    public bool isSelectedToMove = false;

    public bool selectedToShoot = false;
    public bool selectedToOutOfRangeShoot = false;

    public GameObject terrain = null;

    public int xPos;
    public int yPos;

    public void Init(int x, int y)
    {
        if((x%2==0 && y % 2 == 0) || (x%2==1 && y % 2 == 1))
        {
            tileSprite.color = baseColor;
        }
        else
        {
            tileSprite.color = offSetColor;
        }

        tileColor = tileSprite.color;

        position = new Vector2(x, y);
        
        xPos = x;
        yPos = y;
    }

    public void OnMouseEnter()
    {
        tileSprite.color = highlighted;
        if (GameManager.instance.currentState == GameState.PlayerTurn)
        {
            if (PlayerMovementManager.instance.selectedUnit != null)
            {
                if (Mathf.Pow(PlayerMovementManager.instance.selectedUnit.currentTile.xPos - xPos, 2) + Mathf.Pow(PlayerMovementManager.instance.selectedUnit.currentTile.yPos - yPos, 2) < Mathf.Pow(PlayerMovementManager.instance.selectedUnit.speed, 2) && PlayerMovementManager.instance.selectedUnit.movementAvaliable == true && unit == null && terrain == null)
                {
                    tileSprite.color = ableToMoveColor;
                    isSelectedToMove = true;
                } else if (Mathf.Pow(PlayerMovementManager.instance.selectedUnit.currentTile.xPos - xPos, 2) + Mathf.Pow(PlayerMovementManager.instance.selectedUnit.currentTile.yPos - yPos, 2) < Mathf.Pow(PlayerMovementManager.instance.selectedUnit.aimingRange, 2) && unit != null && PlayerMovementManager.instance.selectedUnit.ableToShoot && !PlayerMovementManager.instance.selectedUnit.inCombat)
                {
                    if(unit.isAlly == false)
                    {
                        tileSprite.color = normalAimingColor;
                        selectedToShoot = true;
                    }
                } else if(unit != null && PlayerMovementManager.instance.selectedUnit.ableMeleeAttack && !PlayerMovementManager.instance.selectedUnit.inCombat)
                {
                    tileSprite.color = outOfRangeAimingColor;
                    selectedToOutOfRangeShoot = true;
                } else if(unit != null && PlayerMovementManager.instance.selectedUnit.inCombat)
                {

                }

            }
        }
        if(unit != null)
        {
            string hint = unit.currentHp.ToString();
            ToolTipManager.instance.ShowHint(hint);
        }
    }

    public void OnMouseExit()
    {
        tileSprite.color = tileColor;
        isSelectedToMove = false;
        selectedToShoot = false;
        selectedToOutOfRangeShoot=false;
        ToolTipManager.instance.HideHint();
    }

    public void OnMouseDown()
    {    
        
        if (UnitSpawnManager.instance.currentType != UnitType.None && unit == null && GameManager.instance.currentState == GameState.SpawnAllys)
        {
            SetUnit();
        }
        
        if(GameManager.instance.currentState == GameState.PlayerTurn)
        {
            if (unit != null && unit.isAlly == true && PlayerMovementManager.instance.isMoving == false)
            {
                PlayerMovementManager.instance.SelectUnit(unit);
                tileSprite.color = selectedColor;
                Debug.Log("unit seleted");
            }
            else
            {
                if (isSelectedToMove == true && PlayerMovementManager.instance.selectedUnit.movementAvaliable == true && PlayerMovementManager.instance.selectedUnit.inCombat == false)
                {

                    PlayerMovementManager.instance.MovementAction(this);

                }
                else if (selectedToShoot && PlayerMovementManager.instance.selectedUnit.ableToShoot && PlayerMovementManager.instance.selectedUnit.inCombat == false)
                {
                    PlayerMovementManager.instance.InRangeShoot(this);
                    //PlayerMovementManager.instance.selectedUnit = null;
                }
                else if (selectedToOutOfRangeShoot && PlayerMovementManager.instance.selectedUnit.ableToShoot && PlayerMovementManager.instance.selectedUnit.inCombat == false)
                {
                    PlayerMovementManager.instance.OutOfRangeShoot(this);
                }
                else if (PlayerMovementManager.instance.selectedUnit.inCombat == true && PlayerMovementManager.instance.selectedUnit.ableMeleeAttack && unit != null)
                {
                    PlayerMovementManager.instance.MeelyAttack(this);
                }
            }
        }
    }

    private void SetUnit()
    {
        if (UnitSpawnManager.instance.currentType == UnitType.Range && UnitSpawnManager.instance.numberOfRangeUnits > 0)
        {
            unit = Instantiate(UnitSpawnManager.instance.allyRangeUnit, transform.position, Quaternion.identity);
            UnitSpawnManager.instance.numberOfRangeUnits--;
        }
        else if (UnitSpawnManager.instance.currentType == UnitType.Melee && UnitSpawnManager.instance.numberOfMeleeUnits > 0)
        {
            unit = Instantiate(UnitSpawnManager.instance.allyMeleeUnit, transform.position, Quaternion.identity);
            UnitSpawnManager.instance.numberOfMeleeUnits--;
        }
        if (unit != null)
        {
            unit.Init(true);
            unit.IdentifyTile(this);
            GridManager.instance.playerUnits[GridManager.instance.playerUnitIndex] = unit;
            GridManager.instance.playerUnitIndex++;
            if(unit.type == UnitType.Melee)
            {
                unit.transform.position += new Vector3(0f, 0.65f, 0f);
            }
        }
    }

    public void SetEnemyRiflemanUnit()
    {
        if(unit == null)
        {
            unit = Instantiate(SpawnEnemyManager.instance.enemyRiflemanUnit, transform.position + new Vector3(-0.4f, 0f, 0f), Quaternion.identity);
            SpawnEnemyManager.instance.enemyRiflemansSpawnsLeft--;
            unit.Init(false);
            unit.IdentifyTile(this);
            GridManager.instance.enemyUnits[GridManager.instance.enemyUnitIndex] = unit;
            GridManager.instance.enemyUnitIndex++;
        }
        
    }

    public void SetEnemymeMeleeUnit()
    {
        if (unit == null)
        {
            unit = Instantiate(SpawnEnemyManager.instance.enemyMeleeUnit, transform.position, Quaternion.identity);
            SpawnEnemyManager.instance.enemyMeleeSpawnsLeft--;
            unit.Init(false);
            unit.IdentifyTile(this);
            GridManager.instance.enemyUnits[GridManager.instance.enemyUnitIndex] = unit;
            GridManager.instance.enemyUnitIndex++;
            unit.transform.position += new Vector3(-0.5f, 0.65f, 0f);
        }

    }

    public void SetTerrain()
    {
        if(unit == null && terrain == null)
        {
            terrain = Instantiate(TerrainManager.instance.terrainObject, transform.position, Quaternion.identity);
            TerrainManager.instance.terrainToSpawn--;
        }
    }
    
    public bool IsEmptyTile()
    {
        return unit == null && terrain == null;
    }

    private void Update()
    {
        
    }
}
