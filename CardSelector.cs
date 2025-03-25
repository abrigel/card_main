using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public static GameObject selectedCard = null;
    private static Dictionary<Transform, List<GameObject>> playZones = new Dictionary<Transform, List<GameObject>>();
    private static Dictionary<string, int> zoneLimits = new Dictionary<string, int>
    {
        { "DefenseZone", 5 },
        { "AttackZone", 3 }
    };

    public float hoverHeight = 1.0f;
    public float zSpacing = 0.8f;
    public float minDistance = 3f;

    private Vector3 originalPosition;
    private Transform currentZone = null;
    private bool isInPlayZone = false; // Флаг, что карта уже сыграна

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        // Если карта уже выложена на поле, её нельзя брать обратно
        if (isInPlayZone)
        {
            Debug.Log("Карту нельзя взять обратно!");
            return;
        }

        if (selectedCard == this.gameObject)
        {
            selectedCard = null;
            ResetColor();
        }
        else
        {
            if (selectedCard != null && selectedCard.GetComponent<CardSelector>() != null)
            {
                selectedCard.GetComponent<CardSelector>().ResetColor();
            }
            selectedCard = this.gameObject;
            HighlightCard();
        }
    }

    void ResetColor()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    void HighlightCard()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void TryPlaceCard(Transform zone)
    {
        if (zone == null) return;

        string zoneTag = zone.tag;

        // Если зона существует в лимитах (допустимая зона для карт)
        if (zoneLimits.ContainsKey(zoneTag))
        {
            if (!playZones.ContainsKey(zone))
            {
                playZones[zone] = new List<GameObject>();
            }

            List<GameObject> cardsInZone = playZones[zone];

            // Проверяем, можно ли добавить карту в зону
            if (cardsInZone.Count < zoneLimits[zoneTag])
            {
                // Если карта уже была в зоне, нельзя её перемещать
                if (isInPlayZone)
                {
                    Debug.Log("Карту нельзя перемещать между зонами!");
                    return;
                }

                // Если карта была в другой зоне, удаляем из старой зоны
                if (currentZone != null && playZones.ContainsKey(currentZone))
                {
                    playZones[currentZone].Remove(gameObject);
                }

                cardsInZone.Add(gameObject);
                currentZone = zone;
                isInPlayZone = true; // Устанавливаем флаг, что карта теперь в игре
                selectedCard = null;
                ResetColor();

                AlignCardsInZone(zone, cardsInZone);
            }
            else
            {
                StartCoroutine(ReturnToHand());
            }
        }
        else
        {
            StartCoroutine(ReturnToHand());
        }
    }

    void AlignCardsInZone(Transform zone, List<GameObject> cards)
    {
        if (zone == null || cards == null) return;

        float centerX = zone.position.x;
        float centerZ = zone.position.z;
        int count = cards.Count;

        float totalSpacing = (count - 1) * Mathf.Max(zSpacing, minDistance);
        float startZ = centerZ - totalSpacing / 2;

        for (int i = 0; i < count; i++)
        {
            float zOffset = startZ + i * Mathf.Max(zSpacing, minDistance);
            Vector3 newPosition = new Vector3(centerX, zone.position.y + hoverHeight, zOffset);
            cards[i].transform.position = newPosition;
        }
    }

    IEnumerator ReturnToHand()
    {
        float time = 0;
        Vector3 start = transform.position;
        while (time < 0.3f)
        {
            transform.position = Vector3.Lerp(start, originalPosition, time / 0.3f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
