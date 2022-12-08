using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class curtain : MonoBehaviour
{
    //public GameObject curtain_go;
    private float time;
    private Color anti_transparent = new Color(0f, 0f, 0f, 0.01f);

    void curtain_black(Scene scene, LoadSceneMode mode) {
        time = 0f;
        gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += curtain_black;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= curtain_black;
    }

    public void FixedUpdate() {
        time += Time.deltaTime;
        if (time > 0.1f) {
            gameObject.GetComponent<Image>().color -= anti_transparent;
            if (gameObject.GetComponent<Image>().color.a < 0.02f)
                gameObject.SetActive(false);
        }
    }
}
