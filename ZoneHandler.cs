using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneHandler : MonoBehaviour
{
    void OnMouseDown()
    {
        if (CardSelector.selectedCard != null)
        {
            CardSelector.selectedCard.GetComponent<CardSelector>().TryPlaceCard(transform);
        }
    }
}
