using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WindowsFormsApplication9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string printBuild;
        IEnumerable<jobs> baseQuery, currentQuery;

        private void Form1_Load(object sender, EventArgs e)
        {
            Splash splash = new Splash();
            splash.Show();
            string json = new WebClient().DownloadString(@"http://data.cityofnewyork.us/resource/kpav-sd4t.json");           
            var jobs = JsonConvert.DeserializeObject<List<jobs>>(json);            
            baseQuery = from j in jobs orderby j.agency,j.job_id select j;
            currentQuery = baseQuery;
            splash.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentQuery = baseQuery;
            List<string> addedIDS = new List<string>();
            if (txtlocation.Text != "" || textstart.Text != "")
                listBox1.Items.Clear();
            if (textstart.Text != "Enter Starting Salary")
            {
                currentQuery = from j in currentQuery where double.Parse(j.salary_range_from) >= double.Parse(textstart.Text) select j;
            }
            if (txtlocation.Text != "Enter Location")
            {
                currentQuery = from j in currentQuery where j.work_location.Contains(txtlocation.Text) select j;
            }
            if (textBox2.Text != "Enter Skill")
            {
                currentQuery = from j in currentQuery where j.preferred_skills.Contains(textBox2.Text) select j;
            }
            if (lstLevel.Text != "..." && lstLevel.Text != ""){
                currentQuery = from j in currentQuery where j.level == lstLevel.Text select j;             
            }
            
            listBox1.Items.Clear();
            foreach (var j in currentQuery)
            {
                if (!addedIDS.Contains(j.job_id))
                {
                    listBox1.Items.Add(j.business_title + " ( " + j.agency + " ) ~" + j.job_id);
                    addedIDS.Add(j.job_id);
                }
            }
            MessageBox.Show(currentQuery.Count()/2 + " jobs found");
        }
         
        private void textstart_TextChanged(object sender, EventArgs e)
        {
            if(textstart.Text==("Enter Starting Salary")){
                textstart.Text = "";
                textstart.ForeColor=Color.Black;
            }
        }

        private void textlocation_TextChanged(object sender, EventArgs e)
        {
            if (txtlocation.Text == ("Enter Location"))
            {
                txtlocation.Text = "";
                txtlocation.ForeColor = Color.Black;
            }
        }

        private void textstart_Click(object sender, EventArgs e)
        {
            if (textstart.Text == ("Enter Starting Salary"))
            {
                textstart.Text = "";
                textstart.ForeColor = Color.Black;
            }
        }

        private void textlocation_Click(object sender, EventArgs e)
        {
            if (txtlocation.Text == ("Enter Location"))
            {
                txtlocation.Text = "";
                txtlocation.ForeColor = Color.Black;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string listid = listBox1.Text;
            int idstart = listid.IndexOf("~")+1;
            string jobid = listid.Substring(idstart);
            string jobdescribe = "", jobperskill = "";

            foreach(var j in currentQuery){
                if(j.job_id.Equals(jobid)){
                    jobperskill = j.preferred_skills;
                    jobdescribe = j.job_description;                
                }
            }
            textBox1.Text=jobdescribe;
            txtPerSkill.Text = jobperskill;         
        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            printBuild = "";
            int spot = 1;
            foreach (var j in currentQuery)
            {
                printBuild += spot + ".  " + j.agency + " ( " + j.business_title + " )\n_______________________________________________________________\n  " + j.work_location + "\n_______________________________________________________________  " + "$" + j.salary_range_from + " \n\n";
                spot++;           
            }
            printDialog1.ShowDialog();
            if (printDialog1.PrinterSettings.IsValid)
                printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(printBuild, new Font("Arial", 12), new SolidBrush(Color.Black), new PointF(0, 0));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == ("Enter Skill"))
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == ("Enter Skill"))
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

    }
}
