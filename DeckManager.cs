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

    // Отступ между картами по оси Z
    private float cardOffset = 2.0f;

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

            // Располагаем карты вдоль оси Z с заданным отступом
            Vector3 targetPosition = new Vector3(0, 0, i * cardOffset); // Отступаем по оси Z
            newCard.transform.localPosition = targetPosition;
        }
    }
}
