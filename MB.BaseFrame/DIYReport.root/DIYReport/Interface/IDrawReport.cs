//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-30
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing ;

namespace DIYReport.Interface
{
	/// <summary>
	/// IDrawReport ���Ʊ������ݵĲ�������ӿ�
	/// </summary>
	public interface IDrawReport : System.IDisposable
	{
		DIYReport.ReportModel.RptReport DataReport{get;}
		int HasDrawRowCount{get;set;}
		int PageNumber{get;set;}

		bool DrawReportSection(object graphicsObject,DIYReport.SectionType pSectionType);
		void BeginPrint();
	}
}
