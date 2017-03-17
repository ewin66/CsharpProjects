//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2004-12-15
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Collections ;

namespace DIYReport{
	/// <summary>
	/// Up2BaseCollectionͨ���̳�ArrayList����һ������Ļ���.
	/// </summary>
	[Serializable]
	public abstract class Hashlist : IDictionary, IEnumerable {
		/// <summary>
		/// �洢��ֵ������
		/// </summary>
		protected ArrayList mKeys = new ArrayList();
		/// <summary>
		///  �洢ֵ��HashTable 
		/// </summary>
		protected Hashtable mValues = new Hashtable();		

		#region ICollection implementation
		//ICollection implementation
		/// <summary>
		/// ���������
		/// </summary>
		public int Count {
			get{return mValues.Count;}
		}
		/// <summary>
		/// �Ƿ�ͬ���Լ��ϵķ���
		/// </summary>
		public bool IsSynchronized {
			get{return mValues.IsSynchronized;}
		}
		/// <summary>
		/// ��ȡ��ͬ����HashTable�ķ��ʶ���
		/// </summary>
		public object SyncRoot {
			get{return mValues.SyncRoot;}
		}
		/// <summary>
		/// ��hashTable�еĶ�����һά������ָ����λ��
		/// </summary>
		/// <param name="oArray"></param>
		/// <param name="iArrayIndex"></param>
		public void CopyTo(System.Array oArray, int iArrayIndex) {
			mValues.CopyTo(oArray, iArrayIndex);
		}
		#endregion

		#region IDictionary implementation

		/// <summary>
		///  ��ָ����λ�ò������
		/// </summary>
		/// <param name="pIndex"></param>
		/// <param name="pKey"></param>
		/// <param name="pValue"></param>
		public void Insert(int pIndex ,object pKey , object pValue){
			mKeys.Insert(pIndex,pKey);  
			mValues.Add(pKey,pValue);
		}
		//IDictionary implementation
		/// <summary>
		///  ���Ӷ��󵽼�����
		/// </summary>
		/// <param name="oKey"></param>
		/// <param name="oValue"></param>
		public void Add(object oKey, object oValue) {
			mKeys.Add(oKey);
			mValues.Add(oKey, oValue);
		}
		/// <summary>
		/// ���ü�ֵ�Ĺ̶���С
		/// </summary>
		public bool IsFixedSize {
			get{return mKeys.IsFixedSize;}
		}
		/// <summary>
		/// �õ�һ��ֵ����ֵָʾ ArrayList�Ƿ�Ϊֻ��
		/// </summary>
		public bool IsReadOnly {
			get{return mKeys.IsReadOnly;}
		}
		/// <summary>
		/// �õ������е����м�ֵ
		/// </summary>
		public ICollection Keys {
			get{return mValues.Keys;}
		}
		/// <summary>
		/// ��������е�����ֵ�Լ���
		/// </summary>
		public void Clear() {
			mValues.Clear();
			mKeys.Clear();
		}
		/// <summary>
		/// �ж�ָ���ļ��Ƿ��ڼ�����
		/// </summary>
		/// <param name="oKey"></param>
		/// <returns></returns>
		public bool Contains(object oKey) {
			return mValues.Contains(oKey);
		}
		/// <summary>
		/// �ж�ָ���ļ��Ƿ��ڼ�����
		/// </summary>
		/// <param name="oKey"></param>
		/// <returns></returns>
		public bool ContainsKey(object oKey) {
			return mValues.ContainsKey(oKey);
		}
		/// <summary>
		/// ���ؿ�ѭ�����ʵĽӿ�
		/// </summary>
		/// <returns></returns>
		public IDictionaryEnumerator GetEnumerator() {
			return mValues.GetEnumerator();
		}	
		/// <summary>
		/// �Ƴ�����ָ������Ԫ��
		/// </summary>
		/// <param name="oKey"></param>
		public  void Remove(object oKey) {
			mValues.Remove(oKey);
			mKeys.Remove(oKey);
		}
		/// <summary>
		/// ���ݼ��õ�ָ����Ԫ��
		/// </summary>
		public object this[object oKey] {
			get{return mValues[oKey];}
			set{mValues[oKey] = value;}
		}
		/// <summary>
		/// �õ�����ֵ�ļ���
		/// </summary>
		public ICollection Values {
			get{return mValues.Values;}
		}
		#endregion

		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator() {
			return mValues.GetEnumerator();
		}
		#endregion
		
		#region Hashlist specialized implementation
		//specialized indexer routines
		/// <summary>
		/// ���ݽ��õ�ָ����Ԫ��
		/// </summary>
		public object this[string Key] {
			get{return mValues[Key];}
		}
		/// <summary>
		///  ����Index�õ�ָ����Ԫ��
		/// </summary>
		public object this[int Index] {
			get{return mValues[mKeys[Index]];}
		}
		#endregion
	
	}
}
