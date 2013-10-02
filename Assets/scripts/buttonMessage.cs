using UnityEngine;
using System.Collections;

public class buttonMessage : MonoBehaviour {
	public GameObject loadingPrefab;
	public Material buttonOKMaterial;
	public Material buttonNGMaterial;
	private TmSystem _sys;
	private GameObject mBaseObj;
	private int mToStageId = -1;
	private bool mEnabled = false;
	
	void Awake(){
		mBaseObj = gameObject.transform.GetChild(0).gameObject;
	}
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		mBaseObj.collider.enabled = mEnabled;
	}
	
	// Update is called once per frame
	void Update () {
		if(mEnabled){
			if(_sys.mw.isButtonState(TmMouseWrapper.STATE.UP)){
				if(_sys.mw.hitTarget==mBaseObj){
					if((mToStageId>0) && (loadingPrefab!=null)){
						PlayerPrefs.SetInt(gameScript.STAGE_ID_KEY,mToStageId);
						PlayerPrefs.SetString(gameScript.AFTER_RESULT_SCENE_KEY,"record");
						GameObject ldObj = GameObject.Instantiate(loadingPrefab) as GameObject;
						ldObj.SendMessage("SM_setNextSceneStr","game");
					}
				}
			}
		}
	}

	private void SM_setMaterial(Material _mat){
		gameObject.transform.GetChild(0).renderer.material = _mat;
	}
	private void SM_setToStageId(int _id){
		mToStageId = _id;
		string mes = "シーン"+_id.ToString();
		gameObject.GetComponent<TextMesh>().text = mes;
	}
	private void SM_setButtonEnable(bool _state){
		mEnabled = _state;
		mBaseObj.collider.enabled = mEnabled;
		mBaseObj.renderer.material = _state ? buttonOKMaterial : buttonNGMaterial;
	}
}
