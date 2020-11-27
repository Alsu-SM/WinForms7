using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms7
{
    public class Matrix
    {

        int CityCount;
        double[,] distanceMatrix;

        public Matrix (int n, double[,] M)

        {
            CityCount = n;
            distanceMatrix = new double[n, n];
            distanceMatrix = M;
        }

        public int funcCityCount
        {
            get { return CityCount; } 
        }

        public double[,] funcMatrix
        {
            get { return distanceMatrix; }
        }

        public List<int> Algorithm() // метод жадного алгоритма, возвращает список городов
        {
            List<int> solution = new List<int>();

            int currentCity = 0;
            int chosenCity = 0;
            solution.Add(0);
            double currentMin = 1000;

            while (solution.Count != CityCount)
            {
                for (int i = 0; i < this.CityCount; i++)
                {
                    if (this.distanceMatrix[currentCity, i] < currentMin)
                        if(!solution.Contains(i))
                        {
                            currentMin = this.distanceMatrix[currentCity, i];
                            chosenCity = i;
                        }
                    
                }

                solution.Add(chosenCity);
                currentCity = chosenCity;
                currentMin = 1000;
            }

            solution.Add(0);


            return solution;

        }

        public double Aim(List<int> solution) // метод принимает список городов и возвращает критерий
        {
            double aim = 0;
            for (int i = 0; i < CityCount; i++)
            {
                aim += distanceMatrix[solution[i], solution[i+1]];
            }

            return aim;
        }
    }
}
