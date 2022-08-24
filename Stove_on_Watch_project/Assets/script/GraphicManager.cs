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
    public GameObject canvas;
    public GameObject combat_panel;
    public GameObject adventure_panel;
    public GameObject node_prefab;
    public GameObject edge_prefab;

    private GameObject[,] node_buttons;
    private GameObject[] edges;

    public List<GameObject> combat_Plr_action_buttons;
    public GameObject turn_end_button;

    public temp_json tj;

    public void initial_init() {    //because node_button is connected with GameManager's map, so GraphicManager's creator should be easy to control when it activates
        tj = new temp_json();

        edges = new GameObject[143];
        for (int i = 0; i < 143; i++) {
            edges[i] = Instantiate(edge_prefab);
            edges[i].transform.SetParent(adventure_panel.transform, false);
        }

        node_buttons = new GameObject[11, 11];
        node[,] temp_map = GameManager.g.get_map();
        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                node_buttons[i, j] = Instantiate(this.node_prefab, new Vector2(510 + i * 90, 980 - j * 90), Quaternion.identity, canvas.transform);
                node_buttons[i, j].transform.SetParent(adventure_panel.transform, false);
                node_buttons[i, j].GetComponent<node>().init(i, j);
                temp_map[i, j] = node_buttons[i, j].GetComponent<node>();
                //node size=50, edge size=40, boundary size=65
            }
        }
    }
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

    public void edge_placement() {
        node[,] temp_map = GameManager.g.get_map();
        int edge_count = 0;
        node[] temp_links;
        foreach (node n1 in temp_map) {
            temp_links = n1.get_link();
            for (int i= 2; i<4; i++) {
                if (temp_links[i] != null) {
                    edges[edge_count].GetComponent<edge>().liner(n1.gameObject.GetComponent<RectTransform>().localPosition, temp_links[i].gameObject.GetComponent<RectTransform>().localPosition);
                    edge_count++;
                }
            }
        }
    }

    public void temp_combat_remove() {
        foreach (GameObject g in combat_Plr_action_buttons) { g.GetComponent<RectTransform>().anchoredPosition = new Vector2 (g.GetComponent<RectTransform>().anchoredPosition.x, -640) ; }
        turn_end_button.GetComponent<RectTransform>().anchoredPosition = new Vector2 (1060, turn_end_button.GetComponent<RectTransform>().anchoredPosition.y);
    }
    public void temp_combat_recover()
    {
        foreach (GameObject g in combat_Plr_action_buttons) { g.GetComponent<RectTransform>().anchoredPosition = new Vector2(g.GetComponent<RectTransform>().anchoredPosition.x, -360); }
        turn_end_button.GetComponent<RectTransform>().anchoredPosition = new Vector2 (793, turn_end_button.GetComponent<RectTransform>().anchoredPosition.y);
    }

    void Awake()
    {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
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
