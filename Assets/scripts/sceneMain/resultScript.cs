using UnityEngine;
using System.Collections;

public class resultScript : MonoBehaviour {
	public AudioClip bgmClip;
	public GameObject loadButtonObj;
	public GameObject gradeButtonObj;
	public GameObject gradeTextObj;
	public GameObject gradePrefab;
	public AudioClip[] seClip;
	public Material[] gradeOKMaterial;
	public Material[] gradeNGMaterial;
	public Material[] resultMaterial;
	private TmSystem _sys;
	private int _rno;
	private float _timer;
	private int _grade;
	private int _stageId;
	private int _defStageId;
	private bool[] mRankFlag;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		_rno = 0;
		_timer = 0.0f;
		_stageId = _defStageId = PlayerPrefs.GetInt(gameScript.STAGE_ID_KEY);
		_grade = Random.Range(0,3);
		if(PlayerPrefs.HasKey(gameScript.STAGE_RANK_ID_KEY)){
			_grade = PlayerPrefs.GetInt(gameScript.STAGE_RANK_ID_KEY);
		}

		if(bgmClip!=null){
			_sys.soundCall(TmSystem.SOUND_CH.BGM, bgmClip, 1.0f, false);
		}
		gradeButtonObj.SendMessage("SM_setMaterial",gradeOKMaterial[_grade]);
		gradeButtonObj.SendMessage("SM_setAudioClip",seClip[(_defStageId-1)*3+_grade]);
		renderer.material = resultMaterial[_grade];

		mRankFlag = gameScript.getRankFlag();
		GameObject go;
		Vector3 pos;
		int iy = 10 -_stageId;
		gradeTextObj.GetComponent<TextMesh>().text = "シーン"+_stageId.ToString()+"クリア！";
		for(int ix = 0; ix < 3; ++ix){
			Material selMat = mRankFlag[ix+(9-iy)*3] ? gradeOKMaterial[ix] : gradeNGMaterial[ix];
			pos = new Vector3((2-ix)*0.125f+0.1f,0.15f,-0.1f);
			pos += transform.position;
			go = GameObject.Instantiate(gradePrefab,pos,Quaternion.identity) as GameObject;
			go.SendMessage("SM_setMaterial",selMat);
			if(mRankFlag[ix+(9-iy)*3]){
				go.SendMessage("SM_setAudioClip",seClip[ix+(9-iy)*3]);
			}
		}

		mRankFlag[(_stageId-1)*3+_grade] = true;
		string rankStr = gameScript.setRankStr(mRankFlag);

#if ((UNITY_ANDROID||UNITY_IPHONE) && !UNITY_EDITOR)
		if(AdMobManager.instance!=null){
			AdMobManager.instance.refresh();
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
		switch(_rno){
		case 0:
			_timer += Time.deltaTime;
			if(_timer > 2.0f){
				_rno++;
				_sys.soundCall(TmSystem.SOUND_CH.VOICE, seClip[(_defStageId-1)*3+_grade], 1.0f, false);
			}
			break;
		case 1:
			break;
		}
	}
	
	private void SM_onLoadButton(){
		string nextSceneStr = PlayerPrefs.GetString(gameScript.AFTER_RESULT_SCENE_KEY);
		if(nextSceneStr!=""){
			loadButtonObj.SendMessage("SM_setNextSceneStr",nextSceneStr);
		}else{
			_stageId++;
			// game stage が maxを超えたら"記録"に戻る 
			if(_stageId > gameScript.STAGE_NUM){
				_stageId = 1;
				loadButtonObj.SendMessage("SM_setNextSceneStr","record");
			}
			PlayerPrefs.SetInt(gameScript.STAGE_ID_KEY,_stageId);
		}
	}
}
