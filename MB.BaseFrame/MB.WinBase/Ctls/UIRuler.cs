//---------------------------------------------------------------- 
// Copyright
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

//using DIYReport.ReportModel ; 

namespace MB.WinBase.Ctls
{
	/// <summary>
	/// DesignRuler ������Ƶĳ��ӡ�
	/// </summary>
	public class UIRuler : System.Windows.Forms.UserControl {
		#region �ڲ��Զ����ɴ���...
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

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

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent() {
			// 
			// DesignRuler
			// 
			this.Name = "DesignRuler";
			this.Size = new System.Drawing.Size(344, 80);
			this.Resize += new System.EventHandler(this.DesignRuler_Resize);

		}
		#endregion

		#endregion �ڲ��Զ����ɴ���...

		#region ��������...

		//��ʾ��ˮƽ�ĳ��ӻ��Ǵ�ֱ�� 
		private bool _IsHorizontal = true;
		private int _BeginDrawPoint ;
		private const int LIMIT_FUDGER = 24;
		private const int SEP_FUDGER = 4;
		//private const float SEP_LET =  25.4f /72;//72��=(Լ)25.4 ����

		//private RptSectionList  _SectionList;
		#endregion ��������...

		#region ���캯��...
		public UIRuler() {
			InitializeComponent();
			_BeginDrawPoint = 12;
			if(_IsHorizontal){
				this.Height = LIMIT_FUDGER ;
			}
			else{
				this.Width = LIMIT_FUDGER;
			}
		}
		#endregion ���캯��...

		#region public ����...
		public void DrawRuler(){
			if(this.Width <=0 || this.Height <=0){
				return ;
			}
			Bitmap image = new Bitmap(this.Width , this.Height  );
			Graphics g = Graphics.FromImage(image);   
			//���ö�����λΪ ����
			//1Ӣ��=72��=(Լ)25.4 ����
			//g.PageUnit = GraphicsUnit.Millimeter;
			 
			g.FillRectangle(Brushes.LightGray ,this.DisplayRectangle);   
			if(_IsHorizontal){
				drawHRuler(g);
			}
			else{
				drawVRuler(g);
			}
			this.BackgroundImage = image;
		}
		#endregion public ����...

		#region �����...
		private void  drawHRuler(Graphics g){
			float width = this.Width   ,height =   LIMIT_FUDGER ;// * 20.4f /72 ;
			int j = 0;
			int count = 0;
			for(int i = _BeginDrawPoint ; i < width ; i++){
				if(i % SEP_FUDGER != 0  && i!= _BeginDrawPoint ) continue; 
				int sep = 1,s = j % 10;
				if(s==5){
					sep = 2;
				}
				else if(s==0){
					sep = 3;
				}
				float x1 = i ,y1 = height - 8 * sep,x2 = i ,y2 = height;
				g.DrawLine(new Pen(Brushes.Black ,1   ),x1,y1,x2,y2);  
				if(sep == 3){
					g.DrawString(count.ToString(),new Font("Tahoma",8),Brushes.Black,new PointF(x1,height - 24));
					count ++;
					
				}
				j++;
			}
		}

		private void  drawVRuler(Graphics g){
			float width = LIMIT_FUDGER  ,height = this.Height  ;
 
				int beg =  _BeginDrawPoint ,en = System.Convert.ToInt32( height );
				int j = 0,count = 0;
				for(int i = beg; i < en  ; i++){
					if(i % SEP_FUDGER !=0 && i!=beg) continue;
					int sep = 1,s = j % 10;
					if(s==5){
						sep = 2;
					}
					else if(s==0){
						sep = 3;
					}
					float x1 = width - 8 * sep ,y1 = i,x2 = width ,y2 = i;
					g.DrawLine(new Pen(Brushes.Black ,1  ),x1,y1,x2,y2);  
					if(sep == 3){
						g.DrawString(count.ToString(),new Font("Tahoma",8),Brushes.Black,new PointF(width - 24,y1));
						count ++;
					
					}
					j++;
				}
 
		}

		#endregion �����...

		#region public ����...
		public bool IsHorizontal{
			get{
				return _IsHorizontal;
			}
			set{
				_IsHorizontal = value;
				_BeginDrawPoint = _IsHorizontal? 26 : 0;

			}
		}
//		public int BeginDrawPoint{
//			get{
//				return _BeginDrawPoint;
//			}
//			set{
//				_BeginDrawPoint = value;
//			}
//		}
//		public RptSectionList  SectionList{
//			set{
//				_SectionList = value;
//				DrawRuler();
//			}
//		}
		#endregion public ����...

		#region �ڲ�������...
		struct rulerData{
			public int Begin;
			public int End;
			public rulerData(int pBegin,int pEnd){
				Begin = pBegin;
				End = pEnd;
			}
		}
//		private rulerData[] getDrawVRulerData(){
//			int count = 0;
//			int height = 0;
//			int captionHeight = SectionCaption.CAPTION_HEIGHT ;
//			if(_SectionList!=null){
//				count = _SectionList.Count ;
//			}
//			if(count==0){
//				rulerData[] data = new rulerData[1];
//				data[0].Begin = _BeginDrawPoint ;
//				data[0].End = this.Height ;
//				return data;
//			}
//			else{
//				rulerData[] data = new rulerData[count];
//				for(int i = 0;i <count;i++){
//					RptSection section = _SectionList[i] as RptSection ;
//					if(section.Visibled ){
//						data[i].Begin  = height  + captionHeight ; 
//						data[i].End =  height + section.Height + captionHeight;
//						height +=section.Height + captionHeight;
//					}
//					else{
//						data[i].Begin = -1;
//						data[i].End  = -1;
//					}
//				}
//				return data;
//			}
//		}
		#endregion �ڲ�������...

		#region �����¼�...
		private void DesignRuler_Resize(object sender, System.EventArgs e) {
			if(_IsHorizontal){
				this.Height = LIMIT_FUDGER;
			}
			else{
				this.Width = LIMIT_FUDGER;
			}
			DrawRuler();
		}
		#endregion �����¼�...
	}
}
