//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-15
// Description	:	EditRptObjAttribute End User Design �༭�������ԡ�
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace DIYReport.UserDIY
{
	/// <summary>
	/// EditRptObjAttribute End User Design �༭�������ԡ�
	/// </summary>
	public class EditRptObjAttribute : System.Windows.Forms.UserControl
	{
		#region �ڲ��Զ����ɴ���...
		private System.Windows.Forms.PropertyGrid pgridMain;
		private System.Windows.Forms.ComboBox cobControls;
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
			this.pgridMain = new System.Windows.Forms.PropertyGrid();
			this.cobControls = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// pgridMain
			// 
			this.pgridMain.CommandsVisibleIfAvailable = true;
			this.pgridMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgridMain.LargeButtons = false;
			this.pgridMain.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.pgridMain.Location = new System.Drawing.Point(0, 20);
			this.pgridMain.Name = "pgridMain";
			this.pgridMain.Size = new System.Drawing.Size(240, 348);
			this.pgridMain.TabIndex = 4;
			this.pgridMain.Text = "propertyGrid1";
			this.pgridMain.ViewBackColor = System.Drawing.SystemColors.Window;
			this.pgridMain.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// cobControls
			// 
			this.cobControls.Dock = System.Windows.Forms.DockStyle.Top;
			this.cobControls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cobControls.Location = new System.Drawing.Point(0, 0);
			this.cobControls.Name = "cobControls";
			this.cobControls.Size = new System.Drawing.Size(240, 20);
			this.cobControls.TabIndex = 3;
			this.cobControls.SelectedIndexChanged += new System.EventHandler(this.cobControls_SelectedIndexChanged);
			// 
			// EditRptObjAttribute
			// 
			this.Controls.Add(this.pgridMain);
			this.Controls.Add(this.cobControls);
			this.Name = "EditRptObjAttribute";
			this.Size = new System.Drawing.Size(240, 368);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion �ڲ��Զ����ɴ���...
 
		private DIYReport.ReportModel.RptReport    _Report;

		#region ���캯��...
		public EditRptObjAttribute() {
			InitializeComponent();
			
			addCtlToList();
		}
		#endregion ���캯��...

//		private void timer1_Tick(object sender, System.EventArgs e) {
//			//ˢ�¿ؼ�������
//			object obj = _Report.CurrentObj;
//			if(pgridMain.SelectedObject==null || pgridMain.SelectedObject.Equals(obj)!=false){
//				pgridMain.SelectedObject =  _Report.CurrentObj ;
//			}
//		}

//		[DllImport("User32.dll")]
//		private static extern bool SetWindowPos(IntPtr hwnd,int hWndInsertAfter,int x,int y,int cx,int cy,int wFlagslong);
		#region Public ����...
		/// <summary>
		/// ���ö�������ԡ�
		/// </summary>
		/// <param name="report"></param>
		/// <param name="pObject"></param>
		public void SetPropertryObject(DIYReport.ReportModel.RptReport  report,object pObject){
			_Report = report;
			
			pgridMain.SelectedObject = pObject;
			if(pObject!=null){
				addCtlToList();
				displaySelectedNode(pObject);
			}
		}
		
		#endregion Public ����...

		#region �ڲ�������...

		private void displaySelectedNode(object pObject){
			DataStrtuct da = findNode(pObject);
			cobControls.SelectedItem   = da;
		}
		private DataStrtuct findNode(object pObject){
			foreach(DataStrtuct obj in  cobControls.Items){
				if(pObject.Equals(obj.Data )){
					return obj;
				}
			}
			return null;
		}

		//���ӱ���ڵ� 
		private void addCtlToList(){
			cobControls.Items.Clear();
			if(_Report!=null){
				cobControls.Items.Add(new  DataStrtuct(_Report.Name , _Report));

				addSections( _Report.SectionList);
			}
		}
		//����Section
		private void addSections(DIYReport.ReportModel.RptSectionList pSections ){
			foreach(DIYReport.ReportModel.RptSection section in pSections){
			 
				string txt = section.Name + ":" + Enum.GetName(section.SectionType.GetType() , section.SectionType) ;
				cobControls.Items.Add(new  DataStrtuct(txt , section));
				addRptObjList( section.RptObjList); 
			}
		}

		private void addRptObjList(DIYReport.ReportModel.RptSingleObjList  pControls){
			foreach(DIYReport.ReportModel.RptSingleObj obj in pControls ){
				string txt = obj.Name + ":" + Enum.GetName(obj.Type.GetType() ,obj.Type);
				cobControls.Items.Add(new  DataStrtuct(txt , obj));
			}
		}
		#endregion �ڲ�������...


		private void FrmEditRptObjAttribute_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
//			e.Cancel = true;
//			this.Hide();
		}

		private void cobControls_SelectedIndexChanged(object sender, System.EventArgs e) {
			DataStrtuct da = cobControls.SelectedItem as DataStrtuct ;
			pgridMain.SelectedObject = da.Data ;
		}
	}
	class DataStrtuct{
		private string _Name;
		private object _Data;
		public DataStrtuct(string pName,object pData){
			_Name = pName;
			_Data = pData;
		}
		#region ���ǻ���ķ���...
		public override string ToString() {
			return _Name == null?"":_Name;
		}

		#endregion ���ǻ���ķ���...

		#region Public ����...
		public string Name{
			get{
				return _Name;
			}
			set{
				_Name = value;
			}
		}
		public object Data{
			get{
				return _Data;
			}
			set{
				_Data = value;
			}
		}
		#endregion Public ����...
	}
}
