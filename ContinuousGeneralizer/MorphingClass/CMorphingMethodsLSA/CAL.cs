using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using MorphingClass.CCorrepondObjects;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;

using VBClass;
//using CPlusClass;


namespace MorphingClass.CMorphingMethodsLSA
{
    /// <summary>
    /// ������С����ԭ���Morphing�������ԽǶȺͱ߳�Ϊ����(Least Squares Alogrithm_Coordinate, Angle and Length)
    /// </summary>
    /// <remarks>�˼����ȺͽǶȣ�������Ϊƽ���������ƽ���׼���ƽ��</remarks>
    public class CAL
    {
        private CDataRecords _DataRecords;                    //���ݼ�¼
        private double _dblTX;
        
        
        private CPAL _pCAL = new CPAL();

        private double _dblMaxLengthV;
        private double _dblMinLengthV;
        private double _dblDiffLengthV;
        private double _dblMaxAngleV;
        private double _dblMinAngleV;
        private double _dblDiffAngleV;

        public int intIterationNum = 0;


        //private double _dblMaxELength;   //the max value of Edges' Lengths

        public CAL()
        {

        }

        public CAL(CDataRecords pDataRecords, double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
            //CalParameters();
            //_dblMaxELength = pDataRecords.ParameterResult.FromCpl.GetMaxELength() / 100; 
        }

        public CAL(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
            CPolyline FromCpl = pDataRecords.ParameterResult.FromCpl;
            _dblTX = FromCpl.pPolyline.Length / FromCpl.CptLt .Count  / 1000000;   //������ֵ����
            //CalParameters();
            //_dblMaxELength = pDataRecords.ParameterResult.FromCpl.GetMaxELength()/100;
        }

        private void CalParameters()
        {
            _DataRecords.ParameterInitialize.tsslMessage.Text = "Calculating Parameters......";
            _DataRecords.ParameterInitialize.ststMain.Refresh();

            CPolyline cpl = GetTargetcpl(0.005, _DataRecords.ParameterResult.FromCpl);

            double dblMaxLengthV = 0;
            double dblMinLengthV = cpl.pPolyline.Length;
            double dblMaxAngleV = 0;
            double dblMinAngleV = Math.PI;
            for (int j = 2; j < 200; j++)
            {
                cpl=GetTargetcpl(j*0.005,cpl);

                for (int i = 0; i < cpl.SubCPlLt.Count; i++)
                {
                    if (cpl.SubCPlLt[i].dblLengthV == 0)  //�����0���϶��ǹ̶�������ģ����������
                    {
                        continue;
                    }

                    if (Math.Abs(cpl.SubCPlLt[i].dblLengthV) > dblMaxLengthV)
                    {
                        dblMaxLengthV = Math.Abs(cpl.SubCPlLt[i].dblLengthV);
                    }
                    if (Math.Abs(cpl.SubCPlLt[i].dblLengthV) < dblMinLengthV)
                    {
                        dblMinLengthV = Math.Abs(cpl.SubCPlLt[i].dblLengthV);
                    }
                }

                for (int i = 1; i < cpl.CptLt.Count - 1; i++)
                {
                    if (cpl.CptLt[i].dblAngleV == 0)  //�����0���϶��ǹ̶�������ģ����������
                    {
                        continue;
                    }

                    if (Math.Abs(cpl.CptLt[i].dblAngleV) > dblMaxAngleV)
                    {
                        dblMaxAngleV = Math.Abs(cpl.CptLt[i].dblAngleV);
                    }
                    if (Math.Abs(cpl.CptLt[i].dblAngleV) < dblMinAngleV)
                    {
                        dblMinAngleV = Math.Abs(cpl.CptLt[i].dblAngleV);
                    }
                }
            }

            //double dblLengthInterval=(dblMaxLengthV-dblMinLengthV)/
            _dblMaxLengthV = dblMaxLengthV;
            _dblMinLengthV = dblMinLengthV;
            _dblDiffLengthV = dblMaxLengthV - dblMinLengthV;

            _dblMaxAngleV = dblMaxAngleV;
            _dblMinAngleV = dblMinAngleV;
            _dblDiffAngleV = dblMaxAngleV - dblMinAngleV;

            _DataRecords.ParameterInitialize.tsslMessage.Text = "Ready!";
            _DataRecords.ParameterInitialize.ststMain.Refresh();            
        }

