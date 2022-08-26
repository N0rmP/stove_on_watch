using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_json
{
    public string s1;
    public string s2;
    public string s3;
    public string s4;
    public string s5;
    public string[] s;
    public int i1;
    public int i2;
    public int i3;
    public int i4;
    public int i5;

    public temp_json() { init(); }

    public void init() {    //this default value can remain unless json file change it
        s = new string[2] { "pizza", "chees" };
        s1 = null;
        s2 = null;
        s3 = null;
        s4 = null;
        s5 = null;
        i1 = -1;
        i2 = -1;
        i3 = -1;
        i4 = -1;
        i5 = -1;
    }

    public void print() {
        Debug.Log($" s1={this.s1}, s2={this.s2}, i1={this.i1}, i2={this.i2} ");
    }
}
