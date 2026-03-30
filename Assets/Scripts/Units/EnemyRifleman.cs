using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRifleman : BaseUnit
{
    bool combat;
    public override void Init(bool isAlly)
    {
        base.Init(isAlly);
        InitEnemyRifleman();
    }

    private void InitEnemyRifleman()
    {
        type = UnitType.Range;
        hp = 15;
        currentHp = hp;
        armor = 4;
        balistikSkill = 3;
        weaponSkill = 5;
        rangeDamage = 8;
        meleeDamage = 4;
        speed = 6;
        aimingRange = 15;
        Debug.Log("enemy rifleman was created");
    }

    public override float StartTurn()
    {
        base.StartTurn();
        float time = 0f;
        bool combat = EnemyMovementManager.instance.CombatCheck(this);
        if (!combat)
        {
            BaseUnit closestTarget = EnemyMovementManager.instance.WatchingClosestTarget(this);
            float distanceToClosestTarget = EnemyMovementManager.instance.WatchingClosestDitance(closestTarget, this);
            TileLogic tileToMove = null;
            int x1 = currentTile.xPos;
            int y1 = currentTile.yPos;
            int x = x1;
            int y = y1;
            if (distanceToClosestTarget <= 36f)
            {
                
                for (int t = 1; t <= speed; t++)
                {
                    
                    if (x1 + t < GridManager.instance.xTilesNumber)
                    {
                        if (GridManager.instance.tiles[x1 + 1, y1].unit == null && GridManager.instance.tiles[x1 + t, y1].terrain == null)
                        {
                            x1 = x1 + 1;
                        }
                    }
                    else
                    {
                        if (y1 - t >= 0)
                        {
                            if (currentTile.yPos > closestTarget.currentTile.yPos)
                            {
                                if (GridManager.instance.tiles[x1, y1 - 1].unit == null && GridManager.instance.tiles[x1, y1 - 1].terrain == null)
                                {
                                    y1 = y1 - 1;
                                }
                            }
                        }
                        else
                        {
                            if (y1 + t < GridManager.instance.yTilesNumber)
                            {
                                if (GridManager.instance.tiles[x1, y1 + 1].unit == null && GridManager.instance.tiles[x1, y1 + 1].terrain == null)
                                {
                                    y1 = y1 + 1;
                                }
                            }
                        }
                    }
                }
                
            }
            else
            {
                if(distanceToClosestTarget > aimingRange*aimingRange)
                {
                    for (int t = 1; t <= speed; t++)
                    {
                        int xDifference = Mathf.Abs(currentTile.xPos - t - closestTarget.currentTile.xPos);
                        int yDifference = Mathf.Abs(currentTile.yPos - t - closestTarget.currentTile.yPos);
                        if (xDifference > yDifference)
                        {
                            if (currentTile.xPos - t > 0)
                            {
                                if (GridManager.instance.tiles[x1 - 1, y].unit == null && GridManager.instance.tiles[x - t, y].terrain == null)
                                {
                                    x1 = x1 - 1;
                                }
                                else
                                {
                                    if (currentTile.yPos > closestTarget.currentTile.yPos && y1 - 1 > 0)
                                    {
                                        if (GridManager.instance.tiles[x1, y1 - 1].unit == null && GridManager.instance.tiles[x1, y1 - 1].terrain == null)
                                        {
                                            y1 = y1 - 1;
                                        }

                                    }
                                    else
                                    {
                                        if (y1 + 1 < 7) {
                                            if (GridManager.instance.tiles[x1, y1 + 1].unit == null && GridManager.instance.tiles[x1, y1 + 1].terrain == null)
                                            {
                                                y1 = y1 + 1;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            tileToMove = GridManager.instance.tiles[x1, y1];
            this.tileToMove = tileToMove;
            if(tileToMove != null)
            {
                time = distanceToClosestTarget/120;
                movementActive = true;

                isMoving = true;
                animator.SetBool("isMoving", isMoving);
            }
        }
        return time;
    }
    
    public override void EndTurn()
    {
        base.EndTurn();
        animator.SetBool("isMoving", isMoving);
        bool combat = EnemyMovementManager.instance.CombatCheck(this);
        if (!combat)
        {
            BaseUnit closestTarget = EnemyMovementManager.instance.WatchingClosestTarget(this);
            float distanceToClosestTarget = EnemyMovementManager.instance.WatchingClosestDitance(closestTarget, this);
            if (distanceToClosestTarget <= Mathf.Pow(aimingRange, 2))
            {
                EnemyMovementManager.instance.EnemyShot(closestTarget, this);
                animator.SetTrigger("Attack");
            }
            else
            {
                EnemyMovementManager.instance.EnemyUnaimingShoot(closestTarget, this);
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            EnemyMovementManager.instance.MeleeAttack(this);
            animator.SetTrigger("MeleeAttack");
        }
    }
}