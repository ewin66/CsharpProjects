//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-07
// Description	:	RptCheckBox True �� False ѡ���
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptCheckBox True �� False ѡ���
	/// </summary>
	public class RptCheckBox : DIYReport.ReportModel.RptSingleObj {
		private string _FieldName;
		private string _BingDBFieldName;//�ڲ������ֶΣ�fieldName һ���Ӧ����������ֻ��BingDBFieldName �����������ݿ��Ӧ���ֶΡ�
		private bool _Checked;
		private bool _BingField;
		private bool _ShowFrame;
		#region ���캯��...
		public RptCheckBox() : this(null){
		}
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="pText"></param>
		public RptCheckBox(string pName) : this(pName,null) {

		}
		public RptCheckBox(string pName,string dataSource) : base(pName,DIYReport.ReportModel.RptObjType.CheckBox) {
			if(dataSource==null){
				_FieldName =  DIYReport.Drawing.RptDrawHelper.NO_BING_TAG; 
			}
			else{
				_FieldName = dataSource;
				_BingField = true;
			}
		}
		#endregion ���캯��...

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		[Description("��ȡ��������True ���� False."),Category("��Ϊ")]
		public bool Checked{
			get{
				return _Checked;
			}
			set{
				_Checked = value;
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Description("��ȡ���������Ƿ���Ҫ������Դ."),Category("����")]
		public bool BingField{
			get{
				return _BingField;
			}
			set{
				_BingField = value;
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Description("���û��ߵõ�ͼ������ݿ��ֶΡ�"),Category("����"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
				_BingField = true;
				if(this.Section!=null && this.Section.Report!=null){ 
					string fName = DIYReport.PublicFun.GetFieldNameByDesc(_FieldName,this.Section.Report.DesignField); 
					_BingDBFieldName = fName;
				}
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Browsable(false)] 
		public string BingDBFieldName{
			get{
				return _BingDBFieldName;
			}
			set{
				_BingDBFieldName = value;
			}
		}
		[Description("���û��ߵõ����������Ƿ���ʾ�߿�"),Category("���")]
		public bool ShowFrame {
			get {
				return _ShowFrame;
			}
			set {
				_ShowFrame = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
	}
}
