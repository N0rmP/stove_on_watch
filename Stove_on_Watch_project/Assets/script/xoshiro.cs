using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System;
//using System.Numerics;
//using System.Security.Cryptography;

public class xoshiro
{
    private ulong[] seed_arr;

	public xoshiro() { seed_arr = new ulong[4];	}

    #region seed_part
    public void seed()
	{
		Random ran = new Random();
		byte[] main_temp = new byte[32];
		ran.NextBytes(main_temp);
		ran = null;
		for (int i = 0; i < 4; i++)	{ this.seed_arr[i] = BitConverter.ToUInt64(main_temp, i * 8); }
	}

	public void seed(string desig_seed)
	{
		string temp;
		for (int i = 0; i < 4; i++)
		{
			temp = desig_seed.Substring(i * 16, 16);
			this.seed_arr[i] = Convert.ToUInt64(temp, 16);
		}
	}
    #endregion seed_part

    #region generator_part
    public ulong xoshiro256plus()
	{
		var result = this.seed_arr[0] + this.seed_arr[3];

		var t = this.seed_arr[1] << 17;
		this.seed_arr[2] ^= this.seed_arr[0];
		this.seed_arr[3] ^= this.seed_arr[1];
		this.seed_arr[1] ^= this.seed_arr[2];
		this.seed_arr[0] ^= this.seed_arr[3];
		this.seed_arr[2] ^= t;
		this.seed_arr[3] = (this.seed_arr[3] << 45) | (this.seed_arr[3] >> 19);//BitOperations.RotateLeft(this.seed_arr[3], 45);

		return result;
	}

	public int xoshiro_range(int max)
	{
		if (max == 0) { /*Console.WriteLine("zero inputed to xoshiro_range");*/ return 0; }
		return (int)(xoshiro256plus() % (ulong)max);
	}

	public int xoshiro_range(int min, int max)
	{
		if (max < min) { return -1; }
		if (max == min) { return min; }
		return (int)(xoshiro256plus() % (ulong)(max - min)) + min;
	}
    #endregion generator_part
}
