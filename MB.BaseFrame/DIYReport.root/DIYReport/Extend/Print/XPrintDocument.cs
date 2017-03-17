//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-10
// Description	:	XPrintDocument ����XML�ͼ�¼�����ƶ�Ӧ��ӡ�ĵ�
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
using System.ComponentModel; 

using DIYReport.ReportModel;
using DIYReport.Express ; 
using DIYReport.Print;

using DIYReport.ReportModel.RptObj;
namespace DIYReport.Extend.Print{
	/// <summary>
	/// XPrintDocument ����XML�ͼ�¼�����ƶ�Ӧ��ӡ�ĵ�
	/// </summary>
	public class XPrintDocument : DIYReport.Interface.IDrawReport{ 

		#region ��������...
		//�õ�ҳ����Ҫ������ֶ�
		private DIYReport.Express.ExpressValueList   _FooterExpress;
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
		//private bool _IsLastPage = false;
		private System.Drawing.Printing.PaperSize _DocSize;
		private System.Drawing.Printing.Margins _DocMargin ;

		private DIYReport.ReportModel.RptReport _DataReport;
		private DIYReport.Print.ReportDrawInfo _RptInfo;
		private DIYReport.Print.DrawDetailInfo _DrawDetailInfo;

		private DataRow[] _Rows;
		//�ڵ�ǰҳ���Ѿ����Ƶĸ߶�
		//private int _HasDrawDetailHeight = 0;
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

		//�̶��߶ȺͿ�� Ϊ0����-1 ��ʾ������,�������ֽ�������
		private Size _FixedPageSize;
		private int _CurentDrawLeft = 0;//��ǰ���Ƶ������ left ���� ( ����� SplitCount  �������)
		private int _FirstDrawMarginLeft = 0;// ��ǰ���Ʊ������ LEFT (��ʼλ�õ�����ߣ���Ҫ�Ǽ�¼�����ӱ���ʱ���¼���ƵĿ�ʼλ�á�)
		private int _PageDataSplitCount = 1;//����report �� ReportDataWidth �͵�ǰ��pageSize ȷ������ҳ���ϻ��Ƶĸ�����

		private const string MY_INDEX = "@MyIndex";
		private int _MyIndex = 0;
		private Hashtable _CustomExpression;//������ϡ�����洢��ʱ���͵ı��ʽ��
		#endregion ��������...

		#region Public ����...
		public DIYReport.ReportModel.RptReport DataReport{
			get{
				return _DataReport;
			}
		}
		/// <summary>
		/// ��ǰ���Ʊ����ж���Ŀ�ʼλ�á�
		/// </summary>
		[Browsable(false)] 
		public int FirstDrawMarginLeft {
			get{
				return _FirstDrawMarginLeft;
			}
			set{
				_FirstDrawMarginLeft = value;
			}
		}
        [Browsable(false)] 
        public int CurentDrawLeft {
            get {
                return _CurentDrawLeft;
            }
            set {
                _CurentDrawLeft = value;
            }
        }
        public int PageDataSplitCount {
            get {
                return _PageDataSplitCount;
            }
            set {
                _PageDataSplitCount = value;
            }
        }
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

