//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2005-1-21
// Description	:	 
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Collections ;

namespace DIYReport.Interface
{
	/// <summary>
	/// IActionParent �û�������
	/// </summary>
	public interface IActionParent
	{
		 void SetPropertyValue(ref IList pObjList);
		 void Add(IList pObjList);
		 void Remove(IList pObjList);
	}
}
