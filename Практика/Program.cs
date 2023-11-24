using System.Security.Cryptography;

class Program
{
    static int Priority(char operators)
    {
        if (operators == '(' || operators == ')') return 0;
        else if (operators == '+' || operators == '-') return 1;
        else return 2;
    }

    static List<object> Parse(string userText)
    {
        userText = userText.Replace(" ", "");
        List<object> result = new List<object>();
        string bufer = string.Empty;
        for (int i = 0; i<userText.Length; i++)
        {
            if (char.IsDigit(userText[i]))
                bufer += userText[i];
            else
            {
                if (bufer != string.Empty)
                {
                    result.Add(Convert.ToInt32(bufer));
                    bufer = string.Empty;
                }
                result.Add(userText[i]);
            }
            if (i == userText.Length - 1 && bufer!= string.Empty)
                result.Add(Convert.ToInt32(bufer));
        }
        return result;
    }

    static List<object> RPN(List<object> mathExp) //метод для записи в ОПЗ
    {
        List<object> result = new List<object>();
        Stack<char> oper = new Stack<char>();
        string bufer = string.Empty;
        for (int i = 0; i < mathExp.Count; i++)
        {
            if (mathExp[i] is int)
                result.Add((int)mathExp[i]);
            else
            {
                if (oper.Count == 0)
                    oper.Push((char)mathExp[i]);
                else
                {
                    if ((char)mathExp[i] == '(')
                        oper.Push((char)mathExp[i]);
                    else
                    {
                        if ((Priority((char)mathExp[i]) > Priority(oper.Peek())) && (char)mathExp[i] != ')')
                            oper.Push((char)mathExp[i]);
                        else if (Priority((char)mathExp[i]) <= Priority(oper.Peek()) && (char)mathExp[i] != ')')
                        {
                            if (oper.Contains('('))
                            {
                                while (oper.Peek() != '(')
                                {
                                    result.Add(oper.Peek());
                                    oper.Pop();
                                }
                                oper.Push((char)mathExp[i]);
                            }
                            else if ((Priority((char)mathExp[i]) < Priority(oper.Peek()) && (!oper.Contains('('))))
                            {
                                while (oper.Count != 0)
                                {
                                    result.Add(oper.Peek());
                                    oper.Pop();
                                }
                                oper.Push((char)mathExp[i]);
                            }

                            else if (Priority((char)mathExp[i]) == Priority(oper.Peek()) && (!oper.Contains('(')))
                            {
                                result.Add(oper.Peek());
                                oper.Pop();
                                oper.Push((char)mathExp[i]);
                            }
                        }
                        else if ((char)mathExp[i] == ')')
                        {
                            while (oper.Peek() != '(')
                            {
                                result.Add(oper.Peek());
                                oper.Pop();
                            }
                            oper.Pop();
                        }
                    }
                }
            }

            if ((i == mathExp.Count - 1) && (bufer != string.Empty))
                result.Add((bufer));

            if ((i == mathExp.Count - 1) && (oper.Count != 0))
            {
                while (oper.Count != 0)
                {
                    result.Add(oper.Peek());
                    oper.Pop();
                }
            }
        }
        return result;
    }

    static bool IsOperator(char symbol) //метод для определения оператора
    {
        string operators = "+-*/";
        if (operators.Contains(symbol)) return true;
        else return false;
    }

    static double TheOperation(char oper, int num1, int num2)
    {
        if (oper == '+') return num1 + num2;
        else if (oper == '-') return num1 - num2;
        else if (oper == '*') return num1 * num2;
        else return num1 / num2;
    }


    static List<object> Calculate(List<object> mathExp)
    {
        int i = 0;
        while (mathExp.Count > 1)
        {
            if (i > mathExp.Count)
                i = 0;
            if (mathExp[i] is char && IsOperator((char)mathExp[i]))
            {
                mathExp[i - 2] = TheOperation((char)mathExp[i], Convert.ToInt32(mathExp[i - 2]), Convert.ToInt32(mathExp[i - 1]));
                mathExp.RemoveAt(i);
                mathExp.RemoveAt(i - 1);
                i = 0;
            }
            i++;
        }
        return mathExp;
    }
    static List<int> GetNums(List<object> mathExp) //метод для вывода чисел из начального выражения
    {
        List<int> res = new List<int>();
        foreach (var el in mathExp)
        {
            if (!(el is char))
                res.Add(Convert.ToInt32(el));
        }
        return res;
    }

    static List<char> GetOperators(List<object> mathExp) //метод для вывода операторов из начального выражения
    {
        List<char> res = new List<char>();
        foreach (var el in mathExp)
        {
            if (el is char)
                res.Add(Convert.ToChar(el));
        }
        return res;
    }

    static void Main()
    {
        Console.Write("Your mathematical expression: ");
        string userText = Console.ReadLine();
        Console.WriteLine();
        List<object> rpnUserText = RPN(Parse(userText));
        Console.WriteLine("RPN: " + string.Join(" ", rpnUserText) + "\n");
        Console.WriteLine("The nums: " + string.Join(" ", GetNums(rpnUserText)) + "\n");
        Console.WriteLine("The operators: " + string.Join(" ", GetOperators(rpnUserText)) + "\n");
        Console.WriteLine("The answer: " + Calculate(rpnUserText)[0]);
    }
}