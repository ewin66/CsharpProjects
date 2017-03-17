//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-07
// Description	:	RptBarCode ���������롣
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptHViewSpecFieldBox ��Ҫ���к���������ʾ�����ݿ�
	/// </summary>
	public class RptHViewSpecFieldBox : DIYReport.ReportModel.RptTextObj
	{
		public static readonly string SIZE_FIRST_PREX = "Size_";
		public static readonly string COLOR_FIRST_PREX = "Color_";
		public static readonly string SHIP_PORT_PREX = "ShipPort_";
		public static readonly string ACTIVE_GROUP_FIELD_IDENTITY = "_{0}@@";

		private string _FieldName;
		private string _GroupDisplayField;
		private string _ActiveOrderField;//

		private string _SubTotalFields;
		private string _ForLetHViewKeysFields;
		private string _MapHViewColDataField;
		private ActiveViewArea _DataViewArea;
		private ActiveDataType _ActiveDataType;
		private int _CellWidth;
		private bool _AutoSize;
		

		#region ���캯��...
		/// <summary>
		/// 
		/// </summary>
		public RptHViewSpecFieldBox() : this(null){
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RptHViewSpecFieldBox(string name) : base(name,DIYReport.ReportModel.RptObjType.HViewSpecField)
		{
			_FieldName = DIYReport.Drawing.RptDrawHelper.NO_BING_TAG ; 

			_CellWidth = 26;
		}
		#endregion ���캯��...

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		#region public ����...
		[Description("��̬��ʾ�ֶ����͡�"),Category("����")]
		public ActiveDataType ActiveDataType{
			get{
				return _ActiveDataType;
			}
			set{
				_ActiveDataType = value;
			}
		}
		[Description("����������ʾ������"),Category("����")]
		public ActiveViewArea DataViewArea{
			get{
				return _DataViewArea;
			}
			set{
				_DataViewArea = value;
			}
		}
		[Description("Ĭ�ϵ�Ԫ����ʾ��ȡ�"),Category("����")]
		public int CellWidth{
			get{
				return _CellWidth;
			}
			set{
				_CellWidth = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("�ж��Ƿ�����Ϊ�Զ���Ӧ��ȡ�"),Category("����")]
		public bool AutoSize{
			get{
				return _AutoSize;
			}
			set{
				_AutoSize = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false),Description("��Ҫ���к���ת�����ֶ����ơ�"),Category("����"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}

		[Browsable(false),Description("���û��߻�ȡ�ں���ת������Ҫ���л��ܵ��ֶ�����,����ֶ�֮���÷ֺŸ��"),Category("����")]
		public string SubTotalFields{
			get{
				return _SubTotalFields;
			}
			set{
				_SubTotalFields = value;
			}
		}
		[Browsable(false),Description("���û��߻�ȡ�ں���ת������Ҫ����Ϊ�������ֶ�����,����ֶ�֮���÷ֺŸ��"),Category("����")]
		public string ForLetHViewKeysFields{
			get{
				return _ForLetHViewKeysFields;
			}
			set{
				_ForLetHViewKeysFields = value;
			}
		}
		[Browsable(false),Description("���û��߻�ȡ�ں���ת����ӳ�䵽ת����������ֶΡ�"),Category("����")]
		public string MapHViewColDataField{
			get{
				return _MapHViewColDataField;
			}
			set{
				_MapHViewColDataField = value;
			}
		}
		[Description("��̬���������ʾ�ֶ����ơ�"),Category("��ʾ"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string GroupDisplayField{
			get{
				return _GroupDisplayField;
			}
			set{
				_GroupDisplayField = value;
			}
		}
		[Description("��̬����������ֶ����ơ�"),Category("��ʾ"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string ActiveOrderField{
			get{
				return _ActiveOrderField;
			}
			set{
				_ActiveOrderField = value;
			}
		}

		#endregion public ����...
	}
	/// <summary>
	/// ��̬��ʾ�������͡���ϵͳ�������ô���ʽ��
	/// </summary>
	public enum ActiveDataType{
		Size,
		Color,
		ShipPort
	}

	public enum ActiveViewArea{
		CaptionArea, //��������
		DetailArea, //�ֶ���ϸ����
		SumArea     //��������
	}
}
