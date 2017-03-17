//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-21
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace DIYReport.Express
{
	/// <summary>
	/// ExpressValues ���û��ߵõ�XML���������ļ��еı��ʽ��
	/// Ĭ�������� Value
	/// [DefaultMemberAttribute("Value")]
	/// </summary>
	public class ExpressValue {
		#region ��������...
		private string _Name;
		private string _Value =  "0" ;
		private string _FieldName ;
		private DIYReport.ReportModel.RptSingleObj   _RptObj;
		#endregion ��������...

		#region ���캯��...
		public ExpressValue(DIYReport.ReportModel.RptSingleObj   pObj) {
			_Name = pObj.Name;
			_RptObj = pObj;
			if(RptObj!=null){
				_FieldName = RptObj.FieldName ;
			}
			else{
				_FieldName = "";
			}
		}
		#endregion ���캯��...

		#region ����������...
		public static ExpressValue operator+( ExpressValue pValue,object pObj) {
			double val = PublicFun.ToDouble(pValue.Value);
			double addVal = PublicFun.ToDouble(pObj); 
			if(pValue.RptObj !=null){
				string exName = pValue.RptObj.DataSource;

				val += PublicFun.ToDouble(pObj); 
				//pValue.Value = val.ToString("F"); 
				pValue.Value = val.ToString(); 
			}
			return pValue;
		}
		#endregion ����������...

		#region Public ����...
		public DIYReport.ReportModel.RptObj.RptExpressBox    RptObj{
			get{
				return _RptObj as DIYReport.ReportModel.RptObj.RptExpressBox ;
			}
		}
		public string FieldName{
			get{
				return _FieldName;
			}
		}
		public string Name {
			get {
				return _Name;
			}
			set {
				_Name = value;
			}
		}
		public  string  Value {
			get {
				return _Value;
			}
			set {
				_Value = value;
			}
		}

		#endregion Public ����...





	}
}
