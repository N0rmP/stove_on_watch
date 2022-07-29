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

    public delegate void order_func(int i);
    Queue<order_func> order_list;
    Queue<int> order_value_list;

    public delegate void eff_none();
    private List<eff_none> on_combat_start;     //��on �迭 ���, ������ ������ ��� ĳ������ ������ �Լ� ����
    private List<eff_none> on_Plr_turn_start;   //also describe 'on_enemy_turn_end' with barricade
    private List<eff_none> on_calm;
    private List<eff_none> on_Plr_turn_end;     //also describe 'on_enemy_turn_start' with barricade
    private List<eff_none> on_combat_end;
    //some on_ array is in thing class, they should be called only when its owner acts

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

    #region variant_process
    public int attack(bool is_simulation, thing giver, int value) {
        //�ڴٸ� ��ũ��Ʈ�� delegate ������ �˻�
        foreach (eff_exist e in giver.on_attack) {
            e(value);
        }
        
        return 0;
        if (is_simulation) { /*������ ���� ���ط���*/ }
        else { /*�ڻ����� ü�� -(���� ���� ���ط���)*/ }
    }
    #endregion variant_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; } //player calls it with button instantly to prevent click turn_end twice, enemy calls it in its function directly anyway (it's no problem)
    #endregion get_set

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
