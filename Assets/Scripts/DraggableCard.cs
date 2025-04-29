using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform originalParent;
    private Camera cam;
    private Collider[] zoneColliders;

    void Start()
    {
        cam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            transform.position = hit.point + new Vector3(0, 0.5f, 0); // приподнимаем чуть
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var zone = hit.collider.GetComponent<ZoneHandler>();
            if (zone != null)
            {
                // Отцепляем от старой зоны
                var oldZone = originalParent?.GetComponent<ZoneHandler>();
                oldZone?.RemoveCard(GetComponent<CardData>());

                // Присоединяем к новой
                zone.AddCard(GetComponent<CardData>());
                return;
            }
        }

        // Если не попал в зону — возвращаем назад
        transform.position = startPosition;
    }
}
