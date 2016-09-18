using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace IMF
{
    public class CSVReader
    {

        /// <summary>
        /// セルを区切る為にキャラ
        /// </summary>
        public const char _SPLIT_CHAR = ',';

        // 読み込んだデータ
        List<List<int>> _dataList = null;
        public List<List<int>> dataList { get { return _dataList; } }

        TextAsset _textAsset = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="comment">コメントの文字列</param>
        /// <param name="isReadFromDisc">Resourcesでなくディスク(フルパス)から読み込むか</param>
        public CSVReader()
        {
            _textAsset = null;
            _dataList = new List<List<int>>();
        }

        private TextReader CreateTextReader(string fileName)
        {
            _textAsset = Resources.Load<TextAsset>(fileName);
            return new StringReader(_textAsset.text);
        }

        public bool Load(string fileName)
        {
            _dataList.Clear();
            TextReader reader = CreateTextReader(fileName);

            int counter = 0;
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                // 今の列をマス毎に区切る
                string[] fields = line.Split(_SPLIT_CHAR);
                _dataList.Add(new List<int>());

                foreach (var field in fields)
                {
                    if (field == "")
                    {
                        _dataList[counter].Add((int)GameDefine.eMapType.None);
                        continue;
                    }
                    _dataList[counter].Add(int.Parse(field));
                }
                counter++;
            }
            // 読み込んだリソースを開放する
            Resources.UnloadAsset(_textAsset);
            _textAsset = null;
            Resources.UnloadUnusedAssets();
            return true;
        }
    }
}