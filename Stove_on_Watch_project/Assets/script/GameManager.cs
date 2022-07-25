using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameManager : MonoBehaviour
{
    private GameManager g = null;

    xoshiro ran;
    graph_generator gra;

    private bool is_Plr_turn;

    public delegate void order_func(int i);
    Queue<order_func> order_list;
    Queue<int> order_value_list;

    public delegate int eff_func(bool b, int i);   //b==false means the function is used for calculation, i means action's value such as attack or block
    private List<eff_func> on_combat_start;
    private List<eff_func> on_Plr_turn_start;   //also describe 'on_enemy_turn_end' with barricade
    private List<eff_func> on_action;
    private List<eff_func> on_attack;
    private List<eff_func> on_block;
    private List<eff_func> on_Plr_hp_down;
    private List<eff_func> on_calm;
    private List<eff_func> on_Plr_turn_end;     //also describe 'on_enemy_turn_start' with barricade
    private List<eff_func> on_combat_end;

    #region combat
    private bool combat_process() {   //�ڹ��ѷ����� ���� ���� ���� ��, ���� ó�� �˻� �ʿ�
        order_func temp_order;
        bool temp_Plr_turn = true;
        int temp_value = 0;

        while (true) {
            //��if is_Plr_turn : on_Plr_turn_start
            while (temp_Plr_turn == this.is_Plr_turn | order_list.Count > 0) {
                if (order_list.Count > 0) {
                    order_list.Dequeue()(order_value_list.Dequeue());
                }
                Thread.Sleep(100); //������Ƽ�� ��°�� ���� ���ɼ� ����
            }
            temp_Plr_turn = this.is_Plr_turn;
            //��if is_Plr_turn : on_Plr_turn_end
        }
        return true;    //������ ���� �¸�/�й� ���� ��ȯ, ���ӿ����� ����
    }

    private void turn_end() {   //player calls it with button instantly to prevent click turn_end twice, enemy calls it in its function directly anyway (it's no problem)
        is_Plr_turn = !is_Plr_turn;
    }

    #region trivial_process

    #endregion trivial_process
    #endregion combat

    void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
