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
    public GameObject adventure_panel;
    public GameObject node_prefab;
    public GameObject edge_prefab;
    private GameObject[,] node_buttons;
    private GameObject[] edges;

    public List<GameObject> event_UI;

    public GameObject combat_panel;
    public GameObject combat_button_perfab;
    public GameObject turn_end_button;
    private List<GameObject> combat_buttons;    //★List에서 array로 변경할 것

    public GameObject reward_panel;
    public GameObject reward_button_prefab;
    private GameObject[] reward_buttons;
    public GameObject[] reward_buttons_ { get { return reward_buttons; } }

    public GameObject inventory_panel_outter;
    public GameObject inventory_panel_inner;
    private List<GameObject> inventory_buttons;
    public GameObject inventory_button_prefab;

    public GameObject[] detail_panel;
    private abst_Plr_action inventory_selection;
    public abst_Plr_action inventory_selection_ { get { return inventory_selection; } set { inventory_selection = value; } }

    public temp_json tj;

    private enum screen_type { screen_null, map, combat, event_screen, ability, shelter }
    private screen_type main_screen;


    public void initial_init() {    //because node_button should be created after GameManager's map / before graph generating, GraphicManager's creator should be easy to control its timing
        tj = new temp_json();
        main_screen = screen_type.screen_null;

        edges = new GameObject[143];
        for (int i = 0; i < 143; i++) {
            edges[i] = Instantiate(edge_prefab);
            edges[i].transform.SetParent(adventure_panel.transform, false);
        }

        node_buttons = new GameObject[11, 11];
        node[,] temp_map = GameManager.g.get_map();
        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                node_buttons[i, j] = Instantiate(node_prefab, new Vector2(510 + i * 90, 980 - j * 90), Quaternion.identity, canvas.transform);
                node_buttons[i, j].transform.SetParent(adventure_panel.transform, false);
                node_buttons[i, j].GetComponent<node>().init(i, j);
                temp_map[i, j] = node_buttons[i, j].GetComponent<node>();
                //node size=50, edge size=40, boundary size=65
            }
        }

        combat_buttons = new List<GameObject>();
        for (int i = 0; i < 6; i++) {
            combat_buttons.Add(Instantiate(combat_button_perfab, new Vector2(130 + i * 280, 160), Quaternion.identity, canvas.transform));
            combat_buttons[i].transform.SetParent(combat_panel.transform, false);
            combat_buttons[i].GetComponent<Plr_action_button>().combat_button_order_ = i;
        }

        reward_buttons = new GameObject[10];
        for (int i = 0; i < 10; i++) {
            reward_buttons[i] = Instantiate(reward_button_prefab, new Vector2(2100f, 2200f), Quaternion.identity, canvas.transform);
            //reward_buttons[i].transform.SetParent(reward_panel.transform, false);
        }

        inventory_buttons = new List<GameObject>();
    }

    public void init() { }

    public temp_json get_json(string name) {    //it's used only when outer class uses json directly, GraphicManager doesnt use this
        tj.init();
        tj = JsonConvert.DeserializeObject<temp_json>((Resources.Load(name, typeof(TextAsset)) as TextAsset).text);
        return tj;
    }

    #region adventure
    public void edge_placement() {
        node[,] temp_map = GameManager.g.get_map();
        int edge_count = 0;
        node[] temp_links;
        foreach (node n1 in temp_map) {
            temp_links = n1.get_link();
            for (int i = 2; i < 4; i++) {
                if (temp_links[i] != null) {
                    edges[edge_count].GetComponent<edge>().liner(n1.gameObject.GetComponent<RectTransform>().localPosition, temp_links[i].gameObject.GetComponent<RectTransform>().localPosition);
                    edge_count++;
                }
            }
        }
    }
    #endregion adventure

    #region combat
    public void combat_Plr_action_button_update() {
        List<abst_Plr_action> temp = GameManager.g.get_Plr().actions;

        for (int i=0; i<6; i++) {
            if (temp.Count > i) {
                combat_buttons[i].GetComponent<Plr_action_button>().card_update(temp[i]);
                combat_buttons[i].GetComponent<Plr_action_button>().set_target(temp[i]);
            } else {
                combat_buttons[i].SetActive(false);
            }
        }
    }
    #endregion combat

    #region reward
    public void reward_init() {
        int temp = 0;
        //try {   //get rewards from GameManager and set it into reward_buttons
            foreach (abst_Plr_action a in GameManager.g.rew.rewards_action_) {
                reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                reward_buttons_[temp].GetComponent<reward_button>().card_update(a);
                reward_buttons_[temp].SetActive(true);
                temp++;
            }
            foreach (abst_tool a in GameManager.g.rew.rewards_tool_) {
                reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                //reward_buttons_[temp].GetComponent<reward_button>().card_update((abst_Plr_action)a);
                reward_buttons_[temp].SetActive(true);
                temp++;
            }
            reward_buttons_[temp].GetComponent<reward_button>().set_reward(GameManager.g.rew.rewards_shard_);
            reward_buttons_[temp].GetComponent<reward_button>().card_update(new shards());
            reward_buttons_[temp].SetActive(true);
            while (++temp < 10) {
                reward_buttons_[temp].SetActive(false);
            }
        //} catch (Exception e) {
        //    Debug.Log(temp + "th reward setting error : " + e);
        //}
        GraphicManager.g.reward_update();
        GraphicManager.g.temp_reward_recover();
    }

    public void reward_update() {
        int temp = 0;
        foreach (GameObject g in reward_buttons) {
            if (g.activeSelf) {
                g.GetComponent<RectTransform>().localPosition = new Vector2(-640 + 320 * (temp%5), 220 - 440 * (temp / 5));
                temp++;
            }
        }
    }
    #endregion reward

    #region event
    public void event_output(string name) {
        tj.init();
        tj = JsonConvert.DeserializeObject<temp_json>((Resources.Load(name, typeof(TextAsset)) as TextAsset).text);
        event_UI[1].GetComponent<TextMeshProUGUI>().text = tj.s1;
        event_UI[2].GetComponent<TextMeshProUGUI>().text = tj.s2;
        for (int i = 0; i < 3; i++) {
            if (i < tj.s.Length) { event_UI[i + 3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tj.s[i]; event_UI[i + 3].SetActive(true); } else { event_UI[i + 3].SetActive(false); }
        }
        temp_event_recover();
    }
    #endregion event

    #region inventory_shelter
    public void inventory_update() {
        int temp = 0;
        foreach (GameObject g in inventory_buttons) {
            g.SetActive(false);
        }
        foreach (abst_Plr_action a in GameManager.g.get_Plr().action_inventory_) {
            if (temp >= inventory_buttons.Count) {
                inventory_buttons.Add(
                    Instantiate(inventory_button_prefab, new Vector2(0f, 0f), Quaternion.identity, canvas.transform)
                    );
                inventory_buttons[temp].transform.SetParent(inventory_panel_inner.transform, false);
                inventory_buttons[temp].GetComponent<RectTransform>().localPosition = new Vector2(-700 + 200 * (temp % 5), 328 - 245 * (temp / 5));
            }
            inventory_buttons[temp].GetComponent<inventory_button>().card_update(a);
            inventory_buttons[temp].GetComponent<inventory_button>().set_target(a);
            inventory_buttons[temp].SetActive(true);
            temp++;
        }
        inventory_panel_outter.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            GameManager.g.get_Plr().shards.ToString();
    }

    public void detail_init(abst_Plr_action a) {
        detail_panel[1].GetComponent<abst_Plr_action_image>().card_update(a);
        inventory_selection = a;
        try {
            if (GameManager.g.get_Plr().get_location().event_here.event_name == "shelter") {
                detail_panel[2].GetComponent<Button>().interactable = true;
                detail_panel[4].SetActive(false);
            } else {
                detail_panel[2].GetComponent<Button>().interactable = false;
                detail_panel[4].SetActive(true);
            }
        } catch (Exception e) { }
        temp_detail_recover();
    }
    #endregion inventory_shelter

    #region remove_recover
    public void temp_combat_remove() {
        combat_panel.GetComponent<RectTransform>().localPosition = combat_panel.GetComponent<RectTransform>().localPosition + new Vector3(0f, -1050f, 0f);
        /*
        foreach (GameObject g in combat_buttons) { g.GetComponent<RectTransform>().anchoredPosition = new Vector2 (g.GetComponent<RectTransform>().anchoredPosition.x, -640) ; }
        turn_end_button.GetComponent<RectTransform>().anchoredPosition = new Vector2 (1060, turn_end_button.GetComponent<RectTransform>().anchoredPosition.y);
        */
    }
    public void temp_combat_recover() {
        combat_panel.GetComponent<RectTransform>().localPosition = combat_panel.GetComponent<RectTransform>().localPosition + new Vector3(0f, 1050f, 0f);
        /*
        foreach (GameObject g in combat_buttons) { g.GetComponent<RectTransform>().anchoredPosition = new Vector2(g.GetComponent<RectTransform>().anchoredPosition.x, -360); }
        turn_end_button.GetComponent<RectTransform>().anchoredPosition = new Vector2 (793, turn_end_button.GetComponent<RectTransform>().anchoredPosition.y);
        */
    }

    public void temp_event_remove() {
        event_UI[0].GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(800f, 0f, 0f);
        //★main_screen 표시 방법 구상
    }
    public void temp_event_recover() {
        event_UI[0].GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(-800f, 0f, 0f);
    }

    public void temp_reward_remove() {
        reward_panel.GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(1920f, 0f, 0f);
        //★main_screen 표시 방법 구상
    }
    public void temp_reward_recover() {
        reward_panel.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }

    public void temp_inventory_remove() {
        inventory_panel_outter.GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(0f, -1020f, 0f);
        //★main_screen 표시 방법 구상
    }
    public void temp_inventory_recover() {
        inventory_panel_outter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }

    public void temp_detail_remove() { 
        detail_panel[0].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(-840f, 0f, 0f);
        detail_panel[3].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(-1920f, 0f, 0f);
    }
    public void temp_detail_recover() {
        detail_panel[0].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(840f, 0f, 0f);
        detail_panel[3].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }
    #endregion remove_recover

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
