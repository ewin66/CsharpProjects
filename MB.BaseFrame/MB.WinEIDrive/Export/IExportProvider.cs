using System;

namespace MB.WinEIDrive.Export
{
	/// <summary>
	/// IExportProvider ���ݵ����Ľӿ�˵����
	/// </summary>
	public interface IExportProvider
	{
		object DataSource{get;set;}
		void Commit();

		event ExportProgressEventHandler DataProgress;
		event ExportColumnsEventHandler ColumnProgress;
	}
}
