using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausarMenuDojogo : MonoBehaviour
{
    public bool pausa = false;
    public Canvas canvas;
    

    private void Start()
    {
        Time.timeScale = 1.0f;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarGame();
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("CenaMenu");
    }

    public void PausarGame()
    {
        if (pausa==false)
        {
            Time.timeScale = 0f;
            pausa = true;
            canvas.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            pausa = false;
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }
}
