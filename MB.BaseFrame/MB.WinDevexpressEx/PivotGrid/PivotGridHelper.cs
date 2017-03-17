//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	PivotGridHelper PivotGrid ������صĴ�������
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using MB.Util;
using System.Windows.Forms;
using MB.XWinLib.Share;
using MB.XWinLib.XtraGrid; 
namespace MB.XWinLib.PivotGrid
{
	/// <summary>
	/// PivotGridHelper PivotGrid ������صĴ�������
	/// </summary>
	public class PivotGridHelper {
		private static string LAYOUT_XML_PATH = MB.Util.General.GeApplicationDirectory() + @"UserSetting\";
        //add by aifang ֧�ֶ�ά
        private static readonly string GRID_LAYOUT_FILE_SETTING_FULLNAME = LAYOUT_XML_PATH + "GridLayoutSetting.xml";

        #region Instance...
        private static Object _Obj = new object();
        private static PivotGridHelper _Instance;

        /// <summary>
        /// ����һ��protected �Ĺ��캯������ֹ�ⲿֱ�Ӵ�����
        /// </summary>
        protected PivotGridHelper() { }

        /// <summary>
        /// ���̰߳�ȫ�ĵ�ʵ��ģʽ��
        /// ���ڶ��⹫������ʵ�ַ�����ʹ��SingletionProvider �ĵ�ʱ�����С�
        /// </summary>
        public static PivotGridHelper Instance {
            get {
                if (_Instance == null) {
                    lock (_Obj) {
                        if (_Instance == null)
                            _Instance = new PivotGridHelper();
                    }
                }
                return _Instance;
            }
        }
        #endregion Instance...

		#region public  function...
		/// <summary>
		/// �����ݼ��ص�PivotGrid �ؼ��С�
		/// </summary>
		/// <param name="gridCtl">PivotGrid �ؼ���</param>
		/// <param name="dataSource">׼�����ص�����Դ��</param>
		/// <param name="baseRule">��ǰ���ڴ����ҵ���ࡣ</param>
		public void LoadPivotGridData(DevExpress.XtraPivotGrid.PivotGridControl gridCtl,
			object dataSource,MB.WinBase.IFace.IClientRuleQueryBase baseRule,ColPivotList xmlCfgLst){

            addDataToCtl(gridCtl, dataSource, baseRule, xmlCfgLst);
			RestoreLayout(gridCtl,baseRule);
		}
		#endregion public function...
        /// <summary>
        /// ��XtraGrid�е����ݵ�����Excel �� ����Excel ������ʾ
        /// </summary>
        /// <param name="pDg"></param>
        public void ExportToExcelAndShow(DevExpress.XtraPivotGrid.PivotGridControl xtraGrid) {
            //string temPath = MB.Util.General.GeApplicationDirectory() + @"Temp\";
            //bool b = System.IO.Directory.Exists(temPath);
            //if (!b) {
            //    System.IO.Directory.CreateDirectory(temPath);
            //}
            //string fullPath = temPath + System.Guid.NewGuid().ToString() + ".xls";
            //b = System.IO.File.Exists(fullPath);
            var fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Filter = "Excel �ļ�|.xls";
            fileDialog.ShowDialog();
            var fileFullName = fileDialog.FileName;
            if (string.IsNullOrEmpty(fileFullName)) return;
            try {
                xtraGrid.ExportToXls(fileFullName);
                var b = System.IO.File.Exists(fileFullName);
                //�жϸ��ļ��Ƿ񵼳��ɹ����������ôֱ�Ӵ򿪸��ļ�
                if (b) {
                    //System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                    //Info.FileName = fullPath;
                    ////����һ��������
                    //System.Diagnostics.Process Proc;
                    ////�����ⲿ����
                    //Proc = System.Diagnostics.Process.Start(Info);
                    var dre = MB.WinBase.MessageBoxEx.Question("�ļ������ɹ����Ƿ���Ҫ���ļ����ڵ�Ŀ¼");
                    if (dre == DialogResult.Yes)
                    {
                        System.Diagnostics.ProcessStartInfo explore = new System.Diagnostics.ProcessStartInfo();
                        explore.FileName = "explorer.exe";
                        explore.Arguments = System.IO.Path.GetDirectoryName(fileFullName);
                        System.Diagnostics.Process.Start(explore);
                    }
                }
            }
            catch (Exception e) {
                MB.Util.TraceEx.Write("����Excel �ļ�����" + e.Message, MB.Util.APPMessageType.SysErrInfo);
                MB.WinBase.MessageBoxEx.Show("����Excel �ļ�����");
            }
        }
		
