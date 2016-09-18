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
        private GameObject _goalPoint = null;
        [SerializeField]
        private Camera _mapCamera = null;
        [SerializeField]
        private Camera _playerCamera = null;

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
                            Transform roadTransform = roadObject.transform;
                            roadTransform.SetParent(mapCanvasTransform);
                            roadTransform.localScale = _initScale;
                            Vector3 roadPos = roadTransform.localPosition;
                            roadPos.x = _initPos.x + MAP_SIZE_WIDTH * j;
                            roadPos.y = _initPos.y - MAP_SIZE_HEIGHT * i;
                            roadTransform.localPosition = roadPos;
                            if(i - 1 < 0 || mapInfoList[i - 1][j] == (int)GameDefine.eMapType.None)
                            {
                                continue;
                            }
                            //縦向きなので90度回転させる
                            roadTransform.localRotation = _verticallyRotate;

                            break;
                        case GameDefine.eMapType.Station:
                            GameObject stationObject = Instantiate(_stationObject) as GameObject;
                            Transform stationTransform = stationObject.transform;
                            stationTransform.SetParent(mapCanvasTransform);
                            stationTransform.localScale = _initScale;
                            Vector3 stationPos = stationTransform.localPosition;
                            stationPos.x = _initPos.x + MAP_SIZE_WIDTH * j;
                            stationPos.y = _initPos.y - MAP_SIZE_HEIGHT * i;
                            stationTransform.localPosition = stationPos;
                            break;
                        case GameDefine.eMapType.GoalPoint:
                            //ゴールは初期配置の物のポジションを動かすだけでよい                            
                            Transform goalTransform = _goalPoint.transform;
                            goalTransform.SetParent(mapCanvasTransform);
                            goalTransform.localScale = _initScale;
                            Vector3 goalPos = goalTransform.localPosition;
                            goalPos.x = _initPos.x + MAP_SIZE_WIDTH * j;
                            goalPos.y = _initPos.y - MAP_SIZE_HEIGHT * i;
                            goalTransform.localPosition = goalPos;
                            break;
                        case GameDefine.eMapType.Player1Point:
                            break;
                        case GameDefine.eMapType.None:
                        default:
                            continue;
                    }
                }
            }

            //コピー元は消す
            _roadObject.SetActive(false);
            _stationObject.SetActive(false);

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
