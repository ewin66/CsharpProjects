//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-22
// Description	:	 DrawReport ����XML�ͼ�¼��������(nick 2006-04-10 ������ Ϊ�˴������up2������)
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
//' ���ö�����λΪ ����
//e.Graphics.PageUnit = GraphicsUnit.Millimeter
using System;
using System.Drawing;
using System.Diagnostics ;
using System.Drawing.Printing;
using System.Data;
using System.Reflection ;
using System.Collections;

using DIYReport.ReportModel;
using DIYReport.Express ; 
namespace DIYReport.Print {
	/// <summary>
	/// DrawReport ����XML�ͼ�¼��������()
	/// </summary>
	public class DrawReport : DIYReport.Interface.IDrawReport {

		#region ��������...
		//�õ�ҳ����Ҫ������ֶ�
		private DIYReport.Express.ExpressValueList    _FooterExpress;
		//�õ��������Ҫ��ͳ�Ƽ�����ֶ�
		private DIYReport.Express.ExpressValueList _BottomExpress;
		//��Ҫ�滭�ı�������
		private object _RptData=null; 
		private int _RowPoint = 0;
		private int _HasDrawRowCount = 0;
		private int _PageNumber = 1;
		//Ϊ�˻�����ĽŵĶ����õĵ�ǰ������
		//private int _BottomRow = 0;
		//�ж��Ƿ�Ϊ���һҳ
		private bool _IsLastPage = false;
		private System.Drawing.Printing.PaperSize _DocSize;
		private System.Drawing.Printing.Margins _DocMargin ;

		private DIYReport.ReportModel.RptReport _DataReport;
		private ReportDrawInfo _RptInfo;
		private DrawDetailInfo _DrawDetailInfo;

		private DataRow[] _Rows;
		//�ڵ�ǰҳ���Ѿ����Ƶĸ߶�
		private int _HasDrawDetailHeight = 0;
//		//�ڶ��ֶη����������Ѿ����Ƶ��������index
//		private int _HasDrawGroupHeadMaxIndex = -1;
		//׼����Ҫ���Ƶ�Group Section �� head���У���Ҫ�Ƚ����Ȼ��ƣ�
		private System.Collections.Queue _GroupHeads;
		//׼����Ҫ���Ƶ�Group Section footer��ջ����Ҫ�Ƚ��ĺ���ƣ�
		private Stack _GroupFoots;
		//��ϸ���ֵ����߶�
		private readonly int REAL_DETAIL_HEIGHT ;
		//���ò���ҳ�������
		private readonly int REAL_PAGE_WIDTH ;

		#endregion ��������...
		public DIYReport.ReportModel.RptReport DataReport{
			get{
				return _DataReport;
			}
		}
		#region Public ����...

		#endregion Public ����...

		//		#region �¼��������...
		//		private DrawObjEventHandler _BeforDrawObject;
		//		public event DrawObjEventHandler BeforDrawObject {
		//			add { 
		//				_BeforDrawObject += value; 
		//			} 
		//			remove { 
		//				_BeforDrawObject -= value; 
		//			}
		//
		//		}
		//		private void FireBeforDrawObject(DrawObjEventArgs e) {
		//			if (mBeforDrawObject != null) {
		//				// ������Ӧ��ί�д���
		//				mBeforDrawObject(this, e);
		//			} 
		//		}
		//		#endregion �¼��������...