		/// <summary>
		/// ִ��PivotGrid �Զ����¼���
		/// </summary>
		/// <param name="containerForm"></param>
		/// <param name="xtraGrid"></param>
		/// <param name="menuType"></param>
		public void HandleClickXtraContextMenu(MB.WinBase.IFace.IForm containerForm,
			PivotGridEx xtraGrid,XtraContextMenuType menuType){
			if(containerForm==null)
				return;
			switch(menuType){
				case XtraContextMenuType.Sort:
					break;
				case XtraContextMenuType.Style:
					break;
				case XtraContextMenuType.SaveDefaultStyle:
                    showFrmGridLayoutManager(containerForm, xtraGrid); //modify by aifang 2012-6-7
                    //SavePivotGridLayout(containerForm, xtraGrid);
                    //xtraGrid.ShowPrintPreview(); 
					break;
				default:
					break;
			}

		}
        //��ʾGrid״̬���ô���
        private void showFrmGridLayoutManager(MB.WinBase.IFace.IForm containerForm, PivotGridEx xtraGrid)
        {
            frmGridLayoutManager frm = new frmGridLayoutManager(containerForm, xtraGrid);
            frm.ShowDialog();
        }
        public void SavePivotGridLayout(MB.WinBase.IFace.IForm containerForm, PivotGridEx xtraGrid)
        {
            string name = getLayoutXmlSaveName(containerForm.ClientRuleObject, xtraGrid);
            if (!System.IO.Directory.Exists(LAYOUT_XML_PATH))
                System.IO.Directory.CreateDirectory(LAYOUT_XML_PATH);

            xtraGrid.SaveLayoutToXml(LAYOUT_XML_PATH + name);
            //xtraGrid.SaveLayoutToXml(name);
        }
        public void DeletePivotGridLayout(MB.WinBase.IFace.IForm containerForm, PivotGridEx xtraGrid,GridLayoutInfo layoutInfo)
        {
            string name = getLayoutXmlSessionName(containerForm.ClientRuleObject, xtraGrid);
            if (layoutInfo != null) name = name + "~" + layoutInfo.Name + ".xml";

            if (string.IsNullOrEmpty(name)) return;

            if (System.IO.File.Exists(LAYOUT_XML_PATH + name))
            {
                System.IO.File.Delete(LAYOUT_XML_PATH + name);
            }
        }
        public void RestoreLayout(DevExpress.XtraPivotGrid.PivotGridControl gridCtl, MB.WinBase.IFace.IClientRuleQueryBase baseRule) {
			PivotGridEx pivotGrid = gridCtl as PivotGridEx;
			if(pivotGrid==null)
				return;
			string name = getLayoutXmlSaveName(baseRule,pivotGrid);
			if(System.IO.File.Exists(LAYOUT_XML_PATH + name)){
				gridCtl.OptionsLayout.Columns.RemoveOldColumns = true; 
				gridCtl.RestoreLayoutFromXml( LAYOUT_XML_PATH + name );
			}
		}
        public string getLayoutXmlSessionName(MB.WinBase.IFace.IClientRuleQueryBase baseRule, PivotGridEx xtraGrid) {
			if( xtraGrid.ParentForm==null)
				return null;
            return baseRule.GetType().FullName + "~" + xtraGrid.ParentForm.GetType().FullName + "~" + xtraGrid.Name; 			
		}

        //add by aifang
        private GridLayoutInfo GetPivotGridActiveLayout(MB.WinBase.IFace.IClientRuleQueryBase baseRule, PivotGridEx xtraGrid)
        {
            try
            {
                var gridLayoutMainList = new MB.Util.Serializer.DataContractFileSerializer<List<GridLayoutMainInfo>>(GRID_LAYOUT_FILE_SETTING_FULLNAME).Read();
                if (gridLayoutMainList == null) return null;

                string sectionName = getLayoutXmlSessionName(baseRule, xtraGrid);
                var gridLayoutList = gridLayoutMainList.Find(o => o.GridSectionName.Equals(sectionName));
                if (gridLayoutList == null || gridLayoutList.GridLayoutList.Count == 0) return null;

                return gridLayoutList.GridLayoutList.OrderByDescending(o => o.CreateTime).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MB.Util.TraceEx.Write(ex.Message, Util.APPMessageType.SysErrInfo);
                return null;
            }
        }
        private string getLayoutXmlSaveName(MB.WinBase.IFace.IClientRuleQueryBase baseRule, PivotGridEx xtraGrid)
        {
            try
            {
                if (xtraGrid.ParentForm == null)
                    return null;
                string layoutXmlName = getLayoutXmlSessionName(baseRule, xtraGrid);
                GridLayoutInfo layoutInfo = GetPivotGridActiveLayout(baseRule, xtraGrid);
                //add by aifang 2012-08-29 begin
                //�ж�״̬���������Ƿ���ڶ�̬���������ڣ����ǣ�����Ч��������Ч��
                DateTime dt = layoutInfo.CreateTime;
                var dynamicSetting = XtraGridDynamicHelper.Instance.GetXtraGridDynamicSettingInfo(baseRule);
                if (dynamicSetting != null)
                {
                    if (dynamicSetting.LastModifyDate.Subtract(dt).TotalMilliseconds > 0) layoutInfo = null;
                }
                //end

                if (layoutInfo == null)
                    return layoutXmlName + ".xml";
                else return layoutXmlName + "~" + layoutInfo.Name + ".xml";
            }
            catch (Exception ex)
            {
                MB.Util.TraceEx.Write(ex.Message, Util.APPMessageType.SysErrInfo);
                return null;
            }
        }

