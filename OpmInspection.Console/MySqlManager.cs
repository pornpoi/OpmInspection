using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpmInspection.Console
{
    partial class Program
    {
        /// <summary>
        /// สร้าง mysql function ลง database server
        /// </summary>
        /// <param name="name">ชื่อ function</param>
        /// <returns>ข้อความแจ้งผลลัพท์</returns>
        static string CreateFunction(string name)
        {
            if (name == "truncatetime")
            {
                if (!Program.FunctionIsExist(name))
                {
                    using (var connection = new MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
                    using (var command = connection.CreateCommand())
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("CREATE DEFINER=`root`@`localhost` FUNCTION `TruncateTime`(dateValue DateTime) RETURNS date");
                        sb.AppendLine("BEGIN");
                        sb.AppendLine("RETURN Date(dateValue);");
                        sb.AppendLine("END");

                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = sb.ToString();

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    return "Create \"TruncateTime\" successfully.";
                }
                else
                {
                    return "\"TruncateTime\" already exist.";
                }
            }

            return "Invalid function name.";
        }

        /// <summary>
        /// ตรวจสอบว่าใน database server มี function นี้อยู่หรือไม่
        /// </summary>
        /// <param name="name">ชื่อ function</param>
        /// <returns>bool</returns>
        static bool FunctionIsExist(string name)
        {
            int result = 0;

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT COUNT(ROUTINE_NAME) as Result");
                sb.AppendLine("FROM INFORMATION_SCHEMA.ROUTINES");
                sb.AppendLine("WHERE ROUTINE_TYPE = @RoutineType");
                sb.AppendLine("AND ROUTINE_SCHEMA = @RoutineSchema");
                sb.AppendLine("AND ROUTINE_NAME = @RoutineName;");

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sb.ToString();
                command.Parameters.AddWithValue("@RoutineType", "FUNCTION");
                command.Parameters.AddWithValue("@RoutineSchema", "opminspection");
                command.Parameters.AddWithValue("@RoutineName", name);

                connection.Open();
                result = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }

            return result > 0;
        }
    }
}
