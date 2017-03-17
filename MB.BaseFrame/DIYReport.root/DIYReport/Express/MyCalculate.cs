//---------------------------------------------------------------- 
// Copyright (C) 2004-2004 nick chen (nickchen77@gmail.com) 
// All rights reserved. 
// 
// Author		:	Nick
// Create date	:	2006-09-20.
// Description	:	MyCalculate ���ʽ���㡣
// ��ע������  
// Modify date	:			By:					Why: 
//----------------------------------------------------------------
using System;
using System.Data;
using System.Collections;

namespace DIYReport.Express
{
	/// <summary>
	/// MyCalculate ���ʽ���㡣
	/// </summary>
	public class MyCalculate {
		
		#region private construct function...
		/// <summary>
		/// private construct function��
		/// </summary>
		private  MyCalculate() {

		}
		#endregion private construct function...
		
		#region public ����...
		//����ת���ɺ�����ʽ�ټ���
		// �磺23+56/(102-100)*((36-24)/(8-6))
		// ת���ɣ�23|56|102|100|-|/|*|36|24|-|8|6|-|/|*|+"
		//�Ա�����ջ�ķ�ʽ�����м��㡣
		public static string  CalculateParenthesesExpression(string expression) {
			ArrayList operatorList = new ArrayList();
			string operator1;
			string expressionString = "";
			string operand3;
			expression = expression.Replace(" ","");
			while(expression.Length > 0) {
				operand3 = "";
				//ȡ���ִ���
				if(Char.IsNumber(expression[0])) {
					while(Char.IsNumber(expression[0]) || expression[0].Equals('.')) {
						operand3 += expression[0].ToString() ;
						expression = expression.Substring(1);
						if(expression == "")break;
					}
					expressionString += operand3 + "|";
				}

				//ȡ��C������
				if(expression.Length >0 && expression[0].ToString() == "(") {
					operatorList.Add("("); 
					expression = expression.Substring(1);
				}

				//ȡ��)������
				operand3 = "";
				if(expression.Length >0 && expression[0].ToString() == ")") {
					do {
      
						if(operatorList[operatorList.Count -1].ToString() != "(") {
							operand3 += operatorList[operatorList.Count -1].ToString() + "|" ;
							operatorList.RemoveAt(operatorList.Count - 1) ;
						}
						else {
							operatorList.RemoveAt(operatorList.Count - 1) ;
							break;
						}
      
					}while(true);
					expressionString += operand3;
					expression = expression.Substring(1);
				}

				//ȡ������Ŵ���
				operand3 = "";
				if(expression.Length ==0)break;
				if(expression[0].ToString() == "*" || expression[0].ToString() == "/" || expression[0].ToString() == "+" || expression[0].ToString() == "-"){
					operator1 = expression[0].ToString();
					if(operatorList.Count>0) {
       
						if(operatorList[operatorList.Count -1].ToString() == "(" || verifyOperatorPriority(operator1,operatorList[operatorList.Count - 1].ToString())) {
							operatorList.Add(operator1);
						}
						else {
							operand3 += operatorList[operatorList.Count - 1].ToString() + "|";
							operatorList.RemoveAt(operatorList.Count - 1);
							operatorList.Add(operator1);
							expressionString += operand3 ;
       
						}
      
					}
					else {
						operatorList.Add(operator1);
					}
					expression = expression.Substring(1);
				}
				else if(!Char.IsNumber(expression[0]) && !expression[0].Equals('.') && !expression[0].Equals('-') ){
					throw new Exception("������Ч�ı��ʽ!"); 
				}
			}

			operand3 = "";
			while(operatorList.Count != 0) {
				operand3 += operatorList[operatorList.Count -1].ToString () + "|";
				operatorList.RemoveAt(operatorList.Count -1);
			} 

			expressionString += operand3.Substring(0, operand3.Length -1);  ;
   

			return CalculateParenthesesExpressionEx(expressionString);

		}
		#endregion public ����...

		#region �ڲ���������...
		// �ڶ���:��ת���ɺ������ʽ�Ӽ���
		//23|56|102|100|-|/|*|36|24|-|8|6|-|/|*|+"
		private static string  CalculateParenthesesExpressionEx(string expression) {
			//��������ջ
			ArrayList operandList =new ArrayList();
			float operand1;
			float operand2;
			string[] operand3;
  
			expression = expression.Replace(" ","");
			operand3 = expression.Split(Convert.ToChar("|"));
 
			for(int i = 0;i < operand3.Length;i++) {
				if(Char.IsNumber(operand3[i],0)) {
					operandList.Add( operand3[i].ToString());
				}
				else {
					//������������ջ��һ����������ջ����
					operand2 =(float)Convert.ToDouble(operandList[operandList.Count-1]);
					operandList.RemoveAt(operandList.Count-1); 
					operand1 =(float)Convert.ToDouble(operandList[operandList.Count-1]);
					operandList.RemoveAt(operandList.Count-1);
					operandList.Add(calculate(operand1,operand2,operand3[i]).ToString()) ;
				}
    
			}


			return operandList[0].ToString();
		}

 

