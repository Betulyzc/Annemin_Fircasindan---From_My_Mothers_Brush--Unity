using UnityEngine;

public class BrushManager : MonoBehaviour
{
    public GameObject brushPrefab;
    public RectTransform scratchArea;

    private GameObject currentBrush;
    public AudioSource brushAudioSource; 
    public AudioClip brushSound;
    private bool isPlayingSound = false;


    void Update()
    {
        Vector2 screenPos = Input.mousePosition;

        if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                scratchArea, screenPos, null, out localPoint))
            {
                if (currentBrush == null)
                {
                    currentBrush = Instantiate(brushPrefab, scratchArea);
                }

                currentBrush.GetComponent<RectTransform>().anchoredPosition = localPoint;

                if (!isPlayingSound)
                {
                    brushAudioSource.clip = brushSound;
                    brushAudioSource.loop = true;
                    brushAudioSource.Play();
                    isPlayingSound = true;
                }
            }
        }
        else
        {
            if (currentBrush != null)
            {
                Destroy(currentBrush);
            }

            if (isPlayingSound)
            {
                brushAudioSource.Stop();
                isPlayingSound = false;
            }
        }
    }
}