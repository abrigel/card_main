using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>(); // Колода карт (префабы)
    public Player playerHand; // Ссылка на руку игрока

    void Start()
    {
        ShuffleDeck();
    }

    void OnMouseDown() // Клик по колоде -> берём карту
    {
        DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Count > 0 && playerHand.hand.Count < playerHand.maxHandSize)
        {
            GameObject newCard = deck[0]; // Взять верхнюю карту
            deck.RemoveAt(0); // Удалить её из колоды
            playerHand.AddCardToHand(newCard); // Добавить карту в руку игрока
        }
        else
        {
            Debug.Log("Не могу взять карту: либо колода пуста, либо в руке 5 карт.");
        }
    }

    private void ShuffleDeck() // Перемешивание колоды
    {
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}
