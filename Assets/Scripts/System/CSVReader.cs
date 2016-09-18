using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace IMF
{
public class CSVReader {

		/// <summary>
		/// セルを区切る為にキャラ
		/// </summary>
		public const char _SPLIT_CHAR = ',';

		// 読み込んだデータ
		List<List<int>> m_data = null;
		TextAsset m_textAsset = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="comment">コメントの文字列</param>
		/// <param name="isReadFromDisc">Resourcesでなくディスク(フルパス)から読み込むか</param>
		public CSVReader()
		{
			m_textAsset = null;
			m_data = new List<List<int>>();
		}

		private TextReader CreateTextReader(string fileName)
		{
			m_textAsset = Resources.Load<TextAsset>(fileName);
			return new StringReader(m_textAsset.text);
		}

		public bool Load(string fileName) 
		{            
			m_data.Clear();
			TextReader reader = CreateTextReader(fileName);

			int counter = 0;
			string line = "";
			while ( ( line = reader.ReadLine()) != null ) 
			{
				// 今の列をマス毎に区切る
				string[] fields = line.Split( _SPLIT_CHAR );
				m_data.Add( new List<int>() );

				foreach ( var field in fields )
				{
					if (field == "")
					{
						continue;    
					}
					m_data[ counter ].Add( int.Parse(field) );
				}
				counter++;
			}
			// 読み込んだリソースを開放する
			Resources.UnloadAsset(m_textAsset);
			m_textAsset = null;
			Resources.UnloadUnusedAssets();
			return true;
		}
	}
}