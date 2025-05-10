using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject introducitonPanel;
    public ScratchManager scratchManager; 

    void Start()
    {
        if (introducitonPanel!=null) { 
            introducitonPanel.SetActive(true);
        } 
    }
    public void StartGameButtonMetod()
    {
        SceneManager.LoadScene("GameScene");

    }

    public void StartDrawMethod()
    {
        introducitonPanel.SetActive(false);

        if (scratchManager != null)
        {
            scratchManager.EnableTuvalButton();
            
        }
    }

    public void BackToGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");

    }
}
