using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager g = null;

    private int left_of_range;
    bool is_combat; //0=not in combat / 1=during combat / 2=combat end    ��is it possible to develop this with boolean?
    private bool is_Plr_turn;

    public xoshiro ran;
    graph_generator gra;

    private node[,] map;
    private node selected_node;

    //public delegate void order_func(int i);
    public Queue<abst_action> order_list;
    public abst_Plr_action last_used;

    private player Plr;
    private List<abst_enemy> combat_opponents;
    private List<abst_enemy> wondering_opponents;
    private abst_enemy selected_enemy;

    public abst_event cur_event { get; set; }

    private List<object> rewards;

    public TextMeshProUGUI p;
    public TextMeshProUGUI e;

    #region preparation
    public void init()  //GameManager's init() means 'entire reset of the total game'
    {
        left_of_range = 5;
        if (order_list == null) { order_list = new Queue<abst_action>(); } else { order_list.Clear(); }
        last_used = null;
        if (combat_opponents == null) { combat_opponents = new List<abst_enemy>(); } else { combat_opponents.Clear(); }
        if (wondering_opponents == null) { wondering_opponents = new List<abst_enemy>(); } else { wondering_opponents.Clear(); }
        if (rewards == null) { rewards = new List<object>(); } else { rewards.Clear(); }
        //GraphicManager.g.init();
        gra.temp_BFS();
        GraphicManager.g.edge_placement();
        selected_node = null;
        //�̹� ������ �渷 ����
        //���� ��ġ
        //�� ��ġ
        //����� ��ġ
        Plr.init();
        StartCoroutine(adventure());
    }
    #endregion preparation

    #region general
    #endregion general

    #region reward
    //�ھ����� general �ӿ� ������� ��
    private void reward_shard(int i) {
        rewards.Add(i);
    }
    private void reward_action() {
        rewards.Add(LibraryManager.li.return_action());
    }
    private void reward_action(abst_Plr_action a) {
        rewards.Add(a);
        //Plr.actions.Add((abst_Plr_action)rewards[0]); ��use it
    }
    private void reward_tool() {
        rewards.Add(LibraryManager.li.return_tool());
    }
    private void reward_tool(abst_tool t) {
        rewards.Add(LibraryManager.li.return_tool());
    }
    #endregion reward

    #region adventure
    IEnumerator adventure() {
        yield return StartCoroutine(departure_select());
        node temp;
        while (true) {
            //�ڰ��� ���� ���� Ȯ��
            if (is_Plr_turn) {
                //���÷��̾� ü�� ȸ��
                if (is_combat) {
                    foreach (abst_enemy e in this.combat_opponents) {
                        e.action_choice();
                    }
                    StartCoroutine(combat_unit());
                } else {
                    temp = selected_node;
                    yield return StartCoroutine(move_select()); //move
                    Plr.move_to(selected_node);

                    if (selected_node.is_enemy_here()) {
                        //this section only initiates settings of combat, real combat is preformed in 'if' body above
                        this.is_Plr_turn = true;
                        combat_opponents = selected_node.get_enemies_here();
                        selected_enemy = combat_opponents[0];
                    }
                    if (selected_node.event_here != null) {
                        cur_event = selected_node.event_here;
                        StartCoroutine(cur_event.happen());
                    }
                }
            } else {
                if (is_combat) {
                    foreach (abst_enemy e in this.combat_opponents) {
                        e.act();
                    }
                    StartCoroutine(combat_unit());
                } else {
                    //��wadering opponents�� �ִ� abst_enemy���� �̵���Ű��
                }
            }
        }
    }

    IEnumerator node_select(string notice) {
        node temp = this.selected_node;
        //�ڳ�带 �����϶�� �ȳ� ȭ��
        //�ڸ��� ��� ������ ��Ȳ�� ���, ����� �� �ֵ��� �Ʒ� WaitWhile�� ���� �߰�
        yield return new WaitWhile(() => temp == this.selected_node);
    }

    IEnumerator departure_select() {
        //��GraphicManager�� ��ư�� Ȱ��ȭ��Ű�� �Լ��� �����, �� ���� ������ ���ü� Ȯ��
        map[0, 0].be_interactive();
        map[10, 0].be_interactive();
        map[0, 10].be_interactive();
        map[10, 10].be_interactive();
        yield return StartCoroutine(node_select("testing"));
        Plr.set_location(selected_node);
        map[0, 0].de_interactive();
        map[10, 0].de_interactive();
        map[0, 10].de_interactive();
        map[10, 10].de_interactive();
    }

    IEnumerator move_select() {
        Plr.get_location().be_interactive();
        foreach (node n in Plr.get_location().get_link()) {
            if (n != null) { n.be_interactive(); }
        }
        yield return StartCoroutine(node_select("testing"));
        Plr.get_location().de_interactive();
        foreach (node n in Plr.get_location().get_link()) {
            if (n != null) { n.de_interactive(); } 
        }
    }
    #endregion adventure

    #region combat
    IEnumerator combat_unit() {
        GraphicManager.g.temp_combat_recover(); //��
        bool whose_turn_when_turen_started = is_Plr_turn;

        while (is_Plr_turn == whose_turn_when_turen_started | order_list.Count > 0) {
                //�ڰ��� ���� ����
                //��if !is_combat : yield break
                p.text = Plr.get_cur_hp().ToString();               //��graphicmanager���� �ű� �� ����
                e.text = selected_enemy.get_cur_hp().ToString();    //��graphicmanager���� �ű� �� ����
                if (order_list.Count > 0) {
                    order_list.Dequeue().use();
                }
                yield return new WaitForSeconds(0.05f);
            }
    }

    #region combat_process
    public void attack(thing giver, thing receiver, int value) {
        foreach (abst_power a in giver.powers) { value = a.on_before_attack(receiver, value); }
        //�ڹ� ó��
        hp_change(receiver, -value);
        foreach (abst_power a in giver.powers) { value = a.on_after_attack(value); }
    }
    public void hp_change(thing receiver, int value) {
        if (value < 0) { foreach (abst_power a in receiver.powers) { value = a.on_before_hp_down(value); } } else if (value > 0) { foreach (abst_power a in receiver.powers) { value = a.on_before_hp_up(value); } }
        receiver.set_cur_hp(true, value);
        if (value < 0) { foreach (abst_power a in receiver.powers) { value = a.on_after_hp_down(value); } } else if (value > 0) { foreach (abst_power a in receiver.powers) { value = a.on_after_hp_up(value); } }
    }
    public void haste(thing receiver) {
        //foreach (abst_power a in receiver.powers) { a.on_before_haste(); }
        if (last_used != null) { this.last_used.set_cur_cooltime(true, -1); } else { Debug.Log("thers's no last_used yet"); }//��
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    public void turn_end() {
        //��turn_end_button�� abst_enemy�� ender�� GameManager�� �ű�� �� Ŭ���������� �޼��� �ϳ��� ȣ���ϴ� ��ĵ� ����غ� ��
        //��if is_Plr_turn : on_Plr_turn_end, else on_enemy_turn_end
        is_Plr_turn = !is_Plr_turn;
        //��if is_Plr_turn : on_enemy_turn_start, else on_Plr_turn_start
    }
    public int ROLL() {
        //�ڱ׷��� ȿ�� �� ���� ó��
        int result = this.left_of_range + this.ran.xoshiro_range(5);
        if (result > 5) { this.left_of_range--; } else { this.left_of_range++; }
        return result;
    }
    private int combat_result() {
        //��is_combat �������� ���� ���� Ż��, ���� ����, ���� Ŭ����/���� �� ��� Coroutine ���� �� ó��
        int temp = 0;
        foreach (abst_enemy a in this.combat_opponents) { temp += a.get_cur_hp(); }

        //��Ŭ����, ���ӿ��� ���ο� ���� ���� �� ���� ���� ���� ó��
        if (this.Plr.get_cur_hp() <= 0) {
            Debug.Log("game over");
            return 2;   /*gameover*/
        } else if (temp <= 0) {
            Debug.Log("Plr win");
            return 1; /*Plr win*/
        } else { return 0;   /*combat not completed yet*/ }
    }
    #endregion combat_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; }
    public player get_Plr() { return Plr; }
    public abst_enemy get_selected_enemy() { return selected_enemy; }
    public Queue<abst_action> get_order_list() { return this.order_list; }
    public node[,] get_map() { return this.map; }
    public node get_selected_node() { return this.selected_node; }
    public void set_selected_node(node n) { this.selected_node = n; }
    #endregion get_set

    public void testing() { Debug.Log("this is GameManager"); }

    void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        ran = new xoshiro(); ran.seed();
        gra = new graph_generator();
        map = new node[11, 11];
        GraphicManager.g.initial_init();
        this.Plr = new player();

        this.Plr.actions.Add(new temp_action());    //��
        this.Plr.actions.Add(new temp_action2());
        this.Plr.actions.Add(new temp_action3());
        GraphicManager.g.combat_Plr_action_button_update();
    }

    private void Start()
    {
        this.init();
        new temp_enemy().move_to(map[1,1]);
        map[2, 2].event_here = new temp_event();
        //Debug.Log(LibraryManager.li.return_action().action_name_);
    }
}
