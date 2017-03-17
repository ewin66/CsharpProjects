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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptLable ���ı�������
	/// </summary>
	public class RptLable : DIYReport.ReportModel.RptTextObj
	{
		private string _Text;

		#region ���캯��...
		public RptLable() : this(null){
		}
		public RptLable(string pName):this(pName,null){
		}
		public RptLable(string pName,string pText) : base(pName,DIYReport.ReportModel.RptObjType.Text)
		{
			if(pText==null){
				_Text = "[������]";
			}
			else{
				_Text = pText;
			}
		}
		#endregion ���캯��...

		#region Public ����...
		[Description("���û��ߵõ���ӡ�ؼ����ı�������"),Category("����")]
		public string Text {
			get {
				return _Text;
				
			}
			set {
				_Text = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		#endregion Public ����...

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members
	}
}
