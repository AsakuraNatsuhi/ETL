using ETL.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using ETL.Dao;
using ETL.Dao.impl;
using NPOI.HSSF.UserModel;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace ETL
{

    public partial class SubWindow1 : Window
    {
        private int sqlCount;
        private string target = "";
        private GetMysqlDataDao getMysqlDataDao;
        private GetOracleDataDao getOracleDataDao;
        private Stopwatch sw;

        public SubWindow1()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            getMysqlDataDao = new GetMysqlDataDaoImpl();
            getOracleDataDao = new GetOracleDataDaoImpl();
            sw = new Stopwatch();
            //删除缓存文件
            File.Delete(MyPath.PATHRESULT);
            File.Delete(MyPath.PATHINPUT);
            File.Delete(MyPath.PATHOUTPUT);
            InitializeComponent();

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.MainWindow = mw;
            this.Close();
            mw.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void btnOpenInputFileExcel_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            DataTable dt = Tools.ImportExcelFile();
            sw.Stop();

            if (null == dt)
            {
                return;
            }

            int count = dt.Rows.Count;

            bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHINPUT);

            if (!flag || dt == null)
            {
                MessageBox.Show("インポート失敗です、やり直してください");
                return;
            }

            inputLableTitle.Visibility = Visibility.Visible;
            inputLableResult.Visibility = Visibility.Visible;

            inputLableResult.Content = count + "組のデータがexcelから検索されました、掛かった時間は" + sw.Elapsed + "です";

        }

        private void btnOpenInputFileTxt_Click(object sender, RoutedEventArgs e)
        {

            sw.Start();
            DataTable dt = Tools.ImportTxtFile();
            sw.Stop();

            if (null == dt)
            {
                return;
            }

            bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHINPUT);

            int count = dt.Rows.Count;

            if (!flag || dt == null)
            {
                MessageBox.Show("インポート失敗です、やり直してください");
                return;
            }

            inputLableTitle.Visibility = Visibility.Visible;
            inputLableResult.Visibility = Visibility.Visible;

            inputLableResult.Content = count + "組のデータがテキストから検索されました、掛かった時間は" + sw.Elapsed + "です";
        }

        private void btnOpenOutputFileExcel_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            DataTable dt = Tools.ImportExcelFile();
            sw.Stop();

            if (null == dt)
            {
                return;
            }

            bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHOUTPUT);

            int count = dt.Rows.Count;

            if (!flag || dt == null)
            {
                MessageBox.Show("インポート失敗です、やり直してください");
                return;
            }

            outputLableTitle.Visibility = Visibility.Visible;
            outputLableResult.Visibility = Visibility.Visible;

            outputLableResult.Content = count + "組のデータがexcelから検索されました、掛かった時間は" + sw.Elapsed + "です";


        }

        private void btnOpenOutputFileDb_Click(object sender, RoutedEventArgs e)
        {
            string sql = t1.Text.ToLower();
            bool getTable = true;

            if (!(sql.StartsWith("select") && sql.Contains("from")))
            {
                MessageBox.Show("正しいSQL文を入力してください");
                return;
            }

            if (rMysql.IsChecked == true)
            {
                string message = "";
                sw.Start();
                MySqlConnection conn = getMysqlDataDao.GetMysqlConnection();
                // 校验连接是否正确
                if (!getMysqlDataDao.OpenConnection(conn, out message))
                {
                    MessageBox.Show(message);
                    return;
                }

                DataTable dt = getMysqlDataDao.GetDataTableFromMysql(sql, conn, out getTable);
                sw.Stop();
                int count = dt.Rows.Count;
                if (!getTable)
                {
                    MessageBox.Show("SQL文法エーラー");
                    return;
                }
                if (null == dt)
                {
                    MessageBox.Show("検索されたデータがないです");
                    return;
                }

                bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHOUTPUT);

                if (!flag || dt == null)
                {
                    MessageBox.Show("インポート失敗です、やり直してください");
                    return;
                }
                outputLableTitle.Visibility = Visibility.Visible;
                outputLableResult.Visibility = Visibility.Visible;

                outputLableResult.Content = count + "組のデータmysqlからが検索されました、掛かった時間は" + sw.Elapsed + "です";
                return;
            }
            else
            {
                string message = "";
                sw.Start();
                OracleConnection conn = getOracleDataDao.GetOracleConnection();
                // 校验连接是否正确
                if (!getOracleDataDao.OpenConnection(conn, out message))
                {
                    MessageBox.Show(message);
                    return;
                }

                DataTable dt = getOracleDataDao.GetDataTableFromOracle(sql, conn, out getTable);
                sw.Stop();
                int count = dt.Rows.Count;
                if (!getTable)
                {
                    MessageBox.Show("SQL文法エーラー");
                    return;
                }
                if (null == dt)
                {
                    MessageBox.Show("検索されたデータがないです");
                    return;
                }

                bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHOUTPUT);

                if (!flag || dt == null)
                {
                    MessageBox.Show("インポート失敗です、やり直してください");
                    return;
                }
                outputLableTitle.Visibility = Visibility.Visible;
                outputLableResult.Visibility = Visibility.Visible;

                outputLableResult.Content = count + "組のデータがoracleから検索されました、掛かった時間は" + sw.Elapsed + "です";
                return;
            }

        }

        private void btnReadOutputExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!IoHelper.Exists(MyPath.PATHOUTPUT))
            {
                MessageBox.Show("先にデータベースやエクセルからインポートしてください");
                return;
            }

            target = Tools.OpenExcelFileDialog();
            if ("".Equals(target))
            {
                return;
            }

            if (Tools.DataTableToExcel(Tools.TxtToDataTable(MyPath.PATHOUTPUT), target, "Output"))
            {
                MessageBox.Show("Excelにインポート成功です");
            }
            else
            {
                MessageBox.Show("Excelにoutputsheetが既に存在しているため、インポート失敗です");
            }
        }

        private void btnReadInputExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!IoHelper.Exists(MyPath.PATHINPUT))
            {
                MessageBox.Show("先にテキストやエクセルからインポートしてください");
                return;
            }

            target = Tools.OpenExcelFileDialog();
            if ("".Equals(target))
            {
                return;
            }

            if (Tools.DataTableToExcel(Tools.TxtToDataTable(MyPath.PATHINPUT), target, "Input"))
            {
                MessageBox.Show("Excelにインポート成功です");
            }
            else
            {
                MessageBox.Show("Excelにinputsheetが既に存在しているため、インポート失敗です");
            }
        }

        private void t1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sqlCount != 0)
            {
                return;
            }
            sqlCount++;
            t1.Text = "";
        }

        private void btnSetDB_Click(object sender, RoutedEventArgs e)
        {
            SetDBWindow sdb = new SetDBWindow();
            Application.Current.MainWindow = sdb;
            sdb.Show();
        }

        private void btnOpenOutputFileTxt_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            DataTable dt = Tools.ImportTxtFile();
            sw.Stop();

            if (null == dt)
            {
                return;
            }

            bool flag = Tools.SaveDataTableToTxt(dt, MyPath.PATHOUTPUT);

            int count = dt.Rows.Count;

            if (!flag || dt == null)
            {
                MessageBox.Show("インポート失敗です、やり直してください");
                return;
            }

            outputLableTitle.Visibility = Visibility.Visible;
            outputLableResult.Visibility = Visibility.Visible;

            outputLableResult.Content = count + "組のデータがテキストから検索されました、掛かった時間は" + sw.Elapsed + "です";


        }

    }

}



