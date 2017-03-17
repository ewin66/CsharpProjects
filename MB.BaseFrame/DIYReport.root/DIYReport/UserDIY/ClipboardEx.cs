//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-04-19
// Description	:	ClipboardEx ������������ش���
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace DIYReport.UserDIY {
	/// <summary>
	/// ClipboardEx ������������ش���
	/// </summary>
	public class ClipboardEx {
		private static readonly string MY_CLIPBOARD_DATA_FORMAT_NAME = "DIYReport.ReportModel.RptSingleObj" ;
		/// <summary>
		/// add private construct function to prevent instance.
		/// </summary>
		private ClipboardEx() {
		
		}
		/// <summary>
		/// �Ӽ������л�ȡ
		/// </summary>
		/// <returns></returns>
		public static IList GetFromClipBoard(){
			IDataObject ido = Clipboard.GetDataObject(); 
			if(ido.GetDataPresent(MyClipbordData.Format.Name)) {
				object obj = ido.GetData(MyClipbordData.Format.Name,true);
				MyClipbordData cbCtrl = obj as MyClipbordData; 
				
				return cbCtrl.DataToIList(); 
			}
			return null;
		}
		/// <summary>
		/// �����ݸ��Ƶ��������С�
		/// </summary>
		/// <param name="ctls"></param>
		public static void CopyToClipBoard(IList ctls){
			IDataObject ido = new DataObject();
			MyClipbordData cData = new MyClipbordData(ctls);

			ido.SetData(MyClipbordData.Format.Name,true,cData);
			Clipboard.SetDataObject(ido,false);
		}

	}
	/// <summary>
	/// �洢�ڼ������е����ݸ�ʽ���塣
	/// </summary>
	[Serializable()]
	public class MyClipbordData{
		private static DataFormats.Format _Format; 
		private ArrayList _Ctls;

		#region ���캯��...
		static MyClipbordData(){
			_Format =  DataFormats.GetFormat(typeof(MyClipbordData).FullName);
		}
		public MyClipbordData(){
		}
		public MyClipbordData(IList cData){
			_Ctls = new ArrayList();
			if(cData==null)
				return;
			foreach(object ctl in cData){
				DIYReport.Interface.IRptSingleObj rptObj = ctl as DIYReport.Interface.IRptSingleObj;
				if(rptObj==null)
					continue;
				MySingleRptCtlData ctlData = new MySingleRptCtlData(rptObj);
				_Ctls.Add(ctlData);
			}
		}
		#endregion ���캯��...
		/// <summary>
		/// ת��ΪDiyreport �����Ҫ�Ŀؼ���ʽ��
		/// </summary>
		/// <returns></returns>
		public IList DataToIList(){
			TrackEx.Write("���ڼ����Զ��屨�����:" +  ReportXmlHelper.REPORT_ASSEMBLY);
			System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(DIYReport.ReportXmlHelper.REPORT_ASSEMBLY);
			TrackEx.Write("�Զ��屨��������سɹ���");

			ArrayList ctlList = new ArrayList();
			foreach(MySingleRptCtlData ctl in _Ctls){
				DIYReport.Interface.IRptSingleObj rptObj = asm.CreateInstance(ctl.CtrlFullName) as DIYReport.Interface.IRptSingleObj;    
				if(rptObj==null)
					continue;
				rptObj.BeginUpdate();
				setRptCtlProperties(rptObj,ctl.PropertyList);
				rptObj.EndUpdate();

				ctlList.Add(rptObj);
			}
			return ctlList;
		}
		//���ÿؼ�������ֵ��
		private static void setRptCtlProperties(DIYReport.Interface.IRptSingleObj rptObj,Hashtable propertyList) {
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(rptObj);

			foreach (PropertyDescriptor myProperty in properties) {	
				if(!propertyList.Contains(myProperty.Name))
					continue;
				object val = propertyList[myProperty.Name];
				try {
					myProperty.SetValue(rptObj,val);
				}
				catch(Exception ex) {
					DIYReport.TrackEx.Write("�ڽ��м����������ʱ��,���ÿؼ�������ֵʱ����" + ex.Message);
				}
			}	
		}

		public static DataFormats.Format Format{
			get{
				return _Format;
			}
		}
	}
	/// <summary>
	/// ����������ƿؼ���Ӧ�����ݡ�
	/// </summary>
	[Serializable()]
	public class MySingleRptCtlData{
		private string _CtrlFullName;
		private Hashtable _PropertyList;
		
		#region ���캯��...
		public MySingleRptCtlData() {
			
		}

		public MySingleRptCtlData(DIYReport.Interface.IRptSingleObj  ctrl) {		
			_PropertyList = new Hashtable();

			_CtrlFullName = ctrl.GetType().FullName;
			
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(ctrl);
			foreach (PropertyDescriptor myProperty in properties) {
				try {
					if(!myProperty.PropertyType.IsSerializable)
						continue;
					object val = myProperty.GetValue(ctrl);
					if(val!=null){
						Type objType = val.GetType();
						System.Reflection.MethodInfo methInfo = objType.GetMethod("Clone"); 
						if(methInfo!=null){
							val = methInfo.Invoke(val,null);
						}
					}
					_PropertyList.Add(myProperty.Name,val);	
				}
				catch(Exception ex) {
					DIYReport.TrackEx.Write("�ڽ��м����������ʱ��,��ȡ���Ե�ֵ����" + ex.Message);
				}
			}

		}	
		#endregion ���캯��...

		public string CtrlFullName {
			get {
				return _CtrlFullName;
			}
			set {
				_CtrlFullName = value;
			}
		}

		public Hashtable PropertyList {
			get {
				return _PropertyList;
			}
			
		}
	}
}
