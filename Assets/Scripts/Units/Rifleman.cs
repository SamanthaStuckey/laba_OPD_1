using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rifleman : BaseUnit {

    public override void Init(bool isAlly)
    {
        base.Init(isAlly);
        InitRifleman();
    }

    private void InitRifleman()
    {
        type = UnitType.Range;
        hp = 15;
        currentHp = hp;
        armor = 4;
        balistikSkill = 3;
        weaponSkill = 5;
        rangeDamage = 12;
        meleeDamage = 4;
        speed = 6;
        aimingRange = 15;
    }
}
//UnitType type, int hp, int armor, int balistikSkill, int weaponSkill, int rangeDamage, int meleeDamage, int speed
