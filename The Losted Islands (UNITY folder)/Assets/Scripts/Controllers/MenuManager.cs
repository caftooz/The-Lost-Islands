using DarkHorizon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    static public bool isPaused = false;
    [SerializeField] Canvas canvasPause;
    [SerializeField] Canvas canvasOptions;

    [SerializeField] GameObject main;
    public GameObject UIPanel;
    public GameObject InventoryPanel;

    private void Start()
    {
        canvasPause.enabled = false;
        canvasOptions.enabled = false;

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
        canvasPause.enabled = true;
        main.GetComponent<RotatePl>().enabled = false;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void quitPause()
    {
        canvasPause.enabled = false;
        main.GetComponent<RotatePl>().enabled = true;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PauseOptions()
    {
        canvasPause.enabled = false;
        canvasOptions.enabled = true;
    }
    public void PauseOptionsQuit()
    {
        canvasPause.enabled = true;
        canvasOptions.enabled = false;
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
