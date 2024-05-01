using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Supporter;

public class Entity_Enemy : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param : ICodeProvider
	{
		
		public int index;
		public string code;
		public string name;
		public int stage;
		public float baseHp;
		public float baseDamage;
		public float baseMoveSpeed;
		public float baserotationSpeed;
		public float attackSpeed;
		public float baseRange;
		public float knockBackAmount;
		public float knockBackTime;
		public int attackType;
		public float viewingAngle;
		public int additonalHp;
		public int additionalDamage;
		public int additionalMoveSpeed;
		public string GetCode()
		{
			return code;
		}
	}
}