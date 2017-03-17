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
using System.ComponentModel.Design;
namespace DIYReport.ReportModel
{
	/// <summary>
	///  �¼�ί������
	/// </summary>
	public delegate void RptEventHandler(object sender, RptEventArgs e);

	/// <summary>
	/// RptEventArgs ������������¼�������
	/// </summary>
	public class RptEventArgs: EventArgs {
	}

	#region ����IO ����������Զ����¼�����...
	public delegate void XReportIOEventHandler(object sender,XReportIOEventArgs e);
	
	public class XReportIOEventArgs : EventArgs{
		private RptReport _DataReport;
		private bool _HasProcessed;
		private CommandID  _CommandID;
		public XReportIOEventArgs(RptReport dataReport,CommandID  commandID){
			_DataReport = dataReport;
			_CommandID = commandID;
		}

		#region public ����...
		public RptReport DataReport{
			get{
				return _DataReport;
			}
			set{
				_DataReport = value;
			}
		}
		public bool HasProcessed{
			get{
				return _HasProcessed;
			}
			set{
				_HasProcessed = value;
			}
		}
		public CommandID CommandID{
			get{
				return _CommandID;
			}
			set{
				_CommandID = value;
			}
		}
		#endregion public ����...

	}
	#endregion ����IO ����������Զ����¼�����...

//	public enum XReportIOHandlerType{
//		NewReport,
//		Open,
//		Save,
//		SaveAs,
//	}
}
