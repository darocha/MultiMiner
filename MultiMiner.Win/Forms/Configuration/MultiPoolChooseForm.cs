﻿using MultiMiner.Engine;
using MultiMiner.Engine.Data;
using MultiMiner.Engine.Extensions;
using MultiMiner.Utility.Forms;
using MultiMiner.Xgminer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiMiner.Win.Forms.Configuration
{
    public partial class MultipoolChooseForm : MessageBoxFontForm
    {
        public PoolGroup SelectedMultipool { get; set; }
        public PoolGroup CurrentMultipool { get; set; }

        public MultipoolChooseForm() : this(null) { }

        public MultipoolChooseForm(PoolGroup currentMultipool)
        {
            InitializeComponent();
            CurrentMultipool = currentMultipool;
        }

        private void MultipoolChooseForm_Load(object sender, EventArgs e)
        {
            PopulateAlgorithmCombo();
            if (algoCombo.Items.Count > 0)
                algoCombo.SelectedIndex = 0;

            PopulateMultipoolCombo();
            multipoolCombo.Text = "Other";

            if (CurrentMultipool != null)
            {
                string multipoolAlgorithm = CurrentMultipool.Algorithm;
                algoCombo.Text = multipoolAlgorithm.ToSpaceDelimitedWords();
                string multipoolApi = CurrentMultipool.Id.Replace(":" + multipoolAlgorithm, String.Empty);
                multipoolCombo.Text = multipoolApi;
            }
        }

        private void PopulateMultipoolCombo()
        {
            multipoolCombo.Items.Clear();

            //

            multipoolCombo.Items.Add("Other");
        }

        private void PopulateAlgorithmCombo()
        {
            algoCombo.Items.Clear();
            List<CoinAlgorithm> algorithms = MinerFactory.Instance.Algorithms;
            foreach (CoinAlgorithm algorithm in algorithms)
                algoCombo.Items.Add(algorithm.Name.ToSpaceDelimitedWords());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/nwoolls/MultiMiner/wiki/Multipools");
        }

        private CoinAlgorithm GetSelectedAlgorithm()
        {
            string algorithmName = algoCombo.Text.Replace(" ", String.Empty);
            CoinAlgorithm algorithm = MinerFactory.Instance.GetAlgorithm(algorithmName);
            return algorithm;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            PoolGroup result = new PoolGroup();
            string multipoolAlgorithm = GetSelectedAlgorithm().Name;
            string multipoolApi = multipoolCombo.Text;
            result.Algorithm = multipoolAlgorithm;
            result.Name = multipoolApi + " - " + multipoolAlgorithm.ToSpaceDelimitedWords();
            result.Id = multipoolApi + ":" + multipoolAlgorithm;
            result.Kind = PoolGroup.PoolGroupKind.MultiCoin;
            SelectedMultipool = result;
        }
    }
}