		#region �ڲ���������...
		//�ڲ��������������ݼ��ص�pivot grid �ؼ��С�
		private void addDataToCtl(DevExpress.XtraPivotGrid.PivotGridControl gridCtl, object  dataSource
                                        , MB.WinBase.IFace.IClientRuleQueryBase baseRule, ColPivotList xmlCfgLst) {

            Dictionary<string, MB.WinBase.Common.ColumnPropertyInfo> colPros = XtraGridDynamicHelper.Instance.GetDynamicColumns(baseRule);
            //modify by aifang 2012-08-29 ��̬�м���  Dictionary<string, MB.WinBase.Common.ColumnPropertyInfo> colPros = baseRule.UIRuleXmlConfigInfo.GetDefaultColumns();

			if(colPros==null)
				return;

			//�����ʷ������
			gridCtl.Fields.Clear();
			gridCtl.Groups.Clear();

            List<Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo>> expressionFieldsList = new List<Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo>>();

            foreach (MB.WinBase.Common.ColumnPropertyInfo info in colPros.Values) {
				if(!info.IsPivotExpressionSourceColumn && (!info.Visibled || info.VisibleWidth < 1))
					continue;
				var result =setPivotFieldByCfg(xmlCfgLst,gridCtl,info);
                if (result != null)
                    expressionFieldsList.Add(result);
			}

            //�ڰ�һ�����Ժ��ٴ���ǰ��еı��ʽ�������һ����ᷢ���еļ����Ⱥ�˳��
            foreach (Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo> expressionFields in expressionFieldsList)
            {
                foreach (KeyValuePair<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo> expressionField in expressionFields)
                {
                    expressionField.Key.UnboundType = DevExpress.Data.UnboundColumnType.Object;

                    string[] sourceColumns = expressionField.Value.ExpressionSourceColumns.Split(',');
                    string[] formattedSourceColumns = new string[sourceColumns.Length];
                    for (int i = 0; i < sourceColumns.Length; i++)
                    {
                        DevExpress.XtraPivotGrid.PivotGridField sourceField = gridCtl.Fields[sourceColumns[i]];
                        if (sourceField == null)
                            throw new MB.Util.APPException(string.Format("ExpressionSourceColumns�е���{0}��Expression�е��в����ڣ���ע�������������б����ڱ������е�ǰ��", sourceColumns[i], MB.Util.APPMessageType.SysErrInfo));

                        formattedSourceColumns[i] = sourceField.ExpressionFieldName;
                    }

                    expressionField.Key.UnboundExpression = string.Format(expressionField.Value.Expression, formattedSourceColumns);
                }
            }

			//�����ֶΰﶨ�������
			processUnitField(xmlCfgLst,gridCtl);
			
			//ͨ������������ʽ
			addConditionsForStyles(gridCtl,baseRule);

			gridCtl.DataSource = dataSource;
			
		}

