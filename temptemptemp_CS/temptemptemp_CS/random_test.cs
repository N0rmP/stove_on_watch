using System;
using System.Numerics;
using System.Security.Cryptography;
public class random_test
{
	public ulong[] seed_arr;
	//public ulong seed_val;

	public random_test()
	{
		seed_arr = new ulong[4];
		counter = 0;
	}

	#region real_test
	/*static void Main(string[] args)
	{
		random_test main_ran = new random_test();

		//main_ran.seed_GetNonZeroBytes_arr();
		//main_ran.splitmix();
		Random ran = new Random();
		byte[] main_temp = new byte[32];
		ran.NextBytes(main_temp);
		ran = null;
		main_ran.seed_arr[0] = BitConverter.ToUInt64(main_temp, 0);
		main_ran.seed_arr[1] = BitConverter.ToUInt64(main_temp, 8);
		main_ran.seed_arr[2] = BitConverter.ToUInt64(main_temp, 16);
		main_ran.seed_arr[3] = BitConverter.ToUInt64(main_temp, 24);

		double temp = 0.0;
		for (int i = 0; i < 100000000; i++)
		{
			temp += (double)main_ran.xoshiro256plus() / 0xffffffffffffffff;
		}
		temp /= 100000000;
		Console.WriteLine("표준 분포 시 평균 : " + temp);
		Console.WriteLine("end");
	}*/
	#endregion real_test

	#region seed_part
	private void seed() {
		Random ran = new Random();
		byte[] main_temp = new byte[32];
		ran.NextBytes(main_temp);
		ran = null;
		main_ran.seed_arr[0] = BitConverter.ToUInt64(main_temp, 0);
		main_ran.seed_arr[1] = BitConverter.ToUInt64(main_temp, 8);
		main_ran.seed_arr[2] = BitConverter.ToUInt64(main_temp, 16);
		main_ran.seed_arr[3] = BitConverter.ToUInt64(main_temp, 24);
	}

    /*public void seed_GetNonZeroBytes_arr()
	{
		using (var rng = new RNGCryptoServiceProvider())
		{
			var temp = new byte[32];
			rng.GetNonZeroBytes(temp);
			this.seed_arr[0] = BitConverter.ToUInt64(temp, 0);
			this.seed_arr[1] = BitConverter.ToUInt64(temp, 8);
			this.seed_arr[2] = BitConverter.ToUInt64(temp, 16);
			this.seed_arr[3] = BitConverter.ToUInt64(temp, 24);
		}
	}
	public void seed_basic_val()
	{
		byte[] temp = new byte[32];
		Random ran = new Random();
		ran.NextBytes(temp);
		this.seed_val = BitConverter.ToUInt64(temp);
		ran = null;
	}*/
    #endregion seed_part

    #region generator_part
    public ulong generate()		//xoshiro256
	{
		var result = this.seed_arr[0] + this.seed_arr[3];

		var t = this.seed_arr[1] << 17;
		this.seed_arr[2] ^= this.seed_arr[0];
		this.seed_arr[3] ^= this.seed_arr[1];
		this.seed_arr[1] ^= this.seed_arr[2];
		this.seed_arr[0] ^= this.seed_arr[3];
		this.seed_arr[2] ^= t;
		this.seed_arr[3] = BitOperations.RotateLeft(this.seed_arr[3],45);

		return result;
	}

	public int rand_range(int r) {
		return this.generate() % r;
	}

	/*public void splitmix()
	{
		Random ran = new Random();
		byte[] temp = new byte[8];
		ran.NextBytes(temp);
		ran = null;

		ulong result = BitConverter.ToUInt64(temp);
		for (int i = 0; i < 4; i++)
		{
			result += 0x9E3779B97F4A7C15UL;
			result = (result ^ (result >> 30)) * 0xBF58476D1CE4E5B9UL;
			result = (result ^ (result >> 27)) * 0x94D049BB133111EBUL;
			result ^= (result >> 31);
			this.seed_arr[i] = result;
		}
	}*/
	#endregion generator_part
}
