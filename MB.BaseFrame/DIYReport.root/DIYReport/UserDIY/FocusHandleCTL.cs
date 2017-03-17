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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DIYReport.UserDIY
{

	/// <summary>
	/// FocusHandle ���㡣
	/// </summary>
	[ToolboxItem(false)]
	public class FocusHandleCTL : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panBack;

		private FocusHandleList _FocusList;

		#region �ڲ��Զ����ɴ���...
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary> 
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.panBack = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panBack
			// 
			this.panBack.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
			this.panBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panBack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panBack.Location = new System.Drawing.Point(0, 0);
			this.panBack.Name = "panBack";
			this.panBack.Size = new System.Drawing.Size(104, 24);
			this.panBack.TabIndex = 0;
			this.panBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panBack_MouseUp);
			this.panBack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panBack_MouseMove);
			this.panBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panBack_MouseDown);
			// 
			// FocusHandleCTL
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.panBack);
			this.Name = "FocusHandleCTL";
			this.Size = new System.Drawing.Size(104, 24);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion �ڲ��Զ����ɴ���...
		private static int FOCUS_WIDTH = 6;
		private static int FOCUS_HEIGHT = 6;

		private HandleType _FocusType;
		#region ���캯��...
		public FocusHandleCTL(FocusHandleList pFocusList,HandleType pFocusType) : this(pFocusList,pFocusType,new Point(0,0)){
		}
		public FocusHandleCTL(FocusHandleList pFocusList,HandleType pFocusType,int X,int Y) : this(pFocusList,pFocusType,new Point(X,Y)){
		}
		public FocusHandleCTL(FocusHandleList pFocusList ,HandleType pFocusType,Point pPosition) {
			InitializeComponent();
			_FocusType = pFocusType;
			_FocusList = pFocusList;
			setCursor(_FocusType);
			
			this.Resize +=new EventHandler(FocusHandle_Resize);

			this.Width = FOCUS_WIDTH;
			this.Height = FOCUS_HEIGHT;
		}
		#endregion ���캯��...

		#region Public ����...

		public FocusHandleList FocusList{
			get{
				return _FocusList;
			}
			set{
				_FocusList = value;
			}
		}
		public HandleType FocusType{
			get{
				return _FocusType;
			}
			set{
				_FocusType = value;
				setCursor(_FocusType);
			}
		}
		public Rectangle Rect{
			get{
				return new Rectangle(this.Location ,this.Size) ;
			}
		}
		#endregion Public ����...

		#region public ����...
		/// <summary>
		/// ���ý������ɫ
		/// </summary>
		/// <param name="pColor"></param>
		public void SetBackColor(Color pColor){
			panBack.BackColor =  pColor;
		}
		public void SetFocusType(int pIndex){
			_FocusType = (HandleType) (pIndex % 8);
			setCursor(_FocusType);
		}
		#endregion public ����...

		#region �ڲ�������...
		private void setCursor(HandleType pType){
			switch(pType){
				case HandleType.LeftTop :
					panBack.Cursor = Cursors.SizeNWSE;  
					break;
				case HandleType.MiddleTop  :
					panBack.Cursor = Cursors.SizeNS;  
					break;
				case HandleType.RightTop :
					panBack.Cursor = Cursors.SizeNESW;  
					break;
				case HandleType.RightMiddle :
					panBack.Cursor = Cursors.SizeWE;  
					break;
				case HandleType.RightBottom :
					panBack.Cursor = Cursors.SizeNWSE;  
					break;
				case HandleType.BottomMiddle :
					panBack.Cursor = Cursors.SizeNS;  
					break;
				case HandleType.LeftBottom :
					panBack.Cursor = Cursors.SizeNESW;  
					break;
				case HandleType.LeftMiddle :
					panBack.Cursor = Cursors.SizeWE;  
					break;
				default :
					break;
			}
		}
		#endregion �ڲ�������...

		#region �����¼�...
		private void FocusHandle_Resize(object sender, EventArgs e) {
			this.Width = FOCUS_WIDTH;
			this.Height = FOCUS_HEIGHT;
		}
		#endregion �����¼�...

		private bool isdown = false;
		private Point p1 = Point.Empty;
		
		private Point p2 = Point.Empty;
		private void panBack_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			 
			isdown=true;
			panBack.Capture = true;
			p1 =  new Point(this.Left + e.X , this.Top + e.Y) ;
			//DesignControl ctl =  dList.DEControls.GetControlByPoint(p1);
			//dList.DEControls.SetAllNotSelected(); 
			 
		}

		private void panBack_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(isdown){
				int x =this.Left +  e.X,y = this.Top + e.Y;
				//���������϶���Χ
				x = x<=0?0:x;
				x = x>=this.Parent.Width ?this.Parent.Width :x;
				y = y<=0?0:y;
				y = y >=this.Parent.Height ?this.Parent.Height:y;

				Point p =  new Point(x,y) ;
				if(p2 != Point.Empty) {
					_FocusList.DesignCtl.DeControlList.DrawDragFrameByFocus(this,p1,p2);
				}
				p2 = p;
				_FocusList.DesignCtl.DeControlList.DrawDragFrameByFocus(this,p1,p2);
			}
		}

		private void panBack_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(p1!=Point.Empty){
				int x =this.Left +  e.X,y = this.Top + e.Y;
				//���������϶���Χ
				x = x<=0?0:x;
				x = x>=this.Parent.Width ?this.Parent.Width :x;
				y = y<=0?0:y;
				y = y >=this.Parent.Height ?this.Parent.Height:y;
				p2 =  new Point(x, y) ;
				 
				if(p1.X!= p2.X || p1.Y != p2.Y ){
					_FocusList.DesignCtl.DeControlList.DrawDragFrameByFocus(this,p1,p2);
					_FocusList.DesignCtl.DeControlList.MoveByDragFocus(this,p1,p2);
				}
				//pic.Capture = false;
			
				p2 = Point.Empty;
				_FocusList.DesignCtl.DeControlList.ShowFocusHandle(true);

				panBack.Capture = false;
			}
			isdown=false;
 

		}
	}
}
