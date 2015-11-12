using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace metrikaHolsteda
{
    class Program
    {
        const int MaxLength = 100;

        static bool check(string string_source)
        {
            string comment = @"^//";
            Regex myReg = new Regex(comment);
           if( myReg.IsMatch(string_source))
               return false;
           else
            return true;

        }
        static string GetNameUniqueOpepand(int StartPosition, string StringSource)
        {
            int j = 0, length = 0, i = 0;
            for (i = StartPosition; StringSource[i] != ' ' ; i++)
                length++;
            char[] name = new char[length];
            for (i = StartPosition; StringSource[i] != ' '; i++, j++)
                name[j] = StringSource[i];
            
            string StringName = new string(name);
            return StringName;
        }

        static string GetNameUniqueOperator(int StartPosition, string StringSource)
        {
            int j = 0, length = 0, i = 0;
            for (i = StartPosition; StringSource[i] != '('; i++)
                length++;
            char[] name = new char[length];
            for (i = StartPosition; i < StartPosition + length; i++, j++)
                name[j] = StringSource[i];

            string StringName = new string(name);
            return StringName;
        }

        static int GetCountAppealOperand(string CompareString, FileStream FileSource)
        {
            int CountAppealOperand = -1;
            string StringSource;
            bool b;
            FileSource.Position = 0;
            StreamReader reader = new StreamReader(FileSource);
            StringSource = reader.ReadLine();
            while ((check(StringSource) == false) && (!reader.EndOfStream))
                StringSource = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (b = StringSource.Contains(CompareString))
                     CountAppealOperand++;
                StringSource = reader.ReadLine();
                while ((check(StringSource) == false) && (!reader.EndOfStream))
                    StringSource = reader.ReadLine();
                
            }
            return CountAppealOperand;
        }

        static int GetCountAppealOperator(string CompareString, FileStream FileSource)
        {
            int CountAppealOperator = 0;
            string StringSource;
            FileSource.Position = 0;
            StreamReader reader = new StreamReader(FileSource);
            StringSource = reader.ReadLine();
            while ((check(StringSource) == false) && (!reader.EndOfStream))
                StringSource = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                bool b = StringSource.Contains(CompareString);
                if (b)
                    CountAppealOperator++;
                StringSource = reader.ReadLine();
                while ((check(StringSource) == false) && (!reader.EndOfStream))
                    StringSource = reader.ReadLine();

            }
            return CountAppealOperator;
        }

        static void Operand( FileStream FileSource, StreamReader reader, out int CountUniqueOperands, out int CountOperands)
        {
            string[] StringRegular = {@"var\s", @"const\s", @"import\s", @"\s[0-9]\s"};
            FileSource.Position = 0;
            int CountOfElementsArray = StringRegular.Length;
            CountUniqueOperands = 0;
            CountOperands = 0;
            int IndexRegularExpression = 0, i = 0;

            Console.WriteLine("\n\tTHE COPYBOOK OF UNIQUE OPERAND:");
            Console.WriteLine("\t------------------------------");

            for (int j = 0; j < CountOfElementsArray; j++, IndexRegularExpression++)
            {
                Regex RegularExpression = new Regex(StringRegular[IndexRegularExpression]);
                int LengthRegularExpresion = StringRegular[IndexRegularExpression].Length - 1;
                int StartPosition = 0;
                FileSource.Position = 0;
                int StartPositionForI = CountUniqueOperands;
                string[] ArrayUniqueOperands = new string[MaxLength];
                string StringSource;

                while (!reader.EndOfStream)
                {
                    StringSource = reader.ReadLine();
                    while ((check(StringSource) == false) && (!reader.EndOfStream))
                        StringSource = reader.ReadLine();
                    Match match = RegularExpression.Match(StringSource);
                    if (match.Length != 0)
                    {
                        if (match.Value == " 0 " || match.Value == " 1 " || match.Value == " 2 " || match.Value == " 3 " || match.Value == " 4 " || match.Value == " 5 " || match.Value == " 6 " || match.Value == " 7 " || match.Value == " 8 " || match.Value == " 9 " || match.Value == " 10 ")
                        {
                            CountOperands++;
                        }
                        else
                        {
                            StartPosition = match.Index + LengthRegularExpresion;
                            ArrayUniqueOperands[CountUniqueOperands] = GetNameUniqueOpepand(StartPosition, StringSource);
                            CountOperands += GetCountAppealOperand(ArrayUniqueOperands[CountUniqueOperands], FileSource);
                            CountUniqueOperands++;
                        }
                    }

                }

                for (i = StartPositionForI; i < CountUniqueOperands; i++)
                    Console.WriteLine(ArrayUniqueOperands[i]);       
            }
            Console.WriteLine("\t------------------------------");
            Console.WriteLine("The count of unique operands = " + CountUniqueOperands);
            Console.WriteLine("The count appeal operands = " + CountOperands);
        }

        static void Operator(FileStream FileSource, StreamReader reader, out int CountUniqueOperators, 
                             out int CountOperators)
        {
            FileSource.Position = 0;
            string[] StringRegular = { "for", "if", "else", "new", "func", @"\+{2}", @"\-{2}", @"\+{1}\s", @"\-{1}\s",
                                      @"\*\s", @"\s={1}\s", @"\+=", @"\-=", @"\={2}", "new",  "type", "get", "set",
                                      @"\>",  @"\>=",  @"\<",  @"\<=", "len", @"\:=", "return", "go", @"\.", "do", @"\<-"};
            CountUniqueOperators = 0; 
            CountOperators = 0;
            int start_position = 0;

            int CountOfElementsArray = StringRegular.Length;

            Console.WriteLine("\n~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
            Console.WriteLine("\n\tTHE COPYBOOK OF UNIQUE OPERATORS:");
            Console.WriteLine("\t------------------------------");

             int IndexRegularExpression = 0;
            for (IndexRegularExpression = 0; IndexRegularExpression < CountOfElementsArray; IndexRegularExpression++)
            {
                Regex RegularExpression = new Regex(StringRegular[IndexRegularExpression]);
                FileSource.Position = 0;

                string[] ArrayUniqueOperators = new string[MaxLength];
                string StringSource;

                while (!reader.EndOfStream)
                {
                    StringSource = reader.ReadLine();
                    while ((check(StringSource) == false) && (!reader.EndOfStream))
                        StringSource = reader.ReadLine();
                    Match match = RegularExpression.Match(StringSource);
                    if (match.Length != 0)
                    {
                        if (StringRegular[IndexRegularExpression] == "func")
                        {
                            start_position = match.Index + match.Length + 1;
                            ArrayUniqueOperators[CountUniqueOperators] = GetNameUniqueOperator(start_position, StringSource);
                            CountOperators += GetCountAppealOperator(ArrayUniqueOperators[CountUniqueOperators], FileSource);
                            Console.WriteLine(ArrayUniqueOperators[CountUniqueOperators]);
                            CountUniqueOperators++;
                            break;
                        }
                        start_position = match.Index;
                        ArrayUniqueOperators[CountUniqueOperators] = match.Value; //GetNameUniqueOperator(index, string_source);
                        CountOperators += GetCountAppealOperator(ArrayUniqueOperators[CountUniqueOperators], 
                                                                  FileSource);
                        Console.WriteLine(ArrayUniqueOperators[CountUniqueOperators]);
                        CountUniqueOperators++;
                        break;
                    }
                } 
            }
            Console.WriteLine("\t------------------------------");
            Console.WriteLine("The count of unique operands = " + CountUniqueOperators);
            Console.WriteLine("The count appeal operands = " + CountOperators);
        }
        static void MetrikaHolstedaPart2(int n1, int n2, int N1, int N2, int n, int N, double V)
        {
           
            double n1_ = 2, n2_ = 2;
            double n_ = n1_ + n2_;
            double V_ = n_ * Math.Log(2, n_);
            double N_ = n1 * Math.Log(2, n1) + n2 * Math.Log(2, n2);
            double L = V_ / V;
            double L_ = (double)(2 * n2) / (n1 * N2);
            double I = L_ * V;
            double E = N_ * Math.Log(2, n / L);
            double E_ = N * Math.Log(2, n / L);
            Console.WriteLine("\nТеоретический объем программы  V* = " + V_ + " (бит)");
            Console.WriteLine("Уровень качества программирования  L = " + L);
            Console.WriteLine("Уровень программы   L* = " + L_);
            Console.WriteLine("Информационное содержание  I = " + I);
            Console.WriteLine("Интеллектуальные усилия по написанию программы E = " + E);
            Console.WriteLine("Затрат на восприятие готовой программы E' = " + E_);
        }
        static void Main(string[] args)
        {
            FileStream file = new FileStream ("e:\\учеба\\ПОИТ 3 семестр\\Метрология\\metrikaHolsteda\\source.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            int n1 = 0, n2 = 0, N1 = 0, N2 = 0;
            Operand(file, reader, out n1, out N1);
            Operator(file, reader, out n2, out N2);
            int n = n1 + n2;
            int N = N1 + N2;
            double V = (double)N * Math.Log(2, n);
            Console.WriteLine("\n~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
            Console.WriteLine("Словарь программы   n =  n1 + n2  = " + n);
            Console.WriteLine("Длина программы     N =  N1 + N2  = " + N);
            Console.WriteLine("Объем программы     V =  N*log(2,n) = " + V);
            MetrikaHolstedaPart2(n1, n2, N1, N2, n, N, V);
            reader.Close();
            Console.ReadKey();
        }  
    }
}