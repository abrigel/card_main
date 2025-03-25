using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    private static GameObject selectedCard = null; // Выбранная карта
    private static Dictionary<Transform, List<GameObject>> playZones = new Dictionary<Transform, List<GameObject>>();
    private static Dictionary<string, int> zoneLimits = new Dictionary<string, int>
    {
        { "DefenseZone", 5 },  // В защитной зоне 5 карт
        { "AttackZone", 3 }     // В атакующей зоне 3 карты
    };

    public float hoverHeight = 1.0f; // Высота над зоной
    public float zSpacing = 0.5f; // Расстояние между картами

    private Vector3 originalPosition;
    private Transform currentZone = null;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (selectedCard == this.gameObject)
        {
            selectedCard = null; // Отмена выбора
            ResetColor();
        }
        else
        {
            if (selectedCard != null)
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
        string zoneTag = zone.tag;

        if (zoneLimits.ContainsKey(zoneTag))
        {
            if (!playZones.ContainsKey(zone))
            {
                playZones[zone] = new List<GameObject>();
            }

            List<GameObject> cardsInZone = playZones[zone];

            if (cardsInZone.Count < zoneLimits[zoneTag])
            {
                if (currentZone != null)
                {
                    playZones[currentZone].Remove(gameObject);
                }

                cardsInZone.Add(gameObject);
                currentZone = zone;
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
        float centerX = zone.position.x;
        float centerZ = zone.position.z;
        int count = cards.Count;

        for (int i = 0; i < count; i++)
        {
            float zOffset = (i - (count - 1) / 2.0f) * zSpacing;
            Vector3 newPosition = new Vector3(centerX, zone.position.y + hoverHeight, centerZ + zOffset);
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
