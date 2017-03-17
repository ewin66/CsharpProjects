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
using System.Drawing ;
using System.Xml ;
using System.Collections ;
using System.Diagnostics ;

using DIYReport.Interface;
namespace DIYReport.ReportModel
{
	#region �Ƚ���...
	class RptObjSortComparer : IComparer  {

		//  
		int IComparer.Compare( Object x, Object y )  {
			RptSingleObj xObj = x as RptSingleObj;
			RptSingleObj yObj = y as RptSingleObj;
			return( (new CaseInsensitiveComparer()).Compare( xObj.Location.X ,yObj.Location.X ) );
		}

	}
	class RptObjSortByTopComparer : IComparer  {
		int IComparer.Compare( Object x, Object y )  {
			RptSingleObj xObj = x as RptSingleObj;
			RptSingleObj yObj = y as RptSingleObj;
			return( (new CaseInsensitiveComparer()).Compare( xObj.Location.Y ,yObj.Location.Y ) );
		}

	}
	#endregion �Ƚ���...
	/// <summary>
	/// RptSingleObjList  ����ÿһ��Section �е�
	/// </summary>
	public class RptSingleObjList : ArrayList{
		private float mHeight;
		private RptSection _Section;

		#region ���캯��...
		public RptSingleObjList(){
		}
		public  RptSingleObjList( RptSection pSection){
			_Section = pSection;
		}

		#endregion ���캯��...

		#region this...
		public new IRptSingleObj this[int pIndex] {
			get {
				return  base[pIndex] as IRptSingleObj;
			}
		}
		#endregion this...

		#region Public ����(λ�ô�С�������)...		
		/// <summary>
		/// ��ָ����λ�ò���һ������
		/// </summary>
		/// <param name="pRptObj"></param>
		/// <param name="pLocation"></param>
		/// <returns></returns>
		public RptSingleObj InsertByLocation(RptSingleObj pRptObj,Point pLocation){
			foreach(RptSingleObj obj in this){
				if(obj.Location.X >=pLocation.X){
					//�ڿؼ������ָ��λ�õ���߲���Ҫ�������ұߵ��������ؼ���λ��
					obj.Location = new Point(obj.Location.X + pRptObj.Size.Width,obj.Location.Y);

				}
			}
			this.Add(pRptObj);
			return pRptObj;
		}

		/// <summary>
		/// �Զ�����Section �и��ؼ�(��Ҫ�������ؼ���ӡ������������ǵ���ͷ�����������ʱ����Ҫʹ�ã�
		/// �����ӡ���ݵ����ұ�С�ڱ���Ŀ�ȣ���ô����Ҫ������ 
		/// </summary>
		public void AutoFitSizeWidth(){
			int maxRight = GetMaxCtlRight();
			int sectionWidth = _Section.Width;
			int ctlCount = this.Count ;
			//���ÿؼ�����������
			this.Sort(new RptObjSortComparer());
			if(maxRight > sectionWidth){
				int setWidth = maxRight - sectionWidth;
				int moveSize = 0;
				for(int i = 0;i <ctlCount;i++){
					RptSingleObj obj = this[i] as RptSingleObj;
					obj.Location = new Point(obj.Location.X - moveSize,obj.Location.Y);
					int sm = System.Convert.ToInt32(obj.Size.Width * setWidth / maxRight)   ;
					moveSize += sm;
					obj.Size = new Size(obj.Size.Width - sm,obj.Size.Height);
				}
			}
		}
		/// <summary>
		/// ͨ���ؼ���Top�����˳�����пؼ�
		/// </summary>
		public void SortByTopOrder(){
			this.Sort(new RptObjSortByTopComparer()); 
		}
		#endregion Public ����(λ�ô�С�������)...

		#region Public ����(��ȡ��Ϣ���)...
		/// <summary>
		/// �õ��ؼ���Section �����ұߵ�Right,�����������Ʊ���Ŀ��
		/// </summary>
		/// <returns></returns>
		public int GetMaxCtlRight(){
			int right = 0;
			foreach(RptSingleObj obj in this){
				int newbo = obj.Rect.Right;
				right = right > newbo ? right: newbo ;
			}
			return right;
		}
		/// <summary>
		/// �õ��ؼ���Section ������µ�Bottom,������������Section ����С�߶�
		/// </summary>
		/// <returns></returns>
		public int GetMaxCtlBottom(){
			int bottom = 0;
			foreach(RptSingleObj obj in this){
				int newbo = obj.Rect.Bottom ;
				bottom = bottom > newbo ? bottom: newbo ;
			}
			return bottom;
		}
		#endregion Public ����(��ȡ��Ϣ���)...

		#region Public ����(�༭���)...
		/// <summary>
		/// ͨ��ָ������������һ��
		/// </summary>
		/// <param name="pType"></param>
		/// <returns></returns>
		public IRptSingleObj AddByType( DIYReport.ReportModel.RptObjType pType,DIYReport.ReportModel.RptSection pSection){
			return AddByType(pType,null,pSection);
		}
		public IRptSingleObj AddByType( DIYReport.ReportModel.RptObjType pType,string pDispText,DIYReport.ReportModel.RptSection pSection){
			DIYReport.Interface.IRptSingleObj obj=  DIYReport.RptObjectHelper.CreateObj(pType,pDispText);
			if(obj==null)
				return null;

			obj.Name = getObjNewName(obj.GetType().Name); 
			obj.Section = pSection;
			base.Add(obj);
			return obj;
		}
		public IRptSingleObj Add(IRptSingleObj pClass) {
			base.Add(pClass);
			 
			return pClass;
		}
		#endregion Public ����(�༭���)...

		#region Public ����...

		public float Height {
			get {
				return mHeight;
			}
			set {
				mHeight = value;
			}
		}
		#endregion Public ����...

		#region �ڲ�������...
		private string getObjNewName(string pBegName){
			string newName = pBegName + "0";
			int i = 1;
			while(i<=this.Count){
				newName = pBegName + i.ToString();
				if(getObjByName(newName)==null){
					break;
				}
				i++;
			}
			return newName;
		}
		private DIYReport.Interface.IRptSingleObj getObjByName(string pName){
			foreach(DIYReport.Interface.IRptSingleObj obj in this){
				if(System.String.Compare( obj.Name ,pName ,true)==0){
					return obj;
				}
			}
			return null;
		}
		#endregion �ڲ�������...
	}
 
}
