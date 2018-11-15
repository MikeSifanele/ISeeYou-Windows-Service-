using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Threading.Tasks;
using System.IO;

namespace ISeeYou
{
    public partial class ISeeYou : ServiceBase
    {
        static StreamWriter myWriter;
        static FileInfo myFile;
        static string[] myLine = new string[2];

        public ISeeYou()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //try
            //{


            string[][] myList = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\x86\Debug\tmp.txt")
                   .Select(l => l.Split('|').Select(i => i).ToArray())
                   .ToArray();

            

            string[,] myLog = JaggedToMultidimensional(myList);

            for (int r = 1; r < myLog.GetLength(0); r++)
            {
                if (myLog[r - 1, 0] == "File Deleted" && myLog[r, 0] == "File Created" && myLog[r - 1, 3] == myLog[r, 3])
                {
                    myLog[r - 1, 0] = "File Moved";
                    myLog[r, 0] = "Skip";
                    myLog[r - 1, 4] = myLog[r, 4];

                }

            }



            //}
            //catch
            //{

            //}

            //try
            //{
            myWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\Activity logs.csv");

            for (int r = 0; r < myLog.GetLength(0); r++)
            {
                if (myLog[r, 0] != "Skip")
                {
                    myWriter.WriteLine((myLog[r, 0] + '|' + myLog[r, 1] + '|' + myLog[r, 2] + '|' + myLog[r, 3] + '|' + myLog[r, 4] + '|' + myLog[r, 5]).TrimEnd('|'));
                }

            }

            myWriter.Close();

            //}
            //catch
            //{

            //}
            //finally
            //{

            //}

        }
        public bool IsFileLocked(string filename)
        {
            bool Locked = false;
            try
            {
                FileStream fs =
                    File.Open(filename, FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None);
                fs.Close();
            }
            catch 
            {
                Locked = true;
            }
            return Locked;
        }
        public T[,] JaggedToMultidimensional<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray.Max(subArray => subArray.Length);
            T[,] array = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                cols = jaggedArray[i].Length;
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }
            return array;
        }
        protected override void OnStop()
        {
            
        }       
    }
}
