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
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel.RptObj
{
	public enum BarCodeType{
		Code39,//��׼39��
		EAN13,//�루EAN-13������Ʒ���룩��EAN��UPC�루��Ʒ���룬���������緶Χ��Ψһ��ʶһ����Ʒ�������ڳ���������ľ�����������
		EAN8,//�루EAN-8������Ʒ���룩��
		Code128,
        Code128A,
        Code128B,
        Code128C,
	}
	public enum AlignType {
		Left, Center, Right
	}

	public enum BarCodeWeight {
		Small = 1, Medium, Large
	}

	/// <summary>
	/// RptBarCode ���������롣
	/// </summary>
	public class RptBarCode : DIYReport.ReportModel.RptSingleObj {
		private string _FieldName;
		private string _BingDBFieldName;//�ڲ������ֶΣ�fieldName һ���Ӧ����������ֻ��BingDBFieldName �����������ݿ��Ӧ���ֶΡ�

		private AlignType _Align ;
		private String _Code ;
		private int _LeftMargin ;
		private int _TopMargin;
		private int _Height;
		private bool _ShowHeader;
		private bool _ShowFooter;
		private String _HeaderText ;
		private BarCodeWeight _Weight ;
		private Font _HeaderFont;
		private Font _FooterFont;
		private bool _DrawByBarCode;
		private BarCodeType _CodeType;
		private bool _ShowFrame;
		private float _WID;

		public RptBarCode():this(null){
		}
		/// <summary>
		/// ���캯����
		/// </summary>
		/// <param name="pName"></param>
		public RptBarCode(string pName) : base(pName,DIYReport.ReportModel.RptObjType.BarCode) {
			_Align = AlignType.Center;
			_Code = "1234567890";
			_LeftMargin = 5;
			_TopMargin = 5;
			_Height = 30;
			_ShowHeader = false;
			_ShowFooter = true;
			_HeaderText = "BarCode";
			_Weight = BarCodeWeight.Small;
			_HeaderFont = new Font("Microsoft Sans Serif",12F);
			_FooterFont = new Font("Microsoft Sans Serif", 8F);
			_CodeType = BarCodeType.Code39;
			_WID = 1f;
			_DrawByBarCode = true;
		}
		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		[Description("���û��ߵõ�ͼ������ݿ��ֶΡ�"),Category("����"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
				if(this.Section!=null && this.Section.Report!=null){ 
					string fName = DIYReport.PublicFun.GetFieldNameByDesc(_FieldName,this.Section.Report.DesignField); 
					_BingDBFieldName = fName;
				}
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Browsable(false)] 
		public string BingDBFieldName{
			get{
				return _BingDBFieldName;
			}
			set{
				_BingDBFieldName = value;
			}
		}
		[Description("���û��ߵõ��Ƿ�ͨ��barCode �������͡�"),Category("����")]
		public BarCodeType CodeType{
			get{
				return _CodeType;
			}
			set{
				_CodeType = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ��Ƿ�ͨ��barCode ���ǰ󶨵�����Դ���������롣"),Category("��Ϊ")]
		public bool DrawByBarCode{
			get{
				return _DrawByBarCode;
			}
			set{
				_DrawByBarCode = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�����Ķ��뷽ʽ��"),Category("���")]
		public AlignType VertAlign {
			get { return _Align; }
			set {
				_Align = value; 
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�����ı��롣"),Category("����")]
		public String BarCode {
			get { return _Code; }
			set { _Code = value.ToUpper();
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�����ĸ߶ȡ�"),Category("���")]
		public int BarCodeHeight {
			get { return _Height; }
			set { _Height = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ��������߾ࡣ"),Category("���")]
		public int LeftMargin {
			get { return _LeftMargin; }
			set { _LeftMargin = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�������ϱ߾ࡣ"),Category("���")]
		public int TopMargin {
			get { return _TopMargin; }
			set { _TopMargin = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ��Ƿ���ʾ����ı��⡣"),Category("���")]
		public bool ShowHeader {
			get { return _ShowHeader; }
			set { _ShowHeader = value; 
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ��Ƿ���ʾ��ע��"),Category("���")]
		public bool ShowFooter {
			get { return _ShowFooter; }
			set { _ShowFooter = value; 
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ�����ı���������"),Category("���")]
		public String HeaderText {
			get { return _HeaderText; }
			set { _HeaderText = value; 
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ����������ߵĴ�ϸ��"),Category("���")]
		public BarCodeWeight Weight {
			get { return _Weight; }
			set { _Weight = value; 
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ������������塣"),Category("���")]
		public Font HeaderFont {
			get { return _HeaderFont; }
			set { _HeaderFont = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ������ע���塣"),Category("���")]
		public Font FooterFont {
			get { return _FooterFont; }
			set { _FooterFont = value;
				if(IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ����������Ƿ���ʾ�߿�"),Category("���")]
		public bool ShowFrame {
			get {
				return _ShowFrame;
			}
			set {
				_ShowFrame = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��߻�ȡ�����ӡ��ÿ�����ؿ�ȡ�"),Category("���")]
		public float WID{
			get{
				return _WID;
			}
			set{
				_WID = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
	}
}
