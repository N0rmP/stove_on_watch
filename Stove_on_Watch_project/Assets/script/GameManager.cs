using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public static GameManager g = null;

    xoshiro ran;
    graph_generator gra;

    private bool is_Plr_turn;

    //public delegate void order_func(int i);
    public Queue<abst_Plr_action> order_list;
    public abst_Plr_action last_used;

    private player Plr;
    private List<abst_enemy> combat_opponents;
    private List<abst_enemy> wondering_opponents;

    public GameObject temp_butt;

    #region preparation
    public void init()
    {
        if (order_list == null) { order_list = new Queue<abst_Plr_action>(); } else { order_list.Clear(); }
        last_used = null; 
        if (combat_opponents == null) { combat_opponents = new List<abst_enemy>(); } else { combat_opponents.Clear(); }
        if (wondering_opponents == null) { wondering_opponents = new List<abst_enemy>(); } else { wondering_opponents.Clear(); }
        //�� ����
        //�̹� ������ �渷 ����
        //���� ��ġ
        //�� ��ġ
        //����� ��ġ
        this.Plr.init();
        //�÷��̾� ���� ��ġ ����?
    }
    #endregion preparation

    #region combat
    private bool combat_process() {   //�ڹ��ѷ����� ���� ���� ���� ��, ���� ó�� �˻� �ʿ�
        bool temp_Plr_turn = true;  //describes whose turn is it now during current acction, it can be different from it in actual turn
        abst_Plr_action temp_Plr_action;
        int blocking_infinity=0;

        this.is_Plr_turn = true;
        while (true) {
            //��if is_Plr_turn : on_Plr_turn_start
            while (temp_Plr_turn == this.is_Plr_turn | order_list.Count > 0) {
                if (order_list.Count > 0)
                {
                    temp_Plr_action = order_list.Dequeue();
                    temp_Plr_action.use();
                    last_used = temp_Plr_action;
                }
                this.testing();
                //Thread.Sleep(10); //������Ƽ�� ��°�� ���� ���ɼ� ����

                blocking_infinity++; Debug.Log(blocking_infinity);//��
                if (blocking_infinity >= 2000) { break; }
            }
            break;  //��
            temp_Plr_turn = this.is_Plr_turn;
            //��if is_Plr_turn : on_Plr_turn_end
        }
        return true;    //������ ���� �¸�/�й� ���� ��ȯ, ���ӿ����� ����
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
        this.last_used.set_cur_cooltime(true, -1);
        //foreach (abst_power a in receiver.powers) { a.on_after_haste(); }
    }
    #endregion variant_process

    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; } //player calls it with button instantly to prevent click turn_end twice, enemy calls it in its function directly anyway (it's no problem)
    public player get_Plr() { return Plr; }
    #endregion get_set

    public void testing() { Debug.Log("this is GameManager"); }

    void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        this.Plr = new player();
        this.init();    //��
    }

    private void Start()
    {
        this.combat_process();
    }
}
