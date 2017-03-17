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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel.RptObj
{
	/// <summary>
	/// ExpressBox �����ֶΡ������ͱ�
	/// </summary>
	public class RptExpressBox : DIYReport.ReportModel.RptTextObj {
		private string _DataSource;
		private ExpressType _ExpressType;
		private string _FieldName;
		//�ڲ������ֶ�
		private string _BingDBFieldName;
		private string _ExpressValue;
		/// <summary>
		/// 
		/// </summary>
		public RptExpressBox() : this(null){
		}
		//private readonly string NO_BING_TAG = "[δ��]";
		public RptExpressBox(string pName):this(pName,null){
		}
		public RptExpressBox(string pName,string pDataSource) : base(pName,DIYReport.ReportModel.RptObjType.Express) {
			if(pDataSource==null){
				_DataSource = DIYReport.Drawing.RptDrawHelper.NO_BING_TAG ; 
			}
			else{
				_DataSource = pDataSource; 
			}
	    }
		#region ICloneable Members

		public  override object Clone() {
			object info = this.MemberwiseClone() as object ;
			return info;
		}
 
		#endregion ICloneable Members

		#region public ����...
		[Browsable(false)]
		public string BingDBFieldName{
			get{
				//Ϊ�˼����ϰ汾�ı����ʽ����Ƴ����ַ�ʽ����������ϳ������⣬�ٽ�һ���޸�
				string temp ;
				if(_BingDBFieldName==null || _BingDBFieldName==""){
					if(_ExpressType == ExpressType.Express){
						temp =  _FieldName ;
					}
					else{
						temp = _DataSource;
					}
				}
				else{
					if(_BingDBFieldName == _FieldName || _BingDBFieldName == _DataSource){
						if(this.Section!=null && this.Section.Report!=null){ 
							string fName = DIYReport.PublicFun.GetFieldNameByDesc(_FieldName,this.Section.Report.DesignField); 
							_BingDBFieldName = fName;
						}
					}
					temp =  _BingDBFieldName;
				}
				return temp;
			}
			set{
				_BingDBFieldName = value;
			}
		}
		[Browsable(false)]
		public string ExpressValue{
			get{
				return _ExpressValue;
			}
			set{
				_ExpressValue = value;
			}
		}
		[Description("��ѡ��ͳ�Ƶ�ʱ�����û��ߵõ�ͳ���ֶε����ơ�"),Category("����"),Editor(typeof(DIYReport.Design.RptFieldAttributesEditor), typeof(UITypeEditor))]
		public string FieldName{
			get{
				return _FieldName;
			}
			set{
				_FieldName = value;
				if(DIYReport.UserDIY.DesignEnviroment.IsUserDesign){ 
					//Ϊ�˼����ϰ汾�ı����ʽ����Ƴ����ַ�ʽ����������ϳ������⣬�ٽ�һ���޸�
					if(this.Section!=null && this.Section.Report!=null){ 
						string fName = DIYReport.PublicFun.GetFieldNameByDesc(_FieldName,this.Section.Report.DesignField); 
						_BingDBFieldName = fName;
					}
				}
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}

		[Description("���û��ߵõ�����Դ�����ơ�"),Category("����"),Editor(typeof(DIYReport.Design.RptDataSourceEditor), typeof(UITypeEditor))]
		public string DataSource{
			get{
				return _DataSource;
			}
			set{
				_DataSource = value;
				if(DIYReport.UserDIY.DesignEnviroment.IsUserDesign){ 
					//Ϊ�˼����ϰ汾�ı����ʽ����Ƴ����ַ�ʽ����������ϳ������⣬�ٽ�һ���޸�
					if(_ExpressType== ExpressType.Field){
						if(this.Section!=null && this.Section.Report!=null){ 
							string fName = DIYReport.PublicFun.GetFieldNameByDesc(_FieldName,this.Section.Report.DesignField); 
							_BingDBFieldName = fName;
						}
					}
					if(_ExpressType!= ExpressType.Express){
						_FieldName = "";
					}
				}
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
			}
		}
		[Browsable(true),Description("���û��ߵõ�����Դ�����͡�"),Category("����")]
		public ExpressType ExpressType{
			get{
				return _ExpressType;
			}
			set{
				_ExpressType = value;
				if(DIYReport.UserDIY.DesignEnviroment.IsUserDesign){ 
					_FieldName = "";
					_DataSource = DIYReport.Drawing.RptDrawHelper.NO_BING_TAG; 
				}
				if(IsEndUpdate){base.OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs());} 
				
			}
		}

		#endregion public ����...
	}
}
