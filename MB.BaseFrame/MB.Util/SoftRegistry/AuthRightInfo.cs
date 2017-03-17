//---------------------------------------------------------------- 
// All rights reserved. 
// Author		:	chendc
// Create date	:	2003-01-04
// Description	:	AuthRightInfo
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Text;

namespace MB.Aop.SoftRegistry
{
	/// <summary>
	/// AuthRightInfo Ȩ�޼�����������Ϣ��
	/// </summary>
	public class AuthRightInfo {

		#region ��������...
		//�ж���Ȩ�Ƿ�ͨ��
		private bool _PassIsRight;
		//���ʹ�õ����ʱ��
		private DateTime _EndDate;
		//��Ȩ�����Ӹ���
		private int _LinkCount;
		//Ȩ�޼����ļ�������
		private string _SNFileName;
		//��ǰ��Ӳ��ID
		private string _HardDC;
		//ʹ�ô���
		private int _UserCount;
		private ClientAppType _ClientAppType;
		#endregion ��������...
		
		#region ���캯��...
		/// <summary>
		/// ���캯��...
		/// </summary>
		public AuthRightInfo() {
			//_ClientAppType = CSServer.Utils.ClientAppType.UP2;
		}	
		#endregion ���캯��...

		#region ���ǻ���ķ���...
		/// <summary>
		/// ��ȡ�������ַ��ܡ�
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			string dateStr = _EndDate.ToString("yy-MM-dd"); 
			StringBuilder msgStr = new StringBuilder();
			if(_LinkCount > 0)
				msgStr.Append("��" + _LinkCount.ToString() + "�û�����Ȩ;");
			msgStr.Append("ʹ�ý�ֹ��Ϊ" + dateStr.Substring(0,2) + "��"+ dateStr.Substring(3,2) + 
						"��" + dateStr.Substring(6,2) + "��");
			if(_UserCount > 0)
				msgStr.Append("ʹ�ô���Ϊ��" + _UserCount.ToString());
			return msgStr.ToString();
		}

		#endregion ���ǻ���ķ���...

		#region Public����...
		public bool PassIsRight {
			get{
				return _PassIsRight;
			}
			set{
				_PassIsRight = value;
			}
		}
		public DateTime EndDate {
			get{
				return _EndDate;
			}
			set{
				_EndDate = value;
			}
		}
		public int LinkCount {
			get{
				return _LinkCount;
			}
			set{
				_LinkCount = value;
			}
		}
		public string SNFileName{
			get{
				return _SNFileName;
			}
			set{
				_SNFileName = value;
			}
		}
		public string HardDC{
			get{
				return _HardDC;
			}
			set{
				_HardDC = value;

			}
		}
		public ClientAppType ClientAppType{
			get{
				return _ClientAppType;
			}
			set{
				_ClientAppType = value;

			}
		}
		public int UserCount {
			get{
				return _UserCount;
			}
			set{
				_UserCount = value;
			}
		}
		#endregion Public����...
	}
}

