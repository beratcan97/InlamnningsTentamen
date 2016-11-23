using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Tenta_Database
{
    class Program
    {
        static string cns = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            //Fråga 1
            //ProductsByCategoryName("Confections");
            //Fråga 2
            //SalesByTerritory();
            //Fråga 3
            //EmployeesPerRegion();
            //Fråga 4
            //OrdersPerEmployee();
            //Fråga 5
            //CustomersWithNamesLongerThan25Characters();
            Console.ReadLine();
        }
        public static void ProductsByCategoryName(string categoryname)
        {
            var con = new SqlConnection(cns);
            con.Open();
            var cmd = new SqlCommand("SELECT ProductName, UnitPrice, UnitsInStock FROM Products", con);
            cmd.Parameters.AddWithValue("@CategoryName", categoryname);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                {
                    Console.WriteLine("ProductName: {0}\nUnitPrice: {1}\nUnitsInStock: {2}\n", reader["ProductName"], reader["UnitPrice"], reader["UnitsInStock"]);
                }
        }
        public static void SalesByTerritory()
        {
            var con = new SqlConnection(cns);
            con.Open();
            var cmd = new SqlCommand("SELECT TOP (5) Territories.TerritoryDescription, SUM(([Order Details].UnitPrice * [Order Details].Quantity) * (1 + [Order Details].Discount)) AS Total FROM Employees INNER JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID INNER JOIN Orders ON Employees.EmployeeID = Orders.EmployeeID INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID GROUP BY Territories.TerritoryDescription ORDER BY SUM(([Order Details].UnitPrice * [Order Details].Quantity) * (1 + [Order Details].Discount)) DESC", con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("TerritoryDescription: {0}\nTotal: {1}\n", reader["TerritoryDescription"], reader["Total"]);
            }
        }
        public static void EmployeesPerRegion()
        {
            var con = new SqlConnection(cns);
            con.Open();
            var cmd = new SqlCommand("SELECT Region.RegionDescription, COUNT(Employees.EmployeeID) AS Employees FROM Employees INNER JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID INNER JOIN Region ON Territories.RegionID = Region.RegionID GROUP BY Region.RegionDescription", con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                {   
                    Console.WriteLine("Region: {0}\nEmployees: {1}\n", reader["RegionDescription"], reader["Employees"]);
                }
        }
        public static void CustomersWithNamesLongerThan25Characters()
        {
            var db = new NORTHWND();
            var customers = db.Customers.Where(s => s.CompanyName.Length > 25);
            foreach (var item in customers)
            {
                Console.WriteLine(item.CompanyName);
            }
        }
        public static void OrdersPerEmployee()
        {
            var db = new NORTHWND();
            var employee = db.Employees;
            foreach (var item in employee)
                {
                    Console.WriteLine(item.FirstName + " " + item.LastName + " " + item.Orders.Count());
                }
        }
    }
}