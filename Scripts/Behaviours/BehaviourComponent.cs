using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class BehaviourComponent : MonoBehaviour {
	
	public float Volume = 1f;
	public float Pitch = 1f;
	public bool VolumeIsRelativeToParent = true;
	public bool PitchIsRelativeToParent = true;
	public BoolInherit Loop = BoolInherit.False;
	public BoolInherit ModulateWithTimeScale = BoolInherit.Inherit;
	public BoolInherit PauseWithTimeScale = BoolInherit.Inherit;
	public BoolInherit PauseWithListener = BoolInherit.Inherit;
	
	// is at least one instance of the event behaviour playing?
	abstract protected bool isPlaying();
	public bool IsPlaying{
		get {return isPlaying();}
	}
	// is at least one instance of the event behaviour playing or paused?
	abstract protected bool isActive();
	public bool IsActive{
		get {return isActive();}
	}
	// are all instances of the event behaviour paused?
	public bool IsPaused{
		get {return !IsPlaying && IsActive;}
	}
	
	// exact values for members inherited from parent
	public float GlobalVolume{
		get; private set;
	}
	public float GlobalPitch{
		get; private set;
	}
	public bool GlobalLoop{
		get; private set;
	}
	public bool GlobalModulateWithTimeScale{
		get; private set;
	}
	public bool GlobalPauseWithTimeScale{
		get; private set;
	}
	public bool GlobalPauseWithListener{
		get; private set;
	}
	
	abstract public void Play(Transform tf);
	abstract public void Stop(Transform tf);
	abstract public void Pause(Transform tf);
	abstract public void Unpause(Transform tf);
	virtual public void KeyOff(Transform tf){
		Stop(tf);
	}
		
	/* set all "global" properties */
	protected void setGlobalProperties(){
		updateGlobalLevels();
		GlobalModulateWithTimeScale = boolInheritToBool(this, "modulate");
		GlobalPauseWithTimeScale = boolInheritToBool(this, "pausetimescale");
		GlobalPauseWithListener = boolInheritToBool(this, "pauselistener");
		GlobalLoop = boolInheritToBool(this, "loop");

	}
	
	/* set only the global values of volume/pitch/panning */
	protected void updateGlobalLevels(){
		BehaviourComponent pb = transform.parent.GetComponent<BehaviourComponent>();
		
		GlobalVolume = Volume * ((pb != null && VolumeIsRelativeToParent) ? pb.Volume : 1f);
		GlobalPitch = Pitch * ((pb != null && PitchIsRelativeToParent) ? pb.Pitch : 1f);
		//panning = etc
	}
		
	protected void updatePause(){
		if (GlobalPauseWithTimeScale && Time.timeScale == 0){
			//PauseAll();
		}
		//SoundBehaviour will deal with PauseWithListener
	}
	
	/* search upwards through tree for bool value to inherit */
	protected bool boolInheritToBool(BehaviourComponent thisBehaviour, string propertyName){
		BoolInherit property =
			(propertyName == "loop") ? thisBehaviour.Loop :
			(propertyName == "modulate") ? thisBehaviour.ModulateWithTimeScale :
			(propertyName == "pausetimescale") ? thisBehaviour.PauseWithTimeScale :
			(propertyName == "pauselistener") ? thisBehaviour.PauseWithListener :
			thisBehaviour.PauseWithListener;
		if (property == BoolInherit.Inherit){
			BehaviourComponent pb = transform.parent.GetComponent<BehaviourComponent>();
			if (pb != null) {
				return boolInheritToBool(pb, propertyName);
			}
			//this should never happen with proper UI - behaviours with no parent can't inherit anything
			else{
				Debug.Log("cannot inherit from nonexistent parent");
				return false;
			}
		}
		
		else{
			return (property == BoolInherit.True);
		}
	}
}
