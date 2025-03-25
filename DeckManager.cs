using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>(); // Колода карт
    public List<GameObject> hand = new List<GameObject>(); // Карты в руке
    public Transform handPosition;  // Где располагаются карты в руке
    public int maxHandSize = 5; // Максимальное количество карт в руке

    void Start()
    {
        ShuffleDeck();
    }

    void OnMouseDown() // Клик по колоде
    {
        DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            GameObject newCard = deck[0]; // Берем верхнюю карту
            deck.RemoveAt(0); // Удаляем её из колоды
            hand.Add(newCard); // Добавляем в руку
            newCard.transform.position = GetNextHandPosition(); // Перемещаем в руку
            newCard.SetActive(true); // Показываем карту
        }
        else
        {
            Debug.Log("Невозможно взять карту: либо колода пуста, либо рука заполнена.");
        }
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    private Vector3 GetNextHandPosition()
    {
        float offset = 2.0f; // Смещение между картами
        return new Vector3(handPosition.position.x + hand.Count * offset, handPosition.position.y, handPosition.position.z);
    }
}
