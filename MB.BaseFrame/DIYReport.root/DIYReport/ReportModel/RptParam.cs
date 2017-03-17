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
	/// RptParam ��ժҪ˵����
	/// </summary>
	public class RptParam
	{
		private string _ParamName;
		private object _Value;
		/// <summary>
		/// ��ӡ������Ҫ�Ĳ�����Ϣ
		/// </summary>
		/// <param name="pNode"></param>
		public RptParam(string  pName,object pValue) {
			_ParamName = pName;
			_Value =  pValue;
		}

		public string ParamName {
			get {
				return _ParamName;
			}
			set {
				_ParamName=value;
			}
		}
		public object Value {
			get {
				return _Value;
			}
			set {
				_Value=value;
			}
		}
	}
}
