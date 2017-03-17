//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	ColPivotInfo �ֶν���������϶��Ĳ�����Ϣ���á�
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace MB.XWinLib.PivotGrid
{
	/// <summary>
	/// ColPivotInfo �ֶν���������϶��Ĳ�����Ϣ���á�
	/// </summary>
	public class ColPivotInfo
	{
		#region ��������...
		private string _FieldName;//�ﶨ��Ӧ���ֶ����ơ�
		//���������϶�������(RowArea,DataArea,ColumnArea,FilterArea��������֮���ԷֺŸ��
		private DevExpress.XtraPivotGrid.PivotGridAllowedAreas _AllowedAreas;
		//��ʼ��ʱ������RowArea ���� DataArea ���� ColumnArea ���� FilterArea��
		//Ĭ������¸�������FilterArea��
		private DevExpress.XtraPivotGrid.PivotArea _IniArea;
		private int  _AreaIndex;
		//�ֶη���ͳ�Ʒ���ļ�����á��磺Alphabetical,Date,DateDay,DateDayOfWeek,DateDayOfYear,DateMonth
		//DateQuarter,DateWeekOfMonth,DateWeekOfYear,DateYear,Default,Numeric
		private DevExpress.XtraPivotGrid.PivotGroupInterval  _GroupInterval;
		private int _TopValueCount;
		private string _Description;
		//����ΪPivot �϶���������
		private string _DragGroupName;//�϶����������
		private int _DragGroupIndex;//���϶������е�˳��
		
		//ֵ��ͳ�ƻ��ܵ�����
		private string _SummaryItemType;
		
		//��ʽ����Ϣ
		private DevExpress.Utils.FormatInfo _CellFormat;
		private DevExpress.Utils.FormatInfo _ValueFormat;


        //�������ʽUnboundExpression,�����ƶ�ĳһ���е�ֵ
        private string _Expression;
        //UnboundExpression����������
        private string _ExpressionSourceColumns;

        


		#endregion ��������...

		#region ���캯��...
		/// <summary>
		/// ���캯����
		/// </summary>
		public ColPivotInfo(string fieldName)
		{
			_FieldName = fieldName;
			
			_CellFormat = DevExpress.Utils.FormatInfo.Empty; 
			_ValueFormat = DevExpress.Utils.FormatInfo.Empty; 
		}
		#endregion ���캯��...

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
		public DevExpress.XtraPivotGrid.PivotGridAllowedAreas AllowedAreas{
			get{
				return _AllowedAreas;
			}
			set{
				_AllowedAreas = value;
			}
		}
		public DevExpress.XtraPivotGrid.PivotArea IniArea{
			get{
				return _IniArea;
			}
			set{
				_IniArea = value;
			}
		}
		public int AreaIndex{
			get{
				return _AreaIndex;
			}
			set{
				_AreaIndex = value;
			}
		}
		public DevExpress.XtraPivotGrid.PivotGroupInterval GroupInterval{
			get{
				return _GroupInterval;
			}
			set{
				_GroupInterval = value;
			}
		}
		public int TopValueCount{
			get{
				return _TopValueCount;
			}
			set{
				_TopValueCount = value;
			}
		}
		public string DragGroupName{
			get{
				return _DragGroupName;
			}
			set{
				_DragGroupName = value;
			}
		}
		public int DragGroupIndex{
			get{
				return _DragGroupIndex;
			}
			set{
				_DragGroupIndex = value;
			}
		}
		public DevExpress.Utils.FormatInfo CellFormat{
			get{
				return _CellFormat;
			}
			set{
				_CellFormat = value;
			}
		}
		public DevExpress.Utils.FormatInfo ValueFormat{
			get{
				return _ValueFormat;
			}
			set{
				_ValueFormat = value;
			}
		}
		public string SummaryItemType{
			get{
				return _SummaryItemType;
			}
			set{
				_SummaryItemType = value;
			}
		}

        public string Expression
        {
            get { return _Expression; }
            set { _Expression = value; }
        }

        public string ExpressionSourceColumns
        {
            get { return _ExpressionSourceColumns; }
            set { _ExpressionSourceColumns = value; }
        }
		#endregion Public ����...


	}
}
