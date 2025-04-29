using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform handZone;
    public Transform attackZone;
    public Transform defenseZone;

    public int health = 30;
    public int mana = 5;

    public Text manaText;
    public Text healthText;

    private List<CardData> handCards = new List<CardData>();

    void Start()
    {
        DrawCards(5); // Получаем начальные 5 карт
        UpdatePlayerStats(); // Обновляем отображение
    }

    public void StartTurn()
    {
        mana = Mathf.Min(mana + 5, 10); // Получаем ману
        Debug.Log("Игрок начал ход");

        // ✅ Сброс флага hasAttackedThisTurn у карт в атакующей зоне
        foreach (Transform cardTransform in attackZone)
        {
            CardData card = cardTransform.GetComponent<CardData>();
            if (card != null)
                card.hasAttackedThisTurn = false;
        }

        UpdatePlayerStats();
    }

    public void DrawCards(int count)
    {
        var zoneHandler = handZone.GetComponent<ZoneHandler>();
        if (zoneHandler == null)
        {
            Debug.LogWarning("handZone не содержит ZoneHandler!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load("CardPrefab") as GameObject);
            CardData cardData = cardObject.GetComponent<CardData>();
            cardData.GenerateRandomStats();
            handCards.Add(cardData);
            zoneHandler.AddCard(cardData); // Правильное добавление с расстановкой
        }
    }

    public void PlayCards()
    {
        List<CardData> cardsToPlay = new List<CardData>();

        foreach (CardData card in handCards)
        {
            if (card.manaCost <= mana)
            {
                Transform zone = Random.value > 0.5f ? attackZone : defenseZone;

                mana -= card.manaCost;

                var zoneHandler = zone.GetComponent<ZoneHandler>();
                if (zoneHandler != null)
                {
                    zoneHandler.AddCard(card);
                    cardsToPlay.Add(card);
                }
            }
        }

        foreach (var playedCard in cardsToPlay)
        {
            handCards.Remove(playedCard);
            var handZoneHandler = handZone.GetComponent<ZoneHandler>();
            if (handZoneHandler != null)
                handZoneHandler.RemoveCard(playedCard);
        }

        UpdatePlayerStats();
    }

    public void EndTurn()
    {
        GameManager.Instance.EndPlayerTurn();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Игрок проиграл!");
        }
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats()
    {
        if (manaText != null) manaText.text = "Mana: " + mana;
        if (healthText != null) healthText.text = "HP: " + health;
    }
}
