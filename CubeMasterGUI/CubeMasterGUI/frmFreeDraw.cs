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
    public partial class frmFreeDraw : Form
    {
        private FreeDraw _freeDrawController;

        private List<DrawingFunction> _functions;
        
        private int _voxelGrid_startX = 20;
        private int _voxelGrid_startY = 75;
        private int _voxelSpacing = 9;
        private int _voxelHeight = 80;
        private int _voxelWidth = 80;
        
        private Voxel[,] _voxels;

        private Color _clrVoxelClicked = Color.LightBlue;
        private Color _clrVoxelUnclicked = Color.GhostWhite;

        public frmFreeDraw(ref CubeController.Cube cube, int parentWidth, int parentHeight)
        {
            InitializeComponent();
            _freeDrawController = new FreeDraw(ref cube);

            this.Width = parentWidth;
            this.Height = parentHeight;

            GenerateVoxelGrid();
            AssignDrawingFunctions();
            InvokeTimerProtocol();
            InitializeRadioButtons();
        }

        private void AssignDrawingFunctions()
        {
            // Replace button text with actual shapes. 'Single' would just be a point, 
            // "Line" would be a line, etc. Would be much better looking UI. 
            this.drwSingle.SetText("Single");
            this.drwLine.SetText("Line");
            this.drwRectangle.SetText("Rectangle");
            this.drwCircle.SetText("Circle");

            this.drwSingle.SetController(ref _freeDrawController);
            this.drwLine.SetController(ref _freeDrawController);
            this.drwRectangle.SetController(ref _freeDrawController);
            this.drwCircle.SetController(ref _freeDrawController);

            this.drwSingle.SetDrawingMode(FreeDraw.DRAWING_MODE.SINGLE);
            this.drwLine.SetDrawingMode(FreeDraw.DRAWING_MODE.LINE);
            this.drwRectangle.SetDrawingMode(FreeDraw.DRAWING_MODE.RECTANGLE);
            this.drwCircle.SetDrawingMode(FreeDraw.DRAWING_MODE.CIRCLE);

            _functions = new List<DrawingFunction>();
            _functions.Add(this.drwSingle);
            _functions.Add(this.drwLine);
            _functions.Add(this.drwRectangle);
            _functions.Add(this.drwCircle);
        }

        private void InvokeTimerProtocol()
        {
            if (!this.DesignMode)
            {
                this.tmrFreeDraw.InitializeTimers();
            }
        }

        private void InitializeRadioButtons()
        {
            this.btnAXIS_X.Checked = true;
        }

        private void GenerateVoxelGrid()
        {
            _voxels = new Voxel[8, 8];

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    Voxel tmpVoxel = new Voxel();
                    tmpVoxel.Height = _voxelHeight;
                    tmpVoxel.Width = _voxelWidth;
                    tmpVoxel.Left = _voxelGrid_startX + (i*_voxelWidth + _voxelSpacing);
                    tmpVoxel.Top = _voxelGrid_startY + (j*_voxelHeight + _voxelSpacing);
                    
                    tmpVoxel.X = i;
                    tmpVoxel.Y = j;

                    tmpVoxel.BringToFront();

                    tmpVoxel.MouseClick += frmFreeDraw_VoxelGridClick;

                    _voxels[i, j] = tmpVoxel;
                    this.Controls.Add(tmpVoxel);
                }
            }
        }

        private void frmFreeDraw_MouseMove(object sender, MouseEventArgs e)
        {
            this.tmrFreeDraw.ResetTimers();
        }

        private void frmFreeDraw_MouseClick(object sender, MouseEventArgs e)
        {
            this.tmrFreeDraw.ResetTimers();
        }

        private void frmFreeDraw_VoxelGridClick(object sender, MouseEventArgs e)
        {
            this.tmrFreeDraw.ResetTimers();
            Voxel vox = sender as Voxel;
            vox.VoxelSet = !vox.VoxelSet;
            _voxels[vox.X, vox.Y].BackColor = vox.VoxelSet ? _clrVoxelClicked : _clrVoxelUnclicked;
        }

        private void btnAXIS_X_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btnx = sender as RadioButton;
            if (btnx.Checked)
            {
                _freeDrawController.SelectedAxis = CubeController.Cube.AXIS.AXIS_X;
            }
        }

        private void btnAXIS_Y_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btny = sender as RadioButton;
            if (btny.Checked)
            {
                _freeDrawController.SelectedAxis = CubeController.Cube.AXIS.AXIS_Y;
            }
        }

        private void btnAXIS_Z_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btnz = sender as RadioButton;
            if (btnz.Checked)
            {
                _freeDrawController.SelectedAxis = CubeController.Cube.AXIS.AXIS_Z;
            }
        }

        private void drwSingle_Load(object sender, EventArgs e)
        {

        }
    }
}
