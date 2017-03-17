//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-30
// Description	:	ISubReportCommand �ӱ�����ش�������ӿڡ� 
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Data;
using System.Collections;

namespace DIYReport.Interface
{
	/// <summary>
	/// ISubReportCommand �ӱ�����ش�������ӿڡ�
	/// </summary>
	public interface ISubReportCommand
	{
		/// <summary>
		/// ���ݱ�������ƻ�ȡ��Ӧ���ӱ���
		/// </summary>
		/// <param name="reportName"></param>
		/// <returns></returns>
		DIYReport.ReportModel.RptReport GetReportContent(string reportName);
		/// <summary>
		/// �������ƻ�ȡ��Ӧ����Դ��
		/// </summary>
		/// <param name="reportName"></param>
		/// <returns></returns>
        object GetReportDataSource(DataRow parentRow, string relationMember, string reportName);
		
		/// <summary>
		/// �ӱ�������ơ�
		/// </summary>
		IList  SubReportName{get;}
		
	}
}
