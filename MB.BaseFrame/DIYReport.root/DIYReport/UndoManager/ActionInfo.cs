using System;
using System.Collections ; 

namespace DIYReport.UndoManager
{
	/// <summary>
	/// ActionInfo �洢ÿ�β�������ϸ��������
	/// </summary>
	public class ActionInfo
	{
		#region �ڲ���������...
		private string _Description;
		private IList _ObjList;
		private DIYReport.Interface.IActionParent _ListenerParent;
		private ActionType _ActionType;
		#endregion �ڲ���������...

		#region ���캯��...

		#region ���ǻ���ķ���...
		public override string ToString() {
			return _Description;
		}

		#endregion ���ǻ���ķ���...

		public ActionInfo()
		{
			 
		}
		public ActionInfo(string pDesc,IList pObjList,DIYReport.Interface.IActionParent pListenerParent,
						   ActionType pActionType){
			_Description = pDesc;
			_ObjList = pObjList;
			_ListenerParent = pListenerParent;
			_ActionType = pActionType;

		}
		#endregion ���캯��...

		#region Public ����...
		public string Description{
			get{
				return _Description;
			}
			set{
				_Description = value;
			}
		}
		public IList ObjList{
			get{
				return _ObjList;
			}
			set{
				_ObjList = value;
			}
		}
		public DIYReport.Interface.IActionParent ListenerParent{
			get{
				return _ListenerParent;
			}
			set{
				_ListenerParent = value;
			}
		}
		public ActionType ActionType{
			get{
				return _ActionType;
			}
			set{
				_ActionType = value;
			}
		}
		#endregion Public ����...

	}
}
