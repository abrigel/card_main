using System.Collections.Generic;
using UnityEngine;

public class ZoneHandler : MonoBehaviour
{
    public float spacing = 0.05f; // Расстояние между картами (по Z)
    public List<CardData> cardsInZone = new List<CardData>();

    public void AddCard(CardData card)
    {
        // Защита от повторного добавления
        if (!cardsInZone.Contains(card))
        {
            // Удаляем карту из предыдущей зоны, если была
            ZoneHandler previousZone = card.transform.parent?.GetComponent<ZoneHandler>();
            if (previousZone != null && previousZone != this)
            {
                previousZone.RemoveCard(card);
            }

            cardsInZone.Add(card);
        }

        // Присоединяем карту к этой зоне и сохраняем мировую позицию
        Vector3 worldPos = card.transform.position;
        card.transform.SetParent(transform, true); // true — сохранить мировую позицию
        card.transform.position = worldPos; // вернуть обратно на то же место

        UpdateLayout();
    }

    public void RemoveCard(CardData card)
    {
        if (cardsInZone.Remove(card))
        {
            UpdateLayout();
        }
    }

    public void UpdateLayout()
    {
        int count = cardsInZone.Count;
        float startZ = -(spacing * (count - 1)) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform cardTransform = cardsInZone[i].transform;
            Vector3 targetLocalPos = new Vector3(0, 0, startZ + spacing * i);
            cardTransform.localPosition = targetLocalPos;
        }
    }

    public bool ContainsCard(CardData card)
    {
        return cardsInZone.Contains(card);
    }

    public List<CardData> GetCards()
    {
        return new List<CardData>(cardsInZone);
    }
}
