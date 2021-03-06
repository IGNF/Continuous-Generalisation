﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS;
//using ESRI.ArcGIS.ADF;
//using ESRI.ArcGIS.ADF.BaseClasses;
//using ESRI.ArcGIS.ADF.CATIDs;
//using ESRI.ArcGIS.ADF.COMSupport;
//using ESRI.ArcGIS.ADF.Resources;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;


using ContinuousGeneralizer;
using MorphingClass.CUtility;
using MorphingClass.CMorphingMethods;
using MorphingClass.CGeometry;
using MorphingClass.CAid;


namespace ContinuousGeneralizer.FrmAid
{
    public partial class FrmToIpe : Form
    {
        

        CDataRecords _DataRecords;


        public FrmToIpe()
        {
            InitializeComponent();
        }

        public FrmToIpe(CDataRecords pDataRecords)
        {
            InitializeComponent();
            _DataRecords = pDataRecords;
            //m_mapControl = m_MapControl;
            //_tsslTime = tsslTime;
        }

        private void FrmToIpe_Load(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            ParameterInitialize.cboLayerLt = new List<ComboBox>();
            ParameterInitialize.cboLayerLt.Add(this.cboLayer);
            this.cboSize.SelectedIndex = 0;

            tltCboSize.SetToolTip(this.cboSize, 
                "0.05 Thin; 0.5 Normal; 0.8 Heavier; 1.2 Fat; 2 Ultrafat. 0.05 is too small for points!");


            //Read all the layers
            CHelpFunc.FrmOperation(ref ParameterInitialize);
        }

        public void btnRun_Click(object sender, EventArgs e)
        {
            //get parameters
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            IEnvelope pFLayerEnv = ParameterInitialize.m_mapFeature
                .get_Layer(ParameterInitialize.cboLayerLt[0].SelectedIndex).AreaOfInterest;
            CEnvelope pIpeEnv = new CEnvelope(
                Convert.ToDouble(this.txtIpeMinX.Text), Convert.ToDouble(this.txtIpeMinY.Text),
                Convert.ToDouble(this.txtIpeMaxX.Text), Convert.ToDouble(this.txtIpeMaxY.Text));

            string strBoundWidth = this.cboSize.SelectedItem.ToString();
            if (chkOverrideWidth.Checked == false)
            {
                strBoundWidth = "";
            }

            //save path
            CHelpFunc.SetSavePath(ParameterInitialize);

            var pFLayerLt = CHelpFunc.GetVisibleLayers(ParameterInitialize);

            CToIpe.SaveToIpe(pFLayerLt, pFLayerEnv, pIpeEnv,
                this.chkGroup.Checked, strBoundWidth, ParameterInitialize);            
        }

    }
}