using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Aurality/Event/Event Listener")]

public class EventListener : MonoBehaviour {
	
	public string EventName;
	private BehaviourComponent bc;
	
	void Awake() {
		bc = gameObject.GetComponent<BehaviourComponent>();
	}
	
	void Update() {
		
	}
	
	public void Run(EventAction eventAction, Transform tf) {
		switch (eventAction){
			case EventAction.Play:
				bc.Play(tf);
				break;
			case EventAction.Stop:
				bc.Stop(tf);
				break;
			case EventAction.Pause:
				bc.Pause(tf);
				break;
			case EventAction.KeyOff:
				bc.KeyOff(tf);
				break;
			case EventAction.Unpause:
				bc.Unpause(tf);
				break;
		}
	}
}
