using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_PlayerData : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string code;
		public string name;
		public float baseHp;
		public float baseDamage;
		public float baseMoveSpeed;
		public float baseRotSpeed;
		public float baseRange;
		public float baseAttackSpeed;
		public float bulletSpeed;
		public int cost;
	}
}