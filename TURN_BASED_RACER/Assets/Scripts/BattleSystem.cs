using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
        
    }
    
    IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(2f);
        

    }
    void PlayerTurn()
    {

    }

    public void OnAttack()
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }
}
