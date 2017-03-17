//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-11
// Description	:	XPrintingSystem �����ӡ��
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace DIYReport.Extend.Print
{
	/// <summary>
	/// XPrintingSystem �����ӡ��
	/// </summary>
	public class XPrintingSystem : System.IDisposable
	{
		private DIYReport.Interface.IDrawReport  _DrawObj;
		private DevExpress.XtraPrinting.PrintingSystem printingSystem;
		private static readonly string DIY_REPORT_CFG_FILE = System.AppDomain.CurrentDomain.BaseDirectory + "DiyReportCfg.ini";

		/// <summary>
		/// ���캯��...
		/// </summary>
		/// <param name="pDs"></param>
		/// <param name="dataReport"></param>
		public XPrintingSystem()
		{
			printingSystem = new DevExpress.XtraPrinting.PrintingSystem();
			
		}
		#region public ����...
		/// <summary>
		/// ����dataReport ����printingSystem �� pageSetting ��Ϣ��
		/// </summary>
		/// <param name="dataReport"></param>
		public void SetPrintPageInfo(DIYReport.ReportModel.RptReport dataReport){
            var existsPrinter = DIYReport.Common.EnumPrintersHelperEx.CheckExistsPrinter(dataReport.PrintName);
            if (existsPrinter)
                printingSystem.PageSettings.PrinterName = dataReport.PrintName;

			printingSystem.PageSettings.Landscape =  dataReport.IsLandscape;
            
			System.Drawing.Printing.Margins  mrg = dataReport.Margins;//DIYReport.PublicFun.GetRegionMargins(dataReport.Margins);
			printingSystem.PageSettings.LeftMargin = mrg.Left; 
			printingSystem.PageSettings.TopMargin = mrg.Top;
			printingSystem.PageSettings.RightMargin = mrg.Right;
			printingSystem.PageSettings.BottomMargin = mrg.Bottom;

			printingSystem.PageSettings.PaperKind = dataReport.PaperKind;
			printingSystem.PageSettings.PaperName = dataReport.PaperName;
		}

		/// <summary>
		/// ���ô�ӡҳ�档
		/// </summary>
		/// <param name="dataReport"></param>
		public void PageSetup(DIYReport.ReportModel.RptReport dataReport){
			SetPrintPageInfo(dataReport);
			bool re = printingSystem.PageSetup();
			if(re){
				dataReport.IsLandscape = printingSystem.PageSettings.Landscape ;
				dataReport.Margins.Left = printingSystem.PageSettings.LeftMargin; 
				dataReport.Margins.Top = printingSystem.PageSettings.TopMargin;
				dataReport.Margins.Right = printingSystem.PageSettings.RightMargin ;
				dataReport.Margins.Bottom = printingSystem.PageSettings.BottomMargin;

				dataReport.PaperKind = printingSystem.PageSettings.PaperKind ;
				dataReport.PaperName = printingSystem.PageSettings.PaperName;
				dataReport.PaperSize = DIYReport.Print.RptPageSetting.GetPaperSizeByName(dataReport.PrintDocument,dataReport.PrintName,dataReport.PaperName);   
				if(dataReport.ReportDataWidth > 0 && dataReport.ReportDataWidth > dataReport.PaperSize.Width - dataReport.Margins.Left - dataReport.Margins.Right)
					dataReport.ReportDataWidth = dataReport.PaperSize.Width - dataReport.Margins.Left - dataReport.Margins.Right;
				
				//System.Windows.Forms.MessageBox.Show("��ǰPageName" + dataReport.PaperName);
				 
				savePageSetting(dataReport,dataReport.Margins,dataReport.PaperName,dataReport.PaperKind.ToString());
			}
		}
		/// <summary>
		/// ��ӡ..
		/// </summary>
		public void Print(DIYReport.ReportModel.RptReport dataReport){
            string sessionName = getSessionName(dataReport); ;

            SetPrintPageInfo(dataReport);
            string pagerName = IniFile.ReadString(sessionName, "PaperName", printingSystem.PageSettings.PaperName, DIY_REPORT_CFG_FILE);
            if (pagerName == null || pagerName.Length == 0)
                pagerName = printingSystem.PageSettings.PaperName;

            DevExpress.XtraPrinting.Link link = CreateLinkDoc(pagerName, dataReport);

            readPageSetting(dataReport, link);

            try {
                //System.Windows.Forms.MessageBox.Show("��ǰPageName" + pagerName);
                string kindName = IniFile.ReadString(sessionName, "PaperKind", printingSystem.PageSettings.PaperKind.ToString(), DIY_REPORT_CFG_FILE);
                System.Drawing.Printing.PaperKind pKind = (System.Drawing.Printing.PaperKind)Enum.Parse(typeof(System.Drawing.Printing.PaperKind), kindName, true);
                link.PrintingSystem.PageSettings.PaperKind = pKind;
            }
            catch { }

			link.Landscape = printingSystem.PageSettings.Landscape; 
			link.PaperKind = link.PrintingSystem.PageSettings.PaperKind;

            var existsPrinter = DIYReport.Common.EnumPrintersHelperEx.CheckExistsPrinter(dataReport.PrintName);
            if (existsPrinter)
                link.Print(dataReport.PrintName);
            else
			    link.PrintDlg();
//			if(_DrawObj!=null)
//				_DrawObj.Dispose();
		}
		/// <summary>
		/// �ĵ�Ԥ��...
		/// </summary>
		public void PrintPreview(DIYReport.ReportModel.RptReport dataReport){
			string sessionName = getSessionName(dataReport);; 

			SetPrintPageInfo(dataReport);
			string pagerName = IniFile.ReadString(sessionName,"PaperName",printingSystem.PageSettings.PaperName,DIY_REPORT_CFG_FILE);
			if(pagerName==null || pagerName.Length ==0)
				pagerName = printingSystem.PageSettings.PaperName; 

			DevExpress.XtraPrinting.Link link = CreateLinkDoc(pagerName,dataReport);

			readPageSetting(dataReport,link);

			try{
				//System.Windows.Forms.MessageBox.Show("��ǰPageName" + pagerName);
				string kindName = IniFile.ReadString(sessionName,"PaperKind",printingSystem.PageSettings.PaperKind.ToString() ,DIY_REPORT_CFG_FILE);
				System.Drawing.Printing.PaperKind pKind = (System.Drawing.Printing.PaperKind)Enum.Parse(typeof(System.Drawing.Printing.PaperKind),kindName,true);
				link.PrintingSystem.PageSettings.PaperKind = pKind;
			}
			catch{}

			link.Landscape = printingSystem.PageSettings.Landscape; 
			link.PaperKind = link.PrintingSystem.PageSettings.PaperKind;
			//link.PrintingSystem.PageSettings.PaperName = pagerName; 
			link.ShowPreviewDialog();
           // link.ShowRibbonPreview(DevExpress.LookAndFeel.UserLookAndFeel.Default); 

			savePageSetting(dataReport,link.Margins,link.PrintingSystem.PageSettings.PaperName, link.PaperKind.ToString() );
			
			//System.Windows.Forms.MessageBox.Show(printingSystem.PageSettings.LeftMargin.ToString());
//			if(_DrawObj!=null)
//				_DrawObj.Dispose();
			//Ҳ����������ķ�����ʵ�֡�
//			DevExpress.XtraPrinting.Preview.PrintPreviewForm frm = new DevExpress.XtraPrinting.Preview.PrintPreviewForm();
//			frm.PrintingSystem = this.PrintingSystem; 
//
//			link.CreateDocument();
//			frm.ShowDialog(); 
		}
		#endregion public ����...
		
		#region �ڲ���������...
		//��������������õı߿�
		private void savePageSetting(DIYReport.ReportModel.RptReport dataReport,System.Drawing.Printing.Margins  margins,string paperName, string paperKind){
			string sessionName = getSessionName(dataReport); 
			IniFile.WriteString(sessionName,"LeftMargin",margins.Left.ToString(),DIY_REPORT_CFG_FILE);
			IniFile.WriteString(sessionName,"TopMargin",margins.Top.ToString(),DIY_REPORT_CFG_FILE);
			IniFile.WriteString(sessionName,"RightMargin",margins.Right.ToString(),DIY_REPORT_CFG_FILE);
			IniFile.WriteString(sessionName,"BottomMargin",margins.Bottom.ToString(),DIY_REPORT_CFG_FILE);
			if(paperKind!=null && paperKind.Length >0)
				IniFile.WriteString(sessionName,"PaperKind",paperKind,DIY_REPORT_CFG_FILE);
			if(paperName!=null && paperName.Length >0)
				IniFile.WriteString(sessionName,"PaperName",paperName,DIY_REPORT_CFG_FILE);
		}
		private void readPageSetting(DIYReport.ReportModel.RptReport dataReport,DevExpress.XtraPrinting.Link printLink){
			string sessionName = getSessionName(dataReport); 
			System.Drawing.Printing.Margins  margins = printLink.Margins;
			margins.Left = int.Parse(IniFile.ReadString(sessionName,"LeftMargin",margins.Left.ToString(),DIY_REPORT_CFG_FILE)); 
			margins.Top = int.Parse(IniFile.ReadString(sessionName,"TopMargin",margins.Top.ToString(),DIY_REPORT_CFG_FILE)); 
			margins.Right = int.Parse(IniFile.ReadString(sessionName,"RightMargin",margins.Right.ToString(),DIY_REPORT_CFG_FILE)); 
			margins.Bottom = int.Parse(IniFile.ReadString(sessionName,"BottomMargin",margins.Bottom.ToString(),DIY_REPORT_CFG_FILE)); 
		}
		private string getSessionName(DIYReport.ReportModel.RptReport dataReport){
			return "PageSetting_" + dataReport.IDEX.ToString(); 
		}
		//������ӡ��link
		public DevExpress.XtraPrinting.Link CreateLinkDoc(string paperName,DIYReport.ReportModel.RptReport dataReport){ 
			SetPrintPageInfo(dataReport);

			_DrawObj = new XPrintDocument(dataReport);
			printingSystem.Links.Clear();

			DevExpress.XtraPrinting.Link linkDoc = new MyPrintLink(paperName,printingSystem);//  DevExpress.XtraPrinting.Link();

			//printingSystem.Links.Add(linkDoc);
			 

			linkDoc.EnablePageDialog = false;
			

			//linkDoc.PrintingSystem = printingSystem;
		 
			linkDoc.CreateReportHeaderArea +=new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateReportHeaderArea);
            linkDoc.CreateMarginalHeaderArea += new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateMarginalHeaderArea);
			linkDoc.CreateDetailHeaderArea +=new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateDetailHeaderArea);
			linkDoc.CreateDetailArea +=new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateDetailArea);
			linkDoc.CreateDetailFooterArea +=new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateDetailFooterArea);
            linkDoc.CreateMarginalFooterArea += new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateMarginalFooterArea);
			linkDoc.CreateReportFooterArea +=new DevExpress.XtraPrinting.CreateAreaEventHandler(linkDoc_CreateReportFooterArea);

 
			return linkDoc;
		}


		#endregion �ڲ���������...
		
		#region �ڲ��ĵ��¼���������...
		private void linkDoc_CreateReportHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
		
			//string pag = e.Graph.PrintingSystem.PageSettings.PaperName    ;
			_DrawObj.DrawReportSection(e.Graph,DIYReport.SectionType.ReportTitle);  
		}
		//�ر���������xprint �У�pageheader ��ָTopMargin �� DetailHeader ָ����pageHeader ,footer Ҳ��һ����
		private void linkDoc_CreatePageHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