		//�ж�������������ȼ���
		private static bool verifyOperatorPriority(string Operator1,string Operator2) {
   
			if(Operator1=="*" && Operator2 =="+")
				return true;
			else if(Operator1=="*" && Operator2 =="-")
				return true;
			else if(Operator1=="/" && Operator2 =="+")
				return true;
			else if(Operator1=="/" && Operator2 =="-")
				return true;
			else
				return false;
		}

		//����
		private static  float calculate(float operand1, float operand2,string operator2) {
			switch(operator2) {
				case "*":
					operand1 *=  operand2;
					break;
				case "/":
					operand1 /=  operand2;
					break;
				case "+":
					operand1 +=  operand2;
					break;
				case "-":
					operand1 -=  operand2;
					break;
				default:
					break;
			}
			return operand1;
		}
		#endregion �ڲ���������...

	}
	/// <summary>
	/// �����ֶεı��ʽ���㡣
	/// </summary>
	public class MyCalculateDataRow{
		private static readonly string CONST_OPERATE_STR = @"+-/* ()";
		private static readonly string[] Can_Process_Type_Array = new string[]{"Int16","Int32","Int64","Decimal","Double","Single"};
		
		#region private construct function...
		/// <summary>
		/// private construct function...
		/// </summary>
		private MyCalculateDataRow(){

		}
		#endregion private construct function...

		#region public ��̬����...
		/// <summary>
		/// ��������ֶεı��ʽ��
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static string CalculateExpression(DataRow dr,string expression){
			expression = replaceFieldNameAsValue(dr,expression);
			//Console.WriteLine("ֵת����ı��ʽΪ:" + expression);
			return MyCalculate.CalculateParenthesesExpression(expression); 
		}
		/// <summary>
		/// ���ֶε�Caption ת��Ϊ�ֶε���ʽ��
		/// </summary>
		/// <param name="dcs"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static string ReplaceCaptionAsFieldName(DataColumnCollection dcs,string expression){
			IList descs = getExpression(expression);
			foreach(string desc in descs){
				for(int i  = 0; i < dcs.Count ; i++){
					DataColumn dc = dcs[i];
					if(desc.Equals("@" + dc.Caption)){
						expression = expression.Replace (desc,"@" + dc.ColumnName); 
						break;
					}
//					if(i == dcs.Count -1)
//						throw new Exception("��Ӧ���ֶ�������Դ���Ҳ�����"); 
				}
			}
			return expression;

		}
		#endregion public ��̬����...

		#region �ڲ���������...
		private static string replaceFieldNameAsValue(DataRow dr,string expression){
			IList fields = getExpression(expression);
			DataColumnCollection dcs = dr.Table.Columns;
			foreach(string fieldName in fields){
				string name = fieldName.Remove(0,1);
				if(!dcs.Contains(name))  
					return "Expression Error";
					//throw new Exception("��Ӧ���ֶ�������Դ���Ҳ�����"); 
				DataColumn dc = dcs[name];
				string typeName = dc.DataType.Name;
				//Ŀǰ�ȴ��� ����
				if(Array.IndexOf(Can_Process_Type_Array,typeName) < 0)
					throw new Exception("Ŀǰ���ʽ��Ӧ�е���������ֻ֧��Int16,Int32,Int64,Decimal,Double,Single,��Ӧ���ֶ����ͳ���"); 
				string val = dr[name].ToString();
				if(val!=null && val.Length > 2){
					if(val[0].Equals('-'))//�ж��Ƿ�Ϊ������Ȼ��ת��Ϊ0 - �����ĸ�ʽ��
						val = "(0 - " + val.Remove(0,1) + ")"; 

				}
				expression = expression.Replace (fieldName,val); 
			}
			return expression;

		}
		private  static IList getExpression(string str){
			ArrayList lst = new ArrayList();
			int begin = -1;
			int end = -1;
			for(int i = 0; i< str.Length ; i++){
				if(str[i].Equals('@')){
					begin = i;
				}
				if(begin == -1) continue;
				if(CONST_OPERATE_STR.IndexOf(str[i]) > -1){
					end = i;
				}
				if(end == -1){
					if(i==str.Length-1) 
						end = str.Length ;
					else
						continue;
				}

				string temp = str.Substring(begin,end - begin);
				lst.Add(temp);
				begin = -1;
				end = -1;

			}
			return lst;
		}
		#endregion �ڲ���������...

	}

}
