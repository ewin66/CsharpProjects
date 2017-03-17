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
using System.Collections.Generic;
using System.Windows.Forms;

namespace DIYReport.UserDIY
{
	#region �Ƚ���...
	class DesignObjSortLeft : IComparer  {

		int IComparer.Compare( Object x, Object y )  {
			DesignControl xObj = x as DesignControl;
			DesignControl yObj = y as DesignControl;
			return( (new CaseInsensitiveComparer()).Compare( xObj.Left ,yObj.Left ) );
		}

	}
	class DesignObjSortTop : IComparer  {

		int IComparer.Compare( Object x, Object y )  {
			DesignControl xObj = x as DesignControl;
			DesignControl yObj = y as DesignControl;
			return( (new CaseInsensitiveComparer()).Compare( xObj.Top  ,yObj.Top ) );
		}

	}
	#endregion �Ƚ���...
	/// <summary>
	/// DesignControlList �û�
	/// </summary>
	public class DesignControlList : ArrayList , DIYReport.Interface.IActionParent  
	{
		private DIYReport.UserDIY.DesignSection  _Section;
		private DIYReport.ReportModel.RptSingleObjList _DataObj;  
		 
		
		#region ���캯��...
		public DesignControlList(DIYReport.UserDIY.DesignSection  pSection ){
			_Section = pSection;
			_DataObj = pSection.DataObj.RptObjList;
			foreach(DIYReport.Interface.IRptSingleObj  obj in _DataObj){
				DesignControl ctl = new DesignControl( obj );
				this.Add(ctl);
			}
		 
		}
		#endregion ���캯��...
		private DIYReport.UndoManager.UndoMgr _UndoMgr{
			get{
				return _Section.SectionList.Report.UndoMgr ; 
			}
		}

		#region public ����...
		public DIYReport.UserDIY.DesignSection Section{
			get{
				return _Section;
			}
			set{
				_Section = value;
			}
		}

		#endregion public ����...

		#region Public ����...

