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
using System.Diagnostics; 
using System.Windows.Forms;

 
namespace DIYReport.ReportModel {
	//	#region ����section �����Ͷ��� �Ƚ���...
	//	public class RptSectionSortComparer : IComparer  {
	//		// ���Ƚ���
	//		int IComparer.Compare( Object x, Object y )  {
	//			RptSection   xInfo = x as RptSection;
	//			RptSection yInfo = y as RptSection;
	//				 
	//			return( (new CaseInsensitiveComparer()).Compare((int)xInfo.SectionType ,(int)yInfo.SectionType ) );
	//		}
	//	
	//	}
	//	#endregion �Ƚ���...

	/// <summary>
	///  �¼�ί������
	///  ��ע��������ȷ��֪��������ò�Ҫ��������¼��Ĳ��� 
	/// </summary>
	public delegate void RptSectionEventHandler(object sender,RptSection  e);

	/// <summary>
	/// RptSectionList ��ӡ�����Section ���ϡ�
	/// </summary>
	public class RptSectionList : ArrayList  {
		private RptReport _Report;
		private int _CurrReportWidth = 0;

		#region Public ����...
		public RptReport Report{
			get{
				return _Report;
			}
			set{
				_Report = value;
			}
		}
		public int CurrReportWidth{
			get{
				return _CurrReportWidth;
			}
			set{
				_CurrReportWidth = value;
			}
		}
		#endregion public ����...

		#region �Զ����¼�...
		private  System.EventHandler _AfterCreateNewSection; 
		private RptSectionEventHandler _BeforeRemoveSection;
		public event RptSectionEventHandler BeforeRemoveSection{
			add{
				_BeforeRemoveSection +=value;
			}
			remove{
				_BeforeRemoveSection -=value;
			}
		}
		private void OnBeforeRemoveSection(RptSection arg){
			if(_BeforeRemoveSection!=null){
				_BeforeRemoveSection(this,arg);
			}

		}
		public event System.EventHandler _fterCreateNewSection{
			add{
				_AfterCreateNewSection +=value;
			}
			remove{
				_AfterCreateNewSection -=value;
			}
		}
		private void onAfterCreateNewSection(System.EventArgs  arg){
			if(_AfterCreateNewSection!=null){
				_AfterCreateNewSection(this,arg);
			}
		}
		#endregion �Զ����¼�...

