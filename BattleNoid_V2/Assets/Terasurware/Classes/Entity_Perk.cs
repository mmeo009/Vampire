using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Perk : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string code;
		public string name;
		public int perkType;
		public string perkRarity;
		public bool isReplicatable;
		public int taskAmount;
		public string taskType;
		public string taskCondition;
		public string taskReturnAmount;
	}
}