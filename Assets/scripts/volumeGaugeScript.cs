using UnityEngine;
using System.Collections;

public class volumeGaugeScript : MonoBehaviour {
	public GameObject leverObj;
	public GameObject gaugeObj;
	public AudioClip testAudioClip;
	public TmSystem.SOUND_CH volType;
	public bool isMasterVol;
	private TmSystem _sys;
	private Vector3 _minPos;
	private Vector3 _maxPos;
	private float _rate=0.5f;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		_minPos = gaugeObj.transform.position - gaugeObj.transform.lossyScale * 0.5f;
		_maxPos = gaugeObj.transform.position + gaugeObj.transform.lossyScale * 0.5f;
		if(volType==TmSystem.SOUND_CH.BGM){
			if(testAudioClip!=null){
				_sys.soundCall(volType,testAudioClip,1.0f);
			}
		}
		
		_rate = (isMasterVol) ? _sys.getMasterVolume() : _sys.getChannelVolume(volType);

		Vector3 pos = _minPos + (_maxPos-_minPos)*_rate;
		pos.y = leverObj.transform.position.y;
		pos.z = leverObj.transform.position.z;
		
		leverObj.transform.position = pos;

	}
	
	// Update is called once per frame
	void Update () {
		if(_sys.mw.isMouseState(TmMouseWrapper.STATE.DRAG)){
			if(_sys.mw.dragTarget==leverObj){
				Vector3 pos = _sys.mw.dragPos + _sys.mw.dragTargetOfs;
				pos.x = Mathf.Clamp(pos.x,_minPos.x,_maxPos.x);
				_rate = (pos.x - _minPos.x)/(_maxPos.x-_minPos.x);
				
//				pos.y = Mathf.Clamp(pos.y,_minPos.y,_maxPos.y);
//				pos.z = Mathf.Clamp(pos.z,_minPos.z,_maxPos.z);
				pos.y = leverObj.transform.position.y;
				pos.z = leverObj.transform.position.z;
				leverObj.transform.position = pos;
				
			}
		}
		if(isMasterVol){
			_sys.setMasterVolume(_rate,false);
		}else if(volType==TmSystem.SOUND_CH.BGM){
			_sys.setChannelVolume(volType,_rate,false);
			_sys.soundCall(TmSystem.SOUND_CH.BGM,null,1.0f);
		}else{
			if(_sys.mw.isMouseState(TmMouseWrapper.STATE.UP)){
				if(_sys.mw.dragTargetOld==leverObj){
					_sys.setChannelVolume(volType,_rate,false);
					_sys.soundCall(volType,testAudioClip,1.0f);
				}
			}
		}
		// 離したときにセーブ 
		if(_sys.mw.isMouseState(TmMouseWrapper.STATE.UP)){
			_sys.saveSysData();
		}
	}

}
