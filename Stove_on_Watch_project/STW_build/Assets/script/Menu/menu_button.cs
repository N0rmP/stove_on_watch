using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_button : MonoBehaviour
{
    public void game_start() {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void quit() {
        Application.Quit();
    }
}
