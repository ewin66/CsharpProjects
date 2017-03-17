//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-04-19
// Description	:	�����������ݡ�
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics; 

namespace DIYReport.GroupAndSort
{
	/// <summary>
	/// GroupDataProcess �����������ݡ�
	/// </summary>
	public class GroupDataProcess
	{
		#region public ����...
		/// <summary>
		/// �ѱ����ӡ�����ݰ���ͳ�Ʒ���ķ�ʽ��������
		/// </summary>
		/// <param name="pDs"></param>
		/// <param name="pDataReport"></param>
		/// <returns></returns>
		public static DataRow[] SortData(object pDs,DIYReport.ReportModel.RptReport pDataReport){
			if(pDs==null)
				return new DataRow[0];
			DataView dv = PublicFun.GetDataViewByObject(pDs);

			//����������ַ���
			string sortStr = getSortStr(dv.Table,pDataReport);
			return dv.Table.Select(dv.RowFilter,sortStr);
		}

		public static bool ValueInTheGroup(DIYReport.ReportModel.RptSection pGroupSection,object pGroupValue , object pValue){
			if(pGroupValue==null){
				return true;
			}
			//�ȴ���򵥵���ȷ���
			if(pGroupValue == System.DBNull.Value){
				if( pValue == System.DBNull.Value){
					return true;
				}
			}
			if (pValue == System.DBNull.Value) {
				return false;
			}
			return pGroupValue.ToString() == pValue.ToString();

		}
		#endregion public ����...

		#region �ڲ�������...
		//����������ַ���
		private static string getSortStr(DataTable pDt, DIYReport.ReportModel.RptReport pDataReport){
			DIYReport.ReportModel.RptSectionList sectionList =   pDataReport.SectionList; 
			string filterStr = "";
			foreach(DIYReport.ReportModel.RptSection section in sectionList){
				if(section.SectionType == DIYReport.SectionType.GroupHead){
					string asc = section.GroupField.IsAscending ? " ASC ," :" DESC ,";
					string sname = section.GroupField.FieldName;
					if(pDt.Columns.Contains(sname)){
						filterStr += section.GroupField.FieldName  + asc;
					}
				}
			}
			if( filterStr!=""){
				filterStr = filterStr.Remove(filterStr.Length - 1,1);
			}
			return filterStr;
		}
		#endregion �ڲ�������...
	}
}
