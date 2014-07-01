using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Aurality/Behaviours/Group Behaviour")]

public class GroupBehaviour : BehaviourComponent {
	
	protected delegate void del(BehaviourComponent b, Transform t);
	
	void Awake() {
		setGlobalProperties();		
	}
	
	void Update(){
		updateGlobalLevels();
		updatePause();
	}
	
	override protected bool isPlaying(){
		//if one of the sub-Behaviours is playing, we're playing
		BehaviourComponent cbc;
		foreach (Transform child in transform){
			cbc = child.GetComponent<BehaviourComponent>();
			if (cbc != null){
				if (cbc.IsPlaying) return true;
			}
		}
		return false;
	}
	
	override protected bool isActive(){
		//if one of the sub-Behaviours is active, we're active
		BehaviourComponent cbc;
		foreach (Transform child in transform){
			cbc = child.GetComponent<BehaviourComponent>();
			if (cbc != null){
				if (cbc.IsActive) return true;
			}
		}
		return false;
	}
	
	protected void triggerChildren(Transform tf, del action){
		BehaviourComponent cbc;
		foreach (Transform child in transform){
			cbc = child.GetComponent<BehaviourComponent>();
			if (cbc != null){
				action(cbc, tf);
			}
		}
	}
	
	/* play all behaviours in this group */
	override public void Play(Transform tf){
		triggerChildren(tf, (b,t) => b.Play(t));
	}
	
	/* stop all behaviours in this group */
	override public void Stop(Transform tf){
		triggerChildren(tf, (b,t) => b.Stop(t));
	}
	
	override public void Pause(Transform tf){
		triggerChildren(tf, (b,t) => b.Pause(t));
	}
	
	override public void Unpause(Transform tf){
		triggerChildren(tf, (b,t) => b.Unpause(t));
	}
}