        /// <summary>
        /// ��ʾ�����ص�����ֵ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��״Ҫ��</returns>
        public CPolyline DisplayInterpolation(double dblProportion, CPolyline lastcpl)
        {

            
            CPolyline cpl = GetTargetcpl(dblProportion, lastcpl);
            _DataRecords.ParameterInitialize.txtT.Text = "   t = " + dblProportion.ToString();
            
            //_DataRecords.ParameterInitialize.txtVtPV.Text ="   VtPV = " + cpl.dblVtPV.ToString();

            //// ����滭�ۼ�
            //IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            //IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            //pGra.DeleteAllElements();

            //if (dblProportion == 0)
            //{
            //    int tt = 5;
            //}
            //else if (dblProportion == 1)
            //{
            //    int ss = 5;
            //}


            ////���߶ν���Ⱦɫ
            //for (int i = 0; i < cpl.SubCPlLt.Count; i++)
            //{
            //    double dblColor = 255;
            //    int intColor = Convert.ToInt32((Math.Abs(cpl.SubCPlLt[i].dblLengthV) - _dblMinLengthV) / _dblDiffLengthV * dblColor);
            //    intColor = Math.Abs(intColor);
            //    if (Math.Abs(cpl.SubCPlLt[i].dblLengthV) < _dblTX)
            //    {
            //        cpl.SubCPlLt[i].intGreen = 255;
            //    }

            //    else if (cpl.SubCPlLt[i].dblLengthV > 0)
            //    {
            //        if (intColor <= 255)
            //        {
            //            cpl.SubCPlLt[i].intRed = 255;
            //            cpl.SubCPlLt[i].intGreen = 255 - intColor;
            //            cpl.SubCPlLt[i].intBlue = 255 - intColor;
            //        }
            //        else
            //        {
            //            cpl.SubCPlLt[i].intRed = 255;
            //        }
            //    }
            //    else if (cpl.SubCPlLt[i].dblLengthV < 0)
            //    {
            //        if (intColor <= 255)
            //        {
            //            cpl.SubCPlLt[i].intRed = 255 - intColor;
            //            cpl.SubCPlLt[i].intGreen = 255 - intColor;
            //            cpl.SubCPlLt[i].intBlue = 255;
            //        }
            //        else
            //        {
            //            cpl.SubCPlLt[i].intBlue = 255;
            //        }
            //    }
            //}  

            //for (int i = 0; i < cpl.SubCPlLt.Count; i++)
            //{
            //    CPolyline subcpl = cpl.SubCPlLt[i];
            //    CHelperFunction.ViewPolylineByRGB(m_mapControl, subcpl, subcpl.intRed, subcpl.intGreen, subcpl.intBlue);  //��ʾ���ɵ��߶�
            //}

            ////�Ի���������Ⱦɫ
            //for (int i = 1; i < cpl.CptLt.Count -1; i++)
            //{
            //    double dblColor = 255;
            //    int intColor = Convert.ToInt32((Math.Abs(cpl.CptLt [i].dblAngleV ) - _dblMinAngleV) / _dblDiffAngleV * dblColor);
            //    intColor = Math.Abs(intColor);


            //    if (Math .Abs (cpl.CptLt[i].dblAngleV)<_dblTX)
            //    {
            //        cpl.CptLt[i].intGreen = 255;
            //    }
            //    else if (cpl.CptLt[i].dblAngleV > 0)
            //    {
            //        if (intColor <= 255)
            //        {
            //            cpl.CptLt[i].intRed = 255;
            //            cpl.CptLt[i].intGreen = 255 - intColor;
            //            cpl.CptLt[i].intBlue = 255 - intColor;
            //        }
            //        else
            //        {
            //            cpl.CptLt[i].intRed = 255;
            //        }
            //    }
            //    else if (cpl.CptLt[i].dblAngleV < 0)
            //    {
            //        if (intColor <= 255)
            //        {
            //            cpl.CptLt[i].intRed = 255 - intColor;
            //            cpl.CptLt[i].intGreen = 255 - intColor;
            //            cpl.CptLt[i].intBlue = 255;
            //        }
            //        else
            //        {
            //            cpl.CptLt[i].intBlue = 255;
            //        }
            //    }
            //}

            //for (int i = 1; i < cpl.CptLt.Count-1; i++)
            //{
            //    CPoint cpt=cpl.CptLt[i];
            //    CHelperFunction.ViewPolygonGeometryByRGB(m_mapControl, cpt.Buffer(5), cpt.intRed, cpt.intGreen, cpt.intBlue);  //��ʾ�����������
            //}

            ////��ʾ�߶�
            //IActiveView pAv = pGra as IActiveView;
            //pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);



            // ����滭�ۼ�
            IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            CHelperFunction.ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�


            return cpl;
        }

