using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    private void Awake()
    {
        instance = this;
    }

    public BaseUnit selectedUnit;
    public TileLogic tile;
    public bool isMoving;

    public void SelectUnit(BaseUnit baseUnit)
    {
        selectedUnit = baseUnit;

        CombatCheck();

        Debug.Log(selectedUnit.inCombat);
    }

    public void MovementAction(TileLogic selectedTile)
    {
        tile = selectedTile;
        //selectedUnit.transform.position = tile.transform.position;
        selectedUnit.movementAvaliable = false;
        GridManager.instance.tiles[selectedUnit.currentTile.xPos, selectedUnit.currentTile.yPos].unit = null;
        GridManager.instance.tiles[tile.xPos, tile.yPos].unit = selectedUnit;
        isMoving = true;
        selectedUnit.animator.SetBool("isMoving", true);
        CombatCheck();
    }

    public void InRangeShoot(TileLogic selectedTile)
    {
        Vector3 startPosition = selectedUnit.currentTile.transform.position;
        Vector3 direction = selectedTile.unit.transform.position - startPosition;
        RaycastHit2D hitInfo = Physics2D.Raycast(startPosition, direction.normalized, selectedUnit.aimingRange, ~GridManager.instance.maskToAvoid);
        Debug.DrawRay(startPosition, direction.normalized * selectedUnit.aimingRange, Color.green, 20);
        if (hitInfo && hitInfo.transform.CompareTag("barricade"))
        {
            
        }
        int toHitRoll = Random.Range(1, 6);
        if(toHitRoll >= selectedUnit.balistikSkill)
        {
            int save = Random.Range(0, selectedTile.unit.armor);
            selectedTile.unit.currentHp -= (selectedUnit.rangeDamage - save);
            Debug.Log(selectedTile.unit.currentHp);
            if(selectedTile.unit.currentHp <= 0)
            {
                selectedTile.unit.Dead();
            }
            else
            {
                selectedTile.unit.animator.SetTrigger("GetDamage");
            }
        } else
        {
            Debug.Log("miss hit");
        }
        selectedUnit.animator.SetTrigger("Shoot");
        selectedUnit.ableToShoot = false;
    }

    public void OutOfRangeShoot(TileLogic selectedTile)
    {
        int toHitRoll = Random.Range(1, 6);
        if (toHitRoll == 6)
        {
            int save = Random.Range(0, selectedTile.unit.armor);
            selectedTile.unit.currentHp -= (selectedUnit.rangeDamage - save);
            Debug.Log(selectedTile.unit.currentHp);
            if (selectedTile.unit.currentHp <= 0)
            {
                selectedTile.unit.Dead();
            }
        }
        else
        {
            
        }
        selectedUnit.animator.SetTrigger("Shoot");
        selectedUnit.ableToShoot = false;
    }

    public void MeelyAttack(TileLogic selectedTile)
    {
        int toHitRoll = Random.Range(1, 6);
        if (toHitRoll >= selectedUnit.weaponSkill)
        {
            int save = Random.Range(0, selectedTile.unit.armor);
            selectedTile.unit.currentHp -= (selectedUnit.meleeDamage - save);
            Debug.Log(selectedTile.unit.currentHp);
            if (selectedTile.unit.currentHp <= 0)
            {
                selectedTile.unit.Dead();
            }
        }
        selectedUnit.animator.SetTrigger("CloseCombat");
        selectedUnit.ableMeleeAttack = false;
    }

    public void Update()
    {
        if (isMoving)
        {
            if (selectedUnit.type == UnitType.Melee)
            {
                selectedUnit.transform.position = Vector2.MoveTowards(selectedUnit.transform.position, tile.transform.position + new Vector3(0f, 0.65f, 0f), 0.01f);

                if (selectedUnit.transform.position == tile.transform.position + new Vector3(0f, 0.65f, 0f))
                {
                    selectedUnit.IdentifyTile(tile);
                    CombatCheck();
                    isMoving = false;
                    selectedUnit.animator.SetBool("isMoving", false);
                }
            }
            else
            {
                selectedUnit.transform.position = Vector2.MoveTowards(selectedUnit.transform.position, tile.transform.position, 0.01f);

                if (selectedUnit.transform.position == tile.transform.position)
                {
                    selectedUnit.IdentifyTile(tile);
                    CombatCheck();
                    isMoving = false;
                    selectedUnit.animator.SetBool("isMoving", false);
                }
            }
        }
    }

    public void OnPlayerTurn()
    {
        
    }

    public void CombatCheck()
    {
        bool checkPassed = true;
        int ii = -1;
        int jj = -1;
        int iii = 1;
        int jjj = 1;
        if(selectedUnit.currentTile.xPos == 0)
        {
            ii = 0;
        } else if(selectedUnit.currentTile.xPos == 19)
        {
            iii = 0;
        }
        if (selectedUnit.currentTile.yPos == 0)
        {
            jj = 0;
        }
        else if (selectedUnit.currentTile.yPos == 6)
        {
            jjj = 0;
        }
        for (int i = ii; i <= iii; i++)
        {
            for (int j = jj; j <= jjj; j++)
            {
                if (GridManager.instance.tiles[i + selectedUnit.currentTile.xPos, j + selectedUnit.currentTile.yPos].unit != null)
                {
                    if (GridManager.instance.tiles[i + selectedUnit.currentTile.xPos, j + selectedUnit.currentTile.yPos].unit.isAlly == false)
                    {
                        if (GridManager.instance.tiles[i + selectedUnit.currentTile.xPos, j + selectedUnit.currentTile.yPos].unit.alive == true)
                        {
                            checkPassed = false;
                            break;
                        }

                    }
                }
            }
        }
        if (checkPassed == false)
        {
            selectedUnit.inCombat = true;
            Debug.Log("in combat");
        }
        else
        {
            selectedUnit.inCombat = false;
        }
        
    }
}
