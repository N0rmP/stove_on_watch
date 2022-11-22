using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager g = null;
    public RewardList rew;

    private int left_of_range;
    private int[] range_limit = new int[2];
    bool is_combat;
    private bool is_Plr_turn;

    public xoshiro ran;
    graph_generator gra;

    private bool is_first_stage;
    private node[,] map;
    //0~3 : up/right/down/left, 4 : diagnol direction nodes (clockwise from right upper one)
    private node[,] Plr_POV;
    private node selected_node;
    private bool elite_defeated;
    private node[] center_nodes;
    private node[] elite_nodes;

    //public delegate void order_func(int i);
    public Queue<abst_action> order_list;
    public abst_Plr_action last_used;
    public turn_end TE;

    private player Plr;
    private List<abst_enemy> combat_enemies;
    private List<abst_enemy> wandering_enemies;
    private abst_enemy selected_enemy;

    public abst_event cur_event { get; set; }

    public TextMeshProUGUI p;
    public TextMeshProUGUI e;

    abst_enemy tttemp;

    #region preparation
    public void init()  //GameManager's init() means 'entire reset of the total game'
    {
        is_first_stage = true;
        LibraryManager.li.init();
        rew.init();
        left_of_range = 3;
        range_limit[0] = 0; range_limit[1] = 6;
        if (order_list == null) { order_list = new Queue<abst_action>(); } else { order_list.Clear(); }
        last_used = null;
        if (combat_enemies == null) { combat_enemies = new List<abst_enemy>(); } else { combat_enemies.Clear(); }
        if (wandering_enemies == null) { wandering_enemies = new List<abst_enemy>(); } else { wandering_enemies.Clear(); }
        //GraphicManager.g.init();
        gra.temp_BFS();
        if (Plr_POV == null) { Plr_POV = new node[5,4]; }
        GraphicManager.g.edge_placement();
        selected_node = null;
        elite_defeated = false;
        //이번 게임의 흑막 결정
        //정예 배치
        //적 배치
        //빛기둥 배치
        Plr.init();
        StartCoroutine(adventure());
    }
    #endregion preparation

    #region adventure
    private IEnumerator adventure() {
        yield return StartCoroutine(departure_select());
        List<abst_enemy> temp_ae;
        node temp_n;
        is_Plr_turn = true;
        is_combat = false;
        while (true) {
            try {
                p.text = Plr.get_cur_hp().ToString();               //★graphicmanager에게 옮긴 뒤 삭제
                e.text = selected_enemy.get_cur_hp().ToString();
            } catch (Exception e) { Debug.Log("temp player/enemy hp description error"); }
            if (Plr.get_cur_hp() <= 0) { 
                //★게임 오버 처리
            }
            if (is_Plr_turn) {
                if (is_combat) {
                    foreach (abst_enemy e in this.combat_enemies) {
                        e.action_choice();
                    }
                    yield return StartCoroutine(combat_unit());
                } else {
                    temp_n = selected_node;
                    //select where to move
                    yield return StartCoroutine(move_select());
                    if (selected_node != null) {
                        foreach (node n in center_nodes) {
                            if (n == Plr.get_location()) { 
                                //★Coroutine 종료, 2단계 진입 준비
                                //map, enemy list 전체, node의 things들 (★그 외 기타 등등) 초기화
                                //is_first_stage = false;
                            }
                        }
                        Plr.move_to(selected_node);
                        if (selected_node.event_here != null) {
                            cur_event = selected_node.event_here;
                            StartCoroutine(cur_event.happen());
                        }
                    }

                    turn_end();
                }
            } else {
                //★각 enemy의 방어도 80% 소멸
                if (is_combat) {
                    foreach (abst_enemy enem in this.combat_enemies)
                        enem.act();
                    StartCoroutine(combat_unit());
                }
                foreach (abst_enemy enem in wandering_enemies)
                    enem.map_move();
                turn_end();
                //effects activating on Plr's turn start actually activates here
                //★플레이어 방어도의 80% 소멸
                Plr_POV_update();
                hp_change(Plr, 1);
                foreach (abst_Plr_action a in Plr.actions_)
                    a.set_cur_cooltime(-1, true);
            }

            //initiating combat, actual combat continues in 'if' body in Plr turn
            temp_ae = Plr.get_location().get_enemies_here();

            if (!is_combat && Plr.get_location().is_enemy_here()) {
                is_Plr_turn = true;
                //★적이 여러 명 겹쳤을 때에 combat_board를 정렬할 방법 생각하기
                foreach (abst_enemy ae in temp_ae) {
                    ae.engage();
                    combat_enemies.Add(ae);
                }
                selected_enemy = combat_enemies[0];
                GraphicManager.g.temp_combat_recover();
                is_combat = true;
            }
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
        node temp_plr_loc = Plr.get_location();
        temp_plr_loc.be_interactive();
        foreach (node n in temp_plr_loc.get_link()) {
            if (n != null) { n.be_interactive(); }
        }

        bool plr_near_center = false;
        if (elite_defeated) {            
            foreach (node n in elite_nodes) {
                if (n == temp_plr_loc) {
                    plr_near_center = true;
                    foreach (node m in center_nodes) {
                        m.be_interactive();
                        GraphicManager.g.glowings_add(m.gameObject);
                    }
                    break;
                }
            }
            if (!plr_near_center)
                foreach (node m in center_nodes) {
                    m.de_interactive();
                    GraphicManager.g.glowings_remove(m.gameObject);
                }
        }

        yield return StartCoroutine(node_select("testing"));
        Plr.get_location().de_interactive();
        foreach (node n in Plr.get_location().get_link()) {
            if (n != null) { n.de_interactive(); } 
        }
        if(elite_defeated && plr_near_center)
            foreach (node m in center_nodes)
                m.de_interactive();
    }

    private void Plr_POV_update() {
        node[,] post_POV = Plr_POV;
        Plr_POV = new node[5, 4];
        node cur = Plr.get_location();
        bool[] map_bound = new bool[4] {
            cur.get_coor()[1] > 0,
            cur.get_coor()[0] > 10,
            cur.get_coor()[1] < 10,
            cur.get_coor()[0] > 0
        };
        int[] dir_arr = new int[4] { -1, 0, 1, 0 };
        int distance;
        for (int i = 0; i < 4; i++) {
            cur = Plr.get_location().get_link()[i];
            //if (!map_bound[i] && cur == null) Plr_POV[i, 0] = map[Plr.get_location().get_coor()[0] + dir_arr[3-i], Plr.get_location().get_coor()[1] + dir_arr[i]];
            distance = 0;
            while (cur != null && distance < 4) {
                Plr_POV[i, distance] = cur;
                cur = cur.get_link()[i];
                distance++;
            }
        }
        cur = Plr.get_location();
        if (map_bound[0] && map_bound[1]) Plr_POV[4, 0] = map[cur.get_coor()[0] + 1, cur.get_coor()[1] - 1];
        if (map_bound[2] && map_bound[1]) Plr_POV[4, 1] = map[cur.get_coor()[0] + 1, cur.get_coor()[1] + 1];
        if (map_bound[2] && map_bound[3]) Plr_POV[4, 2] = map[cur.get_coor()[0] - 1, cur.get_coor()[1] + 1];
        if (map_bound[0] && map_bound[3]) Plr_POV[4, 3] = map[cur.get_coor()[0] - 1, cur.get_coor()[1] - 1];
        foreach (node n in post_POV)
            if (n != null)
                n.POV_process(false);
        foreach (node n in Plr_POV)
            if (n != null)
                n.POV_process(true);
    }
    #endregion adventure

    #region combat
    IEnumerator combat_unit() {
        bool whose_turn_when_turen_started = is_Plr_turn;

        while ((is_Plr_turn == whose_turn_when_turen_started | order_list.Count > 0) & is_combat) {
            GraphicManager.g.combat_Plr_action_button_update();
            combat_result();
            if (order_list.Count > 0) {
                order_list.Dequeue().use();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    #region combat_process
    public void attack(thing giver, thing receiver, int value) {
        foreach (abst_power a in giver.powers) { value = a.on_before_attack(receiver, value); }
        //★방어도 처리
        hp_change(receiver, -value);
        foreach (abst_power a in giver.powers) { value = a.on_after_attack(value); }
    }
    public void block(thing target, int value) {
        //★foreach (abst_power a in giver.powers) { value = a.on_before_block(); }
        target.set_block(true, value);
        //★foreach (abst_power a in giver.powers) { value = a.on_after_block(); }
    }
    public void hp_change(thing receiver, int value) {
        if (value < 0)
            foreach (abst_power a in receiver.powers) { value = a.on_before_hp_down(value); }
        else if (value > 0)
            foreach (abst_power a in receiver.powers) { value = a.on_before_hp_up(value); }

        int temp_res = receiver.get_cur_hp() + value;
        if (temp_res > receiver.get_max_hp()) 
            receiver.set_cur_hp(false, receiver.get_max_hp());
        else if (temp_res < 0)
            receiver.set_cur_hp(false, 0);
        else
            receiver.set_cur_hp(true, value);
        
        if (value < 0) 
            foreach (abst_power a in receiver.powers) { value = a.on_after_hp_down(value); } 
        else if (value > 0) 
            foreach (abst_power a in receiver.powers) { value = a.on_after_hp_up(value); }
    }
    public void haste(thing receiver) {
        //foreach (abst_power a in receiver.powers) { a.on_before_haste(); }
        if (last_used != null) { this.last_used.set_cur_cooltime(-1, true); } else { Debug.Log("thers's no last_used yet"); }//★
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    public void turn_end() {
        //★turn_end_button과 abst_enemy의 ender를 GameManager로 옮기고 각 클래스에서는 메서드 하나만 호출하는 방식도 고민해볼 것
        //★if is_Plr_turn : on_Plr_turn_end, else on_enemy_turn_end
        is_Plr_turn = !is_Plr_turn;
        //★if is_Plr_turn : on_enemy_turn_start, else on_Plr_turn_start
    }
    public int ROLL(int predetermined_value = -1, int max_or_min = 0, bool cannot_decrease = false) {
        int result;
        if (predetermined_value != -1)
            result = predetermined_value;
        else if (max_or_min == 0)
            result = this.left_of_range + this.ran.xoshiro_range(5);
        else if (max_or_min < 0)
            result = left_of_range;
        else
            result = left_of_range + 4;
            

        if (result > 5)
            if(!cannot_decrease)
                this.left_of_range--;
        else 
            this.left_of_range++;
        //★그래픽 효과 및 지연 처리
        return result;
    }
    private void combat_result() {
        bool enemy_not_remain = true;
        foreach (abst_enemy a in this.combat_enemies) {
            if (a.get_cur_hp()>0) {
                enemy_not_remain = false;
                break;
            }
                    
        }

        if (enemy_not_remain) {
            //★전투 중인 적이 근원이라면 게임 클리어 처리
            //★따돌림 기능 구현 시 구조 개편 필요
            foreach (abst_enemy a in this.combat_enemies) { 
                a.give_reward();
                a.disappear();
            }
            combat_enemies.Clear();
            selected_enemy = null;
            GraphicManager.g.temp_combat_remove();
            is_combat = false;
            GraphicManager.g.reward_init();
        }
        if (Plr.get_cur_hp() <= 0) {
            Debug.Log("Plr lost");
            is_combat = false; 
        }
    }
    #endregion combat_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; }
    public bool get_is_combat() { return is_combat; }
    public player get_Plr() { return Plr; }
    public abst_enemy get_selected_enemy() { return selected_enemy; }
    public Queue<abst_action> get_order_list() { return this.order_list; }
    public node[,] get_map() { return this.map; }
    public node get_selected_node() { return this.selected_node; }
    public void set_selected_node(node n) { this.selected_node = n; }

    public bool get_elite_defeated() { return elite_defeated; }
    public void set_elite_defeated(bool b) { elite_defeated = b; }
    public void remove_wandering_enemy(abst_enemy a){
        wandering_enemies.Remove(a);
    }

    public int get_left_of_range() {
        return left_of_range;
    }

    public void set_left_of_range(int val) {
        left_of_range += val;
        if (left_of_range < range_limit[0])
            left_of_range = range_limit[0];
        else if (left_of_range > range_limit[1])
            left_of_range = range_limit[1];
    }
    #endregion get_set

    public void testing() { Debug.Log("this is GameManager"); }

    public void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        rew = new RewardList();
        ran = new xoshiro(); ran.seed();
        gra = new graph_generator();
        map = new node[11, 11];
        GraphicManager.g.initial_init();
        this.Plr = new player();
        TE = new turn_end();
        center_nodes = new node[5];
        center_nodes[0] = map[5, 4];
        center_nodes[1] = map[4, 5];
        center_nodes[2] = map[5, 5];
        center_nodes[3] = map[6, 5];
        center_nodes[4] = map[5, 6];
        elite_nodes = new node[4];
        elite_nodes[0] = map[4, 4];
        elite_nodes[1] = map[4, 6];
        elite_nodes[2] = map[6, 4];
        elite_nodes[3] = map[6, 6];
    }

    public void Start()
    {
        this.init();

        this.Plr.actions.Add(new temp_action());    //★
        this.Plr.actions.Add(new temp_action2());
        this.Plr.actions.Add(new temp_action3());
        wandering_enemies.Add(new temp_enemy(1,2));
        wandering_enemies.Add(new temp_enemy(2, 4));
        map[2, 2].event_here = new temp_event();
        map[3, 3].event_here = new event_shelter();
        //Debug.Log(LibraryManager.li.return_action().action_name_);
    }
}
