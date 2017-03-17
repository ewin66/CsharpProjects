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
using System.Diagnostics;

namespace DIYReport.UserDIY {
 
	/// <summary>
	/// ReportDesign ������Ʋ������档
	/// </summary>
	[ToolboxItem(false)]
	public class ReportDesign : System.Windows.Forms.UserControl,DIYReport.Interface.IDesignPanel { 
		#region �ڲ��Զ����ɴ���...

		private System.Windows.Forms.Panel panMain;
		private DIYReport.UserDIY.DesignRuler rulerLeft;
		private DIYReport.UserDIY.DesignRuler rulerTop;
		private System.Windows.Forms.Panel panDesign;
		private System.Windows.Forms.VScrollBar vscrBar;
		private System.Windows.Forms.HScrollBar hscrBar;
		private System.Windows.Forms.Panel panRightBottom;
		private System.Windows.Forms.Panel panLeftTop;
		private System.Windows.Forms.ToolBarButton toolBarButton6;
		private System.Windows.Forms.ToolBarButton tlbButProperty;
		private System.Windows.Forms.ToolBarButton tlbButFormatLeft;
		private System.Windows.Forms.ToolBarButton tlbButFormatRight;
		private System.Windows.Forms.ToolBarButton tlbButFormatTop;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBarButton tlbButFormatBottom;
		private System.Windows.Forms.ToolBarButton tlbButFormatWidth;
		private System.Windows.Forms.ToolBarButton tlbButFormatHeight;
		private System.Windows.Forms.ToolBarButton tlbButSelect;
		private System.Windows.Forms.ToolBarButton tlbButCreateLable;
		private System.Windows.Forms.ToolBarButton tlbButCreateText;
		private System.Windows.Forms.ToolBar tBarTools;
		private System.Windows.Forms.ToolBarButton tlbButCreatePic;
		private System.Windows.Forms.ToolBarButton tblButPageSet;
		private System.Windows.Forms.ToolBarButton tlbButPreview;
		private System.Windows.Forms.ToolBarButton tlbButPrint;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBar tlbTopBar;
		private System.Windows.Forms.ToolBarButton tlbButAddNew;
		private System.Windows.Forms.ToolBarButton tlbButOpen;
		private System.Windows.Forms.ToolBarButton tlbButSave;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolBarButton tlbButLine;
		private System.Windows.Forms.ToolBarButton tlbButRect;
		private System.Windows.Forms.ToolBarButton tlbButDeleteCtl;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton tlbButArrow;
		private System.Windows.Forms.ToolBarButton tlbButDockLeft;
		private System.Windows.Forms.ToolBarButton tlbButDockTop;
		private System.Windows.Forms.ToolBarButton toolBarButton8;
		private System.Windows.Forms.ToolBarButton tlbButRedo;
		private System.Windows.Forms.ToolBarButton tlbButUndo;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolBarButton tlbGroupAndSort;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.ComponentModel.IContainer components;
		#region ������������ʹ�õ���Դ...
		/// <summary> 
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion ������������ʹ�õ���Դ...

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReportDesign));
			this.tlbTopBar = new System.Windows.Forms.ToolBar();
			this.tlbButAddNew = new System.Windows.Forms.ToolBarButton();
			this.tlbButOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.tlbButSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.tblButPageSet = new System.Windows.Forms.ToolBarButton();
			this.tlbButPreview = new System.Windows.Forms.ToolBarButton();
			this.tlbButPrint = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.tlbButDeleteCtl = new System.Windows.Forms.ToolBarButton();
			this.tlbButUndo = new System.Windows.Forms.ToolBarButton();
			this.tlbButRedo = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.tlbGroupAndSort = new System.Windows.Forms.ToolBarButton();
			this.tlbButProperty = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatLeft = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatRight = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatTop = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatBottom = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatWidth = new System.Windows.Forms.ToolBarButton();
			this.tlbButFormatHeight = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.tlbButDockLeft = new System.Windows.Forms.ToolBarButton();
			this.tlbButDockTop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
			this.tlbButArrow = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tBarTools = new System.Windows.Forms.ToolBar();
			this.tlbButSelect = new System.Windows.Forms.ToolBarButton();
			this.tlbButCreateLable = new System.Windows.Forms.ToolBarButton();
			this.tlbButCreateText = new System.Windows.Forms.ToolBarButton();
			this.tlbButCreatePic = new System.Windows.Forms.ToolBarButton();
			this.tlbButLine = new System.Windows.Forms.ToolBarButton();
			this.tlbButRect = new System.Windows.Forms.ToolBarButton();
			this.panMain = new System.Windows.Forms.Panel();
			this.panRightBottom = new System.Windows.Forms.Panel();
			this.panDesign = new System.Windows.Forms.Panel();
			this.vscrBar = new System.Windows.Forms.VScrollBar();
			this.hscrBar = new System.Windows.Forms.HScrollBar();
			this.rulerLeft = new DIYReport.UserDIY.DesignRuler();
			this.rulerTop = new DIYReport.UserDIY.DesignRuler();
			this.panLeftTop = new System.Windows.Forms.Panel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlbTopBar
			// 
			this.tlbTopBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						 this.tlbButAddNew,
																						 this.tlbButOpen,
																						 this.toolBarButton3,
																						 this.tlbButSave,
																						 this.toolBarButton5,
																						 this.tblButPageSet,
																						 this.tlbButPreview,
																						 this.tlbButPrint,
																						 this.toolBarButton4,
																						 this.tlbButDeleteCtl,
																						 this.tlbButUndo,
																						 this.tlbButRedo,
																						 this.toolBarButton2,
																						 this.tlbGroupAndSort,
																						 this.tlbButProperty,
																						 this.toolBarButton6,
																						 this.tlbButFormatLeft,
																						 this.tlbButFormatRight,
																						 this.tlbButFormatTop,
																						 this.tlbButFormatBottom,
																						 this.tlbButFormatWidth,
																						 this.tlbButFormatHeight,
																						 this.toolBarButton1,
																						 this.tlbButDockLeft,
																						 this.tlbButDockTop,
																						 this.toolBarButton8,
																						 this.tlbButArrow});
			this.tlbTopBar.DropDownArrows = true;
			this.tlbTopBar.ImageList = this.imageList1;
			this.tlbTopBar.Location = new System.Drawing.Point(0, 0);
			this.tlbTopBar.Name = "tlbTopBar";
			this.tlbTopBar.ShowToolTips = true;
			this.tlbTopBar.Size = new System.Drawing.Size(552, 28);
			this.tlbTopBar.TabIndex = 0;
			this.tlbTopBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tlbTopBar_ButtonClick);
			// 
			// tlbButAddNew
			// 
			this.tlbButAddNew.ImageIndex = 15;
			this.tlbButAddNew.Tag = "23";
			this.tlbButAddNew.ToolTipText = "������������";
			// 
			// tlbButOpen
			// 
			this.tlbButOpen.ImageIndex = 16;
			this.tlbButOpen.Tag = "22";
			this.tlbButOpen.ToolTipText = "���뱨��";
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbButSave
			// 
			this.tlbButSave.ImageIndex = 17;
			this.tlbButSave.Tag = "21";
			this.tlbButSave.ToolTipText = "������";
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tblButPageSet
			// 
			this.tblButPageSet.ImageIndex = 12;
			this.tblButPageSet.Tag = "20";
			this.tblButPageSet.ToolTipText = "ҳ������";
			// 
			// tlbButPreview
			// 
			this.tlbButPreview.ImageIndex = 10;
			this.tlbButPreview.Tag = "19";
			this.tlbButPreview.ToolTipText = "��ӡԤ��";
			// 
			// tlbButPrint
			// 
			this.tlbButPrint.ImageIndex = 11;
			this.tlbButPrint.Tag = "18";
			this.tlbButPrint.ToolTipText = "��ӡ";
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbButDeleteCtl
			// 
			this.tlbButDeleteCtl.ImageIndex = 19;
			this.tlbButDeleteCtl.Tag = "17";
			this.tlbButDeleteCtl.ToolTipText = "ɾ����ǰѡ��Ŀؼ�";
			// 
			// tlbButUndo
			// 
			this.tlbButUndo.Enabled = false;
			this.tlbButUndo.ImageIndex = 23;
			this.tlbButUndo.Tag = "24";
			// 
			// tlbButRedo
			// 
			this.tlbButRedo.Enabled = false;
			this.tlbButRedo.ImageIndex = 24;
			this.tlbButRedo.Tag = "25";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbGroupAndSort
			// 
			this.tlbGroupAndSort.ImageIndex = 25;
			this.tlbGroupAndSort.Tag = "26";
			// 
			// tlbButProperty
			// 
			this.tlbButProperty.ImageIndex = 0;
			this.tlbButProperty.Tag = "16";
			this.tlbButProperty.ToolTipText = "����";
			// 
			// toolBarButton6
			// 
			this.toolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbButFormatLeft
			// 
			this.tlbButFormatLeft.ImageIndex = 1;
			this.tlbButFormatLeft.Tag = "0";
			this.tlbButFormatLeft.ToolTipText = "�����";
			// 
			// tlbButFormatRight
			// 
			this.tlbButFormatRight.ImageIndex = 2;
			this.tlbButFormatRight.Tag = "2";
			this.tlbButFormatRight.ToolTipText = "�Ҷ���";
			// 
			// tlbButFormatTop
			// 
			this.tlbButFormatTop.ImageIndex = 3;
			this.tlbButFormatTop.Tag = "1";
			this.tlbButFormatTop.ToolTipText = "���˶���";
			// 
			// tlbButFormatBottom
			// 
			this.tlbButFormatBottom.ImageIndex = 4;
			this.tlbButFormatBottom.Tag = "3";
			this.tlbButFormatBottom.ToolTipText = "�׶˶���";
			// 
			// tlbButFormatWidth
			// 
			this.tlbButFormatWidth.ImageIndex = 6;
			this.tlbButFormatWidth.Tag = "4";
			this.tlbButFormatWidth.ToolTipText = "��ͬ���";
			// 
			// tlbButFormatHeight
			// 
			this.tlbButFormatHeight.ImageIndex = 5;
			this.tlbButFormatHeight.Tag = "5";
			this.tlbButFormatHeight.ToolTipText = "��ͬ�߶�";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbButDockLeft
			// 
			this.tlbButDockLeft.ImageIndex = 21;
			this.tlbButDockLeft.Tag = "6";
			this.tlbButDockLeft.ToolTipText = "ˮƽ����";
			// 
			// tlbButDockTop
			// 
			this.tlbButDockTop.ImageIndex = 22;
			this.tlbButDockTop.Tag = "7";
			this.tlbButDockTop.ToolTipText = "��ֱ����";
			// 
			// toolBarButton8
			// 
			this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tlbButArrow
			// 
			this.tlbButArrow.ImageIndex = 20;
			this.tlbButArrow.Tag = "8";
			this.tlbButArrow.ToolTipText = "�ƶ��ؼ�";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tBarTools
			// 
			this.tBarTools.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						 this.tlbButSelect,
																						 this.tlbButCreateLable,
																						 this.tlbButCreateText,
																						 this.tlbButCreatePic,
																						 this.tlbButLine,
																						 this.tlbButRect});
			this.tBarTools.Dock = System.Windows.Forms.DockStyle.Left;
			this.tBarTools.DropDownArrows = true;
			this.tBarTools.ImageList = this.imageList1;
			this.tBarTools.Location = new System.Drawing.Point(0, 28);
			this.tBarTools.Name = "tBarTools";
			this.tBarTools.ShowToolTips = true;
			this.tBarTools.Size = new System.Drawing.Size(24, 332);
			this.tBarTools.TabIndex = 1;
			this.tBarTools.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.tBarTools.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tBarTools_ButtonClick);
			// 
			// tlbButSelect
			// 
			this.tlbButSelect.ImageIndex = 7;
			this.tlbButSelect.Pushed = true;
			this.tlbButSelect.Tag = "0";
			this.tlbButSelect.ToolTipText = "ѡ��";
			// 
			// tlbButCreateLable
			// 
			this.tlbButCreateLable.ImageIndex = 8;
			this.tlbButCreateLable.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tlbButCreateLable.Tag = "1";
			this.tlbButCreateLable.ToolTipText = "�ı�";
			// 
			// tlbButCreateText
			// 
			this.tlbButCreateText.ImageIndex = 9;
			this.tlbButCreateText.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tlbButCreateText.Tag = "2";
			this.tlbButCreateText.ToolTipText = "���ݿ�";
			// 
			// tlbButCreatePic
			// 
			this.tlbButCreatePic.ImageIndex = 13;
			this.tlbButCreatePic.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tlbButCreatePic.Tag = "5";
			this.tlbButCreatePic.ToolTipText = "ͼ��";
			// 
			// tlbButLine
			// 
			this.tlbButLine.ImageIndex = 14;
			this.tlbButLine.Tag = "3";
			this.tlbButLine.ToolTipText = "����";
			// 
			// tlbButRect
			// 
			this.tlbButRect.ImageIndex = 18;
			this.tlbButRect.Tag = "4";
			this.tlbButRect.ToolTipText = "���߿�";
			// 
			// panMain
			// 
			this.panMain.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.panMain.Controls.Add(this.panRightBottom);
			this.panMain.Controls.Add(this.panDesign);
			this.panMain.Controls.Add(this.vscrBar);
			this.panMain.Controls.Add(this.hscrBar);
			this.panMain.Controls.Add(this.rulerLeft);
			this.panMain.Controls.Add(this.rulerTop);
			this.panMain.Controls.Add(this.panLeftTop);
			this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panMain.Location = new System.Drawing.Point(24, 28);
			this.panMain.Name = "panMain";
			this.panMain.Size = new System.Drawing.Size(528, 332);
			this.panMain.TabIndex = 2;
			// 
			// panRightBottom
			// 
			this.panRightBottom.BackColor = System.Drawing.SystemColors.Control;
			this.panRightBottom.Location = new System.Drawing.Point(496, 296);
			this.panRightBottom.Name = "panRightBottom";
			this.panRightBottom.Size = new System.Drawing.Size(16, 16);
			this.panRightBottom.TabIndex = 5;
			// 
			// panDesign
			// 
			this.panDesign.BackColor = System.Drawing.Color.White;
			this.panDesign.Location = new System.Drawing.Point(24, 24);
			this.panDesign.Name = "panDesign";
			this.panDesign.Size = new System.Drawing.Size(448, 264);
			this.panDesign.TabIndex = 4;
			this.panDesign.Resize += new System.EventHandler(this.panDesign_Resize);
			// 
			// vscrBar
			// 
			this.vscrBar.Location = new System.Drawing.Point(496, 24);
			this.vscrBar.Name = "vscrBar";
			this.vscrBar.Size = new System.Drawing.Size(17, 277);
			this.vscrBar.TabIndex = 3;
			this.vscrBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vscrBar_Scroll);
			// 
			// hscrBar
			// 
			this.hscrBar.Location = new System.Drawing.Point(24, 296);
			this.hscrBar.Name = "hscrBar";
			this.hscrBar.Size = new System.Drawing.Size(472, 17);
			this.hscrBar.TabIndex = 2;
			this.hscrBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrBar_Scroll);
			// 
			// rulerLeft
			// 
			this.rulerLeft.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.rulerLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rulerLeft.BackgroundImage")));
			this.rulerLeft.BeginDrawPoint = 16;
			this.rulerLeft.IsHorizontal = false;
			this.rulerLeft.Location = new System.Drawing.Point(0, 24);
			this.rulerLeft.Name = "rulerLeft";
			this.rulerLeft.Size = new System.Drawing.Size(24, 294);
			this.rulerLeft.TabIndex = 1;
			// 
			// rulerTop
			// 
			this.rulerTop.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.rulerTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rulerTop.BackgroundImage")));
			this.rulerTop.BeginDrawPoint = 0;
			this.rulerTop.IsHorizontal = true;
			this.rulerTop.Location = new System.Drawing.Point(24, 0);
			this.rulerTop.Name = "rulerTop";
			this.rulerTop.Size = new System.Drawing.Size(496, 24);
			this.rulerTop.TabIndex = 0;
			// 
			// panLeftTop
			// 
			this.panLeftTop.BackColor = System.Drawing.SystemColors.Control;
			this.panLeftTop.Location = new System.Drawing.Point(0, 0);
			this.panLeftTop.Name = "panLeftTop";
			this.panLeftTop.Size = new System.Drawing.Size(24, 24);
			this.panLeftTop.TabIndex = 0;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// ReportDesign
			// 
			this.Controls.Add(this.panMain);
			this.Controls.Add(this.tBarTools);
			this.Controls.Add(this.tlbTopBar);
			this.Name = "ReportDesign";
			this.Size = new System.Drawing.Size(552, 360);
			this.Resize += new System.EventHandler(this.ReportDesign_Resize);
			this.Load += new System.EventHandler(this.ReportDesign_Load);
			this.panMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion �ڲ��Զ����ɴ���...

		#region ��������...
		private object _CurrentObj;
		private DesignSectionList _SectionList;
		private DIYReport.ReportModel.RptReport _DataObj;

		//ҵ������Ҫ����
		//private DataSet _DsFields = null;
		private DIYReport.Interface.IReportDataIO  _ReportIO;

		private DIYReport.UndoManager.UndoMgr _UndoMgr; 
		#endregion ��������...

		#region ���캯��...
		public ReportDesign() {
			InitializeComponent();
			//panMain.Visible = false;
			_UndoMgr = new DIYReport.UndoManager.UndoMgr();
			
		}
		#endregion ���캯��...
		
		#region public ����...
		/// <summary>
		/// ����һ���µı���
		/// </summary>
		/// <returns></returns>
		public DIYReport.ReportModel.RptReport CreateNewReport(){
			_DataObj = _ReportIO.NewReport(); 
 
			iniNewReportDesign();

			DesignEnviroment.DesignHasChanged = false;
			return _DataObj;
		}
		/// <summary>
		/// ���Ѿ����ڵı���
		/// </summary>
		/// <param name="pReport"></param>
		/// <returns></returns>
		public DIYReport.ReportModel.RptReport OpenReport(DIYReport.ReportModel.RptReport pReport){
			if(pReport!=null ){				 
				_DataObj = pReport;
				//�ж��Ƿ��з����Section����ʼ��������Ƶ��ֶΣ�Ȼ��ǰ��Ƶ��ֶ����õ������Section ��
				iniGroupFieldOnOpen(_DataObj);

				iniNewReportDesign();
			}
			DesignEnviroment.DesignHasChanged = false; 
			return _DataObj;

		}
		/// <summary>
		/// ɾ����ǰѡ�еı���ؼ�
		/// </summary>
		public void DeleteSelectedControls(){
			DesignSection section  = _SectionList.GetActiveSection();
			if(section!=null){
				section.DesignControls.RemoveSelectedControl();
			}
 
		}
		#region ���������ơ�ճ�������������...
		/// <summary>
		/// ��������
		/// </summary>
		public void Cut(){
			Copy();
			DeleteSelectedControls();
		}
		/// <summary>
		/// ���Ʋ���
		/// </summary>
		public void Copy(){
			DesignSection section  = _SectionList.GetActiveSection();
			if(section==null)
				return;

			IList dataLst = section.DesignControls.GetSelectedCtlsData();
			DIYReport.UserDIY.ClipboardEx.CopyToClipBoard(dataLst);
		}
		public void Past(){
			DesignSection section  = _SectionList.GetActiveSection();
			if(section==null)
				return;
			IList dataLst = DIYReport.UserDIY.ClipboardEx.GetFromClipBoard(); 
			section.DesignControls.Add(dataLst); 
		}
		#endregion ���������ơ�ճ�������������...
		#endregion public ����...

		#region ��չ��Public ����...
		/// <summary>
		/// �����ͻָ��Ĺ��������
		/// </summary>
		public DIYReport.UndoManager.UndoMgr UndoMgr{
			get{
				return _UndoMgr;
			}
			set{
				_UndoMgr = value;
			}
		}
		/// <summary>
		///  ���������IO����
		/// </summary>
		public  DIYReport.Interface.IReportDataIO  ReportIO{
			get{
				if(_ReportIO==null){
					_ReportIO = new DIYReport.ReportDataIO(); 
				}
				return _ReportIO;
			}
			set{
				_ReportIO = value;
			}
		}
		/// <summary>
		/// �ñ����������������е�Design Section
		/// </summary>
		public DesignSectionList SectionList{
			get{
				return _SectionList;
			}
			set{
				_SectionList = value;
				}
		}
		/// <summary>
		/// ���������
		/// </summary>
		public DIYReport.ReportModel.RptReport DataObj{
			get{
				return _DataObj;
			}
			set{
				_DataObj = value;
			}
		}
		/// <summary>
		/// ��ǰѡ�����ʲô����
		/// </summary>
		public object CurrentObj{
			get{
				return _CurrentObj;
			}
			set{
				_CurrentObj = value;
			}
		}
		public Panel DesignBack{
			get{
				return panDesign ;
			}
		}
		#endregion ��չ��Public ����...

		#region ����Section...
		/// <summary>
		/// �����е�Design Section ����ʾ���û����
		/// </summary>
		public void CreateDesignSection(){
			
			//DesignSection lastSection = null;
			foreach(DesignSection section in _SectionList){
				if(section.IsDisplay){
					//section.Visible = true;
					//section.CaptionCtl.Location = new Point(0,height);
					//section.Location = new Point(0,height +  SectionCaption.CAPTION_HEIGHT );
					panDesign.Controls.Add(section); 
					//section.BringToFront();
					panDesign.Controls.Add(section.CaptionCtl); 
					//height +=section.Height +  SectionCaption.CAPTION_HEIGHT  ;
					//section.IsDispBottomSection = false;
					//
				}//
			}

		}
		#endregion ����Section...

		#region �����¼�����...
		private void hscrBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e) {
			rulerTop.Left = panLeftTop.Width  -e.NewValue ; 
			panDesign.Left = rulerLeft.Width - e.NewValue ; 
			rulerTop.Width =  panMain.Width - panLeftTop.Width - rulerTop.Left + 20 ;
			
			 
		}
		private void vscrBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e) {
			rulerLeft.Top = panLeftTop.Height  -e.NewValue ; 
			panDesign.Top = rulerTop.Height - e.NewValue ; 
			rulerLeft.Height  = panMain.Height - panLeftTop.Height   - rulerLeft.Top + 20 ;
		}


		private void ReportDesign_Resize(object sender, System.EventArgs e) {
			iniResizeDesignForm();
		}

		private void ReportDesign_Load(object sender, System.EventArgs e) {
			this.ParentForm.KeyPreview = true;
			this.ParentForm.Closed +=new EventHandler(ParentForm_Closed); 
			this.ParentForm.KeyDown +=new KeyEventHandler(ParentForm_KeyDown);
			this.ParentForm.KeyUp +=new KeyEventHandler(ParentForm_KeyUp);

			panLeftTop.Location = new Point(0,0);
			panLeftTop.Size = new Size(rulerLeft.Width ,rulerTop.Height);

			rulerTop.Location = new Point(panLeftTop.Width,0);
			rulerLeft.Location = new Point(0,rulerTop.Height);
			panDesign.Location = new Point( rulerLeft.Width , rulerTop.Height );

			panRightBottom.Location = new Point(panMain.Width - panRightBottom.Width ,panMain.Height - panRightBottom.Height);

			hscrBar.Location = new Point(rulerLeft.Width ,panMain.Height    -  hscrBar.Height);
			hscrBar.Size = new Size(panMain.Width - rulerLeft.Width - panRightBottom.Width  , hscrBar.Height); 

			vscrBar.Location = new Point(panMain.Width - vscrBar.Width, rulerTop.Height);
			vscrBar.Size = new Size(vscrBar.Width ,panMain.Height -  rulerTop.Height  - panRightBottom.Height);
			rulerLeft.Size = new Size(rulerLeft.Width ,panMain.Height - rulerLeft.Top  ); 


			rulerLeft.BringToFront();
			rulerTop.BringToFront();
			hscrBar.BringToFront();
			vscrBar.BringToFront();
			panLeftTop.BringToFront(); 
		}

		private void panDesign_Resize(object sender, System.EventArgs
			e) {
			rulerLeft.SectionList = _SectionList.GetDataSectionList();  

			reCalculateScrollValue();
		}

		private void tBarTools_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e) {
			foreach(ToolBarButton but in tBarTools.Buttons){
				but.Pushed = false;
			}
			e.Button.Pushed = true;
			DesignEnviroment.DrawControlType = (DIYReport.ReportModel.RptObjType)int.Parse(e.Button.Tag.ToString());
			DesignEnviroment.IsCreateControl = DesignEnviroment.DrawControlType!= DIYReport.ReportModel.RptObjType.None;
		}

		private void tlbTopBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e) {
			//e.Button.Tag ��0-5Ϊ�洢��ʽ�ؼ�������ʽ�� 
			this.Cursor = Cursors.WaitCursor ;
			if(e.Button.Tag!=null){
				int index = int.Parse(e.Button.Tag.ToString());  
				if(index<6){
					DesignSection section = _SectionList.GetActiveSection();  
					section.DesignControls.FormatCtl((DIYReport.UserDIY.FormatCtlType)index);  
				}
				switch(index){
					case 6://����߿���
						_SectionList.GetActiveSection().DesignControls.DockToLeft();
						break;
					case 7://���ұ߿���
						_SectionList.GetActiveSection().DesignControls.DockToTop() ;
						break;
					case 8://��ʾ���������
						FrmArrowOperate.ShowArrowForm( _SectionList,this.ParentForm );  
						break;
					case 16:
						//��ʾ���Դ���
						DesignEnviroment.ShowPropertyForm(this.ParentForm,true); 
						break;
					case 17:
						//ɾ���ؼ�
						DeleteSelectedControls();
						break;
					case 18://��ӡ
						using(DIYReport.Print.SwPrintView print = new DIYReport.Print.SwPrintView( DIYReport.UserDIY.DesignEnviroment.DataSource,_DataObj)){
							print.Printer(); 
						}
						break;
					case 19://��ӡԤ��
						using(DIYReport.Print.SwPrintView printView = new DIYReport.Print.SwPrintView( DIYReport.UserDIY.DesignEnviroment.DataSource,_DataObj)){
							printView.ShowPreview();
						}
						break;
					case 20://��ӡҳ������
						DIYReport.Print.RptPageSetting.ShowPageSetupDialog(_DataObj);   
						break;
					case 21://���汨��
						_ReportIO.SaveReport(_DataObj,null); 
						DIYReport.UserDIY.DesignEnviroment.DesignHasChanged = false;
						break;
					case 22://�򿪱��� 
						DIYReport.ReportModel.RptReport report =  _ReportIO.Open(); 
						if(report!=null){
							OpenReport( report );
						}
						break;
					case 23://�������� 
						//CreateNewReport();
						//MessageBox.Show("��ǰ��֧�ִ������½�һ�ű�������.","������ʾ"); 
						//��ʱ�����޸�Ϊ��ӡ����ĵ������ܡ�
						_ReportIO.Save(_DataObj);
						break;
					case 24 ://Undo
						_UndoMgr.Undo();
						break;
					case 25 ://Redo 
						_UndoMgr.Redo();
						break;
					case 26 : //��ʾ���������
						IList fieldsList = DIYReport.UserDIY.DesignEnviroment.CurrentReport.DesignField; 
						
						DIYReport.GroupAndSort.frmSortAndGroup frm = new DIYReport.GroupAndSort.frmSortAndGroup(fieldsList); 
						frm.AfterSortAndGroup +=new DIYReport.GroupAndSort.SortAndGroupEventHandler(frm_AfterSortAndGroup);
						frm.ShowDialog(); 
						break;
					default:
						break;
				}
			}
			this.Cursor = Cursors.Arrow;
		}
		#endregion �����¼�����...

		#region �ڲ���������...
		private void iniResizeDesignForm(){
			//��Ƶĳߴ�

			rulerTop.Size = new Size(panMain.Width - rulerTop.Left ,rulerTop.Height );
			rulerLeft.Size = new Size(rulerLeft.Width ,panMain.Height - rulerLeft.Top  ); 
			//���½��µ�С��Ƭλ��
			panRightBottom.Location = new Point(panMain.Width - panRightBottom.Width ,panMain.Height - panRightBottom.Height);
			//������
			 
			hscrBar.Location = new Point(rulerLeft.Width ,panMain.Height    -  hscrBar.Height);
			hscrBar.Size = new Size(panMain.Width - rulerLeft.Width - panRightBottom.Width  , hscrBar.Height); 

			vscrBar.Location = new Point(panMain.Width - vscrBar.Width, rulerTop.Height);
			vscrBar.Size = new Size(vscrBar.Width ,panMain.Height -  rulerTop.Height  - panRightBottom.Height);

			//section ��Ƶ�����
			//int width = vscrBar.Visible?vscrBar.Width : 0;
			//int height = hscrBar.Visible ? hscrBar.Height : 0 ;
			//panDesign.Size = new Size(panMain.Width - rulerLeft.Width - width ,panMain.Height - rulerTop.Height - height);
			
			reCalculateScrollValue();
		}
		private void reCalculateScrollValue(){
			int maxWidth = panDesign.Width + rulerLeft.Width + panRightBottom.Width - panMain.Width + 50;
			hscrBar.Maximum = maxWidth > 0? maxWidth : 0;
			int maxHeight  = panDesign.Height + rulerTop.Height + panRightBottom.Height - panMain.Height  + 50 ;
			vscrBar.Maximum = maxHeight > 0?maxHeight:0;
		}
		//���²��úʹ���һ���µı������
		private void iniNewReportDesign(){
			if(_DataObj!=null){
				if(_SectionList!=null){
					foreach(DesignSection section in _SectionList){
						panDesign.Controls.Remove(section.CaptionCtl); 
						panDesign.Controls.Remove(section); 
					}
				}
				_DataObj.AfterValueChanged +=new DIYReport.ReportModel.RptEventHandler(_DataObj_AfterValueChanged);
				_SectionList = new DesignSectionList(this);
				_SectionList.BeforeRemoveSection +=new DesignSectionEventHandler(_SectionList_BeforeRemoveSection);
				_SectionList.AfterInsertSection +=new DesignSectionEventHandler(_SectionList_AfterInsertSection);
				_SectionList.AfterRefreshLayout +=new DesignSectionEventHandler(_SectionList_AfterRefreshLayout);
				_SectionList.CreateNewSectionList();
 
				CreateDesignSection();
				DIYReport.UserDIY.DesignEnviroment.CurrentReport  = _DataObj;
			}
		}
		//DIYReport.ReportModel.RptReport pReport
		//��ʼ��������Ƶ��ֶΣ�Ȼ��ǰ��Ƶ��ֶ����õ������Section ��
		private void iniGroupFieldOnOpen(DIYReport.ReportModel.RptReport pReport){
			//�ж��Ƿ��з����Section
			DIYReport.ReportModel.RptSectionList sectionList = pReport.SectionList;
			foreach(DIYReport.ReportModel.RptSection section in sectionList){
				if(section.SectionType == SectionType.GroupHead || section.SectionType == SectionType.GroupFooter){
					DIYReport.GroupAndSort.RptFieldInfo groupField =  section.GroupField;
					if(groupField == null){Debug.Assert(false,"�ڻ�ȡ����ķ���Section ʱ���õ��ֶε���ϢΪ�ա�"); }
					IList fieldList = DIYReport.UserDIY.DesignEnviroment.CurrentReport.DesignField;
					if(fieldList == null){Debug.Assert(false,"û�г�ʼ����Ƶ��ֶΡ�"); }
					int count = fieldList.Count ;
					for(int i =0; i <count;i++){
						DIYReport.GroupAndSort.RptFieldInfo designField = fieldList[i] as DIYReport.GroupAndSort.RptFieldInfo;
						if(designField.FieldName.Trim().ToUpper() == groupField.FieldName.Trim().ToUpper()){
							designField.IsGroup = true;
							designField.IsAscending = groupField.IsAscending ;
							designField.OrderIndex = groupField.OrderIndex;
							designField.SetSort = groupField.SetSort;
							designField.DivideName = groupField.DivideName;
							section.GroupField = designField;
							break;
						}
					}
				}
			}
		}
		#endregion �ڲ���������...

		#region DataObj_AfterValue...

		private void _DataObj_AfterValueChanged(object sender, DIYReport.ReportModel.RptEventArgs e) {
			//_DataObj.SectionList.
			int paperWidth = _DataObj.IsLandscape?_DataObj.PaperSize.Height:_DataObj.PaperSize.Width ;
			int width = paperWidth - _DataObj.Margins.Left - _DataObj.Margins.Right ;
			panDesign.Size = new Size(width ,panDesign.Height); 
			_DataObj.SectionList.ReSizeByPaperSize(width); 
			reCalculateScrollValue();
		}

		#endregion DataObj_AfterValue...

		#region ����Parent Form ���¼���Ϣ...

		private void ParentForm_Closed(object sender, EventArgs e) {
			//������������ʹ�õ���Դ...
			this.ParentForm.Closed -=new EventHandler(ParentForm_Closed); 
			this.ParentForm.KeyDown -=new KeyEventHandler(ParentForm_KeyDown);
			this.ParentForm.KeyUp -=new KeyEventHandler(ParentForm_KeyUp);
			disposeRptImage();
		}
		private void disposeRptImage(){
			foreach(DIYReport.UserDIY.DesignSection section in this.SectionList){
				foreach(DIYReport.UserDIY.DesignControl ctl in section.DesignControls){
					DIYReport.Interface.IRptSingleObj   obj = ctl.DataObj ;
					if(obj.Type == DIYReport.ReportModel.RptObjType.Image){
						System.Drawing.Image img = (obj as DIYReport.ReportModel.RptObj.RptPictureBox).Image ;
						if(img!=null){
							try{
								img.Dispose();
							}
							catch{
							}
						}
					}
				}
			}
		}
		private void ParentForm_KeyDown(object sender, KeyEventArgs e) {
			DIYReport.UserDIY.DesignEnviroment.PressShiftKey = e.KeyValue == 16;
			DIYReport.UserDIY.DesignEnviroment.PressCtrlKey  = e.KeyValue == 17;
			//�ж��û��Ƿ��� Delete ��
			if(e.KeyValue ==46){
				DeleteSelectedControls();
			}
			//�������
            bool isSiae = DIYReport.UserDIY.DesignEnviroment.PressCtrlKey;
            _SectionList.GetActiveSection().DesignControls.ProcessArrowKeyDown(e.KeyValue, isSiae);  
		}

		private void ParentForm_KeyUp(object sender, KeyEventArgs e) {
			DIYReport.UserDIY.DesignEnviroment.PressShiftKey = false;
			DIYReport.UserDIY.DesignEnviroment.PressCtrlKey  = false;
		}

		#endregion ����Parent Form ���¼���Ϣ...
		
		private void timer1_Tick(object sender, System.EventArgs e) {
			tlbButRedo.Enabled = _UndoMgr.CanRedo ;
			tlbButUndo.Enabled = _UndoMgr.CanUndo ;
		}
		//�ڷ������ȷ��֮�������Ű汨�����ƽ���
		private void frm_AfterSortAndGroup(object sender, EventArgs e) {
			_SectionList.DataObj.CreateGroupSection();
			//_SectionList.Refresh();
		}

		private void _SectionList_BeforeRemoveSection(object sender, DesignSectionEventArgs e) {
			panDesign.Controls.Remove(e.Section.CaptionCtl); 
			panDesign.Controls.Remove(e.Section); 
		}

		private void _SectionList_AfterInsertSection(object sender, DesignSectionEventArgs e) {
			//������һ���µ�Design Section �����²����û��ı�
			panDesign.Controls.Add(e.Section.CaptionCtl); 
			panDesign.Controls.Add(e.Section); 

		}

		private void _SectionList_AfterRefreshLayout(object sender, DesignSectionEventArgs e) {
			//���µ��� panDesign ����ʾ�߶�
			int height = _SectionList.GetDesignHeight();
			int paperWidth = _DataObj.IsLandscape?_DataObj.PaperSize.Height:_DataObj.PaperSize.Width ;
			int width = paperWidth- _DataObj.Margins.Left - _DataObj.Margins.Right ;
			panDesign.Size = new Size(width ,height); 
			//panDesign.ResumeLayout(false);
		}
	}
}
