using System;

namespace DIYReport {
	/// <summary>
	/// Number2Chiness ����ת��Ϊ���Ĵ�д�Ľ���
	/// </summary>
	public class Number2Chiness {
		// ˼�룺���������֣���Ϊ8λһ�Σ��ԡ��ڡ��󿪣�8λ���ڣ��ֳ�4λ�󿪣��������ӣ����ʣ�µľ�����λ����λ�������ڵ������ˣ�����Щ�������ֱ������������ټ���С�����ֵĴ����õ���д�Ľ���ˡ�

		private static readonly string _NumChinese="��Ҽ��������½��ƾ�";
		/// <summary>
		/// ת��Ϊ��д�Ľ�
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static  string ConvertToUpperMoney( decimal number ) {
			if ( Math.Abs( number ) < (decimal)0.01  )
				return "��Բ��";
			long y = Convert.ToInt64( Decimal.Truncate( number / 100000000 ) );
			long w = Convert.ToInt64( Decimal.Truncate( (number - y * 100000000)/10000 ) );
			long g = Convert.ToInt64( Decimal.Truncate( number % 10000 ) );
			long x = Convert.ToInt64( Decimal.Truncate( ( number - Decimal.Truncate( number ) ) * 100 ) ); 

			string re;
			if ( y < 10000 && y != 0 )
				re = GetCHN( y ).Substring( 1 ) + "��";
			else
				re = GetCHN( y ); 

			if ( w != 0 ) {
				if ( re != "" )
					re += GetCHN( w ) + "��";
				else
					re = GetCHN( w ).Substring( 1 ) + "��";
			} 

			if ( g != 0 ) {
				if ( re != "" )
					re += GetCHN( g );
				else
					re = GetCHN( g ).Substring( 1 );
			} 

			if ( re != "" ) {  //��λ����ֵ
				if ( x == 0 ) //��С��λ
					re += "Բ��";
				else   //��С��λ
					re += "Բ��" + GetFCHN( x );
			}
			else {    //��λ����ֵ
				if ( x == 0 ) //��С��λ
					re += "��Բ";
				else   //��С��λ
					re += GetFCHN( x );
			} 

			return re;
		} 

		private static string GetFCHN( long aa ) {
			string re = string.Empty;
			int x = Convert.ToInt32(aa % 10);  //��
			int y = Convert.ToInt32((aa - x)/10); //�� 

			if ( y != 0 )
				re = _NumChinese[y] + "��"; 

			if ( x != 0 )
				re += _NumChinese[x] + "��"; 

			return re;
		} 

		private static string GetCHN( double  aa ) {
			string re = string.Empty; 

			if ( aa == 0 )
				return re; 

			int l1 = Convert.ToInt32( aa % 10 );
			int l2 = Convert.ToInt32( (aa - l1)/10 % 10 );
			int l3 = Convert.ToInt32( (aa - l1 - l2 * 10 )/100 % 10 );
			int l4 = Convert.ToInt32( Math.Floor( aa / 1000 ) ); 

			re = "��";
			if ( l4 == 0 ) {
				if ( re[re.Length-1] != '��' && l3 != 0 )
					re += "��";
			}
			else
				re += _NumChinese[l4] + "Ǫ"; 

			if ( l3 == 0 ) {
				if ( re[re.Length-1] != '��' && l2 != 0 )
					re += "��";
			}
			else
				re += _NumChinese[l3] + "��"; 

			if ( l2 == 0 ) {
				if ( re[re.Length-1] != '��' && l1 != 0 )
					re += "��";
			}
			else
				re += _NumChinese[l2] + "ʰ"; 

			if ( l1 != 0 ) re += _NumChinese[l1]; 

			return re;
		}
	}

   /// <summary>
   /// Number2English ����ת��ΪӢ�Ĵ�д�Ľ���
   /// </summary>
	public class Number2English {
		private string[] StrNO = new string[19]; 
		private string[] Unit = new string[8]; 
		private string[] StrTens = new string[9];

		public static Number2English DefaultInstance = new Number2English();

