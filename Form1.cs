using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
//using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Compilers0
{

    
    public partial class Form1 : Form
    {
        //global variables
        string path1;
        string path2;
        //global functions
        //table draw

        void tableDraw()
        {
            //clear table
            dataGridView1.Rows.Clear();
            //draw 3 columns
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "";
            dataGridView1.Columns[1].Name = "Token Value";
            dataGridView1.Columns[2].Name = "Token Type";
            //set the colmns size
            dataGridView1.Columns[0].Width = 42;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 150;
            //change font size
            dataGridView1.DefaultCellStyle.Font = new Font("new times roman",10);
            // set a custom color
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(36, 49, 60);


            //draw table
            string[] outCode = File.ReadAllLines("OUT.txt");
            
            int j = 0;
            //select only odd lines
            for (int i = 3; i < outCode.Length; i += 2)
            {
                outCode[j] = outCode[i];
                j++;
            }
            string[] tokenValue = new string[j];
            string[] tokenType = new string[j];
            //leave | sign
            for (int i = 0; i < j; i++)
            {
                tokenValue[i] = outCode[i].Substring(1, 25);
                tokenType[i]  = outCode[i].Substring(27, 25);

            }
            for (int i = 0; i < j; i++)
             {
                
                string[] row = { (i+1).ToString(), tokenValue[i] , tokenType[i] };
                dataGridView1.Rows.Add(row);
                //set row colors
                
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = myRgbColor;
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                
            }

            dataGridView1.BackgroundColor = myRgbColor;


        }

        //Dynamic Linked Library (DLL)

        [DllImport("aaaaa.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern  void test(string take);

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialize1(string line);

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialize2(string Path1);

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tokenByToken();

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void printAll();

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initializeAndPrintAll(string path1);

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialize3(string value ,string type);

        [DllImport("compilercpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int parseAll();

        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.BackColor = Color.Transparent;
            // set a custom color
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(36, 49, 60);
            dataGridView1.BackgroundColor = myRgbColor;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            // call C++ scanner

            //call initialize 1

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                if (richTextBox1.Lines[i] != ""&& richTextBox1.Lines[i] != " " && richTextBox1.Lines[i] != "  " && richTextBox1.Lines[i] != "   " && richTextBox1.Lines[i] != "    " && richTextBox1.Lines[i] != "      ")
                {
                    initialize1(richTextBox1.Lines[i]);
                }
                

            }

            //call initialize 2

            //initialize2(path1);                      

            while (true)
            {
                if (tokenByToken() == 0) { break; }
               
            }

            printAll();

            //show output on rich text box 3

            

            //show table
            if (viewTokensTableToolStripMenuItem.Checked == true)
            {
                label2.Visible = true;
                dataGridView1.Visible = true;
                button2.Visible = true;
                

            }
            //fill the table
            tableDraw();
            button1.Enabled = true;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {          
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Path.GetExtension(file.FileName).Equals(".txt", StringComparison.InvariantCultureIgnoreCase) || Path.GetExtension(file.FileName).Equals(".cpp", StringComparison.InvariantCultureIgnoreCase))
                {

                    path1 = file.FileName;

                    //clear rich text box 2 before loading
                    richTextBox1.Text = "";


                    string[] sourceCode = File.ReadAllLines(path1);
                    for (int i = 0; i < sourceCode.Length - 1; i++)
                    {
                        //add all lines to rich text box 1                   
                        richTextBox1.Text += sourceCode[i] + "\n";
                    }

                    //for the last line
                    richTextBox1.Text += sourceCode[sourceCode.Length - 1];
                }
                else { MessageBox.Show("please select a valid text file"); }
               
            }
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //view line number
            if (viewToolStripMenuItem.Checked == true)
            {
                richTextBox2.Text = "";

                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    richTextBox2.Text +=  (i + 1).ToString() + "\n" ;
                }
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewToolStripMenuItem.Checked == true)

            {
                richTextBox2.Visible = true;
            }

            if (viewToolStripMenuItem.Checked == false)

            {
                richTextBox2.Visible = false;
            }
        }

        private void viewTokensTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewTokensTableToolStripMenuItem.Checked == true)
            {
                label2.Visible = true;
                dataGridView1.Visible = true;
                button2.Visible = true;
                
            }
            if (viewTokensTableToolStripMenuItem.Checked == false)
            {
                label2.Visible = false;
                dataGridView1.Visible = false;
                button2.Visible = false;
                
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //richTextBox1.MouseWheel += 1;
        }

        private void MyMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            int noError=1;
            //scan then parse mode
            if (scanThenParseToolStripMenuItem.Checked == true)
            {
                noError = parseAll();
            }
            //parse only mode
            if (parserOnlyToolStripMenuItem.Checked == true)
            {

                //call initialize3
               
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    if (richTextBox1.Lines[i] != ""&& richTextBox1.Lines[i].Length>0)
                    {
                       string [] token =richTextBox1.Lines[i].Split(',');
                        initialize3(token[0].Trim(),token[1].Trim());
                    }
                }
                
                noError = parseAll();  
            }
            //if no error show the tree
            if (noError == 1)
            { //show form2
                var form2 = new Form2();
                form2.Show();
                
            }
            //if there is an error
            if (noError == 0)
            {
                //show error message from errors.txt
                string[] error = File.ReadAllLines("errors.txt");
                MessageBox.Show(error[0]);
            }


            //delete error.txt 

            if (File.Exists("errors.txt"))
            {
                File.Delete("errors.txt");
            }
            button2.Enabled = true;
        }

        private void loadListOfTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               
               path2 = file.FileName;

            }
        }

        private void scanThenParseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //select one mode
            scanThenParseToolStripMenuItem.Checked = true;
            parserOnlyToolStripMenuItem.Checked = false;
            //show this mode objects
            button1.Visible = true;
            richTextBox1.Visible = true;
            richTextBox2.Visible = true;
            label1.Visible = true;
            button2.Visible = false;
            dataGridView1.Visible = false;
            label2.Visible = false;
            button2.Location=new Point(627, 876);
        }

        private void parserOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //select one mode
            scanThenParseToolStripMenuItem.Checked = false;
            parserOnlyToolStripMenuItem.Checked = true;
            //show this mode objects
            button1.Visible = false;
            richTextBox2.Visible = false;
            label2.Visible = false;
            dataGridView1.Visible = false;
            label2.Visible = false;
            button2.Visible = true;
            button2.Location = button1.Location;
            richTextBox1.Text = "";
        }

        private void richTextBox2_MouseWheel(object sender, MouseEventArgs e)
        {
          
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            richTextBox1.Text += Clipboard.GetText();
        }
    }



  
}
        