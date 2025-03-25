using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab; // Префаб карты
    public Transform handZone; // Где появляются карты (ссылка на Hand)
    public int maxHandSize = 5; // Максимальное количество карт в руке
    private List<GameObject> hand = new List<GameObject>(); // Карты в руке

    private int totalCards = 20; // Количество карт в колоде

    void OnMouseDown() // Клик по колоде
    {
        DrawCards();
    }

    public void DrawCards()
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Рука заполнена!");
            return;
        }

        int cardsToDraw = Mathf.Min(5 - hand.Count, totalCards); // Сколько можно взять
        for (int i = 0; i < cardsToDraw; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            newCard.transform.SetParent(handZone); // Помещаем в руку
            hand.Add(newCard);
            totalCards--; // Уменьшаем колоду

            // Размещаем карту с отступом
            float offset = 2.0f;
            newCard.transform.position = new Vector3(handZone.position.x + (hand.Count - 1) * offset, handZone.position.y, handZone.position.z);
        }
    }
}
