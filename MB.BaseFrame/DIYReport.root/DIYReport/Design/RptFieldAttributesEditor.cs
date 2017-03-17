//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-22
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
/*
 ����һ��˼·����System.ComponentModel.PropertyDescriptor�̳��Լ����࣬Ȼ��
 ����DisplayName��������ʵ��һ������ܣ�ʹ��Ӧ��Ӣ�����������ġ����Ҫ��PropertyGrid.SelectObject
  ��ѡ�Ķ����ICustomTypeDescriptor�̳У��������е�GetProperties�й����Լ���PropertyDescriptorCollection���ڷ��ء�
*/
using System;
using System.Data ;
using System.Reflection ;
using System.Collections ;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using DIYReport.Common;

namespace DIYReport.Design {
	/// <summary>
	/// BaseDesignListEditor  ListBox xia  ��
	/// </summary>
	public class BaseDesignListEditor : UITypeEditor {
		protected IServiceProvider _Provider;
		public BaseDesignListEditor() {
		}
		public override object EditValue(ITypeDescriptorContext context, 
			IServiceProvider provider, object value) {
			_Provider = provider;
			//Create the listbox for display
			ListBox    lstFields = new ListBox();
			lstFields.SelectedIndexChanged +=new EventHandler(lstFields_SelectedIndexChanged);

			AddDataToList(lstFields);

			// Display the combolist
			
			((IWindowsFormsEditorService)provider.GetService(
				typeof(IWindowsFormsEditorService))).DropDownControl(lstFields);
  
			if(lstFields.SelectedItem!=null){
				string str = lstFields.SelectedItem.ToString(); 
				return str; 
			}
			else{
				return value;
			}
		
		}  
		
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
			
			// Should we return the style for our listbox?
			if ((context != null) && (context.Instance != null))
				return UITypeEditorEditStyle.DropDown;
			
			// Return the default edit style.
			return base.GetEditStyle(context);
			//return UITypeEditorEditStyle.DropDown ;
		} // End GetEditStyle()

		#region �ڲ��������...
		//
		//�������ݿ���ֶ�
		protected virtual void AddDataToList(ListBox pLst){
			
		}
 
		#endregion �ڲ��������...
		private void lstFields_SelectedIndexChanged(object sender, EventArgs e) {
			((IWindowsFormsEditorService)_Provider.GetService(
				typeof(IWindowsFormsEditorService))).CloseDropDown();
		}
	}
	/// <summary>
	/// ѡ����ʾ��������Դ���ֶ�
	/// </summary>
	public class RptFieldAttributesEditor : BaseDesignListEditor{
		protected override void AddDataToList(ListBox pLst) {
			pLst.Items.Clear(); 
			IList fieldList = DIYReport.UserDIY.DesignEnviroment.CurrentReport.DesignField;
			if(fieldList!=null && fieldList.Count > 0 ){
				foreach(object dc in fieldList){
					if(dc.GetType().Name == "RptFieldInfo"){
						DIYReport.GroupAndSort.RptFieldInfo field = dc as DIYReport.GroupAndSort.RptFieldInfo;
						if(string.Compare(field.DataType, "Byte[]",true)!=0){ //byte[] �������Ͳ���Ҫ���з���
							if(field.Description==null || field.Description.Trim().Length ==0) continue;

							pLst.Items.Add(field.Description);  
						}
					}
					else{
						pLst.Items.Add(dc.ToString());
					}
				}
			}
		}

	}
	/// <summary>
	///  ��ʾ�ӱ���ѡ��ı༭��
	/// </summary>
	public class RptSubReportAttributesEditor : BaseDesignListEditor{
		protected override void AddDataToList(ListBox pLst) {
			pLst.Items.Clear(); 
			if(DIYReport.UserDIY.DesignEnviroment.CurrentReport.SubReportCommand!=null){
				IList names = DIYReport.UserDIY.DesignEnviroment.CurrentReport.SubReportCommand.SubReportName;
				if(names==null || names.Count== 0)
					return;
				foreach(object name in names){
					pLst.Items.Add(name.ToString());
				}
			}
			else{
				Hashtable  fieldList = DIYReport.UserDIY.DesignEnviroment.CurrentReport.SubReports;
				if(fieldList==null || fieldList.Count == 0 )
					return;
				foreach(object key in fieldList.Keys){
					pLst.Items.Add(key.ToString()); 
				}
			}
		}
	}
	/// <summary>
	///  ��ʾ�ӱ���ѡ��ı༭��
	/// </summary>
	public class RelationMemberAttributesEditor : BaseDesignListEditor{
		protected override void AddDataToList(ListBox pLst) {
			pLst.Items.Clear(); 
			DataSet ds = DIYReport.PublicFun.GetDataSetByObject(DIYReport.UserDIY.DesignEnviroment.CurrentReport.DataSource);
			if(ds==null || ds.Relations.Count ==0 )
				return;
			foreach(System.Data.DataRelation  relation in ds.Relations){
				pLst.Items.Add(relation.RelationName); 
			}
		}
	}
    /// <summary>
    /// ��ʾ���صĴ�ӡ��
    /// </summary>
    public class EnumPrinterListEditor : BaseDesignListEditor
    {
        protected override void AddDataToList(ListBox pLst) {
            pLst.Items.Clear();
            var printers = DIYReport.Common.EnumPrintersHelperEx.GetLocalPrinters();//.EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL | PrinterEnumFlags.PRINTER_ENUM_NETWORK);
            if (printers == null || printers.Count == 0)
                return;
            foreach (var p in printers) {
                pLst.Items.Add(p);
            }
        }
    }
}
