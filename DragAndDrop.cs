using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragandDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = true;
    private Camera mainCamera;
    private Vector3 originalPosition;
    private static Dictionary<Transform, List<GameObject>> playZones = new Dictionary<Transform, List<GameObject>>();

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
            if (hit.collider.CompareTag("PlayZone"))
            {
                Transform zoneTransform = hit.collider.transform;

                if (!playZones.ContainsKey(zoneTransform))
                {
                    playZones[zoneTransform] = new List<GameObject>();
                }

                List<GameObject> cardsInZone = playZones[zoneTransform];
                cardsInZone.Add(gameObject);

                Vector3 newPosition = zoneTransform.position + new Vector3(cardsInZone.Count * 1.5f, 0, 0);
                transform.position = newPosition;

                originalPosition = transform.position;
            }
            else
            {
                transform.position = originalPosition;
            }
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    /*
    void Update()
    {
        
    }*/
}
