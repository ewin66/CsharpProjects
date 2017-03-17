//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-10-27
// Description	:	��PropertyGrid ����ʾclass ������������Ϣ��
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
/*
 ����һ��˼·����System.ComponentModel.PropertyDescriptor�̳��Լ����࣬Ȼ��
 ����DisplayName��������ʵ��һ������ܣ�ʹ��Ӧ��Ӣ�����������ġ����Ҫ��PropertyGrid.SelectObject
  ��ѡ�Ķ����ICustomTypeDescriptor�̳У��������е�GetProperties�й����Լ���PropertyDescriptorCollection���ڷ��ء�
*/
using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;

namespace DIYReport.Design
{
	/// <summary>
	/// ��PropertyGrid ����ʾclass ������������Ϣ��
	/// </summary>
	public   class DynamicTypeDescriptor : ICustomTypeDescriptor {
		private PropertyDescriptorCollection dynamicProps;

		public DynamicTypeDescriptor() {
		
		}
	
		public bool ShouldSerializePropertyCommands(){return true;}

		public bool ShouldSerializeCategoryCommands(){return true;}

	
		public virtual  string GetLocalizedName(string Name) {
			return Name;
		}
		
		public virtual string GetLocalizedDescription(string Description) {
			return Description;
		}

		
		#region "TypeDescriptor Implementation"
	
		public String GetClassName() {
			return TypeDescriptor.GetClassName(this,true);
		}

		public AttributeCollection GetAttributes() {
			return TypeDescriptor.GetAttributes(this,true);
		}

		public String GetComponentName() {
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter() {
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent() {
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public PropertyDescriptor GetDefaultProperty() {
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public object GetEditor(Type editorBaseType) {
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes) {
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents() {
			return TypeDescriptor.GetEvents(this, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
			return TypeDescriptor.GetProperties(this, attributes, true);
			
		}

		public PropertyDescriptorCollection GetProperties() {
			
			if ( dynamicProps == null) {
				PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this, true);
				dynamicProps = new PropertyDescriptorCollection(null);

				foreach( PropertyDescriptor oProp in baseProps ) {			
				
					dynamicProps.Add(new DynamicPropertyDescriptor(this,oProp));
					
				}
			}
			return dynamicProps;
		}

		public object GetPropertyOwner(PropertyDescriptor pd) {
			return this;
		}
		#endregion
	}


	public class DynamicPropertyDescriptor : PropertyDescriptor {
		private PropertyDescriptor basePropertyDescriptor; 
		private DynamicTypeDescriptor instance;

		public DynamicPropertyDescriptor(DynamicTypeDescriptor instance,PropertyDescriptor basePropertyDescriptor) : base(basePropertyDescriptor) {
			this.instance=instance;
			this.basePropertyDescriptor = basePropertyDescriptor;
		}

		public override bool CanResetValue(object component) {
			return basePropertyDescriptor.CanResetValue(component);
		}

		public override Type ComponentType {
			get { return basePropertyDescriptor.ComponentType; }
		}

		public override object GetValue(object component) {
			return this.basePropertyDescriptor.GetValue(component);
		}

		public override string Description {
			get {
				return instance.GetLocalizedDescription(base.Name);
			}
		}
		public override string Category {
			get {
				return instance.GetLocalizedName(base.Category);
			}
		}

		public override string DisplayName {
			get {
				return instance.GetLocalizedName(base.Name);
			}
		}

		public override bool IsReadOnly {
			get {
 
					return this.basePropertyDescriptor.IsReadOnly; 
 
			}
		}

		public override Type PropertyType {
			get { return this.basePropertyDescriptor.PropertyType; }
		}

		public override void ResetValue(object component) {
			this.basePropertyDescriptor.ResetValue(component);
		}

		public override bool ShouldSerializeValue(object component) {
			return this.basePropertyDescriptor.ShouldSerializeValue(component);
		}

		public override void SetValue(object component, object value) {
			this.basePropertyDescriptor.SetValue(component, value);
		}
	}

}
