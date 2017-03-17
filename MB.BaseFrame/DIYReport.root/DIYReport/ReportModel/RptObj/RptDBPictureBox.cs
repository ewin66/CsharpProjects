//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-03
// Description	:	 RptDBPictureBox �ﶨ���ݿ��ж�Ӧ��image�ֶΡ�
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace DIYReport.ReportModel.RptObj {
	/// <summary>
	/// RptDBPictureBox �ﶨ���ݿ��ж�Ӧ��image�ֶΡ�
	/// </summary>
	public class RptDBPictureBox : DIYReport.ReportModel.RptImage {
 
		private string _FieldName;

		private string _BingDBFieldName;//�ڲ������ֶΣ�fieldName һ���Ӧ����������ֻ��BingDBFieldName �����������ݿ��Ӧ���ֶΡ�
		private bool _ShowFrame;

		//private readonly string NO_BING_TAG = "[δ��]";
 
		
		#region ���캯��...
		public RptDBPictureBox() : this(null){
		}
		/// <summary>
		/// ���캯��...
		/// </summary>
		/// <param name="pName"></param>
		public RptDBPictureBox(string pName):this(pName,null){
		}
		/// <summary>
		/// ���캯��...
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="pDataSource"></param>
		public RptDBPictureBox(string pName,string dataSource) : base(pName,DIYReport.ReportModel.RptObjType.FieldImage) {
			if(dataSource==null)
				_FieldName =  DIYReport.Drawing.RptDrawHelper.NO_BING_TAG; 
			else
				_FieldName = dataSource;
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
		#endregion public ����...
	}
}
