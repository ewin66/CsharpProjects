//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	StyleCfgList UI Style �������ض������ࡣ
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Xml;

namespace MB.XWinLib.Share
{
	/// <summary>
	/// StyleCfgList UI Style �������ض������ࡣ
	/// </summary>
	public class StyleCfgList : System.Collections.Generic.Dictionary<string,StyleCfgInfo>{
		#region ��������...
		private static readonly string STYLE_CONFIG_NODE="/AppConfig/Styles/Style";
		public static  string DEFAULT_CFG_NAME = "AppStylesConfig";
		//�洢�Ѿ���ȡ����ʽ���ݡ�һ����˵��һ��Ӧ�ó�������ֻҪ��һ��Style �Ϳ����ˡ�
		private static StyleCfgList _DataStore; 
		#endregion ��������...

		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		protected StyleCfgList(string cfgFileName) {
			createFromcfg(cfgFileName);
		}
		#endregion ���캯��...

		#region Public Static ����...
		/// <summary>
		/// ʵ����һ����ʽ�ļ����ࡣ����������Ϊ��ʵ�ֵ���ģʽ��
		/// </summary>
		/// <returns></returns>
		public static StyleCfgList CreateInstance(){
			return CreateInstance(DEFAULT_CFG_NAME);
		}
		/// <summary>
		///  ʵ����һ����ʽ�ļ����ࡣ����������Ϊ��ʵ�ֵ���ģʽ��
		/// </summary>
		/// <param name="cfgFileName"></param>
		/// <returns></returns>
		public static StyleCfgList CreateInstance(string cfgFileName){
			if(_DataStore==null)
				_DataStore = new StyleCfgList(cfgFileName);
			return _DataStore;
		}

		#endregion Public Static ����...


		#region public ����...
		/// <summary>
		/// add
		/// </summary>
		/// <param name="colInfo"></param>
		/// <returns></returns>
		public StyleCfgInfo Add(StyleCfgInfo colInfo){
			base.Add(colInfo.Name,colInfo); 

			return colInfo;
		}
		#endregion public ����...

		#region �ڲ���������...
		//�������ļ��л�ȡ��Ϣ��
		private void createFromcfg(string fileName){
            XmlDocument xmlDoc = MB.WinBase.ShareLib.Instance.LoadXmlConfigFile(fileName);
            if (xmlDoc == null)
				return ;
            XmlNodeList nodeList = xmlDoc.SelectNodes(STYLE_CONFIG_NODE);
			if(nodeList==null)
				return ;
			foreach(XmlNode node in nodeList){
				if(node.NodeType != XmlNodeType.Element)
					continue;
				if(node.Attributes["Name"]==null)
					continue;
				string name = node.Attributes["Name"].Value;
				StyleCfgInfo info = new StyleCfgInfo(name);
				
				fillInfo(info,node);

				this.Add(info);
			}
		}
		//��ȡ�ڵ�
		private void fillInfo(StyleCfgInfo info,XmlNode xmlNode){
			if(xmlNode.ChildNodes.Count==0)
				return;
			foreach(XmlNode node in xmlNode.ChildNodes){
				if(node.NodeType != XmlNodeType.Element)
					continue;
				string text = node.InnerText.Trim();
				switch(node.Name){
					case "BackColor":
						info.BackColor = System.Drawing.Color.FromName(text) ;
						break;
					case "ForeColor":
						info.ForeColor = System.Drawing.Color.FromName(text) ;
						break;
					case "Font":
						info.Font = getFont(node);
						break;
					default:
						MB.Util.TraceEx.Write("�ڵ�" + node.Name + "��û�н��д�����ȷ����Ӧ��XML�ļ������Ƿ���ȷ�������ִ�Сд��");  
						break;
				}
			}
		}
		//�������õõ�����
		private System.Drawing.Font getFont(XmlNode fontNode){
			float size = 9f;
			System.Drawing.FontStyle styles = System.Drawing.FontStyle.Regular; 
			
			foreach(XmlNode node in fontNode.ChildNodes){
				if(node.NodeType!= XmlNodeType.Element)
					continue;
				string text = node.InnerText.Trim();
                
				switch(node.Name){
					case "Size":
						size = MB.Util.MyConvert .Instance.ToFloat(text,2);  
						break;
					case "Bold":
						if(MB.Util.MyConvert .Instance.ToBool(text))
							styles = styles | System.Drawing.FontStyle.Bold;
						break;
					case "Italic":
						if(MB.Util.MyConvert .Instance.ToBool(text))
							styles = styles | System.Drawing.FontStyle.Italic;
						break;
					case "Strikeout":
						if(MB.Util.MyConvert .Instance.ToBool(text)) 
							styles = styles | System.Drawing.FontStyle.Strikeout;
						break;
					case "UndeLine":
						if(MB.Util.MyConvert .Instance.ToBool(text))
							styles = styles | System.Drawing.FontStyle.Underline;
						break;
					default:
						MB.Util.TraceEx.Write("�ڵ�" + node.Name + "��û�н��д�����ȷ����Ӧ��XML�ļ������Ƿ���ȷ�������ִ�Сд��");  
						break;
				}

			}
			System.Drawing.Font font = new System.Drawing.Font("Microsoft Sans Serif",size,styles);
			return font;
		}
		#endregion �ڲ���������...
	}
}
