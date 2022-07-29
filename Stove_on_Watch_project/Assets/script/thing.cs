using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : MonoBehaviour
{
    protected int max_hp;
    protected int cur_hp;
    //★플레이어 및 몬스터 스프라이트

    public delegate int eff_exist(int value);    //return conclusional value, it's used for describing card's numbers
    public List<eff_exist> on_action;   //!
    public List<eff_exist> on_attack;   //!
    public List<eff_exist> on_attacked;   //!
    public List<eff_exist> on_block;    //!
    public List<eff_exist> on_hp_down;  //!

    private Dictionary<string, int> status; //similar to presentative description of on_ array

    #region get_set
    
    #endregion get_set

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
