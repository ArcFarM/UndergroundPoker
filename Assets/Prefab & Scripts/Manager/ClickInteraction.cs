using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnderGroundPoker.Prefab.Card;

public class ClickInteraction : MonoBehaviour {
    [SerializeField] Camera targetCamera;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] float maxDistance = 100f;

    void Awake() {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    // 외부에서 클릭 이벤트(예: InputManager)가 이 메서드를 호출한다.
    public void ProcessClick(Vector2 screenPosition) {
        var cam = targetCamera;
        if (cam == null) return;

        var ray = cam.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out var hit, maxDistance, hitLayer)) return;

        var cardClick = hit.collider.GetComponentInParent<CardClick>();
        if (cardClick != null) {
            cardClick.OnPointerClick(null);
        }
    }
}