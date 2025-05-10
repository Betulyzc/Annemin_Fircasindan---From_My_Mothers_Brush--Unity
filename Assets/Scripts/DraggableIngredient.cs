using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI labelText; 
    private Vector3 startPos;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        startPos = transform.position;
        originalParent = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        
        if (transform.parent == originalParent)
        {
            transform.position = startPos;
        }
    }
}