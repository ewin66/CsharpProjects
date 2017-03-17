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

namespace DIYReport.ReportModel
{
	/// <summary>
	/// CreateRptObjParam ��ժҪ˵����
	/// </summary>
	public class CreateRptObjParam
	{
		private RptCssClassList  _CssClasss;
		private RptSection _Section;
		public CreateRptObjParam(RptCssClassList pCssClasss,RptSection  pSection) {
			_CssClasss = pCssClasss;
			_Section = pSection;
		}
		#region Public ����...
		public RptCssClassList CssClasss {
			get {
				return _CssClasss;
			}
		}
		public RptSection Section {
			get {
				return _Section;
			}
		}
		#endregion Public ����...
	}
}
