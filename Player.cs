using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> hand = new List<GameObject>(); // Карты в руке
    public Transform handPosition; // Где появляются карты
    public int maxHandSize = 5; // Максимальное количество карт

    public void AddCardToHand(GameObject card)
    {
        if (hand.Count < maxHandSize)
        {
            hand.Add(card);
            card.transform.SetParent(handPosition);
            card.transform.localPosition = GetNextCardPosition();
            card.SetActive(true);
        }
    }

    private Vector3 GetNextCardPosition()
    {
        float offset = 2.0f; // Смещение между картами
        return new Vector3(hand.Count * offset, 0, 0);
    }
}
