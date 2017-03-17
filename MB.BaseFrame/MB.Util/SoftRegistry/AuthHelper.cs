//---------------------------------------------------------------- 
// All rights reserved. 
// Author		:	chendc
// Create date	:	2003-01-04
// Description	:	AuthHelper
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Data;
using System.Windows.Forms ;
using System.Management;

namespace MB.Aop.SoftRegistry
{
	/// <summary>
	/// AuthHelper ϵͳϵ�кż�����
	/// </summary>
	public class AuthHelper {
		#region private construct...
		/// <summary>
		/// add private construct function to prevent instance.
		/// </summary>
		private  AuthHelper() {
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#endregion private construct...
		private const int MAX_SHOW_MSG = 10;

		#region public static function...
		/// <summary>
		/// ϵͳϵ�кż�����
		/// </summary>
		/// <returns></returns>
		public static bool AuthRight(string privateName){
			AuthRightInfo rightData = new AuthRightInfo();
			rightData.SNFileName = privateName;
			bool b = autoRight(rightData);
			return b;
		}
		/// <summary>
		/// ��ȡ�û���ǰ��Ȩ����Ϣ��
		/// </summary>
		/// <param name="privateName"></param>
		/// <returns></returns>
		public static AuthRightInfo GetAuthInfo(string privateName){
			//UP.Utils.TraceEx.Write("��ʼ����Ȩ�޼���.....");
			AuthRightInfo rightData = new AuthRightInfo();
			rightData.SNFileName = privateName;
			bool b = autoRight(rightData);
			if(b){
				return rightData;
			}
			return rightData;
		}
		#endregion public static function...
		

		#region private function...
		//Ȩ�޼���
		private static bool autoRight(AuthRightInfo  authDataInfo){
			string file;
			if(authDataInfo.SNFileName!=null && authDataInfo.SNFileName.Length >0)
                file = MB.Util.General.GeApplicationDirectory() + authDataInfo.SNFileName + ".Txt";
			else
                file = MB.Util.General.GeApplicationDirectory() + @"UfAccreditSn.Txt";
			//�����Ƿ��Ѿ�ӵ������Ȩ��
	
			try {
				System.IO.StreamReader  read = new System.IO.StreamReader (file);
				string strTemp = read.ReadLine ();
				read.Close();
				//	TraceEx.Write(strTemp);
				AuthRightInfo tempInfo = AuthDataRight(strTemp);
				if (tempInfo!= null) {
					
					authDataInfo.EndDate  = tempInfo.EndDate;
					DateTime curdate = System.DateTime.Now;
					authDataInfo.LinkCount  = tempInfo.LinkCount;
					if (curdate >= authDataInfo.EndDate) {
						MessageBox.Show ("��Ȩ���Ѿ�����,���������Ӧ����ϵ!","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);
						authDataInfo.PassIsRight = false;
					}
					else {
						TimeSpan tPan = authDataInfo.EndDate.Subtract(curdate);

						if(tPan.Days  < MAX_SHOW_MSG)
							MessageBox.Show ("��Ȩ�� "+ tPan.Days + " �����,���������Ӧ����ϵ!","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);

						authDataInfo.PassIsRight = true;
					}
				}//��֤��Ȩ���Ƿ���ȷ
			}
			catch(Exception ee) {
				//UP.Utils.TraceEx.Write("������ػ�ȡ������ݳ���." + ee.Message);
				authDataInfo.PassIsRight = false;
			}

			if (!authDataInfo.PassIsRight) {
				//������Ȩ�����Form
				FrmAuthorization frm = new FrmAuthorization(authDataInfo)  ;
				frm.BringToFront(); 
				frm.ShowDialog();
//				if (!authDataInfo.PassIsRight) {
//					MessageBox.Show("��û����ȷ����Ȩ!,�����˳�","������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);
//				}
			}
			return authDataInfo.PassIsRight ;
		}
		#endregion private function...
		/// <summary>
		/// ������������Ȩ���ַ��ܡ�
		/// </summary>
		/// <param name="autoString"></param>
		/// <returns></returns>
		public static AuthRightInfo AuthDataRight(string autoString){

			string descStr = AuthDataEncrypt.DescryptHDString(autoString);
			if(descStr==null || descStr.Length < 16)
				return null;

			string endDate = AuthDataEncrypt.ToDec(descStr.Substring(descStr.Length - 5,5)).ToString();
			descStr = descStr.Substring(0,descStr.Length - 5);

			string linkCount = AuthDataEncrypt.ToDec(descStr.Substring(descStr.Length - 5,5)).ToString() ;
			descStr = descStr.Substring(0,descStr.Length - 5);
			AuthRightInfo autoInfo = new AuthRightInfo();
			autoInfo.EndDate = DateTime.FromOADate(double.Parse(endDate));
			autoInfo.LinkCount = int.Parse(linkCount);
			autoInfo.HardDC = descStr;
           
			return autoInfo;
		}

	}
}
