using System;
using System.Collections.Generic;

namespace MatrixLab
{
    class Program
    {
        public static double detCalculating(double[,] matrix)
        {
            if (!squareMartix(matrix))
                throw new Exception("Матрица не квадратная");

            double result = 0;
            int size = matrix.GetLength(0);
            
            if (size == 1)
            {
                result += matrix[0, 0];
            } 
            else if (size == 2)
            {
                result = matrix[0, 0] * matrix[1, 1] - (matrix[0, 1] * matrix[1, 0]);
            }
            else
            {
                for (int k = 0; k < size; k++)
                {
                    double[,] submatrix = new double[size - 1, size - 1];
                    List<double> matrixlist = new List<double>();
                    for (int i = 1; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (j != k)
                            {
                                matrixlist.Add(matrix[i, j]);
                            }
                        }
                    }

                    int count = 0;
                    for (int i = 0; i < size - 1; i++)
                    {
                        for (int j = 0; j < size - 1; j++)
                        {
                            submatrix[i, j] = matrixlist[count];
                            count++;
                        }
                    }

                    if (k % 2 == 0)
                        result += matrix[0, k] * detCalculating(submatrix);
                    else
                        result += -1 * matrix[0, k] * detCalculating(submatrix);
                }
            }
            return result;
        }
    

        public static bool squareMartix(double[,]matrix)
        {
            if (matrix.GetLength(0) == matrix.GetLength(1))
                return true;

            return false;
        }

