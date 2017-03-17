//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-22
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing;
using System.Diagnostics ;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DIYReport.Print
{
	/// <summary>
	/// RptPageSetting ��ӡҳ������
	/// </summary>
	public class RptPageSetting
	{
		//1 inch Ӣ��=25.4 millimetres ����
		private static double SEP_MINI = System.Convert.ToDouble(65 / 25.4f); // System.Convert.ToDecimal(3.938)  ;// 
		/// <summary>
		/// ����ҳ�����õĴ���
		/// </summary>
		/// <param name="printDocument"></param>
		/// <returns></returns>
		public  static void ShowPageSetupDialog(DIYReport.ReportModel.RptReport pReport) {
			PageSettings ps = new PageSettings();
 
			try {
				PageSetupDialog psDlg = new PageSetupDialog();
				psDlg.Document = pReport.PrintDocument;
				
				//pReport.PrintDocument.DefaultPageSettings.Margins = pReport.Margins;
				pReport.PrintDocument.DefaultPageSettings.Margins  = new System.Drawing.Printing.Margins( pReport.Margins.Left,
					pReport.Margins.Right,
					pReport.Margins.Top ,
					pReport.Margins.Bottom);

				psDlg.PageSettings = pReport.PrintDocument.DefaultPageSettings ;
				//psDlg.AllowPaper = true;
				DialogResult result = psDlg.ShowDialog();
				if (result == DialogResult.OK) {
					ps = psDlg.PageSettings;
					pReport.PrintDocument.DefaultPageSettings = psDlg.PageSettings;
					pReport.Margins =   convertTomini(psDlg.PageSettings.Margins) ;
					pReport.PrintDocument.DefaultPageSettings.Margins =  pReport.Margins ;
					pReport.PaperSize = psDlg.PageSettings.PaperSize ;
					pReport.IsLandscape = psDlg.PageSettings.Landscape ;
				}
			}
			catch(System.Drawing.Printing.InvalidPrinterException e) {
				MessageBox.Show("δ��װ��ӡ���������ϵͳ���������Ӵ�ӡ����" ,"��ӡ",MessageBoxButtons.OK,MessageBoxIcon.Information);
				Debug.Assert(false,e.Message ,"��ӡ");
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message,"��ӡ",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}

		//	return ps;
//
//			FrmPageSetting frm = new FrmPageSetting(pReport);
//			frm.ShowDialog(); 
		}
		/// <summary>
		/// �������Ƶõ�PaperSize (�÷��������Ѿ�ֹͣʹ�õ�FrmPageSetting�⣬�������Ѿ�����ʹ��)
		/// </summary>
		/// <param name="pDoc"></param>
		/// <param name="pPaperName"></param>
		/// <returns></returns>
		public static System.Drawing.Printing.PaperSize GetPaperSizeByName(System.Drawing.Printing.PrintDocument pDoc,
											string printName,string pPaperName){
			System.Drawing.Printing.PrintDocument doc =pDoc;
			if(doc==null){
				doc = new PrintDocument();
			}
			if(printName!=null && printName.Length >0)
				doc.PrinterSettings.PrinterName = printName;
 
			for(int i  = 0; i < doc.PrinterSettings.PaperSizes.Count;i ++){
				string paName = doc.PrinterSettings.PaperSizes[i].PaperName ;
				if(paName == pPaperName){
					return doc.PrinterSettings.PaperSizes[i];
				}
			}
			
			//�Ҳ����Ļ�����һ��A4�ĸ�ʽֽ��
			return new PaperSize(pPaperName,210,297); 
		}
		/// <summary>
		/// ͨ����������Ϣ��ȡpaperSize.
		/// </summary>
		/// <param name="pDoc"></param>
		/// <param name="printerName"></param>
		/// <param name="paperName"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static System.Drawing.Printing.PaperSize GetSizeByFullInfo(System.Drawing.Printing.PrintDocument pDoc,
				string printerName,string paperName,int width,int height){
			/*
			//����ָ���Ĵ�ӡ���ʹ�ӡֽ�����ƻ�ȡ��
			//�����ȡ�����ٸ��ݴ�ӡֽ����������ȡ��
			//����ٻ�ȡ�����ٸ��� width ��height ����ȡ��
			//����ٻ�ȡ�����ٸ��� width ��height��ȡ�����ֽ�š�
			*/
			System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            int count = 0;
            try {
                count = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count;
            }
            catch (Exception ex) {
                throw new Exception("��ȡ��װ�Ĵ�ӡ����Ϣ���� " + ex.Message);

            }
			int SEP_WIDTH = 2;
			bool existsPrinter = false;
            if (!string.IsNullOrEmpty(printerName)) {
                for (int i = 0; i < count; i++) {
                    string print = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                    if (string.Compare(print, printerName, true) == 0) {
                        existsPrinter = true;
                        break;
                    }
                }
            }
			if(existsPrinter){
				doc.PrinterSettings.PrinterName = printerName;
				foreach(System.Drawing.Printing.PaperSize size in doc.PrinterSettings.PaperSizes){
					if(string.Compare(size.PaperName,paperName,true)==0){
						return size;
					}
				}
			}
            if (!string.IsNullOrEmpty(paperName)) {
                //�������Ĵ����ȡ��������ô��ȡֽ��������ͬ���Ҵ�С��ͬ��
                for (int i = 0; i < count; i++) {
                    string print = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                    doc.PrinterSettings.PrinterName = print;
                    foreach (System.Drawing.Printing.PaperSize size in doc.PrinterSettings.PaperSizes) {
                        if (string.Compare(size.PaperName, paperName, true) == 0 && (size.Width == width || width == 0) && (size.Height == height || height == 0)) {
                            return size;
                        }
                    }
                }
            }
			//����ٻ�ȡ�����ٸ��� width ��height ����ȡ��
			for(int i = 0 ;i <count; i++){
				string print = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
				doc.PrinterSettings.PrinterName = print;
				foreach(System.Drawing.Printing.PaperSize size in doc.PrinterSettings.PaperSizes){
					if(size.Width == width && size.Height == height ){
						return size;
					}
				}
			}
			//����ٻ�ȡ�����ٸ��� width ��height��ȡ�����ֽ�š�
			for(int i = 0 ;i <count; i++){
				string print = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
				doc.PrinterSettings.PrinterName = print;
				foreach(System.Drawing.Printing.PaperSize size in doc.PrinterSettings.PaperSizes){
					if((size.Width - SEP_WIDTH < width && width < size.Width + SEP_WIDTH)  && 
						size.Height - SEP_WIDTH < height && height < size.Height + SEP_WIDTH ){
						return size;
					}
				}
			}
			//�Ҳ����Ļ�����һ��A4�ĸ�ʽֽ�š�
			MessageBox.Show("�ڸü�������Ҳ�����ƵĴ�ӡ������ֽ�����ͣ������°���ơ�");
			return new PaperSize(paperName,210,297); 
		}
		#region �ڲ��Զ�����...
		//����

		//��ģ�������Ŀ϶���microsoft ��һ�� bug!
		//��ô���㶼������ʵ����ʾ�����֣�ֻ���Ȱ�������һ��bug���ڴ�����
		private static  Margins convertTomini(Margins pMar){
			Margins  m = new Margins();
			m.Left = System.Convert.ToInt32(pMar.Left * SEP_MINI);
			m.Right = System.Convert.ToInt32(pMar.Right * SEP_MINI);
			m.Top  = System.Convert.ToInt32(pMar.Top * SEP_MINI);
			m.Bottom = System.Convert.ToInt32(pMar.Bottom * SEP_MINI);

			return m;
		}
		#endregion �ڲ��Զ�����...
	}
}
