//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-15
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Data ;
using System.Collections ;
using System.Windows.Forms ;
using System.Drawing.Printing;
namespace DIYReport.UserDIY {
	/// <summary>
	/// DesignEnviroment �û�DIY������Ƶ��ⲿ������Ϣ��
	/// </summary>
	public class DesignEnviroment{
		private static bool _PressShiftKey;
		private static bool _PressCtrlKey;
		private static DIYReport.ReportModel.RptObjType  _DrawControlType;
		private static bool _IsCreateControl;
		private static PageSettings _PageSettings;
		//��ǰ������Ƶı���
		private static DIYReport.ReportModel.RptReport _CurrentReport;
		//��ǰ���ڻ�ȡ����ı������
		private static object _CurrentRptObj;
		//�����Ӧ������
		private static object  _DataSource ;
		//�������ʱ���ֶ�
		private static IList _DesignField;

		//private static DIYReport.ReportModel.RptParamList _UserParamList ;  
		//�жϵ�ǰ������������
		private static bool _IsUserDesign = false;
		//�жϵ�ǰ��������Ƿ��Ѿ������ı䣬ͨ���������û������Ƿ�洢
		private static bool _DesignHasChanged = false;

		//nick 2006-04-07 ����
		private static UICommandExecutor _UICmidExecutor;

		#region Public ����...
		//		/// <summary>
		//		/// �û���������Ĳ�����Ϣ
		//		/// </summary>
		//		public static DIYReport.ReportModel.RptParamList UserParamList{
		//			get{
		//				if(_UserParamList==null){
		//					_UserParamList = new DIYReport.ReportModel.RptParamList();
		//				}
		//				return _UserParamList;
		//			}
		//			set{
		//				_UserParamList = value;
		//			}
		//		}
		public static object  DataSource{
			get{
				return _DataSource;
			}
			set{
				_DataSource = value;
			}
		}
		/// <summary>
		/// ��ʾ���Դ���
		/// </summary>
		/// <param name="pParentForm"> ��ʾ�ô��ڵĸ�����</param>
		/// <param name="pMustShow">�ж��Ƿ������ʾ</param>
		public static void ShowPropertyForm(Form pParentForm,bool pMustShow){			
			//			bool b = false;
			//			foreach(Form frm in pParentForm.OwnedForms){
			//				if(frm.Name == "FrmEditRptObjAttribute"){
			//					(frm as FrmEditRptObjAttribute).SetPropertryObject( _CurrentRptObj);
			//					b = true;
			//					break;
			//				}
			//			}
			//			if(!b && pMustShow == true){
			//				FrmEditRptObjAttribute frm =  new FrmEditRptObjAttribute();
			//				frm.SetPropertryObject( _CurrentRptObj);
			//				pParentForm.AddOwnedForm(frm);
			//				frm.Show();
			//			}
		}
		//		/// <summary>
		//		/// ������Դ����Ѿ���ʾ��������ؼ���ʱ����ʾ��Ӧ������
		//		/// </summary>
		//		public static void DispProperty(){
		//			if(_PropertyForm!=null && _PropertyForm.Visible){
		//				_PropertyForm.SetPropertryObject( _CurrentRptObj);
		//			}
		//		}
		/// <summary>
		/// �����ֶε������õ��ֶε�����
		/// </summary>
		/// <param name="pFieldDesc"></param>
		/// <returns></returns>
		public static string GetFieldNameByDesc(string pFieldDesc){
			if(pFieldDesc==null || pFieldDesc=="" || _CurrentReport == null || _CurrentReport.DesignField ==null)
				return pFieldDesc;
			foreach(DIYReport.GroupAndSort.RptFieldInfo info in _CurrentReport.DesignField){
				if(info.Description.ToUpper() ==  pFieldDesc.ToUpper()){
					return info.FieldName ;
				}
			}
			return pFieldDesc;
		}
		#endregion Public ����...

		#region Public ����...
		public static bool IsUserDesign{
			get{
				return _IsUserDesign;
			}
			set{
				_IsUserDesign = value;
			}
		}
		public static object CurrentRptObj{
			get{
				return _CurrentRptObj;
			}
			set{
				_CurrentRptObj = value;
				//DispProperty();
			}
		}
		public static DIYReport.ReportModel.RptReport CurrentReport{
			get{
				return _CurrentReport;
			}
			set{
				_CurrentReport = value;
			}
		}
		public static PageSettings PageSettings{
			get{
				if(_PageSettings==null){
					_PageSettings = new PageSettings();
				}
				return _PageSettings;
			}
			set{
				_PageSettings = value;
			}
		}
		public static bool IsCreateControl{
			get{
				return _IsCreateControl;
			}
			set{
				_IsCreateControl = value;
			}
		}
		/// <summary>
		/// �����ؼ������� //
		/// </summary>
		public static DIYReport.ReportModel.RptObjType DrawControlType{
			get{
				return _DrawControlType;
			}
			set{
				_DrawControlType = value;
			}
		}
		public static bool PressShiftKey{
			get{
				return _PressShiftKey;
			}
			set{
				_PressShiftKey = value;
			}
		}
		public static bool PressCtrlKey{
			get{
				return _PressCtrlKey;
			}
			set{
				_PressCtrlKey = value;
			}
		}
		public static bool DesignHasChanged{
			get{
				return _DesignHasChanged;
			}
			set{
				_DesignHasChanged = value;
			}
		}
		public static IList DesignField{
			get{
				if(_DesignField==null || _DesignField.Count==0){
					_DesignField = new ArrayList(); 
					if(_DataSource!=null  ){
						DataView  dv = PublicFun.GetDataViewByObject(_DataSource);  
						DataTable dt = dv.Table ;
						int i = 0;
						foreach(DataColumn dc in dt.Columns){
							//����ID����ʾ��Ϊ��������
							if( string.Compare(dc.ColumnName,"ID",true)==0) continue;

							DIYReport.GroupAndSort.RptFieldInfo info = new DIYReport.GroupAndSort.RptFieldInfo(dc.ColumnName,dc.Caption
																			,dc.DataType.Name,i++);
							_DesignField.Add( info );
							

						}
					}
				}
				return _DesignField;
			}
			set{
				IList temp = value;
				IList newList = null ;
				if(temp!=null){
					if(temp.Count >0){
						if(temp[0].GetType().Name!= "RptFieldInfo"){
							newList = new ArrayList();
							for(int i = 0 ; i < temp.Count;i++){
								object field = temp[i];
								if(string.Compare(field.GetType().Name,"String",true)==0) {
									DIYReport.GroupAndSort.RptFieldInfo info = new DIYReport.GroupAndSort.RptFieldInfo(temp[i].ToString());
									newList.Add(info);
								}
								else{
									newList.Add(field);
								}
							}
						}
						else{
							newList = value;
						}
					}
				}
				_DesignField = newList;
			}
		}
		public static UICommandExecutor UICmidExecutor{
			get{
				return _UICmidExecutor;
			}
			set{
				_UICmidExecutor = value;
			}
		}
		#endregion Public ����...
	}
}
