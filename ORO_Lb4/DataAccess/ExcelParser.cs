using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ORO_Lb4.DataAccess
{
    internal record ExcelParser
    {
        DataTable dataTable;
        System.Windows.Point[] _result;

        public ExcelParser(
            in string path, 
            in int sheetIndex,
            in int pointsNumber,
            in int columnX, 
            in int columnY,
            in int rowStartX,
            in int rowStartY
            )
        {
            DataSet? table;
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    table = reader.AsDataSet();

                    // The result of each spreadsheet is in result.Tables
                }

                dataTable = table.Tables[sheetIndex + 4];

                Parse(pointsNumber, columnX, columnY, rowStartX, rowStartY);
            }
        } 

        public System.Windows.Point[] Result
        {
            get 
            {
                return (System.Windows.Point[]) _result.Clone();
            }
            private set
            {
                _result = value;
            }
        }

        private void Parse(
            in int pointsNumber,
            in int columnX,
            in int columnY,
            in int rowStartX,
            in int rowStartY
            )
        {
            _result = new System.Windows.Point[pointsNumber];
            for (int i = 0; i < pointsNumber; i++)
            {
                double valueX = double.Parse(dataTable.Rows[rowStartX + i][columnX].ToString());
                double valueY = double.Parse(dataTable.Rows[rowStartY + i][columnY].ToString());
                _result[i] = new System.Windows.Point(valueX, valueY);
            }
        }
    }
}