		public XPrintDocument(DIYReport.ReportModel.RptReport pDataReport) {
			_FixedPageSize = new Size(-1,-1);

			_CustomExpression = new Hashtable();
			
			//���м����е����пؼ�����
			pDataReport.SortCtlByTop(); 
 
			_RptData = pDataReport.DataSource;
			//DIYReport.UserDIY.DesignEnviroment.DataSource =  pDataReport.DataSource ;
			 
 
			_DataReport = pDataReport;
	 
			_Rows = DIYReport.GroupAndSort.GroupDataProcess.SortData(pDataReport.DataSource,pDataReport);   

			_RptInfo = new ReportDrawInfo(pDataReport);
			 
			//edit by  nick 2007-10-17(û������ʲô�ô�)
			//			_FooterExpress = DIYReport.Express.ExpressValueList.GetFooterExpress(pDataReport);
			//			_BottomExpress = DIYReport.Express.ExpressValueList.GetBottomExpress(pDataReport) ;
			
			_DocSize = _DataReport.PaperSize;

			_DocMargin = _DataReport.Margins ;

			//��ʼ��ҳҳ��
			DIYReport.Express.ExSpecial._Page = 1;   
			DIYReport.Express.ExSpecial._PageCount = 1;

			_DrawDetailInfo = new DrawDetailInfo(_DataReport);
			_GroupFoots = new Stack();
			_GroupHeads = new Queue();

			float mergeHeight = _DataReport.SectionList.GetExceptDetailHeight() ;
			int rHeight = _DataReport.IsLandscape? _DocSize.Width : _DocSize.Height;
			//REAL_DETAIL_HEIGHT = rHeight  - Convert.ToInt32(mergeHeight) - _DocMargin.Top - _DocMargin.Bottom ;
			REAL_DETAIL_HEIGHT = rHeight  -  _DocMargin.Top - _DocMargin.Bottom ;
			int rWidth = _DataReport.IsLandscape? _DocSize.Height : _DocSize.Width;
			REAL_PAGE_WIDTH = rWidth - _DocMargin.Left - _DocMargin.Right;
			DIYReport.Express.ExSpecial._RowOrderNO = 0;
			
			_ImageList = new ArrayList(); 
			if(_DataReport.ReportDataWidth > 0)
				_PageDataSplitCount = rWidth  /  _DataReport.ReportDataWidth;
			
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
					//					try{
					//						foreach(Image img in _ImageList)
					//							img.Dispose();
					//					}
					//					catch{}
				}
			}
			disposed = true;         
		}
		~XPrintDocument() {
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
		public bool DrawReportSection(object graphicsObject,DIYReport.SectionType sectionType){
			DevExpress.XtraPrinting.BrickGraphics  g = graphicsObject as DevExpress.XtraPrinting.BrickGraphics;
 
			DIYReport.ReportModel.RptSection section = _DataReport.SectionList.GetSectionByType( sectionType);  
			if(section==null)
				return false;
			//int hasDrawHeight = 0;
			if(section.Visibled){
				//�����Section �ڻ� Detail ��ʱ����
				if(section!=null  && sectionType!=DIYReport.SectionType.GroupHead && sectionType!=DIYReport.SectionType.GroupFooter ) {
					if(sectionType==DIYReport.SectionType.Detail ){
						DrawDetailSection(g,section,0,REAL_DETAIL_HEIGHT);
					}
                    else if (sectionType == SectionType.PageFooter) {

                    }
                    else {
                        int height = 0;
                        for (int index = 0; index < _PageDataSplitCount; index++) {
                            _CurentDrawLeft = _DataReport.ReportDataWidth * index;

                            foreach (DIYReport.ReportModel.RptSingleObj obj in section.RptObjList) {
                                DrawRptSimpleObj(g, obj, height);
                            }
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
		public int DrawDetailSection(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.ReportModel.RptSection rptSection,
			int hasDrawHeight,int realDetailHeight) {
			if(_Rows==null || rptSection==null || rptSection.Height == 0 || rptSection.RptObjList.Count == 0 ) {
				return 0;
			}
			int hasDrawDetailHeight = hasDrawHeight;
			int currentDrawSplitCount = 0;
			int beforeDrawFirstSplitHeight = hasDrawHeight;
			_HasDrawRowCount = 0;
			if(_Rows.Length ==0){
				foreach(RptSingleObj  obj in rptSection.RptObjList) {
					if(obj.Type == RptObjType.RichTextBox)//edit by nick 2008-08-07
						drawRichTextBox(g,null,obj,ref  hasDrawDetailHeight);
					else
						DrawRptSimpleObj(g,obj,hasDrawDetailHeight);
				}
				hasDrawDetailHeight += rptSection.Height;
			}
			else{
				for (int i = 0; i < _Rows.Length; i++) {
					if(_PageDataSplitCount > 1){
						if(currentDrawSplitCount > _PageDataSplitCount - 1)
							currentDrawSplitCount = 0;
						if(currentDrawSplitCount==0){
							beforeDrawFirstSplitHeight = hasDrawDetailHeight;
							_CurentDrawLeft = 0;
						}
						else{
							hasDrawDetailHeight = beforeDrawFirstSplitHeight;
							_CurentDrawLeft = _DataReport.ReportDataWidth * currentDrawSplitCount  ;
						}
						currentDrawSplitCount +=1;
					}

					analyseGroupSection(i);
					if(i==0){
						//����������ڲ�����Ҫ������ô�ʹ�������Ͳ�����
						drawGroupHead(g,i,ref hasDrawDetailHeight);
					}
					else{
						//����������ڲ�����Ҫ������ô�ʹ�������Ͳ�����
						drawGroupFoot(g,i - 1,ref hasDrawDetailHeight);
						drawGroupHead(g,i,ref hasDrawDetailHeight);
					}

					DIYReport.Express.ExSpecial._RowOrderNO ++;
					int forSubHasDrawHeight = hasDrawDetailHeight;
					foreach(RptSingleObj  obj in rptSection.RptObjList) {
						if(obj.Type == RptObjType.SubReport){
							drawSubReport(g,_Rows[i],obj,ref forSubHasDrawHeight);
							hasDrawDetailHeight = forSubHasDrawHeight;
						}
						else if(obj.Type == RptObjType.RichTextBox)  //edit by nick 2008-08-07
							drawRichTextBox(g,_Rows[i],obj,ref  hasDrawDetailHeight);
						else{
							DrawRptSimpleObj(g,obj,_Rows[i],hasDrawDetailHeight);
						}
					}
					hasDrawDetailHeight += rptSection.Height;
					_HasDrawRowCount +=1;
				}

				//����������ڲ�����Ҫ������ô�ʹ�������Ͳ�����
				analyseGroupSection(_Rows.Length);
				drawGroupFoot(g,_Rows.Length - 1,ref hasDrawDetailHeight);

				//_BottomRow = _RowPoint;
				if(_DataReport.FillNULLRow){
					//��ʱע�� �� nick 2006-04-21 
					//				int remain = realDetailHeight - (hasDrawDetailHeight %  realDetailHeight) - _DataReport.SectionList.GetExceptDetailHeight();
					//				int mergeCount = remain / pSection.Height;
					//				 mergeCount -=2;
					//				//�ж��Ƿ���ʣ��Ŀռ䣬������ 
					//				for(int j =0; j< mergeCount ;j++) {
					//					foreach(RptSingleObj  obj in pSection.RptObjList) {
					//						drawRect(g,obj,hasDrawDetailHeight);
					//					}
					//					hasDrawDetailHeight += pSection.Height;
					//				}
				}
			}
			return hasDrawDetailHeight;
		}
		#endregion ���������ϸ��Ϣ...

		#region ����Group Section ��ͷ�ͽ���Ϣ...
		//�����ֶε�Group Section��Ϣ�����洢�ڶ�Ӧ�Ķ��к�ջ��
		private void analyseGroupSection(int curRowIndex){
			int rowCount = _Rows.Length;
			IList fields =  _DrawDetailInfo.GroupFields ;
			bool isEnd = false;
			if(fields!=null && fields.Count >0){
				int count = fields.Count;
				int drawFoot = -1;
				int iniGroupIndex = 0;
				if(curRowIndex < rowCount){
					DataRow dRow = _Rows[curRowIndex];
					bool mustDrawHead = false;
					for(int i = iniGroupIndex  ; i <count; i++){
						DrawGroupField groupField = fields[i] as DrawGroupField;
						string fieldName = groupField.GroupFieldName;
						object val = dRow[fieldName];
						DIYReport.ReportModel.RptSection section = _DataReport.SectionList.GetByGroupField(fieldName,true); 
						//�ж��е�ֵ���Ѿ����Ƶ�grou p section head �����ڷ������ֶε�ֵ�Ƿ���ͬ
						bool inTheGroup = !mustDrawHead && DIYReport.GroupAndSort.GroupDataProcess.ValueInTheGroup(section, groupField.CurrGroupValue,val);  
						groupField.CurrGroupValue = val;
						if(!inTheGroup && !mustDrawHead ){
							drawFoot = i;
							mustDrawHead = true;
						}
						if(!groupField.HasDrawGroupHead || mustDrawHead){
							//����Ҫ���Ƶ�group section ��head ��ӵ����еĽ�β
							groupField.PrevFirstRowIndex   = groupField.FirstRowIndex==-1?0:groupField.FirstRowIndex;
							groupField.FirstRowIndex   = curRowIndex;
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
		private void drawGroupHead(DevExpress.XtraPrinting.BrickGraphics  g,int currRowIndex,ref int hasDrawHeight){

			if(_GroupHeads.Count > 0){
				int count = _GroupHeads.Count;
				//_HasDrawDetailHeight
				for(int i = 0; i < count; i ++){
					DIYReport.ReportModel.RptSection section = _GroupHeads.Dequeue() as DIYReport.ReportModel.RptSection;
					if(section.Visibled){					 
						DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(section.GroupField.FieldName);
   
						groupField.HasDrawGroupHead = true;
						foreach(RptSingleObj  obj in section.RptObjList) {
							DrawRptSimpleObj(g,obj,_Rows[currRowIndex], hasDrawHeight);
						}
						hasDrawHeight +=section.Height;

						if(count > 0 && i == count -1){
							DIYReport.Express.ExSpecial._RowOrderNO = 0 ;  
						}
					}
				}
			}
		}
		//�����ֶη���Ľ�
		//���� true ��ʾ��û�н���
		private void drawGroupFoot(DevExpress.XtraPrinting.BrickGraphics  g,int currRowIndex,ref int hasDrawHeight){
			if(_GroupFoots.Count > 0){
				int count = _GroupFoots.Count;
				//_HasDrawDetailHeight
				for(int i = 0; i < count; i ++){
					DIYReport.ReportModel.RptSection section = _GroupFoots.Peek() as DIYReport.ReportModel.RptSection;
					if(section.Visibled){
						int tempHeight = hasDrawHeight + section.Height ;
						section = _GroupFoots.Pop() as DIYReport.ReportModel.RptSection;
						DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(section.GroupField.FieldName);
   
						groupField.HasDrawGroupHead = false;
						DIYReport.Express.ExSpecial._RowOrderNO = 0;
						foreach(RptSingleObj  obj in section.RptObjList) {
							DrawRptSimpleObj(g,obj,_Rows[currRowIndex], hasDrawHeight);
						}
						hasDrawHeight +=section.Height;
					}
				}
			}
		}
		//		//������ҳʣ�µķ���ͷ���߽�
		//		private bool drawLeaveGroupInfo(DevExpress.XtraPrinting.BrickGraphics  g){
		//			bool b = drawGroupHead(g);
		//			if(b){
		//				return true;
		//			}
		//			b = drawGroupFoot(g);
		//			return b;
		//		}
		#endregion ����Group Section ��ͷ�ͽ���Ϣ...

		#region �ڲ����� �õ���������������λ�� �ͻ�����Ķ���...
		//����Ƿ���Ի��Ƶ�ǰ�Ķ���
		private bool checkDrawValid(DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight){
			if(_FixedPageSize.Width > 0){
				if(pObj.Rect.Width + pObj.Rect.X > _FixedPageSize.Width)
					return false;
			}
			if(_FixedPageSize.Height > 0){
				if(pObj.Rect.Height + hasDrawHeight > _FixedPageSize.Height)
					return false;
			}
			return true;
		}
		//		private bool checkDrawValidHeight(RptSingleObj  pObj,int hasDrawHeight){
		//			if(_FixedPageSize.Height > 0){
		//				if(pObj.Rect.Height + hasDrawHeight > _FixedPageSize.Height)
		//					return false;
		//			}
		//			return true;
		//		}
		//		private bool checkDrawValidWidth(RptSingleObj  pObj){
		//			if(_FixedPageSize.Width > 0){
		//				if(pObj.Rect.Width + pObj.Rect.X > _FixedPageSize.Width)
		//					return false;
		//			}
		//			return true;
		//		}
		//������ÿһ������
		private void DrawRptSimpleObj(DevExpress.XtraPrinting.BrickGraphics  g,RptSingleObj  pObj,int hasDrawHeight) {
			if(_Rows==null || _Rows.Length ==0)
				DrawRptSimpleObj(g,pObj,null,hasDrawHeight);
			else
				DrawRptSimpleObj(g,pObj,_Rows[0],hasDrawHeight); //�ѵ�һ����Ϊ���Ʊ�ͷ�Ļ������ݡ�
		}
		private void DrawRptSimpleObj(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj  rptObj,DataRow dataRow,int hasDrawHeight) {
			if(!checkDrawValid(rptObj,hasDrawHeight))
				return;
			switch(rptObj.Type) {
				case RptObjType.Text :
				case RptObjType.Express :
				case RptObjType.FieldTextBox :
					drawTextObj(g,rptObj,dataRow,hasDrawHeight);
					break;
				case RptObjType.Line :
					drawLine(g,rptObj,hasDrawHeight);
					break;
				case RptObjType.Rect :
					drawRect(g,rptObj,hasDrawHeight);
					break;
				case RptObjType.Image :
					drawImage(g,rptObj,hasDrawHeight); 
					break;
				case RptObjType.CheckBox :
					drawCheckBox(g,dataRow,rptObj,hasDrawHeight); 
					break;
				case RptObjType.FieldImage :
					drawFieldImage(g,dataRow, rptObj,hasDrawHeight);
					break;
				case RptObjType.SubReport :
					DIYReport.TrackEx.Write("�п��ܴ����ڷ�detail section �д����ӱ�,Ŀǰ����֧����һ���ܡ�������ȷ��ʾ�ӱ�����ݡ�");
					break;
				case RptObjType.BarCode:
					drawBarCode(g,dataRow,rptObj,hasDrawHeight);
					break;
				case RptObjType.RichTextBox :
					drawRichTextBox(g,dataRow,rptObj,ref hasDrawHeight);
					break;
				case RptObjType.HViewSpecField :
					drawHViewSpecField(g,dataRow,rptObj,hasDrawHeight);
					break;
				default:
					Debug.Assert(false,"��ȷ�ϱ����XML�ļ������Ƿ���ȷ",
						"�ڱ�����һ�����" + rptObj.Name + " �����Ͳ������ó�:" + rptObj.Type.ToString() +"����");
					break;
			}
		}
		//nick 2006-04-11 
		private StringFormat  formateDrawObj(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj  pObj){
			DIYReport.Interface.IRptTextObj textObj = pObj as DIYReport.Interface.IRptTextObj;
			DevExpress.XtraPrinting.BrickStringFormat   strXTraFormat = new DevExpress.XtraPrinting.BrickStringFormat();
			
			if(textObj.WordWrap==false) {
				strXTraFormat.Value.Trimming = StringTrimming.EllipsisCharacter;
				strXTraFormat.Value.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;
			}
			else {
				strXTraFormat.Value.FormatFlags = StringFormatFlags.LineLimit;
			}
			strXTraFormat.Value.Alignment = textObj.Alignment;  
			
			g.StringFormat = strXTraFormat;
			g.BackColor = textObj.BackgroundColor ;
			g.ForeColor = textObj.ForeColor ;
			g.Font = textObj.Font;
			
			return strXTraFormat.Value;
		}
		//���ı�����
		private void drawTextObj(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj  pObj,DataRow pDRow,
			int hasDrawHeight){
			DIYReport.Interface.IRptTextObj textObj = pObj as DIYReport.Interface.IRptTextObj;
			//formateDrawObj(g,pObj);

			RectangleF strRect = RealRectangle(textObj,hasDrawHeight);

			//			SizeF fontSize = g.MeasureString();
			//			float fontFirstY = strRect.Y  +  (textObj.Size.Height - fontSize.Height)/2; 
			//			strRect.Y = fontFirstY;

			DevExpress.XtraPrinting.BorderSide borSide =  textObj.ShowFrame?DevExpress.XtraPrinting.BorderSide.All : DevExpress.XtraPrinting.BorderSide.None ; 
			
			if(textObj.Type == DIYReport.ReportModel.RptObjType.Text){
				DIYReport.ReportModel.RptObj.RptLable lab = pObj as DIYReport.ReportModel.RptObj.RptLable;
				if(lab.Text!=null){
					DrawString(g,lab.Text,textObj,strRect,borSide);    
				}
			}
			else if(textObj.Type == DIYReport.ReportModel.RptObjType.FieldTextBox){
				DrawField(g,pDRow,textObj as RptFieldTextBox,strRect,borSide);
			}
			else{
				//����󶨵Ĳ����ı������ֶΡ����ʽ����ϵͳ�����ȣ���ô����Ҫ�������н��Ͳ�������
				string val = "";
				DIYReport.ReportModel.RptObj.RptExpressBox box = pObj as DIYReport.ReportModel.RptObj.RptExpressBox;
				if(box.DataSource!=null && box.DataSource!="" && box.DataSource!="[δ��]"){
					switch(box.ExpressType ){
						case DIYReport.ReportModel.ExpressType.Express :
							int beginIndex= 0,endIndex = 0;
							if(box.Section.SectionType == DIYReport.SectionType.ReportBottom){ //ͳ�Ƶ��ֶ��ڱ���ţ���ô��ͳ�����е���
								beginIndex = 0;
								endIndex = _Rows.Length;
							}
							else if(box.Section.SectionType == DIYReport.SectionType.PageFooter){//ͳ�Ƶ��ֶ���ҳ�ţ���ô��ͳ�Ƶ�ǰҳ���е���
								beginIndex =  _HasDrawRowCount - _RowPoint;
								endIndex =   _HasDrawRowCount;
							}
							else if(box.Section.SectionType == DIYReport.SectionType.GroupFooter){//ͳ�Ƶ��ֶ��ڷ���Ľţ���ôͳ�Ʒ����漰������
								DIYReport.Print.DrawGroupField groupField = _DrawDetailInfo.GetGroupFieldByName(box.Section.GroupField.FieldName); 
								beginIndex = groupField.PrevFirstRowIndex;
								endIndex =  _HasDrawRowCount;
							}
							else{
								//g.DrawString("��֧�ֵı��ʽ",textObj.ForeColor,strRect,borSide); 
								DrawString(g,string.Empty,textObj,strRect,borSide); 
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
							//if(!_IsPrintCalculate){
							//�������дת��
							if(box.ToUpperMoney){
								decimal cVal = PublicFun.ToDecimal(dval,2); 
								val = box.ToUpperEnglish? Number2English.DefaultInstance.NumberToString(cVal) : Number2Chiness.ConvertToUpperMoney(cVal); 
							}
							//g.DrawString(val,box.Font,foreBrush,strRect,strFormat); 
							//}

							DrawString(g,val,textObj,strRect,borSide); 
							break;
						case DIYReport.ReportModel.ExpressType.SysParam  : //ϵͳ����
							if(string.Compare(box.DataSource,"RowOrderNO",true)==0){
								_MyIndex ++;
								DrawString(g,_MyIndex.ToString(),textObj,strRect,borSide); 
							}
							else if(string.Compare(box.DataSource,"PageInfo",true)==0){
								string pageFormat = box.FormatStyle;
								if(pageFormat==null || pageFormat.Length ==0)
									pageFormat = "{0}/{1}";
                                //strRect.Y += 1;
								DevExpress.XtraPrinting.PageInfoBrick pBrick = g.DrawPageInfo(DevExpress.XtraPrinting.PageInfo.NumberOfTotal,
                                                                               pageFormat,textObj.ForeColor,strRect,borSide);
                                pBrick.Font = textObj.Font;
								//nick 2006-09-20 ��ʱ����ҳ�������⡣
								switch(box.Alignment){
									case System.Drawing.StringAlignment.Near: 
										pBrick.Alignment = DevExpress.XtraPrinting.BrickAlignment.Near;   
										break;
									case System.Drawing.StringAlignment.Center: 
										pBrick.Alignment = DevExpress.XtraPrinting.BrickAlignment.Center;   
										break;
									case System.Drawing.StringAlignment.Far: 
										pBrick.Alignment = DevExpress.XtraPrinting.BrickAlignment.Far;   
										break;
									default:
										pBrick.Alignment = DevExpress.XtraPrinting.BrickAlignment.Near;  
										break;
								}	
								pBrick.AutoWidth = true;
							}
							else{
								Type clsType = System.Type.GetType("DIYReport.Express.ExSpecial");
								MethodInfo meth = clsType.GetMethod(box.DataSource);
								if(meth!=null){
									object speTxt =meth.Invoke(clsType,null);
									val = speTxt.ToString();
								}
								DrawString(g,val,textObj,strRect,borSide);     
							}
							break;
						case DIYReport.ReportModel.ExpressType.UserParam ://�û��ⲿ����
							DIYReport.ReportModel.RptParam param = DIYReport.UserDIY.DesignEnviroment.CurrentReport.UserParamList[ box.DataSource];
							if(param!=null){
								//���ֵ������Ϊbyte ���ͣ���ô��������ͼ��������
								if(param!=null && param.Value!=System.DBNull.Value && param.Value.GetType().Name.Equals("Byte[]")) {
									System.Drawing.Image img = DIYReport.PublicFun.ByteToImage((byte[])param.Value);
									if(img!=null){
										DevExpress.XtraPrinting.ImageBrick imgBrick = g.DrawImage(img,strRect,borSide,textObj.BackgroundColor); 
										imgBrick.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
									}
								}
								else{
									DrawString(g,param.Value.ToString(),textObj,strRect,borSide);  
								}
							}
							break;
						case DIYReport.ReportModel.ExpressType.Calculate ://���ʽ����
							DrawCalcute(g,pDRow,textObj as RptExpressBox,strRect,borSide);
							break;
							//						case DIYReport.ReportModel.ExpressType.Field ://�û����ֶ�
							//							//							string str = "";
							//							//							if(pDRow!=null)   
							//							//								str = pDRow[box.DataSource].ToString();
							//							//							g.DrawString(str,box.Font ,foreBrush,RealRectangle(box) ,strFormat);  
							//							DrawField(g,pDRow,box,strRect,borSide);
							//							break;
						default:
							break;
					}
				}
			}

		}
		#region Protectd virtual ����...
		protected virtual DevExpress.XtraPrinting.TextBrick DrawString(DevExpress.XtraPrinting.BrickGraphics  g,string text,DIYReport.Interface.IRptTextObj  pBox,
			RectangleF pStrRect,DevExpress.XtraPrinting.BorderSide borSide){

			StringFormat sFormt = formateDrawObj(g,pBox);

			DevExpress.XtraPrinting.TextBrick txtBrick = null;
			//�Ȼ��Ʊ߿�
			g.DrawRect( pStrRect,borSide,pBox.BackgroundColor ,pBox.ForeColor);
			//���¾��л�������
			string txt = text.Trim();
			if(txt.Length > 0){
				//StringFormat sForMat = new StringFormat(
				System.Drawing.SizeF textSize = g.MeasureString(txt,pBox.Rect.Width,sFormt);
                textSize.Height += 1;
				RectangleF rect = getDrawMiddleJustifyRect(pStrRect,textSize);
                //
				txtBrick = g.DrawString(txt,pBox.ForeColor,rect,DevExpress.XtraPrinting.BorderSide.None); 
			}

			return txtBrick;
		}
		//��ȡ���ƾ��еı߿�
		private RectangleF getDrawMiddleJustifyRect(RectangleF clientRectangle,System.Drawing.SizeF textSize){
			RectangleF rect = new RectangleF(clientRectangle.X + 1, clientRectangle.Y + (clientRectangle.Height - textSize.Height)/2,
				clientRectangle.Width - 2,clientRectangle.Height  - (clientRectangle.Height - textSize.Height)/2 -1);
			return rect;
		}
		/// <summary>
		/// ���ʽ���㡣
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pDRow"></param>
		/// <param name="pBox"></param>
		/// <param name="pStrRect"></param>
		/// <param name="borSide"></param>
		protected virtual void DrawCalcute(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow,DIYReport.ReportModel.RptObj.RptExpressBox  pBox,
			RectangleF pStrRect,DevExpress.XtraPrinting.BorderSide borSide){

			if(pDRow==null || pBox.DataSource==null || pBox.DataSource.Length ==0){
				DrawString(g,string.Empty,pBox,pStrRect,borSide);
			}
			string expression = string.Empty;
			if(!_CustomExpression.ContainsKey(pBox.DataSource)){
				expression = MyCalculateDataRow.ReplaceCaptionAsFieldName(pDRow.Table.Columns, pBox.DataSource);
				_CustomExpression[pBox.DataSource] = expression;
			}
			else{
				expression = _CustomExpression[pBox.DataSource].ToString();
			}

			string val = MyCalculateDataRow.CalculateExpression(pDRow, expression);
			DrawString(g,val,pBox,pStrRect,borSide);
		}
		/// <summary>
		/// ���û��󶨵��ֶ�
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pDRow"></param>
		/// <param name="pBox"></param>
		/// <param name="pBrush"></param>
		/// <param name="pStrFormat"></param>
		protected virtual void DrawField(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow,
			DIYReport.ReportModel.RptObj.RptFieldTextBox pBox,
			RectangleF pStrRect,DevExpress.XtraPrinting.BorderSide borSide){

			if(pBox.IncludeMultiField){
				DrawMultiField(g,pDRow,pBox,pStrRect,borSide);
				return;
			}
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
			//�������дת��
			if(pBox.ToUpperMoney){
				try{
					decimal cVal = PublicFun.ToDecimal(str,2); 
					str = pBox.ToUpperEnglish? Number2English.DefaultInstance.NumberToString(cVal) : Number2Chiness.ConvertToUpperMoney(cVal); 
					//str = Number2Chiness.ConvertToUpperMoney(cVal); 
				}
				catch{}
			}
			
			DrawString(g,str,pBox,pStrRect,borSide);
			//g.DrawString(str,pBox.Font ,pBrush,pStrRect,pStrFormat);  	
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pDRow"></param>
		/// <param name="pBox"></param>
		/// <param name="pStrRect"></param>
		/// <param name="borSide"></param>
		protected virtual void DrawMultiField(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow,DIYReport.ReportModel.RptObj.RptFieldTextBox pBox,
			RectangleF pStrRect,DevExpress.XtraPrinting.BorderSide borSide){
			string str = string.Empty;
			System.Drawing.Font drawFont = pBox.Font;
			bool isMultiLine = false;
			int dataInMultiIndex = 0;
			if(pDRow!=null && pBox.FieldName!=null && pBox.FieldName.Length > 0){
				string fields = pBox.FieldName;
				//�ֶ�֮���÷ֺŸ
				string[] afiel = fields.Split(';');
				int fieldCount = afiel.Length;
				if(fieldCount==1){
					if(pDRow.Table.Columns.Contains(fields)){
						str = pDRow[fields].ToString().Trim();
					}
				}
				else{
					isMultiLine = true;
					for(int i = 0 ; i<fieldCount;i++){
						string colName = afiel[i];
						if(pDRow.Table.Columns.Contains(colName)){
							
							if(pDRow[colName]!=System.DBNull.Value && pDRow[colName].ToString().Trim().Length >0){
								if(str.Length > 0 ){
									str += "/" + pDRow[colName].ToString().Trim();// + getSplitChar(i); 
									dataInMultiIndex = 0;
								}
								else
									str = pDRow[colName].ToString().Trim();// + getSplitChar(i); 
								dataInMultiIndex = i ;
							}
						}

					}
					if(isMultiLine && str.Length  > 0 && str.IndexOf('/') < 0  && dataInMultiIndex % 2 !=0)
						drawFont = new Font(drawFont.FontFamily.Name,getFontSizeByLength(str.Length),System.Drawing.FontStyle.Underline); //������ڶ��ֵ�Ļ���ô��������С
					else
						drawFont = new Font(drawFont.FontFamily.Name,getFontSizeByLength(str.Length)); //������ڶ��ֵ�Ļ���ô��������С
					
				}
			}
			Font oldF = g.Font;
			g.Font = drawFont;
			DrawString(g,str,pBox,pStrRect,borSide);
			g.Font = oldF;
			//g.DrawString(str,drawFont,pBrush,pStrRect,pStrFormat);  	
		}
		private float getFontSizeByLength(int strLength){
			if(strLength <=3)
				return 9f;
			else if(strLength <= 5)
				return 8f;
			else if(strLength <= 7)
				return 7f;
			else if(strLength <=9)
				return 6f;
			else if(strLength <=11)
				return 5f;
			else if(strLength <=13)
				return 4f;
			else 
				return 3f;
		}
		private string getSplitChar(int index){
			if((index + 1) % 2 != 0)
				return "'";
			else 
				return ('"').ToString();
		}

		#endregion Protectd virtual ����...
		//��Image ͼ��
		private void drawImage(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight) {
			DIYReport.ReportModel.RptObj.RptPictureBox pic = pObj as DIYReport.ReportModel.RptObj.RptPictureBox;
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);
			if(pic.Image!=null){
				DevExpress.XtraPrinting.ImageBrick imgBrick = g.DrawImage(pic.Image,rct); 
				imgBrick.SizeMode = pic.DrawSizeModel;
			}
		}
		//�����ֶε�Image ͼ��
		private void drawFieldImage(DevExpress.XtraPrinting.BrickGraphics  g,DataRow drData, DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight) {
			DIYReport.ReportModel.RptObj.RptDBPictureBox pic = pObj as DIYReport.ReportModel.RptObj.RptDBPictureBox;
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);
			DevExpress.XtraPrinting.BorderSide borSide = pic.ShowFrame?DevExpress.XtraPrinting.BorderSide.All : DevExpress.XtraPrinting.BorderSide.None;
			
			System.Drawing.Image img = null;// 
			if(drData!=null && drData.Table.Columns.Contains(pic.BingDBFieldName) && drData[pic.BingDBFieldName]!=System.DBNull.Value ){
				object imgVal = drData[pic.BingDBFieldName];
				if(string.Compare(imgVal.GetType().Name,"Byte[]",true)==0)
					img = DIYReport.PublicFun.ByteToImage((byte[])drData[pic.BingDBFieldName]);
			}
			if(img!=null){
				DevExpress.XtraPrinting.ImageBrick imgBrick = g.DrawImage(img,rct,borSide,pic.BackgroundColor); 
				imgBrick.SizeMode = pic.DrawSizeModel;
				//img.Dispose();
				//_ImageList.Add(img);
			}
		}
		//������
		private void drawLine(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight) {
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);

			SolidBrush brush = new SolidBrush(pObj.ForeColor);
			Pen pen = new Pen(brush,pObj.LinePound); 
			pen.DashStyle = pObj.LineStyle ;
			
			DIYReport.ReportModel.RptObj.RptLine line = pObj as  DIYReport.ReportModel.RptObj.RptLine;

			PointF p1 = new PointF(rct.Left ,rct.Top);
			PointF p2 = new PointF(pObj.Size.Width,pObj.Size.Height);

			switch(line.LineType){
				case DIYReport.ReportModel.LineType.Horizon://ˮƽ��
					p2 = new PointF(rct.Left + line.Size.Width,rct.Top);
					break;
				case DIYReport.ReportModel.LineType.Vertical ://��ֱ��
					p2 = new PointF(rct.Left,line.Size.Height + rct.Top);
					break;
				case DIYReport.ReportModel.LineType.Bias:// б��
					p2 = new PointF(rct.Left + line.Size.Width,rct.Top + line.Size.Height);
					break;
				case DIYReport.ReportModel.LineType.Backlash://�� б��
					p1= new PointF(rct.Left,rct.Top + line.Size.Height);
					p2 =  new PointF(rct.Left + line.Size.Width,rct.Top);
					break;
				default:
					Debug.Assert(false,"�����ͻ�û�����ã�","");
					break;
			} 
			DevExpress.XtraPrinting.LineBrick  lineBrick = g.DrawLine(p1,p2,pObj.ForeColor,pObj.LinePound); 
			lineBrick.LineStyle = line.LineStyle ;
            
		}
		//���߿�
		private void drawRect(DevExpress.XtraPrinting.BrickGraphics  g,DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight) {
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);

			RectangleF dRect = new RectangleF(rct.Left,rct.Top,rct.Width,rct.Height);//RectangleF(rct.Left-1,rct.Top-3,rct.Width,rct.Height);
		 
			DevExpress.XtraPrinting.VisualBrick brick = g.DrawRect(dRect,DevExpress.XtraPrinting.BorderSide.All,pObj.BackgroundColor,pObj.ForeColor); 
			brick.BorderWidth =  pObj.LinePound;

		}
		//�õ������ļ���ҳ����������λ�� XML���Ƶ�Y���� + Section �� Height
		protected virtual RectangleF RealRectangle(DIYReport.Interface.IRptSingleObj  pRptObj,int hasDrawHeight){
			RectangleF oldRct = pRptObj.Rect;
			//2006-04-21 ���� ��������Ŀ�� _CurentDrawLeft
			oldRct.Width = (oldRct.Width + oldRct.X < _DataReport.ReportDataWidth || _DataReport.ReportDataWidth ==0)?oldRct.Width : _DataReport.ReportDataWidth - oldRct.X;
			RectangleF drawRct = new RectangleF(oldRct.X + _CurentDrawLeft + _FirstDrawMarginLeft ,oldRct.Y + hasDrawHeight,oldRct.Width,oldRct.Height);
			
			return drawRct;
		}
		//���ƶ�̬�������ֶΡ�
		private void drawHViewSpecField(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow, DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight){
			RptHViewSpecFieldBox exObj = pObj as RptHViewSpecFieldBox;
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);
			RectangleF rect = new RectangleF(rct.Left,rct.Top,rct.Width,rct.Height);
			drawRect(g,pObj,hasDrawHeight);
			int cellWidth = exObj.CellWidth;
			int width = exObj.Size.Width;
			int count = width / cellWidth;

			for(int i = 1 ; i < count; i++){
				DevExpress.XtraPrinting.LineBrick  lineBrick = g.DrawLine(new PointF(rect.X + cellWidth * i,rect.Y),new PointF(rect.X + cellWidth * i,rect.Y + rect.Height),pObj.ForeColor,pObj.LinePound); 
				lineBrick.LineStyle = exObj.LineStyle ;
			}
			if(_RptData==null) return;
			DataTable dtData =  PublicFun.GetDataTableByObject( _RptData );
			int hasDrawIndex = 0;
			foreach(DataColumn dc in dtData.Columns){
				if(hasDrawIndex > count + 1) break;
				switch(exObj.ActiveDataType){
					case ActiveDataType.Size:
						if(dc.ColumnName.IndexOf(RptHViewSpecFieldBox.SIZE_FIRST_PREX) >=0){
							string activeGroupField = exObj.GroupDisplayField;
							if(activeGroupField!=null && activeGroupField.Length > 0 && dtData.Columns.Contains(activeGroupField) ){
								string groupVal = string.Format(RptHViewSpecFieldBox.ACTIVE_GROUP_FIELD_IDENTITY,pDRow[exObj.GroupDisplayField].ToString());
								if(dc.ColumnName.IndexOf(groupVal) < 0)
									continue;
							}
							RectangleF txtRect = new RectangleF(rect.X + cellWidth * hasDrawIndex + 1,rect.Y + 1,cellWidth -2,rect.Height -2);
							if(exObj.DataViewArea == ActiveViewArea.CaptionArea)
								DrawString(g,dc.Caption,exObj,txtRect,DevExpress.XtraPrinting.BorderSide.None);
							else if(exObj.DataViewArea == ActiveViewArea.DetailArea){
								if(pDRow!=null)
									DrawString(g,pDRow[dc.ColumnName].ToString(),exObj,txtRect,DevExpress.XtraPrinting.BorderSide.None);
							}
							else if(exObj.DataViewArea == ActiveViewArea.SumArea){
								object cval = dtData.Compute("SUM([" + dc.ColumnName + "])",string.Empty); 
								DrawString(g,cval.ToString(),exObj,txtRect,DevExpress.XtraPrinting.BorderSide.None);
							}
							else{

							}
							hasDrawIndex ++;
						}
						break;
					case ActiveDataType.Color:
						break;
					case ActiveDataType.ShipPort:
						break;
					default:
						break;
				}
			}
		 
		}
		//��checkBox
		private void drawCheckBox(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow, DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight){
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);
			DIYReport.ReportModel.RptObj.RptCheckBox checkBox =  pObj as DIYReport.ReportModel.RptObj.RptCheckBox;

			DevExpress.XtraPrinting.BorderSide borSide = checkBox.ShowFrame?DevExpress.XtraPrinting.BorderSide.All : DevExpress.XtraPrinting.BorderSide.None;
			if(checkBox.BingField){
				if(pDRow==null || pDRow.Table.Columns.Contains(checkBox.BingDBFieldName))
					return;
				g.DrawCheckBox(rct,borSide,checkBox.BackgroundColor,DIYReport.PublicFun.ToBool(pDRow[checkBox.BingDBFieldName])); 
			}
			else{
				g.DrawCheckBox(rct,borSide,checkBox.BackgroundColor,checkBox.Checked); 
			}
		}
		//�� richTextBox ���ݡ�
		private void drawRichTextBox(DevExpress.XtraPrinting.BrickGraphics  g,DataRow drData, DIYReport.Interface.IRptSingleObj pObj,ref int hasDrawHeight){
            //RectangleF rct = RealRectangle(pObj,hasDrawHeight);
            //DIYReport.ReportModel.RptObj.RptRichTextBox richBox =  pObj as DIYReport.ReportModel.RptObj.RptRichTextBox;

            //DevExpress.XtraPrinting.BorderSide borSide = richBox.ShowFrame?DevExpress.XtraPrinting.BorderSide.All : DevExpress.XtraPrinting.BorderSide.None;
			
            //System.Drawing.Image img = null;// 
            //if(drData!=null && drData.Table.Columns.Contains(richBox.BingDBFieldName) && drData[richBox.BingDBFieldName]!=System.DBNull.Value ){
            //    object imgVal = drData[richBox.BingDBFieldName];
            //    string rtfString = string.Empty;
            //    if(string.Compare(imgVal.GetType().Name,"Byte[]",true)==0)
            //        rtfString = System.Text.Encoding.UTF8.GetString((byte[])imgVal);
            //    else
            //        rtfString = imgVal.ToString();

            //    System.Windows.Forms.RichTextBox richTextBox = new System.Windows.Forms.RichTextBox();
            //    richTextBox.Rtf = rtfString;
            //    int iheight = DevExpress.XtraReports.Native.RichEditHelper.MeasureRtf(DIYReport.Drawing.GraphicsDpi.Pixel,rtfString,rct,20);
            //    if(richBox.Font!=null)
            //        richTextBox.Font = richBox.Font;

            //    //
            //    int height = System.Convert.ToInt32(rct.Height);

            //    if(imgVal!=null && imgVal!=System.DBNull.Value){
            //        hasDrawHeight = hasDrawHeight - (height - iheight);
            //        height = iheight; 
            //    }
            //    rct.Height = height;
            //    img = DevExpress.XtraReports.Native.RichEditHelper.GetRtfImage(richTextBox,DIYReport.Drawing.GraphicsDpi.Pixel,new RectangleF(0,0,rct.Width ,rct.Height));
				

            //}
            //if(img!=null){
            //    //g.PageUnit = System.Drawing.GraphicsUnit.Inch ;
            //    DevExpress.XtraPrinting.ImageBrick imgBrick = g.DrawImage(img,rct,borSide,richBox.BackgroundColor); 
            //    //imgBrick.SizeMode = richBox.DrawSizeModel;  
            //    //img.Dispose();
            //    //_ImageList.Add(img);
            //}
            //else{
            //    //DevExpress.XtraPrinting.re
            //    hasDrawHeight = hasDrawHeight - (System.Convert.ToInt32(rct.Height) - 20);
            //    rct.Height = 20;
            //    DevExpress.XtraPrinting.VisualBrick brick = g.DrawRect(rct,DevExpress.XtraPrinting.BorderSide.All,pObj.BackgroundColor,pObj.ForeColor); 
            //    brick.BorderWidth =  pObj.LinePound;
            //}

		}
		//���ӱ��� �����fixedHeight ��ô hasDrawHeight ����Ҫ�ı䣬������Ҫ�ı䣬�ı�������ڵ�ǰ׷�ӵĸ߶ȼ�ȥ���ʱ���Ƶĸ߶ȡ�
		private void drawSubReport(DevExpress.XtraPrinting.BrickGraphics  g,DataRow parentDataRow, DIYReport.Interface.IRptSingleObj pObj,ref int hasDrawHeight){
			if(!checkDrawValid(pObj,hasDrawHeight))
				return;
			DIYReport.ReportModel.RptObj.RptSubReport rpt = pObj as DIYReport.ReportModel.RptObj.RptSubReport;

			if(rpt.ReportFileName==null || rpt.ReportFileName.Length == 0){
				//System.Windows.Forms.MessageBox.Show("�ӱ����������ò���Ϊ�գ������ӱ����ɹ������飡"); 
				DIYReport.TrackEx.Write("�ӱ����������ò���Ϊ�գ������ӱ����ɹ������飡");
				return ;
			}
			int currentDrawHeight = hasDrawHeight + rpt.Rect.Y;
			DIYReport.ReportModel.RptReport subReport = null;
			if(_DataReport.SubReportCommand!=null){
				subReport = _DataReport.SubReportCommand.GetReportContent(rpt.ReportFileName); 
			}
			else{
				subReport = _DataReport.SubReports[rpt.ReportFileName] as DIYReport.ReportModel.RptReport;
			}
			if(subReport==null)
				return;
			subReport.FillNULLRow = subReport.FillNULLRow && rpt.FixedHeight;
			if(_DataReport.SubReportCommand!=null){
                object dataSource = _DataReport.SubReportCommand.GetReportDataSource(parentDataRow, rpt.RelationMember, rpt.ReportFileName);
				subReport.DataSource = dataSource;
			}
			else{
				//��ȡ����������ӱ������ݡ�
				if(rpt.RelationMember==null || rpt.RelationMember.Length ==0){
					DIYReport.TrackEx.Write("�ӱ������������Դ���ò���Ϊ�գ������ӱ����ɹ������飡");
					//System.Windows.Forms.MessageBox.Show("�ӱ������������Դ���ò���Ϊ�գ������ӱ����ɹ������飡"); 
					return ;
				}
				DataRow[] drs = parentDataRow.GetChildRows(rpt.RelationMember,System.Data.DataRowVersion.Default); 
				DataTable newTable  = null;
				if(drs.Length > 0){
					newTable = drs[0].Table.Clone();
					foreach(DataRow dr in drs){
						newTable.Rows.Add(dr.ItemArray);
					}
				}
				subReport.DataSource = newTable;
			}
			//iniGroupFieldOnOpen(subReport);
			PublicFun.IniGroupFieldOnOpen(subReport); 

			subReport.ReportDataWidth = (subReport.ReportDataWidth + rpt.Rect.Left < _DataReport.ReportDataWidth || _DataReport.ReportDataWidth==0) ?
				subReport.ReportDataWidth : _DataReport.ReportDataWidth - rpt.Rect.Left;
			//DIYReport.ReportModel.RptReport rootReport = _DataReport;
			DIYReport.UserDIY.DesignEnviroment.CurrentReport = subReport;
			XPrintDocument xDoc = new XPrintDocument(subReport);
			xDoc.FirstDrawMarginLeft = rpt.Rect.Left ;

			Size fixSize = new Size(-1,-1); 
			if(rpt.FixedWidth) 
				fixSize.Width = (rpt.Rect.Width + rpt.Rect.Left < _DataReport.ReportDataWidth || _DataReport.ReportDataWidth ==0)?rpt.Rect.Width : _DataReport.ReportDataWidth - rpt.Rect.Left;
			if(rpt.FixedHeight)
				fixSize.Height = rpt.Rect.Height;
			xDoc.FixedPageSize = fixSize;
			foreach(DIYReport.ReportModel.RptSection section in subReport.SectionList){
				switch(section.SectionType){
					case DIYReport.SectionType.TopMargin:
					case DIYReport.SectionType.ReportTitle:
					case DIYReport.SectionType.PageHead:
					case DIYReport.SectionType.PageFooter:
					case DIYReport.SectionType.ReportBottom:
					case DIYReport.SectionType.BottomMargin:
                        for (int index = 0; index < xDoc.PageDataSplitCount; index++) {
                            xDoc.CurentDrawLeft = subReport.ReportDataWidth * index;
                            foreach (DIYReport.ReportModel.RptSingleObj obj in section.RptObjList) {

                                xDoc.DrawRptSimpleObj(g, obj, currentDrawHeight);
                            }
                        }
						currentDrawHeight +=section.Height;
						break;
					case DIYReport.SectionType.Detail:
						int realHeight = rpt.Rect.Height - subReport.SectionList.GetExceptDetailHeight(); 
						int re = xDoc.DrawDetailSection(g,section,currentDrawHeight,realHeight);
						currentDrawHeight = re;
						break;
					default:
						break;
				}
			}
			if(!rpt.FixedHeight){
				hasDrawHeight = currentDrawHeight  - rpt.Rect.Y - rpt.Rect.Height;
			}
			DIYReport.UserDIY.DesignEnviroment.CurrentReport = _DataReport;
			if(xDoc!=null)
				xDoc.Dispose(); 
		}
		//��������
		private void drawBarCode(DevExpress.XtraPrinting.BrickGraphics  g,DataRow pDRow, DIYReport.Interface.IRptSingleObj pObj,int hasDrawHeight){
			RectangleF rct = RealRectangle(pObj,hasDrawHeight);
			DIYReport.ReportModel.RptObj.RptBarCode barCode =  pObj as DIYReport.ReportModel.RptObj.RptBarCode;
			DevExpress.XtraPrinting.BorderSide borSide = barCode.ShowFrame?DevExpress.XtraPrinting.BorderSide.All : DevExpress.XtraPrinting.BorderSide.None;
			string code = string.Empty;
			if(barCode.DrawByBarCode){
				code = barCode.BarCode;
			}
			else{
				if(barCode.BingDBFieldName!=null && barCode.BingDBFieldName.Length >0){ 
					if(pDRow==null || pDRow.Table.Columns.Contains(barCode.BingDBFieldName))
						code = pDRow[barCode.BingDBFieldName].ToString();
				}
			}
			//DevExpress.XtraPrinting. Brick barBrick = new DevExpress.XtraPrinting.Brick();
			//barBrick.Draw(
			//g.DrawBrick( 
			DrawCodeBarCode.Instance().DrawBarCodeWithLib(g,barCode,code,Rectangle.Ceiling(rct));


		}
		//		protected virtual RectangleF RealRectangle(DIYReport.Interface.IRptSingleObj  pRptObj) {
		//			RectangleF drawRct;
		//			RectangleF oldRct = pRptObj.Rect;
		//			int marginLeft = _DocMargin.Left ,marginTop = _DocMargin.Top ;
		//			//�������������ÿ�ű���ֻ����һ��title ������detail �ĸ߶�Ҫ��������ſ��ԡ�
		//			switch(pRptObj.Section.SectionType  ) {
		//				case DIYReport.SectionType.ReportTitle:
		//
		//					drawRct = new RectangleF( marginLeft + oldRct.X,marginTop + oldRct.Y ,oldRct.Width,oldRct.Height); 
		//					return drawRct;
		//				case DIYReport.SectionType.PageHead:
		//					 
		//					drawRct = new RectangleF(marginLeft + oldRct.X , marginTop + oldRct.Y + _RptInfo.TitleHeight,
		//						oldRct.Width,oldRct.Height);
		//					return drawRct;
		//				case DIYReport.SectionType.GroupHead:
		//				case DIYReport.SectionType.Detail:
		//				case DIYReport.SectionType.GroupFooter:
		//					drawRct = new RectangleF(marginLeft + oldRct.X ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
		//						_RptInfo.PageHeadHeight + _HasDrawDetailHeight , 
		//						oldRct.Width,oldRct.Height);
		//					return drawRct;
		//				case DIYReport.SectionType.PageFooter:
		//					drawRct = new RectangleF(marginLeft + oldRct.X ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
		//						_RptInfo.PageHeadHeight  + REAL_DETAIL_HEIGHT  ,
		//						oldRct.Width,oldRct.Height);
		//					return drawRct;
		//				case DIYReport.SectionType.ReportBottom :
		//					float rHeight;
		//					if(_IsLastPage && _RowPoint ==0)
		//						rHeight =  _RptInfo.BottomHeight;
		//					else
		//						rHeight = _HasDrawDetailHeight;
		//					drawRct = new RectangleF(marginLeft + oldRct.X  ,marginTop + oldRct.Y + _RptInfo.TitleHeight +
		//						_RptInfo.PageHeadHeight  + rHeight,
		//						oldRct.Width,oldRct.Height);
		//					return drawRct;
		//			}
		//			return oldRct;
		//		}
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
		public Size FixedPageSize{
			get{
				return _FixedPageSize;
			}
			set{
				_FixedPageSize = value;
			}
		}
		#endregion Public ����...
		

	}
}
