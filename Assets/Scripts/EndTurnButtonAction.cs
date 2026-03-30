using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonAction : MonoBehaviour
{
    public void ButtonPressed()
    {
        GameManager.instance.SetGameState(GameState.EnemyTurn);
    }
}
