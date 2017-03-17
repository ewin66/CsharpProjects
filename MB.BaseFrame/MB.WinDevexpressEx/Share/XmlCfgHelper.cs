//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	XmlCfgHelper ����������� XML������Ϣ������ء�
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Xml;

namespace MB.XWinLib.Share
{
	/// <summary>
	/// XmlCfgHelper ����������� XML������Ϣ������ء�
	/// </summary>
	public class XmlCfgHelper
	{
		//Pivot grid ��
		private static readonly string PIVOT_GRID_COLUMN_NODE = "/Entity/PivotGrid/Column";

        #region Instance...
        private static Object _Obj = new object();
        private static XmlCfgHelper _Instance;

        /// <summary>
        /// ����һ��protected �Ĺ��캯������ֹ�ⲿֱ�Ӵ�����
        /// </summary>
        protected XmlCfgHelper() { }

        /// <summary>
        /// ���̰߳�ȫ�ĵ�ʵ��ģʽ��
        /// ���ڶ��⹫������ʵ�ַ�����ʹ��SingletionProvider �ĵ�ʱ�����С�
        /// </summary>
        public static XmlCfgHelper Instance {
            get {
                if (_Instance == null) {
                    lock (_Obj) {
                        if (_Instance == null)
                            _Instance = new XmlCfgHelper();
                    }
                }
                return _Instance;
            }
        }
        #endregion Instance...


		#region public ����...
		/// <summary>
		/// ��ȡ������XML�ļ��еĲ������ݡ�
		/// </summary>
		/// <param name="xmlFileName"></param>
		/// <returns></returns>
		public XWinLib.PivotGrid.ColPivotList GetPivotGridCfgData(string xmlFileName){

            string xmlFileFullName = MB.WinBase.ShareLib.Instance.BuildXmlConfigFileFullName(xmlFileName);
 
			XWinLib.PivotGrid.ColPivotList pivotCols = new XWinLib.PivotGrid.ColPivotList();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileFullName);  
			XmlNodeList nodeList = xmlDoc.SelectNodes(PIVOT_GRID_COLUMN_NODE);
			
			XWinLib.PivotGrid.ColPivotList pivotList = new XWinLib.PivotGrid.ColPivotList();
			//�����е���Ϣ
			loadPivotList(pivotList,nodeList);

			return pivotList;
		}
		#endregion static public ����...

