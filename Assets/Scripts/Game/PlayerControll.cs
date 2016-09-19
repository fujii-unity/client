using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IMF
{
    public class PlayerControll : MonoBehaviour
    {
        private enum eAnimeState
        {
            Wait,
            InputWait,
            Move,
        }

        private enum eMoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        private const float MOVE_WAIT_TIME = 0.1f;
        private const int MOVE_Y_LIST_OFFSET = 10000;

        private int _moveNum = -1;
        public int moveNum { get { return _moveNum; } set { _moveNum = value; } }
        private int _yIndex = -1;
        public int yIndex { get { return _yIndex; } }
        private int _xIndex = -1;
        public int xIndex { get { return _xIndex; } }

        private System.Action<int> _moveCallback = null;
        private bool _isMoveAnimation = false;
        private bool _isBack = false;
        private Transform _playerTransform = null;
        private List<int> _movedList = null;

        private float _moveAnimeTime = 0.0f;

        private eAnimeState _moveState = eAnimeState.Wait;
        private CSVReader _mapReader = null;

        public void Init(int xInit, int yInit, CSVReader mapReader)
        {
            _xIndex = xInit;
            _yIndex = yInit;
            _mapReader = mapReader;
            _playerTransform = this.transform;
            _movedList = new List<int>();
        }

        public void StartMove(int moveNum)
        {
            _isBack = false;
            _moveNum = moveNum;
            _isMoveAnimation = true;
            _moveState = eAnimeState.InputWait;
            _movedList.Clear();
            _movedList.Add(_xIndex + _yIndex * MOVE_Y_LIST_OFFSET);
        }

        public void SetMoveCallback(System.Action<int> callback)
        {
            _moveCallback = callback;
        }

        private void Update()
        {
            if (!_isMoveAnimation)
            {
                return;
            }
            switch (_moveState)
            {
                case eAnimeState.InputWait:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (_xIndex + 1 >= _mapReader.dataList.Count || !CheckMove(_xIndex + 1, yIndex))
                        {
                            break;
                        }
                        MoveAction(eMoveDirection.Down);
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (_xIndex - 1 < 0 || !CheckMove(_xIndex - 1, yIndex))
                        {
                            break;
                        }
                        MoveAction(eMoveDirection.Up);
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (_yIndex - 1 < 0 || !CheckMove(_xIndex, _yIndex - 1))
                        {
                            break;
                        }
                        MoveAction(eMoveDirection.Left);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (_yIndex + 1 >= _mapReader.dataList[_xIndex].Count || !CheckMove(_xIndex, yIndex + 1))
                        {
                            break;
                        }
                        MoveAction(eMoveDirection.Right);
                    }
                    break;
                case eAnimeState.Move:
                    _moveAnimeTime += Time.deltaTime;
                    if (_moveAnimeTime >= MOVE_WAIT_TIME)
                    {
                        if (_isBack)
                        {
                            _moveNum++;
                        }
                        else
                        {
                            _moveNum--;
                        }
                        _isBack = false;
                        if (_moveNum < 0)
                        {
                            _moveState = eAnimeState.Wait;
                        }
                        else
                        {
                            _moveState = eAnimeState.InputWait;
                        }
                        _playerTransform.localPosition = MainController.instance.GetPosition(_xIndex, _yIndex);
                        _moveCallback(_moveNum);
                    }
                    break;
                default:
                    break;
            }
        }

        private bool CheckMove(int x, int y)
        {
            return _mapReader.dataList[x][y] == (int)GameDefine.eMapType.Road;
        }

        private void MoveAction(eMoveDirection moveDirection)
        {
            int moveIndexX = _xIndex;
            int moveIndexY = _yIndex;
            switch (moveDirection)
            {
                case eMoveDirection.Up:
                    while (true)
                    {
                        moveIndexX--;
                        if (_mapReader.dataList[moveIndexX][moveIndexY] != (int)GameDefine.eMapType.Road)
                        {
                            break;
                        }
                    }
                    break;
                case eMoveDirection.Down:
                    while (true)
                    {
                        moveIndexX++;
                        if (_mapReader.dataList[moveIndexX][moveIndexY] != (int)GameDefine.eMapType.Road)
                        {
                            break;
                        }
                    }
                    break;
                case eMoveDirection.Left:
                    while (true)
                    {
                        moveIndexY--;
                        if (_mapReader.dataList[moveIndexX][moveIndexY] != (int)GameDefine.eMapType.Road)
                        {
                            break;
                        }
                    }
                    break;
                case eMoveDirection.Right:
                    while (true)
                    {
                        moveIndexY++;
                        if (_mapReader.dataList[moveIndexX][moveIndexY] != (int)GameDefine.eMapType.Road)
                        {
                            break;
                        }
                    }
                    break;

            }
            if (_movedList.Contains(moveIndexX + moveIndexY * MOVE_Y_LIST_OFFSET))
            {
                _isBack = true;
                _movedList.Remove(moveIndexX + moveIndexY * MOVE_Y_LIST_OFFSET);
            }
            if (!_isBack)
            {
                _movedList.Add(_xIndex + _yIndex * MOVE_Y_LIST_OFFSET);
            }
            _xIndex = moveIndexX;
            _yIndex = moveIndexY;
            _moveState = eAnimeState.Move;
        }
    }
}