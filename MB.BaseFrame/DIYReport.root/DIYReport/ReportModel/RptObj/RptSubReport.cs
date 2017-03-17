//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-03
// Description	:   RptSubReport���״�ģ���´����ӱ���������
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptSubReport���״�ģ���´����ӱ���������
	/// </summary>
	public class RptSubReport: DIYReport.ReportModel.RptSingleObj {
		private string _ReportFileName;
		private string _RelationMember;//Ŀǰ����Ƶ�״̬�£�_RelationMember�����ƾ���_ReportFileName
		private int _PreviewRowCount;//��ʾ������¼��
		private bool _FixedWidth;//�ж��Ƿ�����ָ�����ӱ���ؼ����������
		private bool _FixedHeight;//�ж��Ƿ�����ָ�����ӱ���ؼ��߶�������


		public RptSubReport() : this(null){
		}
		/// <summary>
		/// RptOleObject ���״�ģ���´����ӱ���������
		/// </summary>
		/// <param name="pName"></param>
		public RptSubReport(string pName) : base(pName,DIYReport.ReportModel.RptObjType.SubReport) {
			_PreviewRowCount = 100;
		}

		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		#region public ����...
		[Description("���û��ߵõ�Ҫ��ӡ���ӱ����ļ����ơ�"),Category("����"),Editor(typeof(DIYReport.Design.RptSubReportAttributesEditor), typeof(UITypeEditor))]
		public string ReportFileName{
			get{
				return _ReportFileName;
			}
			set{
				_ReportFileName = value;
				_RelationMember = _ReportFileName;
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Description("������Դ��ָ���ݵ����ơ�"),Category("����"),Editor(typeof(DIYReport.Design.RelationMemberAttributesEditor), typeof(UITypeEditor))]
		public string RelationMember{
			get{
				return _RelationMember;
			}
			set{
				_RelationMember = value;
			}
		}
		[Description("���û��ߵõ�Ҫ��ӡ���ӱ���ļ�¼����"),Category("����")]
		public int PreviewRowCount{
			get{
				return _PreviewRowCount;
			}
			set{
				_PreviewRowCount = value;
			}
		}
		[Description("�ж��Ƿ�����ָ�����ӱ���ؼ���������ơ�"),Category("���")]
		public bool FixedWidth{
			get{
				return _FixedWidth;
			}
			set{
				_FixedWidth = value;
			}
		}
		[Description("�ж��Ƿ�����ָ�����ӱ���ؼ��߶������ơ�"),Category("���")]
		public bool FixedHeight{
			get{
				return _FixedHeight;
			}
			set{
				_FixedHeight = value;
			}
		}
 
		#endregion public ����...

	}
}
