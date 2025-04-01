using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет колодой карт, раздачей и ограничением карт в руке.
/// </summary>
public class DeckManager : MonoBehaviour
{
    /// <summary>
    /// Префаб карты, который будет создаваться при взятии карт из колоды.
    /// </summary>
    public GameObject cardPrefab;

    /// <summary>
    /// Ссылка на зону руки, где будут размещаться взятые карты.
    /// </summary>
    public Transform handZone;

    /// <summary>
    /// Максимальное количество карт, которое может быть в руке.
    /// </summary>
    public int maxHandSize = 5;

    /// <summary>
    /// Список карт, находящихся в руке.
    /// </summary>
    private List<GameObject> hand = new List<GameObject>();

    /// <summary>
    /// Общее количество карт в колоде.
    /// </summary>
    private int totalCards = 20;

    /// <summary>
    /// Расстояние между картами в руке по оси Z.
    /// </summary>
    private float cardOffset = 2.0f;

    /// <summary>
    /// Обрабатывает нажатие на колоду, вызывая процесс взятия карт.
    /// </summary>
    void OnMouseDown()
    {
        DrawCards();
    }

    /// <summary>
    /// Берёт карты из колоды и размещает их в руке, если есть свободное место.
    /// </summary>
    public void DrawCards()
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Рука заполнена!");
            return;
        }

        int cardsToDraw = Mathf.Min(5 - hand.Count, totalCards);
        for (int i = 0; i < cardsToDraw; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            newCard.transform.SetParent(handZone);
            hand.Add(newCard);
            totalCards--;

            Vector3 targetPosition = new Vector3(0, 0, i * cardOffset);
            newCard.transform.localPosition = targetPosition;
        }
    }
}
