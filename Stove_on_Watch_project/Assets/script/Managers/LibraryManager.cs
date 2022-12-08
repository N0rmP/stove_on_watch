using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LibraryManager : MonoBehaviour
{
    public static LibraryManager li; //l is too confusing

    private List<string> action_list;
    //private List<string> used_action_list;
    private List<string> tool_list;
    private List<string> enemy_normal_list;
    private List<string> enemy_elite_list;
    private List<string> enemy_root_list;
    //private List<string> used_enemy_normal_list;
    private List<string> event_first_list;
    //private List<string> used_event_first_list;
    private List<string> event_second_list;
    //private List<string> used_event_second_list;
    private List<string> event_both_list;
    private List<string> ability_list;
    //private List<string> used_ability_list;

    private int code;   //temp

    #region initiators
    public void init() {
        if (action_list == null) { action_list = new List<string>(); } else { action_list.Clear(); }
        //if (used_action_list == null) { used_action_list = new List<string>(); } else { used_action_list.Clear(); }
        if (tool_list == null) { tool_list = new List<string>(); } else { tool_list.Clear(); }
        if (enemy_normal_list == null) { enemy_normal_list = new List<string>(); } else { enemy_normal_list.Clear(); }
        if (enemy_elite_list == null) { enemy_elite_list = new List<string>(); } else { enemy_elite_list.Clear(); }
        if (enemy_root_list == null) { enemy_root_list = new List<string>(); } else { enemy_root_list.Clear(); }
        //if (used_enemy_normal_list == null) { used_enemy_normal_list = new List<string>(); } else { used_enemy_normal_list.Clear(); }
        if (event_first_list == null) { event_first_list = new List<string>(); } else { event_first_list.Clear(); }
        //if (used_event_first_list == null) { used_event_first_list = new List<string>(); } else { used_event_first_list.Clear(); }
        if (event_second_list == null) { event_second_list = new List<string>(); } else { event_second_list.Clear(); }
        //if (used_event_second_list == null) { used_event_second_list = new List<string>(); } else { used_event_second_list.Clear(); }
        if (event_both_list == null) { event_both_list = new List<string>(); } else { event_both_list.Clear(); }
        if (ability_list == null) { ability_list = new List<string>(); } else { ability_list.Clear(); }
        //if (used_ability_list == null) { used_ability_list = new List<string>(); } else { used_ability_list.Clear(); }

        action_init();
        enemy_normal_init();
        enemy_elite_init();
        enemy_root_init();
        event_first_init();
        event_second_init();
        event_both_init();

        /*
        d = new DirectoryInfo("Assets/script/Action_Power/Tool");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            tool_list.Add(file.Name);
        }*/
        //★ability library 추가 (보류)
    }

    private void action_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Action_Power/Plr_action")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            action_list.Add(file.Name);
    }
    private void enemy_normal_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Thing/Enemy/Normal")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            enemy_normal_list.Add(file.Name);
    }
    private void enemy_elite_init() {
        //★다른 root를 만들 수 있다면 확장할 것
        FileInfo[]  f = (new DirectoryInfo("Assets/script/Thing/Enemy/Elite/mother")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            enemy_elite_list.Add(file.Name);
    }
    private void enemy_root_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Thing/Enemy/Root")).GetFiles("*.cs");
        foreach (FileInfo file in f) {
            enemy_root_list.Add(file.Name);
        }
    }
    private void event_first_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Event/Event_first")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            event_first_list.Add(file.Name);
    }
    private void event_second_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Event/Event_second")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            event_second_list.Add(file.Name);
    }
    private void event_both_init() {
        FileInfo[] f = (new DirectoryInfo("Assets/script/Event/Event_both")).GetFiles("*.cs");
        foreach (FileInfo file in f)
            event_both_list.Add(file.Name);
    }

    /*★지정되지 않은 파일로부터 library를 추가할 수 있도록 하여 확장성을 확보할 것
    public void collect_action() {
        DirectoryInfo di = new DirectoryInfo("Assets/script/Actions&Powers/Plr_actions");
        FileInfo[] files = di.GetFiles("*.cs");
        foreach (FileInfo file in files) {
            action_list.Add(file.Name);
        }
    }
    */
    #endregion initiators

    #region return
    public abst_Plr_action return_action() {
        if (action_list.Count <= 0)
            action_init();
        code = GameManager.g.ran.xoshiro_range(action_list.Count);
        string temp = action_list[code];
        action_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, temp.Length - 3))
            ) as abst_Plr_action;
    }

    public abst_tool return_tool() {
        code = GameManager.g.ran.xoshiro_range(tool_list.Count);
        string temp = tool_list[code];
        tool_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, temp.Length - 3))
            ) as abst_tool;
    }

    public abst_enemy return_enemy(int x, int y) {
        if (enemy_normal_list.Count <= 0)
            enemy_normal_init();
        code = GameManager.g.ran.xoshiro_range(enemy_normal_list.Count);
        string temp = enemy_normal_list[code];
        enemy_normal_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, temp.Length - 3)), x, y
            ) as abst_enemy;
    }

    public abst_enemy return_elit_enemy(int x, int y/*bool belongs*/) {
        if (enemy_elite_list.Count <= 0)
            enemy_elite_init();
        code = GameManager.g.ran.xoshiro_range(enemy_elite_list.Count);
        string temp = enemy_elite_list[code];
        enemy_elite_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, temp.Length - 3)), x, y
            ) as abst_enemy;
    }

    public abst_enemy return_root_enemy() {
        return null;
    }

    public abst_event return_event() {
        if (event_first_list.Count <= 0) event_first_init();
        if (event_second_list.Count <= 0) event_second_init();
        if (event_both_list.Count <= 0) event_both_init();
        bool is_first = GameManager.g.get_is_first_stage();
        List<string> ttemp = is_first ? event_first_list : event_second_list;
        code = GameManager.g.ran.xoshiro_range(event_both_list.Count + ttemp.Count);
        string temp;
        if (code < event_both_list.Count) {
            temp = event_both_list[code];
            event_both_list.RemoveAt(code);
            return Activator.CreateInstance(
                Type.GetType(temp.Substring(0, temp.Length - 3))
                ) as abst_event;
        } else {
            code -= event_both_list.Count;
            temp = ttemp[code];
            ttemp.RemoveAt(code);
            return Activator.CreateInstance(
                Type.GetType(temp.Substring(0, temp.Length - 3))
                ) as abst_event;
        }
    }
    #endregion return

    void Awake()
    {
        if (li == null) { li = this; } else { Destroy(this.gameObject); }
        //DontDestroyOnLoad(this.gameObject);
    }
}
