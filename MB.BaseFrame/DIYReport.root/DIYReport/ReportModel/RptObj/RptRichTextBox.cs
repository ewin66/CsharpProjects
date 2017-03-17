//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-22
// Description	:	RptRichTextBox ���ֶε�richtextbox ����
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace DIYReport.ReportModel.RptObj {
	/// <summary>
	/// RptRichTextBox ���ֶε�richtextbox ����
	/// </summary>
	public class RptRichTextBox : DIYReport.ReportModel.RptSingleObj {

		private string _FieldName;
		private string _BingDBFieldName;//�ڲ������ֶΣ�fieldName һ���Ӧ����������ֻ��BingDBFieldName �����������ݿ��Ӧ���ֶΡ�
		private bool _ShowFrame;
		private System.Windows.Forms.PictureBoxSizeMode _SizeModel;
		private System.Drawing.Font _Font;
		#region ���캯��...
		public RptRichTextBox() : this(null){
		}
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="pText"></param>
		public RptRichTextBox(string pName) : this(pName,null) {

		}
		public RptRichTextBox(string pName,string dataSource) : base(pName,DIYReport.ReportModel.RptObjType.RichTextBox) {
			if(dataSource==null){
				_FieldName =  DIYReport.Drawing.RptDrawHelper.NO_BING_TAG; 
			}
			else{
				_FieldName = dataSource;
			}
		}
		#endregion ���캯��...

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		#region public ����...
		[Description("���û��ߵõ�ͼ������ݿ��ֶΡ�"),Category("����"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
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
		[Description("���û��ߵõ������Ƿ���ʾ�߿�"),Category("���")]
		public bool ShowFrame {
			get {
				return _ShowFrame;
			}
			set {
				_ShowFrame = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�����richTextBox ���ݵķ�ʽ��"),Category("���")]
		public System.Windows.Forms.PictureBoxSizeMode DrawSizeModel{
			get{
				return _SizeModel;
			}
			set{
				_SizeModel = value;
			}

		}
		[Description("���û��ߵõ���ʽ�����塣"),Category("���")]
		public System.Drawing.Font Font {
			get {
				return _Font;
			}
			set {
				_Font = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		#endregion public ����...
	}
}