		#region Public ����(λ�ô�С�������)...	
		/// <summary>
		/// ������ѡ�еĿؼ�����߿��루������һ��)
		/// </summary>
		public void DockToLeft(){
			this.Sort(new DesignObjSortLeft());
			int right = 0 ;
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					right = right + ctl.Width  >this.Section.Width ?this.Section.Width  - ctl.Width:right ;
					if(right>0){
						ctl.DataObj.Location = new Point(right,ctl.DataObj.Location.Y);
					}
					right = ctl.DataObj.Rect.Right ;
				}
			}
			ShowFocusHandle(true);
		}
		/// <summary>
		/// ������ѡ�еĿؼ��򶥶˿��루������һ��) 
		/// </summary>
		public void DockToTop(){
			this.Sort(new DesignObjSortLeft());
			int bottom = 0 ;
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					bottom = bottom + ctl.Height   >this.Section.Height ?this.Section.Height   - ctl.Height : bottom ;
					if(bottom>0){
						ctl.DataObj.Location = new Point(ctl.DataObj.Location.X,bottom);
					}
					bottom = ctl.DataObj.Rect.Bottom;
				}
			}
			ShowFocusHandle(true);
		}
		#endregion Public ����(λ�ô�С�������)...		

		#region �༭���...
		public DesignControl Add(DesignControl pControl){
			pControl.DeControlList = this;
			//���ӵ�Ҫ����ļ�����
			base.Add(pControl); 
			//��������ӵ�������
			_Section.DeControls.Add(pControl);
			return pControl;
		}
		/// <summary>
		/// �ѵ�ǰѡ��Ŀؼ�ɾ����
		/// </summary>
		public void RemoveSelectedControl() {
			int i = 0;
			ArrayList unList = new ArrayList();
			while(i < this.Count){ 
				if(this[i]==null){
					break;
				}
				 DesignControl ctl = this[i] as DesignControl;
				if(ctl.IsSelected){
					//unList.Add( ctl.DataObj.Clone());
					removeCtl(ctl);
				}
				else{
					i++;
				}
			}
			//_UndoMgr.Store("�Ƴ�����ؼ�",unList,this,DIYReport.UndoManager.ActionType.Remove); 
		}
		private void removeCtl(DesignControl pCtl){
			//�ж��Ƿ�ΪͼƬ�ļ�������ͼƬ
			if(pCtl.DataObj.Type == DIYReport.ReportModel.RptObjType.Image){
				DIYReport.ReportModel.RptObj.RptPictureBox pic = pCtl.DataObj  as DIYReport.ReportModel.RptObj.RptPictureBox;
				if(pic.Image!=null){
					pic.Image.Dispose();
				}
			}
			//ɾ���ؼ����ݼ����е�
			_DataObj.Remove(pCtl.DataObj);
			//��Section������ɾ��8���ȵ�
			foreach(FocusHandleCTL focus in pCtl.FocusList ){
				_Section.DeControls.Remove(focus);
			}
			pCtl.FocusList.Clear();
			//ɾ��Section �ϵĿؼ����ϣ������ϵĿؼ���
			_Section.DeControls.Remove(pCtl);
			//ɾ����ǰ�����еĶԸö��������
			base.Remove (pCtl);
		}
		/// <summary>
		/// ͨ����괴���ؼ�
		/// </summary>
		/// <param name="pFirst"></param>
		/// <param name="pLast"></param>
		public void CreateControl(DIYReport.ReportModel.RptObjType pType , Point pFirst,Point pLast){
			
			CreateControl(null,true,pType,pFirst,pLast);

			
		}
		public void CreateControl(string pDispText,bool pChangeRect,DIYReport.ReportModel.RptObjType pType , Point pFirst,Point pLast){
			Rectangle rect = PublicFun.ChangeMousePointToRect(pFirst,pLast); 

			Rectangle mousRect = pChangeRect?_Section.RectangleToClient(rect):rect; 

			DIYReport.Interface.IRptSingleObj  data = _DataObj.AddByType(pType,pDispText,_Section.DataObj) ;
			if(data==null)
				return;

			DesignControl ctl = new DesignControl(data );
			ctl.BringToFront();

			data.BeginUpdate(); 
			data.Location = new Point(mousRect.Left ,mousRect.Top );
			data.Size = mousRect.Size;
			data.EndUpdate();
			
			ctl.IsSelected = true;
			ctl.IsMainSelected = true;
			this.Add(ctl);
			//�ж��Ƿ�ͨ�����������
			if(pChangeRect){
                //ArrayList unList = new ArrayList();
                //object cUnData = data.Clone();
                //DIYReport.TrackEx.Write(cUnData!=null,"���ڳ����������Ҫ���ñ��������Ҫ�ṩClone() �ķ�����");
                //if(cUnData!=null){
                //    unList.Add(cUnData); 
                //    _UndoMgr.Store("�½�����ؼ�",unList,this,DIYReport.UndoManager.ActionType.Add); 
                //}
			}
			DesignEnviroment.CurrentRptObj =  data;
		}

		/// <summary>
		/// ��ָ����λ�ñ����ӡ�������Ҫ�Ŀؼ�
		/// </summary>
		/// <param name="pType"></param>
		/// <param name="pObjList"></param>
		/// <param name="pLocation"></param>
		/// <param name="CtlWidth"></param>
		/// <param name="pColCount"></param>
		public void CreateRptObjByList(DIYReport.ReportModel.RptObjType pType ,IList pObjList,Point pLocation,
										Size  pCtlSize,int pColCount){
			int hasCreate = 0;
			int rowCount = 0;
			foreach(string sizeName in pObjList){
				Point firstP;
				Point lastP;
				if(hasCreate > pColCount){
					hasCreate = 0;
					rowCount ++;
				}
				//���㶯̬�����ؼ���������
				firstP = new Point(pLocation.X + hasCreate * pCtlSize.Width,pLocation.Y + rowCount * pCtlSize.Height  );
				int lastY = pLocation.Y + (rowCount + 1) * pCtlSize.Height;
				//���Section �ĸ߶Ȳ�������Ҫ���·���
				if(lastY > _Section.DataObj.Height){
					//��ʱ�Ȳ������Զ�����Section �ĸ߶�
					//_Section.DataObj.Height = lastY;
				}
				int lastX = pLocation.X + (hasCreate + 1) * pCtlSize.Width;
				lastX = lastX > 0?lastX+1:lastX;
				lastY = lastY > 0?lastY+1:lastY;
				lastP = new Point(lastX,lastY );
				CreateControl(sizeName,false,DIYReport.ReportModel.RptObjType.Text,firstP,lastP);
				hasCreate ++;
			}
			ShowFocusHandle(true);
		}

		#endregion �༭���...

		/// <summary>
		/// ͨ�����ѡ��ؼ� 
		/// </summary>
		/// <param name="pRect"></param>
		public void SelectCtlByMouseRect(Point pFirst,Point pLast){
			//�����ѡ�������ת���ɴ����ϵ����µ�ѡ��ʽ
			Rectangle rect = PublicFun.ChangeMousePointToRect(pFirst,pLast); 
			SelectCtlByMouseRect(rect);
		}
		 
		/// <summary>
		/// ͨ�����ѡ��ؼ� ��
		/// </summary>
		/// <param name="pRect"></param>
		public void SelectCtlByMouseRect(Rectangle pRect){
			Rectangle mousRect = _Section.RectangleToClient(pRect); 
			DesignControl lastSelectCtl = null ;
			foreach(DesignControl ctl in this){
				Rectangle rect = Rectangle.Intersect( mousRect, new Rectangle(ctl.Location,ctl.Size)) ;
				bool b = rect!=Rectangle.Empty ; 
				if(b){
					ctl.IsMainSelected = false;
					ctl.IsSelected = b;
					lastSelectCtl = ctl;
				}
				else{
					if(DesignEnviroment.PressCtrlKey!=true && DesignEnviroment.PressShiftKey!=true){
						ctl.IsSelected = false;	
					}
				}
			}
			if(lastSelectCtl!=null){
				lastSelectCtl.IsMainSelected = true;

				DesignEnviroment.CurrentRptObj =  lastSelectCtl.DataObj; 
			}
		}

		#region ����������...
