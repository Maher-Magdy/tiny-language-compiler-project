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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;


namespace Compilers0
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        //get  number of nodes from kero (N)
        string[] keroNodes = File.ReadAllLines("parse_out.txt");
        
    //create an arry for levels , names ...etc
    int[] levels = new int[500];
        string[] nodeIndex = new string[500];
        string[] nodeName = new string[500];
        string[] nodeShape = new string[500];
        string[] nodeParentOrBrother = new string[500];
        string[] nodeParentIndex = new string[500];

        int levelsLength = 0;
        int nodesLength = 0;
        //create 2*N nodes (rectangle , round)
        Button[] rectangleNode = new Button[250];
        RoundButton[] roundNode = new RoundButton[250];
        
        // variables for moving buttons
        private bool enableMoving = false;
        int moveIndex;
        private Point initialClickedPoint;
        // 2 functions to move a button
        ////////////////////////////////////////////////////////////////
        private void btnButton_MouseDown(object sender, MouseEventArgs e)
        {                      
            initialClickedPoint = e.Location;
        }

        private void btnButton_MouseUp(object sender, MouseEventArgs e )
        {            
          // Location = new Point(e.X + roundNode[moveIndex].Left - initialClickedPoint.X,
            //            e.Y + roundNode[moveIndex].Top - initialClickedPoint.Y);
            
        }


        ///////////////////////////////////////////////////////////////////////

        // function to connect nodes
        private void drawLine(Button from , Button to , string type)
        {
            int x1 = 0; int x2 = 0; int y1 = 0; int y2=0;
            if (type == "P") //parent to child 
            { 
                x1 = from.Location.X + from.Width/2;
                y1 = from.Location.Y + from.Height;
                x2 = to.Location.X + to.Width / 2;
                y2 = to.Location.Y ;
            }
            if (type =="B") //brother to brother
            {
                x1 = from.Location.X + from.Width ;
                y1 = from.Location.Y + from.Height/2;
                x2 = to.Location.X ;
                y2 = to.Location.Y + to.Height/2;
            }
            Pen pen = new Pen(Color.White, 1);

            Graphics g = panel1.CreateGraphics();

            g.DrawLine(pen, x1, y1, x2, y2);
            
           
        }
        private void deleteLine(Button from, Button to, string type)
        {
            int x1 = 0; int x2 = 0; int y1 = 0; int y2 = 0;
            if (type == "P") //parent to child 
            {
                x1 = from.Location.X + from.Width / 2;
                y1 = from.Location.Y + from.Height;
                x2 = to.Location.X + to.Width / 2;
                y2 = to.Location.Y;
            }
            if (type == "B") //brother to brother
            {
                x1 = from.Location.X + from.Width;
                y1 = from.Location.Y + from.Height / 2;
                x2 = to.Location.X;
                y2 = to.Location.Y + to.Height / 2;
            }
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(36, 49, 60);
            
            Pen pen = new Pen(myRgbColor, 1);

            Graphics g = panel1.CreateGraphics();

            g.DrawLine(pen, x1, y1, x2, y2);

            
        }

        // a function to translate the file tree.txt
        private void translate()
        {
            // translate the lines of tree.txt

            for (int i = 0; i < keroNodes.Length; i++)
            {

                string[] split = new string[5];
                if (keroNodes[i].Substring(0, 1) == "$")
                {
                    levels[levelsLength] = Int32.Parse(keroNodes[i].Substring(1, keroNodes[i].Length-1));
                    levelsLength++;
                }
                else
                {
                    split = keroNodes[i].Split(',');

                    nodeIndex[nodesLength] = split[0];
                    nodeName[nodesLength] = split[1];
                    nodeShape[nodesLength] = split[4];
                    nodeParentOrBrother[nodesLength] = split[2];
                    nodeParentIndex[nodesLength] = split[3];
                    //remove null nodes
                    if (nodeName[nodesLength] =="NULL"|| nodeName[nodesLength] == "nullstring") {levels[levelsLength-1]--; continue; }
                    nodesLength++;

                }
            }
            //delete parse_out.txt
            if (File.Exists("parse_out.txt"))
            {
                File.Delete("parse_out.txt");
            }



        }
        private void Form2_Load(object sender, EventArgs e)
        {

            translate();

            //suspend layout then release when all is done
            panel1.SuspendLayout();

            for (int i = 0; i < nodesLength; i++)
            {

                //create buttons
                rectangleNode[i] = new Button();
                roundNode[i] = new RoundButton();
                //show buttons on the panel
                panel1.Controls.Add(rectangleNode[i]);
                panel1.Controls.Add(roundNode[i]);
                //set size and color
                rectangleNode[i].BackColor = Color.White;
                rectangleNode[i].ForeColor = Color.Black;

                roundNode[i].BackColor = Color.White;
                roundNode[i].ForeColor = Color.Black;

                rectangleNode[i].Enabled = false;
                roundNode[i].Enabled = false;

                rectangleNode[i].Visible = false;
                roundNode[i].Visible = false;

                //set node name
                rectangleNode[i].Text = nodeName[i];
                roundNode[i].Text = nodeName[i];

                //set size
                if (nodeName[i].Length < 15)
                {
                    rectangleNode[i].Size = new Size(80, 30);
                    roundNode[i].Size = new Size(75, 30);
                }

                else
                {
                    rectangleNode[i].Size = new Size(nodeName[i].Length*8, 30);
                    roundNode[i].Size = new Size(nodeName[i].Length*8, 30);

                }
                

                //determine if oval or rectangle
                if (nodeShape[i]=="D")
                { roundNode[i].Visible = true; }
                else if (nodeShape[i] == "N")
                { rectangleNode[i].Visible = true; }

                
                





                //allow for movements
                roundNode[i].MouseUp += new MouseEventHandler(btnButton_MouseUp);
                roundNode[i].MouseDown += new MouseEventHandler(btnButton_MouseDown);
            }
            //use levels[i] to determin spacing between nodes
            //find biggest level
            int maxLevel = 0;
            for (int i=0;i<levelsLength;i++) 
            { if (maxLevel<levels[i]) { maxLevel = levels[i]; } }
            int k = 0;            
            for (int i = 0; i < levels.Length; i++)
            {
                for (int j = 0; j < levels[i]; j++)                 
                {

                    //set location
                    //use max level to set location or use max level of two Consecutive levels
                    rectangleNode[k].Left = 150*maxLevel/levels[i]*j;
                    rectangleNode[k].Top = 100*i;
                    roundNode[k].Left = 150 * maxLevel / levels[i] * j;
                    roundNode[k].Top = 100 * i;
                    k++;
                }
                    
            }


            //release layout
            panel1.ResumeLayout(true);
            //set syntax tree text location
            label1.Left = panel1.Left + panel1.Width /2 -80;

        }
       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            // set a custom color
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(36, 49, 60);
            panel1.BackColor = myRgbColor;
            for (int i = 1; i < nodesLength; i++)
            {
                int j = 0;
                for ( j=0;j< nodesLength;j++) 
                {
                    if (nodeParentIndex[i]==nodeIndex[j]) { break; }
                }
                //draw lines connecting nodes
                drawLine(rectangleNode[j], rectangleNode[i], nodeParentOrBrother[i]);
            }   
            

        }
    }
}














//unused code
/*
 Bitmap bmp = new Bitmap(1000, 1000);
            Bitmap pp = new Bitmap(1000, 1000);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Green, 0, 0, 10, 10);
                g.DrawEllipse(Pens.Black, 10, 10, 900, 900);
            }

            //pictureBox1.Image = bmp;
            using System.Windows.Forms.DataVisualization.Charting;
// roundNode[i].MouseUp += delegate (object senderr, MouseEventArgs ee) { btnButton_MouseUp(sender, ee, button1); };
*/
