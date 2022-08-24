using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class GameManager : MonoBehaviour {
    private int left_of_range;

    public static GameManager g = null;

    public xoshiro ran;
    graph_generator gra;

    private node[,] map;
    private node selected_node;

    private bool is_Plr_turn;

    //public delegate void order_func(int i);
    public Queue<abst_action> order_list;
    public abst_Plr_action last_used;

    private player Plr;
    private List<abst_enemy> combat_opponents;
    private List<abst_enemy> wondering_opponents;
    private abst_enemy selected_enemy;

    #region preparation
    public void init()  //GameManager's init() means 'entire reset of the total game'
    {
        left_of_range = 5;
        if (order_list == null) { order_list = new Queue<abst_action>(); } else { order_list.Clear(); }
        last_used = null;
        if (combat_opponents == null) { combat_opponents = new List<abst_enemy>(); } else { combat_opponents.Clear(); }
        if (wondering_opponents == null) { wondering_opponents = new List<abst_enemy>(); } else { wondering_opponents.Clear(); }
        //GraphicManager.g.init();
        gra.temp_BFS();
        GraphicManager.g.edge_placement();
        selected_node = null;
        //이번 게임의 흑막 결정
        //정예 배치
        //적 배치
        //빛기둥 배치
        this.Plr.init();
        StartCoroutine(adventure());
    }
    #endregion preparation

    #region adventure
    IEnumerator adventure() {
        yield return StartCoroutine(departure_select());
        node temp;
        while (true) {
            temp = selected_node;
            yield return StartCoroutine(move_select()); //move
            Plr.move_to(selected_node);
        }
    }

    IEnumerator node_select(string notice) {
        node temp = this.selected_node;
        //★노드를 선택하라는 안내 화면
        //★만약 취소 가능한 상황일 경우, 취소할 수 있도록 아래 WaitWhile에 조건 추가
        yield return new WaitWhile(() => temp == this.selected_node);
    }

    IEnumerator departure_select() {
        //★GraphicManager에 버튼을 활성화시키는 함수를 만들고, 색 변경 등으로 가시성 확보
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
            if (n != null) { n.be_interactive(); Debug.Log("Plr near node activated"); } else { Debug.Log("it was null"); }
        }
        yield return StartCoroutine(node_select("testing"));
        Plr.get_location().de_interactive();
        foreach (node n in Plr.get_location().get_link()) {
            if (n != null) { n.de_interactive(); } 
        }
    }
    #endregion adventure

    #region combat
    IEnumerator combat_process() {
        int temp_combat_result = 0;

        this.is_Plr_turn = true;
        while (true) {
            if (is_Plr_turn) {
                foreach (abst_enemy e in this.combat_opponents) {
                    e.action_choice();
                }
            } else {
                foreach (abst_enemy e in this.combat_opponents) {
                    e.act();
                }
            }
            while (this.is_Plr_turn | order_list.Count > 0) {
                Debug.Log("cur Plr hp : " + Convert.ToString(this.Plr.get_cur_hp()));
                Debug.Log("cur enemy hp : " + Convert.ToString(this.combat_opponents[0].get_cur_hp()));
                if (order_list.Count > 0) {
                    order_list.Dequeue().use();
                    temp_combat_result = this.combat_result();
                }
                yield return new WaitForSeconds(0.05f);
            }
            if (temp_combat_result != 0) { break; }
        }
    }

    #region variant_process
    public void attack(thing giver, thing receiver, int value) {
        foreach (abst_power a in giver.powers) { value = a.on_before_attack(receiver, value); }
        //★방어도 처리
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
        if (last_used != null) { this.last_used.set_cur_cooltime(true, -1); } else { Debug.Log("thers's no last_used yet"); }//★
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    public void turn_end() {
        //★turn_end_button과 abst_enemy의 ender를 GameManager로 옮기고 각 클래스에서는 메서드 하나만 호출하는 방식도 고민해볼 것
        //★if is_Plr_turn : on_Plr_turn_end, else on_enemy_turn_end
        Debug.Log("we are in turn ending");
        is_Plr_turn = !is_Plr_turn;
        //★if is_Plr_turn : on_enemy_turn_start, else on_Plr_turn_start
    }
    public int ROLL() {
        //★그래픽 효과 및 지연 처리
        int result = this.left_of_range + this.ran.xoshiro_range(5);
        if (result > 5) { this.left_of_range--; } else { this.left_of_range++; }
        return result;
    }
    private int combat_result() {
        int temp = 0;
        foreach (abst_enemy a in this.combat_opponents) { temp += a.get_cur_hp(); }

        //★클리어, 게임오버 여부에 따른 보상 및 게임 정지 등의 처리
        if (this.Plr.get_cur_hp() <= 0) {
            Debug.Log("game over");
            return 2;   /*gameover*/
        } else if (temp <= 0) {
            Debug.Log("Plr win");
            return 1; /*Plr win*/
        } else { return 0;   /*combat not completed yet*/ }
    }
    #endregion variant_process
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

        this.Plr.actions.Add(new temp_action());    //★
        this.Plr.actions.Add(new temp_action());
        this.Plr.actions.Add(new temp_action());
        GraphicManager.g.combat_Plr_action_button_update();
    }

    private void Start()
    {
        this.init();
        this.combat_opponents.Add(new temp_enemy());
        this.selected_enemy = this.combat_opponents[0];
        StartCoroutine(combat_process());
    }
}
