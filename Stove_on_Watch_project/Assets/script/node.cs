using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class node : MonoBehaviour
{
    private int[] coor = new int[2];
    private node[] link = new node[4];  //0 up, 1 right, 2 down, 3 left
    private bool visited;
    private List<thing> things_here;

    public void init(int x, int y) {
        this.coor[0] = x;
        this.coor[1] = y;
        for (int i = 0; i < 4; i++) { this.link[i] = null; }
        this.visited = false;
        this.things_here.Clear();
    }

    public void connect(node n, int dir) {
        if (n == null) { Debug.Log("from (" + this.coor[0] + "," + this.coor[1] + ") direction " + dir + " null node connecting error"); return; }
        this.link[dir] = n;
        n.set_link(dir < 2 ? dir + 2 : (dir + 2) % 4, this);
    }

    public void click() {
        //Debug.Log($" (x, y) =  ({this.coor[0]}, {this.coor[1]}) clicked");
        GameManager.g.set_selected_node(this.GetComponent<node>());
    }

    public void be_interactive() {
        Button temp = this.gameObject.GetComponent<Button>();
        temp.interactable = true;

        /*ColorBlock temp_colorblock = temp.colors;
        temp_colorblock.normalColor = new Color(1f, 1f, 1f, 1f);
        temp.colors = temp_colorblock;*/
    }

    public void de_interactive() {
        Button temp = this.gameObject.GetComponent<Button>();
        temp.interactable = false;
    }

    public void hand_thing(thing t, node n) {
        n.get_things_here().Add(this.things_here.Find( a=>a==t ));
        this.things_here.Remove(t);
    }

    #region get_set
    public int[] get_coor() { return this.coor; }
    public void set_coor(int[] i) { this.coor = i; }
    public node[] get_link() { return this.link; }
    public void set_link(int index, node n) { this.link[index] = n; }
    public bool get_visited() { return this.visited; }
    public void set_visited(bool b) { this.visited = b; }
    public List<thing> get_things_here() { return things_here; }
    #endregion get_set

    public void Awake() {
        things_here = new List<thing>();
    }
    public void FixedUpdate() {
        //★things_here에 Plr 존재 시 색 변경
        //★things_here에 종류불문 enemy 존재 시 색 변경
    }
    public void existence_test() { Debug.Log("cordinate of this is : " + this.coor[0] + ", " + this.coor[1]); }
}
