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
	/// LineType ֱ�����͡�
	/// </summary>
	public enum LineType : int
	{
		Horizon = 0,//ˮƽ��
		Vertical = 1,//��ֱ��
		Bias = 2,// б��
		Backlash = 3 //��б��
	}
}
