using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> hand = new List<GameObject>(); // Карты в руке
    public Transform handPosition; // Где появляются карты
    public int maxHandSize = 5; // Максимальное количество карт

    // Координаты для сетки
    private Vector3[] rowOne = { new Vector3(-2f, 0, 0), new Vector3(0f, 0, 0), new Vector3(2f, 0, 0) }; // Первый ряд (3 карты)
    private Vector3[] rowTwo = { new Vector3(-2f, -1f, 0), new Vector3(0f, -1f, 0), new Vector3(2f, -1f, 0) }; // Второй ряд (3 карты)

    private Quaternion[] rotationGrid = new Quaternion[] // Углы поворота карт
    {
        Quaternion.Euler(0, 0, 15), Quaternion.Euler(0, 0, -15),
        Quaternion.Euler(0, 0, 20), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -20)
    };

    public void AddCardToHand(GameObject card)
    {
        if (hand.Count < maxHandSize)
        {
            hand.Add(card);
            card.transform.SetParent(handPosition);
            card.SetActive(true); // Показываем карту
            UpdateHandLayout(); // Обновляем расположение карт
        }
    }

    private void UpdateHandLayout()
    {
        // Обновляем позицию карт в зависимости от их количества
        for (int i = 0; i < hand.Count; i++)
        {
            Vector3 targetPosition = Vector3.zero;
            Quaternion targetRotation = rotationGrid[i];

            // Выбираем ряд в зависимости от количества карт
            if (hand.Count <= 3) // 1-3 карты
            {
                targetPosition = rowOne[i]; // Позиции из первого ряда
            }
            else if (hand.Count > 3) // 4-5 карт
            {
                if (i < 3) // Для карт в первом ряду
                {
                    targetPosition = rowOne[i];
                }
                else // Для карт во втором ряду
                {
                    targetPosition = rowTwo[i - 3]; // Сдвиг по индексу для второго ряда
                }
            }

            // Устанавливаем позицию и угол поворота
            hand[i].transform.localPosition = targetPosition;
            hand[i].transform.localRotation = targetRotation;
        }
    }
}
