using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_json
{
    public string s1;
    public string s2;
    public int i1;
    public int i2;

    public temp_json() {
        s1 = "";
        s2 = "";
        i1 = -1;
        i2 = -1;
    }

    public void print() {
        Debug.Log($" s1={this.s1}, s2={this.s2}, i1={this.i1}, i2={this.i2} ");
    }
}
