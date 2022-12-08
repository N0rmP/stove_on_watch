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

    public xoshiro ran;
    public graph_generator gra;

    private int left_of_range;
    private int[] range_limit = new int[2];
    private bool is_combat;
    private bool is_prime_combat;
    private bool is_Plr_turn;
    private abst_power[] temp_powers;

    private bool is_first_stage;
    private node[,] map;
    //0~3 : up/right/down/left, 4 : diagnol direction nodes (clockwise from right upper one)
    private List<node> Plr_POV;
    private node selected_node;
    private bool elite_defeated;
    private node[] center_nodes;
    private node[] elite_nodes;

    //public delegate void order_func(int i);
    public Queue<abst_action> order_list;
    public abst_Plr_action last_used;
    public turn_end TE;

    private player Plr;
    private CombatEnemyList combat_enemies;
    private List<abst_enemy> wandering_enemies;

    public abst_event cur_event { get; set; }

    public IEnumerator delay(float time) {
        yield return new WaitForSeconds(time);
    }

    #region preparation
    public void game_init()  //GameManager's init() means 'entire reset of the total game'
    {
        is_first_stage = true;
        LibraryManager.li.init();
        left_of_range = 3;
        range_limit[0] = 0; range_limit[1] = 6;
        last_used = null;
        //GraphicManager.g.init();
        elite_defeated = false;
        //이번 게임의 흑막 결정
        //정예 배치
        //적 배치
        //빛기둥 배치
        Plr.init();
        stage_init();
        StartCoroutine(adventure());
    }

    public void stage_init() {
        rew.init();
        if (order_list == null) { order_list = new Queue<abst_action>(); } else { order_list.Clear(); }
        foreach (node n in map)
            n.init();
        gra.temp_BFS();
        GraphicManager.g.edge_placement();
        if (combat_enemies == null) { combat_enemies = new CombatEnemyList(); } else { combat_enemies.clear(); }
        if (wandering_enemies == null) { wandering_enemies = new List<abst_enemy>(); } else { wandering_enemies.Clear(); }
        selected_node = null;
        Plr_POV = new List<node>();
        GraphicManager.g.glowings_clear();

        //elite or root placement
        if (is_first_stage) {
            if (gra.elite_pos != 0) wandering_enemies.Add(LibraryManager.li.return_elit_enemy(4, 4));
            if (gra.elite_pos != 1) wandering_enemies.Add(LibraryManager.li.return_elit_enemy(4, 6));
            if (gra.elite_pos != 2) wandering_enemies.Add(LibraryManager.li.return_elit_enemy(6, 4));
            if (gra.elite_pos != 3) wandering_enemies.Add(LibraryManager.li.return_elit_enemy(6, 6));
        }
        else
            wandering_enemies.Add(new mother(5, 5));

        //enemy placement
        while (wandering_enemies.Count < 8) {
            place_enemy();
        }

        //event placement
        place_event();
    }

    public void second_stage() {
        //map, enemy list 전체, node의 things들 (★그 외 기타 등등) 초기화
        is_first_stage = false;
        stage_init();
        StartCoroutine(adventure());
    }

    public void place_enemy() {
        int enem_num_when_enter = wandering_enemies.Count;
        int temp_ran;
        bool Plr_far;
        while (wandering_enemies.Count <= enem_num_when_enter) {
            temp_ran = ran.xoshiro_range(121);
            if (Plr.get_location() == null)
                Plr_far = true;
            else
                Plr_far = (Plr.get_location().get_coor()[0] * 11 + Plr.get_location().get_coor()[1] - temp_ran > 1);
            if (gra.is_placable(temp_ran / 11, temp_ran % 11) && Plr_far)
                wandering_enemies.Add(LibraryManager.li.return_enemy(temp_ran / 11, temp_ran % 11));
        }
    }

    public void place_event() {
        int event_num = 0;
        int temp_ran;
        bool Plr_far;
        while (event_num < 9) {
            temp_ran = ran.xoshiro_range(121);
            if (Plr.get_location() == null)
                Plr_far = true;
            else
                Plr_far = (Plr.get_location().get_coor()[0] * 11 + Plr.get_location().get_coor()[1] - temp_ran > 1);

            if (gra.is_placable(temp_ran / 11, temp_ran % 11) && Plr_far) {
                if (event_num > 5)
                    map[temp_ran / 11, temp_ran % 11].event_here_ = new shelter();
                else
                    map[temp_ran / 11, temp_ran % 11].event_here_ = LibraryManager.li.return_event();
                event_num++;
            }
        }
    }
    #endregion preparation

    #region general
    public void over_check() {
        if (Plr.get_cur_hp() <= 0) {
            StopAllCoroutines();
            GraphicManager.g.over();
        }
    }

    public void stop_coroutine() {
        StopAllCoroutines();
    }
    #endregion general

    #region adventure
    private IEnumerator adventure() {
        yield return StartCoroutine(departure_select());
        List<abst_enemy> temp_ae;
        node temp_n;
        is_Plr_turn = true;
        is_combat = false;
        is_prime_combat = false;
        while (true) {
            over_check();
            if (is_Plr_turn) {
                //Plr's turn start
                Plr.set_block(true, -Plr.get_block() * 4 / 5);
                Plr_POV_update();
                hp_change(Plr, 1);
                foreach (abst_Plr_action a in Plr.actions_)
                    a.set_cur_cooltime(-1, true);
                temp_powers = Plr.get_powers().ToArray();
                foreach (abst_power ap in temp_powers)
                    ap.on_Plr_turn_start();
                foreach (abst_enemy ae in combat_enemies.get_list()) {
                    temp_powers = ae.get_powers().ToArray();
                    foreach (abst_power ap in temp_powers)
                        ap.on_Plr_turn_start();
                }

                if (is_combat) {
                    //while combat
                    foreach (abst_enemy e in combat_enemies.get_list()) {
                        e.action_choice();
                    }
                    while(is_Plr_turn && is_combat)
                        yield return StartCoroutine(combat_unit());
                } else {
                    //while map_moving
                    temp_n = selected_node;
                    //select where to move
                    yield return StartCoroutine(move_select());
                    if (selected_node != null) {
                        Plr.move_to(selected_node);
                        //if player enters center nodes at first stage, quit coroutine and move to next stage
                        if (is_first_stage) {
                            foreach (node n in center_nodes)
                                if (n == Plr.get_location()) {
                                    second_stage();
                                    yield break;
                                }
                        }
                        //check enemy here and initiate combat
                        temp_ae = Plr.get_location().get_enemies_here();
                        if (!is_combat && Plr.get_location().is_enemy_here())
                            general_combat_start(temp_ae);
                        //event
                        else if (selected_node.event_here_ != null) {
                            cur_event = selected_node.event_here_;
                            yield return StartCoroutine(cur_event.happen());
                        }
                    }
                    turn_end();
                }
            } else {
                foreach (abst_enemy enem in wandering_enemies) {
                    enem.set_block(true, -enem.get_block() * 4 / 5);
                    temp_powers = enem.get_powers().ToArray();
                    foreach (abst_power ap in temp_powers)
                        ap.on_enemy_turn_start();
                    enem.act();
                    temp_powers = enem.get_powers().ToArray();
                    foreach (abst_power ap in temp_powers)
                        ap.on_enemy_turn_end();
                }
                if (is_combat)
                    StartCoroutine(combat_unit());
                turn_end();
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
        GraphicManager.g.set_image_color(map[0, 0].gameObject, new Color(1f, 1f, 1f, 1f));
        GraphicManager.g.set_image_color(map[10, 0].gameObject, new Color(1f, 1f, 1f, 1f));
        GraphicManager.g.set_image_color(map[0, 10].gameObject, new Color(1f, 1f, 1f, 1f));
        GraphicManager.g.set_image_color(map[10, 10].gameObject, new Color(1f, 1f, 1f, 1f));
        map[0, 0].be_interactive();
        map[10, 0].be_interactive();
        map[0, 10].be_interactive();
        map[10, 10].be_interactive();
        yield return StartCoroutine(node_select("testing"));
        Plr.move_to(selected_node);
        map[0, 0].de_interactive();
        map[10, 0].de_interactive();
        map[0, 10].de_interactive();
        map[10, 10].de_interactive();
        //Plr_POV_update();
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
                        GraphicManager.g.set_image_color(m.gameObject, new Color(0.5f, 0.5f, 0.5f, 1f));
                        GraphicManager.g.glowings_add(m.gameObject);
                        m.be_interactive();
                    }
                    break;
                }
            }
            if (!plr_near_center)
                foreach (node m in center_nodes) {
                    m.de_interactive();
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
        List<node> post_POV = Plr_POV;
        Plr_POV = new List<node>();
        node cur = Plr.get_location();
        bool[] map_bound = new bool[4] {
            cur.get_coor()[1] > 0,
            cur.get_coor()[0] > 10,
            cur.get_coor()[1] < 10,
            cur.get_coor()[0] > 0
        };

        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                try {
                    Plr_POV.Add(map[cur.get_coor()[0] + i, cur.get_coor()[1] + j]);
                } catch (Exception e) { }
            }
        }

        int[] dir_arr = new int[4] { -1, 0, 1, 0 };
        int distance;
        for (int i = 0; i < 4; i++) {
            cur = Plr.get_location().get_link()[i];
            distance = 0;
            
            while (cur != null && distance < 4) {
                if (distance != 0)
                    Plr_POV.Add(cur);
                cur = cur.get_link()[i];                
                distance++;                
            }
        }

        foreach (node n in post_POV)
            if (n != null)
                n.POV_process(false);
        foreach (node n in Plr_POV)
            if (n != null)
                n.POV_process(true);
    }
    #endregion adventure

    #region combat
    public void general_combat_start(List<abst_enemy> temp_ae = null) {
        is_Plr_turn = false;    //right after this, turn end makes it Plr's turn
        is_combat = true;

        if (temp_ae != null)
            foreach (abst_enemy ae in temp_ae) {
                ae.engage();
                if ((ae.enemy_tier_ == abst_enemy.enemy_tiers.elite) ||
                        (ae.enemy_tier_ == abst_enemy.enemy_tiers.root))
                    is_prime_combat = true;
            }

        temp_powers = Plr.passives_.ToArray();
        foreach (abst_power ap in temp_powers)
            ap.on_combat_start();

        GraphicManager.g.combat_Plr_action_button_update();
        GraphicManager.g.combat_recover();
    }

    IEnumerator combat_unit() {
        bool whose_turn_when_turen_started = is_Plr_turn;

        while ((is_Plr_turn == whose_turn_when_turen_started | order_list.Count > 0) & is_combat) {
            GraphicManager.g.combat_Plr_action_button_update();
            combat_result();
            if (order_list.Count > 0) {
                order_list.Dequeue().use();
                GraphicManager.g.combat_Plr_action_button_update();
                yield return new WaitForSeconds(0.3f);
            }else
                yield return new WaitForSeconds(0.1f);
        }
    }

    #region combat_process
    public void attack(thing giver, thing receiver, int value) {
        foreach (abst_power a in giver.get_powers()) { a.on_before_attack(receiver, ref value); }

        int temp_block = receiver.get_block();
        if (temp_block > 0) {
            if (temp_block < value) {
                value -= temp_block;
                receiver.set_block(true, -temp_block);
            } else {
                receiver.set_block(true, -value);
                value = 0;
            }
        }
        hp_change(receiver, -value);
        if(value > 0)
            GraphicManager.g.damaged_effect(receiver);

        foreach (abst_power a in giver.get_powers()) { a.on_after_attack(value); }
    }
    public void block(thing target, int value) {
        temp_powers = target.get_powers().ToArray();
        foreach (abst_power a in temp_powers) 
            a.on_before_block(ref value);

        target.set_block(true, value);

        temp_powers = target.get_powers().ToArray();
        foreach (abst_power a in temp_powers) 
            a.on_after_block(value);
    }
    public void hp_change(thing receiver, int value) {
        if (value < 0) {
            temp_powers = receiver.get_powers().ToArray();
            foreach (abst_power a in temp_powers)
                a.on_before_hp_down(ref value);
        } else if (value > 0) {
            temp_powers = receiver.get_powers().ToArray();
            foreach (abst_power a in temp_powers)
                a.on_before_hp_up(ref value);
        }

        int temp_res = receiver.get_cur_hp() + value;
        if (temp_res > receiver.get_max_hp()) 
            receiver.set_cur_hp(false, receiver.get_max_hp());
        else if (temp_res < 0)
            receiver.set_cur_hp(false, 0);
        else
            receiver.set_cur_hp(true, value);

        if (value < 0) {
            temp_powers = receiver.get_powers().ToArray();
            foreach (abst_power a in temp_powers)
                a.on_after_hp_down(ref value);
        } else if (value > 0) {
            temp_powers = receiver.get_powers().ToArray();
            foreach (abst_power a in temp_powers)
                a.on_after_hp_up(ref value);
        }
    }
    public void haste(thing receiver) {
        //foreach (abst_power a in receiver.powers) { a.on_before_haste(); }
        if (last_used != null) { this.last_used.set_cur_cooltime(-1, true); } else { Debug.Log("thers's no last_used yet"); }//★
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    public void turn_end() {
        if (is_Plr_turn) {
            temp_powers = Plr.get_powers().ToArray();
            foreach (abst_power ap in temp_powers)
                ap.on_Plr_turn_end();
        }
        is_Plr_turn = !is_Plr_turn;
    }
    public int ROLL(int predetermined_value = -1, int max_or_min = 0, int crease_set = -1) {
        temp_powers = Plr.get_powers().ToArray();
        foreach (abst_power ap in temp_powers)
            ap.on_before_ROLL(ref predetermined_value, ref max_or_min, ref crease_set);
        foreach (abst_enemy ae in combat_enemies.get_list()) {
            temp_powers = ae.get_powers().ToArray();
            foreach (abst_power ap in temp_powers)
                ap.on_before_ROLL(ref predetermined_value, ref max_or_min, ref crease_set);
        }

        int result;
        if (predetermined_value != -1)
            result = predetermined_value;
        else if (max_or_min == 0)
            result = this.left_of_range + this.ran.xoshiro_range(5);
        else if (max_or_min < 0)
            result = left_of_range;
        else
            result = left_of_range + 4;

        //Debug.Log("R value : " + result);
        if ((crease_set > 0) || (result < 6)) {
            this.left_of_range++;
            GraphicManager.g.range_increase();
        } else if (crease_set < 0) {
            this.left_of_range--;
            GraphicManager.g.range_decrease();
        }

        temp_powers = Plr.get_powers().ToArray();
        foreach (abst_power ap in temp_powers)
            ap.on_after_ROLL(ref result);
        foreach (abst_enemy ae in combat_enemies.get_list()) {
            temp_powers = ae.get_powers().ToArray();
            foreach (abst_power ap in temp_powers)
                ap.on_after_ROLL(ref result);
        }
        //★그래픽 효과 및 지연 처리
        return result;
    }
    private void combat_result() {
        //game over check
        over_check();

        //enemy remaining check
        bool enemy_not_remain = true;
        foreach (abst_enemy a in combat_enemies.get_list()) {
            if (a.get_cur_hp()>0) {
                enemy_not_remain = false;
                break;
            }
        }

        if (enemy_not_remain) {
            //★전투 중인 적이 근원이라면 게임 클리어 처리
            //★따돌림 기능 구현 시 구조 개편 필요
            temp_powers = Plr.get_powers().ToArray();
            foreach (abst_power ap in temp_powers)
                ap.on_combat_end();
            foreach (abst_enemy ae in combat_enemies.get_list()) {
                temp_powers = ae.get_powers().ToArray();
                foreach (abst_power ap in temp_powers)
                    ap.on_combat_end();
            }
            Plr.get_powers().Clear();

            rew.init();
            foreach (abst_enemy a in combat_enemies.get_list()) { 
                a.give_reward();
                if (a.enemy_tier_ == abst_enemy.enemy_tiers.root)
                    return;
                a.disappear();
            }
            combat_enemies.clear();
            is_combat = false;
            GraphicManager.g.combat_remove();
            GraphicManager.g.reward_init();
        }
    }
    #endregion combat_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; }
    public bool get_is_first_stage() { return is_first_stage; }
    public void set_is_first_stage(bool b) { is_first_stage = b; }
    public CombatEnemyList get_combat_enemies() { return combat_enemies; }
    public List<abst_enemy> get_wandering_enemies() { return wandering_enemies; }
    public bool get_is_combat() { return is_combat; }
    public void set_is_combat(bool b) { is_combat = b; }
    public bool get_is_prime_combat() { return is_prime_combat; }
    public void set_is_prime_combat(bool b) { is_prime_combat = b; }
    public player get_Plr() { return Plr; }
    public abst_enemy get_selected_enemy() { return combat_enemies.get_selected(); }
    //public void set_selected_enemy(abst_enemy ae) { selected_enemy = ae; }
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

    public void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        //DontDestroyOnLoad(this.gameObject);
        ran = new xoshiro(); ran.seed();
        gra = new graph_generator();
        map = new node[11, 11];
        
    }

    public void Start()
    {
        GraphicManager.g.initial_init();
        Plr = new player();
        rew = new RewardList();
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

        game_init();

        this.Plr.actions.Add(new attack_player());    //★
        this.Plr.actions.Add(new block_player());
        this.Plr.actions.Add(new bomb());
        this.Plr.actions.Add(new bomb());
        this.Plr.actions.Add(new medicine());
    }

    public void testing() { Debug.Log("this is GameManager"); }
}
