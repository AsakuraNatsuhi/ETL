using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using ETL.Utils;

namespace ETL
{
    /// <summary>
    /// Interaction logic for SetDBWindow.xaml
    /// </summary>
    public partial class SetDBWindow : Window
    {
        
        private XDocument mysql;
        private XElement mysqlRoot;
        private XElement mysqlUsername;
        private XElement mysqlPassword;
        private XElement mysqlServer;
        private XElement mysqlDatabase;

        private XDocument oracle;
        private XElement oracleRoot;
        private XElement oracleUsername;
        private XElement oraclePassword;
        private XElement oracleHost;
        private XElement oraclePort;
        private XElement oracleServicename;

        public SetDBWindow()
        {
            InitializeComponent();

            mysql = XDocument.Load(MyPath.MYSQLPATH);
            mysqlRoot = mysql.Root;
            mysqlUsername = mysqlRoot.Element("userid");
            mysqlPassword = mysqlRoot.Element("password");
            mysqlServer = mysqlRoot.Element("server");
            mysqlDatabase = mysqlRoot.Element("database");

            oracle = XDocument.Load(MyPath.ORACLEPATH);
            oracleRoot = oracle.Root;
            oracleUsername = oracleRoot.Element("userid");
            oraclePassword = oracleRoot.Element("password");
            oracleHost = oracleRoot.Element("host");
            oraclePort = oracleRoot.Element("port");
            oracleServicename = oracleRoot.Element("servicename");

            tMysqlUserid.Text = mysqlUsername.Value;
            tMysqlPassword.Text = mysqlPassword.Value;
            tMysqlServer.Text = mysqlServer.Value;
            tMysqlDatabase.Text = mysqlDatabase.Value;

            tOracleUserid.Text = oracleUsername.Value;
            tOraclePassword.Text = oraclePassword.Value;
            tOracleHost.Text = oracleHost.Value;
            tOraclePort.Text = oraclePort.Value;
            tOracleServicename.Text = oracleServicename.Value;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            mysqlRoot.SetElementValue("userid", tMysqlUserid.Text);
            mysqlRoot.SetElementValue("password", tMysqlPassword.Text);
            mysqlRoot.SetElementValue("server", tMysqlServer.Text);
            mysqlRoot.SetElementValue("database", tMysqlDatabase.Text);
            mysql.Save(MyPath.MYSQLPATH);

            oracleRoot.SetElementValue("userid", tOracleUserid.Text);
            oracleRoot.SetElementValue("password", tOraclePassword.Text);
            oracleRoot.SetElementValue("host", tOracleHost.Text);
            oracleRoot.SetElementValue("port", tOraclePort.Text);
            oracleRoot.SetElementValue("servicename", tOracleServicename.Text);
            oracle.Save(MyPath.ORACLEPATH);

            MessageBox.Show("設定が更新されました");
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
