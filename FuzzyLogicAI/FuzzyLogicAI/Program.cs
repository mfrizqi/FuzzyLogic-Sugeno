using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FuzzyLogicAI
{
    public class Blt
    {
        public int Num { get; set; }
        public double Income { get; set; }
        public double Debt { get; set; }
        public List<double> IncList { get; set; } = new List<double>();
        public List<double> DebtList { get; set; } = new List<double>();

        public double infAcc { get; set; }
        public double infRej { get; set; }
        public double sugenoVal { get; set; }
        public String StatusBlt { get; set; }

    }

    class Program
    {
        // Menentukan Titik Income High, Med & Low
        static double IncomeHigh(double inc)
        {
            if (inc >= 1.500)
            {
                return 1;
            }
            else if (inc <= 1.320)
            {
                return 0;
            }
            else
            {
                return -1 * ((inc - 1.500) / (1.500 - 1.320));
            }
        }
        static double IncomeMed(double inc)
        {
            if (inc <= 0.75 || inc > 1.400)
            {
                return 0;
            }
            else if (inc > 0.75 && inc <= 0.825)
            {
                return (inc - 0.75) / (0.825 - 0.75);
            }
            else if (inc > 0.825 && inc <= 1.100)
            {
                return 1;
            }
            else
            {
                return (1.400 - inc) / (1.400 - 1.100);
            }
        }
        static double IncomeLow(double inc)
        {

            if (inc <= 0.664)
            {
                return 1;
            }
            else if (inc >= 0.817)
            {
                return 0;
            }
            else
            {
                return (inc - 0.664) / (0.817 - 0.664);
            }
        }

        // Menentukan Titik Debt High, Med & Low
        static double DebtHigh(double debt)
        {
            if (debt >= 47)
            {
                return 1;
            }
            else if (debt <= 38)
            {
                return 0;
            }
            else
            {
                return ((debt - 38) / (47 - 38));
            }
        }
        static double DebtMed(double debt)
        {
            if (debt <= 24 || debt > 50)
            {
                return 0;
            }
            else if (debt > 24 && debt <= 32)
            {
                return (debt - 24) / (32 - 24);
            }
            else if (debt > 32 && debt <= 40)
            {
                return 1;
            }
            else
            {
                return (50 - debt) / (50 - 40);
            }
        }
        static double DebtLow(double debt)
        {

            if (debt <= 25)
            {
                return 1;
            }
            else if (debt >= 30)
            {
                return 0;
            }
            else
            {
                return (debt - 25) / (30 - 25);
            }

        }

        static void Main(string[] args)
        {

            //Memasukan data ke list object BLT
            List<Blt> blts = new List<Blt>();
            var reader = new StreamReader(File.OpenRead("DataTugas2.csv"));
            reader.ReadLine(); // Skip baris kategori

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                var num = Int32.Parse(values[0]);
                var inc = double.Parse(values[1], CultureInfo.InvariantCulture);
                var debt = double.Parse(values[2], CultureInfo.InvariantCulture);
                blts.Add(new Blt { Num = num, Income = inc, Debt = debt });
            }

            //Melakukan Fuzzification 
            foreach (var el in blts)
            {
                el.IncList.Add(IncomeHigh(el.Income));
                el.IncList.Add(IncomeMed(el.Income));
                el.IncList.Add(IncomeLow(el.Income));
                el.DebtList.Add(DebtHigh(el.Debt));
                el.DebtList.Add(DebtMed(el.Debt));
                el.DebtList.Add(DebtLow(el.Debt));
            }

            //Melakukan Inference terhadap inputan
            foreach (var el in blts)
            {
                /* 
                 * * Memasukan Nilai prob tidak diterima pada infRej
                 * 0: High
                 * 1: Med
                 * 2: Low
                 */

                //High Inc Vs High Debt
                if (el.IncList[0] != 0 && el.DebtList[0] != 0)
                {
                    if (el.IncList[0] < el.DebtList[0])
                    {
                        el.infRej = el.IncList[0];
                    }
                    else
                    {
                        el.infRej = el.DebtList[0];
                    }
                }

                //High Inc Vs Mid Debt
                if (el.IncList[0] != 0 && el.DebtList[1] != 0)
                {
                    if (el.IncList[0] < el.DebtList[1])
                    {
                        el.infRej = el.IncList[0];
                    }
                    else
                    {
                        el.infRej = el.DebtList[1];
                    }
                }

                //High Inc Vs Low Debt
                if (el.IncList[0] != 0 && el.DebtList[2] != 0)
                {
                    if (el.IncList[0] < el.DebtList[2])
                    {
                        el.infRej = el.IncList[0];
                    }
                    else
                    {
                        el.infRej = el.DebtList[2];
                    }
                }

                //Med Inc Vs Med Debt
                if (el.IncList[1] != 0 && el.DebtList[1] != 0)
                {
                    if (el.IncList[1] < el.DebtList[1])
                    {
                        el.infRej = el.IncList[1];
                    }
                    else
                    {
                        el.infRej = el.DebtList[1];
                    }
                }

                //Med Inc Vs Low Debt
                if (el.IncList[1] != 0 && el.DebtList[2] != 0)
                {
                    if (el.IncList[1] < el.DebtList[2])
                    {
                        el.infRej = el.IncList[1];
                    }
                    else
                    {
                        el.infRej = el.DebtList[2];
                    }
                }

                //Low Inc Vs Low Debt
                if (el.IncList[2] != 0 && el.DebtList[2] != 0)
                {
                    if (el.IncList[2] < el.DebtList[2])
                    {
                        el.infRej = el.IncList[2];
                    }
                    else
                    {
                        el.infRej = el.DebtList[2];
                    }
                }

                // End Input ke rejectValue


                /*
                 * Memasukan Nilai prob diterima pada infAcc
                 */


                //Med Inc Vs High Debt
                if (el.IncList[1] != 0 && el.DebtList[0] != 0)
                {
                    if (el.IncList[1] < el.DebtList[0])
                    {
                        el.infAcc = el.IncList[1];
                    }
                    else
                    {
                        el.infAcc = el.DebtList[0];
                    }
                }

                //Low Inc Vs High Debt
                if (el.IncList[2] != 0 && el.DebtList[0] != 0)
                {
                    if (el.IncList[2] < el.DebtList[0])
                    {
                        el.infAcc = el.IncList[2];
                    }
                    else
                    {
                        el.infAcc = el.DebtList[0];
                    }
                }

                //Low Inc Vs Med Debt
                if (el.IncList[2] != 0 && el.DebtList[1] != 0)
                {
                    if (el.IncList[2] < el.DebtList[1])
                    {
                        el.infAcc = el.IncList[2];
                    }
                    else
                    {
                        el.infAcc = el.DebtList[1];
                    }
                }
            }

            //Melakukan Defuzzification Sugeno
            const int Accept = 80;
            const int Reject = 45;
            foreach (var el in blts)
            {
                el.sugenoVal = ((el.infAcc * Accept) + (el.infRej * Reject)) / (el.infAcc + el.infRej);
            }

            List<Blt> bltdump = new List<Blt>();
            List<Blt> bltRes = new List<Blt>();
            bltdump = blts;


            //Sort Desc dari field sugenoVal pada llist bltdump
            bltdump.Sort(delegate (Blt x, Blt y)
            {
                return x.sugenoVal.CompareTo(y.sugenoVal);
            });

            bltdump.Reverse();

            int i = 0;
            foreach (var el in bltdump)
            {
                bltRes.Add(el);
                if (i > 18)
                {
                    break;
                }
                i++;
            }

            //Sort Desc dari Field Num pada list bltRes
            bltRes.Sort(delegate (Blt x, Blt y)
            {
                return x.Num.CompareTo(y.Num);
            });

            //Memasukan data 20 Kepala Keluarga ke dalam file csv
            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("Orang");
            foreach (var el in bltRes)
            {
                Console.Write(el.Num + " ");
                int num = el.Num;
                string snum = num.ToString();
                csvcontent.AppendLine(snum);
            }

            string csvpath = "D:/Perkuliahan/Semester 5/AI/tugas/FuzzyLogic-Sugeno/TebakanTugas2.csv";
            File.AppendAllText(csvpath, csvcontent.ToString());

            Console.ReadKey();

        }
    }
}
