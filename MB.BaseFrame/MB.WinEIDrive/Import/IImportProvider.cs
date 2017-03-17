using System;

namespace MB.WinEIDrive.Import
{
	/// <summary>
	/// IImportProvider ���ݵ���ӿ�˵����
	/// </summary>
	public interface IImportProvider
	{
		void Commit();

		event ImportProgressEventHandler DataProgress;
	}
}