		//ͨ�����õ�PivotList ����PivotField ����Ϣ 
        private Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo> setPivotFieldByCfg(ColPivotList xmlCfgLst, DevExpress.XtraPivotGrid.PivotGridControl gridCtl,
													MB.WinBase.Common.ColumnPropertyInfo fieldInfo){

			if(xmlCfgLst==null)
				return null;
			if(xmlCfgLst.Columns.Count ==0 && !xmlCfgLst.AutoCreatedGridField)
				return null;

			IList<MB.XWinLib.PivotGrid.ColPivotInfo>  infos = xmlCfgLst.GetColPivotInfos(fieldInfo.Name);
			DevExpress.XtraPivotGrid.PivotGridGroup pivotGridGroup = null;
			if(infos.Count > 1 && xmlCfgLst.SameFieldGroup){
				pivotGridGroup = new DevExpress.XtraPivotGrid.PivotGridGroup();
				gridCtl.Groups.Add(pivotGridGroup);
			}
			if(infos.Count== 0){
				createNewPivotField(fieldInfo.Name,fieldInfo.Description,gridCtl);
                //add by aifang ȥ�����ܺ�����ʾ��ʽ��ԭ�������ǽ�����������ʾ��ǰ׺��begin
                if (fieldInfo.DataType.Equals("System.Decimal"))
                {
                    gridCtl.Fields[fieldInfo.Name].CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                }
                //add by aifang end
				return null;
			}
			else{

                Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo> expressionFields = 
                    new Dictionary<DevExpress.XtraPivotGrid.PivotGridField, ColPivotInfo>();

                
				//����һ���ֶζ�Ӧ�����������
                foreach (ColPivotInfo info in infos)
                {
                    DevExpress.XtraPivotGrid.PivotGridField pivField = createNewPivotField(fieldInfo.Name, fieldInfo.Description, gridCtl);
                    //���������Expression���ʾ���е�ֵ��ͨ�������л������ֶ����������
                    if (!string.IsNullOrEmpty(info.Expression))
                    {
                        expressionFields.Add(pivField, info);
                    }

                    if (info.Description != null && info.Description.Length > 0)
                        pivField.Caption = info.Description;

                    pivField.Area = info.IniArea;
                    pivField.AreaIndex = info.AreaIndex;
                    pivField.AllowedAreas = info.AllowedAreas;
                    pivField.TopValueCount = info.TopValueCount;
                    pivField.GroupInterval = info.GroupInterval;

                    if (info.SummaryItemType != null && info.SummaryItemType.Length > 0)
                        pivField.SummaryType = (DevExpress.Data.PivotGrid.PivotSummaryType)Enum.Parse(typeof(DevExpress.Data.PivotGrid.PivotSummaryType), info.SummaryItemType);

                    //setDefaultFormatByGroup(pivField);
                    //��ʽ����ʾ���ݡ�
                    if (info.CellFormat != DevExpress.Utils.FormatInfo.Empty)
                    {
                        pivField.CellFormat.Format = info.CellFormat.Format;
                        pivField.CellFormat.FormatType = info.CellFormat.FormatType;
                        pivField.CellFormat.FormatString = info.CellFormat.FormatString;
                    }
                    if (info.ValueFormat != DevExpress.Utils.FormatInfo.Empty)
                    {
                        pivField.ValueFormat.Format = info.CellFormat.Format;
                        pivField.ValueFormat.FormatType = info.CellFormat.FormatType;
                        pivField.ValueFormat.FormatString = info.CellFormat.FormatString;
                    }
                    //�����ֶΰ󶨷��顣
                    if (pivotGridGroup != null)
                    {
                        pivotGridGroup.Add(pivField);
                    }
                    //DevExpress.XtraPivotGrid.PivotGroupInterval.DateDayOfWeek

                }
                return expressionFields;
			}
		}
		//�����ֶν�ϰ󶨵����
		private void processUnitField(ColPivotList xmlCfgLst,DevExpress.XtraPivotGrid.PivotGridControl gridCtl){
			if(xmlCfgLst==null || xmlCfgLst.FieldGroups==null || xmlCfgLst.FieldGroups.Length ==0)
				return;
			string[] groups = xmlCfgLst.FieldGroups.Split(';');
			foreach(string group in groups){
				string[] fields = group.Split(',');
				DevExpress.XtraPivotGrid.PivotGridGroup pivotGridGroup = new DevExpress.XtraPivotGrid.PivotGridGroup();
				gridCtl.Groups.Add(pivotGridGroup);
				foreach(string field in fields){
					DevExpress.XtraPivotGrid.PivotGridField pivField = gridCtl.Fields[field];
					if(pivField==null)
						continue;
					pivotGridGroup.Add(pivField);
				}
			}

		}
		//���ݷ������������Ĭ�ϵķ������ݡ�
//		private static void setDefaultFormatByGroup(DevExpress.XtraPivotGrid.PivotGridField pivField){
//			switch(pivField.GroupInterval){
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateYear:
//					pivField.ValueFormat.FormatString = "{0} ��";
//					pivField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//					break;
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateQuarter:
//					pivField.ValueFormat.FormatString = "���� {0}";
//					pivField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//					break;
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateMonth:
//					pivField.ValueFormat.FormatString = "{0} ��";
//					pivField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//					break;
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateWeekOfYear:
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateWeekOfMonth:
//					pivField.ValueFormat.FormatString = "�� {0} ��";
//					pivField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//					break;
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateDayOfWeek:
//					pivField.ValueFormat.FormatString = "���� {0}";
//					pivField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//					break;
//				case DevExpress.XtraPivotGrid.PivotGroupInterval.DateDay:
//					break;
//				default:
//					break;
//			}
//
//		}
		//����һ���µ�PivotGridField ����
		private DevExpress.XtraPivotGrid.PivotGridField createNewPivotField(string fieldName,string fieldDesc,DevExpress.XtraPivotGrid.PivotGridControl gridCtl){
			DevExpress.XtraPivotGrid.PivotGridField pivField = new DevExpress.XtraPivotGrid.PivotGridField();
			pivField.Name = "pivCol" + fieldName;
			pivField.FieldName = fieldName;
			pivField.Caption = fieldDesc;
			gridCtl.Fields.Add(pivField); 
			gridCtl.OptionsLayout.StoreAllOptions = true; 
			return pivField;
		}
		//����������ʾ�ĸ�ʽ����ʽ��Ϣ��
        private void addConditionsForStyles(DevExpress.XtraPivotGrid.PivotGridControl gridCtl, MB.WinBase.IFace.IClientRuleQueryBase busObj) {			
			if(busObj == null)
				return;


            Dictionary<string, MB.WinBase.Common.StyleConditionInfo> styleConditions = busObj.UIRuleXmlConfigInfo.GetDefaultStyleConditions();
			if(styleConditions==null)return;

			XWinLib.Share.StyleCfgList styleList =  XWinLib.Share.StyleCfgList.CreateInstance();
			if(styleList==null)return;
              
			foreach(MB.WinBase.Common.StyleConditionInfo info in  styleConditions.Values){
				//�ж϶������ʽ�Ƿ���ڡ�
				if(!styleList.ContainsKey(info.StyleName)){
					MB.Util.TraceEx.Write(string.Format("��ʽ {0} ��Ҫ��AppStylesConfig.xml �ļ�������,����Ѿ����������Сд��",info.StyleName),MB.Util.APPMessageType.SysErrInfo);  
					continue;
				}
				DevExpress.XtraPivotGrid.PivotGridField pivField = gridCtl.Fields[info.ColumnName];
				if(pivField==null) {
					MB.Util.TraceEx.Write(string.Format("�ֶ� {0} �� Style Format condition �����Ƶ����ã�������Դ���Ҳ�����Ӧ���У����顣",info.ColumnName ),MB.Util.APPMessageType.SysErrInfo);  
					continue;
				}

				XWinLib.Share.StyleCfgInfo styleInfo = styleList[info.StyleName];

				//�������DispColName �Ļ�����ҪҲ�Ǵ����ڱ�Ӧ�ó���������GridView��RowCellStyle�¼��н��д���
				if(info.IsByEvent || (info.DispColName!=null && info.DispColName.Length > 0 && info.DispColName!=info.ColumnName))
					continue;
				//����FormatCondition��  
				if(gridCtl.FormatConditions[info.Name]==null){
					DevExpress.XtraGrid.FormatConditionEnum cc = (DevExpress.XtraGrid.FormatConditionEnum)Enum.Parse( typeof(DevExpress.XtraGrid.FormatConditionEnum),Enum.GetName(typeof(DataFilterConditions),info.Condition));
					DevExpress.XtraPivotGrid.PivotGridStyleFormatCondition styleCondition = new DevExpress.XtraPivotGrid.PivotGridStyleFormatCondition();

					styleCondition.Appearance.BackColor = styleInfo.BackColor;
					styleCondition.Appearance.ForeColor = styleInfo.ForeColor;
					styleCondition.Appearance.Font = styleInfo.Font;

					styleCondition.Appearance.Options.UseBackColor = true;
					styleCondition.Appearance.Options.UseForeColor = true;
					styleCondition.ApplyToCustomTotalCell = false;
					styleCondition.ApplyToGrandTotalCell = false;
					styleCondition.ApplyToTotalCell = false;

					styleCondition.Condition = (DevExpress.XtraGrid.FormatConditionEnum)Enum.Parse( typeof(DevExpress.XtraGrid.FormatConditionEnum),Enum.GetName(typeof(DataFilterConditions),info.Condition));
					styleCondition.Field = pivField;
					styleCondition.Value1 = info.Value ;
					styleCondition.Value2 = info.Value2; 

					gridCtl.FormatConditions.Add(styleCondition);

				}
			}
		}
		
		#endregion �ڲ���������...
	}
}