		public DrawReport(object pDs,DIYReport.ReportModel.RptReport pDataReport) {
			_RptData = pDs;
			DIYReport.UserDIY.DesignEnviroment.DataSource =  pDs;
 
			_DataReport = pDataReport;
			_Rows = DIYReport.GroupAndSort.GroupDataProcess.SortData(pDs,pDataReport);   

			_RptInfo = new ReportDrawInfo(pDataReport);
			 
			_FooterExpress = DIYReport.Express.ExpressValueList.GetFooterExpress(pDataReport);
			_BottomExpress = DIYReport.Express.ExpressValueList.GetBottomExpress(pDataReport) ;
			
			_DocSize = _DataReport.PaperSize;

			_DocMargin = _DataReport.Margins ;

			//��ʼ��ҳҳ��
			DIYReport.Express.ExSpecial._Page = 1;   
			DIYReport.Express.ExSpecial._PageCount = 1;

			_DrawDetailInfo = new DrawDetailInfo(pDataReport);
			_GroupFoots = new Stack();
			_GroupHeads = new Queue();

			float mergeHeight = _DataReport.SectionList.GetExceptDetailHeight() ;
			int rHeight = _DataReport.IsLandscape? _DocSize.Width : _DocSize.Height;
			REAL_DETAIL_HEIGHT = rHeight  - Convert.ToInt32(mergeHeight) - _DocMargin.Top - _DocMargin.Bottom ;
			int rWidth = _DataReport.IsLandscape? _DocSize.Height : _DocSize.Width;
			REAL_PAGE_WIDTH = rWidth - _DocMargin.Left - _DocMargin.Right;
			DIYReport.Express.ExSpecial._RowOrderNO = 0;
		}
		#region IDDispose...
		private bool disposed = false;
		private ArrayList _ImageList;
		/// <summary>
		/// ��������������
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing) {
			// Check to see if Dispose has already been called.
			if(!disposed) {
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing) {
					// Dispose managed resources.
				}
				if(_ImageList!=null && _ImageList.Count>0){
					foreach(Image img in _ImageList)
						img.Dispose();
				}
			}
			disposed = true;         
		}
		~DrawReport() {
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}
		#endregion IDDispose...
		/// <summary>
		/// ��ӡ֮ǰ��Ҫ��ʼ��������
		/// </summary>
		public void BeginPrint(){
			HasDrawRowCount=0;
			PageNumber = 1;
			DIYReport.Express.ExSpecial._Page = 1;   
			DIYReport.Express.ExSpecial._PageCount = 1;
			DIYReport.Express.ExSpecial._RowOrderNO = 0;
			_GroupFoots.Clear();
			_GroupHeads.Clear();
			foreach(object field in _DrawDetailInfo.GroupFields){
				DrawGroupField groupField = field as DrawGroupField;
				groupField.CurrGroupValue = null; 
			}
		}
		
		/// <summary>
		/// ���Ʊ����Section 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pSectionType"></param>
		public bool DrawReportSection(object graphicsObject,DIYReport.SectionType pSectionType){
			Graphics g = graphicsObject as Graphics;

			DIYReport.ReportModel.RptSection section = _DataReport.SectionList.GetSectionByType( pSectionType);  
			if(section.Visibled){
				//�����Section �ڻ� Detail ��ʱ����
				if(section!=null  && pSectionType!=DIYReport.SectionType.GroupHead && pSectionType!=DIYReport.SectionType.GroupFooter ) {
					if(pSectionType==DIYReport.SectionType.Detail ){
						return DrawDetail(g,section);
					}
					else{
						foreach(DIYReport.ReportModel.RptSingleObj   obj in section.RptObjList) {
							DrawRptSimpleObj(g,obj);
						}
					}
				}
			}
			return true;
		}
		#region ���������ϸ��Ϣ...
		/// <summary>
		/// ����ϸ��Ϣ 
		/// </summary>
		/// <param name="g"></param>
		/// <returns>���������һҳ����ô����True,���� False</returns>
		private bool DrawDetail(Graphics g,DIYReport.ReportModel.RptSection pSection) {
			if(_RptData==null || pSection==null || pSection.Height == 0 || pSection.RptObjList.Count == 0 ) {
				_IsLastPage = true;
				return false;
			}
			int initialRow = this.HasDrawRowCount ;
			_RowPoint = 0;
			_HasDrawDetailHeight = 0;
			//_BottomRow = 0;
			_IsLastPage = false;
			bool reB = false;
			//�ж��Ƿ����ʣ�����ͷ������Ų�������
			drawLeaveGroupInfo(g);
			if(_Rows.Length > 0 && initialRow==0){
				analyseGroupSection(0);
				reB = drawGroupHead(g);
				if(reB){
					return true;
				}
			}
			for (int i = initialRow; i < _Rows.Length; i++) {
				DIYReport.Express.ExSpecial._RowOrderNO ++;
				foreach(RptSingleObj  obj in pSection.RptObjList) {
					DrawRptSimpleObj(g,obj,_Rows[i]);
				}
				_HasDrawDetailHeight += pSection.Height;
				_RowPoint++;
				_HasDrawRowCount ++;
				
				//������һ�������е���Ϣ����������������Ƶ�ǰ�е���Ϣ�����������ô�ͻ���
				analyseGroupSection(i + 1);
				reB = drawGroupFoot(g);
				if(reB){//�ж��Ƿ�������
					return true;
				}
				reB = drawGroupHead(g);
				if(reB){
					return true;
				}
				//�Ѿ��������еĸ߶� 
				int temHeight = _HasDrawDetailHeight + pSection.Height  ;
				if( temHeight > REAL_DETAIL_HEIGHT ) {
					return true;
				}
			}
			int temH = _HasDrawDetailHeight  + _RptInfo.BottomHeight;
			if( temH > REAL_DETAIL_HEIGHT ) {
				return true;
			}
			//_BottomRow = _RowPoint;
			int mergeCount = REAL_DETAIL_HEIGHT / pSection.Height;
			mergeCount -= _RowPoint;
			mergeCount +=1;
			//�ж��Ƿ���ʣ��Ŀռ䣬������ 
			for(int j =0; j< mergeCount ;j++) {
				if(_DataReport.FillNULLRow){
					foreach(RptSingleObj  obj in pSection.RptObjList) {
						drawRect(g,obj);
					}
				} 
				_RowPoint++;
			} // end for(int j =0; j< mergeCount ;j++) 
			_IsLastPage = true;
			return false;
		}
		#endregion ���������ϸ��Ϣ...

		#region ����Group Section ��ͷ�ͽ���Ϣ...
		//�����ֶε�Group Section��Ϣ�����洢�ڶ�Ӧ�Ķ��к�ջ��
		private void analyseGroupSection(int pRowIndex){
			int rowCount = _Rows.Length;
			IList fields =  _DrawDetailInfo.GroupFields ;
			bool isEnd = false;
			if(fields!=null && fields.Count >0){
				int count = fields.Count;
				int drawFoot = -1;
				int iniGroupIndex = 0;
				if(pRowIndex < rowCount){
					DataRow dRow = _Rows[pRowIndex];
					bool mustDrawHead = false;
					for(int i = iniGroupIndex  ; i <count; i++){
						DrawGroupField groupField = fields[i] as DrawGroupField;
						string fieldName = groupField.GroupFieldName;
						object val = dRow[fieldName];
						DIYReport.ReportModel.RptSection section = _DataReport.SectionList.GetByGroupField(fieldName,true); 
						//�ж��е�ֵ���Ѿ����Ƶ�grou p section head �����ڷ������ֶε�ֵ�Ƿ���ͬ
						bool inTheGroup = !mustDrawHead && DIYReport.GroupAndSort.GroupDataProcess.ValueInTheGroup(section, groupField.CurrGroupValue,val);  
						groupField.CurrGroupValue = val;
						if(!inTheGroup && !mustDrawHead){
							drawFoot = i;
							mustDrawHead = true;
						}
						if(!groupField.HasDrawGroupHead || mustDrawHead){
							//����Ҫ���Ƶ�group section ��head ��ӵ����еĽ�β
							groupField.PrevFirstRowIndex   = groupField.FirstRowIndex==-1?0:groupField.FirstRowIndex;
							groupField.FirstRowIndex   = _HasDrawRowCount;
							_GroupHeads.Enqueue( section);
							
						}
					}
				}
				else{
					drawFoot = 0;
					isEnd = true;
				}
				if(drawFoot > -1 ){
					for(int i = drawFoot; i <count;i++){
						DrawGroupField groupField = fields[i] as DrawGroupField;
						if(isEnd){
							groupField.PrevFirstRowIndex = groupField.FirstRowIndex; 
						}
						//groupField.FirstRowIndex = groupField.PrevFirstRowIndex; 
						string fieldName = groupField.GroupFieldName;
						DIYReport.ReportModel.RptSection section = _DataReport.SectionList.GetByGroupField(fieldName,false);   
						//����Ҫ���Ƶ�group section ��footer ѹ��ջ��
						_GroupFoots.Push(section);  
						//groupField.HasDrawGroupHead = false;
					} // end for(int i = drawFoot; i <count;i++){
				} //end drawFoot > -1
			} // end fields!=null && fields.Count >0
		}
		//�����ֶη����ͷ
		//���� true ��ʾ��û�н���
		private bool drawGroupHead(Graphics g){

			if(_GroupHeads.Count > 0){
				int count = _GroupHeads.Count;
				//_HasDrawDetailHeight
				for(int i = 0; i < count; i ++){
					DIYReport.ReportModel.RptSection section = _GroupHeads.Dequeue() as DIYReport.ReportModel.RptSection;
					if(section.Visibled){
						int tempHeight = _HasDrawDetailHeight + section.Height ;
						if(tempHeight > REAL_DETAIL_HEIGHT){
							return true;
						}
						DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(section.GroupField.FieldName);
   
						groupField.HasDrawGroupHead = true;
						foreach(RptSingleObj  obj in section.RptObjList) {
							DrawRptSimpleObj(g,obj,_Rows[_HasDrawRowCount]);
						}
						_HasDrawDetailHeight +=section.Height;

						if(count > 0 && i == count -1){
							DIYReport.Express.ExSpecial._RowOrderNO = 0 ;  
						}
					}
				}
			}
			return false;
		}
		//�����ֶη���Ľ�
		//���� true ��ʾ��û�н���
		private bool drawGroupFoot(Graphics g){
			if(_GroupFoots.Count > 0){
				int count = _GroupFoots.Count;
				//_HasDrawDetailHeight
				for(int i = 0; i < count; i ++){
					DIYReport.ReportModel.RptSection section = _GroupFoots.Peek() as DIYReport.ReportModel.RptSection;
					if(section.Visibled){
						int tempHeight = _HasDrawDetailHeight + section.Height ;
						if(tempHeight > REAL_DETAIL_HEIGHT){
							return true;
						}
						section = _GroupFoots.Pop() as DIYReport.ReportModel.RptSection;
						DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(section.GroupField.FieldName);
   
						groupField.HasDrawGroupHead = false;
						DIYReport.Express.ExSpecial._RowOrderNO = 0;
						foreach(RptSingleObj  obj in section.RptObjList) {
							DrawRptSimpleObj(g,obj,_Rows[_HasDrawRowCount - 1]);
						}
						_HasDrawDetailHeight +=section.Height;
					}
				}
			}
			return false;
		}
		//������ҳʣ�µķ���ͷ���߽�
		private bool drawLeaveGroupInfo(Graphics g){
			bool b = drawGroupHead(g);
			if(b){
				return true;
			}
			b = drawGroupFoot(g);
			return b;
		}
		#endregion ����Group Section ��ͷ�ͽ���Ϣ...

		#region �ڲ����� �õ���������������λ�� �ͻ�����Ķ���...
		//������ÿһ������
		private void DrawRptSimpleObj(Graphics g,RptSingleObj  pObj) {
			DrawRptSimpleObj(g,pObj,null);
		}
		private void DrawRptSimpleObj(Graphics g,DIYReport.Interface.IRptSingleObj  pObj,DataRow pDRow) {
			switch(pObj.Type) {
				case RptObjType.Text :
				case RptObjType.Express :
					drawTextObj(g,pObj,pDRow);
					break;
				case RptObjType.Line :
					drawLine(g,pObj);
					break;
				case RptObjType.Rect :
					drawRect(g,pObj);
					break;
				case RptObjType.Image :
					drawImage(g,pObj); 
					break;
				default:
					Debug.Assert(false,"��ȷ�ϱ����XML�ļ������Ƿ���ȷ",
						"�ڱ�����һ�����" + pObj.Name + " �����Ͳ������ó�:" + pObj.Type.ToString() +"����");
					break;
			}
		}

		//���ı�����
		private void drawTextObj(Graphics g,DIYReport.Interface.IRptSingleObj  pObj,DataRow pDRow){
			DIYReport.Interface.IRptTextObj textObj = pObj as DIYReport.Interface.IRptTextObj;
			StringFormat strFormat = new StringFormat();
			SolidBrush foreBrush = new SolidBrush(pObj.ForeColor);
			if(textObj.WordWrap==false) {
				strFormat.Trimming = StringTrimming.EllipsisCharacter;
				strFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;
			}
			else {
				strFormat.FormatFlags = StringFormatFlags.LineLimit;
			}
			strFormat.Alignment = textObj.Alignment;  
			RectangleF strRect = RealRectangle(textObj);
			SizeF fontSize = g.MeasureString("A",textObj.Font);
			float fontFirstY = strRect.Y  +  (textObj.Size.Height - fontSize.Height)/2; 
			strRect.Y = fontFirstY;
			if(textObj.Type == DIYReport.ReportModel.RptObjType.Text){
				DIYReport.ReportModel.RptObj.RptLable lab = pObj as DIYReport.ReportModel.RptObj.RptLable;
				if(lab.Text!=null){
					g.DrawString(lab.Text,lab.Font,foreBrush,strRect,strFormat);   
				}
			}
			else{
				//����󶨵Ĳ����ı������ֶΡ����ʽ����ϵͳ�����ȣ���ô����Ҫ�������н��Ͳ�������
				string val = "";
				DIYReport.ReportModel.RptObj.RptExpressBox box = pObj as DIYReport.ReportModel.RptObj.RptExpressBox;
				if(box.DataSource!=null && box.DataSource!="" && box.DataSource!="[δ��]"){
					switch(box.ExpressType ){
						case ReportModel.ExpressType.Express :
							int beginIndex= 0,endIndex = 0;
							if(box.Section.SectionType == DIYReport.SectionType.ReportBottom){ //ͳ�Ƶ��ֶ��ڱ���ţ���ô��ͳ�����е���
								beginIndex = 0;
								endIndex = _Rows.Length;
							}
							else if(box.Section.SectionType == DIYReport.SectionType.PageFooter){//ͳ�Ƶ��ֶ���ҳ�ţ���ô��ͳ�Ƶ�ǰҳ���е���
								beginIndex = _HasDrawRowCount - _RowPoint;
								endIndex =  _HasDrawRowCount;
							}
							else if(box.Section.SectionType == DIYReport.SectionType.GroupFooter){//ͳ�Ƶ��ֶ��ڷ���Ľţ���ôͳ�Ʒ����漰������
								DIYReport.Print.DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(box.Section.GroupField.FieldName); 
								beginIndex = groupField.PrevFirstRowIndex;
								endIndex =  _HasDrawRowCount;
							}
							else{
								//g.DrawString("��֧�ֵı��ʽ",box.Font,foreBrush,strRect,strFormat); 
								g.DrawString(string.Empty,box.Font,foreBrush,strRect,strFormat); 
								break;
							}
							val = DIYReport.Express.ExStatistical.GetStatisticalValue(beginIndex,endIndex -1,box.DataSource,box.BingDBFieldName,_Rows );
							if(val==null)
								val = " ";
							double dval = DIYReport.PublicFun.ToDouble(val); 
							if(box.FormatStyle!=null && box.FormatStyle!=""){
								try{
									//val = String.Format(box.FormatStyle,dval);
									val = DIYReport.PublicFun.FormatString(box.FormatStyle.Trim(),dval);
								}
								catch{}
							}
							g.DrawString(val,box.Font,foreBrush,strRect,strFormat); 
							break;
						case ReportModel.ExpressType.SysParam  : //ϵͳ����
							Type clsType = System.Type.GetType("DIYReport.Express.ExSpecial");
							MethodInfo meth = clsType.GetMethod(box.DataSource);
							if(meth!=null){
								object speTxt =meth.Invoke(clsType,null);
								val = speTxt.ToString();
							}
							g.DrawString(val,box.Font ,foreBrush,strRect ,strFormat);  
							break;
						case ReportModel.ExpressType.UserParam ://�û��ⲿ����
							DIYReport.ReportModel.RptParam param = DIYReport.UserDIY.DesignEnviroment.CurrentReport.UserParamList[ box.DataSource];
							if(param!=null){
								g.DrawString(param.Value.ToString() ,box.Font ,foreBrush,strRect,strFormat);  
							}
							break;
						case ReportModel.ExpressType.Field ://�û����ֶ�
//							string str = "";
//							if(pDRow!=null)   
//								str = pDRow[box.DataSource].ToString();
//							g.DrawString(str,box.Font ,foreBrush,RealRectangle(box) ,strFormat);  
							DrawField(g,pDRow,box,foreBrush,strRect,strFormat);
							break;
						default:
							break;
					}
				}
			}
			if(textObj.ShowFrame == true)
				drawRect(g,pObj);
		}
		#region Protectd virtual ����...
		/// <summary>
		/// ���û��󶨵��ֶ�
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pDRow"></param>
		/// <param name="pBox"></param>
		/// <param name="pBrush"></param>
		/// <param name="pStrFormat"></param>
		protected virtual void DrawField(Graphics g,DataRow pDRow,DIYReport.ReportModel.RptObj.RptExpressBox pBox,
										SolidBrush pBrush, RectangleF pStrRect,StringFormat pStrFormat){
			string str = "";
			if(pDRow!=null){
				if(pDRow.Table.Columns.Contains(pBox.BingDBFieldName) && pDRow[pBox.BingDBFieldName]!= System.DBNull.Value ){
					if(pBox.FormatStyle!=null && pBox.FormatStyle.Trim()!=""){
						try{
							//str = String.Format(pBox.FormatStyle.Trim(),pDRow[pBox.BingDBFieldName]); 
							str = DIYReport.PublicFun.FormatString(pBox.FormatStyle.Trim(),pDRow[pBox.BingDBFieldName]);
						}
						catch{}
					}
					else{
						str = pDRow[pBox.BingDBFieldName].ToString();
					}
					
				}
			}
			g.DrawString(str,pBox.Font ,pBrush,pStrRect,pStrFormat);  	
		}
		#endregion Protectd virtual ����...
		//��Image ͼ��
		private void drawImage(Graphics g,DIYReport.Interface.IRptSingleObj pObj) {
			DIYReport.ReportModel.RptObj.RptPictureBox pic = pObj as DIYReport.ReportModel.RptObj.RptPictureBox;
			RectangleF rct = RealRectangle(pObj);
			if(pic.Image!=null)
				g.DrawImage(pic.Image,rct.Left,rct.Top,rct.Width,rct.Height); 
		}

		//������
		private void drawLine(Graphics g,DIYReport.Interface.IRptSingleObj pObj) {
			RectangleF rct = RealRectangle(pObj);
			SolidBrush brush = new SolidBrush(pObj.ForeColor);
			Pen pen = new Pen(brush,pObj.LinePound); 
			pen.DashStyle = pObj.LineStyle ;
			
			DIYReport.ReportModel.RptObj.RptLine obj = pObj as  DIYReport.ReportModel.RptObj.RptLine;

			PointF p1 = new PointF(rct.Left ,rct.Top);
			PointF p2 = new PointF(pObj.Size.Width,pObj.Size.Height);

			switch(obj.LineType){
				case DIYReport.ReportModel.LineType.Horizon://ˮƽ��
					p2 = new PointF(rct.Left + obj.Size.Width,rct.Top);
					break;
				case DIYReport.ReportModel.LineType.Vertical ://��ֱ��
					p2 = new PointF(rct.Left,obj.Size.Height + rct.Top);
					break;
				case DIYReport.ReportModel.LineType.Bias:// б��
					p2 = new PointF(rct.Left + obj.Size.Width,rct.Top + obj.Size.Height);
					break;
				case DIYReport.ReportModel.LineType.Backlash://�� б��
					p1= new PointF(rct.Left,rct.Top + obj.Size.Height);
					p2 =  new PointF(rct.Left + obj.Size.Width,rct.Top);
					break;
				default:
					Debug.Assert(false,"�����ͻ�û�����ã�","");
					break;
			}
			g.DrawLine(pen,p1.X,p1.Y  ,p2.X ,p2.Y ); 
		}
		//���߿�
		private void drawRect(Graphics g,DIYReport.Interface.IRptSingleObj pObj) {
			RectangleF rct = RealRectangle(pObj);
			SolidBrush brush = new SolidBrush(pObj.ForeColor);
			Pen pen = new Pen(brush,pObj.LinePound); 
			pen.DashStyle = pObj.LineStyle ;
			g.DrawRectangle(pen,rct.Left-1,rct.Top-3,rct.Width,rct.Height); 
		}
		//�õ������ļ���ҳ����������λ�� XML���Ƶ�Y���� + Section �� Height
		protected virtual RectangleF RealRectangle(DIYReport.Interface.IRptSingleObj  pRptObj) {
			RectangleF drawRct;
			RectangleF oldRct = pRptObj.Rect;
			int marginLeft = _DocMargin.Left ,marginTop = _DocMargin.Top ;
			//�������������ÿ�ű���ֻ����һ��title ������detail �ĸ߶�Ҫ��������ſ��ԡ�
			switch(pRptObj.Section.SectionType  ) {
				case DIYReport.SectionType.ReportTitle:

					drawRct = new RectangleF( marginLeft + oldRct.X,marginTop + oldRct.Y ,oldRct.Width,oldRct.Height); 
					return drawRct;
				case DIYReport.SectionType.PageHead:
					 
					drawRct = new RectangleF(marginLeft + oldRct.X , marginTop + oldRct.Y + _RptInfo.TitleHeight,
						oldRct.Width,oldRct.Height);
					return drawRct;
				case DIYReport.SectionType.GroupHead:
				case DIYReport.SectionType.Detail:
				case DIYReport.SectionType.GroupFooter:
					drawRct = new RectangleF(marginLeft + oldRct.X ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
						_RptInfo.PageHeadHeight + _HasDrawDetailHeight , 
						oldRct.Width,oldRct.Height);
					return drawRct;
				case DIYReport.SectionType.PageFooter:
					drawRct = new RectangleF(marginLeft + oldRct.X ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
						_RptInfo.PageHeadHeight  + REAL_DETAIL_HEIGHT  ,
						oldRct.Width,oldRct.Height);
					return drawRct;
				case DIYReport.SectionType.ReportBottom :
					float rHeight;
					if(_IsLastPage && _RowPoint ==0)
						rHeight =  _RptInfo.BottomHeight;
					else
						rHeight = _HasDrawDetailHeight;
					drawRct = new RectangleF(marginLeft + oldRct.X  ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
						_RptInfo.PageHeadHeight  + rHeight,
						oldRct.Width,oldRct.Height);
					return drawRct;
			}
			return oldRct;
		}
		// 
		#endregion �ڲ����� �õ���������������λ�� �ͻ�����Ķ���...
		
		#region Public ����...
		public int HasDrawRowCount {
			get {
				return _HasDrawRowCount;
			}
			set {
				_HasDrawRowCount = value;
			}
		}
		public int PageNumber {
			get {
				return _PageNumber;
			}
			set {
				_PageNumber = value;
				DIYReport.Express.ExSpecial._Page = _PageNumber;   
				DIYReport.Express.ExSpecial._PageCount = _PageNumber;
			}
		}
		#endregion Public ����...
		

	}
}
