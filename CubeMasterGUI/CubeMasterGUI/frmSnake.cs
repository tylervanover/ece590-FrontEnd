﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CubeMasterGUI
{
    public partial class frmSnake : Form
    {
        private FreeDraw _freeDrawController;

        public frmSnake(ref CubeController.Cube cube, int parentWidth, int parentHeight)
        {
            InitializeComponent();

            this.Width = parentWidth;
            this.Height = parentHeight;

            InvokeTimerProtocol();
        }

        private void InvokeTimerProtocol()
        {
            if (!this.DesignMode)
            {
                this.tmrSnake.InitializeTimers();
            }
        }
                
        private void frmGames_MouseMove(object sender, MouseEventArgs e)
        {
            this.tmrSnake.ResetTimers();
        }

        private void frmGames_MouseClick(object sender, MouseEventArgs e)
        {
            this.tmrSnake.ResetTimers();
        }
    }
}