//			e.Graph.PrintingSystem.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
//			e.Graph.PrintingSystem.PageSettings.PaperName = "AA";
// 
			
		}
        private void linkDoc_CreateMarginalHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
            _DrawObj.DrawReportSection(e.Graph, DIYReport.SectionType.TopMargin);
        }

        private void linkDoc_CreateMarginalFooterArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
            _DrawObj.DrawReportSection(e.Graph, DIYReport.SectionType.BottomMargin);
        }
		private void linkDoc_CreateDetailHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
			
			_DrawObj.DrawReportSection(e.Graph,DIYReport.SectionType.PageHead);
		}

		private void linkDoc_CreateDetailArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {

			_DrawObj.DrawReportSection(e.Graph,DIYReport.SectionType.Detail);
		}

		private void linkDoc_CreateDetailFooterArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {

			_DrawObj.DrawReportSection(e.Graph,DIYReport.SectionType.PageFooter);
		}

		private void linkDoc_CreateReportFooterArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {

			_DrawObj.DrawReportSection(e.Graph,DIYReport.SectionType.ReportBottom);
		}
		#endregion �ڲ��ĵ��¼���������...
		
		/// <summary>
		/// 
		/// </summary>
		public DevExpress.XtraPrinting.PrintingSystem PrintingSystem{
			get{
				return  printingSystem;
			}
		}

		#region IDisposable ��Ա

		public void Dispose() {
			
		}

		#endregion

	}
	/// <summary>
	/// ��չ��֧���Զ���ֽ�ŵĴ�ӡ��
	/// </summary>
	public class MyPrintLink : DevExpress.XtraPrinting.Link{
		private string _PaperName;
		private static System.Drawing.Printing.PageSettings defaultPageSettings;
		static MyPrintLink(){
			defaultPageSettings = new System.Drawing.Printing.PrintDocument().DefaultPageSettings;
		}
		public MyPrintLink(string paperName, DevExpress.XtraPrinting.PrintingSystem printingSystem) : base(printingSystem){
			_PaperName = paperName;
		}
		protected override void ApplyPageSettings() {
			base.ApplyPageSettings ();
			System.Drawing.Printing.PaperSize paperSize = GetPaperSizeEx(_PaperName);
			this.PrintingSystem.PageSettings.Assign(this.Margins,this.PaperKind,new System.Drawing.Size(paperSize.Width,paperSize.Height),this.Landscape); 
			this.PrintingSystem.PageSettings.PaperName = _PaperName; 
		}
		
		private System.Drawing.Printing.PaperSize GetPaperSizeEx(string paperName) {
			foreach(System.Drawing.Printing.PaperSize paperSize in  defaultPageSettings.PrinterSettings.PaperSizes){
				if(paperSize.Kind == this.PaperKind && paperSize.PaperName == paperName)
					return paperSize;
			}
			return new System.Drawing.Printing.PaperSize("Custom", 850, 1100); 
		}
	}
}
