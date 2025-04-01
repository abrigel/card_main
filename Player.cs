using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет игроком, его картами в руке и их расположением.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Карты в руке игрока.
    /// </summary>
    public List<GameObject> hand = new List<GameObject>();

    /// <summary>
    /// Позиция, где появляются карты в руке.
    /// </summary>
    public Transform handPosition;

    /// <summary>
    /// Максимальное количество карт, которое может быть в руке.
    /// </summary>
    public int maxHandSize = 5;

    /// <summary>
    /// Координаты для первого ряда карт.
    /// </summary>
    private Vector3[] rowOne = { new Vector3(-2f, 0, 0), new Vector3(0f, 0, 0), new Vector3(2f, 0, 0) };

    /// <summary>
    /// Координаты для второго ряда карт.
    /// </summary>
    private Vector3[] rowTwo = { new Vector3(-2f, -1f, 0), new Vector3(0f, -1f, 0), new Vector3(2f, -1f, 0) };

    /// <summary>
    /// Углы поворота карт в руке.
    /// </summary>
    private Quaternion[] rotationGrid = new Quaternion[]
    {
        Quaternion.Euler(0, 0, 15), Quaternion.Euler(0, 0, -15),
        Quaternion.Euler(0, 0, 20), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -20)
    };

    /// <summary>
    /// Добавляет карту в руку игрока и обновляет расположение карт.
    /// </summary>
    public void AddCardToHand(GameObject card)
    {
        if (hand.Count < maxHandSize)
        {
            hand.Add(card);
            card.transform.SetParent(handPosition);
            card.SetActive(true);
            UpdateHandLayout();
        }
    }

    /// <summary>
    /// Обновляет расположение карт в руке игрока.
    /// </summary>
    private void UpdateHandLayout()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            Vector3 targetPosition = Vector3.zero;
            Quaternion targetRotation = rotationGrid[i];

            if (hand.Count <= 3)
            {
                targetPosition = rowOne[i];
            }
            else
            {
                if (i < 3)
                {
                    targetPosition = rowOne[i];
                }
                else
                {
                    targetPosition = rowTwo[i - 3];
                }
            }

            hand[i].transform.localPosition = targetPosition;
            hand[i].transform.localRotation = targetRotation;
        }
    }
}
