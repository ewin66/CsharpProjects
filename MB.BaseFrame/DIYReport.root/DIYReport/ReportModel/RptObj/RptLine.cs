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
using System.Drawing ;
using System.Xml ;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using DIYReport.ReportModel ;
namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptLine ֱ�ߡ�
	/// </summary>
	public class RptLine : DIYReport.ReportModel.RptSingleObj  
	{
		private LineType _LineType;
		public RptLine() : this(null){
		}
		public RptLine(string pName) : base(pName,DIYReport.ReportModel.RptObjType.Line )
		{
			_LineType = LineType.Bias;
			base.IsTranspControl = true;
		}

		#region Public ����...
		[Description("���û��ߵõ����������͡�"),Category("���")]
		public LineType LineType{
			get{
				return _LineType;
			}
			set{
				_LineType = value;
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
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
