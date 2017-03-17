//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	Localization ���ػ����Դ����ࡣ
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Resources;
using System.Globalization;

namespace MB.XWinLib.Localization
{
	/// <summary>
	/// GridLocal ���ػ����Դ����ࡣ��
	/// </summary>
	public class GridLocal : DevExpress.XtraGrid.Localization.GridLocalizer {
		#region ��������...
		private static GridLocal _LocaObj;
		ResourceManager manager = null;
		#endregion ��������...

		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		public GridLocal() {
			CreateResourceManager();
		}
		#endregion ���캯��...

		#region ��չ��static ����...
		public static GridLocal Create(){
			if(_LocaObj == null){
				_LocaObj = new GridLocal();
			}
			return _LocaObj;
		}
		#endregion ��չ��static ����...

		#region ���ǻ���ķ���...
		protected virtual void CreateResourceManager() {
			if(manager != null) this.manager.ReleaseAllResources();
            string resName = "MB.XWinLib.Localization.GridLocalRes";
			CultureInfo curInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
			string cName = curInfo.Name;
			if(!cName.Equals("zh-CN")) //��������ĵĻ�ֱ�ӻ�ȡĬ��ֵ�Ϳ���,�����ȡӢ�����Դ�ļ���
                resName = resName + "_en";// "_" + cName;

			this.manager = new ResourceManager(resName,this.GetType().Assembly);

		}
		protected virtual ResourceManager Manager { get { return manager; } }
		public override string Language { get { return CultureInfo.CurrentUICulture.Name; }}
		public override string GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId id) {
			string resStr = "GridStringId." + id.ToString();
            string ret = Manager.GetString(resStr);
            if (ret == null) {
                MB.Util.TraceEx.Write(string.Format("��XtraGrid �ַ� {0} ���Ա��ػ�ʱû�ҵ�",resStr));
                ret = "";
            }
            return ret;
		}
		#endregion ���ǻ���ķ���...
	}
}
