/**///<summary>--------------------------------------------------- 
/// Copyright (C)2004-2004 nick chen (nickchen77@gmail.com)
/// All rights reerved. 
/// 
/// Author		:	Nick
/// Create date	:	2004-07-17
/// Description	:	Ini �ļ������ࡣ
/// Modify date	:			By:					Why: 
///</summary>-----------------------------------------------------
using System; 
using System.IO; 
using System.Runtime.InteropServices; 
using System.Text;

namespace DIYReport{
	/// <summary>
	/// Ini �ļ������ࡣ
	/// </summary>
����public class IniFile {

	  //дINI�ļ�
	  [ DllImport("kernel32") ]
	  private static extern bool WritePrivateProfileString ( string section ,string key , string val , string filePath );
����//��ini�ļ����ַ�
	  [ DllImport("kernel32") ]
	  private static extern int GetPrivateProfileString ( string section ,string key , string def , StringBuilder retVal ,int size , string filePath ); 
����//��ini�ļ�������
	  [ DllImport("kernel32") ]
	  private static extern int GetPrivateProfileInt( string section ,string key , int def , string filePath );

	  //Ĭ������
	  public static  int MAX_STR_LENGTH = 1024 * 4;
	  #region ���캯��...
	  /// <summary>
	  /// private construct function to prevent instance.
	  /// </summary>
	  private  IniFile(){
		  
	  } 
	  #endregion ���캯��...

	  #region Public static ����...
	  /// <summary>
	  /// ��ȡ���ݡ�
	  /// </summary>
	  /// <param name="section"></param>
	  /// <param name="key"></param>
	  /// <param name="def">ȱʡ��ֵ</param>
	  /// <returns></returns>
	  public static string ReadString(string section,string key,string defaultVal,string fullFileName){
		  StringBuilder temp = new StringBuilder(MAX_STR_LENGTH); 
		  GetPrivateProfileString(section,key,defaultVal,temp,MAX_STR_LENGTH,fullFileName); 
		  return temp.ToString().Trim(); 
	  } 
	  /// <summary>
	  /// д����
	  /// </summary>
	  /// <param name="section"></param>
	  /// <param name="key"></param>
	  /// <param name="strVal"></param>
	  public static void WriteString(string section,string key,string strVal,string fullFileName){
		  WritePrivateProfileString(section,key,strVal,fullFileName); 
	  } 
	  /// <summary>
	  /// ɾ����ֵ��
	  /// </summary>
	  /// <param name="section"></param>
	  /// <param name="key"></param>
	  public static void DelKey(string section,string key,string fullFileName) { 
		  WritePrivateProfileString(section,key,null,fullFileName); 
	  }
	  /// <summary>
	  /// ɾ��Section��
	  /// </summary>
	  /// <param name="section"></param>
	  public static void DelSection(string section,string fullFileName) { 
		  WritePrivateProfileString(section,null,null,fullFileName); 
	  }
	  #endregion Public static ����...
  }
 
}
