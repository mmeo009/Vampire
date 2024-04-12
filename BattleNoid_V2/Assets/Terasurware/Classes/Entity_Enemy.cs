using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Enemy : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string code;
		public string name;
		public int stage;
		public float baseHp;
		public float baseDamage;
		public float baseMoveSpeed;
		public float baserotationSpeed;
		public float baseRange;
		public float knockBackAmount;
		public float knockBackTime;
		public int attackType;
	}
}