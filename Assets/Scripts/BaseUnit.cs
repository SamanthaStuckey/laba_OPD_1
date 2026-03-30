using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public UnitType type;
    public int hp;
    public int currentHp;
    public int armor;
    public int balistikSkill;
    public int weaponSkill;
    public int rangeDamage;
    public int meleeDamage;
    public int speed;
    public bool isAlly;
    public TileLogic currentTile;
    public bool movementAvaliable;
    public int aimingRange;
    public bool ableToShoot;
    public bool alive;
    public bool ableMeleeAttack;
    public bool inCombat;
    public SpriteRenderer sprite;

    public Animator animator;

    public bool movementActive = false;

    public TileLogic tileToMove;


    public bool isMoving = false;

    public virtual void Init(bool isAlly)
    {
        this.isAlly = isAlly;
        movementAvaliable = true;
        ableToShoot = true;
        alive = true;
        ableMeleeAttack = true;
        inCombat = false;
        animator = transform.GetComponent<Animator>();
    }

    public void IdentifyTile(TileLogic currentTile)
    {
        this.currentTile = currentTile;
        
    }

    public void Update()
    {
        //sprite.sortingOrder = 25 - currentTile.yPos;
        if (movementActive && !isAlly)
        {
            if (type == UnitType.Range)
            {
                transform.position = Vector2.MoveTowards(transform.position, tileToMove.transform.position + new Vector3(-0.4f, 0f, 0f), 0.01f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, tileToMove.transform.position + new Vector3(-0.5f, 0.65f, 0f), 0.01f);
            }
            if(Vector3.Distance(transform.position, tileToMove.transform.position + new Vector3(-0.4f, 0f, 0f)) <= 0.001f && type == UnitType.Range)
            {
                movementActive = false;
                currentTile.unit = null;
                IdentifyTile(tileToMove);
                tileToMove.unit = this;
            }
            else if (Vector3.Distance(transform.position, tileToMove.transform.position + new Vector3(-0.5f, 0.65f, 0f)) <= 0.001f && type == UnitType.Melee)
            {
                movementActive = false;
                currentTile.unit = null;
                IdentifyTile(tileToMove);
                tileToMove.unit = this;
            }
        }
    }

    public void Dead()
    {
        alive = false;
        animator.SetTrigger("Die");
    }

    public void NewTurnAbilities()
    {
        movementAvaliable = true;
        ableToShoot = true;
        ableMeleeAttack = true;
    }

    public void Movement(TileLogic tileToMove)
    {
        movementActive = true;
        this.tileToMove = tileToMove;
        
        
    }

    public virtual float StartTurn()
    {
        return 0f;
    }

    public virtual void EndTurn()
    {
        isMoving = false;
    }
}