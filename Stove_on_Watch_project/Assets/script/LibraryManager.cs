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
    private List<string> enemy_list;
    //private List<string> used_enemy_list;
    private List<string> event_first_list;
    //private List<string> used_event_first_list;
    private List<string> event_second_list;
    //private List<string> used_event_second_list;
    private List<string> event_both_list;
    private List<string> ability_list;
    //private List<string> used_ability_list;

    private int code;   //temp

    public void init() {
        if (action_list == null) { action_list = new List<string>(); } else { action_list.Clear(); }
        //if (used_action_list == null) { used_action_list = new List<string>(); } else { used_action_list.Clear(); }
        if (tool_list == null) { tool_list = new List<string>(); } else { tool_list.Clear(); }
        if (enemy_list == null) { enemy_list = new List<string>(); } else { enemy_list.Clear(); }
        //if (used_enemy_list == null) { used_enemy_list = new List<string>(); } else { used_enemy_list.Clear(); }
        if (event_first_list == null) { event_first_list = new List<string>(); } else { event_first_list.Clear(); }
        //if (used_event_first_list == null) { used_event_first_list = new List<string>(); } else { used_event_first_list.Clear(); }
        if (event_second_list == null) { event_second_list = new List<string>(); } else { event_second_list.Clear(); }
        //if (used_event_second_list == null) { used_event_second_list = new List<string>(); } else { used_event_second_list.Clear(); }
        if (event_both_list == null) { event_both_list = new List<string>(); } else { event_both_list.Clear(); }
        if (ability_list == null) { ability_list = new List<string>(); } else { ability_list.Clear(); }
        //if (used_ability_list == null) { used_ability_list = new List<string>(); } else { used_ability_list.Clear(); }

        DirectoryInfo d; FileInfo[] f;
        d = new DirectoryInfo("Assets/script/Action_Power/Plr_action");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            action_list.Add(file.Name);
        }
        d = new DirectoryInfo("Assets/script/Action_Power/Tool");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            action_list.Add(file.Name);
        }
        d = new DirectoryInfo("Assets/script/Thing/Enemy/Normal");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            action_list.Add(file.Name);
        }
        d = new DirectoryInfo("Assets/script/Event/Event_first");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            action_list.Add(file.Name);
        }
        d = new DirectoryInfo("Assets/script/Event/Event_second");
        f = d.GetFiles("*.cs");
        foreach (FileInfo file in f) {
            action_list.Add(file.Name);
        }
        //★ability library 추가
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

    #region return
    public abst_Plr_action return_action() {
        code = GameManager.g.ran.xoshiro_range(action_list.Count);
        string temp = action_list[code];
        action_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, action_list[code].Length - 3))
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

    public abst_enemy return_enemy() {
        code = GameManager.g.ran.xoshiro_range(enemy_list.Count);
        string temp = enemy_list[code];
        enemy_list.RemoveAt(code);
        return Activator.CreateInstance(
            Type.GetType(temp.Substring(0, temp.Length - 3))
            ) as abst_enemy;
    }

    public abst_event return_event(bool is_first) {
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
        DontDestroyOnLoad(this.gameObject);
        init(); //★
    }
}