		#region ��չ��Public ����...
		/// <summary>
		/// ����section 
		/// </summary>
		public void AddbySectionType(DIYReport.SectionType sectionType){
			//����Ӧ��section �Ƿ��Ѿ�����
			foreach(RptSection section in this){
				if(section.SectionType == sectionType){
					MessageBox.Show("��Ҫ���ӵ�Section �Ѿ�����,ÿ��Section ֻ�ܴ���һ�Ρ�","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);    
					return;
				}
			}
			if(sectionType == DIYReport.SectionType.GroupHead || sectionType == DIYReport.SectionType.GroupFooter)
				Debug.Assert(false,"���������section ��ʹ��CreateGroupSection");
			RptSection newSection = new RptSection(sectionType);
			this.Add(newSection);

			onAfterCreateNewSection(null);
		}
		/// <summary>
		/// ɾ��ָ����section.
		/// </summary>
		/// <param name="section"></param>
		public void RemoveSection(RptSection section){
			if(this.Count == 1){
				MessageBox.Show("���һ��Section ���ܽ���ɾ����","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);    
				return;
			}
			if(section.SectionType == SectionType.GroupHead || section.SectionType == SectionType.GroupFooter){
				MessageBox.Show("����� Section ���ܽ���ɾ���������ͨ���϶����ĸ߶����ﵽ��ӡ����ʾ��Ч����","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);    
				return;
			}
			if(base.Contains(section)){
				OnBeforeRemoveSection(section);
				base.Remove(section); 
			}

		}
		/// <summary>
		/// ���������Data Section 
		/// </summary>
		/// <param name="pGroupField"> ��Ҫ���з��鴦����ֶ���Ϣ </param>
		/// <returns></returns>
		public bool CreateGroupSection(){
			ArrayList  rptFields = DIYReport.UserDIY.DesignEnviroment.CurrentReport.DesignField as ArrayList;
			//��������
			rptFields.Sort(new DIYReport.GroupAndSort.FieldSortComparer());
			//int width = (this[0] as RptSection).Width ;
			//��ɾ������Ҫ�ķ���
			foreach(DIYReport.GroupAndSort.RptFieldInfo info in rptFields){ 
				if(info.IsGroup)
					continue;
				//�ж��ڷ����Ƿ���ڣ������Ѿ�����Ϊ�����Data Sectionɾ����
				bool b = deleteGroupSection(info);
				if(b){
					//��ɾ����ҳ��
					deleteGroupSection(info);
				}
			}
			foreach(DIYReport.GroupAndSort.RptFieldInfo info in rptFields){ 
				if(!info.IsGroup)
					continue;
				bool hasCreate = false;
				foreach(RptSection section in this){
					if(section.GroupField!=null &&  section.GroupField.Equals(info)){
						hasCreate = true;
						break;
					}
				}
				if(hasCreate){continue;}
				//�ȴ�������ı���
				RptSection newSection = new RptSection(DIYReport.SectionType.GroupHead,info);
				this.Add(newSection);
				newSection = new RptSection(DIYReport.SectionType.GroupFooter,info);
				//newSection.Width = width;
				//	newSection.Height = 60;
				this.Add(newSection);
			}
			onAfterCreateNewSection(null);
			return true;
		}
		private bool deleteGroupSection(DIYReport.GroupAndSort.RptFieldInfo pFieldinfo){
			RptSection groupSection = null;
			foreach(RptSection section in this){
				if(section.GroupField!=null &&  section.GroupField.Equals(pFieldinfo)){
					groupSection = section;
					break;
				}
			}
			if(groupSection!=null){
				OnBeforeRemoveSection(groupSection);
				this.Remove(groupSection);
				return true;
			}
			return false;
		}

		#endregion ��չ��Public ����...

		#region Public ����...
		/// <summary>
		/// ���ݷ�����ֶ����Ƶõ�����Section ��Ϣ
		/// </summary>
		/// <param name="pGroupFieldName"></param>
		/// <param name="pIsGroupHead"></param>
		/// <returns></returns>
		public RptSection GetByGroupField(string pGroupFieldName,bool pIsGroupHead){
			foreach(RptSection section in this){
				DIYReport.SectionType findType = pIsGroupHead?DIYReport.SectionType.GroupHead:DIYReport.SectionType.GroupFooter;
				if(section.SectionType == findType){
					DIYReport.GroupAndSort.RptFieldInfo field = section.GroupField;
					if(field==null){
						throw new Exception("����Section " + section.Name + "�� GroupField Ϊ�ա�");
					}
					if(field.FieldName == pGroupFieldName){
						return section;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// �õ���Detail�����������Section ��ӵĸ߶ȵ��ܺ�
		/// </summary>
		/// <returns></returns>
		public int GetExceptDetailHeight(){
			int height = 0;
			foreach(RptSection section in this){
				if(section.SectionType !=  DIYReport.SectionType.Detail && section.SectionType !=  DIYReport.SectionType.GroupHead
					&& section.SectionType !=  DIYReport.SectionType.GroupFooter){
					height +=section.Height ;
				}
			}
			return height;
		}
		/// <summary>
		/// ��ȡҳ��Margin �ĸ߶ȡ�
		/// </summary>
		/// <returns></returns>
		public int GetMarginHeight(){
			int height = 0;
			foreach(RptSection section in this){
				if(section.SectionType ==  DIYReport.SectionType.TopMargin || section.SectionType ==  DIYReport.SectionType.BottomMargin)
					 {
					height +=section.Height ;
				}
			}
			return height;
		}
		/// <summary>
		/// ͨ��Section Type �õ� ָ����Section�ĸ߶� 
		/// </summary>
		/// <param name="pType"></param>
		/// <returns></returns>
		public int GetSectionHeightByType(DIYReport.SectionType pType){
			RptSection section =  GetSectionByType(pType);
			if(section!=null && section.Visibled){
				return section.Height ;
			}
			else{
				return 0;
			}
		}
		/// <summary>
		/// ͨ��Section Type �õ� ָ����Section
		/// </summary>
		/// <param name="pType"></param>
		public RptSection GetSectionByType(DIYReport.SectionType pType){
			foreach(RptSection section in this){
				if(section.SectionType == pType){
					return section;
				}
			}
			return null;
		}
		/// <summary>
		/// ���µ���Section �Ŀ��
		/// </summary>
		public void ReSizeByPaperSize(int pWidth){
			foreach(RptSection section in this){
				section.Width = pWidth;
			}
		}
		/// <summary>
		/// ADD
		/// </summary>
		/// <param name="pParam"></param>
		/// <returns></returns>
		public RptSection Add(RptSection pParam) {
			insertNewSection(pParam);
			return pParam;
		}
		/// <summary>
		/// ��ָ����λ�ò���Section 
		/// </summary>
		/// <param name="pIndex"></param>
		/// <param name="pParam"></param>
		/// <returns></returns>
		[Obsolete("���ڵķ�������ʹ�� ADD ����")] 
		public RptSection Insert(int pIndex , RptSection pParam) {
			base.Insert(pIndex,pParam);
			return pParam;
		}
		#endregion Public ����...

		#region �ڲ�������...
		//�жϲ������µ�section....
		private void insertNewSection(RptSection section){
			section.Report = _Report;
			int position = -1;
			for(int i  = 0; i < this.Count ; i++){
				int oldIndex = (int)(this[i] as RptSection).SectionType;
				if( oldIndex == (int)section.SectionType){//������˵���Ƿ���������
					if(section.SectionType == SectionType.GroupHead){
						position = i;
					}
					else{
						position = i + 1;
					}
					break;
				}
				if( oldIndex > (int)section.SectionType){
					position = i;
					break;
				}
			}
			if(position == -1 || position == this.Count){
				base.Add(section);
			}
			else{
				base.Insert(position,section);
			}

		}
		//�õ� Detail Section �ڵ�ǰ�����е�Index
		private int getDetailSectionIndex(){
			int count = this.Count ;
			for(int i =0 ; i<count;i++){
				if((this[i] as RptSection).SectionType == DIYReport.SectionType.Detail){
					return i;
				}
			}
			return -1;
		}

		#endregion �ڲ�������...
	}
}
