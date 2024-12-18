using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PainelMenuScript : MonoBehaviour
{
    public Button startGame;
    public Button QuitGame;// Start is called before the first frame update
    

    public void clickBtnStartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void clickBtnQuitGame() 
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
                Application.Quit();
    #endif
    }
}
