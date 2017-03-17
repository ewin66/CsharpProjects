using System;
using System.Data;

namespace MB.WinEIDrive
{
	/// <summary>
	/// DataHelpers ���ݴ���
	/// </summary>
	public class DataHelpers
	{
		#region private construct function...
		/// <summary>
		/// 
		/// </summary>
		private DataHelpers(){
		}
		#endregion private construct function...

		/// <summary>
		/// ����������ת��ΪDataView �ĸ�ʽ��
		/// </summary>
		/// <param name="pObj"></param>
		/// <returns></returns>
		public static  DataView  ToDataView(object objData){
			string name = objData.GetType().Name ;
			DataView dv = null;
			switch(name){
				case "DataSet":
					DataSet ds = objData as DataSet ;
					dv = ds.Tables[0].DefaultView;
					break;
				case "DataTable":
					dv = (objData as DataTable).DefaultView  ;
					break;
				case "DataView":
					dv =  objData as DataView ;
					break;
				default:
					throw new Exception("����ԴĿǰ��֧��"+ name +"���͵�����.");
			}
			return dv;
		}
	}
}
