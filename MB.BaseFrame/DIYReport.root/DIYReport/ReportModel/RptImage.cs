//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-08
// Description	:	 ����Image ͼ��
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing ;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel
{
	/// <summary>
	/// RptImage ����Image ͼ��
	/// </summary>
	public class RptImage : DIYReport.ReportModel.RptSingleObj 
	{ 
		private System.Windows.Forms.PictureBoxSizeMode _DrawSizeModel;
		private bool _DrawFrame;
		public RptImage(string pName,DIYReport.ReportModel.RptObjType type) : base(pName,type) {
		}
		[Description("����ͼ����ָ�����е�λ��."),Category("��Ϊ")]
		public System.Windows.Forms.PictureBoxSizeMode DrawSizeModel{
			get{
				return _DrawSizeModel;
			}
			set{
				_DrawSizeModel = value;
			}
		}
		[Description("��ȡ���������Ƿ����ͼ��ı߿�."),Category("��Ϊ")]
		public bool DrawFrame{
			get{
				return _DrawFrame;
			}
			set{
				_DrawFrame = value;
			}
		}
	}
}
