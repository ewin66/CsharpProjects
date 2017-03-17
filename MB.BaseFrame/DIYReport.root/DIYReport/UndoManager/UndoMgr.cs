using System;
using System.Collections;
using System.Reflection;

namespace DIYReport.UndoManager
{
	/// <summary>
	/// UndoMgr undo/redo��������
	/// </summary>
	public class UndoMgr
	{
		private Stack _RedoStack;
		private Stack _UndoStack;
		public UndoMgr()
		{
			_RedoStack = new Stack();
			_UndoStack = new Stack();

		}
		#region �Զ����¼�...
		private System.EventHandler _UndoMgrChanged;
		public event System.EventHandler UndoMgrChanged{
			add{
				_UndoMgrChanged +=value;
			}
			remove{
				_UndoMgrChanged -=value;
			}
		}
		private void onUndoMgrCHanged(System.EventArgs arg){
			if(_UndoMgrChanged!=null){
				_UndoMgrChanged(this,arg);
			}
		}
		#endregion �Զ����¼�...

		#region Public ����...
		/// <summary>
		/// �ж��Ƿ���Խ���Undo�Ĳ���
		/// </summary>
		public bool CanUndo{
			get{
				return _UndoStack.Count > 0 ;
				}
		}
		/// <summary>
		/// �ж��Ƿ���Խ���Redo�Ĳ���
		/// </summary>
		public bool CanRedo{
			get{
				return _RedoStack.Count > 0;
			}
		}
		#endregion Public ����...

		#region public ����...
		/// <summary>
		/// �Ƴ�Redo ��Undo stack �е����г�Ա
		/// </summary>
		public void Clear(){
			_RedoStack.Clear();
			_UndoStack.Clear();
		}
		/// <summary>
		/// ���û���Ʋ����Ķ����洢��Undo ��Ӧ��stack ��
		/// </summary>
		/// <param name="pAction"></param>
		public void Store(ActionInfo pAction){
			_UndoStack.Push(pAction);
			onUndoMgrCHanged(null);
		}
		public void Store(string pDescription,IList  pObjList,
			DIYReport.Interface.IActionParent pActionParent,ActionType pActType){

			Store(new ActionInfo(pDescription,pObjList,pActionParent,pActType));
			
			_RedoStack.Clear();
			
		}
		/// <summary>
		/// ��������
		/// </summary>
		public void Undo(){
			if(CanUndo){
				ActionInfo actInfo = _UndoStack.Pop() as ActionInfo ;
				ActionType aType = actInfo.ActionType ;
				switch(aType){
					case ActionType.PropertyChange :
						//setPropertyValue(actInfo.ObjList,actInfo.PropertyList);
						IList  pro = actInfo.ObjList;
						//_RedoStack.Push( actInfo);
						actInfo.ListenerParent.SetPropertyValue( ref pro); 
						_RedoStack.Push( new ActionInfo(actInfo.Description,pro,actInfo.ListenerParent ,actInfo.ActionType));
						break;
					case ActionType.Add :
						_RedoStack.Push( actInfo);
						actInfo.ListenerParent.Remove(actInfo.ObjList);   
						break;
					case ActionType.Remove  :
						_RedoStack.Push( actInfo);
						actInfo.ListenerParent.Add(actInfo.ObjList);   
						break;
					default:
						//Debug 
						break;

				}
				onUndoMgrCHanged(null);
				//_RedoStack.Push( actInfo);
			}
		}
		/// <summary>
		/// �ظ�����
		/// </summary>
		public void Redo(){
			if(CanRedo ){
				ActionInfo actInfo = _RedoStack.Pop() as ActionInfo ;
				ActionType aType = actInfo.ActionType ;
				switch(aType){
					case ActionType.PropertyChange :
						//setPropertyValue(actInfo.ObjList,actInfo.PropertyList);
						IList  pro = actInfo.ObjList;
						actInfo.ListenerParent.SetPropertyValue( ref pro ); 
						_UndoStack.Push( new ActionInfo(actInfo.Description,pro,actInfo.ListenerParent ,actInfo.ActionType));
						break;
					case ActionType.Add :
						_UndoStack.Push( actInfo);
						actInfo.ListenerParent.Add (actInfo.ObjList);   
						break;
					case ActionType.Remove  :
						_UndoStack.Push( actInfo);
						actInfo.ListenerParent.Remove(actInfo.ObjList);   
						break;
					default:
						//Debug 
						break;

				}
				onUndoMgrCHanged(null);
			}
		}

		#endregion public ����...
	}
}
