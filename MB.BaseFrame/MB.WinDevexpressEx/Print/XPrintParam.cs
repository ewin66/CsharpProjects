//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	XPrintParam XPrint ��Ҫ�Ĳ���������Ϣ��
//                  ��Ҫ�����ñ���ı��� �͹�˾��Loger
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;

namespace MB.XWinLib.Print
{
	/// <summary>
	/// XPrintParam XPrint ��Ҫ�Ĳ���������Ϣ��
	/// </summary>
	public class XPrintParam
	{
		private string _ReportHeaderTitle;
		private string _PageHeaderTitle;
		private bool _Landscape;
		
		/// <summary>
		/// ���캯��
		/// </summary>
		public XPrintParam(string reportTitle,string pageHeader)
		{
			_ReportHeaderTitle = reportTitle;
			_PageHeaderTitle = pageHeader;
			_Landscape = false;
		}

		#region Public ����...
		public string ReportHeaderTitle{
			get{
				return _ReportHeaderTitle;
			}
			set{
				_ReportHeaderTitle = value;
			}
		}
		public string PageHeaderTitle{
			get{
				return _PageHeaderTitle;
			}
			set{
				_PageHeaderTitle = value;
			}
		}
		public bool Landscape{
			get{
				return _Landscape;
			}
			set{
				_Landscape = value;
			}
		}
		#endregion Public ����...
	}
}
