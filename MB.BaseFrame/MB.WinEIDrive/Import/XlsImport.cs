using System;
using System.Data;
using System.Collections;

using MB.WinEIDrive.Excel;
//using GemBox.ExcelLite; 
namespace MB.WinEIDrive.Import
{
	/// <summary>
	/// XlsImport ��Excel �е������ݡ�
	/// </summary>
	public class XlsImport : ImportBase,IImportProvider 
	{
		#region ��������...
		private ExcelFile _ExcelObj;
		private DataTable _DtData;
		private Hashtable _ColumnAndIndex;
        private bool _DataTableIsAutoCreate;
		#endregion ��������...

		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		/// <param name="dtData"></param>
		/// <param name="fullFileName"></param>
		public XlsImport(object dtData,string fullFileName){
            if (dtData != null) {
                DataView dv = MB.WinEIDrive.DataHelpers.ToDataView(dtData);
                if (dv != null)
                    _DtData = dv.Table;
            }
			_ExcelObj = new ExcelFile();
 
			_ExcelObj.LoadXls( fullFileName );
			_ColumnAndIndex = new Hashtable();
            if (dtData == null) {
                _DtData = createDataTable();
                _DataTableIsAutoCreate = true;
            }
		}
		#endregion ���캯��...

		#region Public ����...
		/// <summary>
		/// ִ�����ݵ���Ĳ�����
		/// </summary>
		/// <returns></returns>
		public virtual void Commit(){
            try {
                if (!_DataTableIsAutoCreate) {
                    bool b = setColumnAndIndex(_DtData);
                    if (!b)
                        return;
                }
                ExcelWorksheet sheet = _ExcelObj.Worksheets.ActiveWorksheet;
                //��ʾ�������κμ�¼��
                if (sheet.Rows.Count == 1)
                    return;
                int rowCount = sheet.Rows.Count;
                int colCount = sheet.Rows[0].AllocatedCells.Count;
                if (colCount == 0)
                    return;
                for (int i = 1; i < rowCount; i++) {
                    bool isEmptyRow = true;
                    ExcelRow dataRow = sheet.Rows[i];
                    DataRow newDr = _DtData.NewRow();
                    for (int j = 0; j < colCount; j++) {
                        if (!_ColumnAndIndex.ContainsKey(j))
                            continue;
                        DataColumn dc = (DataColumn)_ColumnAndIndex[j];
                        ExcelCell cell = dataRow.AllocatedCells[j];
                        object val = cell.Value;
                        if (val != null && !string.IsNullOrEmpty(val.ToString().TrimEnd()))
                            isEmptyRow = false;
                        OnProviderProgress(dc.ColumnName, ref val);
                        setRowValue(newDr, val, dc.ColumnName, dc.DataType);
                    }
                    //����ǿ��еĻ������ӵ����ص�DataTable��
                    if (!isEmptyRow)
                        _DtData.Rows.Add(newDr);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
			return;
		}
		#endregion Public ����...

        /// <summary>
        /// ��ȡ���ڵ�������ݡ�
        /// </summary>
        public DataTable ImportData {
            get {
                return _DtData;
            }
        }
		
		#region �ڲ���������...
        //����һ���յı�ṹ
        private DataTable createDataTable() {
            ExcelWorksheet sheet = _ExcelObj.Worksheets.ActiveWorksheet;
            if (sheet.Rows.Count == 0)
                return null;
            ExcelRow headRow = sheet.Rows[0];
            DataTable newDt = new DataTable();
            int aCount = headRow.AllocatedCells.Count;
            for (int i = 0; i < aCount; i++) {
                ExcelCell cell = headRow.AllocatedCells[i];
                if (cell.Value == null)
                    continue;
                DataColumn dc = new DataColumn(cell.Value.ToString().Trim());
                dc.Caption = cell.Value.ToString().Trim();
                _ColumnAndIndex.Add(i, dc);
                newDt.Columns.Add(dc);
            }
            return newDt;
        }
		//����Caption �ҵ�datatable�е���,���Ѻ�Excel �ж�Ӧ�е�Index һ��洢��hashTable����Ϊ
		//��ȡ�е���ʱ��Ӧ���С�
		private bool setColumnAndIndex(DataTable dt){
			ExcelWorksheet sheet = _ExcelObj.Worksheets.ActiveWorksheet;
			if(sheet.Rows.Count == 0)
				return false;
			ExcelRow headRow = sheet.Rows[0];
			int aCount = headRow.AllocatedCells.Count;
			for(int i=0; i < aCount; i++){
				ExcelCell cell = headRow.AllocatedCells[i];
				if(cell.Value==null)
					continue;
				foreach(DataColumn dc in dt.Columns){
					if(string.Compare(dc.Caption,cell.Value.ToString() ,true)!=0)
						continue;
					_ColumnAndIndex.Add(i,dc);
					break;
				}
			}
			return _ColumnAndIndex.Count > 0; 
		}
		//��ֵ����. 
		private void setRowValue(DataRow drData,object val,string fieldName,Type dataType){
            if (val == null || val == System.DBNull.Value) return;
 
			try{
                if (dataType != null && string.Compare(dataType.Name, "Boolean", true) == 0)
                    drData[fieldName] = getBooleanPropertyValue(val);
                else
				    drData[fieldName] = System.Convert.ChangeType( val,dataType );
			}
			catch{
			}
		}
        //����Boolean ֵ��
        private bool getBooleanPropertyValue(object val) {
            bool bVal = false;
            if (val != null && val != System.DBNull.Value) {
                bVal = string.Compare(val.ToString(), "1") == 0 || 
                       string.Compare(val.ToString(), "TRUE", true) == 0 ||
                       string.Compare(val.ToString(), "��", true) == 0 ||
                       string.Compare(val.ToString(), "ѡ��", true) == 0 ||
                       string.Compare(val.ToString(), "Checked", true) == 0;
            }
            return bVal;
        }
		#endregion �ڲ���������...
		
	}
}

