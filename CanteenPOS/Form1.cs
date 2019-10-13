using DataAccessLayer.Models;
using DataAccessLayer.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CanteenPOS
{
    public partial class Form1 : Form
    {
        List<CustomerSale> customerSalesToSendInDB = new List<CustomerSale>();
        List<EmployeeSale> employeeSalesToSendInDB = new List<EmployeeSale>();
        EmployeeOperatios employeeOperatios = new EmployeeOperatios();
        SalesOperations salesOperation = new SalesOperations();
        int order = 1, total = 0;
        int employeeBillOrder = 1, employeeBillTotal = 0;


        public Form1()
        {
            InitializeComponent();
            slidePanel.Height = btnCustomerBill.Height;
            slidePanel.Top = btnCustomerBill.Top;
            btnCustomerBill.BringToFront();
            SumSale();
            ManageAdmin();

            //adding delete buton on manage employee gridview
            var deleteButton = new DataGridViewButtonColumn
            {
                Name = "dataGridViewDeleteButton",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            this.dataGridViewMngEmployee.Columns.Add(deleteButton);




            //adding deete buton on employee bill gridview
            var deleteButtonGrid = new DataGridViewButtonColumn
            {
                Name = "dataGridViewDeleteButton",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            this.dataGridViewEmployeeBill.Columns.Add(deleteButtonGrid);


            //adding deete buton on employee bill gridview
            var deleteButtonBillGrid = new DataGridViewButtonColumn
            {
                Name = "dataGridViewDeleteButton",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            this.dataGridView.Columns.Add(deleteButtonBillGrid);



            //adding deete buton on product gridview
            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "dataGridViewDeleteButton",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            this.dataGridViewProducts.Columns.Add(btnDelete);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            receiptBindingSource.DataSource = new List<Receipt>();
            pnlCustomerBill.BringToFront();

        }

        #region master layout



        private void btnCustomerBill_Click(object sender, EventArgs e)
        {
            receiptBindingSource.Clear();
            order = 1;
            total = 0;
            txtTotal.Text = "";
            txtTotalEmployeeBill.Text = "";

            employeeBillOrder = 1;
            employeeBillTotal = 0;
            pnlCustomerBill.BringToFront();
            slidePanel.Height = btnCustomerBill.Height;
            slidePanel.Top = btnCustomerBill.Top;
            btnCustomerBill.BringToFront();

            AddControlCustomerBillFlowLayoutPanel();

        }

        private void btnEmployeeBill_Click(object sender, EventArgs e)
        {
            receiptBindingSource.Clear();


            order = 1;
            total = 0;
            txtTotal.Text = "";
            txtTotalEmployeeBill.Text = "";

            employeeBillOrder = 1;
            employeeBillTotal = 0;



            pnlEmployeeBill.BringToFront();
            slidePanel.Top = btnEmployeeBill.Top;
            btnEmployeeBill.BringToFront();


            AddControlEmployeeBillFlowLayoutPanel();

        }

        private void btnEmployeeHistory_Click(object sender, EventArgs e)
        {
            slidePanel.Height = btnEmployeeHistory.Height;
            slidePanel.Top = btnEmployeeHistory.Top;
            comboBoxEmployeeHistoryNames.DataSource = null;
            comboBoxEmployeeHistoryNames.DataSource = employeeOperatios.SelectEmployees();
            comboBoxEmployeeHistoryNames.DisplayMember = "Name";
            comboBoxEmployeeHistoryNames.ValueMember = "Id";
            btnEmployeeHistory.BringToFront();
            pnlEmployeeHistory.BringToFront();
        }

        private void btnTotalSale_Click(object sender, EventArgs e)
        {
            slidePanel.Height = btnTotalSale.Height;
            slidePanel.Top = btnTotalSale.Top;
            btnTotalSale.BringToFront();
            pnlTotalSale.BringToFront();
        }

        private void btnManageCustomer_Click(object sender, EventArgs e)
        {
            pnlMngEmployee.BringToFront();
            slidePanel.Height = btnManageCustomer.Height;
            slidePanel.Top = btnManageCustomer.Top;
            btnManageCustomer.BringToFront();
            dataGridViewMngEmployee.DataSource = employeeOperatios.SelectEmployees();



        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnkWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string targetURL = @"http://empactsolutions.co";
            System.Diagnostics.Process.Start(targetURL);
        }


        public void ManageAdmin()
        {
            if (!Password.isAdmin)
            {
                btnEmployeeHistory.Enabled = false;
                btnTotalSale.Enabled = false;
                btnManageCustomer.Enabled = false;
                btnManageProducts.Enabled = false;
            }
            else
            {
                btnEmployeeHistory.Enabled = true;
                btnTotalSale.Enabled = true;
                btnManageCustomer.Enabled = true;
                btnManageProducts.Enabled = true;
            }
        }


        #endregion

        #region CustomerBill

      


        public void AddControlCustomerBillFlowLayoutPanel()
        {

            flowLayoutPanelCustomer.Controls.Clear();
            for (int i = 0; i < salesOperation.SelectCustomerProducts().Rows.Count; i++)
            {
                Button b = new Button
                {

                    Name = salesOperation.SelectCustomerProducts().Rows[i][0].ToString(),
                    BackgroundImage = Image.FromFile(salesOperation.SelectCustomerProducts().Rows[i][3].ToString()),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Height = 100,
                    Width = 100,
                    Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Bold),


                };


                flowLayoutPanelCustomer.Controls.Add(b);
                b.Click += (s, e) =>
                {
                    if (s is Button button)
                    {

                        var selectedProduct = salesOperation.SelectCustomerProducts(Convert.ToInt32(button.Name));
                        if (selectedProduct.Rows.Count > 0)
                        {
                            Receipt obj = new Receipt
                            {
                                Id = order++,
                                ProductName = selectedProduct.Rows[0][1].ToString(),
                                Price = Convert.ToInt32(selectedProduct.Rows[0][4].ToString()),
                                Quantity = 1
                            };
                            total += obj.Price * obj.Quantity;
                            receiptBindingSource.Add(obj);
                            receiptBindingSource.MoveLast();
                            txtTotal.Text = string.Format("RS {0}", total);
                            CustomerSale customerSale = new CustomerSale
                            {
                                ProductName = obj.ProductName,
                                Quantity = obj.Quantity.ToString(),
                                SaleDate = DateTime.Now.ToShortDateString(),
                                Total = obj.Total.ToString()
                            };
                            customerSalesToSendInDB.Add(customerSale);
                        }
                    }
                };

                Label l = new Label
                {
                    Height = 100,
                    Width = 150,
                    Padding = new Padding(12),
                    Text = "Name: " + salesOperation.SelectCustomerProducts().Rows[i][1].ToString() + "\n price:" + salesOperation.SelectCustomerProducts().Rows[i][4].ToString(),

                    Name = "lbl" + salesOperation.SelectCustomerProducts().Rows[i][0].ToString(),



                };
                flowLayoutPanelCustomer.Controls.Add(l);
            }



            flowLayoutPanelCustomer.AutoScroll = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCash.Text))
            {
                salesOperation.InsertAllCustomerSales(customerSalesToSendInDB);
                using (frmprint frm = new frmprint(receiptBindingSource.DataSource as List<Receipt>, string.Format("RS {0}", total), string.Format("RS {0}", txtCash.Text), string.Format("RS {0}", Convert.ToInt32(txtCash.Text) - total), DateTime.Now.ToString()))
                {
                    frm.ShowDialog();
                }
                do
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        try
                        {
                            dataGridView.Rows.Remove(row);
                        }
                        catch (Exception) { }
                    }
                } while (dataGridView.Rows.Count > 0);
                total = 0;
                txtTotal.Text = "0 RS";
            }
            else
                MessageBox.Show("enter cash");
        }


        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == dataGridViewEmployeeBill.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridView.Columns["dataGridViewDeleteButton"].Index)
            {
                var id = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                var totalCOlumnValue = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());

                total -= totalCOlumnValue;
                txtTotal.Text = string.Format("RS {0}", total);
                customerSalesToSendInDB.RemoveAt(customerSalesToSendInDB.Count - 1);
                receiptBindingSource.RemoveAt(e.RowIndex);
            }

        }

        #endregion

        #region EmployeeBill
        public void AddControlEmployeeBillFlowLayoutPanel()
        {

            flowLayoutPanelEmployee.Controls.Clear();
            for (int i = 0; i < salesOperation.SelectEmployeeProducts().Rows.Count; i++)
            {
                Button b = new Button
                {

                    Name = salesOperation.SelectEmployeeProducts().Rows[i][0].ToString(),
                    BackgroundImage = Image.FromFile(salesOperation.SelectEmployeeProducts().Rows[i][3].ToString()),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Height = 100,
                    Width = 100,
                    Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Bold),


                };


                flowLayoutPanelEmployee.Controls.Add(b);
                b.Click += (s, e) =>
                {
                    if (s is Button button)
                    {
                        if (!string.IsNullOrWhiteSpace(txtEmployeeBillID.Text))
                        {
                            var selectedProduct = salesOperation.SelectEmployeeProducts(Convert.ToInt32(button.Name));
                            if (selectedProduct.Rows.Count > 0)
                            {
                                Receipt obj = new Receipt
                                {
                                    Id = employeeBillOrder++,
                                    ProductName = selectedProduct.Rows[0][1].ToString(),
                                    Price = Convert.ToInt32(selectedProduct.Rows[0][4].ToString()),
                                    Quantity = 1
                                };
                                employeeBillTotal += obj.Price * obj.Quantity;
                                receiptBindingSource.Add(obj);
                                receiptBindingSource.MoveLast();
                                txtTotalEmployeeBill.Text = string.Format("RS {0}", employeeBillTotal);
                                EmployeeSale employeeSale = new EmployeeSale
                                {
                                    E_ID = Convert.ToInt32(txtEmployeeBillID.Text),
                                    ProductName = obj.ProductName,
                                    Quantity = obj.Quantity.ToString(),
                                    SaleDate = DateTime.Now.ToShortDateString(),
                                    Total = obj.Total.ToString()
                                };
                                employeeSalesToSendInDB.Add(employeeSale);
                            }
                        }






                    }
                };

                Label l = new Label
                {
                    Height = 100,
                    Width = 150,
                    Padding = new Padding(12),
                    Text = "Name: " + salesOperation.SelectEmployeeProducts().Rows[i][1].ToString() + "\n price:" + salesOperation.SelectEmployeeProducts().Rows[i][4].ToString(),

                    Name = "lbl" + salesOperation.SelectEmployeeProducts().Rows[i][0].ToString(),



                };
                flowLayoutPanelEmployee.Controls.Add(l);
            }



            flowLayoutPanelEmployee.AutoScroll = true;
        }


        private void btnEmployeeBillRemove_Click(object sender, EventArgs e)
        {
            Receipt obj = receiptBindingSource.Current as Receipt;
            if (obj != null)
            {
                employeeBillTotal -= obj.Price * obj.Quantity;
                txtTotalEmployeeBill.Text = string.Format("RS {0}", employeeBillTotal);
                employeeSalesToSendInDB.RemoveAt(employeeSalesToSendInDB.Count - 1);
            }
            receiptBindingSource.RemoveCurrent();
        }

        private void btnEmployeeBillSave_Click(object sender, EventArgs e)
        {
            try
            {
                salesOperation.InsertAllEmployeeSales(employeeSalesToSendInDB);
               // printDocumentEmployeeBill.Print();
                MessageBox.Show("saved");
                using (frmprint frm = new frmprint(receiptBindingSource.DataSource as List<Receipt>, string.Format("RS {0}", employeeBillTotal), string.Format("NILL"), string.Format("NILL"+ "          --"+lblSelectedName.Text+"  --"), DateTime.Now.ToString()))
                {
                    frm.ShowDialog();
                }
                do
                {
                    foreach (DataGridViewRow row in dataGridViewEmployeeBill.Rows)
                    {
                        try
                        {
                            dataGridViewEmployeeBill.Rows.Remove(row);
                        }
                        catch (Exception) { }
                    }
                } while (dataGridViewEmployeeBill.Rows.Count > 0);
                employeeBillTotal = 0;
                txtTotalEmployeeBill.Text = "0 RS";
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        private void dataGridViewEmployeeBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == dataGridViewEmployeeBill.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridViewEmployeeBill.Columns["dataGridViewDeleteButton"].Index)
            {
                //Put some logic here, for example to remove row from your binding list.

                int columnTotal = Convert.ToInt32(dataGridViewEmployeeBill.Rows[e.RowIndex].Cells[4].Value.ToString());
                employeeBillTotal -= columnTotal;
                txtTotalEmployeeBill.Text = (employeeBillTotal).ToString();
                employeeSalesToSendInDB.RemoveAt(e.RowIndex);
                receiptBindingSource.RemoveAt(e.RowIndex);

            }
        }
        #endregion

        #region TotalSale

        private void btnExportTotalSale_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;
           

            for (i = 0; i <= dataGridViewTotalSale.RowCount - 1; i++)
            {
                for (j = 0; j <= dataGridViewTotalSale.ColumnCount - 1; j++)
                {
                    DataGridViewCell cell = dataGridViewTotalSale[j, i];
                    xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                }
            }

            xlWorkBook.SaveAs(Application.StartupPath + DateTime.Now.Ticks.ToString() + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created");
        }

        private void lnkShowAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var saleFromDb = salesOperation.SelectSale();
            dataGridViewTotalSale.DataSource = saleFromDb;
            SumSale();
        }



        private void btnShowDateRangeRecord_Click(object sender, EventArgs e)
        {
            var saleFromDb = salesOperation.SelectSale(dtePiclerFrom.Value, dtePickerTo.Value);
            dataGridViewTotalSale.DataSource = saleFromDb;
            SumSale();
        }



        public void SumSale()
        {
            lblTotalSale.Text = "Total Sale: " + (from DataGridViewRow row in dataGridViewTotalSale.Rows
                                                  where row.Cells[2].FormattedValue.ToString() != string.Empty
                                                  select Convert.ToInt32(row.Cells[3].FormattedValue)).Sum().ToString();
        }


        #endregion

        #region Employee History
        private void btnEmployeeHistorySelect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxEmployeeHistoryNames.Text))
            {
                dataGridViewEmployeeHistory.DataSource = employeeOperatios.SelectEmployees(Convert.ToInt32(comboBoxEmployeeHistoryNames.SelectedValue.ToString()),
                                                                                           dateTimePickerEmployeeFromHistoryDetails.Value,
                                                                                           dateTimePickerEmployeeToHistoryDetails.Value);
                SumEmployeeSale();
            }
        }
        public void SumEmployeeSale()
        {
            lblEmployeeTotalSale.Text = "Total Sale: " + (from DataGridViewRow row in dataGridViewEmployeeHistory.Rows
                                                          where row.Cells[2].FormattedValue.ToString() != string.Empty
                                                          select Convert.ToInt32(row.Cells[3].FormattedValue)).Sum().ToString();
        }

        private void btnEmployeeHistoryAllSelect_Click(object sender, EventArgs e)
        {
            dataGridViewEmployeeHistory.DataSource = employeeOperatios.SelectEmployeeSale(dateTimePickerEmployeeSaleFrom.Value, dateTimePickerEmployeeSaleTo.Value);
            SumEmployeeSale();
        }

        private void lblNewProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewProduct newProduct = new NewProduct();
            newProduct.Show();

        }

        private void btnExportExcelEmployeeHistory_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;

            for (i = 0; i <= dataGridViewEmployeeHistory.RowCount - 1; i++)
            {
                for (j = 0; j <= dataGridViewEmployeeHistory.ColumnCount - 1; j++)
                {
                    DataGridViewCell cell = dataGridViewEmployeeHistory[j, i];
                    xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                }
            }

            xlWorkBook.SaveAs(Application.StartupPath + DateTime.Now.Ticks.ToString() + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnEmployeePrint_Click(object sender, EventArgs e)
        {
            printDocumentEmployee.Print();
        }

        private void printDocumentEmployee_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridViewEmployeeHistory.Width, this.dataGridViewEmployeeHistory.Height);
            dataGridViewEmployeeHistory.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridViewEmployeeHistory.Width, this.dataGridViewEmployeeHistory.Height));
            e.Graphics.DrawImage(bm, 0, 0);

        }
        #endregion

        #region Employee Management
        private void btnMngEmployeeAdd_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee
            {
                Name = txtMngEmployeeName.Text,
                Department = txtMngEmployeeDepartment.Text
            };
            var id = employeeOperatios.InsertEmployee(employee);
            MessageBox.Show("Inserted ID= " + id.ToString());
            dataGridViewMngEmployee.DataSource = employeeOperatios.SelectEmployees();
            txtMngEmployeeName.Text = string.Empty;
            txtMngEmployeeDepartment.Text = string.Empty;
        }
      

        private void dataGridViewMngEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == dataGridViewMngEmployee.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridViewMngEmployee.Columns["dataGridViewDeleteButton"].Index)
            {
                //Put some logic here, for example to remove row from your binding list.
                string myvalue = dataGridViewMngEmployee.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                employeeOperatios.DeleteEmployee(myvalue);
                dataGridViewMngEmployee.DataSource = employeeOperatios.SelectEmployees();

            }
        }



        #endregion

        #region Product Management
        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            dataGridViewProducts.DataSource = salesOperation.SelectProducts();
            slidePanel.Height = btnManageProducts.Height;
            slidePanel.Top = btnManageProducts.Top;
            pnlMngProducts.BringToFront();



        }

        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == dataGridViewProducts.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridViewProducts.Columns["dataGridViewDeleteButton"].Index)
            {

                string myvalue = dataGridViewProducts.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                salesOperation.DeleteProduct(myvalue);
                dataGridViewProducts.DataSource = salesOperation.SelectAllProducts();

            }
        }

        private void txtEmployeeBillID_TextChanged(object sender, EventArgs e)
        {
            if (IsDigitsOnly(txtEmployeeBillID.Text))
            {
                lblSelectedName.Text = "NaN";
                var selectedEmployee = employeeOperatios.SelectEmployee(txtEmployeeBillID.Text);
                if (selectedEmployee.Rows.Count > 0)
                {
                    lblSelectedName.Text = "Name:" + selectedEmployee.Rows[0][1].ToString();
                }
            }
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

       

        private void btnMngProductUploadImage_Click(object sender, EventArgs e)
        {
            lblAddress.Text = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {

                Title = "Browse Image",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lblAddress.Text = openFileDialog1.FileName;
            }
        }



        private void btnMngInsertProduct_Click(object sender, EventArgs e)
        {
            string pathToNewFolder = System.IO.Path.Combine(Application.StartupPath, "images") + "\\";
            DirectoryInfo directory = Directory.CreateDirectory(pathToNewFolder);
            string dest = pathToNewFolder + DateTime.Now.Ticks.ToString() + Path.GetFileName(lblAddress.Text);
            if (lblAddress.Text != "")
            {
                File.Copy(lblAddress.Text, dest);
            }


            if (txtMngEmployeeName != null && cmboMngProductType.Text != null && lblAddress.Text != "")
            {
                Product product = new Product
                {
                    Name = txtMngProductName.Text,
                    Type = cmboMngProductType.Text,
                    Price = txtMngProductPrice.Text,
                    Image = dest
                };
                salesOperation.InserProduct(product);
                MessageBox.Show("Inserted");
                dataGridViewProducts.DataSource = salesOperation.SelectProducts();
            }
            else
                MessageBox.Show("please fill out the info");

        }
        #endregion
    }
}
