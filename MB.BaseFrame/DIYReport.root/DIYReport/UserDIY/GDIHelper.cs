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
using System.Windows.Forms ;

namespace DIYReport.UserDIY
{
	/// <summary>
	/// GDIHelper ���ȵ�
	/// </summary>
	public class GDIHelper
	{	
		//�����ȵ�Ĵ�С
		public static int FUDGER = 6;

//		public static void DrawFocus(Graphics g , DesignControl pControl){
//			FocusHandleList focusList = pControl.FocusList;
//			foreach(FocusHandle focus in focusList){
//				g.DrawRectangle(new Pen(Brushes.Black),focus.Rect  );
//			}
//		}
		public static void DrawReversibleRect(Point pFirst,Point pLast,FrameStyle pStyle){
			DrawReversibleRect(new Rectangle(pFirst.X ,pFirst.Y ,pLast.X,pLast.Y),pStyle);
		}
		public static void DrawReversibleRect(Rectangle pRect,FrameStyle pStyle){
			ControlPaint.DrawReversibleFrame(pRect,Color.Black,pStyle);
		}

		public static void DrawMouseSelected(Point pFirst,Point pLast){
			ControlPaint.DrawReversibleFrame(new Rectangle(pFirst,new Size(pLast.X-pFirst.X,pLast.Y - pFirst.Y)) ,Color.Black,FrameStyle.Dashed);
		}
	}
}
