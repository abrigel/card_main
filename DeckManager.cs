using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab; // Префаб карты
    public Transform deckPosition; // Где лежит колода
    public int deckSize = 20; // Количество карт
    public Player playerHand; // Ссылка на игрока
    private List<GameObject> deck = new List<GameObject>(); // Список карт

    void Start()
    {
        GenerateDeck();
        ShuffleDeck();
    }

    void OnMouseDown() // Нажатие по колоде
    {
        DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Count > 0 && playerHand.hand.Count < playerHand.maxHandSize)
        {
            GameObject newCard = deck[deck.Count - 1]; // Берём верхнюю карту
            deck.RemoveAt(deck.Count - 1); // Убираем её из колоды
            playerHand.AddCardToHand(newCard);
            newCard.SetActive(false);

            // Присваиваем рандомный ранг
            Card cardComponent = newCard.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.AssignRandomRank();
                Debug.Log($"Выдана карта с рангом: {cardComponent.rank}");
            }
        }
        else
        {
            Debug.Log("Колода пуста или рука заполнена.");
        }
    }

    private void GenerateDeck() // Создаём колоду
    {
        for (int i = 0; i < deckSize; i++)
        {
            GameObject card = Instantiate(cardPrefab, deckPosition.position + new Vector3(0, i * 0.02f, 0), Quaternion.identity);
            card.transform.SetParent(deckPosition);
            card.SetActive(true); // Прячем карту
            deck.Add(card);
        }
    }

    private void ShuffleDeck() // Перемешивание
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
