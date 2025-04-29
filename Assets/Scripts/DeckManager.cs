using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handZone;
    public int maxHandSize = 5;
    public float cardSpacing = 0.15f;

    private List<GameObject> hand = new List<GameObject>();

    void OnMouseDown()
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Рука заполнена!");
            return;
        }
        DrawCards();
    }

    public void DrawCards()
    {
        int cardsToDraw = Mathf.Min(maxHandSize - hand.Count, 5);
        for (int i = 0; i < cardsToDraw; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(handZone, false);

            var data = card.GetComponent<CardData>();
            data.attack = Random.Range(1, 5);
            data.health = Random.Range(1, 6);
            data.manaCost = Random.Range(1, 4);

            hand.Add(card);
        }
        UpdateHandLayout();
    }

    public void RemoveCardFromHand(GameObject card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            UpdateHandLayout();
        }
    }

    private void UpdateHandLayout()
    {
        float angleStep = 10f;
        float radius = 1.5f;
        int count = hand.Count;
        float startAngle = -angleStep * (count - 1) / 2;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radius;
            Quaternion rot = Quaternion.Euler(0, angle, 0);

            hand[i].transform.localPosition = pos;
            hand[i].transform.localRotation = rot;
        }
    }

}
