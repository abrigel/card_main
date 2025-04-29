using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController player;
    public EnemyController enemy;

    public Button endTurnButton; // Ссылка на кнопку "Закончить ход"

    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
        }
        else
        {
            Debug.LogWarning("End Turn Button не назначена в GameManager!");
        }

        StartPlayerTurn();
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        player.StartTurn();
        ResetPlayerCardAttacks();
        SetEndTurnButtonInteractable(true);
        Debug.Log("Ход игрока начался");
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        SetEndTurnButtonInteractable(false);
        Debug.Log("Ход игрока завершён");
        Invoke(nameof(StartEnemyTurn), 1f);
    }

    private void StartEnemyTurn()
    {
        enemy.StartTurn();
        Debug.Log("Ход противника начался");
    }

    public void EndEnemyTurn()
    {
        isPlayerTurn = true;
        StartPlayerTurn();
    }

    private void ResetPlayerCardAttacks()
    {
        ZoneHandler attackZoneHandler = player.attackZone.GetComponent<ZoneHandler>();
        if (attackZoneHandler != null)
        {
            foreach (var card in attackZoneHandler.cardsInZone)
            {
                card.hasAttackedThisTurn = false;
            }
        }

        ZoneHandler defenseZoneHandler = player.defenseZone.GetComponent<ZoneHandler>();
        if (defenseZoneHandler != null)
        {
            foreach (var card in defenseZoneHandler.cardsInZone)
            {
                card.hasAttackedThisTurn = false;
            }
        }
    }

    private void OnEndTurnButtonClicked()
    {
        if (isPlayerTurn)
        {
            Debug.Log("Кнопка 'Закончить ход' нажата.");
            EndPlayerTurn();
        }
    }

    private void SetEndTurnButtonInteractable(bool interactable)
    {
        if (endTurnButton != null)
            endTurnButton.interactable = interactable;
    }
}
