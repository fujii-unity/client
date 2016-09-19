using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IMF
{

    public class MainController : SingletonMonoBehaviour<MainController>
    {
        private const int MAP_SIZE_WIDTH = 64;
        private const int MAP_SIZE_HEIGHT = 64;

        private CSVReader _mapReader = null;

        private readonly Vector3 _initPos = new Vector3(-400,260);
        private readonly Vector3 _initScale = Vector3.one;
        private readonly Quaternion _verticallyRotate = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        [SerializeField]
        private Canvas _mapCanvas = null;
        [SerializeField]
        private GameObject _stationObject = null;
        [SerializeField]
        private GameObject _roadObject = null;
        [SerializeField]
        private GameObject _plusObject = null;
        [SerializeField]
        private GameObject _minusObject = null;
        [SerializeField]
        private GameObject _goalPoint = null;
        [SerializeField]
        private PlayerControll[] _playerArray = new PlayerControll[GameDefine.MAX_PLAYER_NUM];
        [SerializeField]
        private Dice _dice = null;
        [SerializeField]
        private Camera _mapCamera = null;
        [SerializeField]
        private Camera _playerCamera = null;

        private int _nowPlayerIndex = 0;
        private int _activePlayerNum = 0;
        private GameDefine.eTurnState _turnState = GameDefine.eTurnState.Wait;

        public void Init()
        {
            CreateMapObject();
        }

        private void CreateMapObject()
        {
            //マップを読み込み
            _mapReader = new CSVReader();
            _mapReader.Load(GameDefine.MAP_CSV_NAME);

            //マップを作成する（非同期にしたいが夢のまた夢）
            List<List<int>> mapInfoList = _mapReader.dataList;
            int maxRow = mapInfoList.Count;
            Transform mapCanvasTransform = _mapCanvas.transform;
            for (int i = 0; i < maxRow; ++i)
            {
                for(int j = 0; j < mapInfoList[i].Count; ++j)
                {
                    switch ((GameDefine.eMapType)mapInfoList[i][j])
                    {
                        case GameDefine.eMapType.Road:
                            GameObject roadObject = Instantiate(_roadObject) as GameObject;
                            SetTransform(roadObject, i, j, mapCanvasTransform);
                            if (i - 1 < 0 || mapInfoList[i - 1][j] == (int)GameDefine.eMapType.None)
                            {
                                continue;
                            }
                            //縦向きなので90度回転させる
                            roadObject.transform.localRotation = _verticallyRotate;
                            break;
                        case GameDefine.eMapType.Station:
                            GameObject stationObject = Instantiate(_stationObject) as GameObject;
                            SetTransform(stationObject, i, j, mapCanvasTransform);
                            break;
                        case GameDefine.eMapType.Plus:
                            GameObject plusObject = Instantiate(_plusObject) as GameObject;
                            SetTransform(plusObject, i, j, mapCanvasTransform);
                            break;
                        case GameDefine.eMapType.Minus:
                            GameObject minusObject = Instantiate(_minusObject) as GameObject;
                            SetTransform(minusObject, i, j , mapCanvasTransform);
                            break;
                        case GameDefine.eMapType.Player1Point:
                        case GameDefine.eMapType.Player2Point:
                            _activePlayerNum++;
                            int playerIndex = mapInfoList[i][j] - (int)GameDefine.eMapType.Player1Point;
                            SetTransform(_playerArray[playerIndex].gameObject, i, j, mapCanvasTransform,false);
                            _playerArray[playerIndex].Init(i, j, _mapReader);
                            _playerArray[playerIndex].SetMoveCallback(FinishPlayerMove);
                            //青マススタートにする
                            GameObject playerBaseObject = Instantiate(_plusObject) as GameObject;
                            SetTransform(playerBaseObject, i, j, mapCanvasTransform);
                            break;
                        case GameDefine.eMapType.GoalPoint:
                            //ゴールは初期配置の物のポジションを動かすだけでよい 
                            SetTransform(_goalPoint, i, j, mapCanvasTransform);
                            break;
                        case GameDefine.eMapType.None:
                        default:
                            continue;
                    }
                }
            }

            for(int i = _activePlayerNum; i < GameDefine.MAX_PLAYER_NUM; ++i)
            {
                _playerArray[i].gameObject.SetActive(false);
            }

            //コピー元は消す
            _roadObject.SetActive(false);
            _stationObject.SetActive(false);
            _plusObject.SetActive(false);
            _minusObject.SetActive(false);

        }

        private void SetTransform(GameObject baseObject ,int height, int width, Transform parentTransform = null, bool setScale = true)
        {
            Transform baseTransform = baseObject.transform;
            if(parentTransform != null)
            {
                baseTransform.SetParent(parentTransform);
            }
            if (setScale)
            {
                baseTransform.localScale = _initScale;
            }
            Vector3 basePos = baseTransform.localPosition;
            basePos.x = _initPos.x + MAP_SIZE_WIDTH * width;
            basePos.y = _initPos.y - MAP_SIZE_HEIGHT * height;
            baseTransform.localPosition = basePos;
        }

        public Vector3 GetPosition(int height,int width)
        {
            Vector3 basePos = Vector3.zero;
            basePos.x = _initPos.x + MAP_SIZE_WIDTH * width;
            basePos.y = _initPos.y - MAP_SIZE_HEIGHT * height;
            return basePos;
        }

        public void GameStart()
        {
            _nowPlayerIndex = 0;
            SetStartPlayerState();
        }

        // Update is called once per frame
        void Update()
        {
            switch (_turnState)
            {
                case GameDefine.eTurnState.Wait:
                default:
                    break;
                case GameDefine.eTurnState.Dice:
                    if (Input.GetMouseButtonDown(0))
                    {
                        DecideDice();
                    }
                    break;
                case GameDefine.eTurnState.PlayerAct:
                    break;
                case GameDefine.eTurnState.Event:
                    break;
            }
        }

        private void SetStartPlayerState()
        {
            //_playerCamera.transform.localPosition = _playerArray[_nowPlayerIndex].transform.localPosition;
            _turnState = GameDefine.eTurnState.Dice;
            _dice.StartAnimation();
        }

        private void DecideDice()
        {
            _dice.DecideDice();
            _playerArray[_nowPlayerIndex].StartMove(_dice.diceIndex);
            _turnState = GameDefine.eTurnState.PlayerAct;
        }

        public void FinishPlayerMove(int moveIndex)
        {
            if(moveIndex < 0)
            {
                _turnState = GameDefine.eTurnState.Event;
                ActEvent();
            }
            else
            {
                _dice.SetImage(moveIndex);
            }
        }

        private void ActEvent()
        {
            //今はイベントないのでそのまま次のプレイヤーへ
            ChangePlayerIndex();
            SetStartPlayerState();
        }

        void ChangePlayerIndex()
        {
            _nowPlayerIndex++;
            if(_nowPlayerIndex >= _activePlayerNum)
            {
                _nowPlayerIndex = 0;
            } 
        }
    }
}
