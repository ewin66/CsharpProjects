//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-13
// Description	:	RptFieldTextBox ���ֶε��ı���   
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace DIYReport.ReportModel.RptObj {
	/// <summary>
	/// RptFieldTextBox ���ֶε��ı���
	/// </summary>
	public class RptFieldTextBox : DIYReport.ReportModel.RptTextObj {
		private string _FieldName;

		private string _BingDBFieldName;//�ڲ������ֶΣ�fieldName һ���Ӧ����������ֻ��BingDBFieldName �����������ݿ��Ӧ���ֶΡ�
		private bool _IncludeMultiField;

		#region ���캯��...
		/// <summary>
		/// 
		/// </summary>
		public RptFieldTextBox() : this(null){
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RptFieldTextBox(string name) : base(name,DIYReport.ReportModel.RptObjType.FieldTextBox) {
			_FieldName = DIYReport.Drawing.RptDrawHelper.NO_BING_TAG ; 
			
		}
		#endregion ���캯��...

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		#region public ����...
		[Description("������Դ���ֶ����ơ�"),Category("����") ,Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
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
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("�ڲ����ֶ�,�����Ա༭�ɰ�FieldNameʱ�Զ���ѯ����ȡ��") ] 
        [Browsable(false)]
		public string BingDBFieldName{
			get{
				return _BingDBFieldName;
			}
			set{
				_BingDBFieldName = value;
			}
		}
		[Description("��ǰ�ֶΰ��Ƿ��������ֶ�,������ֶ�֮��Ҫ�÷ֺŷֿ���"),Category("����")]
		public bool IncludeMultiField{
			get{
				return _IncludeMultiField;
			}
			set{
				_IncludeMultiField = value;
			}
		}
		#endregion public ����...
	}
}
