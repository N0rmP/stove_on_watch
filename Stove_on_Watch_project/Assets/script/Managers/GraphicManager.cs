using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI.Extensions;

public class GraphicManager : MonoBehaviour
{
    private float time;

    public GameObject clear_particle;
    public GameObject damage_particle;

    public static GraphicManager g;
    public GameObject canvas;
    public GameObject adventure_panel;
    public GameObject node_prefab;
    public GameObject edge_prefab;
    private GameObject[,] node_buttons;
    private GameObject[] edges;

    public GameObject Plr_board;

    public List<GameObject> event_UI;

    public GameObject combat_panel;
    public GameObject combat_board_prefab;
    private Stack<GameObject> combat_board_stack;
    public GameObject combat_button_perfab;
    private GameObject[] combat_buttons;    //★List에서 array로 변경할 것
    public GameObject power_block_prefab;
    private Stack<GameObject> power_block_stack;
    public GameObject power_information;
    public GameObject enemy_action_list;
    public GameObject enemy_action_block_prefab;
    private List<GameObject> enemy_cur_action_list;
    //private List<GameObject> enemy_next_action_list;
    private List<GameObject> enemy_discarded_action_list;
    public List<GameObject> range_nums;
    public GameObject unused_range_num;
    public GameObject turn_end_button;  //★사용하지 않을 것이라 예상되면 삭제할 것


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

    public GameObject curtain;

    private Dictionary<GameObject, bool> glowings;
    private readonly Color glow_lighten = new Color(0.01f, 0.01f, 0.01f, 0f);
    private Dictionary<GameObject, Vector3> movings;
    private Dictionary<TextMeshProUGUI, float> fadings_text;
    private Dictionary<Image, float> fadings_image;
    private readonly Color fade_emerge = new Color(0f, 0f, 0f, 0.1f);
    private Dictionary<TextMeshProUGUI, int> countings;
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

        combat_buttons = new GameObject[6];
        for (int i = 0; i < 6; i++) {
            combat_buttons[i] = Instantiate(combat_button_perfab, new Vector2(130 + i * 280, 160), Quaternion.identity, canvas.transform);
            combat_buttons[i].transform.SetParent(combat_panel.transform, false);
            combat_buttons[i].GetComponent<Plr_action_button>().combat_button_order_ = i;
        }

        power_block_stack = new Stack<GameObject>();
        for (int i = 0; i < 20; i++) {
            power_block_stack.Push(Instantiate(power_block_prefab, new Vector2(0, 1600), Quaternion.identity, canvas.transform));
        }

        combat_board_stack = new Stack<GameObject>();
        for (int i = 0; i < 5; i++) {
            combat_board_stack.Push(Instantiate(combat_board_prefab, new Vector2(2270f, -440f), Quaternion.identity, combat_panel.transform));
        }

        enemy_cur_action_list = new List<GameObject>();
        //enemy_next_action_list = new List<GameObject>();
        enemy_discarded_action_list = new List<GameObject>();

        reward_buttons = new GameObject[10];
        for (int i = 0; i < 10; i++) {
            reward_buttons[i] = Instantiate(reward_button_prefab, new Vector2(2100f, 2200f), Quaternion.identity, canvas.transform);
            reward_buttons[i].transform.SetParent(reward_panel.transform, false);
        }

        inventory_buttons = new List<GameObject>();

