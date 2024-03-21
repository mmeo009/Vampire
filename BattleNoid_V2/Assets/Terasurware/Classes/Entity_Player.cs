using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Player : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string code;
		public string name;
		public double baseHp;
		public double baseDamage;
		public double baseMoveSpeed;
		public double baseRange;
		public double additionalHp;
		public double additionalDamage;
		public double additionalMoveSpeed;
		public double additionalRange;
		public int cost;
	}
}