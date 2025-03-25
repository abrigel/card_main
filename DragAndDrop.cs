using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragandDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = true;
    private Camera mainCamera;
    private Vector3 originalPosition;

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
                transform.position = hit.collider.transform.position; // Фиксируем карту в зоне
            }
            else
            {
                transform.position = originalPosition; // Возвращаем карту в руку
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
