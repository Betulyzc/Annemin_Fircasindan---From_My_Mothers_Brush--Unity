using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class PlateDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped != null)
        {
            dropped.GetComponent<Image>().enabled = false;

            if (dropped.TryGetComponent<CanvasGroup>(out var cg))
                cg.blocksRaycasts = false;

            if (dropped.TryGetComponent<DraggableItem>(out var drag))
            {
                drag.enabled = false;

                if (drag.labelText != null)
                {
                    string originalText = drag.labelText.text;
                    drag.labelText.text = $"<s>{originalText}</s>";
                }
            }

            KitchenSceneManager.Instance.FoodDropped();
        }
    }
}
