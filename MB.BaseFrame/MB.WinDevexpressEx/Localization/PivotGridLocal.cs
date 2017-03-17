//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	PivotGridLocal ���ػ����Դ����ࡣ
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Resources;
using System.Globalization;

using DevExpress.XtraPivotGrid.Localization;
namespace MB.XWinLib.Localization
{
	/// <summary>
	/// PivotGridLocal ���ػ����Դ����ࡣ
	/// </summary>
	public class PivotGridLocal : DevExpress.XtraPivotGrid.Localization.PivotGridLocalizer{
		#region ��������...
        private static PivotGridLocal _LocaObj;
		ResourceManager manager = null;
		#endregion ��������...

		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		public PivotGridLocal() {
			CreateResourceManager();
		}
		#endregion ���캯��...

		#region ��չ��static ����...
        public static PivotGridLocal Create() {
			if(_LocaObj == null){
                _LocaObj = new PivotGridLocal();
			}
			return _LocaObj;
		}
		#endregion ��չ��static ����...

		#region ���ǻ���ķ���...
		protected virtual void CreateResourceManager() {
			if(manager != null) this.manager.ReleaseAllResources();
            string resName = "MB.XWinLib.Localization.PivotGridLocalRes";
			CultureInfo curInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
			string cName = curInfo.Name;
			if(!cName.Equals("zh-CN")) //��������ĵĻ�ֱ�ӻ�ȡĬ��ֵ�Ϳ��ԡ�
                resName = resName + "_en";
				//resName = resName + "_" + cName;

			this.manager = new ResourceManager(resName,this.GetType().Assembly);

		}
		protected virtual ResourceManager Manager { get { return manager; } }
		public override string Language { get { return CultureInfo.CurrentUICulture.Name; }}
		public override string GetLocalizedString(PivotGridStringId id) {
			string resStr = "PivotGridStringId." + id.ToString();
			string ret = Manager.GetString(resStr);
            if (ret == null) {
                MB.Util.TraceEx.Write(string.Format("��PivotGridLocal �ַ� {0} ���Ա��ػ�ʱû�ҵ�", resStr));
                ret = "";
            }
			return ret;
		}
		#endregion ���ǻ���ķ���...
	}
}
