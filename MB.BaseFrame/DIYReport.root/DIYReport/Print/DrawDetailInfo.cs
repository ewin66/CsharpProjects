//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-04-29
// Description	:	����Report Detail ʱ����ǰҪ�洢�Ĵ�����Ϣ
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Collections;


using DIYReport.GroupAndSort; 
using DIYReport.Print;
namespace DIYReport.Print
{
	#region ���Ʊ���ʱ�������Ϣ...
	public struct ReportDrawInfo{
		private int _TitleHeight;
		private int _PageHeadHeight;
		private int _DetailHeight;
		private int _PageFooterHeight;
		private int _BottomHeight;

		public ReportDrawInfo(DIYReport.ReportModel.RptReport  _Report){
			DIYReport.ReportModel.RptSectionList sectionList = _Report.SectionList;
			_TitleHeight = sectionList.GetSectionHeightByType( DIYReport.SectionType.ReportTitle); 
			_PageHeadHeight = sectionList.GetSectionHeightByType( DIYReport.SectionType.PageHead); 
			_DetailHeight = sectionList.GetSectionHeightByType( DIYReport.SectionType.Detail); 
			_PageFooterHeight = sectionList.GetSectionHeightByType( DIYReport.SectionType.PageFooter); 
			_BottomHeight = sectionList.GetSectionHeightByType( DIYReport.SectionType.ReportBottom); 
		}
		#region Public ����...
		public int TitleHeight{
			get{
				return _TitleHeight;
			}
			set{
				_TitleHeight = value;
			}
		}
		public int PageHeadHeight{
			get{
				return _PageHeadHeight;
			}
			set{
				_PageHeadHeight = value;
			}
		}
		public int DetailHeight{
			get{
				return _DetailHeight;
			}
			set{
				_DetailHeight = value;
			}
		}
		public int PageFooterHeight{
			get{
				return _PageFooterHeight;
			}
			set{
				_PageFooterHeight = value;
			}
		}
		public int BottomHeight{
			get{
				return _BottomHeight;
			}
			set{
				_BottomHeight = value;
			}
		}

		#endregion Public ����...
	}
	#endregion ���Ʊ���ʱ�������Ϣ...

	#region �ڻ�����ͳ�Ƶ�ʱ�򣬴洢�����ֶθ�ʽ...
	public class DrawGroupField{
		#region �ڲ���������...
		//��������ĵ�һ����Index  �� -1 Ϊû�п�ʼ���з����÷�����ֶ�
		private int _FirstRowIndex;
		private int _PrevFirstRowIndex = 0;
		//������ֶ�����
		private string _GroupFieldName;
		//��ǰ�������Ӧ��ֵ
		private object _CurrGroupValue;
		//�ж��Ƿ��Ѿ�����Group Section ��ͷ
		private bool _HasDrawGroupHead;
		#endregion �ڲ���������...
		
		#region ���캯��...
		/// <summary>
		///  ���캯��
		/// </summary>
		/// <param name="pFirstRowIndex"></param>
		/// <param name="pGroupFieldName"></param>
		public DrawGroupField(int pFirstRowIndex , string pGroupFieldName){
			_FirstRowIndex = pFirstRowIndex;
			_CurrGroupValue = null;
			_GroupFieldName = pGroupFieldName;
			_HasDrawGroupHead = false;
		}
		#endregion ���캯��...

		#region Public ����...
		public int FirstRowIndex{
			get{
				return _FirstRowIndex;
			}
			set{
				_FirstRowIndex = value;
			}
		}
		public int PrevFirstRowIndex{
			get{
				return _PrevFirstRowIndex;
			}
			set{
				_PrevFirstRowIndex = value;
			}
		}
		public string GroupFieldName{
			get{
				return _GroupFieldName;
			}
			set{
				_GroupFieldName = value;
			}
		}
		public object CurrGroupValue{
			get{
				return _CurrGroupValue;
			}
			set{
				_CurrGroupValue = value;
			}
		}
		public bool HasDrawGroupHead{
			get{
				return _HasDrawGroupHead;
			}
			set{
				_HasDrawGroupHead = value;
			}
		}
		#endregion Public ����...


	}
	#endregion �ڻ�����ͳ�Ƶ�ʱ�򣬴洢�����ֶθ�ʽ...

	#region DrawDetailInfo...
	/// <summary>
	/// DrawDetailInfo ����Report Detail ʱ����ǰҪ�洢�Ĵ�����Ϣ��
	/// </summary>
	public class DrawDetailInfo
	{
		#region ��������...
		//��ǰҳ�ĵ�һ���е�Index 
		private int _CurrPageFirstRowIndex;
		//��ǰ���ڻ��Ƶ��е�Index 
		private int _CurrDrawRowIndex;
		//��ǰҳ��˳���
		private int _CurrPageNumber;
		//������ֶ���Ϣ
		private IList  _GroupFields;
		#endregion ��������...
		
		#region ���캯��...
		/// <summary>
		///  ����DesignEnviroment�е�DesignField�����һ������Detail��ʱ�洢����Ϣ
		/// </summary>
		/// <param name="pDataReport"></param>
		public DrawDetailInfo(DIYReport.ReportModel.RptReport report )
		{
			//-1 ��ʾ��ǰҳ��û�п�ʼ����
			_CurrPageFirstRowIndex = -1;
			_CurrPageNumber = -1;

			IList fieldList = report.DesignField;
			//�Է�����ֶν��������Ի�ȡ�����ֶε�˳��
			(fieldList as ArrayList).Sort(new FieldSortComparer());  
			_GroupFields = new ArrayList();
			foreach(DIYReport.GroupAndSort.RptFieldInfo field in fieldList){
				if(field.IsGroup){
					DrawGroupField drwafield = new DrawGroupField(-1,field.FieldName); 
					_GroupFields.Add(drwafield); 
				}
			}  
		}
		#endregion ���캯��...

		#region Public ����...
		/// <summary>
		/// �õ����Ʒ����Ӧ�ֶδ�����Ϣ 
		/// </summary>
		/// <param name="pFieldName">������ֶ�����</param>
		/// <returns></returns>
		public DrawGroupField GetGroupFieldByName(string pFieldName){
			int count = _GroupFields.Count ;
			for(int i = 0 ; i < count;i++){
				DrawGroupField field = _GroupFields[i] as DrawGroupField;
				if(field.GroupFieldName == pFieldName){
					return field;
				}
			}
			return (DrawGroupField)null;
		}
		#endregion Public ����...

		#region Public ����...
		/// <summary>
		/// ��ǰҳ��Index
		/// </summary>
		public int CurrPageFirstRowIndex{
			get{
				return _CurrPageFirstRowIndex;
			}
			set{
				_CurrPageFirstRowIndex = value;
			}
		} 
		/// <summary>
		/// ��ǰ����ҳ��number 
		/// </summary>
		public int CurrPageNumber{
			get{
				return _CurrPageNumber;
			}
			set{
				_CurrPageNumber = value;
			}
		}
		/// <summary>
		/// ������ֶ� 
		/// </summary>
		public IList GroupFields{
			get{
				return _GroupFields;
			}
			set{
				_GroupFields = value;
			}
		}
		/// <summary>
		/// ��ǰ�Ѿ����Ƶ����е�index 
		/// </summary>
		public int CurrDrawRowIndex{
			get{
				return _CurrDrawRowIndex;
			}
			set{
				_CurrDrawRowIndex = value;
			}
		}
		#endregion Public ����...
	}

	#endregion DrawDetailInfo...
}
