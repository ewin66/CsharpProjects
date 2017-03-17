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
using System.Collections ;
using System.Drawing ;

using DIYReport;
namespace DIYReport.UserDIY
{
	/// <summary>
	/// DesignSectionList ���屨���Section
	/// </summary>
	public class DesignSectionList : ArrayList 
	{
		#region ��������...
		private DIYReport.Interface.IDesignPanel   _Report;
		private DIYReport.ReportModel.RptSectionList  _DataObj; 
		#endregion ��������...

		#region �Զ����¼�...
		private DesignSectionEventHandler _BeforeRemoveSection;
		private DesignSectionEventHandler _AfterInsertSection;
		private DesignSectionEventHandler _AfterRefreshLayout;
		public event DesignSectionEventHandler BeforeRemoveSection{
			add{
				_BeforeRemoveSection +=value;
			}
			remove{
				_BeforeRemoveSection -=value;
			}
		}
		public event DesignSectionEventHandler AfterInsertSection{
			add{
				_AfterInsertSection +=value;
			}
			remove{
				_AfterInsertSection-=value;
			}
		}
		public event DesignSectionEventHandler AfterRefreshLayout{
			add{
				_AfterRefreshLayout +=value;
			}
			remove{
				_AfterRefreshLayout-=value;
			}
		}
		private void OnBeforeRemoveSection(DesignSectionEventArgs arg){
			if(_BeforeRemoveSection!=null){
				_BeforeRemoveSection(this,arg);
			}

		}
		private void OnAfterInsertSection(DesignSectionEventArgs arg){
			if(_AfterInsertSection!=null){
				_AfterInsertSection(this,arg);
			}

		}
		private void OnAfterRefreshLayout(DesignSectionEventArgs arg){
			if(_AfterRefreshLayout!=null){
				_AfterRefreshLayout(this,arg);
			}

		}
		#endregion �Զ����¼�...

		#region ���캯��...
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pReport"></param>
		public DesignSectionList(DIYReport.Interface.IDesignPanel  pReport){
			_Report = pReport;
		}
		#endregion ���캯��...
	
		#region Public ����...
		public DIYReport.Interface.IDesignPanel   Report{
			get{
				return _Report;
			}
			set{
				_Report = value;
			}
		}
		public DIYReport.ReportModel.RptSectionList DataObj{
			get{
				return _DataObj; 
			}
		}
		#endregion Public ����...

		#region public ����...
		/// <summary>
		/// ����һ���µ�Section List
		/// </summary>
		public void CreateNewSectionList(){
			_DataObj = _Report.DataObj.SectionList ; 

			foreach(DIYReport.ReportModel.RptSection dataSection in  _DataObj){
				DesignSection section = new DesignSection(dataSection); 
				section.IsDisplay = true;
				SectionCaption caption = new SectionCaption();
				section.CaptionCtl = caption;
				this.Add(section);
			}
			RefreshDesignLayout();
			_DataObj.BeforeRemoveSection +=new DIYReport.ReportModel.RptSectionEventHandler(_DataObj_BeforeRemoveSection);
			_DataObj._fterCreateNewSection+=new System.EventHandler(_DataObj_AfterCreateNewSection);
		}

