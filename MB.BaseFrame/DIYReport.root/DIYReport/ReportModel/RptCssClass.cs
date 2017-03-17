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
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel
{
	/// <summary>
	/// CssClass ��ӡ������ʽ�����á�
	/// </summary>
	public class RptCssClass
	{
		private string _Key;
		private string _Name;
		private Font _Font;
		private Color _FontColor;
		private Color _BackgroundColor;
		private bool _WordWrap;
		private bool _ShowFrame;
		private StringAlignment _Alignment;
		private string _FormatStyle;	//��ʽ����ʽ
		//���û��ö����ʱ������Ƿ�����¼� //
		private bool _RaiseEvent =false;
		public RptCssClass(string pKey){
			_Key = pKey;
			_Name = pKey;
			_Font = new Font("Tahoma",9);
			_FontColor = Color.Black ;
			_BackgroundColor  = Color.White ;

		}
		public RptCssClass(XmlNode pNode) {
			try {
				int fontSize = int.Parse(pNode.Attributes["Font-Size"].Value);
				_Font = new Font( "Tahoma",fontSize);
				//_Font.Bold = bool.Parse(pNode.Attributes["Font-Bold"].Value );
				_Name = pNode.Attributes["Name"].Value ;
				//��XML�ļ������õ�������ɫת����ϵͳ����ɫ 
				_FontColor = System.Drawing.ColorTranslator.FromHtml(pNode.Attributes["Font-Color"].Value );
				_BackgroundColor = System.Drawing.ColorTranslator.FromHtml(pNode.Attributes["Background-Color"].Value );

				_WordWrap = bool.Parse(pNode.Attributes["WordWrap"].Value ); 
				_ShowFrame = bool.Parse(pNode.Attributes["ShowFrame"].Value ); 
				string align = pNode.Attributes["Alignment"].Value;
				switch(align.ToUpper()) {
					case "LEFT":
						_Alignment = StringAlignment.Near ; 
						break;
					case "RIGHT":
						_Alignment = StringAlignment.Far;
						break;
					case "MIDDLE" :
						_Alignment = StringAlignment.Center;
						break;
					default:
						_Alignment = StringAlignment.Near ; 
						break;
				}
				// 
				if (pNode.Attributes["FormatStyle"]!=null) {
					_FormatStyle=pNode.Attributes["FormatStyle"].Value;
				}
				if (pNode.Attributes["RaiseEvent"]!=null) {
					_RaiseEvent=bool.Parse(pNode.Attributes["RaiseEvent"].Value);
				}

			}
			catch(Exception e) {
				Debug.Assert(false,"�õ������CssClass�д�!",e.ToString());
			}
		}

		#region Public ����...
		[Browsable(false)]
		public string Key{
			get{
				return _Key;
			}
			set{
				_Key = value;
			}
		}
		[Description("���û��ߵõ���ʽ�����ơ�"),Category("����")]
		public string Name {
			get {
				return _Name;
			}
			set {
				_Name = value;
			}
		}
		[Description("���û��ߵõ���ʽ�����塣"),Category("����")]
		public Font Font {
			get {
				return _Font;
			}
			set {
				_Font = value;
			}
		}
		[Description("���û��ߵõ���ʽ��������ɫ��"),Category("����")]
		public Color FontColor {
			get {
				return _FontColor;
			}
			set {
				_FontColor = value;
			}
		}
		[Description("���û��ߵõ���ӡ����ı�����ɫ��"),Category("����")]
		public Color BackgroundColor {
			get {
				return _BackgroundColor;
			}
			set {
				_BackgroundColor = value;
			}
		}
		[Description("���û��ߵõ����������Ƿ��Զ��������塣"),Category("����")]
		public bool WordWrap {
			get {
				return _WordWrap;
			}
			set {
				_WordWrap = value;
			}
		}
		[Description("���û��ߵõ����������Ƿ���ʾ�߿�"),Category("����")]
		public bool ShowFrame {
			get {
				return _ShowFrame;
			}
			set {
				_ShowFrame = value;
			}
		}
		[Description("���û��ߵõ�������ַ��ܵĶ��뷽ʽ��"),Category("����")]
		public StringAlignment Alignment {
			get {
				return _Alignment;
			}
			set {
				_Alignment = value;
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
		[Description("���û��ߵõ��ڴ�ӡ���������ʱ���Ƿ����һ���ڲ����¼�"),Category("����"),DefaultValue(false)]
		public bool RaiseEvent {
			get {
				return _RaiseEvent;
			}
			set {
				_RaiseEvent=value;
			}
		}
		#endregion Public ����...

		#region ���ǻ���ķ���...
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return _Name;
		}

		#endregion ���ǻ���ķ���...

		//��ʽ���ַ���  
		public string FormatString(object pObj) {
			string strRet="";
			if (this.FormatStyle!=null) {
				switch (pObj.GetType().Name.ToLower()) {
					case "datetime":
						strRet=Convert.ToDateTime(pObj).ToString(this.FormatStyle);
						break;
					case "int16":
					case "int32":
					case "int64":
					case "uint16":
					case "uint32":
					case "uint64":
					case "single":
					case "double":
					case "decimal":
						strRet=Convert.ToDecimal(pObj).ToString(this.FormatStyle);
						break;
					default:
						strRet=pObj.ToString();
						break;
				}
			}
			else {
				strRet=pObj.ToString();
			}
			return strRet;
		}
	}
}