//		/// <summary>
//		/// ͨ�����õ�ѡ��ؼ��µĿؼ�����
//		/// </summary>
//		/// <param name="pPoint"></param>
//		/// <returns></returns>
//		public FocusHandle GetFocusByMouse(Point pPoint){
//			foreach(DesignControl ctl in this.Values){
//				bool b = ctl.MouseIsOverFocus(pPoint);
//				if(b){
//					FocusHandleList focus = ctl.FocusList ;
//					FocusHandle hand = focus.GetFocusByPoint(pPoint); 
//					if(hand!=null){
//						return hand;
//					}
//				}
//			}
//			return null;
//		}
		/// <summary>
		/// ���ݵ�õ��õ��µĿؼ�
		/// </summary>
		/// <param name="pPoint"></param>
		/// <returns></returns>
		public DesignControl GetControlByPoint(Point pPoint){
			foreach(DesignControl ctl in this){
				bool b = ctl.MouseIsOver(pPoint);
				if(b){
					return ctl;
				}
			}
			return null;
		}
		/// <summary>
		/// ���û�����ؼ�����ʱ��ʾ�϶��ı߿�
		/// </summary>
		/// <param name="pFocus"></param>
		/// <param name="pPoint"></param>
		public void DrawDragFrameByFocus(FocusHandleCTL pFocus,Point pFirst,Point pLast){
			int width = pLast.X - pFirst.X ,height = pLast.Y - pFirst.Y ;
			int SEP =1;
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					Rectangle dt = new Rectangle(new Point(ctl.Left + SEP,ctl.Top + SEP) ,ctl.Size);
					dt = ReSetCtlDragFrame(pFocus.FocusType,dt,width,height);
//					//���϶���ʱ����ʾ�ؼ�
//					if(dt.Left <=0) dt.Location = new Point(0,dt.Top);
//					if(dt.Top <=0) dt.Location = new Point(dt.Left ,0);
//					if(dt.Right >=_Section.Width) dt.Width = _Section.Width - dt.Left ;
//					if(dt.Bottom  >=_Section.Height)dt.Height = _Section.Height -dt.Top ;

					Rectangle rect = _Section.RectangleToScreen(dt);
					GDIHelper.DrawReversibleRect(rect,FrameStyle.Thick);  
				}
			}
		}
		/// <summary>
		/// ���û���������϶�ʱ��ı�ؼ��Ĵ�С
		/// </summary>
		/// <param name="pFocus"></param>
		/// <param name="pPoint"></param>
		public void MoveByDragFocus(FocusHandleCTL pFocus,Point pFirst,Point pLast){
			int width = pLast.X - pFirst.X ,height = pLast.Y - pFirst.Y ;
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					Rectangle dt = new Rectangle(new Point(ctl.Left ,ctl.Top) ,ctl.Size);
					dt = ReSetCtlDragFrame(pFocus.FocusType,dt,width,height);
//					if(dt.Left <=0) dt.Location = new Point(0,dt.Top);
//					if(dt.Top <=0) dt.Location = new Point(dt.Left ,0);
//					if(dt.Right >=_Section.Width) dt.Width = _Section.Width - dt.Left ;
//					if(dt.Bottom  >=_Section.Height)dt.Height = _Section.Height -dt.Top ;

					DIYReport.Interface.IRptSingleObj  dataObj =  ctl.DataObj;
					dataObj.BeginUpdate(); 
					dataObj.Location = dt.Location ;
					dataObj.Size = dt.Size ;
					dataObj.EndUpdate();
					 
				}
			}
		}

		#endregion ����������...

		/// <summary>
		/// ���϶��ı߿�
		/// </summary>
		public void DrawReversibleFrame(Point pFirst,Point pLast){
			int width = pLast.X - pFirst.X ;
			int height = pLast.Y - pFirst.Y ;
			int SEP = 1;
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					Rectangle dt = ctl.DisplayRectangle;
					int left = ctl.Left +  width + SEP;
					int top = ctl.Top + height + SEP;
					//���������϶���Χ
					left = left < 0 ?0:left;
					top = top <0 ? 0:top;
					left = left +ctl.Width > _Section.Width? _Section.Width - ctl.Width : left;
					top = top + ctl.Height  > _Section.Height?_Section.Height - ctl.Height : top;

					dt.Location = new Point(left,top);
					Rectangle rect = _Section.RectangleToScreen(dt);
					GDIHelper.DrawReversibleRect(rect,FrameStyle.Thick);  
				}
			}
		}

		/// <summary>
		///  ��ѡ��Ŀؼ��϶���ָ����λ��
		/// </summary>
		/// <param name="pFirst"></param>
		/// <param name="pLast"></param>
		public void MoveToPoint(Point pFirst,Point pLast){
			int width = pLast.X - pFirst.X ;
			int height = pLast.Y - pFirst.Y ;
           // List<DesignControl> unList = new List<DesignControl>();
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					int left = ctl.Left +  width ;
					int top = ctl.Top + height ;
                    
					//unList.Add(ctl.DataObj.WiseClone()); 

					//���������϶���Χ
					left = left < 0? 0:left;
					top = top < 0? 0:top;
					left = left +ctl.Width > _Section.Width? _Section.Width - ctl.Width : left;
					top = top + ctl.Height  > _Section.Height?_Section.Height - ctl.Height : top;

					ctl.DataObj.Location = new Point(left,top);
                    //ctl.Visible = false;
                    //unList.Add(ctl);
				}
			}
            //foreach (DesignControl ctl in unList)
            //    ctl.Visible = true;
			//_UndoMgr.Store("�ƶ�����ؼ�",unList,this,DIYReport.UndoManager.ActionType.PropertyChange);    

		}
		#region ͨ�����������λ�úʹ�С...
		/// <summary>
		/// ����������µ�ʱ��
		/// </summary>
		/// <param name="pKeyValue"></param>
		public void ProcessArrowKeyDown(int pKeyValue,bool pIsSize){
			if(pIsSize){
				ProcessResizeByKey(pKeyValue);
			}
			else{
				ProcessReLocationByKey(pKeyValue);
			}
		}

		private void ProcessReLocationByKey(int pKeyValue){
			if(pKeyValue ==37 || pKeyValue==38 || pKeyValue==39||pKeyValue==40){
				int mX = 0,mY = 0;
				if(pKeyValue ==37 ){mX = -1;}
				if(pKeyValue ==39 ){mX = +1;}
				if(pKeyValue == 38){mY = -1;}
				if(pKeyValue == 40){mY = 1;}
				foreach(DesignControl ctl in this){
					if(ctl.IsSelected){
						int left = ctl.DataObj.Location.X + mX;
						int top = ctl.DataObj.Location.Y + mY;
						//�����ƶ�����Χ 
						left = left < 0 ?0:left;
						top = top <0 ? 0:top;
						left = left +ctl.Width > _Section.Width? _Section.Width - ctl.Width : left;
						top = top + ctl.Height  > _Section.Height?_Section.Height - ctl.Height : top;
						 
						ctl.DataObj.Location = new Point(left,top);		
						ctl.FocusList.ShowFocusHandle(true);
					}
				}
			}
		}
		private void ProcessResizeByKey(int pKeyValue){
			if(pKeyValue ==37 || pKeyValue==38 || pKeyValue==39||pKeyValue==40){
				int mX = 0,mY = 0,width = 0,height = 0;
				if(pKeyValue ==37 ){
					mX = -1;
					width = 1;}
				if(pKeyValue ==39 ){
					mX = 0;
					width = 1;}
				if(pKeyValue == 38){
					mY = -1;
					height = 1;}
				if(pKeyValue == 40){
					mY = 0;
					height = 1;}

				foreach(DesignControl ctl in this){
					if(ctl.IsSelected){
						int left = ctl.DataObj.Location.X + mX;
						int top = ctl.DataObj.Location.Y + mY;
						//�����ƶ�����Χ
						left = left < 0 ?0:left;
						top = top <0 ? 0:top;
						left = left +ctl.Width > _Section.Width? _Section.Width - ctl.Width : left;
						top = top + ctl.Height  > _Section.Height?_Section.Height - ctl.Height : top;
						
						ctl.DataObj.BeginUpdate(); 
						ctl.DataObj.Location = new Point(left,top);		
						Size s = ctl.DataObj.Size;
						ctl.DataObj.Size = new Size(s.Width + width,s.Height + height); 
						ctl.DataObj.EndUpdate();
						ctl.FocusList.ShowFocusHandle(true);
					}
				}
			}
		}
		#endregion ͨ�����������λ�úʹ�С...
		/// <summary>
		/// ��ʾѡ��ؼ����ȵ�
		/// </summary>
		/// <param name="pShow"></param>
		public void ShowFocusHandle(bool pShow){
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					ctl.FocusList.ShowFocusHandle(pShow);
				}
			}
		}

		/// <summary>
		/// ���õ�ǰ����µĿؼ� 
		/// </summary>
		/// <param name="pCtl"></param>
		public void SetMainSelected(DesignControl pCtl){
			foreach(DesignControl ctl in this){
				ctl.IsMainSelected = false;
			}
			pCtl.IsMainSelected = true;
		}
		/// <summary>
		/// ��ȡ��ǰѡ�����ƿؼ���
		/// </summary>
		/// <returns></returns>
		public IList GetSelectedCtlsData(){
			ArrayList dataLst = new ArrayList();
			foreach(DesignControl ctl in this){
				if(!ctl.IsSelected)
					continue;
				dataLst.Add(ctl.DataObj);
			}
			return dataLst;
		}
		/// <summary>
		/// �õ���ǰ����µ�ѡ��Ŀؼ�
		/// </summary>
		/// <returns></returns>
		public DesignControl GetMainSelectedCtl(){
			foreach(DesignControl ctl in this){
				if(ctl.IsMainSelected){
					return ctl;
				}
			}
			return null;
		}
		/// <summary>
		/// �������еĿؼ�Ϊ��ѡ��״̬
		/// </summary>
		public void SetAllNotSelected(){
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					ctl.FocusList.ShowFocusHandle(false);
					ctl.IsSelected = false;
				}
			}
		}

		/// <summary>
		/// ��ʽ��ѡ��Ŀؼ�
		/// </summary>
		/// <param name="pCurrentCtl"></param>
		/// <param name="pType"></param>
		public void FormatCtl(FormatCtlType pType){
			DesignControl ctl = GetMainSelectedCtl();
			if(ctl!=null){
				FormatCtl(new Rectangle(ctl.Location,ctl.Size) ,pType);
			}
		}
		public void FormatCtl(Rectangle pRect,FormatCtlType pType){
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					DIYReport.Interface.IRptSingleObj  dataObj = ctl.DataObj;
					switch(pType){
						case FormatCtlType.Left  :
							dataObj.Location = new Point(pRect.Left,ctl.Top)  ;
							break;
						case FormatCtlType.Top :
							dataObj.Location = new Point(ctl.Left, pRect.Top );
							//ctl.Top = pRect.Top ;
							break;
						case FormatCtlType.Right  :
							dataObj.Location = new Point(pRect.Right - ctl.Width ,ctl.Top);
							//ctl.Left  = pRect.Right - ctl.Width  ;
							break;
						case FormatCtlType.Bottom  :
							dataObj.Location = new Point(ctl.Left,pRect.Top + pRect.Height  - ctl.Height);
							//ctl.Top = pRect.Top + pRect.Height  - ctl.Height  ;
							break;
						case FormatCtlType.Width  :
							dataObj.Size = new Size( pRect.Width,ctl.Height);
							//ctl.Width  = pRect.Width  ;
							break;
						case FormatCtlType.Height  :
							dataObj.Size = new Size(ctl.Width , pRect.Height);
							//ctl.Height  = pRect.Height  ;
							break;
						default:
							break;
					}
				}
			}
			ShowFocusHandle(true); 
		}


		/// <summary>
		/// 
		/// </summary>
		public void Invalidate(){
			foreach(DesignControl ctl in this){
				if(ctl.IsSelected){
					ctl.Invalidate(); 
				}
			}
		}
		public Rectangle ReSetCtlDragFrame(HandleType pType , Rectangle pRect,int pWidth,int pHeight){
			int left = pRect.Left ,top = pRect.Top  ;
			Rectangle newRect;
			switch(pType){
				case HandleType.LeftTop :
					newRect = new Rectangle(left + pWidth,top + pHeight ,pRect.Width - pWidth, pRect.Height - pHeight); 
					break;
				case HandleType.MiddleTop :
					newRect = new Rectangle(left,top + pHeight ,pRect.Width , pRect.Height - pHeight); 
					break;
				case HandleType.RightTop :
					newRect = new Rectangle(left, top + pHeight ,pRect.Width  + pWidth , pRect.Height - pHeight); 
					break;
				case HandleType.RightMiddle :
					newRect = new Rectangle(left ,top,pRect.Width  + pWidth   , pRect.Height ); 
					break;
				case HandleType.RightBottom :
					newRect = new Rectangle(left ,top ,pRect.Width  + pWidth  , pRect.Height + pHeight ); 
					break;
				case HandleType.BottomMiddle :
					newRect = new Rectangle(left ,top , pRect.Width  , pRect.Height + pHeight); 
					break;
				case HandleType.LeftBottom :
					newRect = new Rectangle(left +  pWidth ,top , pRect.Width -  pWidth,  pRect.Height + pHeight ); 
					break;
				case HandleType.LeftMiddle  :
					newRect = new Rectangle(left +  pWidth, top, pRect.Width -  pWidth , pRect.Height); 
					break;
				default:
					newRect = pRect;
					break;
			}
			return newRect;
		}
		#endregion Public ����...

		#region �ڲ�������...

		#endregion �ڲ�������...

		#region ʵ�� IActionParent �ӿڵķ��� ...
		//ʵ�ָýӿ���Ϊ��ʵ���û������� Undo ��Redo �Ĳ���
		public void SetPropertyValue(ref IList pObjList){
			//�ڸı�������Եĳ��������� Ŀǰֻ����������λ�úʹ�С
			ArrayList proList = new ArrayList();
			foreach(DIYReport.Interface.IRptSingleObj  obj in _DataObj){
				for(int i =0;i < pObjList.Count ; i++){
					DIYReport.Interface.IRptSingleObj utem = pObjList[i] as DIYReport.Interface.IRptSingleObj;
					if(utem!=null && obj.Name == utem.Name){
						proList.Add(obj.WiseClone());
						obj.BeginUpdate();
						obj.Location = utem.Location ;
						obj.Size = utem.Size;
						obj.EndUpdate();
					}
				}
			}
			pObjList = proList;
		}
		// �ڳ��������У��������ɾ���Ĳ�������Ҫ�ڳ���ʱ�������ӻ���
		public void Add(IList pObjList){
			DesignControl last = null;
			int selectHeight = this.Section.DataObj.Height;
 
			for(int i =0;i < pObjList.Count;i++){
				DIYReport.Interface.IRptSingleObj data =  pObjList[i] as DIYReport.Interface.IRptSingleObj ;
				data = data.Clone() as DIYReport.Interface.IRptSingleObj;

				if(data.Location.Y + data.Size.Height > selectHeight)
					selectHeight = data.Location.Y + data.Size.Height;
				
				DesignControl ctl = new DesignControl( data as DIYReport.Interface.IRptSingleObj );

				data.EndUpdate();

				ctl.IsSelected = true;
				//ctl.IsMainSelected = true;
				this._DataObj.Add(data);
				this.Add(ctl);
				last = ctl;
			}
			if(last!=null){
				SetMainSelected(last);
				ShowFocusHandle(true); 
			}
			if(selectHeight > this.Section.DataObj.Height )
				this.Section.DataObj.Height = selectHeight;
		}
		//�ڳ��������У�����������ӵĲ�������Ҫ�ڳ���ʱɾ����
		public void Remove(IList pObjList){
			int i = 0;
			while(i < this.Count){ 
				if(this[i]==null){
					break;
				}
				bool b = false;
				DesignControl ctl = this[i] as DesignControl;
				for(int j =0;j < pObjList.Count;j++){
					DIYReport.Interface.IRptSingleObj data = pObjList[j] as DIYReport.Interface.IRptSingleObj;
					if(data!=null && ctl.DataObj.Name == data.Name){
						removeCtl(ctl);
						b = true;
					}
				}
				 if(!b){
					i++;
				}
			}
		}
		#endregion ʵ�� IActionParent �ӿڵķ��� ...
	}
}
