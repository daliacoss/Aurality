using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Aurality/Behaviours/Random Behaviour")]

public class RandomBehaviour : GroupBehaviour {
	
	private List<BehaviourComponent> childrenBC = new List<BehaviourComponent>();
	
	void Awake () {
		setGlobalProperties();
		BehaviourComponent cbc;
		foreach (Transform child in transform){
			cbc = child.GetComponent<BehaviourComponent>();
			if (cbc != null){
				childrenBC.Add(cbc);
			}
		}
	}
		
	override public void Play(Transform tf){
		BehaviourComponent cbc = childrenBC[Random.Range(0, childrenBC.Count)];
		cbc.Play(tf);
	}
}
