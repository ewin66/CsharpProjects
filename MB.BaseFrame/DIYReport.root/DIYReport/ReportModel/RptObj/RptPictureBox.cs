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
using System.Drawing ;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// RptPictureBox ��ͼ��
	/// </summary>
	public class RptPictureBox : DIYReport.ReportModel.RptImage  
	{
		private Image _Image;
		private bool _ShowFrame;
		public RptPictureBox() : this(null){
		}
		public RptPictureBox(string pName): base(pName,DIYReport.ReportModel.RptObjType.Image)
		{
		}
		#region public ����...
		[Description("���û��ߵõ�Ҫ��ӡ��ͼƬ��"),Category("����")]
		public Image Image{
			get{
				return _Image;
			}
			set{
				if(_Image!=null){
					_Image.Dispose();
				}
				Bitmap bit = new Bitmap(value);

				System.Drawing.Image img =  System.Drawing.Image.FromHbitmap(bit.GetHbitmap());
				_Image = img;
				value.Dispose();
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Description("���û��ߵõ����������Ƿ���ʾ�߿�"),Category("���")]
		public bool ShowFrame {
			get {
				return _ShowFrame;
			}
			set {
				_ShowFrame = value;
				if(base.IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		#endregion public ����...

		#region ICloneable Members

		public  override object Clone() {
			RptPictureBox info = this.MemberwiseClone() as RptPictureBox ;
			if(_Image!=null){
				try{
					info.Image = _Image.Clone() as Image ;
				}
				catch{}
			}
			
			return info;
		}
 
		#endregion ICloneable Members
	}
}
