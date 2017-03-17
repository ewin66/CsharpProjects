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
using System.Diagnostics ;
using System.Drawing ;
using System.Xml ;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel
{
	/// <summary>
	/// RptSingleObj ��������ʹ�ӡ�ĵ�������
	/// </summary>
	public class RptSingleObj : DIYReport.Interface.IRptSingleObj {
		#region ��������...
		private string _Name;
		private RptObjType  _Type;
		private Point _Location;
		private Size _Size;
		private Rectangle _Rect;
 
		private int _Left;
		private int _Top;
		private int _Right;
		private int _Bottom;

		private RptSection  _Section;
		 
		private Color _ForeColor;
		private Color _BackgroundColor;
	 
		private int _LinePound = 1;//�ߵĿ��
		private System.Drawing.Drawing2D.DashStyle _LineStyle;//�ߵ���ʽ

		//���û��ö����ʱ������Ƿ�����¼� 
		private bool _RaiseEvent =false;
		private bool _IsEndUpdate = true;
		private bool _IsTranspControl = false;//͸���Ŀؼ�




		#endregion ��������...


		#region �Զ����¼�...
		private RptEventHandler _AfterValueChanged;
		public event RptEventHandler AfterValueChanged{
			add{
				_AfterValueChanged +=value;
			}
			remove{
				_AfterValueChanged -=value;
			}
		}
		protected void OnAfterValueChanged(RptEventArgs arg){
			if(_AfterValueChanged!=null){
				DIYReport.UserDIY.DesignEnviroment.DesignHasChanged = true;
				_AfterValueChanged(this,arg);
			}

		}
		#endregion �Զ����¼�...
		
		#region ���캯��...
		public RptSingleObj(){
		}
		public RptSingleObj(string pName,DIYReport.ReportModel.RptObjType pType ){
			_Name = pName;
			_Type =  pType;
			_ForeColor = Color.Black ;
			_BackgroundColor = Color.White ;
			 
			_Location = new Point(0,0);
			Size = new Size(100,25);
		}
		#endregion ���캯��...

		#region ��չ��Public ����...
		public void BeginUpdate(){
			_IsEndUpdate = false;
		}
		public void EndUpdate(){
			_IsEndUpdate = true;
			OnAfterValueChanged(new RptEventArgs());
		}
		#endregion ��չ��Public ����...

		#region �ڲ�������...
		//�õ����ı��Ŀ�ʼ��

		#endregion �ڲ�������...

		#region Public ����...
		[Description("�õ���ӡ�ؼ������Left��"),Category("����")]
		public int Left{
			get{
				return _Left;
			}
		}
		[Description("�õ���ӡ�ؼ������Top��"),Category("����")]
		public int Top{
			get{
				return _Top;
			}
		}
		[Description("�õ���ӡ�ؼ������Right��"),Category("����")]
		public int Right{
			get{
				return _Right;
			}
		}
		[Description("�õ���ӡ�ؼ������Bottom��"),Category("����")]
		public int Bottom{
			get{
				return _Bottom;
			}
		}

		[Browsable(false)]
		public bool IsTranspControl{
			get{
				return _IsTranspControl;
			}
			set{
				_IsTranspControl = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ��߿���������ʽ��"),Category("���")]
		public  System.Drawing.Drawing2D.DashStyle LineStyle{
			get{
				return _LineStyle;
			}
			set{
				if(value!=System.Drawing.Drawing2D.DashStyle.Custom){
					_LineStyle = value;
					if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
				}
			}
		}
		[Description("���û��ߵõ��߿������Ŀ�ȡ�"),Category("���")]
		public int LinePound{
			get{
				return _LinePound;
			}
			set{
				_LinePound = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false)]
		public RptSection Section {
			get {
				return _Section;
			}
			set {
				_Section = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false),Description("���û��ߵõ���ӡ�ؼ����ݵ�������Դ���͡�"),Category("��Ϊ")]
		public RptObjType Type {
			get {
				return _Type;
			}
			set {
				_Type = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}

		[Description("���û��ߵõ���ӡ�ؼ������λ�á�"),Category("����")]
		public Point Location{
			get{
				return _Location;
			}
			set{
				_Location = value; 
				_Left = _Location.X;
				_Top = _Location.Y ;
				_Right = _Location.X + _Size.Width ;
				_Bottom = _Location.Y + _Size.Height ;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ���ӡ�ؼ�����Ĵ�С��"),Category("����")]
		public Size Size{
			get{
				return _Size;
			}
			set{
				_Size = value;
				_Right = _Location.X + _Size.Width ;
				_Bottom = _Location.Y + _Size.Height ;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false)]
		public Rectangle Rect {
			get {
				return new Rectangle(_Location,_Size);
			}
			set {
				_Rect = value;
				_Location = _Rect.Location ;
				_Size = _Rect.Size ;
			}
		}
		[Browsable(false)]
		public Rectangle InnerRect{
			get{
				return new Rectangle(0,0,_Size.Width - 1,_Size.Height - 1);
			}
		}
//		[Description("���û��ߵõ���ӡ�ؼ��������ʽ������"),Category("���"),
//		Editor(typeof(DIYReport.Design.RptCssClassAttributesEditor), typeof(UITypeEditor))]
//		public RptCssClass CssClass {
//			get {
//				return _CssClass;
//			}
//			set {
//				_CssClass = value;
//			}
//		}
		 
		[Description("���û��ߵõ���ʽ�����ơ�"),Category("���")]
		public string Name {
			get {
				return _Name;
			}
			set {
				_Name = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ���ʽ��������ɫ��"),Category("���")]
		public Color ForeColor {
			get {
				return _ForeColor;
			}
			set {
				_ForeColor = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Description("���û��ߵõ���ӡ����ı�����ɫ��"),Category("���")]
		public Color BackgroundColor {
			get {
				return _BackgroundColor;
			}
			set {
				_BackgroundColor = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
 
 
		[Description("���û��ߵõ��ڴ�ӡ���������ʱ���Ƿ����һ���ڲ����¼�"),Category("��Ϊ"),DefaultValue(false)]
		public bool RaiseEvent {
			get {
				return _RaiseEvent;
			}
			set {
				_RaiseEvent=value;
			}
		}
		[Browsable(false)]
		public bool IsEndUpdate{
			get{
				return _IsEndUpdate;
			}
		}
		#endregion Public ����...

		#region ICloneable Members
		
		public virtual object WiseClone(){
			object info = this.MemberwiseClone() as object ;
			return info;
		}
		public  virtual object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
		object ICloneable.Clone() {
			return Clone();
		}

		#endregion ICloneable Members
	}
}
