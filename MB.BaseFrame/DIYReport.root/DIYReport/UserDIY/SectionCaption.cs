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

namespace DIYReport.UserDIY
{
	/// <summary>
	/// SectionCaption ��ժҪ˵����
	/// </summary>
	[ToolboxItem(false)]
	public class SectionCaption : System.Windows.Forms.UserControl
	{
		#region �ڲ��Զ����ɴ���...
		private System.Windows.Forms.Label labCaption;
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
			this.labCaption = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labCaption
			// 
			this.labCaption.Location = new System.Drawing.Point(0, 0);
			this.labCaption.Name = "labCaption";
			this.labCaption.Size = new System.Drawing.Size(200, 16);
			this.labCaption.TabIndex = 0;
			this.labCaption.Text = "label1";
			this.labCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labCaption_MouseDown);
			// 
			// SectionCaption
			// 
			this.BackColor = Color.Silver ;
			this.Controls.Add(this.labCaption);
			this.Name = "SectionCaption";
			this.Size = new System.Drawing.Size(416, 32);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SectionCaption_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion �ڲ��Զ����ɴ���...

		#region ��������...
		public static int CAPTION_HEIGHT = 14;
		private DIYReport.UserDIY.DesignSection _Section;
		#endregion ��������...

		#region ���캯��...
		public SectionCaption() {
			InitializeComponent();
			
			this.Height = CAPTION_HEIGHT;
			labCaption.Height = CAPTION_HEIGHT;
			labCaption.Location = new Point(0,0);

		}
		#endregion ���캯��...
		#region ��չ��Public ����...
		public void SetActive(bool pActive){
			if(pActive){
				this.BackColor = Color.Blue;
				labCaption.ForeColor = Color.White ;  
			}
			else{
				this.BackColor = Color.Silver  ;
				labCaption.ForeColor = Color.Black ;
				
			}
			labCaption.BackColor =  this.BackColor;
		}
		#endregion ��չ��Public ����...

		private void SectionCaption_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.Section.SectionList.SetActiveSection(this.Section); 

			DesignEnviroment.CurrentRptObj = this.Section.DataObj ; 
			DesignEnviroment.UICmidExecutor.ExecCommand(UICommands.SetObjProperty) ; 
		}

		private void labCaption_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.Section.SectionList.SetActiveSection(this.Section); 

			DesignEnviroment.CurrentRptObj = this.Section.DataObj ; 
			DesignEnviroment.UICmidExecutor.ExecCommand(UICommands.SetObjProperty) ; 
		}
		#region ��չ��Public ����...
		
		public DIYReport.UserDIY.DesignSection Section{
			get{
				return _Section;
			}
			set{
				_Section = value;
			}
		}
		public string Caption{
			get{
				return labCaption.Text ;
			}
			set{
				labCaption.Text = value;
			}
		}
		#endregion ��չ��Public ����...
	}
}
