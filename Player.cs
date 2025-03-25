using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> hand = new List<GameObject>(); // Карты в руке
    public Transform handPosition; // Где карты появляются
    public int maxHandSize = 5; // Максимум карт в руке

    public int mana = 10;
    private DeckManager deckManager;

    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        StartGameDraw();
    }

    void StartGameDraw()
    {
        for (int i = 0; i < 5; i++)
        {
            deckManager.DrawCard();
        }
    }

    public void AddCardToHand(GameObject card)
    {
        if (hand.Count < maxHandSize)
        {
            hand.Add(card);
            card.transform.SetParent(handPosition); // Карта в руке
            card.transform.localPosition = GetNextCardPosition(); // Позиция карты
            card.SetActive(true); // Показываем карту
        }
    }

    private Vector3 GetNextCardPosition()
    {
        float offset = 2.0f; // Смещение между картами
        return new Vector3(hand.Count * offset, 0, 0);
    }
}