        glowings = new Dictionary<GameObject, bool>();
        movings = new Dictionary<GameObject, Vector3>();
        fadings_text = new Dictionary<TextMeshProUGUI, float>();
        fadings_image = new Dictionary<Image, float>();
        countings = new Dictionary<TextMeshProUGUI, int>();
    }

    public void init() {
        time = 0f;
        glowings.Clear();
        movings.Clear();
        fadings_text.Clear();
        fadings_image.Clear();
        countings.Clear();
    }

    #region general
    public void set_text(GameObject tar, String con) {
        tar.GetComponent<TextMeshProUGUI>().text = con;
    }

    public void set_image_color(GameObject tar, Color cor) {
        //glow precedes other color change
        foreach (GameObject g in glowings.Keys)
            if (g == tar)
                return;
        tar.GetComponent<Image>().color = cor;
    }

    public void set_image(GameObject tar, Sprite spr) {
        tar.GetComponent<Image>().sprite = spr;
    }

    public temp_json get_json(string name) {    //it's used only when outer class uses json directly, GraphicManager doesnt use this
        tj.init();
        try {
            tj = JsonConvert.DeserializeObject<temp_json>((Resources.Load(name, typeof(TextAsset)) as TextAsset).text);
        } catch (Exception e) {
            Debug.Log(name + " : json error");
        }
        return tj;
    }

    public void over() {
        curtain.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Game Over\nPress Anywhere To Continue";
        curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 0f, 0f, 0f);
        curtain.SetActive(true);
        fadings_add(curtain.GetComponent<Image>(), 0.5f);
        fadings_add(curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), 0.5f);
    }

    public void clear() {
        clear_particle.GetComponent<RectTransform>().localPosition =
            GameManager.g.get_combat_enemies().get_selected().combat_board_.GetComponent<RectTransform>().localPosition + 
            GameManager.g.get_combat_enemies().get_selected().combat_board_.transform.GetChild(2).GetComponent<RectTransform>().localPosition;
        clear_particle.GetComponent<UIParticleSystem>().StartParticleEmission();

        curtain.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The City Retrieves its Light";
        curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0f);
        curtain.SetActive(true);
        fadings_add(curtain.GetComponent<Image>(), 0.05f);
        fadings_add(curtain.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), 0.05f);
    }

    #region glow
    private void glow() {
        float temp_r = -1;
        GameObject[] temp_g = new GameObject[glowings.Keys.Count];
        glowings.Keys.CopyTo(temp_g, 0);
        foreach (GameObject g in temp_g) {
            if (glowings[g])
                g.GetComponent<Image>().color += glow_lighten;
            else
                g.GetComponent<Image>().color -= glow_lighten;

            temp_r = g.GetComponent<Image>().color.r;
            if ( temp_r > 0.99f) 
                glowings[g] = false;
            else if(temp_r < 0.01f)
                glowings[g] = true;
        }
    }

    public void glowings_add(GameObject g) {
        glowings[g] = true;
    }

    public void glowings_remove(GameObject g) {
        glowings.Remove(g);
    }

    public void glowings_clear() {
        glowings.Clear();
    }
    #endregion glow
    #region move
    private void move() {
        GameObject[] temp_g = new GameObject[movings.Keys.Count];
        movings.Keys.CopyTo(temp_g, 0);
        foreach (GameObject g in temp_g) {
            if (movings[g].magnitude < 1) {
                g.GetComponent<RectTransform>().localPosition += movings[g];
                movings.Remove(g);
            } else {
                g.GetComponent<RectTransform>().localPosition += movings[g] * 0.1f;
                movings[g] *= 0.9f;
            }
        }
    }

    public void movings_add(GameObject g, Vector2 goal) {
        if (movings.ContainsKey(g))
            movings_remove(g);
        Vector3 temp_v = (Vector3)goal - g.GetComponent<RectTransform>().localPosition;
        movings[g] = temp_v;
    }

    public void movings_remove(GameObject g) {
        movings.Remove(g);
    }
    public void movings_clear() {
        movings.Clear();
    }
    #endregion move
    #region fade
    private void fade() {
        TextMeshProUGUI[] temp_t = new TextMeshProUGUI[fadings_text.Keys.Count];
        Image[] temp_i = new Image[fadings_image.Count];
        fadings_text.Keys.CopyTo(temp_t, 0);
        fadings_image.Keys.CopyTo(temp_i, 0);

        //text
        foreach (TextMeshProUGUI t in temp_t) {
            if (fadings_text[t] > 0) {
                //emerge
                if (t.color.a > 0.95f) {
                    t.color += new Color(0f, 0f, 0f, 1f);
                    fadings_text.Remove(t);
                } else
                    t.color += fade_emerge * fadings_text[t];
            } else {
                //disapper
                if (t.color.a < 0.05f) {
                    t.color -= new Color(0f, 0f, 0f, 1f);
                    fadings_text.Remove(t);
                } else
                    t.color += fade_emerge * fadings_text[t];
            }    
        }

        //image
        foreach (Image i in temp_i) {
            if (fadings_image[i] > 0) {
                //emerge
                if (i.color.a > 0.95f) {
                    i.color += new Color(0f, 0f, 0f, 1f);
                    fadings_remove(i);
                } else
                    i.color += fade_emerge * fadings_image[i];
            } else {
                //disappear
                if (i.color.a < 0.05f) {
                    i.color -= new Color(0f, 0f, 0f, 1f);
                    fadings_image.Remove(i);
                } else
                    i.color += fade_emerge * fadings_image[i];
            }
        }
    }

    public void fadings_text_add(GameObject tar, float emerge_rate) {
        fadings_text[tar.GetComponent<TextMeshProUGUI>()] = emerge_rate;
    }
    public void fadings_add(TextMeshProUGUI tar, float emerge_rate) {
        fadings_text[tar] = emerge_rate;
    }
    public void fadings_text_remove(TextMeshProUGUI tar) {
        fadings_text.Remove(tar);
    }

    public void fadings_image_add(GameObject tar, float emerge_rate) {
        fadings_image[tar.GetComponent<Image>()] = emerge_rate;
    }
    public void fadings_add(Image tar, float emerge_rate) {
        fadings_image[tar] = emerge_rate;
    }
    public void fadings_remove(Image tar) {
        fadings_image.Remove(tar);
    }
    public void fadiings_clear() {
        fadings_image.Clear();
        fadings_text.Clear();
    }
    #endregion fade
    #region count
    private void count() {
        TextMeshProUGUI[] temp_t = new TextMeshProUGUI[countings.Count];
        countings.Keys.CopyTo(temp_t, 0);

        //★이걸 구현하려면 정말 코루틴뿐인 걸까?
        int temp_i = -1;
        foreach (TextMeshProUGUI t in temp_t) {
            temp_i = int.Parse(t.text);
            if (temp_i > countings[t]) {
                temp_i--;
                t.text = temp_i.ToString();
            } else if (int.Parse(t.text) < countings[t]) {
                temp_i++;
                t.text = temp_i.ToString();
            } else {
                countings_remove(t);
            }

        }
    }
    public void countings_add(GameObject tar, int goal) {
        TextMeshProUGUI temp_t = tar.GetComponent<TextMeshProUGUI>();
        if (countings.ContainsKey(temp_t))
            countings.Remove(temp_t);
        countings[temp_t] = goal;
    }
    public void countings_remove(TextMeshProUGUI tar) {
        countings.Remove(tar);
    }
    public void countings_clear() {
        countings.Clear();
    }
    #endregion count
    #endregion general

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
                    n1.set_adjacent_edge(i, edges[edge_count]);
                    temp_links[i].set_adjacent_edge(i - 2, edges[edge_count]);
                    edges[edge_count].SetActive(false);
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
                combat_buttons[i].GetComponent<Plr_action_button>().set_target(temp[i]);
                combat_buttons[i].GetComponent<Plr_action_button>().card_update();
            } else {
                combat_buttons[i].SetActive(false);
            }
        }
    }

    public void damaged_effect(thing t) {
        damage_particle.GetComponent<RectTransform>().localPosition =
            t.combat_board_.GetComponent<RectTransform>().localPosition +
            t.combat_board_.transform.GetChild(2).GetComponent<RectTransform>().localPosition;
        damage_particle.GetComponent<UIParticleSystem>().StartParticleEmission();
    }

    public void prepare_power_block(abst_power ap) {
        GameObject temp = power_block_stack.Pop();
        ap.set_block(temp);
        temp.GetComponent<power_block>().set_power(ap);
        temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ap.get_title();
        temp.transform.SetParent(ap.get_owner().combat_board_.transform.GetChild(3).GetChild(0).GetChild(0));
    }

    public void push_power_block(GameObject g) {
        g.GetComponent<power_block>().set_power(null);
        g.transform.SetParent(canvas.transform);
        g.GetComponent<RectTransform>().localPosition = new Vector2(0f, 1600f);
        power_block_stack.Push(g);
    }

    public void prepare_combat_board(abst_enemy ae) {
        ae.combat_board_ = combat_board_stack.Pop();
    }

    public void push_combat_board(GameObject g) {
        combat_board_stack.Push(g);
    }

    public void show_power_information(power_block pb) {
        //★power_information의 위치를 다른 것에 의존토록 변경, 현재로써는 enemy_board에 상관해서 위치가 마우스 옆이 아니라 이상한 데에 가있다
        power_information.GetComponent<RectTransform>().localPosition =
            pb.GetComponent<RectTransform>().localPosition + new Vector3(-280, 0);
        power_information.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pb.get_power().get_title();
        power_information.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pb.get_power().get_description();
        power_information.SetActive(true);
    }

    public void show_action_list() {
        //enemy_cur_action_list.Add(Instantiate(enemy_action_block_prefab, new Vector2(0, 0), Quaternion.identity, enemy_action_list.transform));
        //enemy_cur_action_list[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 * i);

        abst_enemy ae = GameManager.g.get_selected_enemy();
        abst_enemy_action[] temp_next_actions = new abst_enemy_action[ae.next_actions_.Count];
        ae.next_actions_.CopyTo(temp_next_actions, 0);
        int left_index = 0;
        int goal_num;
        int loop_num;

        //solve lack of blocks
        goal_num = ae.cur_action_list_.Count + ae.passives_.Count + temp_next_actions.Length;
        if (goal_num > enemy_cur_action_list.Count)
            action_list_fitter(enemy_cur_action_list, goal_num);
        if (ae.discarded_action_list_.Count > enemy_discarded_action_list.Count)
            action_list_fitter(enemy_discarded_action_list, ae.discarded_action_list_.Count);

        //show cur_action_list
        goal_num = ae.cur_action_list_.Count;
        loop_num = 0;
        while (loop_num < goal_num) {
            enemy_cur_action_list[left_index].GetComponent<action_list_block>().assigned_action_ = ae.cur_action_list_[loop_num];
            enemy_cur_action_list[left_index].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            glowings.Remove(enemy_cur_action_list[left_index]);
            left_index++; loop_num++;
            //enemy_cur_action_list[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ae.cur_action_list_[i].action_name_;
            //enemy_cur_action_list[i].SetActive(true);
        }

        //show passives
        goal_num = ae.passives_.Count;
        loop_num = 0;
        while (loop_num < goal_num) {
            enemy_cur_action_list[left_index].GetComponent<action_list_block>().assigned_power_ = ae.passives_[loop_num];
            enemy_cur_action_list[left_index].GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            glowings.Remove(enemy_cur_action_list[left_index]);
            left_index++; loop_num++;
        }

        //show next_action_Queue
        goal_num = temp_next_actions.Length;
        loop_num = 0;
        while (loop_num < goal_num) {
            enemy_cur_action_list[left_index].GetComponent<action_list_block>().assigned_action_ = temp_next_actions[loop_num];
            enemy_cur_action_list[left_index].GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            glowings[enemy_cur_action_list[left_index]] = true;
            left_index++; loop_num++;
        }

        //hide not used left list
        goal_num = enemy_cur_action_list.Count;
        while (left_index < goal_num) {
            enemy_cur_action_list[left_index].SetActive(false);
            left_index++;
        }

        //show discarded_action_list
        goal_num = ae.discarded_action_list_.Count;
        loop_num = 0;
        while (loop_num < goal_num) {
            enemy_discarded_action_list[loop_num].GetComponent<action_list_block>().assigned_action_ = ae.discarded_action_list_[loop_num];
            loop_num++;
        }

        //hide not used right list
        goal_num = enemy_discarded_action_list.Count;
        while (loop_num < goal_num) {
            enemy_discarded_action_list[loop_num].SetActive(false);
            loop_num++;
        }

        enemy_action_list.transform.SetAsLastSibling();
        enemy_action_list.SetActive(true);
    }

    private void action_list_fitter(List<GameObject> list_g, int goal_num) {
        GameObject temp;
        bool is_cur = (list_g == enemy_cur_action_list);
        //bool is_next = (list_g == enemy_next_action_list);

        while (goal_num > list_g.Count) { 
            temp = Instantiate(enemy_action_block_prefab, new Vector2(0, 0), Quaternion.identity, enemy_action_list.transform);
            if (is_cur)
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 * enemy_cur_action_list.Count);
            //else if (is_next)
            //    temp.GetComponent<action_list_block>().is_next_ = true;
            else {
                temp = Instantiate(enemy_action_block_prefab, new Vector2(0, 0), Quaternion.identity, enemy_action_list.transform);
                temp.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
                temp.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                temp.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 * enemy_discarded_action_list.Count);
                temp.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }
            list_g.Add(temp);
        }
    }

    public void show_action_list_description(abst_enemy_action ae) {
        enemy_action_list.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ae.action_name_;
        enemy_action_list.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = ae.action_description_;
    }

    public void show_action_list_description(abst_power ap) {
        enemy_action_list.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ap.get_title();
        enemy_action_list.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = ap.get_description();
    }

    public void hide_action_list() {
        enemy_action_list.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        enemy_action_list.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        enemy_action_list.SetActive(false);
    }

    public void range_increase() {
        unused_range_num.GetComponent<TextMeshProUGUI>().text = (GameManager.g.get_left_of_range() + 4).ToString();
        unused_range_num.GetComponent<RectTransform>().localPosition = new Vector3(600f, 0f, 0f);
        range_nums.Add(unused_range_num);
        for (int i = 0; i < 6; i++) {
            movings_add(range_nums[i], new Vector2(120 * (i - 1), 0f));
        }
        fadings_add(range_nums[0].GetComponent<TextMeshProUGUI>(), -2);
        fadings_add(range_nums[5].GetComponent<TextMeshProUGUI>(), 2);
        unused_range_num = range_nums[0];
        range_nums.RemoveAt(0);
    }

    public void range_decrease() {
        unused_range_num.GetComponent<TextMeshProUGUI>().text = (GameManager.g.get_left_of_range()).ToString();
        unused_range_num.GetComponent<RectTransform>().localPosition = new Vector3(-120f, 0f, 0f);
        range_nums.Insert(0,unused_range_num);
        for (int i = 0; i < 6; i++) {
            movings_add(range_nums[i], new Vector2(120 * i, 0f));
        }
        fadings_add(range_nums[0].GetComponent<TextMeshProUGUI>(), 2);
        fadings_add(range_nums[5].GetComponent<TextMeshProUGUI>(), -2);
        unused_range_num = range_nums[5];
        range_nums.RemoveAt(5);
    }
    #endregion combat

    #region reward
    public void reward_init() {
        int temp = 0;
        //try {   //get rewards from GameManager and set it into reward_buttons
            foreach (abst_Plr_action a in GameManager.g.rew.rewards_action_) {
                reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                reward_buttons_[temp].GetComponent<reward_button>().card_update();
                temp++;
            }
            /*foreach (abst_tool a in GameManager.g.rew.rewards_tool_) {
                reward_buttons_[temp].GetComponent<reward_button>().set_reward(a);
                //reward_buttons_[temp].GetComponent<reward_button>().card_update((abst_Plr_action)a);
                reward_buttons_[temp].SetActive(true);
                temp++;
            }*/
            /*
            rewards_shard.amount_ = GameManager.g.rew.rewards_shard_;
            reward_buttons_[temp].GetComponent<reward_button>().set_reward(rewards_shard);
            reward_buttons_[temp].GetComponent<reward_button>().card_update();*/
            while (temp < 10) {
                reward_buttons_[temp].SetActive(false);
                temp++;
            }
        //} catch (Exception e) {
        //    Debug.Log(temp + "th reward setting error : " + e);
        //}
        GraphicManager.g.reward_update();
        GraphicManager.g.reward_recover();
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
        temp_json temp_tj;
        //tj = JsonConvert.DeserializeObject<temp_json>((Resources.Load("Event/"+name, typeof(TextAsset)) as TextAsset).text);
        temp_tj = get_json("Event/" + name);
        event_UI[1].GetComponent<TextMeshProUGUI>().text = tj.s1;
        event_UI[2].GetComponent<TextMeshProUGUI>().text = tj.s2;
        for (int i = 0; i < 3; i++) {
            if (i < tj.s.Length) { event_UI[i + 3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tj.s[i]; event_UI[i + 3].SetActive(true); } else { event_UI[i + 3].SetActive(false); }
        }
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
            //inventory_buttons[temp].GetComponent<inventory_button>().set_target(a);
            inventory_buttons[temp].GetComponent<inventory_button>().card_update(a);
            inventory_buttons[temp].SetActive(true);
            temp++;
        }
        inventory_panel_outter.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            GameManager.g.get_Plr().get_shards().ToString();
    }

    public void detail_init(abst_Plr_action a) {
        detail_panel[1].GetComponent<abst_Plr_action_image>().card_update(a);
        inventory_selection = a;
        try {
            if (GameManager.g.get_Plr().get_location().event_here_.GetType() == typeof(shelter)) {
                if(GameManager.g.get_Plr().get_shards() > 99)
                    detail_panel[2].GetComponent<Button>().interactable = true;
                else
                    detail_panel[2].GetComponent<Button>().interactable = false;
                detail_panel[4].SetActive(false);
            } else {
                detail_panel[2].GetComponent<Button>().interactable = false;
                detail_panel[4].SetActive(true);
            }
        } catch (Exception e) { }
        detail_recover();
    }
    #endregion inventory_shelter

    #region remove_recover
    public void combat_remove() {
        movings_add(combat_panel, new Vector2(0f, -1080f));
        movings_add(Plr_board.transform.GetChild(3).gameObject, new Vector2(150f, -980f));
        //combat_panel.GetComponent<RectTransform>().localPosition = combat_panel.GetComponent<RectTransform>().localPosition + new Vector3(0f, -1080f, 0f);
    }
    public void combat_recover() {
        movings_add(combat_panel, new Vector2(0f, 0f));
        movings_add(Plr_board.transform.GetChild(3).gameObject, new Vector2(150f, 250f));
        //combat_panel.GetComponent<RectTransform>().localPosition = combat_panel.GetComponent<RectTransform>().localPosition + new Vector3(0f, 1080f, 0f);
    }

    public void event_remove() {
        movings_add(event_UI[0], new Vector2(1380f, 0f));
        movings_add(event_UI[6], new Vector2(1920f, 0f));
        //event_UI[0].GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(800f, 0f, 0f);
    }
    public void event_recover() {
        movings_add(event_UI[0], new Vector2(540f, 0f));
        movings_add(event_UI[6], new Vector2(0f, 0f));
        //event_UI[0].GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(-800f, 0f, 0f);
    }

    public void reward_remove() {
        movings_add(reward_panel, new Vector2(0f, 1080f));
        //reward_panel.GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(1920f, 0f, 0f);
    }
    public void reward_recover() {
        movings_add(reward_panel, new Vector2(0f, 0f));
        //reward_panel.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }

    public void inventory_remove() {
        movings_add(inventory_panel_outter, new Vector2(0f, -1080f));
        //inventory_panel_outter.GetComponent<RectTransform>().localPosition = event_UI[0].GetComponent<RectTransform>().localPosition + new Vector3(-1770f, 1000f, 0f);
    }
    public void inventory_recover() {
        movings_add(inventory_panel_outter, new Vector2(0f, 0f));
        //inventory_panel_outter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }

    public void detail_remove() {
        movings_add(detail_panel[0], new Vector2(-1380f, 0f));
        movings_add(detail_panel[3], new Vector2(-1920f, 0f));
        //detail_panel[0].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(-840f, 0f, 0f);
        //detail_panel[3].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(-1920f, 0f, 0f);
    }
    public void detail_recover() {
        movings_add(detail_panel[0], new Vector2(-540f, 0f));
        movings_add(detail_panel[3], new Vector2(0f, 0f));
        //detail_panel[0].GetComponent<RectTransform>().localPosition = detail_panel[0].GetComponent<RectTransform>().localPosition + new Vector3(840f, 0f, 0f);
        //detail_panel[3].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
    }

    public void curtain_remove() {
        curtain.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
        curtain.SetActive(false);
    }
    public void curtain_recover() {
        curtain.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        curtain.SetActive(true);
    }
    #endregion remove_recover

    public void FixedUpdate() {
        glow();
        move();
        fade();

        time += Time.deltaTime;
        if (time > 0.01f) {
            count();
            time = 0f;
        }
    }

    void Awake()
    {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        //DontDestroyOnLoad(this.gameObject);
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
