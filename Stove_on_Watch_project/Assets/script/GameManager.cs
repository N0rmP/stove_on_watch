using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public static GameManager g = null;

    public xoshiro ran;
    graph_generator gra;

    private bool is_Plr_turn;

    //public delegate void order_func(int i);
    public Queue<abst_action> order_list;
    public abst_Plr_action last_used;

    private player Plr;
    private List<abst_enemy> combat_opponents;
    private List<abst_enemy> wondering_opponents;

    public GameObject temp_butt;

    #region preparation
    public void init()
    {
        if (order_list == null) { order_list = new Queue<abst_action>(); } else { order_list.Clear(); }
        last_used = null; 
        if (combat_opponents == null) { combat_opponents = new List<abst_enemy>(); } else { combat_opponents.Clear(); }
        if (wondering_opponents == null) { wondering_opponents = new List<abst_enemy>(); } else { wondering_opponents.Clear(); }
        //맵 생성
        //이번 게임의 흑막 결정
        //정예 배치
        //적 배치
        //빛기둥 배치
        this.Plr.init();
        //플레이어 시작 위치 선택?
    }
    #endregion preparation

    #region combat
    IEnumerator combat_process()
    {
        bool temp_Plr_turn = true;  //describes whose turn is it now during current acction, it can be different from it in actual turn
        abst_Plr_action temp_Plr_action;

        this.is_Plr_turn = true;
        while (true)
        {
            //★if is_Plr_turn : on_Plr_turn_start
            while (temp_Plr_turn == this.is_Plr_turn | order_list.Count > 0)
            {
                if (order_list.Count > 0)
                {
                    order_list.Dequeue().use();
                }
                this.testing();
                yield return new WaitForSeconds(0.05f);
            }
            break;  //★
            temp_Plr_turn = this.is_Plr_turn;
            //★if is_Plr_turn : on_Plr_turn_end
        }
        //return true;    //★추후 전투 승리/패배 여부 반환, 게임오버와 연결
    }

    #region variant_process
    public void attack(thing giver, thing receiver, int value) {
        foreach (abst_power a in giver.powers) { value = a.on_before_attack(receiver, value); }
        hp_change(receiver, -value);
        foreach (abst_power a in giver.powers) { value = a.on_after_attack(value); }
    }
    public void hp_change(thing receiver, int value) {
        if (value < 0) { foreach (abst_power a in receiver.powers) { value = a.on_before_hp_down(value); } }
        else if (value > 0) { foreach (abst_power a in receiver.powers) { value = a.on_before_hp_up(value); } }
        receiver.set_cur_hp(true, value);
        if (value < 0) { foreach (abst_power a in receiver.powers) { value = a.on_after_hp_down(value); } }
        else if (value > 0) { foreach (abst_power a in receiver.powers) { value = a.on_after_hp_up(value); } }
    }
    public void haste(thing receiver) {
        //foreach (abst_power a in receiver.powers) { a.on_before_haste(); }
        if (last_used != null) { this.last_used.set_cur_cooltime(true, -1); }
        else { Debug.Log("thers's no last_used yet"); }//★
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    #endregion variant_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; } //player calls it with button instantly to prevent click turn_end twice, enemy calls it in its function directly anyway (it's no problem)
    public player get_Plr() { return Plr; }

    public Queue<abst_action> get_order_list() { return this.order_list; }
    #endregion get_set

    public void testing() { Debug.Log("this is GameManager"); }

    void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        this.Plr = new player();
        this.Plr.actions.Add(new temp_action());    //★
        GameManager.g.temp_butt.GetComponent<Plr_action_button>().set_target(this.Plr.actions[0]);
        this.init();    //★
    }

    private void Start()
    {
        StartCoroutine("combat_process");
    }
}
