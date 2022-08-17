using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;
using System.IO;

public class GraphicManager : MonoBehaviour
{
    public static GraphicManager g;

    public List<GameObject> combat_Plr_action_buttons;

    public temp_json tj;

    public void init()
    {
        
    }

    private void Update()
    {
        tj = JsonConvert.DeserializeObject<temp_json>((Resources.Load("asset_test", typeof(TextAsset)) as TextAsset).text);
        this.combat_Plr_action_buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tj.s1;   //¡Ú
        //Convert.ToString(this.combat_Plr_action_buttons[0].GetComponent<Plr_action_button>().get_target().get_cur_cooltime())
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

        tj = new temp_json();
    }

    /*private void json_practice()
    {
        test_class t2 = new test_class();
        t2.init("initiated", 8383);

        t2 = JsonConvert.DeserializeObject<test_class>((Resources.Load("asset_test", typeof(TextAsset)) as TextAsset).text);
        Debug.Log($" s = {t2.s}, i = {t2.i} ");
    }*/
}

/*public class test_class {
    public string s;
    public int i;

    public void init(string input1, int input2) {
        this.s = input1;
        this.i = input2;
    }
}*/
