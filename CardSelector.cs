using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет выбором и размещением карт в игровых зонах.
/// </summary>
public class CardSelector : MonoBehaviour
{
    /// <summary>
    /// Выбранная карта.
    /// </summary>
    public static GameObject selectedCard = null;

    /// <summary>
    /// Словарь, хранящий игровые зоны и карты в них.
    /// </summary>
    private static Dictionary<Transform, List<GameObject>> playZones = new Dictionary<Transform, List<GameObject>>();

    /// <summary>
    /// Ограничения по количеству карт в каждой зоне.
    /// </summary>
    private static Dictionary<string, int> zoneLimits = new Dictionary<string, int>
    {
        { "DefenseZone", 3 },
        { "AttackZone", 5 }
    };

    /// <summary>
    /// Высота, на которую поднимается карта при выделении.
    /// </summary>
    public float hoverHeight = 1.0f;

    /// <summary>
    /// Расстояние между картами в зоне по оси Z.
    /// </summary>
    public float zSpacing = 0.8f;

    /// <summary>
    /// Минимальное расстояние между картами.
    /// </summary>
    public float minDistance = 3f;

    private Vector3 originalPosition;
    private Transform currentZone = null;
    private bool isInPlayZone = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
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

    /// <summary>
    /// Сбрасывает цвет карты на стандартный.
    /// </summary>
    void ResetColor()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    /// <summary>
    /// Выделяет карту, изменяя её цвет.
    /// </summary>
    void HighlightCard()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    /// <summary>
    /// Пытается разместить карту в указанной игровой зоне.
    /// </summary>
    /// <param name="zone">Игровая зона для размещения карты.</param>
    public void TryPlaceCard(Transform zone)
    {
        if (zone == null) return;

        // Получаем компонент Card, чтобы узнать стоимость маны
        Card cardComponent = GetComponent<Card>();
        if (cardComponent == null)
        {
            Debug.LogError("Компонент Card не найден!");
            return;
        }

        // Проверка маны
        if (!TurnManager.instance.HasEnoughMana(cardComponent.manaCost))
        {
            Debug.Log("Недостаточно маны для этой карты!");
            StartCoroutine(ReturnToHand());
            return;
        }

        // Если зона существует в лимитах (допустимая зона для карт)
        string zoneTag = zone.tag;
        if (TurnManager.instance != null && zoneLimits.ContainsKey(zoneTag))
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
                isInPlayZone = true;
                selectedCard = null;
                ResetColor();

                // Списываем ману
                TurnManager.instance.SpendMana(cardComponent.manaCost);

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


    /// <summary>
    /// Выравнивает карты в игровой зоне.
    /// </summary>
    /// <param name="zone">Игровая зона.</param>
    /// <param name="cards">Список карт в зоне.</param>
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

    /// <summary>
    /// Возвращает карту в руку при невозможности её размещения.
    /// </summary>
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
