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
	/// PrintLocal ���ػ����Դ����ࡣ��
	/// </summary>
	public class PrintLocal: DevExpress.XtraPrinting.Localization.PreviewLocalizer {
		ResourceManager manager = null;

		public PrintLocal() {
			CreateResourceManager();
		}
		protected virtual void CreateResourceManager() {
			if(manager != null) this.manager.ReleaseAllResources();
            string resName = "MB.XWinLib.Localization.PrintLocalRes";
			CultureInfo curInfo = System.Threading.Thread.CurrentThread.CurrentCulture    ;
			string cName = curInfo.Name;
			if(!cName.Equals("zh-CN")) //��������ĵĻ�ֱ�ӻ�ȡĬ��ֵ�Ϳ��ԡ�
                resName = resName + "_en";
				//resName = resName + "_" + cName;
			this.manager = new ResourceManager(resName, this.GetType().Assembly);
		}
		protected virtual ResourceManager Manager { get { return manager; } }
		public override string Language { get { return CultureInfo.CurrentUICulture.Name; }}
		public override string GetLocalizedString(DevExpress.XtraPrinting.Localization.PreviewStringId id) {
                string resStr = "PreviewStringId." + id.ToString();
                string ret = Manager.GetString(resStr);
                if (ret == null) {
                    MB.Util.TraceEx.Write(string.Format("��PrintLocal �ַ� {0} ���Ա��ػ�ʱû�ҵ�", resStr));

                    ret = "";
                }
                return ret;
          
		}
	}
}
