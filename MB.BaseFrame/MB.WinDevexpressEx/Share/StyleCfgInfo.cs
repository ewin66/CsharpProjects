//---------------------------------------------------------------- 
// Author		:	Nick
// Create date	:	2009-02-13
// Description	:	StyleCfgInfo ����UI Style��
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;


namespace MB.XWinLib.Share
{
	/// <summary>
	/// StyleCfgInfo ����UI Style��
	/// </summary>
	public class StyleCfgInfo {
		#region ��������...
		private string _Name;
		private System.Drawing.Color _BackColor;
		private System.Drawing.Color _ForeColor;
		private System.Drawing.Font _Font;
		#endregion ��������...


		#region ���캯��...
		/// <summary>
		/// ���캯��.
		/// </summary>
		/// <param name="name"></param>
		public StyleCfgInfo(string name) {
			_Name = name;
			_BackColor = System.Drawing.Color.White;
			_ForeColor = System.Drawing.Color.Black;
			_Font = new System.Drawing.Font("Microsoft Sans Serif",9.0f,System.Drawing.FontStyle.Regular );
		}
		#endregion ���캯��...

		#region Public ����...
		public string Name{
			get{
				return _Name;
			}
			set{
				_Name = value;
			}
		}
		public  System.Drawing.Color BackColor{
			get{
				return _BackColor;
			}
			set{
				_BackColor = value;
			}
		}
		public  System.Drawing.Color ForeColor{
			get{
				return _ForeColor;
			}
			set{
				_ForeColor = value;
			}
		}
		public  System.Drawing.Font Font{
			get{
				return _Font;
			}
			set{
				_Font = value;
			}
		}
		#endregion Public ����...
	}
}
