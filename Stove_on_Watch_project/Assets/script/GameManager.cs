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
    private List<eff_none> on_combat_start;     //★on 배열 대신, 전투에 참여한 모든 캐릭터의 지정된 함수 실행
    private List<eff_none> on_Plr_turn_start;   //also describe 'on_enemy_turn_end' with barricade
    private List<eff_none> on_calm;
    private List<eff_none> on_Plr_turn_end;     //also describe 'on_enemy_turn_start' with barricade
    private List<eff_none> on_combat_end;
    //some on_ array is in thing class, they should be called only when its owner acts

    #region combat
    private bool combat_process() {   //★무한루프로 인해 게임 정지 시, 병렬 처리 검색 필요
        order_func temp_order;
        bool temp_Plr_turn = true;
        int temp_value = 0;

        while (true) {
            //★if is_Plr_turn : on_Plr_turn_start
            while (temp_Plr_turn == this.is_Plr_turn | order_list.Count > 0) {
                if (order_list.Count > 0) {
                    order_list.Dequeue()(order_value_list.Dequeue());
                }
                Thread.Sleep(100); //★유니티가 통째로 멈출 가능성 높음
            }
            temp_Plr_turn = this.is_Plr_turn;
            //★if is_Plr_turn : on_Plr_turn_end
        }
        return true;    //★추후 전투 승리/패배 여부 반환, 게임오버와 연결
    }

    #region variant_process
    public int attack(bool is_simulation, thing giver, int value) {
        //★다른 스크립트의 delegate 실행방법 검색
        foreach (eff_exist e in giver.on_attack) {
            e(value);
        }
        
        return 0;
        if (is_simulation) { /*★최종 계산된 피해량값*/ }
        else { /*★상대방의 체력 -(최종 계산된 피해량값)*/ }
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
