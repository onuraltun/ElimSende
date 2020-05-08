using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ElimSende
{
    public partial class Form1 : Form
    {
        bool trace = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBasla_Click(object sender, EventArgs e)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\Users\\onur.altun\\AppData\\Local\\Google\\Chrome\\User Data");
            options.AddArgument("--profile-directory=Default");
            options.AddArgument("--start-maximized");

            ChromeDriver driver = new ChromeDriver(options);

            driver.Url = txtUrl.Text;

            Thread.Sleep(10000);

            var allCells = driver.FindElements(By.XPath("//react-feed-listener-cell"));

            var bidCells = allCells.Where(c => c.GetAttribute("field-name") == "bidAmount1").ToList();

            int.TryParse(txtLimit.Text, out var limit);

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            var result = wait.Until((webDriver) =>
            {
                do
                {
                    foreach (var bidCell in bidCells)
                    {
                        long.TryParse(bidCell.Text.Replace(".", ""), out long currentBid);

                        if (limit > currentBid && currentBid > 0)
                        {
                            return bidCells[0].GetAttribute("security-name");
                        }
                    }

                    Thread.Sleep(10);
                } while (trace);

                return null;
            });

            if (trace && !string.IsNullOrWhiteSpace(result))
            {
                var sellButtons = driver.FindElements(By.Id("quick_order_sell"));

                foreach (var button in sellButtons)
                {
                    button.Click();
                }

                var orderEditButton = driver.FindElement(By.ClassName("cancelBtn"));

                orderEditButton.Click();
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            trace = false;
        }
    }
}
