﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MB.Util.Model.Chart
{
    // 需要引用的命名空间  using System.Runtime.Serialization;  

    /// <summary> 
    /// 文件生成时间 2012-12-26 10:39 
    /// </summary> 
    [DataContract]
    public class ChartTemplateInfo
    {
        private int _ID;
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _NAME;
        [DataMember]
        public string NAME
        {
            get { return _NAME; }
            set { _NAME = value; }
        }
        private string _RULE_PATH;
        [DataMember]
        public string RULE_PATH
        {
            get { return _RULE_PATH; }
            set { _RULE_PATH = value; }
        }
        private string _GRID_NAME;
        [DataMember]
        public string GRID_NAME
        {
            get { return _GRID_NAME; }
            set { _GRID_NAME = value; }
        }
        private string _FILTER_STRING;
        [DataMember]
        public string FILTER_STRING
        {
            get { return _FILTER_STRING; }
            set { _FILTER_STRING = value; }
        }
        private byte[] _TEMPLATE_FILE;
        [DataMember]
        public byte[] TEMPLATE_FILE
        {
            get { return _TEMPLATE_FILE; }
            set { _TEMPLATE_FILE = value; }
        }
        private string _TEMPLATE_TYPE;
        [DataMember]
        public string TEMPLATE_TYPE
        {
            get { return _TEMPLATE_TYPE; }
            set { _TEMPLATE_TYPE = value; }
        }
        private string _CREATE_USER;
        [DataMember]
        public string CREATE_USER
        {
            get { return _CREATE_USER; }
            set { _CREATE_USER = value; }
        }
        private DateTime _CREATE_DATE;
        [DataMember]
        public DateTime CREATE_DATE
        {
            get { return _CREATE_DATE; }
            set { _CREATE_DATE = value; }
        }
    } 

}
