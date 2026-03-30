using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RangeButtonManager : MonoBehaviour
{
    // Start is called before the first frame update

    int rangedUnitsLeftCounter = 3;
    public TMP_Text buttonRangeSpawnText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UnitSpawnManager.instance.numberOfRangeUnits != rangedUnitsLeftCounter)
        {
            rangedUnitsLeftCounter = UnitSpawnManager.instance.numberOfRangeUnits;
            buttonRangeSpawnText.text = $"Units left: {rangedUnitsLeftCounter}";
            if(rangedUnitsLeftCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
