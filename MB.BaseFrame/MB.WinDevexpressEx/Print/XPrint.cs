//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	XPrint XWin ��ӡ����
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing;

using DevExpress.XtraPrinting;
namespace MB.XWinLib.Print
{
	/// <summary>
	/// XPrint XWin ��ӡ����
	/// </summary>
	public class XPrint
	{
		#region ��������...
		private DevExpress.XtraPrinting.PrintingSystem _PS;
		private DevExpress.XtraPrinting.PrintableComponentLink _PrintableLink;
		private XPrintParam _PrintParam;

		private static XPrint _PrintObj;

		private const int REPORT_HEADER_HEIGHT = 50;
		#endregion ��������...
		
		#region ���캯��...

		#endregion ���캯��...

       #region Instance...
        private static Object _Obj = new object();
        private static XPrint _Instance;

        /// <summary>
        /// ����һ��protected �Ĺ��캯������ֹ�ⲿֱ�Ӵ�����
        /// </summary>
        /// <summary>
        /// ���캯��...
        /// </summary>
        protected XPrint() {
            _PS = new DevExpress.XtraPrinting.PrintingSystem();
            _PrintableLink = new DevExpress.XtraPrinting.PrintableComponentLink(_PS);
           
        }

        /// <summary>
        /// ���̰߳�ȫ�ĵ�ʵ��ģʽ��
        /// ���ڶ��⹫������ʵ�ַ�����ʹ��SingletionProvider �ĵ�ʱ�����С�
        /// </summary>
        public static XPrint Instance {
            get {
                if (_Instance == null) {
                    lock (_Obj) {
                        if (_Instance == null)
                            _Instance = new XPrint();
                    }
                }
                return _Instance;
            }
        }
        #endregion Instance...

		#region public ����...
		/// <summary>
		/// ��ʾԤ�����ڡ�
		/// </summary>
		/// <param name="printable"></param>
		public void ShowPreview(DevExpress.XtraPrinting.IPrintable printable,XPrintParam printParam){
			_PrintParam = printParam;
			CreateLink(printable);
			_PrintableLink.ShowPreview();
		}
		#endregion public ����...
		
		#region protected virtual ����...
		protected virtual object CreateLink(DevExpress.XtraPrinting.IPrintable printable) {
			if(_PrintParam!=null)
				_PrintableLink.PrintingSystem.PageSettings.Landscape = _PrintParam.Landscape;
			if(_PrintableLink == null) return null;
			_PrintableLink.Component = printable;
			_PrintableLink.CreateDocument();
			return _PrintableLink;
		}
		#endregion protected virtual ����...
		
		#region �����¼�...
		private void _PrintableLink_CreatePageHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {

			if(_PrintParam!=null && _PrintParam.PageHeaderTitle!=null && _PrintParam.PageHeaderTitle.Length > 0){
				RectangleF r = new RectangleF(0, 0,e.Graph.ClientPageSize.Width, e.Graph.Font.Height);
				TextBrick brick = e.Graph.DrawString(_PrintParam.PageHeaderTitle, Color.Blue, r, BorderSide.None);
				brick.StringFormat = new BrickStringFormat(StringAlignment.Far);
				 
			}
		}

		private void _PrintableLink_CreateReportHeaderArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
			if(_PrintParam!=null && _PrintParam.ReportHeaderTitle!=null && _PrintParam.ReportHeaderTitle.Length > 0){

				RectangleF r = new RectangleF(0, 0,e.Graph.ClientPageSize.Width, REPORT_HEADER_HEIGHT);
				TextBrick brick = e.Graph.DrawString(_PrintParam.ReportHeaderTitle, Color.Black, r, BorderSide.None);
				brick.StringFormat = new BrickStringFormat(StringAlignment.Center);
				brick.Font = getDefaultFont(12F,FontStyle.Bold);

			}
		}

		private void _PrintableLink_CreatePageFooterArea(object sender, DevExpress.XtraPrinting.CreateAreaEventArgs e) {
			string format = "��ǰҳ:{0}  ��ҳ�� {1}";
			e.Graph.Font = e.Graph.DefaultFont;
			e.Graph.BackColor = Color.Transparent;

			RectangleF r = new RectangleF(0, 0, 0, e.Graph.Font.Height);

			PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, format, Color.Black, r, BorderSide.None);
			brick.Alignment = BrickAlignment.Far;
			brick.AutoWidth = true;

			brick = e.Graph.DrawPageInfo(PageInfo.DateTime, "", Color.Black, r, BorderSide.None);
			brick.Alignment = BrickAlignment.Near;
			brick.AutoWidth = true;
		}
		#endregion �����¼�...

		#region �ڲ���������...
		//��ȡĬ�����õ�����
		private Font getDefaultFont(float size,System.Drawing.FontStyle style){
			return new Font("Microsoft Sans Serif",size,style, System.Drawing.GraphicsUnit.Point);
		}
		
		#endregion �ڲ���������...
	}


}
