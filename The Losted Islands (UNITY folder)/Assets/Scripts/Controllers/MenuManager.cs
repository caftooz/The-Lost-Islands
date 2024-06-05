using DarkHorizon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    static public bool isPaused = false;
    [SerializeField] GameObject canvasPause;
    [SerializeField] GameObject canvasOptions;

    [SerializeField] GameObject main;
    public GameObject UIPanel;
    public GameObject InventoryPanel;

    private void Start()
    {
        canvasPause.SetActive(false);
        canvasOptions.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                if (InventoryManager.inventoryIsOpened)
                {
                    InventoryManager.inventoryIsOpened = false;
                    UIPanel.SetActive(false);
                    InventoryPanel.SetActive(false);
                    isPaused = false;
                    main.GetComponent<RotatePl>().enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else 
                openPause();
            } 
            else
            {
                PauseOptionsQuit();
                quitPause();
            }
        }



    }

    private void openPause()
    {
        canvasPause.SetActive(true);
        main.GetComponent<RotatePl>().enabled = false;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void quitPause()
    {
        canvasPause.SetActive(false);
        main.GetComponent<RotatePl>().enabled = true;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PauseOptions()
    {
        canvasPause.SetActive(false);
        canvasOptions.SetActive(true);
    }
    public void PauseOptionsQuit()
    {
        canvasPause.SetActive(true);
        canvasOptions.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
