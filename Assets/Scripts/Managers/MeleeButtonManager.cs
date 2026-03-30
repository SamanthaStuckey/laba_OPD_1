using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeButtonManager : MonoBehaviour
{
    int meleeUnitsLeftCounter = 3;
    public TMP_Text buttonMeleeSpawnText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnitSpawnManager.instance.numberOfMeleeUnits != meleeUnitsLeftCounter)
        {
            meleeUnitsLeftCounter = UnitSpawnManager.instance.numberOfMeleeUnits;
            buttonMeleeSpawnText.text = $"Units left: {meleeUnitsLeftCounter}";
            if (meleeUnitsLeftCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
