using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KAutoHelper;
using MySql.Data.MySqlClient;
using OpenQA.Selenium.Chrome;




namespace capbu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }   
        
        public void Chrome()
        {
            ChromeOptions option = new ChromeOptions();
            ChromeDriver chromeDriver = new ChromeDriver(option);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // mo trihnh duyet chrome
            ChromeOptions option = new ChromeOptions();
            ChromeDriver chromeDriver = new ChromeDriver(option);
            chromeDriver.Url = "https://vnpt-ca.vn/certificate/logOn.xhtml";

            var username = chromeDriver.FindElementByXPath("/html/body/section[3]/div/div/div[1]/form/div[2]/input[4]");
            //MessageBox.Show(user.Text);
            //user.SendKeys("quynh trang");
            //user.Submit();
            username.SendKeys("cto-tms");

            //mat khau /html/body/section[3]/div/div/div[1]/form/div[2]/input[5]
            var password = chromeDriver.FindElementByXPath("/html/body/section[3]/div/div/div[1]/form/div[2]/input[5]");
            password.SendKeys("123456");
            password.Submit();

            //vao dia chi gia han
            chromeDriver.Url = "https://vnpt-ca.vn/certificate/renewByExtension";
            chromeDriver.Navigate();

            //pass gen
            var passgen = chromeDriver.FindElementByXPath("/html/body/div[2]/div[1]/div[3]/table/tbody/tr[2]/td/div/form/div/table[1]/tbody/tr[3]/td[2]/input");
            passgen.SendKeys("admin#999");

            //load token 
            Delay(2);
            var token = chromeDriver.FindElementByXPath("/html/body/div[2]/div[1]/div[3]/table/tbody/tr[2]/td/div/form/div/table[1]/tbody/tr[6]/td[2]/input[2]");
            token.Click();

            
            //click chon token
            Delay(30);
            var ctoken = chromeDriver.FindElementByXPath("/html/body/div[4]/div[2]/table/tbody/tr/td[1]/a");
            ctoken.Click();

            //test ket noi DB
            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                Console.WriteLine("Openning Connection ...");
                conn.Open();
                //MessageBox.Show("Connection successful!");
                String select = "SELECT oid,seri FROM capbu limit 2";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //MessageBox.Show(rdr[0].ToString()+ "---" +rdr[1].ToString());
                    //oid
                    var oid = chromeDriver.FindElementByXPath("/html/body/div[2]/div[1]/div[3]/table/tbody/tr[2]/td/div/form/div/table[1]/tbody/tr[2]/td[2]/input");
                    oid.SendKeys(rdr[0].ToString());

                    

                    //nut kich hoat
                    var kichhoat = chromeDriver.FindElementByXPath("/html/body/div[2]/div[1]/div[3]/table/tbody/tr[2]/td/div/form/div/table[2]/tbody/tr[6]/td[2]/input[1]");
                    kichhoat.Click();


                    //nhap pin 2 lan
                    inputPin();

                    //xoa oid
                    Delay(3);
                    oid.Clear();



                }
            }
            catch (Exception loi)
            {
                MessageBox.Show("Error: " + loi.Message );
            }
            




            void Delay(double delay)
            {
                double delayTime = 0;
                while (delayTime < delay)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    delayTime++;
                }
            }
            //Ham truy van
            void NonQuery(String sql, MySqlConnection con)
            {
                //MySqlConnection con = DBUtils.GetDBConnection();
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                con.Close();
            }

            //hàm nhập PIN
            void inputPin() 
            {
                var hd = IntPtr.Zero;
                Delay(5);
                hd = AutoControl.FindWindowHandle;
                MessageBox.Show(hd.ToString());
                var btn = IntPtr.Zero;
                btn = AutoControl.FindHandle(hd, "Button", "Hủy");
                AutoControl.SendClickOnControlByHandle(btn);
            }
           
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //r window = Application.Current.Windows.OfType<MyCustomWindowClass>().OrderByDescending(w => w.ActivationTime).FirstOrDefault();
        }
    }
}
