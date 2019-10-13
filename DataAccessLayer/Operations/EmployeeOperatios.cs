using DataAccessLayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Operations
{
    public class EmployeeOperatios
    {
        DBAccess db = new DBAccess();
        public int InsertEmployee(Employee employee)
        {

            string query = "Insert INTO Employee (Name, Department) Values (@Name, @Department); SELECT SCOPE_IDENTITY()";
            Hashtable parameter = new Hashtable();
            parameter.Add("@Name", employee.Name);
            parameter.Add("@Department", employee.Department);
            return db.ExecuteIDQuery(query, parameter);
        }

        public DataTable SelectEmployees()
        {
            string query = "SELECT * FROM Employee";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public void DeleteEmployee(string id)
        {
            string query = "DELETE FROM Employee WHERE ID='" + id + "'";
            DataTable dt = new DataTable();
            db.ExecuteQuery(query);

        }
        public DataTable SelectEmployee(string id)
        {
            string query = "SELECT * FROM Employee WHERE ID='" + id + "'";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;

        }

        public DataTable SelectEmployees(int employeeId, DateTime from, DateTime to)
        {
            string query = "SELECT * FROM EmployeeSale WHERE SaleDate >= '" + from.AddDays(-1).ToString() + "'AND SaleDate <='" + to.ToString() + "'AND E_ID ='" + employeeId + "'";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }

        public DataTable SelectEmployeeSale(DateTime from, DateTime to)
        {
            //string query = "SELECT * FROM CustomerSale Where SaleDate BETWEEN '" + from.ToString() + "'AND'" + to.ToString() + "'";
            string query = "SELECT * FROM EmployeeSale WHERE SaleDate >= '" + from.AddDays(-1).ToString() + "'AND SaleDate <='" + to.ToString() + "'";

            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }

    }
}
