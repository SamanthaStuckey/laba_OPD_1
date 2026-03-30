using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnManager : MonoBehaviour
{
    
    public static UnitSpawnManager instance;

    public GameObject currentObjectToSpawn;

    public UnitType currentType = UnitType.None;

    public BaseUnit allyRangeUnit;
    public BaseUnit allyMeleeUnit;

    public int numberOfRangeUnits = 3;
    public int numberOfMeleeUnits = 2;

    public GameObject rangeUnitButton;
    public GameObject meleeUnitButton;
    public GameObject startGameButton;
    public GameObject endTurnButton;

    private void Awake()
    {
        instance = this;
    }

    public void Test()
    {
        Debug.Log("Hi");
    }

    public void SelectObjectToSpawn(int type)
    {
        
        currentType = (UnitType)type;
        
    }
    
    public void ActivateButtons()
    {
        rangeUnitButton.SetActive(true);
        meleeUnitButton.SetActive(true);
        startGameButton.SetActive(true);
    }

    public void DisableButtons()
    {
        rangeUnitButton.SetActive(false);
        meleeUnitButton.SetActive(false);
        startGameButton.SetActive(false);
        ActivateEndTurn();
    }

    public void ActivateEndTurn()
    {
        endTurnButton.SetActive(true);
    }

    



}
