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
using System.Drawing.Printing ;
using System.Data ;
using System.Collections;
using System.Diagnostics ;
using System.Configuration ;
using System.Runtime.InteropServices ;

using System.IO ;
using System.Runtime.Serialization.Formatters.Binary ;

namespace DIYReport {
	/// <summary>
	/// PublicFun �����Ĵ�������
	/// </summary>
	public class PublicFun {
		#region ����ת�����...
		/// <summary>
		/// �õ����ε�����
		/// </summary>
		/// <param name="pData"></param>
		/// <returns></returns>
		public static int  ToInt(object pData){
			string  val = SetRound(pData,0);
			return int.Parse(val);
		}
		public static double ToDouble(object pVal) {
			if(pVal==null || pVal.ToString().Trim()=="") {
				return  0;
			}
			try {
				double val = double.Parse(pVal.ToString());
				return val;
			}
			catch {
				return  0;
			}
		}
		public static float ToFloat(object pVal) {
			if(pVal==null || pVal.ToString().Trim()=="") {
				return  0;
			}
			try {
				float val = float.Parse(pVal.ToString());
				return val;
			}
			catch {
				return  0;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pData"></param>
		/// <param name="pDesLength"></param>
		/// <returns></returns>
		public static decimal  ToDecimal(object pData,int pDesLength){
			string  val = SetRound(pData,pDesLength); 
			return decimal.Parse(val);
		}
		/// <summary>
		/// ������ת���� Decimal ,��ת��Ϊ���Ƶ�ָ������
		/// </summary>
		/// <param name="pData"></param>
		/// <param name="pDesLength"></param>
		/// <returns></returns>
		public static decimal  ToDecimal(object pData,int pDesLength,bool pDesIsFix){
			string  val = SetRound(pData,pDesLength); 
			if(pDesIsFix){
				int index = val.LastIndexOf('.');
				if(index < 0){
					val +=".";
					for(int i = 0 ; i < pDesLength  ; i++){
						val +="0";
					}
				}
				else{
					string des = val.Substring(index + 1,val.Length - index - 1); 
					for(int i = 0 ; i < pDesLength - des.Length ;i++){
						val +="0";
					}
				}
			}
			return decimal.Parse(val);
		}
		/// <summary>
		/// ת��Ϊbool ������
		/// </summary>
		/// <param name="pData"></param>
		/// <returns></returns>
		public static bool ToBool(object pData){
			if(pData==null || pData == System.DBNull.Value || pData.ToString()  == ""){
				return false;
			}
			if(pData.ToString().Trim() == "1" || pData.ToString().Trim().ToUpper() == "TRUE"){
				return true;
			}
			return false;
		}
		/// <summary>
		/// ����1111111.525 * 100.0ʱ�õ��Ľ����1111111.52499999999999
		/// Math.Round�������⣬�������Լ���������������롣
		/// </summary>
		/// <param name="data"></param>
		/// <param name="DesLength"></param>
		/// <returns></returns>
		public static string SetRound(object pData, int pDesLength) {
			if(pData==null || pData.ToString().Trim()=="")
				return "0";
			double d = Convert.ToDouble(pData);
			double dp = Math.Pow(10,pDesLength);
			double l = Math.Floor(dp*d+0.5); 
			return Convert.ToString(Math.Round(l/dp, pDesLength));
		}
		#endregion ����ת�����...
		
 
		//
		/// <summary>
		/// ��ʼ��������Ƶ��ֶΣ�Ȼ��ǰ��Ƶ��ֶ����õ������Section ��
		/// </summary>
		/// <param name="pReport"></param>
		public static  void IniGroupFieldOnOpen(DIYReport.ReportModel.RptReport pReport){
			//�ж��Ƿ��з����Section
			DIYReport.ReportModel.RptSectionList sectionList = pReport.SectionList;
			foreach(DIYReport.ReportModel.RptSection section in sectionList){
				if(section.SectionType == SectionType.GroupHead || section.SectionType == SectionType.GroupFooter){
					DIYReport.GroupAndSort.RptFieldInfo groupField =  section.GroupField;
					if(groupField == null){Debug.Assert(false,"�ڻ�ȡ����ķ���Section ʱ���õ��ֶε���ϢΪ�ա�"); }
					IList fieldList =pReport.DesignField;
					if(fieldList == null){Debug.Assert(false,"û�г�ʼ����Ƶ��ֶΡ�"); }
					int count = fieldList.Count ;
					for(int i =0; i <count;i++){
						DIYReport.GroupAndSort.RptFieldInfo designField = fieldList[i] as DIYReport.GroupAndSort.RptFieldInfo;
						if(designField.FieldName.Trim().ToUpper() == groupField.FieldName.Trim().ToUpper()){
							designField.IsGroup = true;
							designField.IsAscending = groupField.IsAscending ;
							designField.OrderIndex = groupField.OrderIndex;
							designField.SetSort = groupField.SetSort;
							designField.DivideName = groupField.DivideName;
							section.GroupField = designField;
							break;
						}
					}
				}
			}
		}
		/// <summary>
		/// ����߾�...
		/// </summary>
		/// <param name="mrg"></param>
		/// <returns></returns>
		public static Margins GetRegionMargins(Margins mrg) {
			Margins reg = mrg.Clone() as Margins;
			return System.Globalization.RegionInfo.CurrentRegion.IsMetric ?
				PrinterUnitConvert.Convert(reg,PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display ) :
				reg;
		}

		/// <summary>
		/// �����ֶε�������ȡ��Ӧ���ֶ����ơ�
		/// </summary>
		/// <param name="pFieldDesc"></param>
		/// <param name="designField"></param>
		/// <returns></returns>
		public static string GetFieldNameByDesc(DataTable dtData,string pFieldDesc){
			if(pFieldDesc==null || pFieldDesc.Length ==0 || dtData==null)
				return pFieldDesc;
			foreach(DataColumn dc in dtData.Columns){
				if(string.Compare(dc.Caption,pFieldDesc,true)==0){
					return dc.ColumnName;
				}
			}
			return pFieldDesc;

		}
		/// <summary>
		/// �����ֶε�������ȡ��Ӧ���ֶ����ơ�
		/// </summary>
		/// <param name="pFieldDesc"></param>
		/// <param name="designField"></param>
		/// <returns></returns>
		public static string GetFieldNameByDesc(string pFieldDesc,IList designField){
			if(pFieldDesc==null || pFieldDesc.Length ==0 || designField == null)
				return pFieldDesc;
			foreach(DIYReport.GroupAndSort.RptFieldInfo info in designField){
				if(string.Compare(info.Description,pFieldDesc,true)==0){
					return info.FieldName ;
				}
			}
			return pFieldDesc;
		}
		
		/// <summary>
		/// ��������Դת��Ϊ��Ӧ������ֶ���Ҫ���б�
		/// </summary>
		/// <param name="dataSource"></param>
		/// <returns></returns>
		public  static IList ToDesignField(object dataSource){
			IList dList = new ArrayList(); 
			if(dataSource!=null  ){
				DataTable dt = GetDataTableByObject(dataSource);  
				int i = 0;
				foreach(DataColumn dc in dt.Columns){
					//����ID����ʾ��Ϊ��������
					if( string.Compare(dc.ColumnName,"ID",true)==0)
						continue;
						DIYReport.GroupAndSort.RptFieldInfo info = new DIYReport.GroupAndSort.RptFieldInfo(dc.ColumnName,dc.Caption
							,dc.DataType.Name,i++);
						dList.Add( info );
				}
			}
			return dList;
		}

		#region ��Ϊ�������...
		public static string GetTextBySectionType(SectionType pType){
			switch(pType){
				case SectionType.ReportTitle :
					return "����ҳü";
				case SectionType.PageHead :
					return "ҳü";
				case SectionType.Detail:
					return "������";
				case SectionType.ReportBottom  :
					return "����ҳ��";
				case SectionType.PageFooter :
					return "ҳ��";
				case SectionType.GroupHead :
					return "��ҳü";
				case SectionType.GroupFooter :
					return "��ҳ��";
				case SectionType.TopMargin:
					return "�ϱ߾���";
				case SectionType.BottomMargin:
					return "�±߾���";
				default:
					return "�����ͻ�û�д���";
			}
		}
		#endregion ��Ϊ�������...

		#region ����ת�����...
		/// <summary>
		/// ������Ļ����������������ת������һ������ľ���
		/// </summary>
		/// <param name="pFirst"></param>
		/// <param name="pLast"></param>
		/// <returns></returns>
		public static Rectangle ChangeMousePointToRect(Point pFirst,Point pLast){
			Point newFPoint = pFirst;
			Point newLPoint = pLast;
			newFPoint.X = pFirst.X < pLast.X ? pFirst.X : pLast.X ;
			newFPoint.Y  = pFirst.Y  < pLast.Y  ? pFirst.Y  : pLast.Y  ;
			newLPoint.X = pFirst.X > pLast.X ? pFirst.X : pLast.X ;
			newLPoint.Y = pFirst.Y  > pLast.Y  ? pFirst.Y  : pLast.Y  ;
			return new Rectangle(newFPoint ,new Size(newLPoint.X - newFPoint.X ,newLPoint.Y - newFPoint.Y));
		}
		#endregion ����ת�����...

		#region ϵͳ...
		[DllImport("User32.dll")]
		public static extern bool SetWindowPos(IntPtr hwnd,int hWndInsertAfter,int x,int y,int cx,int cy,int wFlagslong);
		#endregion ϵͳ...

		#region ��������...
		/// <summary>
		/// ��������� 
		/// </summary>
		/// <param name="pDividend"></param>
		/// <param name="pDivisor"></param>
		/// <returns></returns>
		public static decimal Dividend(object pDividend ,object pDivisor ){
			if(pDividend == null || pDividend == System.DBNull.Value 
				|| pDivisor==null || pDivisor == System.DBNull.Value ){

				return decimal.Parse("0.00");
			}
			if(double.Parse(pDivisor.ToString())==0){
				return decimal.Parse("0.00");
			}
			decimal re = decimal.Parse(pDividend.ToString()) / decimal.Parse(pDivisor.ToString());
			return re;
		}
		/// <summary>
		/// ���������
		/// </summary>
		/// <param name="pDividend"></param>
		/// <param name="pDivisor"></param>
		/// <param name="pDesLength"></param>
		/// <returns></returns>
		public static decimal Dividend(object pDividend ,object pDivisor ,int pDesLength){
			if(pDividend == null || pDividend == System.DBNull.Value 
				|| pDivisor==null || pDivisor == System.DBNull.Value ){

				return decimal.Parse("0.00");
			}
			if(double.Parse(pDivisor.ToString())==0){
				return decimal.Parse("0.00");
			}
			decimal re = decimal.Parse(pDividend.ToString()) / decimal.Parse(pDivisor.ToString());
			return decimal.Parse(SetRound(re,pDesLength));
		}
		/// <summary>
		/// ��������� 
		/// </summary>
		/// <param name="pDividend"></param>
		/// <param name="pDivisor"></param>
		/// <returns></returns>
		public static float DividendToFloat(object pDividend ,object pDivisor ){
			if(pDividend == null || pDividend == System.DBNull.Value 
				|| pDivisor==null || pDivisor == System.DBNull.Value ){

				return float.Parse("0.00");
			}
			if(double.Parse(pDivisor.ToString())==0){
				return float.Parse("0.00");
			}
			float re = float.Parse(pDividend.ToString()) / float.Parse(pDivisor.ToString());
			return re;
		}
		/// <summary>
		/// ���������
		/// </summary>
		/// <param name="pDividend"></param>
		/// <param name="pDivisor"></param>
		/// <param name="pDesLength"></param>
		/// <returns></returns>
		public static float DividendToFloat(object pDividend ,object pDivisor ,int pDesLength){
			if(pDividend == null || pDividend == System.DBNull.Value 
				|| pDivisor==null || pDivisor == System.DBNull.Value ){

				return float.Parse("0.00");
			}
			if(double.Parse(pDivisor.ToString())==0){
				return float.Parse("0.00");
			}
			float re = float.Parse(pDividend.ToString()) / float.Parse(pDivisor.ToString());
			return float.Parse(SetRound(re,pDesLength));
		}
		#endregion ��������...

		#region ��ͼ���...
		/// <summary>
		/// �������ĺ�
		/// </summary>
		/// <param name="pData1"></param>
		/// <param name="pData2"></param>
		/// <returns></returns>
		public static int SumToInt32(object pData1,object pData2){
			int data1 = ToInt( pData1 );
			int data2 = ToInt( pData2 );
			return  data1 + data2;

		}
		/// <summary>
		/// �������ĺ�
		/// </summary>
		/// <param name="pData1"></param>
		/// <param name="pData2"></param>
		/// <returns></returns>
		public static decimal SumToDecimal(object pData1,object pData2){
			decimal data1 = ToDecimal( pData1 ,2 , true );
			decimal data2 = ToDecimal( pData2 ,2 ,true);
			return  data1 + data2;

		}
		#endregion ��ͼ���...

		#region ͼ����...
		/// <summary>
		/// ��ͼ���ļ�ת����string ����
		/// </summary>
		/// <param name="pImage"></param>
		/// <returns></returns>
		public static string ImageToString(System.Drawing.Image pImage){
			byte[] bts = ImageToByte(pImage);
			return Convert.ToBase64String(bts);
			//return System.Text.Encoding.UTF8.GetString(bts);    
		}
		/// <summary>
		/// ��ͼ���ļ�ת����Byte[] ����
		/// </summary>
		/// <param name="pImage"></param>
		/// <returns></returns>
		public static Byte[] ImageToByte(System.Drawing.Image pImage){
			if(pImage==null){
				return null;
			}
			FileStream  st = null;
			Byte[] buffer = null;
			try{
				string cPath = Environment.CurrentDirectory + "TempJpg.Temp";
				pImage.Save(cPath);
				st  =  new FileStream(cPath, FileMode.Open, FileAccess.Read);
				BinaryReader  mbr   = new BinaryReader(st);
				buffer  = new Byte[st.Length];
				mbr.Read(buffer, 0 ,  int.Parse(st.Length.ToString()) );
				
			}
			catch{
				//UP.Utils.TraceEx.Write("ͼ��ת��ΪByte[] ���ͳ���");
				return null;
			}
			finally{
				if(st!=null){
					st.Close();
				}
			}
			return buffer;
		}
		/// <summary>
		/// ͨ��string ת����Image�ļ���ʽ����ʽ
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static System.Drawing.Image StringToImage(string data){
			byte[] bts = Convert.FromBase64String(data);
			//byte[] bts = System.Text.Encoding.UTF8.GetBytes(data); 
			return ByteToImage(bts);
		}
		/// <summary>
		/// ͨ��Byte[] �ֽ���ת����Image�ļ���ʽ����ʽ
		/// </summary>
		/// <param name="pByte"></param>
		/// <returns></returns>
		public static System.Drawing.Image ByteToImage(Byte[] pByte){ 
			if (pByte!=null && pByte.Length > 0){
				try{
					MemoryStream stream = new MemoryStream(pByte, true);
					stream.Write(pByte, 0, pByte.Length);
					System.Drawing.Image img =  Image.FromStream(stream );
					return img;
				}
				catch(Exception e){
					//throw new UP.Utils.AppExceptionMsg("�����ݿ��еõ��ļ���Ϣ����" + e.Message); 
					return null;
				}
			}
			else{
				return null;
			}
		}
		#endregion ͼ����...

		#region ����Դת��...
		/// <summary>
		///  ���������������ת���� DataView �ĸ�ʽ
		/// </summary>
		/// <param name="pObj"></param>
		/// <returns></returns>
		public static  DataView  GetDataViewByObject(object pObj){
			if(pObj==null)
				return null;
			string name = pObj.GetType().Name ;
			DataView dv = null;
			switch(name){
				case "DataSet":
					DataSet ds = pObj as DataSet ;
					dv = ds.Tables[0].DefaultView;
					break;
				case "DataTable":
					dv = (pObj as DataTable).DefaultView  ;
					break;
				case "DataView":
					dv =  pObj as DataView ;
					break;
				default:
					Debug.Assert(false,"����ԴĿǰ��֧��"+ name +"���͵�����","");
					break;
			}
			return dv;
		}
		/// <summary>
		///  ���������������ת���� DataTable �ĸ�ʽ
		/// </summary>
		/// <param name="pObj"></param>
		/// <returns></returns>
		public static  DataTable  GetDataTableByObject(object pDataSource){
			if(pDataSource == null)
				return null;
			string name = pDataSource.GetType().Name ;
			DataTable  dt = null;
			switch(name){
				case "DataSet":
					DataSet ds = pDataSource as DataSet ;
					dt = ds.Tables[0];
					break;
				case "DataTable":
					dt = pDataSource as DataTable ;
					break;
				case "DataView":
					dt =  (pDataSource as DataView).Table  ;
					break;
				default:
					DIYReport.TrackEx.Write("����ԴĿǰ��֧��"+ name +"���͵�����.");
					break;
			}
			return dt;
		}
		/// <summary>
		/// ���������������ת���� DataSet �ĸ�ʽ
		/// </summary>
		/// <param name="pDataSource"></param>
		/// <returns></returns>
		public static DataSet  GetDataSetByObject(object pDataSource){
			string sName = pDataSource.GetType().Name ;
			DataSet  ds = null;
			switch(sName){
				case "DataSet":
					ds = (pDataSource as DataSet) ;
					break;
				case "DataTable":
					ds = (pDataSource as DataTable).DataSet  ;
					break;
				case "DataView":
					ds = (pDataSource as DataView).Table.DataSet  ;
					break;
				default:
					Debug.Assert(false,"Ŀǰ��֧������"+ sName +"�������͵�����Դ��","");
					break;
			}
			return ds;
		}
		#endregion ����Դת��...

		/// <summary>
		/// ��ʽ���ַ���  
		/// </summary>
		/// <param name="formatString"></param>
		/// <param name="pObj"></param>
		/// <returns></returns>
		public static string FormatString(string formatString,object pObj) {
			if(pObj==null || pObj==System.DBNull.Value || pObj.ToString().Length ==0)
				return string.Empty;

			string strRet="";
			if (formatString!=null && formatString.Length > 0) {
				switch (pObj.GetType().Name.ToLower()) {
					case "string":
						strRet = string.Format(formatString,pObj.ToString());
						break;
					case "datetime":
						strRet=Convert.ToDateTime(pObj).ToString(formatString);
						break;
					case "int16":
					case "int32":
					case "int64":
					case "uint16":
					case "uint32":
					case "uint64":
					case "single":
					case "double":
					case "decimal":
						strRet=Convert.ToDecimal(pObj).ToString(formatString);
						break;
					default:
						strRet=pObj.ToString();
						break;
				}
			}
			else {
				strRet=pObj.ToString();
			}
			return strRet;
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public class TrackEx{
		private static string _APP_TEMP_PATH = AppDomain.CurrentDomain.BaseDirectory + @"\temp";
		public static bool RequireSaveCodeRunInfo = true;
		private static bool _HshGetConfigInfo = false;
		
		/// <summary>
		/// ��¼���е���Ϣ��
		/// </summary>
		/// <param name="msg"></param>
		public static void Write(string msg){
			Write(false,msg);
		}
		/// <summary>
		/// ��¼���е���Ϣ��
		/// </summary>
		/// <param name="pCondition"></param>
		/// <param name="pMsgStr"></param>
		/// <param name="pFullPath"></param>
		/// <returns></returns>
		public static bool Write(bool pCondition,string pMsgStr) {
			if(pCondition){
				return true;
			}
			//�ж��Ƿ�Ϊ��¼�������е���Ϣ������ǣ�������Ӧ�ó���������������Ƿ�洢

			if(!_HshGetConfigInfo){
				string str = ConfigurationSettings.AppSettings["SaveCodeRunInfo"];
				if(str==null || str==""){
					return true;
				}
				else{
					RequireSaveCodeRunInfo = (str =="Y") || (str == "1");
				}
			}
			if(!RequireSaveCodeRunInfo){return true;}


			string fullPath = null ;
			if(fullPath==null || fullPath==""){
				string logFile = ConfigurationSettings.AppSettings["AppLogFile"];
				if(logFile==null || logFile==""){
					logFile = "App.Run.Log";
				}
				//���ڳ�����ռǼ���ʱ���������ļ��Ĵ�С
				string curDate = "(" + DateTime.Now.ToShortDateString() +")" ; 
				fullPath = _APP_TEMP_PATH;
				bool fb = System.IO.Directory.Exists(fullPath);
				if(!fb){
					System.IO.Directory.CreateDirectory(fullPath); 
				}
				fullPath = fullPath + @"\" + curDate + logFile;
			}
			bool b = System.IO.File.Exists(fullPath);
			if(!b){
				//System.IO.File.CreateText(fullPath) ;
			}
			StreamWriter swFile = null;
			try {
				swFile=new StreamWriter(fullPath,true);
				string errTypeStr =   "-->";
				string strLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + errTypeStr + pMsgStr;
				swFile.WriteLine(strLine);
			}
			catch{
			}
			finally{
				if(swFile!=null){
					swFile.Flush();
					swFile.Close();
				}
			}
			return true;
		}
	}
}
