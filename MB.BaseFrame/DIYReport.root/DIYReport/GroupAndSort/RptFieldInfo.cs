//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-04-19
// Description	:	��������ֶε�����
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace DIYReport.GroupAndSort
{
	/// <summary>
	/// FieldInfo ��������ֶε�����
	/// </summary>
	public class RptFieldInfo : ICloneable	 
	{
		#region ��������...
		private string _FieldName;
		private string _Description;
		private string _DataType;
		private int _OrderIndex;
		//�ж��Ƿ�ѡ����Ϊ������ֶ�
		private bool _IsGroup;
		//�ڶ��ֶη����е�˳��(��0��ʼ)
		//private int _OrderIndex;
		//����ļ������
		private string _DivideName;
		//���ֶ��Ƿ�Ϊ�������� 
		private bool _IsAscending;
		//�ж��Ƿ���������Ŀ���
		private bool _SetSort;
		#endregion ��������...

		#region ���캯��...
		public RptFieldInfo(){
		}
		public RptFieldInfo(string pFieldName){
			_FieldName = pFieldName;
			_Description = pFieldName;
			_DataType = "String";
		}
		public RptFieldInfo(string pFieldName,string pDescription ,string pDataType,int pOrderIndex){
			_FieldName = pFieldName;
			_Description = pDescription;
			_DataType = pDataType;
			_OrderIndex = pOrderIndex;
		}
		#endregion ���캯��...

		#region ����ToString����...
		public override string ToString() {
			return _Description;
		}

		#endregion ����ToString����...

		public object Clone(){
			return this.MemberwiseClone();
		}

		#region Public ����...
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
			}
		}
		public string Description{
			get{
				return _Description;
			}
			set{
				_Description = value;
			}
		}
		public string DataType{
			get{
				return _DataType;
			}
			set{
				_DataType = value;
			}
		}
		public int OrderIndex{
			get{
				return _OrderIndex;
			}
			set{
				_OrderIndex = value;
			}
		}
		public bool IsGroup{
			get{
				return _IsGroup;
			}
			set{
				_IsGroup = value;
			}
		}
		public string  DivideName{
			get{
				return _DivideName;
			}
			set{
				_DivideName = value;
			}
		} //
		public bool IsAscending{
			get{
				return _IsAscending;
			}
			set{
				_IsAscending = value;
			}
		}
		public bool SetSort{
			get{
				return _SetSort;
			}
			set{
				_SetSort = value;
			}
		}
		#endregion Public ����...
	}
}