		#region NumberToString
		public string NumberToString(decimal Number) { 
			string Str; 
			string BeforePoint; 
			string AfterPoint; 
			string tmpStr; 
			int nBit; 
			string CurString;
			int nNumLen;
			Init(); 
			Str = Convert.ToString(Math.Round(Number,2)); 
			if (Str.IndexOf(".")==-1) { 
				BeforePoint = Str; 
				AfterPoint = ""; 
			} 
			else { 
				BeforePoint = Str.Substring(0,Str.IndexOf(".")); 
				AfterPoint = Str.Substring(Str.IndexOf(".")+1,Str.Length - Str.IndexOf(".")-1); 
			}
			if (BeforePoint.Length > 12) { 
				return null; 
			}
			Str = "";
			while (BeforePoint.Length > 0) {
				nNumLen = Len(BeforePoint); 
				if (nNumLen % 3 == 0) { 
					CurString = Left(BeforePoint, 3); 
					BeforePoint = Right(BeforePoint, nNumLen - 3); 
				} 
				else { 
					CurString = Left(BeforePoint, (nNumLen % 3)); 
					BeforePoint = Right(BeforePoint, nNumLen - (nNumLen % 3)); 
				}
				nBit = Len(BeforePoint) / 3; 
				tmpStr = DecodeHundred(CurString);
				if((BeforePoint == Len(BeforePoint).ToString() || nBit ==0) && Len(CurString) ==3) {
					if (System.Convert.ToInt32(Left(CurString, 1)) != 0 & System.Convert.ToInt32(Right(CurString, 2)) != 0) { 
						tmpStr = Left(tmpStr, tmpStr.IndexOf(Unit[3]) + Len(Unit[3])) + Unit[7] + " " + Right(tmpStr, Len(tmpStr) - (tmpStr.IndexOf(Unit[3]) + Len(Unit[3]))); 
					} 
					else { 
						tmpStr = Unit[7] + " " + tmpStr; 
					}
				}
				if (nBit == 0) { 
					Str = Convert.ToString(Str + " " + tmpStr).Trim();
				} 
				else { 
					Str = Convert.ToString(Str + " " + tmpStr + " " + Unit[nBit-1]).Trim(); 
				}
				if (Left(Str, 3) == Unit[7]) { 
					Str = Convert.ToString(Right(Str, Len(Str) - 3)).Trim(); 
				}
				if( BeforePoint == Len(BeforePoint).ToString()) {
					return "";
				}
			}
			BeforePoint = Str; 
			if (Len(AfterPoint) > 0) { 
				AfterPoint = Unit[5] + " " + DecodeHundred(AfterPoint) + " " + Unit[6]; 
			} 
			else { 
				AfterPoint = Unit[4]; 
			} 
			return BeforePoint + " " + AfterPoint;

		}

		#endregion

		private void Init() { 
			if (StrNO[0] != "ONE") { 
				StrNO[0] = "ONE"; 
				StrNO[1] = "TWO"; 
				StrNO[2] = "THREE"; 
				StrNO[3] = "FOUR"; 
				StrNO[4] = "FIVE"; 
				StrNO[5] = "SIX"; 
				StrNO[6] = "SEVEN"; 
				StrNO[7] = "EIGHT"; 
				StrNO[8] = "NINE"; 
				StrNO[9] = "TEN"; 
				StrNO[10] = "ELEVEN"; 
				StrNO[11] = "TWELVE"; 
				StrNO[12] = "THIRTEEN"; 
				StrNO[13] = "FOURTEEN"; 
				StrNO[14] = "FIFTEEN"; 
				StrNO[15] = "SIXTEEN"; 
				StrNO[16] = "SEVENTEEN"; 
				StrNO[17] = "EIGHTEEN"; 
				StrNO[18] = "NINETEEN"; 
				StrTens[0] = "TEN"; 
				StrTens[1] = "TWENTY"; 
				StrTens[2] = "THIRTY"; 
				StrTens[3] = "FORTY"; 
				StrTens[4] = "FIFTY"; 
				StrTens[5] = "SIXTY"; 
				StrTens[6] = "SEVENTY"; 
				StrTens[7] = "EIGHTY"; 
				StrTens[8] = "NINETY"; 
				Unit[0] = "THOUSAND"; 
				Unit[1] = "MILLION"; 
				Unit[2] = "BILLION"; 
				Unit[3] = "HUNDRED"; 
				Unit[4] = "ONLY"; 
				Unit[5] = "POINT"; 
				Unit[6] = "CENT"; 
				Unit[7] = "AND"; 
			} 
		}


		#region DecodeHundred
		private string DecodeHundred(string HundredString) { 
			int tmp;
			string rtn="";
			if( Len(HundredString) > 0 && Len(HundredString) <= 3) {
				switch (Len(HundredString)) {
					case 1:
						tmp = System.Convert.ToInt32(HundredString); 
						if (tmp != 0) { 
							rtn=StrNO[tmp-1].ToString(); 
						} 
						break;
					case 2:
						tmp = System.Convert.ToInt32(HundredString); 
						if (tmp != 0) { 
							if ((tmp < 20)) { 
								rtn=StrNO[tmp-1].ToString(); 
							} 
							else { 
								if (System.Convert.ToInt32(Right(HundredString, 1)) == 0) { 
									rtn=StrTens[Convert.ToInt32(tmp / 10)-1].ToString(); 
								} 
								else { 
									rtn=Convert.ToString(StrTens[Convert.ToInt32(tmp / 10)-1] + "-" + StrNO[System.Convert.ToInt32(Right(HundredString, 1))-1]);
								} 
							} 
						} 
						break;
					case 3:
						if (System.Convert.ToInt32(Left(HundredString, 1)) != 0) { 
							rtn=Convert.ToString(StrNO[System.Convert.ToInt32(Left(HundredString, 1))-1] + " " + Unit[3] + " " + DecodeHundred(Right(HundredString, 2))); 
						} 
						else { 
							rtn=DecodeHundred(Right(HundredString, 2)).ToString(); 
						} 
						break;
					default:
						break;
				}
			}
			return rtn;
		}

		#endregion

		#region Left
		private string Left(string str,int n) {
			return str.Substring(0,n);
		}
		#endregion

		#region Right
		private string Right(string str,int n) {
			return str.Substring(str.Length-n,n);
		}
		#endregion

		#region Len
		private int Len(string str) {
			return str.Length;
		}
		#endregion
	}
}