        /// <summary>
        /// ��ȡ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>�ڴ�����״Ҫ��ʱ��������ԭ��״Ҫ�صı߽��п�������״Ҫ�ش����������������������״Ҫ��</returns>
        public CPolyline GetTargetcpl(double dblProportion, CPolyline lastcpl)
        {
            this.intIterationNum = Convert.ToInt32(_DataRecords.ParameterInitialize .txtIterationNum  .Text ); 

            if (dblProportion == 0)
            {
                int aa = 5;
            }
            //intIterationNum = Convert.ToInt32(_DataRecords.ParameterInitialize.txtIterationNum.Text);     //the maximum itrative times
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //��ȡ���ݺ󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            int intPtNum = pCorrCptsLt.Count;
            int intXYNum = 2 * intPtNum;

            ////generate the list of vertices based on linear interpolation
            //List<CPoint> newcptlt = new List<CPoint>();
            //for (int i = 0; i < pCorrCptsLt.Count ; i++)
            //{
            //    double dblnewx = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.X + dblProportion * pCorrCptsLt[i].ToCpt.X;
            //    double dblnewy = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.Y + dblProportion * pCorrCptsLt[i].ToCpt.Y;
            //    CPoint newcpt = new CPoint(i, dblnewx, dblnewy);
            //    newcptlt.Add(newcpt);
            //}


            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double dblTune = 3;
            double[] adblLength0 = new double[intPtNum - 1];
            double dblSumFrLength0 = 0;
            double dblSumToLength0 = 0;
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeometricMethods.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                double dbltosublength = CGeometricMethods.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);

                //double dble=Math .E;
                //double dbla = (dblfrsublength - dbltosublength) * dble / (dble - 1);
                //double dblb = (dble * dbltosublength - dblfrsublength) / (dble - 1);

                //adblLength0[i] = dbla * Math.Pow(dble, -dblProportion) + dblb;



                adblLength0[i] = (1 - dblProportion) * dblfrsublength + dblProportion * dbltosublength;

                //adblLength0[i] = (1 - Math.Pow(dblProportion, 1 / dblTune)) * dblfrsublength + Math.Pow(dblProportion, 1 / dblTune) * dbltosublength;
                //dblSumFrLength0 = dblSumFrLength0 + dblfrsublength;
                //dblSumToLength0 = dblSumToLength0 + dbltosublength;

                //pCorrCptsLt[i].FrCpt.isCtrl = false;  //�����ʼ���Ժ��������Ӷ�Ӧ��Ϊ�̶��㣬�ʴ�������Ϊfalse
            }

            //double dblFrRealSumLength = dblSumFrLength0 - CGeometricMethods.CalDis(pCorrCptsLt[0].FrCpt, pCorrCptsLt[1].FrCpt) - CGeometricMethods.CalDis(pCorrCptsLt[intPtNum - 2].FrCpt, pCorrCptsLt[intPtNum - 1].FrCpt);
            //double dblToRealSumLength = dblSumToLength0 - CGeometricMethods.CalDis(pCorrCptsLt[0].ToCpt, pCorrCptsLt[1].ToCpt) - CGeometricMethods.CalDis(pCorrCptsLt[intPtNum - 2].ToCpt, pCorrCptsLt[intPtNum - 1].ToCpt);