		#region �ڲ���������...
		//����PivotList �е���Ϣ
		private void loadPivotList(XWinLib.PivotGrid.ColPivotList pivotList,XmlNodeList nodeList){
			if(nodeList.Count==0)
				return;
			foreach(XmlNode node in nodeList){
				if(node.NodeType != XmlNodeType.Element)
					continue;
				if(node.Attributes["FieldName"]==null)
					continue;

				string fieldName = node.Attributes["FieldName"].Value;
				XWinLib.PivotGrid.ColPivotInfo info = new XWinLib.PivotGrid.ColPivotInfo(fieldName);

				if(node.Attributes["Description"]!=null)
					info.Description =  node.Attributes["Description"].Value;

				if(node.Attributes["AllowedAreas"]!=null)
					info.AllowedAreas = getAllowAreasByString(node.Attributes["AllowedAreas"].Value);
				else
					info.AllowedAreas = DevExpress.XtraPivotGrid.PivotGridAllowedAreas.All;

				if(node.Attributes["IniArea"]!=null)
					info.IniArea = (DevExpress.XtraPivotGrid.PivotArea)Enum.Parse(typeof(DevExpress.XtraPivotGrid.PivotArea),node.Attributes["IniArea"].Value);
				else
					info.IniArea = DevExpress.XtraPivotGrid.PivotArea.FilterArea ;

				if(node.Attributes["AreaIndex"]!=null)
					info.AreaIndex = MB.Util.MyConvert.Instance.ToInt( node.Attributes["AreaIndex"].Value);
				if(node.Attributes["GroupInterval"]!=null)
					info.GroupInterval = (DevExpress.XtraPivotGrid.PivotGroupInterval)Enum.Parse(typeof(DevExpress.XtraPivotGrid.PivotGroupInterval),node.Attributes["GroupInterval"].Value);
				if(node.Attributes["TopValueCount"]!=null)
					info.TopValueCount = MB.Util.MyConvert.Instance.ToInt( node.Attributes["TopValueCount"].Value);

				if(node.Attributes["DragGroupName"]!=null)
					info.DragGroupName =  node.Attributes["DragGroupName"].Value ;

				if(node.Attributes["DragGroupIndex"]!=null)
					info.DragGroupIndex = MB.Util.MyConvert.Instance.ToInt( node.Attributes["DragGroupIndex"].Value);

				if(node.Attributes["SummaryItemType"]!=null)
					info.SummaryItemType = node.Attributes["SummaryItemType"].Value ;

                if (node.Attributes["Expression"] != null)
                    info.Expression = node.Attributes["Expression"].Value;

                if (node.Attributes["ExpressionSourceColumns"] != null)
                    info.ExpressionSourceColumns = node.Attributes["ExpressionSourceColumns"].Value;

                if (!string.IsNullOrEmpty(info.Expression))
                {
                    //�ڱ��ʽ�ж����ռλ������
                    var placeInExpressionCount = System.Text.RegularExpressions.Regex.Matches(info.Expression, @"\{\d\}").Count;

                    //�Զ����SourceColumn�ĸ�������Ҫ��Expression��ռλ���ĸ��������
                    var sourceColumnCount = string.IsNullOrEmpty(info.ExpressionSourceColumns) ? 0 : info.ExpressionSourceColumns.Split(',').Length;
                    if (placeInExpressionCount != sourceColumnCount)
                        throw new MB.Util.APPException(string.Format("��{0}��Expression�е�ռλ��ExpressionSourceColumns�ĸ�����ƥ��", fieldName, MB.Util.APPMessageType.SysErrInfo));
                }

				if(node.ChildNodes.Count > 0 ){
					setFormatData(node.ChildNodes,info);
				}
				
				pivotList.Add(info); 
			}
		}
		//�����û����õ���ʾ��ʽ��Ϣ��
		private void setFormatData(XmlNodeList nodeList,XWinLib.PivotGrid.ColPivotInfo info){
			foreach(XmlNode node in nodeList){
				if(node.NodeType != XmlNodeType.Element)
					continue;
				if(string.Compare(node.Name,"DisplayFormat",true)!=0)
					continue;
				DevExpress.Utils.FormatInfo cell = new DevExpress.Utils.FormatInfo();
				foreach(XmlNode childNode in node.ChildNodes){
					if(childNode.NodeType != XmlNodeType.Element)
						continue;
					if(string.Compare(childNode.Name,"FormatType",true)==0)
						cell.FormatType = (DevExpress.Utils.FormatType)Enum.Parse(typeof(DevExpress.Utils.FormatType),childNode.InnerText.Trim());
					else if(string.Compare(childNode.Name,"FormatString",true)==0)
						cell.FormatString = childNode.InnerText.Trim();
					else 
						continue;
					
				}
				info.CellFormat = cell;
                info.ValueFormat = cell;
			}
			
		}
		//�����ַ��ܻ�ȡ���п��������϶�������
		private DevExpress.XtraPivotGrid.PivotGridAllowedAreas getAllowAreasByString(string allAreas){
			if(allAreas==null || allAreas.Length ==0)
				return DevExpress.XtraPivotGrid.PivotGridAllowedAreas.All;
			string[] areas = allAreas.Split(';');
			DevExpress.XtraPivotGrid.PivotGridAllowedAreas areasVal;
			areasVal = (DevExpress.XtraPivotGrid.PivotGridAllowedAreas)Enum.Parse(typeof(DevExpress.XtraPivotGrid.PivotGridAllowedAreas),areas[0]);
			if(areas.Length > 1){
				for(int i =1; i < areas.Length; i++){
					areasVal = areasVal | (DevExpress.XtraPivotGrid.PivotGridAllowedAreas)Enum.Parse(typeof(DevExpress.XtraPivotGrid.PivotGridAllowedAreas),areas[i]);
				}
			}
			return areasVal;
		}
		#endregion �ڲ���������...
 
	}
}
