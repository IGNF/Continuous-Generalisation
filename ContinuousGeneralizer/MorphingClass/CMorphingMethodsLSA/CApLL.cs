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



namespace MorphingClass.CMorphingMethodsLSA
{
    /// <summary>
    /// ������С����ԭ���Morphing�������ԡ��Ƕȡ��߳�������һ���������֮��ľ��루���ࣩ��Ϊ�۲�ֵ(Least Squares Alogrithm_Coordinate, Angle and Length)
    /// </summary>
    /// <remarks>�˼����ȺͽǶȺ�������һ���������֮��ľ��룬������Ϊƽ���������ƽ���׼���ƽ��</remarks>
    public class CApLL
    {
        private CDataRecords _DataRecords;                    //records of data
        private double _dblTX;
        
        
        private CPAL _pCAL = new CPAL();

        private double _dblMaxLengthV;
        private double _dblMinLengthV;
        private double _dblDiffLengthV;
        private double _dblMaxAngleV;
        private double _dblMinAngleV;
        private double _dblDiffAngleV;


        //private double _dblMaxELength;   //the max value of Edges' Lengths

        public CApLL()
        {

        }

        public CApLL(CDataRecords pDataRecords, double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
            //CalParameters();
            //_dblMaxELength = pDataRecords.ParameterResult.FromCpl.GetMaxELength() / 100; 
        }

        public CApLL(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
            CPolyline FromCpl = pDataRecords.ParameterResult.FromCpl;
            _dblTX = FromCpl.pPolyline.Length / FromCpl.CptLt .Count  / 10000;   //������ֵ����
            //CalParameters();
            //_dblMaxELength = pDataRecords.ParameterResult.FromCpl.GetMaxELength()/100;
        }

        private void CalParameters()
        {
            _DataRecords.ParameterInitialize.tsslMessage.Text = "CALCCulating Parameters......";
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
        /// <param name="dblProp">��ֵ����</param>
        /// <returns>��״Ҫ��</returns>
        public CPolyline DisplayInterpolation(double dblProp, CPolyline lastcpl)
        {


            CPolyline cpl = GetTargetcpl(dblProp, lastcpl);

            //_DataRecords.ParameterInitialize.txtT.Text = "   t = " + dblProp.ToString();
            //_DataRecords.ParameterInitialize.txtVtPV.Text ="   VtPV = " + cpl.dblVtPV.ToString();

            //// ����滭�ۼ�
            //IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            //IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            //pGra.DeleteApLLElements();

            //if (dblProp == 0)
            //{
            //    int tt = 5;
            //}
            //else if (dblProp == 1)
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
            //    CDrawInActiveView.ViewPolylineByRGB(m_mapControl, subcpl, subcpl.intRed, subcpl.intGreen, subcpl.intBlue);  //��ʾ���ɵ��߶�
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
            //    CHelpFunc.ViewPolygonGeometryByRGB(m_mapControl, cpt.Buffer(5), cpt.intRed, cpt.intGreen, cpt.intBlue);  //��ʾ�����������
            //}

            ////��ʾ�߶�
            //IActiveView pAv = pGra as IActiveView;
            //pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);



            // ����滭�ۼ�
            IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            CDrawInActiveView.ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�


            return cpl;
        }

        /// <summary>
        /// ��ȡ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProp">��ֵ����</param>
        /// <returns>�ڴ�����״Ҫ��ʱ��������ԭ��״Ҫ�صı߽��п�������״Ҫ�ش����������������������״Ҫ��</returns>
        public CPolyline GetTargetcpl(double dblProp, CPolyline lastcpl)
        {

            if (dblProp == 0)
            {
                int aa = 5;
            }

            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //��ȡ���ݺ󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            int intPtNum = pCorrCptsLt.Count;
            int intXYNum = 2 * intPtNum;

            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double[] adblLength0 = new double[intPtNum - 1];
            double[] adblFrLength0 = new double[intPtNum - 1];
            double[] adblToLength0 = new double[intPtNum - 1];
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                adblFrLength0[i] = dblfrsublength;
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);
                adblToLength0[i] = dbltosublength;
                adblLength0[i] = (1 - dblProp) * dblfrsublength + dblProp * dbltosublength;
            }