            //����Ƕȳ�ʼֵ��ȫ�����㣩
            double[] adblAngle0 = new double[intPtNum - 2];
            double dblSumFrAngle0 = 0;
            double dblSumToAngle0 = 0;
            for (int i = 0; i < pCorrCptsLt.Count - 2; i++)
            {
                //�ϴ��������״Ҫ���ϵļн�
                double dblfrAngle = CGeometricMethods.CalAngle2(pCorrCptsLt[i].FrCpt, pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i + 2].FrCpt);
                //��С��������״Ҫ���ϵļн�
                double dbltoAngle = CGeometricMethods.CalAngle2(pCorrCptsLt[i].ToCpt, pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i + 2].ToCpt);

                //double dble = Math.E;
                //double dbla = (dbltoAngle - dblfrAngle) * dble / (dble - 1);
                //double dblb = (dble * dblfrAngle - dbltoAngle) / (dble - 1);

                ////�Ƕȳ�ʼֵ
                //adblAngle0[i] = dbla * Math.Pow(dble, dblProportion-1) + dblb;

                adblAngle0[i] = (1 - dblProportion) * dblfrAngle + dblProportion * dbltoAngle;

                //adblAngle0[i] = (1 - Math.Pow(dblProportion, dblTune)) * dblfrAngle + Math.Pow(dblProportion, dblTune) * dbltoAngle;

                //dblSumFrAngle0 = dblSumFrAngle0 + dblfrAngle;
                //dblSumToAngle0 = dblSumToAngle0 + dbltoAngle;
            }
            //double dblLengthAngleRatio = (dblSumFrToLength0 / (pCorrCptsLt.Count - 1)) / (dblSumFrToAngle0 / (pCorrCptsLt.Count - 2));

            //for (int i = 0; i < pCorrCptsLt.Count - 2; i++)
            //{
            //    adblAngle0[i] = CGeometricMethods.CalAngle2(newcptlt[i], newcptlt[i + 1], newcptlt[i + 2]);
            //}


            //���������ʼֵ���Լ����߶η�λ�ǳ�ʼֵ


            //for (int i = 0; i < pCorrCptsLt.Count; i++)
            //{
            //    pCorrCptsLt[i].FrCpt.isCtrl = false;
            //}
            //ע�⣺Ĭ�Ϲ̶���һ����
            pCorrCptsLt[0].FrCpt.isCtrl = true;
            pCorrCptsLt[1].FrCpt.isCtrl = true;
            //�̶����������            
            pCorrCptsLt[intPtNum - 2].FrCpt.isCtrl = true;
            pCorrCptsLt[intPtNum - 1].FrCpt.isCtrl = true;


            //ͳ�Ʋ�ֵ����
            int intKnownPt = 0;           //�̶������Ŀ
            int intUnknownPt = 0;         //�ǹ̶������Ŀ

            bool[] isFixed = new bool[intPtNum]; // is vertex i fixed?
            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == true)
                {
                    intKnownLocationLt.Add(i);
                    intKnownPt += 1;

                    isFixed[i] = true;
                }
                else
                {
                    intUnknownPt += 1;                   
                }
            }
            int intUnknownXY = intUnknownPt * 2;   //ÿ���㶼��X��Y����

            //�ҳ����ȹ̶���λ��(���һ���߶ε�ǰ�������㶼�̶�����ó��ȹ̶�)�����⣬���ȹ̶���ñߵķ�λ��Ҳ�̶�
            List<int> intKnownLengthLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 1; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1)
                {
                    intKnownLengthLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownLength = intPtNum - 1 - intKnownLengthLt.Count;

            //�ҳ��Ƕȹ̶���λ��(���һ���̶������ǰ�������㶼�̶�����ýǶȹ̶�)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 2; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1 && (intKnownLocationLt[i + 2] - intKnownLocationLt[i + 1]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = intPtNum - 2 - intKnownAngleLt.Count;

            //��δ֪��
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;
            List<CPoint> lastcptlt = lastcpl.CptLt;


            #region new codes
            ////����һ�ν����ֵ��Ϊ�µĹ���ֵ
            double[] adblx = new double[intPtNum];
            double[] adbly = new double[intPtNum];

            for (int i = 0; i < intPtNum; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == false)
                {
                    adblx[i] = lastcptlt[i].X;
                    adbly[i] = lastcptlt[i].Y;
                }
                else
                {
                    adblx[i] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.X + dblProportion * pCorrCptsLt[i].ToCpt.X;
                    adbly[i] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.Y + dblProportion * pCorrCptsLt[i].ToCpt.Y;
                }
            }

            double[] weight = new double[intUnknownLengthAngle];
            for (int i = 0; i < intUnknownLength; i++)
            {
                weight[i] = 1;
                //weight[i] = 0.0036;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                weight[intUnknownLength + i] = 39.48;
            }




            double[] xout = new double[intPtNum];
            double[] yout = new double[intPtNum];
            //CPlusClass.Matrix.leastSquaresAdjust(intPtNum, adblx, adbly, isFixed, adblLength0, adblAngle0, weight, dblTX, intIterationNum, xout, yout);

            //MessageBox.Show("" + xout[5]);


            //����Ŀ���߶�
            List<CPoint> CTargetPtLt = new List<CPoint>();
            for (int i = 0; i < intPtNum; i++)
            {
                CPoint cpt = new CPoint(i);
                cpt.X = xout[i];
                cpt.Y = yout[i];

                if (pCorrCptsLt[i].FrCpt.isCtrl == true)
                {
                    cpt.isCtrl = true;
                }
                else
                {
                    cpt.isCtrl = false;
                }

                CTargetPtLt.Add(cpt);
            }
            CPolyline cpl = new CPolyline(0, CTargetPtLt);
            cpl.CreateSubPllt();
            #endregion






            #region old codes          

            ////����Ȩ�ؾ���***************************************************************************************����Ȩ�ؾ���************************************************************************************************����Ȩ�ؾ���//
            //VBMatrix P = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
            //for (int i = 0; i < intUnknownLength; i++)
            //{
            //    P[i, i] = 0.0036;
            //}
            //for (int i = 0; i < intUnknownAngle; i++)
            //{
            //    P[intUnknownLength + i, intUnknownLength + i] = 40;
            //    //P[intUnknownLength + i, intUnknownLength + i] = 0.0002;
            //}

            ////����һ�ν����ֵ��Ϊ�µĹ���ֵ
            //VBMatrix X0 = new VBMatrix(intXYNum, 1);
            //for (int i = 0; i < intPtNum; i++)
            //{
            //    if (pCorrCptsLt[i].FrCpt.isCtrl == false)
            //    {
            //        X0[2 * i + 0, 0] = lastcptlt[i].X;
            //        X0[2 * i + 1, 0] = lastcptlt[i].Y;
            //    }
            //    else
            //    {
            //        X0[2 * i + 0, 0] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.X + dblProportion * pCorrCptsLt[i].ToCpt.X;
            //        X0[2 * i + 1, 0] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.Y + dblProportion * pCorrCptsLt[i].ToCpt.Y;
            //    }
            //}

            ////�����������ֵ����XA
            //VBMatrix XA = new VBMatrix(intUnknownXY, 1);
            //VBMatrix XA0 = new VBMatrix(intUnknownXY, 1);
            //int intSumCount0 = 0;
            //for (int i = 0; i < intUnknownPt; i++)
            //{
            //    if (pCorrCptsLt[intSumCount0].FrCpt.isCtrl == false)
            //    {
            //        XA[i * 2 + 0, 0] = X0[intSumCount0 * 2, 0];
            //        XA[i * 2 + 1, 0] = X0[intSumCount0 * 2 + 1, 0];

            //        XA0[i * 2 + 0, 0] = XA[i * 2 + 0, 0];
            //        XA0[i * 2 + 1, 0] = XA[i * 2 + 1, 0];
            //    }
            //    else
            //    {
            //        i -= 1;
            //    }
            //    intSumCount0 += 1;
            //}


            ////Xmix��洢��XA��X0�����»��ֵ���˾����ڹ�ʽ�Ƶ��в������ڣ�ֻ��Ϊ�˷����д�����������
            //VBMatrix Xmix = new VBMatrix(intXYNum, 1);
            //for (int i = 0; i < intXYNum; i++)
            //{
            //    Xmix[i, 0] = X0[i, 0];
            //}

            ////����ֵ��۲�ֵ֮��matl��ƽ���е�-l
            //VBMatrix matl = new VBMatrix(intUnknownLengthAngle, 1);

            ////����ϵ������A(�����̶�����ĵ���ֵ)��A�ĵ���ֵ����ѭ���и���
            //VBMatrix A = new VBMatrix(intUnknownLengthAngle, intUnknownXY);
            //VBMatrix V = new VBMatrix();
            //double dblJudge1 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            //double dblJudge2 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            //int intJudgeIndex = intUnknownLength / 4;
            //int intIterativeCount = 0;
            //double[] adblSubDis = new double[intPtNum - 1];
            //double[] adblAngle = new double[intPtNum - 2];
            //double[] adblAzimuth = new double[intPtNum - 1]; ;
            //VBMatrix LPlusV = new VBMatrix(intUnknownLengthAngle, 1);
            //do
            //{
            //    //����ϵ������A��0�е�"intUnknownLength"�еĸ�Ԫ�أ����߶γ��ȶԸ�δ֪����ƫ����ֵ
            //    //�ȼ������ĸֵ��ע�⣺��ĸʵ��������ƫ�����һ����ֵ����ȴǡ�õ�������֮����룬�������㹫ʽ�������㹫ʽ��ͬ
            //    adblSubDis = new double[intPtNum - 1];
            //    for (int i = 0; i < intPtNum - 1; i++)
            //    {
            //        adblSubDis[i] = Math.Pow((Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) * (Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) + (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]) * (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]), 0.5);
            //    }
            //    //�����µķ�λ��
            //    adblAzimuth[0] = CGeometricMethods.CalAxisAngle(Xmix[0, 0], Xmix[1, 0], Xmix[2, 0], Xmix[3, 0]);
            //    for (int i = 1; i < intPtNum - 1; i++)
            //    {
            //        adblAngle[i - 1] = CGeometricMethods.CalAngle2(Xmix[i * 2 - 2, 0], Xmix[i * 2 - 1, 0], Xmix[i * 2, 0], Xmix[i * 2 + 1, 0], Xmix[i * 2 + 2, 0], Xmix[i * 2 + 3, 0]);
            //        adblAzimuth[i] = adblAzimuth[i - 1] + adblAngle[i - 1] - Math.PI;
            //    }


            //    //����ϵ�������й��ڳ���ֵ�ĵ�������
            //    _pCAL.CalADevLength(pCorrCptsLt, 0, intUnknownLength, ref A, ref matl, adblSubDis, adblAzimuth, adblLength0);


            //    //����ϵ�������й��ڼн�ֵ�ĵ�������
            //    _pCAL.CalADevAngle(pCorrCptsLt, intUnknownLength, intUnknownAngle, Xmix, ref A, ref matl, adblSubDis, adblAngle, adblAngle0);



            //    //CHelperFunctionExcel.ExportDataToExcel2(A, "maxA", _DataRecords.ParameterInitialize.strSavePath);
            //    //CHelperFunctionExcel.ExportDataToExcelP(P, "maxP", _DataRecords.ParameterInitialize.strSavePath);
            //    //CHelperFunctionExcel.ExportDataToExcel2(matl, "maxmatl", _DataRecords.ParameterInitialize.strSavePath);


            //    //ƽ��
            //    VBMatrix Temp = A.Trans() * P * A;
            //    VBMatrix InvTemp = Temp.Inv(Temp);
            //    VBMatrix x = InvTemp * A.Trans() * P * matl;

            //    //CHelperFunctionExcel.ExportDataToExcel2(x, "maxX", _DataRecords.ParameterInitialize.strSavePath);

            //    XA += x;

            //    //CHelperFunctionExcel.ExportDataToExcel2(x, "maxX", _DataRecords.ParameterInitialize.strSavePath);
            //    //CHelperFunctionExcel.ExportDataToExcel2(XA, "maxXA", _DataRecords.ParameterInitialize.strSavePath);


            //    V = A * x - matl;
            //    //VtPVֵ
            //    double dblVtPV = (V.Trans() * P * V).MatData[0, 0];
            //    //XA -= x;
            //    _DataRecords.ParameterInitialize.txtVtPV.Text = "   VtPV = " + dblVtPV.ToString();


            //    //VBMatrix L = new VBMatrix(intUnknownLengthAngle, 1);
            //    //for (int j = 0; j < intUnknownLength; j++)
            //    //{
            //    //    L[j, 0] = adblSubDis[j];
            //    //}
            //    //for (int j = 0; j < intUnknownAngle; j++)
            //    //{
            //    //    L[intUnknownLength+ j, 0] = adblAngle[j];
            //    //}

            //    //LPlusV = L + V;



            //    //������ֵ���и���
            //    int intSumCount5 = 0;
            //    for (int i = 0; i < intUnknownPt; i++)
            //    {
            //        if (pCorrCptsLt[intSumCount5].FrCpt.isCtrl == false)
            //        {
            //            Xmix[intSumCount5 * 2 + 0, 0] = XA[i * 2 + 0, 0];
            //            Xmix[intSumCount5 * 2 + 1, 0] = XA[i * 2 + 1, 0];
            //        }
            //        else
            //        {
            //            i -= 1;
            //        }
            //        intSumCount5 += 1;
            //    }


            //    ////����ϵ������A��0�е�"intUnknownLength"�еĸ�Ԫ�أ����߶γ��ȶԸ�δ֪����ƫ����ֵ
            //    ////�ȼ������ĸֵ��ע�⣺��ĸʵ��������ƫ�����һ����ֵ����ȴǡ�õ�������֮����룬�������㹫ʽ�������㹫ʽ��ͬ
            //    //for (int i = 0; i < intPtNum - 1; i++)
            //    //{
            //    //    L[i,0] = Math.Pow((Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) * (Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) + (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]) * (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]), 0.5);
            //    //}
            //    ////�����µķ�λ��
            //    //adblAzimuth[0] = CGeometricMethods.CalAxisAngle(Xmix[0, 0], Xmix[1, 0], Xmix[2, 0], Xmix[3, 0]);
            //    //for (int i = 1; i < intPtNum - 1; i++)
            //    //{
            //    //    L[intUnknownLength + i -1, 0] = CGeometricMethods.CalAngle2(Xmix[i * 2 - 2, 0], Xmix[i * 2 - 1, 0], Xmix[i * 2, 0], Xmix[i * 2 + 1, 0], Xmix[i * 2 + 2, 0], Xmix[i * 2 + 3, 0]);
            //    //}



            //    //CHelperFunctionExcel.ExportDataToExcel2(LPlusV, "matLPlusV", _DataRecords.ParameterInitialize.strSavePath);
            //    //CHelperFunctionExcel.ExportDataToExcel2(L, "matL", _DataRecords.ParameterInitialize.strSavePath);



            //    if (intIterativeCount == 50)
            //    {
            //        int kk = 5;
            //    }

            //    intIterativeCount += 1;


            //    if (intIterativeCount >= intIterationNum)
            //    {
            //        break;
            //    }

            //    //����ֻ�����ȡ�����м�ֵ�Թ۲��Ƿ�����
            //    double dblLength0 = CGeometricMethods.CalLengthofVector(x);
            //    if (dblLength0 <= dblTX)
            //    {
            //        break;
            //    }

            //} while (true);



            ////����Ŀ���߶�
            //List<CPoint> CTargetPtLt = new List<CPoint>();
            //for (int i = 0; i < intPtNum; i++)
            //{
            //    CPoint cpt = new CPoint(i);
            //    cpt.X = Xmix[2 * i, 0];
            //    cpt.Y = Xmix[2 * i + 1, 0];

            //    if (pCorrCptsLt[i].FrCpt.isCtrl == true)
            //    {
            //        cpt.isCtrl = true;
            //    }
            //    else
            //    {
            //        cpt.isCtrl = false;
            //    }

            //    CTargetPtLt.Add(cpt);
            //}
            //CPolyline cpl = new CPolyline(0, CTargetPtLt);
            //cpl.CreateSubPllt();
            #endregion











            ////��¼��ƽ��ɹ�
            ////�������ֵ
            //VBMatrix Xc = XA-XA0;
            ////�۲�ֵ����ֵ����V
            //VBMatrix V = A * Xc + matl;
            ////VtPVֵ
            //cpl.dblVtPV = (V.Trans() * P * V).MatData[0, 0];


            //VBMatrix VLength = V.GetSubMatrix(0, intUnknownLength, 0, 1);
            //VBMatrix VAngle = V.GetSubMatrix(intUnknownLength, intUnknownAngle, 0, 1);

            //VBMatrix PLength = P.GetSubMatrix(0, intUnknownLength, 0, intUnknownLength);
            //VBMatrix PAngle = P.GetSubMatrix(intUnknownLength, intUnknownAngle, intUnknownLength, intUnknownAngle);

            //VBMatrix VtPVLength = VLength.Trans() * PLength * VLength;
            //VBMatrix VtPVAngle = VAngle.Trans() * PAngle * VAngle;

            //double dblVtPVLength = VtPVLength[0, 0];
            //double dblVtPVAngle = VtPVAngle[0, 0];
            //double pdblVtPV = (V.Trans() * P * V)[0, 0];



            //double ss = dblProportion;
            ////
            //int intUnKnownCountL6 = 0;
            //for (int i = 0; i < intPtNum - 1; i++)
            //{
            //    if (pCorrCptsLt[i].FrCpt.isCtrl == false || pCorrCptsLt[i + 1].FrCpt.isCtrl == false)
            //    {
            //        cpl.SubCPlLt[i].dblLengthV = cpl.SubCPlLt[i].Length - adblLength0[i];
            //        //double dblLength = cpl.SubCPlLt[i].Length;
            //        //double dblLength0 = adblLength0[i];


            //        intUnKnownCountL6 += 1;
            //    }
            //    else
            //    {
            //        cpl.SubCPlLt[i].dblLengthV = 0;
            //    }
            //}

            //int intUnKnownCountA6 = 0;
            //for (int i = 0; i < intPtNum - 2; i++)
            //{
            //    if (pCorrCptsLt[i].FrCpt.isCtrl == false || pCorrCptsLt[i + 1].FrCpt.isCtrl == false || pCorrCptsLt[i + 2].FrCpt.isCtrl == false)
            //    {
            //        double dblAngle = CGeometricMethods.CalAngle2(cpl.CptLt[i], cpl.CptLt[i + 1], cpl.CptLt[i + 2]);
            //        cpl.CptLt[i + 1].dblAngleV = dblAngle - adblAngle0[i];
            //        //double dblAngle = CGeometricMethods.CalAngle2(cpl.CptLt[i], cpl.CptLt[i + 1], cpl.CptLt[i + 2]);
            //        //cpl.CptLt[i + 1].dblAngleV = V[intUnknownLength + intUnKnownCountA6, 0];
            //        intUnKnownCountA6 += 1;
            //    }
            //    else
            //    {
            //        cpl.CptLt[i + 1].dblAngleV = 0;
            //    }
            //}
            //intIterationNum++;
            return cpl;

            //return null;
        }


    }
}
