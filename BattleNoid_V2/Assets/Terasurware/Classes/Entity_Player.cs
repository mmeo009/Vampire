using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Supporter;

public class Entity_Player : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param : ICodeProvider
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
		public string firstSkillName;
		public float firstSkillCoolDown;
		public string secondSkillName;
		public float secondSkillCoolDown;
		public int cost;
		public string GetCode()
		{
			return code;
		}
	}
}