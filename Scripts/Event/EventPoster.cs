using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Aurality/Event/Event Poster")]

public class EventPoster : MonoBehaviour {
	
	public enum TriggerOn{
		Start,
		Disable,
		CollisionEnter,
		CollisionExit,
		TriggerEnter,
		TriggerExit
	}
	
	public string EventName;
	public EventAction Action;
	public TriggerOn triggerOn;
	
	void Start () {
		if (triggerOn == TriggerOn.Start) post();
	}
	
	void OnCollisionEnter(){
		if (triggerOn == TriggerOn.CollisionEnter) post();
	}
	
	void post(){
		AudioManager.Instance.Post(Action, EventName, transform);
	}
}