            //����Ƕȳ�ʼֵ��ȫ�����㣩
            double[] adblAngle0 = new double[intPtNum - 2];
            for (int i = 0; i < pCorrCptsLt.Count - 2; i++)
            {
                //�ϴ��������״Ҫ���ϵļн�
                double dblfrAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[i].FrCpt, pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i + 2].FrCpt);
                //��С��������״Ҫ���ϵļн�
                double dbltoAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[i].ToCpt, pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i + 2].ToCpt);

                //�Ƕȳ�ʼֵ
                adblAngle0[i] = (1 - dblProp) * dblfrAngle + dblProp * dbltoAngle;
            }

            //���������ʼֵ���Լ����߶η�λ�ǳ�ʼֵ
            //ע�⣺Ĭ�Ϲ̶���һ����
            pCorrCptsLt[0].FrCpt.isCtrl = true;
            pCorrCptsLt[1].FrCpt.isCtrl = true;
            //�̶����������
            pCorrCptsLt[intPtNum - 1].FrCpt.isCtrl = true;
            pCorrCptsLt[intPtNum - 2].FrCpt.isCtrl = true;

            VBMatrix X0 = new VBMatrix(intXYNum, 1);

            //����һ�ν����ֵ��Ϊ�µĹ���ֵ
            List<CPoint> lastcptlt = lastcpl.CptLt;
            for (int i = 0; i < intPtNum; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == false)
                {
                    X0[2 * i + 0, 0] = lastcptlt[i].X;
                    X0[2 * i + 1, 0] = lastcptlt[i].Y;
                }
                else
                {
                    X0[2 * i + 0, 0] = (1 - dblProp) * pCorrCptsLt[i].FrCpt.X + dblProp * pCorrCptsLt[i].ToCpt.X;
                    X0[2 * i + 1, 0] = (1 - dblProp) * pCorrCptsLt[i].FrCpt.Y + dblProp * pCorrCptsLt[i].ToCpt.Y;
                }
            }

            //ͳ�Ʋ�ֵ����
            int intKnownPt = 0;           //�̶������Ŀ
            int intUnknownPt = 0;         //�ǹ̶������Ŀ

            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == true)
                {
                    intKnownLocationLt.Add(i);
                    intKnownPt += 1;
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

            //���ȼ��Ƕ�δ֪��������ʹ�ã�
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;
            //��δ֪��
            int intUnknownLengthAnglePt = intUnknownLength + intUnknownAngle +intUnknownPt;

            //����Ȩ�ؾ���
            int intKnownCount = 0;
            int intUnKnownCount = 0;
            VBMatrix P = new VBMatrix(intUnknownLengthAnglePt, intUnknownLengthAnglePt);
            for (int i = 0; i < intUnknownLength; i++)
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                int intSumCount = intKnownCount + intUnKnownCount;

                //��ʼ����ϵ��ֵ�����ڽ������������������Ͻ��а����������˰����·�ʽ����
                if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount+1].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 2].FrCpt.isCtrl == true)
                {                    
                    i -= 1;
                }
                else
                {
                    double dblWeight = 0;
                    if (pCorrCptsLt[intSumCount+1].FrCpt.isCtrl == false)
                    {
                        dblWeight = adblFrLength0[intSumCount] + adblFrLength0[intSumCount + 1] + adblToLength0[intSumCount] + adblToLength0[intSumCount + 1]; 
                    }
                    else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 2].FrCpt.isCtrl == false)
                    {
                        dblWeight = adblFrLength0[intSumCount] + adblFrLength0[intSumCount + 1] + adblToLength0[intSumCount] + adblToLength0[intSumCount + 1];
                    }
                    else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 2].FrCpt.isCtrl == false)
                    {
                        dblWeight = adblFrLength0[intSumCount + 1] + adblToLength0[intSumCount + 1];
                    }
                    else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 2].FrCpt.isCtrl == true)
                    {
                        dblWeight =  adblFrLength0[intSumCount] +  adblToLength0[intSumCount];
                    }

                    P[intUnknownLength + i, intUnknownLength + i] = dblWeight;
                }

                if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true)
                {
                    intKnownCount += 1;
                }
                else
                {
                    intUnKnownCount += 1;
                }
            }
            for (int i = 0; i < intUnknownPt; i++)
            {
                P[intUnknownLengthAngle + i, intUnknownLengthAngle + i] = 0.000000000000001;
            }

            //�����������ֵ����XA
            VBMatrix XA = new VBMatrix(intUnknownXY, 1);
            VBMatrix XA0 = new VBMatrix(intUnknownXY, 1);
            int intSumCount0 = 0;
            for (int i = 0; i < intUnknownPt; i++)
            {
                if (pCorrCptsLt[intSumCount0].FrCpt.isCtrl == false)
                {
                    XA0[i * 2 + 0, 0] = X0[intSumCount0 * 2 + 0, 0];
                    XA0[i * 2 + 1, 0] = X0[intSumCount0 * 2 + 1, 0];

                    XA[i * 2 + 0, 0] = XA0[i * 2 + 0, 0]-0.0000000001;
                    XA[i * 2 + 1, 0] = XA0[i * 2 + 1, 0] - 0.0000000001;
                }
                else
                {
                    i -= 1;
                }
                intSumCount0 += 1;
            }

            //Xmix��洢��XA��X0�����»��ֵ���˾����ڹ�ʽ�Ƶ��в������ڣ�ֻ��Ϊ�˷����д�����������
            VBMatrix Xmix = new VBMatrix(intXYNum, 1);
            for (int i = 0; i < intXYNum; i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //����ֵ��۲�ֵ֮��matl��ƽ���е�-l
            VBMatrix matl = new VBMatrix(intUnknownLengthAnglePt, 1);

            //����ϵ������A(�����̶�����ĵ���ֵ)��A�ĵ���ֵ����ѭ���и���
            VBMatrix A = new VBMatrix(intUnknownLengthAnglePt, intUnknownXY);
            double dblJudge1 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            double dblJudge2 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            int intJudgeIndex = intUnknownLength / 4;
            int intIterativeCount = 0;
            double[] adblSubDis = new double[intPtNum - 1];
            double[] adblAngle = new double[intPtNum - 2];
            double[] adblAzimuth = new double[intPtNum - 1]; 
            do
            {
                //����ϵ������A��0�е�"intUnknownLength"�еĸ�Ԫ�أ����߶γ��ȶԸ�δ֪����ƫ����ֵ
                for (int i = 0; i < intPtNum - 1; i++)
                {
                    adblSubDis[i] = Math.Pow((Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) * (Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) + (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]) * (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]), 0.5);
                }
                //�����µķ�λ��
                adblAzimuth[0] = CGeoFunc.CalAxisAngle(Xmix[0, 0], Xmix[1, 0], Xmix[2, 0], Xmix[3, 0]);
                for (int i = 1; i < intPtNum - 1; i++)
                {
                    adblAngle[i - 1] = CGeoFunc.CalAngle_Counterclockwise(Xmix[i * 2 - 2, 0], Xmix[i * 2 - 1, 0], Xmix[i * 2, 0], Xmix[i * 2 + 1, 0], Xmix[i * 2 + 2, 0], Xmix[i * 2 + 3, 0]);
                    adblAzimuth[i] = adblAzimuth[i - 1] + adblAngle[i - 1] - Math.PI;
                }

                //����ϵ�������й��ڳ���ֵ�ĵ�������
                _pCAL.CalADevLength(pCorrCptsLt, 0, intUnknownLength, ref A, ref matl, adblSubDis, adblAzimuth, adblLength0);

                //����ϵ�������й��ڼн�ֵ�ĵ�������
                _pCAL.CalADevAngle(pCorrCptsLt, intUnknownLength, intUnknownAngle, Xmix, ref A, ref matl, adblSubDis, adblAngle, adblAngle0);

                //����ϵ�������й�������һ��LSA����������ĵ�������
                for (int i = 0; i < intUnknownPt ; i++)
                {
                    //ƽ���ĵ������ֵ��֮��ľ���
                    double dblDis = Math.Pow((XA[2 * i, 0] - XA0[2 * i, 0]) * (XA[2 * i, 0] - XA0[2 * i, 0]) + (XA[2 * i + 1, 0] - XA0[2 * i + 1, 0]) * (XA[2 * i + 1, 0] - XA0[2 * i + 1, 0]), 0.5);

                    A[intUnknownLengthAngle + i, i * 2 + 0] = (XA[2 * i + 0, 0] - XA0[2 * i + 0, 0]) / dblDis;
                    A[intUnknownLengthAngle + i, i * 2 + 1] = (XA[2 * i + 1, 0] - XA0[2 * i + 1, 0]) / dblDis; 

                    //if (dblDis==0)
                    //{
                    //    //����ֵ
                    //    A[intUnknownLengthAngle + i, i * 2 + 0] = 0;
                    //    A[intUnknownLengthAngle + i, i * 2 + 1] = 0; 
                    //}
                    //else
                    //{
                    //    //����ֵ
                    //    A[intUnknownLengthAngle + i, i * 2 + 0] = (XA[2 * i + 0, 0] - XA0[2 * i + 0, 0]) / dblDis;
                    //    A[intUnknownLengthAngle + i, i * 2 + 1] = (XA[2 * i + 1, 0] - XA0[2 * i + 1, 0]) / dblDis; 
                    //}

                    matl[intUnknownLengthAngle + i, 0] = -dblDis;    //ͼ���㣬˳�����matl
                }

                //CHelpFuncExcel.ExportDataToExcel2(A, "maxA", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcelP(P, "maxP", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcel2(matl, "maxmatl", _DataRecords.ParameterInitialize.strSavePath);


                //ƽ�������С���˷���
                VBMatrix Temp = A.Trans() * P * A;                
                //VBMatrix E = new VBMatrix(intUnknownXY, intUnknownXY);  //��λ����
                //for (int i = 0; i < intUnknownXY; i++)
                //{
                //    E[i, i] = 1;
                //}
                //Temp = Temp + E;
                VBMatrix InvTemp = Temp.Inv(Temp);
                VBMatrix x = InvTemp * A.Trans() * P * matl;

                //CHelpFuncExcel.ExportDataToExcel2(x, "maxX", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcel2(XA, "maxXA", _DataRecords.ParameterInitialize.strSavePath);

                XA += x;

                //������ֵ���и���
                int intSumCount5 = 0;
                for (int i = 0; i < intUnknownPt; i++)
                {
                    if (pCorrCptsLt[intSumCount5].FrCpt.isCtrl == false)
                    {
                        Xmix[intSumCount5 * 2 + 0, 0] = XA[i * 2 + 0, 0];
                        Xmix[intSumCount5 * 2 + 1, 0] = XA[i * 2 + 1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCount5 += 1;
                }

                if (intIterativeCount == 50)
                {
                    int kk = 5;
                }

                intIterativeCount += 1;


                if (intIterativeCount >= 20)
                {
                    break;
                }

                //����ֻ�����ȡ�����м�ֵ�Թ۲��Ƿ�����
                dblJudge1 = Math.Abs(x[intJudgeIndex, 0]);
                dblJudge2 = Math.Abs(x[3 * intJudgeIndex, 0]);
            } while ((dblJudge1 > dblTX) || (dblJudge2 > dblTX));


            //����Ŀ���߶�
            List<CPoint> CTargetPtLt = new List<CPoint>();
            for (int i = 0; i < intPtNum; i++)
            {
                CPoint cpt = new CPoint(i);
                cpt.X = Xmix[2 * i, 0];
                cpt.Y = Xmix[2 * i + 1, 0];

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

            //��¼��ƽ��ɹ�
            //�������ֵ
            VBMatrix Xc = XA-XA0;
            //�۲�ֵ����ֵ����V
            VBMatrix V = A * Xc + matl;
            //VtPVֵ
            cpl.dblVtPV = (V.Trans() * P * V).MatData[0, 0];


            int intUnKnownCountL6 = 0;
            for (int i = 0; i < intPtNum - 1; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == false || pCorrCptsLt[i + 1].FrCpt.isCtrl == false)
                {
                    cpl.SubCPlLt[i].dblLengthV = cpl.SubCPlLt[i].pPolyline.Length - adblLength0[i];
                    //double dblLength = cpl.SubCPlLt[i].Length;
                    //double dblLength0 = adblLength0[i];


                    intUnKnownCountL6 += 1;
                }
                else
                {
                    cpl.SubCPlLt[i].dblLengthV = 0;
                }
            }

            int intUnKnownCountA6 = 0;
            for (int i = 0; i < intPtNum - 2; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == false || pCorrCptsLt[i + 1].FrCpt.isCtrl == false || pCorrCptsLt[i + 2].FrCpt.isCtrl == false)
                {
                    double dblAngle = CGeoFunc.CalAngle_Counterclockwise(cpl.CptLt[i], cpl.CptLt[i + 1], cpl.CptLt[i + 2]);
                    cpl.CptLt[i + 1].dblAngleV = dblAngle - adblAngle0[i];
                    //double dblAngle = CGeoFunc.CalAngle_Counterclockwise(cpl.CptLt[i], cpl.CptLt[i + 1], cpl.CptLt[i + 2]);
                    //cpl.CptLt[i + 1].dblAngleV = V[intUnknownLength + intUnKnownCountA6, 0];
                    intUnKnownCountA6 += 1;
                }
                else
                {                    
                    cpl.CptLt[i + 1].dblAngleV = 0;
                }
            }

            return cpl;

        }


    }
}
