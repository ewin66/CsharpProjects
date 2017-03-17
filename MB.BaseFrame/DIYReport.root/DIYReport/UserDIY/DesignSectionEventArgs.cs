using System;

namespace DIYReport.UserDIY
{
	/// <summary>
	///  �¼�ί������
	///  ��ע��������ȷ��֪��������ò�Ҫ��������¼��Ĳ��� 
	/// </summary>
	public delegate void DesignSectionEventHandler(object sender,DesignSectionEventArgs  e);
	/// <summary>
	/// DesignSectionEventArgs Design Section �¼����塣
	/// </summary>
	public class DesignSectionEventArgs : System.EventArgs 
	{
		#region ��������...
		//��Ƶ�Sction 
		private DesignSection _Section;
		//����Section �ڼ����е�λ��
		private int _InsertPosition;
		#endregion ��������...

		#region ���캯��...
		public DesignSectionEventArgs()
		{
		}
		public DesignSectionEventArgs(int pInsertPosition,DesignSection pSection) {
			_InsertPosition = pInsertPosition;
			_Section = pSection;
		}
		#endregion ���캯��...

		#region Public ����...
		public DesignSection Section{
			get{
				return  _Section;
			}
			set{
				_Section = value;
			}
		}
		public int InsertPosition{
			get{
				return _InsertPosition;
			}
			set{
				_InsertPosition = value;
			}
		}
		#endregion Public ����...
	}
}
