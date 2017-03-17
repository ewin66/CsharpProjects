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
using System.Drawing ;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace DIYReport.ReportModel
{
	/// <summary>
	/// RptTextObj �������ı�����
	/// </summary>
	public class RptTextObj : RptSingleObj, DIYReport.Interface.IRptTextObj 
	{
		private Font _Font;
		private bool _WordWrap;
		private bool _ShowFrame;
		private StringAlignment _Alignment;
		private string _FormatStyle;	//��ʽ����ʽ
		private bool _ToUpperMoney; //ת��Ϊ��д�Ľ��
		private bool _ToUpperEnglish;

		public RptTextObj(string pName ,DIYReport.ReportModel.RptObjType pType) : base(pName,pType){
			_Font = new Font("Tahoma",9);
			_Alignment = StringAlignment.Near ;
			_ShowFrame = true;
		}
		#region Public ����...

		[Description("���û��ߵõ���ʽ�����塣"),Category("���")]
		public Font Font {
			get {
				return _Font;
			}
			set {
				_Font = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ����������Ƿ��Զ��������塣"),Category("���")]
		public bool WordWrap {
			get {
				return _WordWrap;
			}
			set {
				_WordWrap = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
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
		[Description("���û��ߵõ�������ַ��ܵĶ��뷽ʽ��"),Category("���")]
		public StringAlignment Alignment {
			get {
				return _Alignment;
			}
			set {
				_Alignment = value;

				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		public string FormatStyle {
			get {
				return _FormatStyle;
			}
			set {
				_FormatStyle=value;
			}
		}
		[Description("���û����Ƿ�ѽ��ת��Ϊ��д�Ľ�"),Category("���")]
		public bool ToUpperMoney{
			get{
				return _ToUpperMoney;
			}
			set{
				_ToUpperMoney = value;
			}
		}
		[Description("���û����Ƿ�ѽ��ת��ΪӢ�Ĵ�д�Ľ�"),Category("���")]
		public bool ToUpperEnglish{
			get{
				return _ToUpperEnglish;
			}
			set{
				_ToUpperEnglish = value;
			}
		}
		#endregion Public ����...
	}
}
