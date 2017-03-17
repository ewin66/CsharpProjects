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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DIYReport.ReportModel
{
	/// <summary>
	/// RptSection �����section������
	/// </summary>
	public class RptSection
	{
		#region ��������...
		private string _Name;
		private SectionType _SectionType;
		private Point  _Location;
		private int _Height;
		private int _Width;
		private bool _Visibled = true;
		private bool _IsEndUpdate = true;

		private RptSingleObjList _RptObjList;

		private RptReport _Report;

		//���鴦��������ֶε����ƣ���ָ���ҳü��ҳ����������õģ�
		private int _Index; //Section �ڼ����е�˳��λ��
		private DIYReport.GroupAndSort.RptFieldInfo   _GroupField;

		//�жϸ�����Section �Ƿ��Ѿ�������Ӧ��Design Section 
		private bool _HasCreateViewDesign; 
		
		//�ڻ��Ʊ���ʱ�������Ŀ�ʼλ��
		private int _DrawLocationY = 0;
		#endregion ��������...

		#region �Զ����¼�...
		private RptEventHandler _AfterValueChanged;
		public event RptEventHandler AfterValueChanged{
			add{
				_AfterValueChanged +=value;
			}
			remove{
				_AfterValueChanged -=value;
			}
		}
		protected void OnAfterValueChanged(RptEventArgs arg){
			if(_AfterValueChanged!=null){
				_AfterValueChanged(this,arg);
			}

		}
		#endregion �Զ����¼�...

		#region ���캯��...
		public RptSection() : this(DIYReport.SectionType.Detail){
		}
		/// <summary>
		///  �������ʹ���һ���µ�Section 
		/// </summary>
		/// <param name="pType">Section ������</param>
		public RptSection(DIYReport.SectionType pType) : this(pType,null){
		}
		public RptSection(DIYReport.SectionType pType,DIYReport.GroupAndSort.RptFieldInfo  pGroupField){
			_SectionType = pType;
			_GroupField = pGroupField;
			_RptObjList = new RptSingleObjList(this);
			_Height = RptReport.DEFAULT_SELCTION_HEIGHT ;
			_Visibled = true;
			if(_GroupField!=null)
				_Name = DIYReport.PublicFun.GetTextBySectionType( pType) + "GROUP BY " + _GroupField.FieldName;
			else
				_Name = DIYReport.PublicFun.GetTextBySectionType( pType);

		}
		#endregion ���캯��...
		
		#region ��չ��Public ����...
		public void BeginUpdate(){
			_IsEndUpdate = false;
		}
		public void EndUpdate(){
			_IsEndUpdate = true;
			OnAfterValueChanged(new RptEventArgs());
		}
		#endregion ��չ��Public ����...
		
		#region public ����...
		[Browsable(false)]
		public int Index{
			get{
				return _Index;
			}
			set{
				_Index = value;
			}
		}

		[Browsable(false)]
		public DIYReport.GroupAndSort.RptFieldInfo GroupField{
			get{
				return _GroupField;
			}
			set{
				_GroupField = value;
			}
		}
		[Browsable(false)]
		public RptReport Report{
			get{
				return _Report;
			}
			set{
				_Report = value;
			}
		}
		[Browsable(false)]
		public RptSingleObjList RptObjList{
			get{
				return _RptObjList;
			}
			set{
				_RptObjList = value;
			}
		}
		[ReadOnly(true),Description("���û��ߵõ�Section �Ŀ��"),Category("���")]
		public int Width{
			get{
				return _Width;
			}
			set{
				_Width = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
        private Image _BackgroundImage; 
        [Description("���û��ߵõ�Ҫ��ӡ��ͼƬ��"), Category("����")]
        public Image BackgroundImage {
            get {
                return _BackgroundImage;
            }
            set {
                
                if (_BackgroundImage != null) {
                    _BackgroundImage.Dispose();
                }
                if (value != null) {
                    Bitmap bit = new Bitmap(value);

                    System.Drawing.Image img = System.Drawing.Image.FromHbitmap(bit.GetHbitmap());
                    _BackgroundImage = img;
                    value.Dispose();
                } else {
                    _BackgroundImage = null;
                }
                if (_IsEndUpdate) { OnAfterValueChanged(new DIYReport.ReportModel.RptEventArgs()); }
            }
        }
		[Description("���û��ߵõ�Section �ĸ߶�"),Category("���")]
		public int Height{
			get{
				return _Height;
			}
			set{
				_Height = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false),Description("�Ƿ���ʾ��Section"),Category("��Ϊ")]
		public bool Visibled{
			get{
				return _Visibled;
			}
			set{
				_Visibled = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[ReadOnly(true),Description("��Section ������"),Category("��Ϊ")]
		public SectionType SectionType{
			get{
				return _SectionType;
			}
			set{
				_SectionType = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false)]
		public Point Location{
			get{
				return _Location;
			}
			set{
				_Location = value;
			}
		}
		[Browsable(false),Description("���û��ߵõ���Section������"),Category("����")]
		public string Name{
			get{
				return _Name;
			}
			set{
				_Name = value;
				if(_IsEndUpdate){OnAfterValueChanged(new RptEventArgs());}
			}
		}
		[Browsable(false)]
		public bool HasCreateViewDesign{
			get{
				return _HasCreateViewDesign;
			}
			set{
				_HasCreateViewDesign = value;
			}
		}//
		[Browsable(false)]
		public int DrawLocationY{
			get{
				return _DrawLocationY;
			}
			set{
				_DrawLocationY = value;
			}
		}
		#endregion public ����...
	}
}
