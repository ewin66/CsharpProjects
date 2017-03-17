//---------------------------------------------------------------- 
// All rights reserved. 
// Author		:	chendc
// Create date	:	2003-01-04
// Description	:	AuthDataEncrypt
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Text;
using System.Security.Cryptography ;
using System.IO ;
using System.Management;

namespace MB.Aop.SoftRegistry
{
	/// <summary>
	/// AuthDataEncrypt ��Ȩ����ҵ�����ࡣ
	/// </summary>
	public class AuthDataEncrypt
	{
		#region ��������...
		public const int RAND_KEY_LEN = 4;
		private const int MAX_SERIAL_NUMBER_LEN = 16;
		private static string KEY_HD = "ufSoft";
		public const int GET_RAND_MAX = 10000;
		private static string REPLACE_CHAR = "=";
		#endregion ��������...


		#region add private construct function...
		/// <summary>
		/// add private construct function to prevent instance.
		/// </summary>
		private AuthDataEncrypt(){
		}
		#endregion add private construct function...

		/// <summary>
		/// ��ȡ���ܵ��������
		/// </summary>
		/// <returns></returns>
		public static string GetRandKey(){
			Random r = new Random();
			int rand = r.Next(0,GET_RAND_MAX);
			
			return rand.ToString();
		}
		#region Ӳ��ϵ�кż��ܽ��ܴ������...
		/// <summary>
		/// ����Ӳ��ϵ�кš�
		/// </summary>
		/// <param name="serialNumber"></param>
		/// <returns></returns>
		public static string DescryptHDString(string serialNumber){
			if(serialNumber==null || serialNumber.Length < MAX_SERIAL_NUMBER_LEN){
				//UP.Utils.TraceEx.Write("�ַ���:" + serialNumber + "��������");
				return string.Empty;
			}
			string hexRand = serialNumber.Substring(0,RAND_KEY_LEN);
			int rand = ToDec(hexRand);
			string str  =  DecryptString(serialNumber.Substring(RAND_KEY_LEN,serialNumber.Length - RAND_KEY_LEN),KEY_HD + rand.ToString()); 

			return str;
		}
		/// <summary>
		/// ����Ӳ��ϵ�кš�
		/// </summary>
		/// <param name="serialNumber"></param>
		/// <returns></returns>
		public static string EncryptHDString(string serialNumber){
			string randKey = GetRandKey();
			if(serialNumber==null || serialNumber.Length < MAX_SERIAL_NUMBER_LEN){
				//UP.Utils.TraceEx.Write("�ַ���:" + serialNumber + "��������");
				return string.Empty;
			}
			string str = EncryptString(serialNumber,KEY_HD + randKey);
			string hexStr = ToHex(int.Parse(randKey),RAND_KEY_LEN);
			return hexStr + str;

		}

		#endregion Ӳ��ϵ�кż��ܽ��ܴ������...

		#region ����Ҫ���16���ƺ�10 ����֮���ת������...
		/// <summary>
		/// ʮ����ת��Ϊ�̶����ȵ�ʮ�����ƣ�����ط��������ַ������档
		/// </summary>
		/// <param name="number"></param>
		/// <param name="fixLenth"></param>
		/// <returns></returns>
		public static  string ToHex(int number,int fixLenth){
			string hexStr = BODHConvert(number.ToString(),10,16);
			hexStr = FormatStr(hexStr, fixLenth,char.Parse(REPLACE_CHAR));
			return hexStr;
		}
		/// <summary>
		/// �������ʮ������ת��Ϊʮ���ơ�
		/// </summary>
		/// <param name="hexStr"></param>
		/// <returns></returns>
		public static int ToDec(string  hexStr){
			string hexRand = hexStr.Replace(REPLACE_CHAR,"") ;
			int rand = int.Parse(BODHConvert(hexRand,16,10));
			return rand;
		}

		#endregion ����Ҫ���16���ƺ�10 ����֮���ת������...

