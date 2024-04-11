using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Text dialogueText;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());

    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Racer " + enemyUnit.unitName + "has pulled up on you!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator PlayerHeal()
    {
        int rand = Random.Range(1, 3);
        playerUnit.Heal(10);
        yield return new WaitForSeconds(1f);
        switch (rand)
        {
            case <= 1:
                dialogueText.text = "You made a quick pit stop!";
                break;
            case >= 2:
                dialogueText.text = "Your gas tank has been refilled. Pettle to the metal!";
                break;
        }
    }
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        int rand = Random.Range(1, 3);
        switch (rand)
        {
            case <= 1:
                dialogueText.text = "The drift was successful!";
                break;
            case >= 2:
                dialogueText.text = "The you overtook the enemy racer!";
                break;
        }
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " slides into you!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "YOU HAVE WON THE DEADLY RACE";

        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Better luck next time kid!";
        }
    }
    void PlayerTurn()
    {
        dialogueText.text = "Choose an action racer!: ";
    }

    public void OnAttack()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }
    public void OnHeal()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerHeal());
    }
}
