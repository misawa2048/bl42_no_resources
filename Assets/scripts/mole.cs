using UnityEngine;
using System.Collections;

public class mole : MonoBehaviour {
	private const float SOUND_WAIT_TIME = 0.5f;
	public AudioClip[] moleVoiceList;
	public moleController.MOLE_TYPE moleType;
	public float destroySpeed = 300.0f;
	public int touchCount = 1;
	public float scoreRate = 1.0f;
	public float scoreDecSpeed = 10.0f;
	private int defTouchCount;
	private float scoreBase = 100.0f;
	private bool isStarted;
	private bool isToDestroy;
	private Vector3 defScale;
	private float scaleRange;
	private float scaleTimer;
	private float soundWaitTimer;
	private float tickDir = 0.0f;
	private float defScaleSize = 60.0f;	// 60deg.のサイズ基準(0-180) 
	private TmSystem _sys;
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		if(transform.childCount==0) return;
		isStarted = false;
		isToDestroy = false;
		scaleTimer = 0.0f;
		defTouchCount = touchCount;
		defScale = transform.lossyScale;
		transform.GetChild(0).renderer.enabled=false;
		scaleRange = Mathf.Sin(defScaleSize/180.0f*Mathf.PI);
		transform.GetChild(0).transform.localScale = Vector3.zero;
		transform.GetChild(0).renderer.enabled=true;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount==0) return;
		if(!isStarted){
			scaleTimer = Mathf.Min(scaleTimer+destroySpeed*Time.deltaTime,defScaleSize);
			if(scaleTimer==defScaleSize){
				isStarted = true;
			}
		}else if(isToDestroy){
			toDestroy();
		}else{
			scoreBase = Mathf.Max( scoreBase-Time.deltaTime*scoreDecSpeed , 1.0f);
			tickDir += Time.deltaTime;
			gameObject.transform.Rotate(0,0,Mathf.Cos(tickDir*Mathf.PI)*0.1f);
		}
		Vector3 tmpScale = defScale * Mathf.Sin(scaleTimer/180.0f*Mathf.PI) / scaleRange;
		tmpScale *= (1.0f-Mathf.Cos(tickDir*5.0f)*0.02f);
		transform.GetChild(0).transform.localScale = tmpScale/transform.localScale.x;
	}
	
	void SM_onMouseDown(float addScore) {
		if(!isToDestroy){
			if(defTouchCount==1){
				if(!_sys.mw.isButtonState(TmMouseWrapper.STATE.DOWN)){
					return;
				}
			}
			_sys.soundCall(TmSystem.SOUND_CH.SE, moleVoiceList[Random.Range(0,moleVoiceList.Length)], 1.0f, false);
			// toDestroyStart
			isToDestroy = true;
			scaleTimer = defScaleSize;
			soundWaitTimer = SOUND_WAIT_TIME;
			if(addScore>0.0f){
			}else if(addScore<0.0f){ // マイナスなら即終了 
				touchCount = 1;
			}
		}
	}

	public bool isKilled() { return (touchCount==0); }
	
	public float getScore() {
		return scoreBase*scoreRate;
	}
	
	private void toDestroy(){
		scaleTimer = Mathf.Min(scaleTimer+destroySpeed*Time.deltaTime,180.0f);
		if(touchCount>1){
			if(scaleTimer >= 120.0f){
				scaleTimer = defScaleSize;
				touchCount--;
				isToDestroy = false;
				scoreRate *= 2.0f;	// 連続押しでスコア倍増 
			}
		}else{
			gameObject.collider.enabled = false;
			touchCount = 0;
			soundWaitTimer -= Time.deltaTime;
			if((scaleTimer >= 180.0f)&&(soundWaitTimer<0.0f)){
				Destroy(gameObject);
			}
		}
	}
}
