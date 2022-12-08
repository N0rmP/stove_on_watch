using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class node : MonoBehaviour
{
    private int[] coor = new int[2];
    private node[] link = new node[4];  //0 up, 1 right, 2 down, 3 left
    private GameObject[] adjacent_edges = new GameObject[4];
    private bool visited;
    private bool reachable_center;
    public int route_homedirection;
    private List<thing> things_here;

    protected abst_event event_here;
    public abst_event event_here_ {
        get { return event_here; }
        set { 
            event_here = value; 
            if(value != null)
                value.cur_pos_ = this;
        }
    }

    public void init() {
        for (int i = 0; i < 4; i++) {
            link[i] = null;
            adjacent_edges[i] = null;
        }
        visited = false;
        route_homedirection = -1;
        if (things_here == null)
            things_here = new List<thing>();
        else
            things_here.Clear();
        event_here = null;
        gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
    }

    public void init(int x, int y){
        coor[0] = x;
        coor[1] = y;
        init();
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
        GraphicManager.g.set_image_color(transform.GetChild(0).gameObject, new Color(0f, 0f, 0f, 0f));
        /*ColorBlock temp_colorblock = temp.colors;
        temp_colorblock.normalColor = new Color(1f, 1f, 1f, 1f);
        temp.colors = temp_colorblock;*/
    }

    public void de_interactive() {
        Button temp = this.gameObject.GetComponent<Button>();
        temp.interactable = false;
        GraphicManager.g.set_image_color(transform.GetChild(0).gameObject, new Color(0f, 0f, 0f, 0.3f));
    }

    public void hand_thing(thing t, node n) {
        n.add_thing(t);
        this.things_here.Remove(t);
    }

    public bool is_enemy_here() {
        foreach (thing t in things_here) {
            if (t.GetType().IsSubclassOf(typeof(abst_enemy))) {
                return true;
            }
        }
        return false;
    }

    public bool is_Plr_here() {
        foreach (thing t in things_here) {
            if (t.GetType() == typeof(player))
                return true;
        }
        return false;
    }

    //called when this node enter or get out of player's POV
    public void POV_process(bool entered) {
        //★여유가 있다면 색으로 나타내는 대신 플레이어/몬스터는 아이콘을 올리는 것으로 변경할 것
        Color temp_color = new Color(0f, 0f, 0f, 1f);
        if (entered) {
            if (is_Plr_here())
                temp_color.g = 1f;
            else if (is_enemy_here())
                temp_color.r = 1f;
            else if (event_here != null) {
                temp_color.r = 1f;
                temp_color.g = 1f;
            } else {
                temp_color.r = 1f;
                temp_color.g = 1f;
                temp_color.b = 1f;
            }
            GraphicManager.g.set_image_color(this.gameObject, temp_color);
                
            GraphicManager.g.set_image_color(transform.GetChild(0).gameObject, new Color(0f, 0f, 0f, 0f));
            foreach (GameObject g in adjacent_edges)
                if (g != null)
                    g.SetActive(true);
        } else {
            GraphicManager.g.set_image_color(this.gameObject, new Color(0.3f, 0.3f, 0.3f, 1f));
        }

    }

    public void set_adjacent_edge(int dir, GameObject edge_param) {
        adjacent_edges[dir] = edge_param;
    }

    #region get_set
    public int[] get_coor() { return this.coor; }
    public void set_coor(int[] i) { this.coor = i; }
    public node[] get_link() { return this.link; }
    public void set_link(int index, node n) { this.link[index] = n; }
    public bool get_visited() { return this.visited; }
    public void set_visited(bool b) { this.visited = b; }
    public List<abst_enemy> get_enemies_here() {
        List<abst_enemy> temp = new List<abst_enemy>();
        foreach (thing t in things_here) {
            if (t.GetType().IsSubclassOf(typeof(abst_enemy))) {
                temp.Add((abst_enemy)t);
            }
        }
        return temp;
    }
    public void add_thing(thing t) { things_here.Add(t); }
    public void remove_thing(thing t) { things_here.Remove(t); }
    #endregion get_set

    public void existence_test() { Debug.Log("cordinate of this is : " + this.coor[0] + ", " + this.coor[1]); }
}
