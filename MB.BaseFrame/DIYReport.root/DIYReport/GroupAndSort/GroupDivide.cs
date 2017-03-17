//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-04-19
// Description	:	�ڷ��鴦���ж���ָ�������ͣ�����ʲô��ʽ������ͷ��飩��
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace DIYReport.GroupAndSort
{
	/// <summary>
	/// GroupDivide �ڷ��鴦���ж���ָ�������ͣ�����ʲô��ʽ������ͷ��飩��
	/// </summary>
	public class GroupDivide
	{
		/// <summary>
		/// �õ���������������Ϣ
		/// </summary>
		/// <param name="pType"></param>
		/// <returns></returns>
		public static string[] GetDivideTextByType(string pType){
			string[] vals = null;
			switch(pType){
				case "String":
					vals = new string[]{"��ͨ","��һ����ĸ","������д��ĸ","������д��ĸ","�ĸ���д��ĸ","�����д��ĸ"};
					break;
				case "DateTime":
					vals = new string[]{"��ͨ","����","����","����","����","����","��Сʱ","������"};
					break;
				case "Decimal":
				case "Int16":
				case "Int32":
				case "Int64":
					vals = new string[]{"��ͨ","10s","50s","100s","500s","1000s","5000s","10000s"};
					break;
				default:
					vals = new string[]{"��ͨ","��һ����ĸ","������д��ĸ","������д��ĸ","�ĸ���д��ĸ","�����д��ĸ"};
					break;
			}
			return vals;
		}

		
	}
}
