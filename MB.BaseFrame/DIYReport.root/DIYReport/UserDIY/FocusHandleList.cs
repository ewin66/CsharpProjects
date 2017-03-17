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
using System.Collections ;
using System.Windows.Forms ;

namespace DIYReport.UserDIY
{
	/// <summary>
	/// FocusHandleList ��ժҪ˵����
	/// </summary>
	public class FocusHandleList : ArrayList  
	{
		private static int FUDGER = GDIHelper.FUDGER ;
		private static int HALF_FUDGER = GDIHelper.FUDGER /2;
		private DesignControl _DesignCtl;
		public FocusHandleList(DesignControl pControl)
		{
			_DesignCtl = pControl;

			//�����ӽ���߿�
			//_CtlRect = new Rectangle(rect.X - FUDGER ,rect.Y -FUDGER  ,rect.Width + 2*FUDGER  ,rect.Height + 2*FUDGER  );
			create8Focus();
		}
		#region public ����...
		/// <summary>
		/// �ڶ�ѡ��������õ�ǰѡ��Ŀؼ�
		/// </summary>
		/// <param name="pIsMain"></param>
		public void SetMainSelected( bool pIsMain){
			foreach(FocusHandleCTL focus in this){
				focus.SetBackColor(pIsMain? Color.Blue:Color.White) ;
 
			}
		}

//		/// <summary>
//		/// ͨ�����õ�����ƶ��µ��ȵ�
//		/// </summary>
//		/// <param name="pPoint"></param>
//		/// <returns></returns>
//		public FocusHandle GetFocusByPoint(Point pPoint){
//			int X = pPoint.X ,Y = pPoint.Y ;
//			foreach(FocusHandle focus in this){
//				Rectangle rect = focus.Rect ;
//				bool b = rect.Contains(pPoint);
//				if(b){
//					return focus;
//				}
//			}
//			return null;
//		}
		#endregion public ����...

		#region Public ����...
		public DesignControl DesignCtl {
			get{
				return _DesignCtl;
			}
			set{
				_DesignCtl = value;
			}
		}
		#endregion Public ����...
		
		#region �ڲ�����...
		private  void create8Focus(){
			for(int i =0 ;i < 8 ;i ++){
				base.Add( new FocusHandleCTL(this,(HandleType)i));
			}
			DockFocusToCtl();

		}
		/// <summary>
		/// ��ʾ����
		/// </summary>
		/// <param name="pShow"></param>
		public void ShowFocusHandle(bool pShow){
			//DockFocusToCtl(pShow);
			foreach(FocusHandleCTL ctl in this) {
				ctl.Visible = pShow;
				if(pShow){
					if(!_DesignCtl.Parent.Controls.Contains(ctl)){
						_DesignCtl.Parent.Controls.Add(ctl);  
					}
					ctl.BringToFront();
				}
			}
		}
		/// <summary>
		/// �ѽ���
		/// </summary>
		public void DockFocusToCtl(){
			DockFocusToCtl(false);
		}
		public void DockFocusToCtl(bool pVisible){
			Rectangle rect = _DesignCtl.GetOutFocusRect();
			int RECT_X = rect.X,RECT_Y = rect.Y ,
				RECT_WIDTH = rect.Width ,RECT_HEIGHT = rect.Height;
			foreach(FocusHandleCTL ctl in this){
				HandleType type = ctl.FocusType ;
				ctl.Visible = false;
				switch(type){
					case HandleType.LeftTop://���˿���
						ctl.Location = new Point(RECT_X ,RECT_Y); 
						break;
					case HandleType.MiddleTop://������
						ctl.Location = new Point(RECT_X + RECT_WIDTH / 2 - HALF_FUDGER ,RECT_Y); 
						break;
					case HandleType.RightTop://������
						ctl.Location = new Point(RECT_X + RECT_WIDTH - FUDGER ,RECT_Y); 
						break;
					case HandleType.RightMiddle://���м�
						ctl.Location = new Point(RECT_X + RECT_WIDTH - FUDGER ,RECT_Y + RECT_HEIGHT/2 -HALF_FUDGER ); 
						break;
					case HandleType.RightBottom://�׿���
						ctl.Location = new Point(RECT_X + RECT_WIDTH - FUDGER ,RECT_Y + RECT_HEIGHT - FUDGER); 
						break;
					case HandleType.BottomMiddle ://�׿���
						ctl.Location = new Point(RECT_X + RECT_WIDTH / 2 - HALF_FUDGER ,RECT_Y + RECT_HEIGHT - FUDGER); 
						break;
					case HandleType.LeftBottom://�׿���
						ctl.Location = new Point(RECT_X ,RECT_Y + RECT_HEIGHT - FUDGER); 
						break;
					case HandleType.LeftMiddle ://���м�
						ctl.Location = new Point(RECT_X ,RECT_Y + RECT_HEIGHT/2 -HALF_FUDGER); 
						break;
				}
				ctl.Visible = pVisible;
			}
		}
		#endregion �ڲ�����...
	}
}
