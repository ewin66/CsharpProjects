//---------------------------------------------------------------- 
// Copyright (C) 2008-2009 www.metersbonwe.com
// All rights reserved. 
// Author		:	chendc
// Create date	:	2009-02-18
// Description	:	��չ DevExpress.XtraGrid.GridControl �ؼ�,��Ҫ��Ϊ���ṩ�� XtraGridEx ʹ�õġ�
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Grid.Handler;
using DevExpress.XtraGrid.Menu ;
using MB.WinBase.Common;
using DevExpress.XtraGrid.Columns;
using MB.WinBase;

namespace MB.XWinLib.XtraGrid{
	/// <summary>
	/// ��չ DevExpress.XtraGrid.GridControl �ؼ�,��Ҫ��Ϊ���ṩ�� XtraGridEx ʹ�õġ�
	/// </summary>
	[ToolboxItem(true)]
	public class GridControlEx : DevExpress.XtraGrid.GridControl {
        public static Color ODD_ROW_BACK_COLOR = Color.FromArgb(245, 245, 250);   
		#region ��������...
		private bool _IsDesign;
		private bool _ValidedDeleteKeyDown;//�ж���
		private DevExpress.XtraGrid.Views.Grid.GridView _GridView;
		private int _LocationIndex = 0;
		private int _FirstDockHandle;
		private GridViewOptionsInfo _CurrentInfo;
		private System.Windows.Forms.MenuItem _OptionMenu;
        private System.Windows.Forms.MenuItem _CopyCellMenu;

		private bool _ShowOptionMenu;
		private string _OptionsCFGName = string.Empty;
		private const int FIRST_LOCATION_TOP = 15;
		private const int MAX_WAIT_TIME = 2000;
		private Form _ParentFrm = null;
		private Form _ParentMdiFrm;
		
		private bool _HasInitilizeGridLoad; //���������������Gird�ĳ�ʼ����ִ��������
		private  IGridExDataIOControl _DataIOControl;
        private string _XmlCfgFileName;
        private Dictionary<string, ColumnEditCfgInfo> _ColEidtPros; //�༭�е�����

        private System.Guid _InstanceIdentity;
		#endregion ��������...

		#region ��չ���Զ����¼��������...
		private GridControlExMenuEventHandle _BeforeContextMenuClick;
		public event GridControlExMenuEventHandle BeforeContextMenuClick{
			add{
				_BeforeContextMenuClick +=value;
			}
			remove{
				_BeforeContextMenuClick -=value;
			}
		}
		public void OnBeforeContextMenuClick(GridControlExMenuEventArg arg){
			if(_BeforeContextMenuClick!=null)
				_BeforeContextMenuClick(this,arg);
		}

        
        public event EventHandler DefaultViewColumnsCleared;

        public void OnDefaultViewColumnsCleared()
        {
            if(DefaultViewColumnsCleared!=null)
            {
                DefaultViewColumnsCleared(this, new EventArgs());
            }
        }

		#endregion ��չ���Զ����¼��������...

		#region ���캯��...
		/// <summary>
		/// ���캯��
		/// </summary>
		public GridControlEx(){
          

            _IsDesign = MB.Util.General.IsInDesignMode();
			if(!_IsDesign){
				//�����н׶����ñ��ػ�����Ӧ��
                //DevExpress.XtraGrid.Localization.GridLocalizer.Active = MB.XWinLib.Localization.GridLocal.Create();
			}
			//_IgnoreKeyOnDataImport = true;

            
		}
		#endregion ���캯��...

