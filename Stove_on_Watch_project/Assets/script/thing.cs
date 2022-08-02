using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : MonoBehaviour
{
    protected int max_hp;
    protected int cur_hp;
    //★플레이어 및 몬스터 스프라이트

    private List<power> powers;

    public void init()
    {
        this.cur_hp = this.max_hp;
        if (this.powers == null) { this.powers = new List<power>(); }
        else { this.powers.Clear(); }
    }

    #region get_set
    public int get_max_hp() { return this.max_hp; }
    public void set_max_hp(int i) { this.max_hp = i; }
    public int get_cur_hp() { return this.cur_hp; }
    public void set_cur_hp(int i) { this.cur_hp = i; }
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
