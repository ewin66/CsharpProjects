//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-21
// Description	:	ExSpecial �����ӡ�������ֶ�(����ϵͳ������������Ϣ���統ǰҳ����ҳ����ǰʱ�䡢���ڵ�)
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
namespace DIYReport.Express
{
	/// <summary>
	/// ExSpecial �����ӡ�������ֶ�(����ϵͳ������������Ϣ���統ǰҳ����ҳ����ǰʱ�䡢���ڵ�)
	/// </summary>
	public class ExSpecial
	{
		//������������ֵ��ÿ��Ԥ�����ߴ�ӡ�Ĺ����ж���Ҫ����
		public static int _Page = 0;
		public static int _PageCount = 0;
		public static int _RowOrderNO = 0;
		#region ��̬�ķ���...
		[Description("ҳ��")]
		public static int Page(){
			return _Page;
		}
		[Description("ҳ����")]
		public static int PageCount(){
			return _PageCount;
		}
		[Description("ҳ����Ϣ")]
		public static int PageInfo(){
			return 0;
		}
		[Description("��ӡ����")]
		public static string PrintDate(){
			return System.DateTime.Now.ToShortDateString()  ;
		}
		[Description("��ӡʱ��")]
		public static string PrintTime(){
			return System.DateTime.Now.ToShortTimeString() ;
		}
		[Description("������˳���")]
		public static int RowOrderNO(){
			return _RowOrderNO ;
		}
		#endregion ��̬�ķ���...
	}
}
