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
    /// ������С����ԭ���Morphing�������ԽǶȺͱ߳�Ϊ����(Least Squares Alogrithm_Coordinate, Angle and Length)
    /// </summary>
    /// <remarks>�˼����ȺͽǶȣ�������Ϊƽ���������ƽ����������������Ĺ۲�ֵ���и���������Ϊ���в���������ƽ���</remarks>
    public class CALm
    {
        private CDataRecords _DataRecords;                    //records of data
        private double _dblTX;
        
        

        public CALm()
        {

        }

        public CALm(CDataRecords pDataRecords, double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
        }

        public CALm(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
            CPolyline FromCpl = pDataRecords.ParameterResult.FromCpl;
            _dblTX = FromCpl.pPolyline.Length / FromCpl.CptLt .Count  / 10000000000;   //������ֵ����
        }


        /// <summary>
        /// ��ʾ�����ص�����ֵ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProp">��ֵ����</param>
        /// <returns>��״Ҫ��</returns>
        public CPolyline DisplayInterpolation(double dblProp)
        {
            if (dblProp < 0 || dblProp > 1)
            {
                MessageBox.Show("��������ȷ������");
                return null;
            }

            CPolyline cpl = GetTargetcpl(dblProp);

            // ����滭�ۼ�
            IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            CHelpFunc.ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�
            return cpl;
        }

        /// <summary>
        /// ��ȡ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProp">��ֵ����</param>
        /// <returns>�ڴ�����״Ҫ��ʱ��������ԭ��״Ҫ�صı߽��п�������״Ҫ�ش����������������������״Ҫ��</returns>
        public CPolyline GetTargetcpl(double dblProp)
        {
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //��ȡ���ݺ󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            int intPtNum = pCorrCptsLt.Count;
            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double[] adblLength0 = new double[intPtNum - 1];
            double[] adblLength = new double[intPtNum - 1];   //���Ա仯��ֵ
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);
                adblLength0[i] = (1 - dblProp) * dblfrsublength + dblProp * dbltosublength;
                adblLength[i] = adblLength0[i];
            }

            //����Ƕȳ�ʼֵ��ȫ�����㣩
            double[] adblAngle0 = new double[intPtNum - 2];
            double[] adblAngle = new double[intPtNum - 2];  //���Ա仯��ֵ
            for (int i = 0; i < pCorrCptsLt.Count - 2; i++)
            {
                //�ϴ��������״Ҫ���ϵļн�
                double dblfrAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[i].FrCpt, pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i + 2].FrCpt);
                //��С��������״Ҫ���ϵļн�
                double dbltoAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[i].ToCpt, pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i + 2].ToCpt);

                //�Ƕȳ�ʼֵ
                adblAngle0[i] = (1 - dblProp) * dblfrAngle + dblProp * dbltoAngle;
                adblAngle[i] = adblAngle0[i];
            }
            
            //���ɵ����飨��ʼֵ����ͬʱ������߶η�λ�ǻ��ֵ
            //ע�⣺Ĭ�Ϲ̶���һ����
            List<CPoint> cptlt = new List<CPoint>();
            double[] adblAzimuth = new double[intPtNum - 1];
            //�����һ����͵ڶ�����
            double dblnewX0 = (1 - dblProp) * pCorrCptsLt[0].FrCpt.X + dblProp * pCorrCptsLt[0].ToCpt.X;
            double dblnewY0 = (1 - dblProp) * pCorrCptsLt[0].FrCpt.Y + dblProp * pCorrCptsLt[0].ToCpt.Y;
            CPoint newcpt0 = new CPoint(0, dblnewX0, dblnewY0);
            newcpt0.isCtrl = true;
            cptlt.Add(newcpt0);
            double dblnewX1 = (1 - dblProp) * pCorrCptsLt[1].FrCpt.X + dblProp * pCorrCptsLt[1].ToCpt.X;
            double dblnewY1 = (1 - dblProp) * pCorrCptsLt[1].FrCpt.Y + dblProp * pCorrCptsLt[1].ToCpt.Y;
            CPoint newcpt1 = new CPoint(1, dblnewX1, dblnewY1);
            newcpt1.isCtrl = true;
            cptlt.Add(newcpt1);
            adblAzimuth[0] = CGeoFunc.CalAxisAngle(newcpt0, newcpt1);
            ////����������㣨���̶���������㣩
            ////pCorrCptsLt[intPtNum - 2].FrCpt.isCtrl = false;
            ////pCorrCptsLt[intPtNum - 1].FrCpt.isCtrl = false;
            //for (int i = 2; i < pCorrCptsLt.Count; i++)
            //{
            //    CPoint newcpt = new CPoint();
            //    if (pCorrCptsLt[i].FrCpt.isCtrl == false)
            //    {
            //        adblAzimuth[i - 1] = adblAzimuth[i - 2] + adblAngle0[i - 2] - Math.PI;
            //        double dblnewX = cptlt[i - 1].X + adblLength0[i - 1] * Math.Cos(adblAzimuth[i - 1]);
            //        double dblnewY = cptlt[i - 1].Y + adblLength0[i - 1] * Math.Sin(adblAzimuth[i - 1]);
            //        newcpt = new CPoint(i, dblnewX, dblnewY);
            //    }
            //    else
            //    {
            //        double dblnewX = (1 - dblProp) * pCorrCptsLt[i].FrCpt.X + dblProp * pCorrCptsLt[i].ToCpt.X;
            //        double dblnewY = (1 - dblProp) * pCorrCptsLt[i].FrCpt.Y + dblProp * pCorrCptsLt[i].ToCpt.Y;
            //        newcpt = new CPoint(i, dblnewX, dblnewY);
            //        newcpt.isCtrl = true;

            //        //����Ƕȣ����ܲ��á�CGeoFunc.CalAxisAngle������Ϊ�˴��ķ�λ�ǲ�һ����0��2Pi֮�䣬�����������ӷ�
            //        double dblAngle = CGeoFunc.CalAngle_Counterclockwise(cptlt[cptlt.Count - 2], cptlt[cptlt.Count - 1], newcpt);  //����ʵ�ʼн� 
            //        adblAzimuth[i - 1] = adblAzimuth[i - 2] + dblAngle - Math.PI;
            //    }
            //    cptlt.Add(newcpt);
            //}

            //����������㣨�̶���������㣩
            for (int i = 2; i < pCorrCptsLt.Count - 2; i++)
            {
                CPoint newcpt = new CPoint();
                if (pCorrCptsLt[i].FrCpt.isCtrl == false)
                {
                    adblAzimuth[i - 1] = adblAzimuth[i - 2] + adblAngle0[i - 2] - Math.PI;
                    double dblnewX = cptlt[i - 1].X + adblLength0[i - 1] * Math.Cos(adblAzimuth[i - 1]);
                    double dblnewY = cptlt[i - 1].Y + adblLength0[i - 1] * Math.Sin(adblAzimuth[i - 1]);
                    newcpt = new CPoint(i, dblnewX, dblnewY);
                }
                else
                {
                    double dblnewX = (1 - dblProp) * pCorrCptsLt[i].FrCpt.X + dblProp * pCorrCptsLt[i].ToCpt.X;
                    double dblnewY = (1 - dblProp) * pCorrCptsLt[i].FrCpt.Y + dblProp * pCorrCptsLt[i].ToCpt.Y;
                    newcpt = new CPoint(i, dblnewX, dblnewY);
                    newcpt.isCtrl = true;

                    //����Ƕȣ����ܲ��á�CGeoFunc.CalAxisAngle������Ϊ�˴��ķ�λ�ǲ�һ����0��2Pi֮�䣬�����������ӷ�
                    double dblAngle = CGeoFunc.CalAngle_Counterclockwise(cptlt[cptlt.Count - 2], cptlt[cptlt.Count - 1], newcpt);  //����ʵ�ʼн� 
                    adblAzimuth[i - 1] = adblAzimuth[i - 2] + dblAngle - Math.PI;
                }
                cptlt.Add(newcpt);
            }
            //�������������
            double dblnewXlast1 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 2].FrCpt.X + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 2].ToCpt.X;
            double dblnewYlast1 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 2].FrCpt.Y + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 2].ToCpt.Y;
            CPoint newcptlast1 = new CPoint(pCorrCptsLt.Count - 2, dblnewXlast1, dblnewYlast1);
            newcptlast1.isCtrl = true;
            cptlt.Add(newcptlast1);
            double dblAnglelast1 = CGeoFunc.CalAngle_Counterclockwise(cptlt[cptlt.Count - 3], cptlt[cptlt.Count - 2], cptlt[cptlt.Count - 1]);  //����ʵ�ʼн� 
            adblAzimuth[pCorrCptsLt.Count - 3] = adblAzimuth[pCorrCptsLt.Count - 4] + dblAnglelast1 - Math.PI;

            double dblnewXlast0 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 1].FrCpt.X + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 1].ToCpt.X;
            double dblnewYlast0 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 1].FrCpt.Y + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 1].ToCpt.Y;
            CPoint newcptlast0 = new CPoint(pCorrCptsLt.Count - 1, dblnewXlast0, dblnewYlast0);
            newcptlast0.isCtrl = true;
            cptlt.Add(newcptlast0);
            double dblAnglelast0 = CGeoFunc.CalAngle_Counterclockwise(cptlt[cptlt.Count - 3], cptlt[cptlt.Count - 2], cptlt[cptlt.Count - 1]);  //����ʵ�ʼн� 
            adblAzimuth[pCorrCptsLt.Count - 2] = adblAzimuth[pCorrCptsLt.Count - 3] + dblAnglelast0 - Math.PI;


            //ͳ�Ʋ�ֵ����
            int intKnownPt = 0;           //�̶������Ŀ
            int intUnknownPt = 0;         //�ǹ̶������Ŀ

            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < cptlt.Count; i++)
            {
                if (cptlt[i].isCtrl == true)
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
            int intUnknownLength = cptlt.Count - 1 - intKnownLengthLt.Count;

            //�ҳ��Ƕȹ̶���λ��(���һ���̶������ǰ�������㶼�̶�����ýǶȹ̶�)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 2; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1 && (intKnownLocationLt[i + 2] - intKnownLocationLt[i + 1]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = cptlt.Count - 2 - intKnownAngleLt.Count;

            //��δ֪��
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;

            //����Ȩ�ؾ���
            VBMatrix P = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
            for (int i = 0; i < intUnknownLength; i++)
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                P[intUnknownLength + i, intUnknownLength + i] = 1;
            }

            //����Ȩ�ؾ�����
            VBMatrix QLL = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
            for (int i = 0; i < intUnknownLength; i++)
            {
                QLL[i, i] = 1;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                QLL[intUnknownLength + i, intUnknownLength + i] = 1;
            }
            //for (int i = 0; i < intXYCst; i++)
            //{
            //    P[intUnknownLengthAngle + i, intUnknownLengthAngle + i] = 1;
            //}
            //for (int i = 0; i < intAngleCst; i++)
            //{
            //    P[intUnknownLengthAngle + intXYCst + i, intUnknownLengthAngle + intXYCst + i] =100000;
            //}

            //�����ʼֵ����X0
            VBMatrix X0 = new VBMatrix(2 * intPtNum, 1);
            for (int i = 0; i < intPtNum; i++)
            {
                X0[2 * i, 0] = cptlt[i].X;
                X0[2 * i + 1, 0] = cptlt[i].Y;
            }
            //Xmix��洢��XA��X0�����»��ֵ���˾����ڹ�ʽ�Ƶ��в������ڣ�ֻ��Ϊ�˷����д�����������
            VBMatrix Xmix = new VBMatrix(2 * intPtNum, 1);
            for (int i = 0; i < (2 * intPtNum); i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //����ϵ������B(�����̶Գ��ȡ��Ƕȵĵ���ֵ)
            VBMatrix B = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
            for (int i = 0; i < intUnknownLengthAngle; i++)
            {
                B[i, i] = -1;
            }

            //����ϵ������A(�����̶�����ĵ���ֵ)��A�ĵ���ֵ����ѭ���и���
            VBMatrix A = new VBMatrix(intUnknownLengthAngle, intUnknownXY);
            double dblJudge1 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            double dblJudge2 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            int intJudgeIndex = intUnknownLength  / 4;
            int intIterativeCount = 0;
            double[] dblSubDis = new double[intPtNum - 1];
            do
            {
                VBMatrix w = new VBMatrix(intUnknownLengthAngle, 1);

                //����ϵ������A��0�е�"intUnknownLength"�еĸ�Ԫ�أ����߶γ��ȶԸ�δ֪����ƫ����ֵ
                //�ȼ������ĸֵ��ע�⣺��ĸʵ��������ƫ�����һ����ֵ����ȴǡ�õ�������֮����룬�������㹫ʽ�������㹫ʽ��ͬ
                dblSubDis = new double[intPtNum - 1];
                for (int i = 0; i < intPtNum - 1; i++)
                {
                    dblSubDis[i] = Math.Pow((Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) * (Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) + (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]) * (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]), 0.5);
                }
                //�����µķ�λ��
                adblAzimuth[0] = CGeoFunc.CalAxisAngle(Xmix[0, 0], Xmix[1, 0], Xmix[2, 0], Xmix[3, 0]);
                for (int i = 1; i < intPtNum - 1; i++)
                {
                    double dblAngle = CGeoFunc.CalAngle_Counterclockwise(Xmix[i * 2 - 2, 0], Xmix[i * 2 - 1, 0], Xmix[i * 2, 0], Xmix[i * 2 + 1, 0], Xmix[i * 2 + 2, 0], Xmix[i * 2 + 3, 0]);
                    adblAzimuth[i] = adblAzimuth[i-1] + dblAngle - Math.PI;
                }

                //��ʼ����ϵ�������"intUnknownXY"�е�"intUnknownXY+intUnknownLength-1"�еĸ�Ԫ��
                int intKnownCount2 = 0;
                int intUnKnownCount2 = 0;
                for (int j = 0; j < intUnknownLength; j++)
                {
                    int intSumCount = intKnownCount2 + intUnKnownCount2;
                    if (cptlt[intSumCount].isCtrl == false && cptlt[intSumCount + 1].isCtrl == false)
                    {
                        A[j, 2 * intUnKnownCount2 + 0] = -Math.Cos(adblAzimuth[intSumCount]);
                        A[j, 2 * intUnKnownCount2 + 1] = -Math.Sin(adblAzimuth[intSumCount]);
                        A[j, 2 * intUnKnownCount2 + 2] = -A[j, 2 * intUnKnownCount2 + 0];
                        A[j, 2 * intUnKnownCount2 + 3] = -A[j, 2 * intUnKnownCount2 + 1];

                        w[j, 0] = dblSubDis[intSumCount] - adblLength[intSumCount];   //ͼ���㣬˳�����matl

                        intUnKnownCount2 += 1;
                    }
                    else if (cptlt[intSumCount].isCtrl == false && cptlt[intSumCount + 1].isCtrl == true)
                    {
                        A[j, 2 * intUnKnownCount2 + 0] = -Math.Cos(adblAzimuth[intSumCount]);
                        A[j, 2 * intUnKnownCount2 + 1] = -Math.Sin(adblAzimuth[intSumCount]);

                        w[j, 0] = dblSubDis[intSumCount] - adblLength[intSumCount];   //ͼ���㣬˳�����matl

                        intUnKnownCount2 += 1;
                    }
                    else if (cptlt[intSumCount].isCtrl == true && cptlt[intSumCount + 1].isCtrl == false)
                    {
                        //ע���������������"pCorrCptsLt[intSumCount].FrCpt.isCtrl == true"��ռλ�ӣ�����ռ�У�������������ȻΪ" 2 * intUnKnownCount2 + 0"��" 2 * intUnKnownCount2 + 1"��������+2,+3
                        A[j, 2 * intUnKnownCount2 + 0] = Math.Cos(adblAzimuth[intSumCount]);
                        A[j, 2 * intUnKnownCount2 + 1] = Math.Sin(adblAzimuth[intSumCount]);

                        w[j, 0] = dblSubDis[intSumCount] - adblLength[intSumCount];   //ͼ���㣬˳�����matl

                        intKnownCount2 += 1;
                    }
                    else
                    {
                        intKnownCount2 += 1;
                        j -= 1;
                    }
                }

                //����ϵ������A��"intUnknownXY+intUnknownLength"�е�"intUnknownXY+intUnknownLength+intUnknownAngle"�еĸ�Ԫ�أ����ǶȶԸ�δ֪����ƫ����ֵ
                int intKnownCount3 = 0;
                int intUnKnownCount3 = 0;
                for (int j = 0; j < intUnknownAngle; j++)
                {
                    //����̫�����ˣ���Ȼ����������ʱ��н�ʱ��ֶ���������ۣ���������ĵ�����ʽȴ��һ�µģ���ʡ�˲��ٱ�̾�����������
                    int intSumCount = intKnownCount3 + intUnKnownCount3;

                    //��������׼��
                    double dblA2 = dblSubDis[intSumCount] * dblSubDis[intSumCount];
                    double dblB2 = dblSubDis[intSumCount + 1] * dblSubDis[intSumCount + 1];

                    //��ʼ����ϵ��ֵ�����ڽ������������������Ͻ��а����������˰����·�ʽ����
                    if (cptlt[intUnKnownCount3 + intKnownCount3].isCtrl == true && cptlt[intUnKnownCount3 + intKnownCount3 + 1].isCtrl == true && cptlt[intUnKnownCount3 + intKnownCount3 + 2].isCtrl == true)
                    {
                        intKnownCount3 += 1;
                        j -= 1;
                    }
                    else
                    {
                        double dblNewAngle = CGeoFunc.CalAngle_Counterclockwise(Xmix[2 * intSumCount + 0, 0], Xmix[2 * intSumCount + 1, 0],
                                                                       Xmix[2 * intSumCount + 2, 0], Xmix[2 * intSumCount + 3, 0],
                                                                       Xmix[2 * intSumCount + 4, 0], Xmix[2 * intSumCount + 5, 0]);
                        w[intUnknownLength + j, 0] = dblNewAngle - adblAngle[intSumCount];   //ͼ���㣬˳�����matl

                        int intPreTrueNum = 0;
                        int intUnKnownCount3orginal = intUnKnownCount3;
                        int intKnownCount3orginal = intKnownCount3;
                        if (cptlt[intUnKnownCount3orginal + intKnownCount3orginal + 0].isCtrl == false)
                        {
                            //X1,Y1�ĵ���ֵ(ע�⣺�ò����Ǽ��������ֵΪ�����ĸ���)
                            A[intUnknownLength + j, 2 * intUnKnownCount3orginal + 0] = -(Xmix[2 * intSumCount + 3, 0] - Xmix[2 * intSumCount + 1, 0]) / dblA2;
                            A[intUnknownLength + j, 2 * intUnKnownCount3orginal + 1] = (Xmix[2 * intSumCount + 2, 0] - Xmix[2 * intSumCount + 0, 0]) / dblA2;

                            intUnKnownCount3 += 1;
                        }
                        else
                        {
                            intPreTrueNum += 1;
                            intKnownCount3 += 1;
                        }

                        if (cptlt[intUnKnownCount3orginal + intKnownCount3orginal + 1].isCtrl == false)
                        {
                            //X2,Y2�ĵ���ֵ
                            //A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 2] = -(Xmix[2 * intSumCount + 5, 0] - Xmix[2 * intSumCount + 3, 0]) / dblB2 - (Xmix[2 * intSumCount + 3, 0] - Xmix[2 * intSumCount + 1, 0]) / dblA2;
                            //A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 3] = (Xmix[2 * intSumCount + 4, 0] - Xmix[2 * intSumCount + 2, 0]) / dblB2 + (Xmix[2 * intSumCount + 2, 0] - Xmix[2 * intSumCount + 0, 0]) / dblA2;

                            A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 2] = (Xmix[2 * intSumCount + 5, 0] - Xmix[2 * intSumCount + 3, 0]) / dblB2 + (Xmix[2 * intSumCount + 3, 0] - Xmix[2 * intSumCount + 1, 0]) / dblA2;
                            A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 3] = -(Xmix[2 * intSumCount + 4, 0] - Xmix[2 * intSumCount + 2, 0]) / dblB2 - (Xmix[2 * intSumCount + 2, 0] - Xmix[2 * intSumCount + 0, 0]) / dblA2;
                        }
                        else
                        {
                            intPreTrueNum += 1;
                        }
                        if (cptlt[intUnKnownCount3orginal + intKnownCount3orginal + 2].isCtrl == false)
                        {
                            //X3,Y3�ĵ���ֵ
                            A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 4] = -(Xmix[2 * intSumCount + 5, 0] - Xmix[2 * intSumCount + 3, 0]) / dblB2;
                            A[intUnknownLength + j, 2 * (intUnKnownCount3orginal - intPreTrueNum) + 5] = (Xmix[2 * intSumCount + 4, 0] - Xmix[2 * intSumCount + 2, 0]) / dblB2;
                        }
                    }
                }

                //һ���м�ֵ
                VBMatrix InvBQLLBt = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
                for (int i = 0; i < intUnknownLengthAngle; i++)  //ע�⣺�˴�֮���Կ���������������ΪB������Ԫ��Ϊ-1�ĶԽǾ�����QLL�ǶԽǾ���
                {
                    InvBQLLBt[i, i] = 1 / (QLL[i, i] * QLL[i, i]);
                }

                //����Q22
                VBMatrix TempQ22 = A.Trans() * InvBQLLBt * A;
                VBMatrix NegQ22 = TempQ22.Inv(TempQ22);
                VBMatrix Q22 = new VBMatrix(intUnknownXY, intUnknownXY);
                for (int i = 0; i < intUnknownXY; i++)
                {
                    for (int j = 0; j < intUnknownXY; j++)
                    {
                        Q22[i, j] = -NegQ22[i, j];
                    }
                }

                //����Q12
                VBMatrix NegQ12 = InvBQLLBt * A * Q22;
                VBMatrix Q12 = new VBMatrix(intUnknownLengthAngle, intUnknownXY);
                for (int i = 0; i < intUnknownLengthAngle; i++)
                {
                    for (int j = 0; j < intUnknownXY; j++)
                    {
                        Q12[i, j] = -NegQ12[i, j];
                    }
                }

                //����Q21
                VBMatrix Q21 = Q12.Trans();

                //����Q11********************************************************************
                VBMatrix I = new VBMatrix(intUnknownLengthAngle, intUnknownLengthAngle);
                for (int i = 0; i < intUnknownLengthAngle; i++)
                {
                    I[i, i] = 1;
                }
                VBMatrix  Q11= InvBQLLBt*(I-A*Q21 );                
             
                //���㸺v
                VBMatrix Negv = QLL * B.Trans() * Q11 * w;

                //���㸺x
                VBMatrix Negx = Q21 * w;

                //�Թ۲�ֵ���и���
                int intSumCountL4 = 0;
                for (int i = 0; i < intUnknownLength; i++) //���Ƚ���ֵ����
                {
                    if (cptlt[intSumCountL4].isCtrl == false || cptlt[intSumCountL4 + 1].isCtrl == false)
                    {
                        adblLength[intSumCountL4] = adblLength[intSumCountL4] - Negv[i, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountL4 += 1;
                }
                int intSumCountA4 = 0;
                for (int i = intUnknownLength; i < intUnknownLengthAngle; i++) //�ǶȽ���ֵ����
                {
                    if (cptlt[intSumCountA4].isCtrl == false || cptlt[intSumCountA4 + 1].isCtrl == false || cptlt[intSumCountA4 + 2].isCtrl == false)
                    {
                        adblAngle[intSumCountA4] = adblAngle[intSumCountA4] - Negv[i, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountA4 += 1;
                }

                //������ֵ���и���
                int intSumCount5 = 0;
                for (int i = 0; i < intUnknownPt; i++)
                {
                    if (cptlt[intSumCount5].isCtrl == false)
                    {
                        Xmix[intSumCount5 * 2, 0] = Xmix[intSumCount5 * 2, 0] - Negx[i * 2, 0];
                        Xmix[intSumCount5 * 2 + 1, 0] = Xmix[intSumCount5 * 2 + 1, 0] - Negx[i * 2 + 1, 0];
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


                if (intIterativeCount >= 1000)
                {
                    break;
                }


                //����ֻ�����ȡ�����м�ֵ�Թ۲��Ƿ�����

                dblJudge1 = Math.Abs(Negx[intJudgeIndex, 0]);
                dblJudge2 = Math.Abs(Negx[3 * intJudgeIndex, 0]);
            } while ((dblJudge1 > dblTX) || (dblJudge2 > dblTX));


            //����Ŀ���߶�
            List<CPoint> CTargetPtLt = new List<CPoint>();
            for (int i = 0; i < intPtNum; i++)
            {
                CPoint cpt = new CPoint(i);
                cpt.X = Xmix[2 * i, 0];
                cpt.Y = Xmix[2 * i + 1, 0];
                CTargetPtLt.Add(cpt);
            }
            CPolyline cpl = new CPolyline(0, CTargetPtLt);
            return cpl;

        }


    }
}