		#region ���ǻ���ķ���...
        protected override void OnLoaded() {
            base.OnLoaded();
            if (_IsDesign) return;
            if (base.ContextMenu == null) {
                XtraContextMenu xMenu = new XtraContextMenu(this,XtraContextMenuType.Copy | XtraContextMenuType.Export | XtraContextMenuType.ExportAsTemplet |
                    XtraContextMenuType.DataImport | XtraContextMenuType.SaveGridState | XtraContextMenuType.ColumnsAllowSort |
                    XtraContextMenuType.Chart | XtraContextMenuType.ChartDesign);
                base.ContextMenu = xMenu.GridContextMenu;
            }

            if (_HasInitilizeGridLoad || !_ShowOptionMenu) return;

            _HasInitilizeGridLoad = true;
            _ParentFrm = MB.WinBase.ShareLib.Instance.GetControlParentForm(this);
            if (_ParentFrm == null)
                return;

            MB.WinBase.IFace.IForm viewFrm = _ParentFrm as MB.WinBase.IFace.IForm;
            if (viewFrm != null && viewFrm.ClientRuleObject != null) {
                _XmlCfgFileName = viewFrm.ClientRuleObject.ClientLayoutAttribute.UIXmlConfigFile;
                _OptionsCFGName = viewFrm.GetType().FullName + "~" + viewFrm.ClientRuleObject.GetType().FullName + "~" + this.Name + ".xml";
                var layOutXmlHelper = LayoutXmlConfigHelper.Instance;
                var colPros = layOutXmlHelper.GetColumnPropertys(_XmlCfgFileName);
                var colEditPros = layOutXmlHelper.GetColumnEdits(colPros, _XmlCfgFileName);
            }
            else
                _OptionsCFGName = _ParentFrm.GetType().FullName + "~" + this.Name + ".xml";

            _CurrentInfo = GridViewOptionsHelper.Instance.CreateFromXMLToInfo(_OptionsCFGName);

            _GridView = this.DefaultView as DevExpress.XtraGrid.Views.Grid.GridView;

            if (_GridView != null) {
                _GridView.OptionsView.EnableAppearanceEvenRow = true;

                _GridView.Appearance.EvenRow.BackColor = ODD_ROW_BACK_COLOR;
                // _GridView.OptionsView.EnableAppearanceEvenRow = true;
                //  _GridView.OptionsView.EnableAppearanceOddRow = true; 
                _GridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(_GridView_RowCellStyle);
                _GridView.CustomFilterDialog += new DevExpress.XtraGrid.Views.Grid.CustomFilterDialogEventHandler(_GridView_CustomFilterDialog);
            }

            setGridViewByOptions();
            createOptionsMenu(ContextMenu);
            createCopyCellMenu(ContextMenu);

        }


        void _GridView_CustomFilterDialog(object sender, DevExpress.XtraGrid.Views.Grid.CustomFilterDialogEventArgs e) {
            MyXtraGridFilterDialog dialog = new MyXtraGridFilterDialog(e.Column);
            dialog.ShowDialog(this);
            e.FilterInfo = new ColumnFilterInfo(dialog.GetCustomFiltersCriterion());
            e.Handled = true;
        }


        void _GridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e) {
            //if (e.RowHandle < 0) return;
            //if ((e.RowHandle % 2) != 0 && _GridView.FocusedRowHandle != e.RowHandle)
            //    e.Appearance.BackColor = ODD_ROW_BACK_COLOR;   
        }

