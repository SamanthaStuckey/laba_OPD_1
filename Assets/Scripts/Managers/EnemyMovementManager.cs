using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovementManager : MonoBehaviour {
  public static EnemyMovementManager instance;

  BaseUnit closestTarget;
  BaseUnit baseTarget;
  float distanceToTarget = 0;
  float distanceToClosestTarget = 0;
  public TileLogic tileToMove;
  public int meleeTargetX;
  public int meleeTargetY;

  int movementIndex = 0;

  private void Awake() {
    instance = this;
  }

  public void EnemyActions() {
    StartCoroutine(DelayLoop());
  }

  private IEnumerator DelayLoop() {
    for (int i = 0; i < 10; i++) {
      if (GridManager.instance.enemyUnits[i] != null) {
        if (GridManager.instance.enemyUnits[i].type == UnitType.Range &&
            GridManager.instance.enemyUnits[i].alive) {
          BaseUnit currentEnemy = GridManager.instance.enemyUnits[i];
          // RiflemanBrain(i);
          // yield return new WaitForSeconds(2);

          float timeToWait = currentEnemy.StartTurn();
          yield return new WaitForSeconds(timeToWait);
          currentEnemy.EndTurn();
          yield return new WaitForSeconds(1);
        } else if (GridManager.instance.enemyUnits[i].type == UnitType.Melee &&
                   GridManager.instance.enemyUnits[i].alive) {
          BaseUnit currentEnemy = GridManager.instance.enemyUnits[i];
          // RiflemanBrain(i);
          // yield return new WaitForSeconds(2);

          float timeToWait = currentEnemy.StartTurn();
          yield return new WaitForSeconds(timeToWait);
          currentEnemy.EndTurn();
          yield return new WaitForSeconds(1);
        }
      }
    }
    // set Gamestate;
    UnitSpawnManager.instance.ActivateEndTurn();
    GameManager.instance.SetGameState(GameState.PlayerTurn);
  }

  public void EnemyAiming1(BaseUnit target, BaseUnit actingUnit) {
    distanceToTarget = Mathf.Pow(actingUnit.currentTile.xPos - target.currentTile.xPos, 2) +
                       Mathf.Pow(actingUnit.currentTile.yPos - target.currentTile.yPos, 2);
    if (distanceToTarget < Mathf.Pow(actingUnit.aimingRange, 2)) {
      baseTarget = target;
    }
    if (closestTarget == null) {
      closestTarget = target;
      distanceToClosestTarget = distanceToTarget;
    } else if (distanceToTarget < distanceToClosestTarget) {
      closestTarget = target;
      distanceToClosestTarget = distanceToTarget;
    }
  }

  public void EnemyShot(BaseUnit target, BaseUnit actingUnit) {
    int toHitRoll = Random.Range(1, 6);
    if (TerrainSave(actingUnit, target)) {
      toHitRoll--;
      Debug.Log("terrain works");
    }
    if (toHitRoll >= actingUnit.balistikSkill) {
      int save = Random.Range(0, target.armor);
      target.currentHp -= (actingUnit.rangeDamage - save);
      Debug.Log(target.currentHp);
      if (target.currentHp <= 0) {
        target.Dead();
      } else
        target.animator.SetTrigger("GetDamage");
    } else {
      Debug.Log("miss hit");
    }
  }

  public void EnemyUnaimingShoot(BaseUnit target, BaseUnit actingUnit) {
    int toHitRoll = Random.Range(1, 6);
    if (TerrainSave(actingUnit, target)) {
      toHitRoll--;
      Debug.Log("terrain works");
    }
    if (toHitRoll == 6) {
      int save = Random.Range(0, target.armor);
      target.currentHp -= (actingUnit.rangeDamage - save);
      Debug.Log(target.currentHp);
      if (target.currentHp <= 0) {
        target.Dead();
      } else
        target.animator.SetTrigger("GetDamage");
    } else {
      Debug.Log("miss hit");
    }
  }

  public void MeleeAttack(BaseUnit actingUnit) {
    BaseUnit target = WatchingCloseCombatTarget(actingUnit);
    int toHitRoll = Random.Range(1, 6);
    if (toHitRoll >= actingUnit.weaponSkill) {
      int save = Random.Range(0, target.armor);
      target.currentHp -= (actingUnit.rangeDamage - save);
      Debug.Log(target.currentHp);
      if (target.currentHp <= 0) {
        target.Dead();
      } else
        target.animator.SetTrigger("GetDamage");
    } else {
      Debug.Log("miss hit");
    }
  }

  public bool CombatCheck(BaseUnit checkingUnit) {
    bool checkUnPassed = false;
    int ii = -1;
    int jj = -1;
    int iii = 1;
    int jjj = 1;
    if (checkingUnit.currentTile.xPos == 0) {
      ii = 0;
    } else if (checkingUnit.currentTile.xPos == 19) {
      iii = 0;
    }
    if (checkingUnit.currentTile.yPos == 0) {
      jj = 0;
    } else if (checkingUnit.currentTile.yPos == 6) {
      jjj = 0;
    }

    // Debug.Log(selectedUnit.currentTile.xPos);
    // Debug.Log(selectedUnit.currentTile.yPos);
    for (int i = ii; i <= iii; i++) {
      for (int j = jj; j <= jjj; j++) {
        if (GridManager.instance
                .tiles[i + checkingUnit.currentTile.xPos, j + checkingUnit.currentTile.yPos]
                .unit != null) {
          if (GridManager.instance
                  .tiles[i + checkingUnit.currentTile.xPos, j + checkingUnit.currentTile.yPos]
                  .unit.isAlly == true) {
            meleeTargetX = i + checkingUnit.currentTile.xPos;
            meleeTargetY = j + checkingUnit.currentTile.yPos;
            checkUnPassed = true;
          }
        }
      }
    }
    return checkUnPassed;
  }

  public BaseUnit WatchingCloseCombatTarget(BaseUnit checkingUnit) {
    int ii = -1;
    int jj = -1;
    int iii = 1;
    int jjj = 1;
    if (checkingUnit.currentTile.xPos == 0) {
      ii = 0;
    } else if (checkingUnit.currentTile.xPos == 20) {
      iii = 0;
    }
    if (checkingUnit.currentTile.yPos == 0) {
      jj = 0;
    } else if (checkingUnit.currentTile.yPos == 7) {
      jjj = 0;
    }

    // Debug.Log(selectedUnit.currentTile.xPos);
    // Debug.Log(selectedUnit.currentTile.yPos);
    for (int i = ii; i < iii; i++) {
      for (int j = jj; j < jjj; j++) {
        if (GridManager.instance
                .tiles[i + checkingUnit.currentTile.xPos, j + checkingUnit.currentTile.yPos]
                .unit != null) {
          if (GridManager.instance
                  .tiles[i + checkingUnit.currentTile.xPos, j + checkingUnit.currentTile.yPos]
                  .unit.isAlly == true) {
            meleeTargetX = i + checkingUnit.currentTile.xPos;
            meleeTargetY = j + checkingUnit.currentTile.yPos;
            return GridManager.instance
                .tiles[i + checkingUnit.currentTile.xPos, j + checkingUnit.currentTile.yPos]
                .unit;
          }
        }
      }
    }
    return null;
  }

  public bool TerrainSave(BaseUnit selectedUnit, BaseUnit target) {
    Vector3 startPosition = selectedUnit.currentTile.transform.position;
    Vector3 direction = target.currentTile.unit.transform.position - startPosition;
    RaycastHit2D hitInfo =
        Physics2D.Raycast(startPosition, direction.normalized, selectedUnit.aimingRange,
                          ~GridManager.instance.maskToAvoid);
    Debug.DrawRay(startPosition, direction.normalized * selectedUnit.aimingRange, Color.green, 20);
    if (hitInfo && hitInfo.transform.CompareTag("barricade")) {
      return true;
    } else
      return false;
  }

  private void Update() {}

  public void EnemyTurn() {
    for (int i = 0; i < 10; i++) {
      if (GridManager.instance.enemyUnits[i] != null) {
        GridManager.instance.enemyUnits[i].StartTurn();
      }
    }
  }

  public BaseUnit WatchingClosestTarget(BaseUnit aimingUnit) {
    BaseUnit potentialTarget;
    float closestDistance = 100000f;
    BaseUnit closestTarget = null;
    for (int j = 0; j < 10; j++) {
      if (GridManager.instance.playerUnits[j] != null &&
          GridManager.instance.playerUnits[j].alive) {
        potentialTarget = GridManager.instance.playerUnits[j];
        distanceToTarget =
            Mathf.Pow(aimingUnit.currentTile.xPos - potentialTarget.currentTile.xPos, 2) +
            Mathf.Pow(aimingUnit.currentTile.yPos - potentialTarget.currentTile.yPos, 2);
        if (closestTarget == null) {
          closestTarget = potentialTarget;
          closestDistance = distanceToTarget;
        } else if (closestDistance > distanceToTarget) {
          closestDistance = distanceToTarget;
          closestTarget = potentialTarget;
        }
      }
    }
    return closestTarget;
  }

  public float WatchingClosestDitance(BaseUnit closestTarget, BaseUnit aimingUnit) {
    return Mathf.Pow(aimingUnit.currentTile.xPos - closestTarget.currentTile.xPos, 2) +
           Mathf.Pow(aimingUnit.currentTile.yPos - closestTarget.currentTile.yPos, 2);
  }
}