		/// <summary>
		/// ��ȡӲ��ϵ�к�
		/// </summary>
		/// <returns></returns>
		public static  string GetHd() {  //97 bit   after Substring  still have 40 bit
			ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher();

			wmiSearcher.Query = new SelectQuery(
				"Win32_DiskDrive", //Win32_PhysicalMedia //
				"", 
				new string[]{"PNPDeviceID"}//SerialNumber//
				);
			ManagementObjectCollection myCollection = wmiSearcher.Get();
			ManagementObjectCollection.ManagementObjectEnumerator em = myCollection.GetEnumerator();
			em.MoveNext();
			ManagementBaseObject mo = em.Current;
			string id = mo.Properties["PNPDeviceID"].Value.ToString().Trim();
			int leftSign = id.LastIndexOf(@"\");
 
			string hdStr = id.Substring(leftSign + 1,id.Length - leftSign -1);
			if(hdStr.Length >16){
				hdStr = hdStr.Substring(0,16);
			}
			else{
				if(hdStr!=null && hdStr.Length > 0){
					int llen = 16 - hdStr.Length;
					//�������16λ�Ͳ�0
					for(int i =0 ; i < llen; i++){
						hdStr +="0";
					}
				}
			}
			return hdStr;
		}

		#region �ڲ���������...
		//�����ַ���
		private  static string DecryptString(string pDecryptStr,string encr_key) {

			DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
			//����key
			PasswordDeriveBytes db = new PasswordDeriveBytes(encr_key, null);
			byte[] key = db.GetBytes(8);
			//�洢���ܺ������
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms,desc.CreateDecryptor(key, key),CryptoStreamMode.Write);
			//ȡ�����ܺ�����ݵ��ֽ���������Ǳ��浽�ļ�
			try{
				byte[] databytes =  Convert.FromBase64String(pDecryptStr);
				//��������
				cs.Write(databytes, 0, databytes.Length);
				cs.FlushFinalBlock();
				byte[] res = ms.ToArray();
				//���ؽ��ܺ�����ݣ����ﷵ�ص�����Ӧ�úͲ���pwd��ֵ��ͬ��
				return  System.Text.Encoding.UTF8.GetString(res);
			}
			catch{
				//UP.Utils.TraceEx.Write("�ַ���" + pDecryptStr + "���ܲ��ɹ���");   
				return null;
			}
		}
		//�����ַ���
		private static string EncryptString(string pEncryptedStr,string encr_key) {
			//des���м���
			DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
			//����key
			PasswordDeriveBytes db = new PasswordDeriveBytes(encr_key, null);
			byte[] key = db.GetBytes(8);
			//�洢���ܺ������
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms,desc.CreateEncryptor(key, key),CryptoStreamMode.Write);
			//ȡ��������ֽ���
			try{
				byte[] data = Encoding.UTF8.GetBytes(pEncryptedStr);
				//���м���
				cs.Write(data, 0, data.Length);
				cs.FlushFinalBlock();
				//ȡ���ܺ������
				byte[] res = ms.ToArray();
				return Convert.ToBase64String(res);
			}
			catch{
				//UP.Utils.TraceEx.Write("�ַ���" + pEncryptedStr + "���ܲ��ɹ���");   
				return null;
			}

		}
		private static string BODHConvert(string valStr, int fromBase, int toBase) {
			int intValue = Convert.ToInt32(valStr,fromBase);

			return Convert.ToString(intValue, toBase);
		}
		public  static string FormatStr(string pStr,int pLen,char pFormatChar){
			string s = pStr==null?"":pStr;
			if(s.Length > pLen){
				return pStr.Substring(0,pLen);
			}
			else if(s==""){
				for(int i =0 ;i <pLen;i++){
					s +=pFormatChar.ToString();
				}
				return s;
			}
			else if(s.Length < pLen){
				int l = pLen - s.Length;
				for(int i =0 ;i < l;i++){
					s +=pFormatChar.ToString();
				}
				return s;
			}
			return s;
		}
		#endregion �ڲ���������...
	}
}
