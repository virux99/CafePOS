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
    public class SalesOperations
    {
        DBAccess db = new DBAccess();
        public void InsertAllCustomerSales(List<CustomerSale> customerSales)
        {
            foreach (var sales in customerSales)
            {
                string query = "Insert INTO CustomerSale (ProductName, Quantity, Total, SaleDate) Values (@ProductName, @Quantity, @Total, @SaleDate)";
                Hashtable parameter = new Hashtable
                {
                    { "@ProductName", sales.ProductName },
                    { "@Quantity", sales.Quantity },
                    { "@Total", sales.Total },
                    { "@SaleDate", sales.SaleDate }
                };
                db.ExecuteQuery(query, parameter);
            }
        }

        public DataTable SelectSale()
        {
            string query = "SELECT * FROM CustomerSale";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public DataTable SelectSale(DateTime from, DateTime to)
        {
            //string query = "SELECT * FROM CustomerSale Where SaleDate BETWEEN '" + from.ToString() + "'AND'" + to.ToString() + "'";
            string query = "SELECT * FROM CustomerSale WHERE SaleDate >= '" + from.AddDays(-1).ToString() + "'AND SaleDate <='" + to.ToString() + "'";

            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public DataTable SelectAllProducts()
        {
            string query = "SELECT * FROM Products";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }



        public void InsertAllEmployeeSales(List<EmployeeSale> employeeSales)
        {
            foreach (var sales in employeeSales)
            {
                string query = "Insert INTO EmployeeSale (ProductName, Quantity, Total, SaleDate, E_ID) Values (@ProductName, @Quantity, @Total, @SaleDate, @E_ID)";
                Hashtable parameter = new Hashtable
                {
                    { "@ProductName", sales.ProductName },
                    { "@Quantity", sales.Quantity },
                    { "@Total", sales.Total },
                    { "@SaleDate", sales.SaleDate },
                    { "@E_ID", sales.E_ID }
                };

                db.ExecuteQuery(query, parameter);
            }
        }
        public void InserProduct(Product product)
        {

            string query = "Insert INTO Products (Name, Type ,Image ,Price) Values (@Name, @Type, @Image, @Price)";
            Hashtable parameter = new Hashtable
                {
                    { "@Name", product.Name },
                    { "@Type", product.Type },
                    { "@Image", product.Image },
                    { "@Price", product.Price }

                };

            db.ExecuteQuery(query, parameter);

        }
        public DataTable SelectProducts()
        {
            string query = "SELECT * FROM Products";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public DataTable SelectEmployeeProducts()
        {
            string query = "SELECT * FROM Products where Type = 'Employee' ";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public DataTable SelectCustomerProducts()
        {
            string query = "SELECT * FROM Products where Type = 'Customer' ";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }


        public DataTable SelectEmployeeProducts(int id)
        {
            string query = "SELECT * FROM Products where Type = 'Employee' AND Id= '"+id+"' ";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }
        public DataTable SelectCustomerProducts(int id)
        {
            string query = "SELECT * FROM Products where Type = 'Customer' AND Id= '" + id+"' ";
            DataTable dt = new DataTable();
            dt = db.GetDataTable(query);
            return dt;
        }

        public void DeleteProduct(string id)
        {
            string query = "DELETE FROM Products WHERE ID='" + id + "'";
            DataTable dt = new DataTable();
            db.ExecuteQuery(query);

        }


    }
}
