using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обрабатывает взаимодействие зон с картами.
/// </summary>
public class ZoneHandler : MonoBehaviour
{
    /// <summary>
    /// Обрабатывает нажатие на зону. Если выбрана карта, пытается разместить её в этой зоне.
    /// </summary>
    void OnMouseDown()
    {
        if (CardSelector.selectedCard != null)
        {
            CardSelector.selectedCard.GetComponent<CardSelector>().TryPlaceCard(transform);
        }
    }
}
