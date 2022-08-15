using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphicManager : MonoBehaviour
{
    public static GraphicManager g;

    public List<GameObject> combat_Plr_action_buttons;

    public void init()
    {
        
    }

    public void combat_Plr_action_button_update() {
        for (int i=0;i<6;i++) {
            try
            {
                this.combat_Plr_action_buttons[i].GetComponent<Plr_action_button>().set_target(GameManager.g.get_Plr().actions[i]);
            }
            catch (Exception e) {
                break;
            }
        }
    }

    void Awake()
    {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
    }
}