		/// <summary>
		///  ���ò˵�Ӧ��..
		/// </summary>
		public override ContextMenu ContextMenu {
			get {
				return base.ContextMenu;
			}
			set {
				base.ContextMenu = value;
			}
		}
		/// <summary>
		/// �жϲ�����Delete ���������
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e) {
			if(_ValidedDeleteKeyDown && (e.KeyCode == System.Windows.Forms.Keys.Delete)){
               // MB.XWinLib.XtraGrid.XtraGridEditHelper.Instance.DeleteFocusedRow(this);    
			}
			base.OnKeyDown (e);
		}
		#endregion ���ǻ���ķ���...

		#region ��չ��Public ����...
		/// <summary>
		/// �������õ������ڵĲ˵��
		/// </summary>
		/// <param name="createMenus"></param>
		public virtual void ReSetContextMenu(XtraContextMenuType   createMenus){
			XtraContextMenu xMenu = new XtraContextMenu(this,createMenus);
			base.ContextMenu = xMenu.GridContextMenu;
			createOptionsMenu(ContextMenu);
            createCopyCellMenu(ContextMenu);
		}
		#endregion ��չ��Public ����...

		#region ��չ������...
        /// <summary>
        /// ��ǰʵ������ݱ�ǡ�
        /// ������һ���̶�����ݣ�������ͨ�� System.Guid.NewGuid() ������
        /// </summary>
        [Browsable(false)]
        public System.Guid InstanceIdentity {
            get {
                
                return _InstanceIdentity;
            }
            set {
                _InstanceIdentity =  value;
            }

        }
		/// <summary>
		/// ��ȡ��������������IO����ʱ���ƵĶ���
		/// </summary>
		[Description("��ȡ��������������IO����ʱ���ƵĶ���")] 
		public IGridExDataIOControl  DataIOControl{
			get{
				return _DataIOControl;
			}
			set{
				_DataIOControl = value;
			}
		}

		/// <summary>
		/// ��ȡ�ؼ����ڵĴ��ڡ�
		/// </summary>
		[Browsable(false)]
		public virtual System.Windows.Forms.Form ParentForm{
			get{
				if(!_IsDesign){
					return MB.WinBase.ShareLib.Instance.GetControlParentForm(this);
				}
				else
					return null;
			}
		}
        /// <summary>
        /// ���û��ȡ���û�����Deleted ��ʱ�Ƿ�ɾ��Grid �ϵļ�¼.
        /// </summary>
		[Description("���û��ȡ���û�����Deleted ��ʱ�Ƿ�ɾ��Grid �ϵļ�¼��")] 
		public bool ValidedDeleteKeyDown{
			get{
				return _ValidedDeleteKeyDown;
			}
			set{
				_ValidedDeleteKeyDown = value;
			}
		}
        /// <summary>
        /// ���û��ȡ�Ƿ���ʾѡ����Ʋ˵�
        /// </summary>
		[Description("���û��ȡ�Ƿ���ʾѡ����Ʋ˵�")] 
		public bool ShowOptionMenu{
			get{
				return _ShowOptionMenu;
			}
			set{
				_ShowOptionMenu = value;
				
			}
		}
		#endregion ��չ������...

		#region �ڲ��������¼�����...
		//�����̶���ѡ��Ŀ�˵�
		private void createOptionsMenu(ContextMenu contextMenu){
			if(!_ShowOptionMenu ||  contextMenu==null)
				return;
			if(_OptionMenu==null){
				_OptionMenu = new MenuItem();
				_OptionMenu.Text = "ѡ��";
				_OptionMenu.Click +=new EventHandler(_OptionMenu_Click);
			}
			if(!contextMenu.MenuItems.Contains(_OptionMenu)){
				contextMenu.MenuItems.Add("-");
				contextMenu.MenuItems.Add(_OptionMenu);
			}
		}
 
		private void _OptionMenu_Click(object sender, EventArgs e) {
			if(_OptionsCFGName==null || _OptionsCFGName.Length ==0)
				return;

            FrmXtraGridViewOptions frm = new FrmXtraGridViewOptions(_GridView,_CurrentInfo, _OptionsCFGName);
			
            var result=frm.ShowDialog();

            // ȷ��,Ӧ��������ʽ
            if (result == DialogResult.OK)
            {
                setGridViewByOptions();
            }

            // ȡ��,���¼��������ļ�
            if (result == DialogResult.Cancel)
            {
                _CurrentInfo = GridViewOptionsHelper.Instance.CreateFromXMLToInfo(_OptionsCFGName);
            }
 
		}

        #region ���Ƶ�Ԫ���Ҽ��˵�����
        /// <summary>
        ///  ��ÿһ��Grid����Ӹ��Ƶ�Ԫ��Ĺ���
        /// </summary>
        /// <param name="contextMenu">����ؼ���context menu</param>
        private void createCopyCellMenu(ContextMenu contextMenu) {
            if (contextMenu == null)
                return;
            if (_CopyCellMenu == null) {
                _CopyCellMenu = new MenuItem();
                _CopyCellMenu.Text = "���Ƶ�Ԫ��";
                _CopyCellMenu.Click += new EventHandler(_CopyCellMenu_Click);
            }
            if (!contextMenu.MenuItems.Contains(_CopyCellMenu)) {
                contextMenu.MenuItems.Add("-");
                contextMenu.MenuItems.Add(_CopyCellMenu);
            }
        }

        /// <summary>
        /// ��ֵ��Ԫ���Ҽ��˵��¼���Ӧ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CopyCellMenu_Click(object sender, EventArgs e) {
            try {
                ContextMenu menu = sender as ContextMenu;
                object value = getCellValueFromGrid();
                if (value != null)
                    Clipboard.SetText(value.ToString());
                //menu.
            }
            catch (Exception ex) {
                MB.Util.TraceEx.Write("���Ƶ�Ԫ��ʧ��," + ex.ToString());
                MB.WinBase.MessageBoxEx.Show(this, "���Ƶ�Ԫ��ʧ��");
            }


        }

        /// <summary>
        /// ���ݵ�ǰ������ĵ�Ԫ��õ���Ԫ�������
        /// </summary>
        /// <returns></returns>
        private object getCellValueFromGrid() {
            return getCellValueFromGrid(string.Empty);
        }

        /// <summary>
        /// �����������õ���Ԫ�������
        /// ������ʾ��ָ����������
        /// </summary>
        /// <param name="cfgPropertyName">��Ԫ�������������Ϊ�գ�����ݵ�ǰ������ĵ�Ԫ��</param>
        /// <returns></returns>
        private object getCellValueFromGrid(string cfgPropertyName) {
            string propertyName = cfgPropertyName;
            object value = null;
            DevExpress.XtraGrid.GridControl grid = this;
            if (grid != null) {
                var view = grid.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                if (view.FocusedRowHandle < 0) return null;

                object row = view.GetFocusedRow();
                if (string.IsNullOrEmpty(propertyName))
                    propertyName = view.FocusedColumn.FieldName;

                #region ����ClickInputButtonȥʵ�ֵ��У�COPY���е����ֲ�����GIRD����ʾ�����֡�������Ҫ����
                if (_ColEidtPros != null && _ColEidtPros.Count > 0) {
                    if (_ColEidtPros.ContainsKey(propertyName)) { 
                        string textFieldNameFromSource = _ColEidtPros[propertyName].TextFieldName;
                        var dataMappings = _ColEidtPros[propertyName].EditCtlDataMappings;
                        if (dataMappings != null && dataMappings.Count > 0) {
                            foreach (var dataMapping in dataMappings) {
                                if (dataMapping.ColumnName.CompareTo(textFieldNameFromSource) == 0) {
                                    propertyName = dataMapping.ColumnName;
                                }
                            }
                        }
                    }
                }
                #endregion

                if (string.Compare(row.GetType().Name, "DataRowView", true) == 0) {
                    System.Data.DataRowView drRow = row as System.Data.DataRowView;
                    value = drRow[propertyName];
                }
                else {
                    if (MB.Util.MyReflection.Instance.CheckObjectExistsProperty(row, propertyName))
                        value = MB.Util.MyReflection.Instance.InvokePropertyForGet(row, propertyName);
                }
            }
            return value;
        }
        #endregion

        private void setGridViewByOptions(){
			if(_GridView!=null && _CurrentInfo!=null){
				_GridView.OptionsView.ShowHorzLines =  _CurrentInfo.ShowHorzLines;
				_GridView.OptionsView.ShowVertLines = _CurrentInfo.ShowVertLines;
				_GridView.OptionsView.AllowCellMerge = _CurrentInfo.AllowCellMerge;
                //����������ֵ��ʽ
                if (_CurrentInfo.StyleConditions != null && _CurrentInfo.StyleConditions.Count > 0) {
                    MB.XWinLib.XtraGrid.XtraGridViewHelper.Instance.AddStylesForConditions(this, _CurrentInfo.StyleConditions);     
                }
			}
		}
        private DevExpress.XtraGrid.Columns.GridColumn getColumnByStyleName(string name) {
           var col = _GridView.Columns.ColumnByFieldName(name);
           if (col != null)
               return col;
           else
               return null;
        }
		#endregion �ڲ��������¼�����...
	}

	#region �Զ����¼��������...
	public delegate void GridControlExMenuEventHandle(object sender,GridControlExMenuEventArg arg);
	public class GridControlExMenuEventArg{
		private bool _Handled;
        private DevExpress.XtraGrid.Columns.GridColumn _Column;  
		private XtraContextMenuType _MenuType ;
		public GridControlExMenuEventArg(XtraContextMenuType menuType){
			_MenuType = menuType;
		}
		public bool Handled{
			get{
				return _Handled;
			}
			set{
				_Handled = value;
			}
		}
		public XtraContextMenuType MenuType{
			get{
				return _MenuType;
			}
			set{
				_MenuType = value;
			}
		}
        /// <summary>
        /// ��ǰ��Ӧ������
        /// </summary>
        public DevExpress.XtraGrid.Columns.GridColumn Column {
            get {
                return _Column;
            }
            set {
                _Column = value;
            }
        }
	}
	#endregion �Զ����¼��������...

	#region ���ݵ���������...
	/// <summary>
	/// ʵ���������ݵ��뵼���Ŀ����ࡣ
	/// </summary>
	public interface IGridExDataIOControl{
		bool IgnoreKeyOnDataImport{get;set;}
		bool BeforeIOData();
		void AfterIOData();
	}
	public class GridExDataIOControl : IGridExDataIOControl{
		private bool _IgnoreKeyOnDataImport;
		public GridExDataIOControl() : this(true)
		{

		}
		public GridExDataIOControl(bool ignoreKeyOnDataImport){
			_IgnoreKeyOnDataImport = ignoreKeyOnDataImport;
		}
		public virtual bool BeforeIOData(){
			return true;
		}
		public virtual void AfterIOData(){
		}
		public bool IgnoreKeyOnDataImport{
			get{
				return _IgnoreKeyOnDataImport;
			}set{
				 _IgnoreKeyOnDataImport = value;
			 }
		}
	}
	#endregion ���ݵ���������...
}

