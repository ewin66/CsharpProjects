using System;

namespace MB.WinEIDrive
{
	/// <summary>
	/// FileLib �ļ������ࡣ
	/// </summary>
	public class FileLib
	{
		#region private construct function...
		/// <summary>
		/// add private construct function to prevent instance.
		/// </summary>
		private FileLib()
		{
		}
		#endregion private construct function...

		#region public static function...
		/// <summary>
		///  ���ļ���
		/// </summary>
		/// <param name="fileName"></param>
		public static void TryToOpenFile(string fileName) {
			try {
				System.Diagnostics.Process.Start(fileName);
			}
			catch(Exception e) {
				throw new Exception("�ļ�:" + fileName + "���ܱ��򿪣�������ļ��Ƿ���ڡ�" + e.Message);
			}
		}//
		#endregion public static function...
	}
}
