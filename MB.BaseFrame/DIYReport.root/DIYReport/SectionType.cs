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

namespace DIYReport 
{
	/// <summary>
	/// SectionType ����Section �����͡�
	/// </summary>
	public enum SectionType : int
	{
		TopMargin = 0,
		ReportTitle = 1, //�������
		PageHead = 2, //ҳͷ����
		GroupHead = 3, //����������
		Detail = 4, //ҳ���Detail ��Ϣ
		GroupFooter = 5, //��������
		PageFooter = 6, //ҳ�ŵ�������Ϣ
		ReportBottom = 7, //�����β����Ϣ
		BottomMargin = 8

		
	}
}
