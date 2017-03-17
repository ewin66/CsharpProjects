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

namespace DIYReport.ReportModel
{
	/// <summary>
	/// RptObjType �������������(ԭ�ȵı��������������ͨ��index �Ĵ���ģ���������ֻ��ͨ����������)
	/// </summary>
	public enum RptObjType : int
	{
		 None = 0,
		 Text = 1,//���� ��ӡ���������ʱ��ֱ����Ϊ�ַ����
		 Express = 2,//�ֶΣ�����\����� ��Ҫ�滻����Դ�ж�Ӧ������
		 Line = 3,//��Ҫ��ֱ��
		 Rect = 4,
		 Image = 5, //ͼƬ
		 //2006-04-07 ����
		 FieldTextBox = 6,//���ֶε��ı���
		 CheckBox = 7,
		 FieldImage = 8,//���ֶε�ͼ��ؼ�
		 SubReport = 9,//�ӱ������
		 OleObject = 10,// ole �󶨶���
		 BarCode = 11,//������
		 HViewSpecField = 12, //���ú���ת�����ֶο�
		 RichTextBox = 13 //

		 
	}
}