		#region ����ˢ�±��������Section ����ʾ...
//		/// <summary>
//		/// section �Ĵ�С��λ�õ���
//		/// </summary>
//		/// <param name="pSection"></param>
//		public void ReizeSectionsLocation(DesignSection pSection){
//			int height = 0;
//			int captionHeight = SectionCaption.CAPTION_HEIGHT;
//			foreach(DesignSection section in this ){
//				if(pSection==null || (int)section.SectionType > (int)pSection.SectionType){
//					section.Location = new Point(0,height + captionHeight);
//					section.CaptionCtl.Location = new Point(0,height );
//				}
//				height += section.Height + captionHeight ;
//			}
//		}
		/// <summary>
		/// �õ�����Design Section ������߶�
		/// </summary>
		/// <returns></returns>
		public int GetDesignHeight(){
			int height = 0;
			foreach(DesignSection section in this){
				if(section.IsDisplay){
					height +=section.Height +  SectionCaption.CAPTION_HEIGHT  ;
				}//
			}
			return height;
		}
		// ���²���Design Section ����ʾ
		public void RefreshDesignLayout(){
			int height = 0;
			int drawY = 0;
			int paperWidth = _Report.DataObj.IsLandscape?_Report.DataObj.PaperSize.Height:_Report.DataObj.PaperSize.Width ;
			int width = paperWidth - _Report.DataObj.Margins.Left - _Report.DataObj.Margins.Right ;
			
			foreach(DIYReport.ReportModel.RptSection rptSection in this.DataObj){
				DesignSection section = getDesignSectionByDataSection(rptSection);
				if(section==null)
					continue;
				//DIYReport.TrackEx.Write(section!=null,"�ڵ���DesignLayout��ʱ����ָ���rptsection ��ȡ designsection Ϊ�յ������"); 
				bool b = section.IsDisplay;
				section.Visible = b;
				section.CaptionCtl.Visible = b;
				section.DataObj.DrawLocationY = drawY;
				if(b){
					section.BringToFront();
					section.CaptionCtl.BringToFront();
					section.CaptionCtl.Location = new Point(0,height);
					section.Location = new Point(0,height +  SectionCaption.CAPTION_HEIGHT );
					section.Width = width;
					section.CaptionCtl.Width =  width;
					height +=section.Height +  SectionCaption.CAPTION_HEIGHT;
					drawY +=section.Height;
				}//
				
			}
			OnAfterRefreshLayout((DesignSectionEventArgs)null); 
		}
		private DesignSection getDesignSectionByDataSection(DIYReport.ReportModel.RptSection rptSection){
			foreach(DesignSection section in this){
				if(section.DataObj.Equals(rptSection))
					return section;
			}
			return null;
		}
		#endregion ����ˢ�±��������Section ����ʾ...
		/// <summary>
		/// �õ���ǰ���ڻ״̬��Section 
		/// </summary>
		/// <returns></returns>
		public DesignSection GetActiveSection(){
			foreach(DesignSection section in this ){
				if(section.IsActive){
					return section;
				}
			}
			return null;
		}
		/// <summary>
		/// ����Section �Ƿ���ѡ���״̬
		/// </summary>
		/// <param name="pSection"></param>
		public void SetActiveSection(DesignSection pSection){
			foreach(DesignSection section in this ){
				if((int)section.SectionType == (int)pSection.SectionType){
					if(pSection.SectionType == DIYReport.SectionType.GroupFooter || pSection.SectionType == DIYReport.SectionType.GroupHead){
						if(pSection.DataObj.GroupField == section.DataObj.GroupField){
							section.IsActive = true;
							continue;
						}
					}
					else{
						section.IsActive = true;
						continue;
					}
				}

				if(section.IsActive){
					section.DesignControls.SetAllNotSelected(); 
				}
				section.IsActive = false;

			}
		}
		/// <summary>
		/// ������Section �����пؼ��ڲ�ѡ��״̬
		/// </summary>
		public void SetOtherSectionAllNotSelected(DesignSection pSection){
			foreach(DesignSection section in this ){
				if((int)section.SectionType != (int)pSection.SectionType){
					section.DesignControls.SetAllNotSelected(); 
				}
			}
		}
		/// <summary>
		/// ͬʱ��������section ����ƿؼ�
		/// </summary>
		/// <param name="pShow"></param>
		public void ShowFocusHandle(bool pShow){
			foreach(DesignSection section in this ){
				section.DesignControls.ShowFocusHandle(pShow); 
			}
		}
		/// <summary>
		/// ͨ��Data Section List ���� Design Section List ��������Ϣ
		/// </summary>
		/// <param name="pDataSectionList"></param>
		public void SetDataSectionList(DIYReport.ReportModel.RptSectionList pDataSectionList ){
			foreach(DIYReport.ReportModel.RptSection  section in pDataSectionList){
				DesignSection dse = GetSectionByType(section.SectionType);
				if(dse!=null){
					dse.SetDataSection(section);
				}
			}
			//ReizeSectionsLocation( (DesignSection)null );
			RefreshDesignLayout(); 
		}
		/// <summary>
		/// ���ݱ�����Ƶ�Section �õ�
		/// </summary>
		/// <returns></returns>
		public DIYReport.ReportModel.RptSectionList GetDataSectionList(){
//			DIYReport.ReportModel.RptSectionList sList = new DIYReport.ReportModel.RptSectionList();
//			foreach(DesignSection section in this){
//				DIYReport.ReportModel.RptSection rpt = new DIYReport.ReportModel.RptSection();
//				rpt.Height = section.Height  ;
//				rpt.Width = section.Width ;
//				rpt.SectionType = section.SectionType ;
//				rpt.Visibled = section.IsDisplay ;
//				sList.Add(rpt);
//			}
			return _DataObj ;
		}
		
		public DesignSection GetSectionByType(DIYReport.SectionType pType){
			foreach(DesignSection section in this){
				if(section.SectionType == pType){
					return section;
				}
			}
			return null;
		}
 
		/// <summary>
		/// Add 
		/// </summary>
		/// <param name="pSection"></param>
		/// <returns></returns>
		public DesignSection Add(DesignSection pSection){
			base.Add(pSection);
			pSection.SectionList = this;
			return pSection;
		}
		#endregion public ����...

		#region Data SectionList �¼�...

		private void _DataObj_BeforeRemoveSection(object sender, DIYReport.ReportModel.RptSection e) {
			DesignSection deSection = null;
			foreach(DesignSection section in this){
				if(section.DataObj.Equals(e)){
					deSection = section;
					break;
				}
			}
			if(deSection!=null){
				OnBeforeRemoveSection( new DesignSectionEventArgs(-1,deSection));
				this.Remove(deSection); 
			}
			//��ɾ��Section �����²���
			RefreshDesignLayout(); 
		}
		#endregion Data SectionList �¼�...

		#region �ڲ�������...
		// ����ˢ�±������������Section ����ʾ
		private void createDesignSection(){
			//��� DataSection ��DesignSectionList ���Ƿ��Ѿ���������������ڣ��ʹ�����
			int i = 0;
			foreach(DIYReport.ReportModel.RptSection dataSection in  _DataObj){
				if(!dataSection.HasCreateViewDesign){
					DesignSection section = new DesignSection(dataSection); 
					SectionCaption caption = new SectionCaption();
					section.IsDisplay = true;
					section.CaptionCtl = caption;
					section.SectionList = this;
					this.Add(section);
					OnAfterInsertSection(new DesignSectionEventArgs(i,section)); 
				}
				i++;
			}
			RefreshDesignLayout();
		}
//		
//		//����Section �ڼ����е�index �õ�location
//		private Point getSectionPosition(int pIndex,out int pCaptionY){
//			int height = 0;
//			for(int i = 0;i < pIndex;i++){
//				 DesignSection section = this[i] as DesignSection ;
//				if(section.IsDisplay){
//					height +=section.Height +  SectionCaption.CAPTION_HEIGHT ;
//				}//
//			}
//			pCaptionY =  height;
//			return new Point(0, height + SectionCaption.CAPTION_HEIGHT);
//		}
	#endregion �ڲ�������...

		private void _DataObj_AfterCreateNewSection(object sender, System.EventArgs  e) {
			createDesignSection();
		}
	}
}
