using System;
using System.Collections ;

namespace DIYReport.GroupAndSort
{
	/// <summary>
	/// FieldSortComparer ��ժҪ˵����
	/// </summary>
	public class FieldSortComparer : IComparer  {
		// �������
		int IComparer.Compare( Object x, Object y )  {
			RptFieldInfo xObj = x as RptFieldInfo;
			RptFieldInfo yObj = y as RptFieldInfo;
			if(xObj!=null && yObj!=null){
				return( (new CaseInsensitiveComparer()).Compare( xObj.OrderIndex  ,yObj.OrderIndex) );
			}
			else{
				return 0;
			}
		}
	}
}
