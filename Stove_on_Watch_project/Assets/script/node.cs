using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    public int[] coor = new int[2];
    public node[] link = new node[4];  //0 up, 1 right, 2 down, 3 left
    public bool visited;
    ★일부러 틀렸다, node와 node_button 통합 진행시킬 것

    private GameObject node_button;

    public node(int x, int y)
    {
        this.coor[0] = x;
        this.coor[1] = y;
        for (int i = 0; i < 4; i++) { this.link[i] = null; }
        this.visited = false;
    }

    public void connect(node n, int dir)
    {
        if (n == null) { Debug.Log("from (" + this.coor[0] + "," + this.coor[1] + ") direction " + dir + " null node connecting error"); return; }
        this.link[dir] = n;
        n.link[dir < 2 ? dir + 2 : (dir + 2) % 4] = this;
    }

    #region get_set
    public GameObject get_node_button() { return this.node_button; }
    public void set_node_button(GameObject g) { this.node_button = g; }
    #endregion get_set

    public void existence_test() { Debug.Log("cordinate of this is : " + this.coor[0] + ", " + this.coor[1]); }
}
