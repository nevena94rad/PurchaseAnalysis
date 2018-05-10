using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Baza.DTO
{
    public class Customer
    {
        public List<string> itemNos;
        public List<int> lastPurchases;
        public string custNo;
        public int modelID = 0;

        public static int TotalCount = 0;
        public static int DoneCount = 0;
        public static int totalWrites = 0;
        public static Object thisLock = new Object();
        public static Object thisLock2 = new Object();
        public static event System.Action OnProgressUpdate;
        public static event System.Action<string> OnProgressFinish;
        public static System.ComponentModel.BackgroundWorker worker = null;
        private static ILog log = LogManager.GetLogger(typeof(Customer));

        
        //novo
        public static DateTime GetLastTransactionDate()
        {
            int lastTransactionDate = 0;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select max(" + PurchaseDate + ") from " + Table;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastTransactionDate = (int)reader[0];
                    }
                }
            }

            return DateManipulation.intToDateTime(lastTransactionDate);
        }
        public static List<string> GetAllCustomers(int date)
        {
            List<string> allCustomers = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select distinct(" + CustomerID + ") from " + Table + " where " + PurchaseDate + "< @date and " +
                PurchaseDate + "> @dateMin";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@dateMin",
                    DateManipulation.DateTimeToint(DateManipulation.intToDateTime(date).AddMonths(-Parameters.customerRecency)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCustomers.Add(reader[0].ToString());
                    }
                }
            }

            return allCustomers;
        }

        public static List<string> GetAllItemsWithGPIs(string custNo)
        {
            List<string> itemNos = new List<string>();

            DateTime processingDateDateFormat = DateManipulation.intToDateTime(Parameters.processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string PurchasePeriods_Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string PurchasePeriods_CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string PurchasePeriods_ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string PurchasePeriods_Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PurchasePeriods_PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];
            string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
            string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
            string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];


            string queryString = "select distinct(" + ItemGPI_GPI + ") from ( (select * from " + PurchasePeriods_Table +
                ") a inner join ( select * from " + ItemGPI_Table + ") b on a." + PurchasePeriods_ItemID + "= b." + ItemGPI_ItemID + ") " +
                " where " + PurchasePeriods_CustomerID + "= @custNo" + " and " + PurchasePeriods_PeriodEnd + "<@bDate and " + PurchasePeriods_PeriodEnd + ">@cDate" +
                " and " + ItemGPI_GPI + " is not null" +
                " group by " + ItemGPI_GPI + " having count(*)>1 and max(" + PurchasePeriods_PeriodEnd + ")>@bDateMinusNMonths " +
                " and min(" + PurchasePeriods_Period + ") * 0.5< DATEDIFF(DAY, max(" + PurchasePeriods_PeriodEnd + "), @bDate) + 7" +
                " and max(" + PurchasePeriods_Period + ") * 1.5 > DATEDIFF(DAY, max(" + PurchasePeriods_PeriodEnd + "), @bDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", processingDateDateFormat.ToShortDateString());
                command.Parameters.AddWithValue("@cDate", processingDateDateFormat.AddYears(-2).ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinusNMonths", processingDateDateFormat.AddMonths(-Parameters.customerRecency).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemNos.Add(((String)reader[0]).Replace(" ", String.Empty));
                    }
                }
            }

            return itemNos;
        }

        public static List<string> GetAllItemsWithOUTGPIs(string custNo)
        {
            List<string> itemNos = new List<string>();

            DateTime processingDateDateFormat = DateManipulation.intToDateTime(Parameters.processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];

            string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
            string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
            string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];

            string queryString = "select distinct(" + ItemID + ") from " + Table +
                " where " + CustomerID + "= @custNo" + " and " + PeriodEnd + "<@bDate and " + PeriodEnd + ">@cDate" +
                " group by " + ItemID + " having count(*)>1 and max(" + PeriodEnd + ")>@bDateMinusNMonths " +
                " and min(" + Period + ") * 0.5< DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) + 7" +
                " and max(" + Period + ") * 1.5 > DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) and " + ItemID + " not in" +
                " (select " + ItemGPI_ItemID + " from " + ItemGPI_Table + " where " + ItemGPI_GPI + " is not null)";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", processingDateDateFormat.ToShortDateString());
                command.Parameters.AddWithValue("@cDate", processingDateDateFormat.AddYears(-2).ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinusNMonths", processingDateDateFormat.AddMonths(-Parameters.customerRecency).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemNos.Add(((String)reader[0]).Replace(" ", String.Empty));
                    }
                }
            }

            return itemNos;
        }

        public static List<string> GetAllItems(string custNo)
        {
            List<string> itemNos = new List<string>();

            DateTime processingDateDateFormat = DateManipulation.intToDateTime(Parameters.processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];

            string queryString = "select distinct(" + ItemID + ") from " + Table +
                " where " + CustomerID + "= @custNo" + " and " + PeriodEnd + "<@bDate and " + PeriodEnd + ">@cDate"  +
                " group by " + ItemID + " having count(*)>1 and max(" + PeriodEnd + ")>@bDateMinusNMonths " +
                " and min(" + Period + ") * 0.5< DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) + 7" +
                " and max(" + Period + ") * 1.5 > DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", processingDateDateFormat.ToShortDateString());
                command.Parameters.AddWithValue("@cDate", processingDateDateFormat.AddYears(-2).ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinusNMonths", processingDateDateFormat.AddMonths(-Parameters.customerRecency).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemNos.Add(((String)reader[0]).Replace(" ", String.Empty));
                    }
                }
            }

            return itemNos;
        }

        //PROVERITI zasto bDateMinus6Months???
        public static List<int> GetLastPurchases(string custNo)
        {
            List<int> lastPurchases = new List<int>();

            DateTime processingDateDateFormat = DateManipulation.intToDateTime(Parameters.processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];

            string queryString = "select max(" + PeriodEnd + ") from " + Table +
                " where " + CustomerID + "= @custNo" + " and " + PeriodEnd + "<@bDate" +
                " group by " + ItemID + " having count(*)>1 and max(" + PeriodEnd + ")>@bDateMinus6Months " +
                " and min(" + Period + ") * 0.5< DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) + 7" +
                " and max(" + Period + ") * 1.5 > DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", processingDateDateFormat.ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinus6Months", processingDateDateFormat.AddMonths(-6).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastPurchases.Add(DateManipulation.DateTimeToint((DateTime)reader[0]));
                    }
                }
            }

            return lastPurchases;
        }

        public static int getPurchaseQuantity(string custNo,string item, int date, bool isGPI)
        {
            int sum = 0;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string queryString;

            if (isGPI == false)
            {
                string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
                string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

                queryString = "select sum(" + PurchaseQuantity + ") from " + Table +
                    " where " + CustomerID + "=@custNo and " + ItemID + "=@itemNo and " + PurchaseDate + "=@definedDate";
            }
            else
            {
                string PurchaseHistory_Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string PurchaseHistory_CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string PurchaseHistory_ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseHistory_PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
                string PurchaseHistory_PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

                string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
                string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
                string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];

                queryString = "select sum(" + PurchaseHistory_PurchaseQuantity + ") from " + PurchaseHistory_Table +
                    " where " + PurchaseHistory_CustomerID + "=@custNo and " + PurchaseHistory_PurchaseDate + "=@definedDate and " + PurchaseHistory_ItemID + " in ( select " +
                    ItemGPI_ItemID + " from " + ItemGPI_Table + " where CAST(LEFT(" + ItemGPI_GPI + ", "+ Parameters.gpiDigits +") AS VARCHAR(20)) = @itemNo)";
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                if (isGPI == false)
                    command.Parameters.AddWithValue("@itemNo", item);
                else
                    command.Parameters.AddWithValue("@itemNo", item.Length>= Parameters.gpiDigits ? item.Substring(0, Parameters.gpiDigits): item);
                command.Parameters.AddWithValue("@definedDate", date);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sum = (int)reader[0];
                    }
                }
            }

            return sum;
        }
    }
}

