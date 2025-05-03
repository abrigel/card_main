using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private CardData cardData;

    private void Awake()
    {
        cardData = GetComponent<CardData>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsPlayerTurn())
            return;

        Transform parentZone = transform.parent;

        ShowCardStatsInConsole();

        if (parentZone == GameManager.Instance.player.attackZone)
        {
            // Нажали на свою карту в атакующей зоне
            AttackController.Instance.SelectAttacker(cardData);
        }
        else if (parentZone == GameManager.Instance.enemy.attackZone || parentZone == GameManager.Instance.enemy.defenseZone)
        {
            // Нажали на карту врага
            AttackController.Instance.SelectTarget(cardData);
        }
        else
        {
            Debug.Log("Нажали на карту вне зоны атаки/защиты.");
        }
    }

    // Метод для вывода в консоль силы атаки и здоровья карты
    private void ShowCardStatsInConsole()
    {
        Debug.Log($"Карта {cardData.name}: Атака = {cardData.attack}, Здоровье = {cardData.health}");
    }
}
