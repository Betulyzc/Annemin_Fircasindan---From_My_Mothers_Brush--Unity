using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 


public class ScratchManager : MonoBehaviour
{
    public MemoryData memoryData; 

    public RawImage scratchArea;
    public Texture brushTexture;
    public RenderTexture renderTexture;
    public Image backgroundImage;

    public Image revealedImage;
    public GameObject talePanel;
    public TextMeshProUGUI taleText;
    public Button nextButton;
    public Image tuval;
    public Button tuvalDekorButton;
    private bool canDraw = false;
    private bool mouseWasPressed = false;



    private int currentIndex = 0;
    private bool entryRevealed = false;

    void Start()
    {
        ClearRenderTexture();
        LoadMemoryEntry();
        talePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
        tuval.gameObject.SetActive(false);
        tuvalDekorButton.interactable = false;
        tuvalDekorButton.onClick.AddListener(() =>
        {
            tuval.gameObject.SetActive(true);
            canDraw = false;
            mouseWasPressed = true;
            tuvalDekorButton.gameObject.SetActive(false);

            Color originalColor = backgroundImage.color;
            Color.RGBToHSV(originalColor, out float h, out float s, out float v);
            Color adjustedColor = Color.HSVToRGB(h, s, 0.30f);
            adjustedColor.a = originalColor.a; 
            backgroundImage.color = adjustedColor;
        });

    }

    void Update()
    {
        
        if (mouseWasPressed && !Input.GetMouseButton(0))
        {
            canDraw = true;
            mouseWasPressed = false;
        }

        if (entryRevealed || !canDraw) return;

        Vector2 drawPosition;
        if (Input.GetMouseButton(0))
        {
            if (TryGetTouchPosition(out drawPosition))
            {
                DrawAt(drawPosition);
            }
        }

        if (GetRevealProgress() > 0.99 && !entryRevealed)
        {
            entryRevealed = true;
            talePanel.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }

    public void EnableTuvalButton()
    {
        tuvalDekorButton.gameObject.SetActive(true);
        tuvalDekorButton.interactable = true;
    }
    bool TryGetTouchPosition(out Vector2 localCursor)
    {
        localCursor = Vector2.zero;
        Vector2 screenPos = Input.mousePosition;

        if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }

        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scratchArea.rectTransform,
            screenPos,
            null,
            out localCursor
        );
    }

    void DrawAt(Vector2 localCursor)
    {
        RectTransform rt = scratchArea.rectTransform;
        Vector2 pivotAdjusted = new Vector2(
            localCursor.x + rt.rect.width * rt.pivot.x,
            localCursor.y + rt.rect.height * rt.pivot.y
        );

        int px = Mathf.Clamp((int)(pivotAdjusted.x * renderTexture.width / rt.rect.width), 0, renderTexture.width);
        int py = renderTexture.height - Mathf.Clamp(
            (int)(pivotAdjusted.y * renderTexture.height / rt.rect.height), 0, renderTexture.height);

        RenderTexture.active = renderTexture;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);

        Graphics.DrawTexture(
            new Rect(px - brushTexture.width / 2, py - brushTexture.height / 2,
                     brushTexture.width, brushTexture.height),
            brushTexture
        );

        GL.PopMatrix();
        RenderTexture.active = null;
    }

    float GetRevealProgress()
    {
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();
        RenderTexture.active = null;

        Color[] pixels = tempTexture.GetPixels();
        int nonWhitePixelCount = 0;
        foreach (Color c in pixels)
        {
            if (c.r < 0.95f) nonWhitePixelCount++;
        }

        Destroy(tempTexture);
        return (float)nonWhitePixelCount / pixels.Length;
    }

    void ClearRenderTexture()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;
    }

    void LoadMemoryEntry()
    {
        var entry = memoryData.entries[currentIndex];
        revealedImage.sprite = entry.image;
        taleText.text = entry.caption;

        if (currentIndex == memoryData.entries.Count - 1)
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Haydi mutfaða";
        }
        else
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Devam Et";
        }
    }

    public void LoadNextEntry()
    {
        if (currentIndex >= memoryData.entries.Count - 1)
        {
            SceneManager.LoadScene("KitchenScene");
            return;
        }

        currentIndex++;
        ClearRenderTexture();
        LoadMemoryEntry();
        entryRevealed = false;
        talePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

}
