using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighter : BaseUnit
{
    
    public override void Init(bool isAlly)
    {   
        base.Init(isAlly);
        InitMeleeFighter();
        
    }

    private void InitMeleeFighter()
    {
        type = UnitType.Melee;
        hp = 20;
        currentHp = hp;
        armor = 8;
        balistikSkill = 4;
        weaponSkill = 3;
        rangeDamage = 4;
        meleeDamage = 15;
        speed = 9;
        aimingRange = 6;
        Debug.Log("rifleman was created");
    }
}
