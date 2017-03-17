//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	ColPivotList �ֶν���������϶��Ĳ��������ࡣ
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MB.XWinLib.PivotGrid
{
	/// <summary>
	/// ColPivotList �ֶν���������϶��Ĳ��������ࡣ
	/// </summary>
	public class ColPivotList 
	{
		private IList<ColPivotInfo> _Columns;
		//�ڲ����õ�������Ƿ񴴽����Ӧ���ֶ��
		private bool _AutoCreatedGridField;
		//�ж��ֶ�������ͬ��pivotefield �Ƿ�������һ��
		private bool _SameFieldGroup;
		//�ֶΰﶨ�������� ��ͬ����֮���ԷֺŸ��ͬһ��Ĳ�ͬ�ֶ�֮���Զ��ŷֿ���
		private  string _FieldGroups;
		//private Il
		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		public ColPivotList() 
		{
			_Columns = new List<ColPivotInfo>() ;
			_SameFieldGroup = true;
			_AutoCreatedGridField = true;
		}
		#endregion ���캯��...

 
		
		#region Public ����...
		/// <summary>
		/// �����ֶε����ƻ�ȡ���ö�Ӧ����Ϣ���п��ܴ���һ���ֶζ�Ӧ���Pivot�������Ϣ��
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public IList<ColPivotInfo> GetColPivotInfos(string fieldName){
			IList<ColPivotInfo> pivots = new List<ColPivotInfo>();
			foreach(ColPivotInfo info in this.Columns){
				if(string.Compare(info.FieldName,fieldName,true)!=0)
					continue;
				pivots.Add(info);
			}
			return pivots;
		}

		/// <summary>
		/// Public ������
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public ColPivotInfo Add(ColPivotInfo info){
			_Columns.Add(info);
			return info;
		}
		#endregion Public ����...

		#region ��չ��Public ����
		/// <summary>
		/// �����е�������Ϣ��
		/// </summary>
        public IList<ColPivotInfo> Columns {
			get{
				return _Columns;
			}
		}

		public bool AutoCreatedGridField{
			get{
				return _AutoCreatedGridField;
			}
			set{
				_AutoCreatedGridField = value;
			}
		}
		public bool SameFieldGroup{
			get{
				return _SameFieldGroup;
			}
			set{
				_SameFieldGroup = value;
			}
		}
		public string FieldGroups{
			get{
				return _FieldGroups;
			}
			set{
				_FieldGroups = value;
			}
		}
		#endregion ��չ��Public ����
	}
}
