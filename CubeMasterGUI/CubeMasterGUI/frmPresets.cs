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
    public partial class frmPresets : Form
    {
        /// <summary>
        /// Controller for the presets form
        /// </summary>
        private Presets _presetsController;

        /// <summary>
        /// Spacing for the buttons on the form
        /// </summary>
        private int _spacing = 9;

        /// <summary>
        /// Constructor for Presets form
        /// </summary>
        /// <param name="cube"></param>
        /// <param name="parentWidth">Parent form width.</param>
        /// <param name="parentHeight">Parent form height</param>
        public frmPresets(ref CubeController.Cube cube, int parentWidth, int parentHeight)
        {
            InitializeComponent();
            _presetsController = new Presets(ref cube);

            this.ShowInTaskbar = false;
            pnlPresetLauncher.HorizontalScroll.Enabled = false;
            pnlPresetLauncher.HorizontalScroll.Visible = false;
        }

        /// <summary>
        /// Update the track bar speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            _presetsController.UpdateDelay(trackBarSpeed.Value);
        }

        /// <summary>
        /// Load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPresets_Load(object sender, EventArgs e)
        {
            var presets = _presetsController.GetButtonList();
            PopulatePanel(presets);
        }

        /// <summary>
        /// Populate the panel with buttons
        /// </summary>
        /// <param name="presets"></param>
        private void PopulatePanel(List<Button> presets)
        {
            int btnWidth = presets[0].Width;
            int btnHeight = presets[0].Height;
            int xCount = pnlPresetLauncher.Size.Width / (btnWidth + _spacing);

            for (int i = 0; i < presets.Count; i++)
            {
                var location = new Point
                {
                    X = (i%xCount)*btnWidth + _spacing,
                    Y = (i/xCount)*btnWidth + _spacing
                };
                presets[i].Location = location;
                pnlPresetLauncher.Controls.Add(presets[i]);       
            }
        }
    }
}
