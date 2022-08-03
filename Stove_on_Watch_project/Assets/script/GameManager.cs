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
    public Queue<abst_action> order_list;

    private player Plr;

    #region combat
    private bool combat_process() {   //★무한루프로 인해 게임 정지 시, 병렬 처리 검색 필요
        bool temp_Plr_turn = true;  //describes whose turn is it now during current acction, it can be different from it in actual turn
        int temp_value = 0;

        while (true) {
            //★if is_Plr_turn : on_Plr_turn_start
            while (temp_Plr_turn == this.is_Plr_turn | order_list.Count > 0) {
                order_list.Dequeue().use();
                Thread.Sleep(100); //★유니티가 통째로 멈출 가능성 높음
            }
            temp_Plr_turn = this.is_Plr_turn;
            //★if is_Plr_turn : on_Plr_turn_end
        }
        return true;    //★추후 전투 승리/패배 여부 반환, 게임오버와 연결
    }

    #region variant_process
    public int attack(bool is_simulation, thing giver, int value) {
        
        return 0;
        if (is_simulation) { /*★최종 계산된 피해량값*/ }
        else { /*★상대방의 체력 -(최종 계산된 피해량값)*/ }
    }
    #endregion variant_process
    #endregion combat

    #region get_set
    public bool get_is_Plr_turn() { return this.is_Plr_turn; }
    public void set_is_Plr_turn(bool b) { is_Plr_turn = b; } //player calls it with button instantly to prevent click turn_end twice, enemy calls it in its function directly anyway (it's no problem)
    public player get_Plr() { return Plr; }
    #endregion get_set

    void Awake() {
        if (g == null) { g = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
    }
}
