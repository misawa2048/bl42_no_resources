using UnityEngine;
using System.Collections;

public class gameStartPlaneScript : MonoBehaviour {
	public AudioClip[] startVoice;
	private TmSystem _sys;
	private bool _isMoveEnd;
	private Vector3 startPos;
	public bool isMoveEnd{ get{ return _isMoveEnd; } }
	private int _rno;
	private float _Timer;

	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		
		_rno = 0;
		_Timer = 0.0f;
		_isMoveEnd = false;
		startPos = gameObject.transform.position;
		startPos.x -= transform.localScale.x * 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		switch(_rno){
		case 0:
			_Timer += Time.deltaTime;
			if(_Timer > 2.0f){
				_rno++;
				_Timer = 0.0f;
				_sys.soundCall(TmSystem.SOUND_CH.VOICE, startVoice[Random.Range(0,startVoice.Length)], 1.0f, false);
			}
			break;
		case 1:
			_Timer += Time.deltaTime;
			if(_Timer > 2.0f){
				_rno++;
			}
			break;
		case 2:
			{
				Vector3 diff = startPos - gameObject.transform.position;
				gameObject.transform.position += diff * Time.deltaTime * 2.5f;
				if(diff.magnitude < 0.1f){
					_isMoveEnd = true;
					_rno++;
				}
			}
			break;
		case 3:
			Destroy(gameObject);
			break;
		}
	}
}
