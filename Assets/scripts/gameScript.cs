using UnityEngine;
using System.Collections;

public class gameScript : MonoBehaviour {
	public const string STAGE_ID_KEY = "gameStageId";
	public const string RANK_GET_KEY = "rankGetStr";
	public const string STAGE_RANK_ID_KEY = "stageRankId";
	public const string AFTER_RESULT_SCENE_KEY = "afterResultSceneStr";
	public const int STAGE_NUM = 10;
	public AudioClip bgmClip;
	public GameObject gameStartPanelPrefab;
	public GameObject moleController;
	public Material[] stageMat;
	public AudioClip[] stageVoiceClip;
	private TmSystem _sys;
	private int _stageId;
	public int stageId { get{ return _stageId; } }
	private GameObject _startObj;
	private bool _started;
	public bool started { get{ return _started; } }
		
	// Use this for initialization
	void Start () {
		_started = false;
		_sys = TmSystem.instance;
		_sys.soundCall(TmSystem.SOUND_CH.BGM, bgmClip, 1.0f, false);
		_stageId = 1;
		if(PlayerPrefs.HasKey(STAGE_ID_KEY)){
			_stageId = PlayerPrefs.GetInt(STAGE_ID_KEY);
		}

		renderer.material = stageMat[_stageId-1];

		if((_stageId==1)&&(gameStartPanelPrefab!=null)){
			_startObj = GameObject.Instantiate(gameStartPanelPrefab) as GameObject;
		}
		Debug.Log("stage"+_stageId+" start");
	}
	
	// Update is called once per frame
	void Update () {
		if(!_started){
			if(_startObj==null){
				_sys.soundCall(TmSystem.SOUND_CH.VOICE, stageVoiceClip[_stageId-1], 1.0f, false);
				moleController.SendMessage("SM_toStart");
				_started = true;
			}
		}
	}
	
	// "0000000000" 一つもクリアしていない状態 
	// "1240000000" 1st:A 2st:B 3st:C それ以外クリアなし 
	// "7600000000" 1st:ABC 2st:BC それ以外クリアなし 
	public static bool[] getRankFlag(string _rankGetStr=""){
		bool[] rankFlag;
		if(_rankGetStr==""){
			if(PlayerPrefs.HasKey(gameScript.RANK_GET_KEY)){
				_rankGetStr = PlayerPrefs.GetString(gameScript.RANK_GET_KEY);
			}
			if(_rankGetStr==""){
				_rankGetStr = "0000000000";
			}			
		}

		rankFlag = new bool[_rankGetStr.Length*3];
		char[] chars = _rankGetStr.ToCharArray();
		for(int iy = 0; iy < chars.Length; ++iy){
			int bts = int.Parse(chars[iy].ToString());
			for(int ix = 0; ix < 3; ++ix){
				rankFlag[iy*3+ix] = (((bts >> ix)&0x01)==1);
			}
		}
		return rankFlag;
	}
	
	public static string setRankStr(bool[] _rankFlag=null){
		string rankGetStr="";
		if(_rankFlag==null){
			if(PlayerPrefs.HasKey(gameScript.RANK_GET_KEY)){
				rankGetStr = PlayerPrefs.GetString(gameScript.RANK_GET_KEY);
			}
			if(rankGetStr!=""){
				rankGetStr = "0000000000";
				PlayerPrefs.SetString(gameScript.RANK_GET_KEY,rankGetStr);
			}			
		}else{
			for(int iy = 0; iy < _rankFlag.Length/3; ++iy){
				int chr = 0;
				for(int ix = 0; ix < 3; ++ix){
					chr += ((_rankFlag[iy*3+ix]?1:0) << ix);
				}
				rankGetStr += chr.ToString();
			}
			PlayerPrefs.SetString(gameScript.RANK_GET_KEY,rankGetStr);
		}
		return rankGetStr;
	}
	
}
