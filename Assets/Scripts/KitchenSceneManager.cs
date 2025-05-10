using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class KitchenSceneManager : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject tezgahPanel;
    public Button startCookingButton;

    public Image plateImage;
    public Sprite fullPlateSprite;
    public Button continueButton;

    public int totalNeeded = 4; //draggable object count

    private int droppedCount = 0;
    public static KitchenSceneManager Instance;
    public Image backgroundImage;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        introPanel.SetActive(true);
        tezgahPanel.SetActive(false);

        continueButton.gameObject.SetActive(false);

        startCookingButton.onClick.AddListener(() =>
        {
            introPanel.SetActive(false);
            tezgahPanel.SetActive(true);

            
            if (backgroundImage != null)
            {
                Color originalColor = backgroundImage.color;
                Color.RGBToHSV(originalColor, out float h, out float s, out float v);
                Color adjustedColor = Color.HSVToRGB(h, s, 0.30f); 
                adjustedColor.a = originalColor.a;
                backgroundImage.color = adjustedColor;
            }
        }
        );

        continueButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("FinalScene");
        });
    }

    public void FoodDropped()
    {
        droppedCount++;

        if (droppedCount >= totalNeeded)
        {
            plateImage.sprite = fullPlateSprite;
            continueButton.gameObject.SetActive(true);
        }
    }
}
