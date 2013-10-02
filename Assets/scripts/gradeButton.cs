using UnityEngine;
using System.Collections;

public class gradeButton : MonoBehaviour {
	public AudioClip seClip;
	private TmSystem _sys;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(_sys.mw.isMouseState(TmMouseWrapper.STATE.UP)){
			if(_sys.mw.hitTarget == gameObject){
				_sys.soundCall(TmSystem.SOUND_CH.VOICE, seClip, 1.0f, false);
			}
		}
	}
	
	private void SM_setMaterial(Material _mat){
		gameObject.renderer.material = _mat;
	}
	private void SM_setAudioClip(AudioClip _clip){
		seClip = _clip;
	}
	
}