        public static double[,] RedhefferMatrixGenerator(int n)
        {
            double[,] RedhefferMatrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (((j + 1) % (i + 1) == 0) || (j + 1 == 1))
                        RedhefferMatrix[i, j] = 1;
                    else
                        RedhefferMatrix[i, j] = 0;
                }
            }
            return RedhefferMatrix;
        }

        public static void CramerMethrod(double[,]matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (rows != cols - 1)
                throw new Exception("Система линейных уравнений не является Крамеровской");

            double[,] mainMatrix = new double[rows,cols-1];
            List<double> matrixRootlist = new List<double>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (j == cols-1)
                        matrixRootlist.Add(matrix[i, j]);
                    else
                        mainMatrix[i, j] = matrix[i, j];
                }
            }

            if (detCalculating(mainMatrix) != 0)
            {
                for (int k = 0; k < rows; k++)
                {
                    int count = 0;
                    double[,] rootMatrix = new double[rows, rows];

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            if (j == k)
                            {
                                rootMatrix[i, j] = matrixRootlist[count];
                                count++;
                            }
                            else
                                rootMatrix[i, j] = mainMatrix[i, j];
                        }
                    }
                    Console.WriteLine($"Корень {k + 1} = {detCalculating(rootMatrix) / detCalculating(mainMatrix)}");
                }
            }
            else
            {
                bool noRoots = false;

                for (int k = 0; k < rows; k++)
                {
                    int count = 0;
                    double[,] rootMatrix = new double[rows, rows];

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            if (j == k)
                            {
                                rootMatrix[i, j] = matrixRootlist[count];
                                count++;
                            }
                            else
                                rootMatrix[i, j] = mainMatrix[i, j];
                        }
                    }

                    if (detCalculating(rootMatrix) != 0)
                    {
                        Console.WriteLine("Система не имеет решений");
                        noRoots = true;
                        break;
                    }
                }

                if (!noRoots)
                    Console.WriteLine("Система имеет бесконечное число решений");
            }
        }

        public static void PrintMatrix(double[,]matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void userInterface()
        {
            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("(найти определитель, вычислить определитель матрицы Редхеффера, решить систему линейных уравнений методом Крамера,запустить тесты)");
            string input = Console.ReadLine();
            if (input == "найти определитель")
            {
                Console.WriteLine("Какой порядок имеет матрица?");
                string input_matrix = Console.ReadLine();
                int size = Convert.ToInt32(input_matrix);
                double[,] matrix = new double[size, size];
                Console.WriteLine("Введите все элементы матрицы по строкам");
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                    Console.WriteLine($"строка под номером {i + 1} заполнена");
                }
                Console.WriteLine("-------------------");
                Console.WriteLine("Матрица:");
                PrintMatrix(matrix);
                Console.WriteLine("-------------------");
                Console.WriteLine("Определитель матрицы:");
                Console.WriteLine(detCalculating(matrix));
            }
            else if (input == "вычислить определитель матрицы Редхеффера")
            {
                Console.WriteLine("Какой порядок имеет матрица?");
                string input_matrix = Console.ReadLine();
                int size = Convert.ToInt32(input_matrix);
                double[,] matrix = RedhefferMatrixGenerator(size);
                Console.WriteLine("-------------------");
                Console.WriteLine("Матрица:");
                PrintMatrix(matrix);
                Console.WriteLine("-------------------");
                Console.WriteLine("Определитель матрицы:");
                Console.WriteLine(detCalculating(matrix));
            }
            else if (input == "решить систему линейных уравнений методом Крамера")
            {
                Console.WriteLine("Введи количество уравнений");
                string input_rows = Console.ReadLine();
                int rows = Convert.ToInt32(input_rows);
                Console.WriteLine("Введи количество переменных");
                string input_cols = Console.ReadLine();
                int cols = Convert.ToInt32(input_cols) + 1;
                double[,] matrix = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                    Console.WriteLine($"уравнение под номером {i+1} записано");
                }
                Console.WriteLine("-------------------");
                Console.WriteLine("Матрица:");
                PrintMatrix(matrix);
                Console.WriteLine("-------------------");
                CramerMethrod(matrix);
            }
            else if (input == "запустить тесты")
            {
                test();
            }
            else throw new Exception("Некорректный ввод");
        }

        public static void test()
        {
            double[,] matrix = new double[,] {{1,2,3},{4,5,6 },{7,8,9}};
            if (detCalculating(matrix) != 0)
                Console.WriteLine("err1");

            double[,] matrix_2 = new double[,] { { 10, 45, 70,14 }, { 51, 80, 45, 31 }, { 1, 2, 7, 8}, { 100, 5, 6, 2 } };
            if (detCalculating(matrix_2) != 2691202)
                Console.WriteLine("err2");

            double[,] matrix_3 = new double[,] { {25} };
            if (detCalculating(matrix_3) != 25)
                Console.WriteLine("err3");

            double[,] matrix_4 = new double[,] { { 100,22 },{543,23 } };
            if (detCalculating(matrix_4) != -9646)
                Console.WriteLine("err4");

            double[,] matrix_5 = new double[,] { { 25,54,61,56,321,5 },{25,54,61,56,321,5},{ 25,54,61,56,321,5 },
                                                 { 25,54,61,56,321,5 },{ 25,54,61,56,321,5 },{ 25,54,61,56,321,5 }};
            if (detCalculating(matrix_5) != 0)
                Console.WriteLine("err5");

            double[,] matrix_6 = new double[,] { {10,20, 30, 40, 50, 60 ,70, 80},
                                                 {11, 22, 33, 44, 55, 66, 77, 88},
                                                {12 ,23, 34, 45, 56, 67, 78, 89},
                                                {13, 24, 35, 46, 57 ,68 ,79 ,90},
                                                {14 ,25 ,36 ,47, 58, 69 ,80 ,91},
                                                {15, 26, 37, 48, 59, 70, 81 ,92},
                                                {16 ,27, 38, 49, 60 ,71 ,82 ,93},
                                                {17 ,28 ,39 ,50 ,61 ,72, 83 ,94} };
            if (detCalculating(matrix_6) != 0)
                Console.WriteLine("err6");

            if (detCalculating(RedhefferMatrixGenerator(1)) != 1)
                Console.WriteLine("err7");

            if (detCalculating(RedhefferMatrixGenerator(2)) != 0)
                Console.WriteLine("err8");

            if (detCalculating(RedhefferMatrixGenerator(3)) != -1)
                Console.WriteLine("err9");

            if (detCalculating(RedhefferMatrixGenerator(4)) != -1)
                Console.WriteLine("err10");

            if (detCalculating(RedhefferMatrixGenerator(5)) != -2)
                Console.WriteLine("err11");

            if (detCalculating(RedhefferMatrixGenerator(6)) != -1)
                Console.WriteLine("err12");

            if (detCalculating(RedhefferMatrixGenerator(7)) != -2)
                Console.WriteLine("err13");

            if (detCalculating(RedhefferMatrixGenerator(8)) != -2)
                Console.WriteLine("err14");

            Console.WriteLine("Ответы: 3 , -4 , 1");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 6,2,-5,5 }, { 1,3,-1,-10 }, {3, -5, -4, 25} });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: 1 , 1 , 1");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 2, 3, -1, 4 }, { 1, 1, 3, 5 }, { 3, -4, 1, 0 } });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: -2 , 1 , -1");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 1, 5, 2, 1 }, { 2, 3, 2, -3 }, { 1, 3, 4, -3 } });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: бесконечное число решений");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 1,1,0 }, { 2, 2, 0} });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: бесконечное число решений");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 4,-8, 2 }, { 2,-4,1} });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: нет решений");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 3,-1,2 }, {3,-1,5} });
            Console.WriteLine("-------------------");

            Console.WriteLine("");
            Console.WriteLine("Ответы: нет решений");
            Console.WriteLine("-------------------");
            CramerMethrod(new double[,] { { 1, 1, 4 }, { 2, 2, 12 } });
            Console.WriteLine("-------------------");

            Console.WriteLine("Конец тестов");
        }
        static void Main(string[] args)
        {
            userInterface();
        }
    }
}
