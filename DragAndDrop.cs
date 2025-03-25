using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Transform currentZone = null;

    private static Dictionary<Transform, List<GameObject>> playZones = new Dictionary<Transform, List<GameObject>>();
    private static Dictionary<string, int> zoneLimits = new Dictionary<string, int>
    {
        { "DefenseZone", 5 },
        { "AttackZone", 3 }
    };

    public float hoverHeight = 1.0f;
    public float returnSpeed = 0.3f;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        TryPlaceCard();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    void TryPlaceCard()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            string zoneTag = hit.collider.tag;

            if (zoneLimits.ContainsKey(zoneTag))
            {
                Transform zoneTransform = hit.collider.transform;

                if (!playZones.ContainsKey(zoneTransform))
                {
                    playZones[zoneTransform] = new List<GameObject>();
                }

                List<GameObject> cardsInZone = playZones[zoneTransform];

                if (cardsInZone.Count < zoneLimits[zoneTag])
                {
                    if (currentZone != null)
                    {
                        playZones[currentZone].Remove(gameObject);
                    }

                    cardsInZone.Add(gameObject);
                    currentZone = zoneTransform;

                    AlignCardsInZone(zoneTransform, cardsInZone);
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
        else
        {
            StartCoroutine(ReturnToHand());
        }
    }

    void AlignCardsInZone(Transform zone, List<GameObject> cards)
    {
        float zoneWidth = 5.0f;  // Ширина зоны
        float startX = zone.position.x - zoneWidth / 2;
        float spacing = zoneWidth / Mathf.Max(1, cards.Count);

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 newPosition = new Vector3(startX + i * spacing, zone.position.y + hoverHeight, zone.position.z);
            cards[i].transform.position = newPosition;
        }
    }

    IEnumerator ReturnToHand()
    {
        float time = 0;
        Vector3 start = transform.position;
        while (time < returnSpeed)
        {
            transform.position = Vector3.Lerp(start, originalPosition, time / returnSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
