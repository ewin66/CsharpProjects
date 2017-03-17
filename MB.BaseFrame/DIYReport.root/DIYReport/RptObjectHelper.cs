//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-08
// Description	:	 RptObjectHelper ������������
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Diagnostics; 

namespace DIYReport
{
	/// <summary>
	/// RptObjectHelper ������������
	/// </summary>
	public class RptObjectHelper
	{
		private RptObjectHelper()
		{
			
		}
		/// <summary>
		/// �������ʹ����������
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DIYReport.Interface.IRptSingleObj CreateObj(DIYReport.ReportModel.RptObjType pType){
			return CreateObj(pType,null);
		}
		/// <summary>
		/// �������ʹ����������
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dispText"></param>
		/// <returns></returns>
		public static DIYReport.Interface.IRptSingleObj CreateObj(DIYReport.ReportModel.RptObjType pType,string dispText){
			DIYReport.Interface.IRptSingleObj obj= null;
			switch(pType){
				case DIYReport.ReportModel.RptObjType.Line :
					obj = new DIYReport.ReportModel.RptObj.RptLine(null);    
					break;
				case DIYReport.ReportModel.RptObjType.Rect :
					obj = new DIYReport.ReportModel.RptObj.RptRect(null); 
					break;
				case DIYReport.ReportModel.RptObjType.Text :
					obj = new DIYReport.ReportModel.RptObj.RptLable(null,dispText);
					break;
				case DIYReport.ReportModel.RptObjType.Express :
					obj = new DIYReport.ReportModel.RptObj.RptExpressBox(null,dispText); 
					break;
				case DIYReport.ReportModel.RptObjType.Image :
					obj = new DIYReport.ReportModel.RptObj.RptPictureBox(null); 
					break;
				case DIYReport.ReportModel.RptObjType.FieldImage:
					obj = new DIYReport.ReportModel.RptObj.RptDBPictureBox(null); 
					break;
				case DIYReport.ReportModel.RptObjType.CheckBox:
					obj = new DIYReport.ReportModel.RptObj.RptCheckBox(null); 
					break;
				case DIYReport.ReportModel.RptObjType.BarCode:
					obj = new DIYReport.ReportModel.RptObj.RptBarCode(null);  
					break;
				case DIYReport.ReportModel.RptObjType.SubReport:
					obj = new DIYReport.ReportModel.RptObj.RptSubReport(null);  
					break;
				case DIYReport.ReportModel.RptObjType.FieldTextBox:
					obj = new DIYReport.ReportModel.RptObj.RptFieldTextBox(null);   
					break;
				case DIYReport.ReportModel.RptObjType.HViewSpecField:
					obj = new DIYReport.ReportModel.RptObj.RptHViewSpecFieldBox(null);   
					break;
				case DIYReport.ReportModel.RptObjType.RichTextBox:
					obj = new DIYReport.ReportModel.RptObj.RptRichTextBox(null);   
					break;
				default:
					Debug.Assert(false,"�ÿؼ�����Ŀǰ��û�д���","");
					return null;
			}
			return obj;
		}
	}
}
