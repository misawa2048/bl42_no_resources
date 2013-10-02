using UnityEngine;
using System.Collections;

public class moleController : MonoBehaviour {
	private const float ADD_ZOFS = 0.00001f;
	public enum MOLE_TYPE{
		AUTO,
		NORMAL,
		SP,
		SP2,
	};
	public GameObject gameObj;
	public GameObject[] molePrefab;
	public GameObject hitAreaObj;
	public GameObject generateAreaObj;
	public GameObject loadingPrefab;
	public int startStock =30;
	public int maxMolesOnScreen =10;
	public float scoreCountUpSpeed = 10.0f;
	public string clearSceneStr = "result";
	public float maxIntervalTick=2.0f;
	public float minIntervalTick=0.1f;
	public AudioClip gameClearClip;

	private TmSystem _sys;
	private bool mIsStart;
	public bool isStart { get{ return mIsStart; } }
	private bool mIsFinish;
	public bool isFinish { get{ return mIsFinish; } }
	GameObject[] mBonusAreaGos;
	private int mRno0;
	private int mStockNum;
	private float mTickTimer;
	private float mIntervalTick;
	private int mSpecialObjId;
	private float mMoleCount;
	private float mFireworksNum=5;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		
		mIsStart = false;
		mIsFinish = false;
		mTickTimer = 0.0f;
		mIntervalTick = maxIntervalTick;
		if(startStock<=0) startStock = 1;
		mStockNum = startStock;
		mRno0 = 0;

		Transform[] trs = hitAreaObj.transform.GetComponentsInChildren<Transform>();
		mBonusAreaGos = new GameObject[trs.Length];
		for(int ii = 0; ii < trs.Length; ++ii){
			mBonusAreaGos[ii] = trs[ii].gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch(mRno0){
			case 0: if( !move00() ){ mRno0++; }  break;
			case 1: if( !move01() ){ mRno0++; }  break;
			case 2: if( !move02() ){ mRno0++; }  break;
			case 3: if( !move03() ){ mRno0++; }  break;
		}
	}
	
	//------------------------------------------
	private bool move00(){
		bool ret = true;
		if(mIsStart){
			mTickTimer = 0.0f;
			ret = false;
		}
		return ret;
	}
	//------------------------------------------
	private bool move01(){
		bool ret = true;
		mTickTimer += Time.deltaTime;
		
		if(_sys.mw.isButtonState(TmMouseWrapper.STATE.DOWN)){
			if((_sys.mw.isHover(_sys.mw.hitTarget))&&(_sys.mw.hitTarget.tag=="tagMole")){
				_sys.mw.hitTarget.SendMessage("SM_onMouseDown",10.0f);
			}
		}else if(_sys.mw.isButtonState(TmMouseWrapper.STATE.ON)){
			if((_sys.mw.isEnter(_sys.mw.hitTarget))&&(_sys.mw.hitTarget.tag=="tagMole")){
				_sys.mw.hitTarget.SendMessage("SM_onMouseDown",10.0f);
			}
		}
		
		if(mStockNum>0){
			if(mTickTimer > mIntervalTick){
				mTickTimer -= mIntervalTick;
				mIntervalTick = Random.Range(minIntervalTick,maxIntervalTick);
				GameObject[] gos = GameObject.FindGameObjectsWithTag("tagMole");
				if(gos.Length < maxMolesOnScreen){
					float zOfs = (float)(mStockNum-startStock)*ADD_ZOFS;
					if(addMole(MOLE_TYPE.AUTO,zOfs)!=null){
						mStockNum--;
					}
				}
			}
		}else{
			GameObject[] gos = GameObject.FindGameObjectsWithTag("tagMole");
			if(gos.Length==0){
				mTickTimer = 0.0f;
				ret = false;
			}
		}
		return ret;
	}
	//------------------------------------------
	private bool move02(){
		mTickTimer = 0.0f;
		mIsFinish = true;
		_sys.soundStop(TmSystem.SOUND_CH.BGM);
		_sys.soundCall(TmSystem.SOUND_CH.SE, gameClearClip, 1.0f, false);
		return false;
	}
	//------------------------------------------
	private bool move03(){
		bool ret = true;
		mTickTimer += Time.deltaTime;
		if(mTickTimer>2.0f){
			mTickTimer = 0.0f;
			GameObject loadingObj = GameObject.Instantiate(loadingPrefab) as GameObject;
			loadingObj.SendMessage("SM_setNextSceneStr",clearSceneStr);
			ret = false;
		}
		return ret;
	}
	//------------------------------------------
	private GameObject addMole(MOLE_TYPE _type, float _zofs=0.0f){
		Vector3 pos;
		int timeoutCnt = 0;
		if(_type == MOLE_TYPE.AUTO){
			_type = (mStockNum==1) ? MOLE_TYPE.SP : MOLE_TYPE.NORMAL;
		}
		string useAreaStr;
		switch(_type){
			default:
			case MOLE_TYPE.NORMAL:  useAreaStr = "useArea"; break;
			case MOLE_TYPE.SP:      useAreaStr = "bonusArea"; break;
			case MOLE_TYPE.SP2:     useAreaStr = "useArea"; break;
		}
		do{
			pos = new Vector3(Random.value-0.5f,Random.value-0.5f,_zofs);
			pos.Scale(generateAreaObj.transform.lossyScale);
			pos += generateAreaObj.transform.position;
			timeoutCnt++;
		}while( !isInArea(pos,useAreaStr) && (timeoutCnt<100) );
		
		Quaternion rot = Quaternion.AngleAxis(Random.value*360.0f,Vector3.forward);
		GameObject pref;
		switch(_type){
			default:
			case MOLE_TYPE.NORMAL:  pref = molePrefab[0]; break;
			case MOLE_TYPE.SP:      pref = molePrefab[1]; break;
			case MOLE_TYPE.SP2:     pref = molePrefab[2]; break;
		}
		GameObject moleObj = Instantiate(pref,pos,rot) as GameObject;
		moleObj.transform.parent = transform;
		return moleObj;
	}
	
	//------------------------------------------
	private bool isInArea(Vector3 _pos, string _name=""){
		bool ret = false;
		foreach(GameObject bgo in mBonusAreaGos){
			if((_name=="")||bgo.name==_name){
				Vector3 iPos = bgo.transform.InverseTransformPoint(_pos);
				if(Mathf.Abs(iPos.x)<0.5f){
					if(Mathf.Abs(iPos.y)<0.5f){
						ret = true;
						break;
					}
				}
			}
		}
		return ret;
	}

	//------------------------------------------
	//------------------------------------------
	private void SM_toStart(){
		mIsStart = true;
	}
}
