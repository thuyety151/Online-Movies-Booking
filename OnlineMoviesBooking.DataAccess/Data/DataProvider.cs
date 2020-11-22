using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OnlineMoviesBooking.DataAccess.Data
{
    public class DataProvider
    {
        private static DataProvider instance;

        private string connectionSTR = "Data Source=DESKTOP-HLV4F7T;Initial Catalog=QuanLyQuanCaPhe;Integrated Security=True";

        //string conn = Configuration.GetConnectionString("DefaultConnection");
        public static DataProvider Instance  // Ctrl +R+E
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return DataProvider.instance;
            }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }

        public DataTable ExcuteQuery(string query, object[] parameter = null)  // chỉ đưa null ở cuối cùng
        {

            // sử dụng using để giải phóng bộ nhớ
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open(); // mở data ,nhớ có đóng

                SqlCommand command = new SqlCommand(query, connection); // thực thi câu query trên connection đó

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }
            return data;

        }

        // số dòng thành công
        public int ExcuteNonQuery(string query, object[] parameter = null)  // chỉ đưa null ở cuối cùng
        {
            int data = 0;
            // sử dụng using để giải phóng bộ nhớ
            //DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open(); // mở data ,nhớ có đóng

                SqlCommand command = new SqlCommand(query, connection); // thực thi câu query trên connection đó

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }
            return data;

        }

        // số lượng ( count)
        public object ExcuteScalar(string query, object[] parameter = null)  // chỉ đưa null ở cuối cùng
        {
            object data = 0;
            // sử dụng using để giải phóng bộ nhớ
            //DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open(); // mở data ,nhớ có đóng

                SqlCommand command = new SqlCommand(query, connection); // thực thi câu query trên connection đó

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();
            }
            return data;

        }

    }
}
