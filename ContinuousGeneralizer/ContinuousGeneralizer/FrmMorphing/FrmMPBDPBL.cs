using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;

using ContinuousGeneralizer;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CMorphingMethods;
using MorphingClass.CGeometry;

namespace ContinuousGeneralizer.FrmMorphing
{
    public partial class FrmMPBDPBL : Form
    {
        private CDataRecords _DataRecords;                    //records of data
        
        
        

        private CPolyline _RelativeInterpolationCpl;
        private double _dblProp;

        /// <summary>���ԣ����ݼ�¼</summary>
        public CDataRecords DataRecords
        {
            get { return _DataRecords; }
            set { _DataRecords = value; }
        }

        /// <summary>���캯��</summary>
        public FrmMPBDPBL()
        {
            InitializeComponent();
        }

        /// <summary>���캯��</summary>
        public FrmMPBDPBL(CDataRecords pDataRecords)
        {
            InitializeComponent();
            _DataRecords = pDataRecords;
        }

        private void FrmMPBDPBL_Load(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            
            
            
            ParameterInitialize.cboLargerScaleLayer = this.cboLargerScaleLayer;
            ParameterInitialize.cboSmallerScaleLayer = this.cboSmallerScaleLayer;
            ParameterInitialize.txtAngleBound = this.txtAngleBound;
            CConstants.strMethod = "MPBDPBL";
            //Read all the layers
            CHelpFunc.FrmOperation(ref ParameterInitialize);
            throw new ArgumentException("improve loading layesr!");
        }

        public void btnRun_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.ShowDialog();
            ParameterInitialize.strSavePath = SFD.FileName;
            ParameterInitialize.pWorkspace = CHelpFunc.OpenWorkspace(ParameterInitialize.strSavePath);

            //CMPBDPBL pMPBDPBL = new CMPBDPBL(ParameterInitialize);
            //pMPBDPBL.MPBDPBLMorphing();
            
            //_DataRecords.ParameterResult = pMPBDPBL.ParameterResult;

        }


        private void btn010_Click(object sender, EventArgs e)
        {
            _dblProp = 0.1;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn020_Click(object sender, EventArgs e)
        {
            _dblProp = 0.2;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn030_Click(object sender, EventArgs e)
        {
            _dblProp = 0.3;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn040_Click(object sender, EventArgs e)
        {
            _dblProp = 0.4;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn050_Click(object sender, EventArgs e)
        {
            _dblProp = 0.5;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn060_Click(object sender, EventArgs e)
        {
            _dblProp = 0.6;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn070_Click(object sender, EventArgs e)
        {
            _dblProp = 0.7;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn080_Click(object sender, EventArgs e)
        {
            _dblProp = 0.8;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn090_Click(object sender, EventArgs e)
        {
            _dblProp = 0.9;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn000_Click(object sender, EventArgs e)
        {
            _dblProp = 0;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn025_Click(object sender, EventArgs e)
        {
            _dblProp = 0.25;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn075_Click(object sender, EventArgs e)
        {
            _dblProp = 0.75;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }
        private void btn100_Click(object sender, EventArgs e)
        {
            _dblProp = 1;
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);
        }

        private void btnInputedScale_Click(object sender, EventArgs e)
        {
            _dblProp = Convert.ToDouble(this.txtProportion.Text);
            _RelativeInterpolationCpl = CDrawInActiveView.DisplayInterpolation(_DataRecords, _dblProp);

        }

        private void btnSaveInterpolation_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            List<CPolyline> cpllt = new List<CPolyline>();
            cpllt.Add(_RelativeInterpolationCpl);
            string strFileName = _dblProp.ToString();
            CHelpFunc.SaveCPlLt(cpllt, strFileName, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
        }


        private void btnIntegral_Click(object sender, EventArgs e)
        {
            CDeflection pDeflection = new CDeflection(_DataRecords);
            double dblDeflection = pDeflection.CalDeflection();
            this.txtEvaluation.Text = dblDeflection.ToString();
        }


        private void btnTranslation_Click(object sender, EventArgs e)
        {
            CTranslation pTranslation = new CTranslation(_DataRecords);
            double dblTranslation = pTranslation.CalTranslation();
            //double dblTranslation = pTranslation.CalDTranslation();
            this.txtEvaluation.Text = dblTranslation.ToString();
        }


        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            CHelpFuncExcel.ExportEvaluationToExcel(_DataRecords.ParameterResultToExcel, _DataRecords.ParameterInitialize, "0");
            //CHelpFuncExcel.KillExcel();
        }




    }
}