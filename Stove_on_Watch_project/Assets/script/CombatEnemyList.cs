using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnemyList {
    private List<abst_enemy> combat_enemy;
    private int selected = 0;
    public CombatEnemyList() {
        combat_enemy = new List<abst_enemy>();
    }

    public List<abst_enemy> get_list() {
        return combat_enemy;
    }

    public abst_enemy get(int ind) {
        return combat_enemy[ind];
    }

    public abst_enemy get_selected() {
        return combat_enemy[selected];
    }

    public int get_number() {
        return combat_enemy.Count;
    }

    public void add(abst_enemy ae) {
        combat_enemy.Add(ae);
        set_index(combat_enemy.Count - 1);

    }

    public void reomve(abst_enemy ae) {
        combat_enemy.Remove(ae);
    }

    public void clear() {
        combat_enemy.Clear();
        selected = 0;
    }

    public void next() {
        selected++;
        if (selected >= combat_enemy.Count) {
            selected = 0;
        }
        combat_enemy[selected].combat_board_.transform.SetAsLastSibling();
    }

    public void post() {
        selected--;
        if (selected < 0) {
            selected = combat_enemy.Count - 1;
        }
        combat_enemy[selected].combat_board_.transform.SetAsLastSibling();
    }

    public void set_index(int i){
        selected = i;
        if (selected >= combat_enemy.Count) {
            selected = 0;
        }else if (selected < 0) {
            selected = combat_enemy.Count - 1;
        }
        combat_enemy[selected].combat_board_.transform.SetAsLastSibling();
    }
}
