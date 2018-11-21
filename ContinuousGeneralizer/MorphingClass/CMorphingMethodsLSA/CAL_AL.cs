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
    /// ������С����ԭ���Morphing�������ԽǶȺͱ߳�Ϊ����(Least Squares Alogrithm_Angle and Length)
    /// </summary>
    /// <remarks>�÷����Ա߳��ͽǶ���Ϊ��������ƽ�������ƽ���ı߳��Ƕ��������</remarks>
    public class CAL_AL
    {
        private CDataRecords _DataRecords;                    //records of data
        private double _dblTX;
        
        
        private CPAL _pCAL = new CPAL();

        public CAL_AL()
        {

        }

        public CAL_AL(CDataRecords pDataRecords,double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
        }

        public CAL_AL(CDataRecords pDataRecords)
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
            CDrawInActiveView.ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�
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
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //Read Datasets�󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            int intPtNum = pCorrCptsLt.Count;
            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double[] adblLength0 = new double[intPtNum - 1];
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);
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

            //���ɵ����飨��ʼֵ����ͬʱ������߶η�λ�ǻ��ֵ
            //ע�⣺Ĭ�Ϲ̶���һ����
            List <CPoint > cptlt=new List<CPoint> ();
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
            int intKnownNum = 0;           //�̶������Ŀ
            int intUnknownNum = 0;         //�ǹ̶������Ŀ

            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < cptlt.Count; i++)
            {
                if (cptlt[i].isCtrl == true)
                {
                    intKnownLocationLt.Add(i);
                    intKnownNum += 1;
                }
                else
                {
                    intUnknownNum += 1;
                }
            }

            //�ҳ����ȹ̶���λ��(���һ���߶ε�ǰ�������㶼�̶�����ó��ȹ̶�)�����⣬���ȹ̶���ñߵķ�λ��Ҳ�̶�
            List<int> intKnownLengthLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 1; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1)
                {
                    intKnownLengthLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownLength = pCorrCptsLt.Count - 1 - intKnownLengthLt.Count;

            //�ҳ��Ƕȹ̶���λ��(���һ���̶������ǰ�������㶼�̶�����ýǶȹ̶�)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 2; i++)
            {
                if ((intKnownLocationLt[i+1] - intKnownLocationLt[i]) == 1 && (intKnownLocationLt[i + 2] - intKnownLocationLt[i+1]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = pCorrCptsLt.Count - 2 - intKnownAngleLt.Count;

            //��δ֪��
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;

            //����Լ������
            int intXYCst = (intKnownLocationLt.Count - 1 - intKnownLengthLt.Count) * 2; //����������ڵ㶼�ǿ��Ƶ㣬�����������ڵ�֮�䲻��������Լ��
            //�н�Լ������
            int intAngleCst = intKnownLengthLt.Count - 1-intKnownAngleLt.Count;  //ͬ����Լ��������������ڱ߶�Ϊ��֪�ߣ���֮�䲻���ڼн�Լ��
            //��Լ������
            int intSumCst = intUnknownLengthAngle + intXYCst + intAngleCst;

            //����Ȩ�ؾ���
            VBMatrix P = new VBMatrix(intSumCst, intSumCst);
            for (int i = 0; i < intUnknownLength; i++)
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                P[intUnknownLength + i, intUnknownLength + i] = 1000;
            }
            for (int i = 0; i < intXYCst; i++)
            {
                P[intUnknownLengthAngle + i, intUnknownLengthAngle + i] = 1;
            }
            for (int i = 0; i < intAngleCst; i++)
            {
                P[intUnknownLengthAngle + intXYCst + i, intUnknownLengthAngle + intXYCst + i] =1;
            }

            //�����ʼֵ����X0��ע�⣺�˴���X0�в�δ�����κ����꣬���ǳ��Ⱥͼнǵĳ�ֵ��
            VBMatrix X0 = new VBMatrix(intPtNum * 2 - 3, 1);
            for (int i = 0; i < (intPtNum - 1); i++)
            {
                X0[i, 0] = adblLength0[i];
            }
            for (int i = 0; i < (intPtNum - 2); i++)
            {
                X0[intPtNum - 1 + i, 0] = adblAngle0[i];
            }

            //Xmix��洢��XA��X0�����»��ֵ���˾����ڹ�ʽ�Ƶ��в������ڣ�ֻ��Ϊ�˷����д�����������
            VBMatrix Xmix = new VBMatrix(intPtNum * 2 - 3, 1);
            for (int i = 0; i < X0.Row; i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //�������ֵ����XA��ע�⣺ͬ�ϣ��˴���XA�в�δ�����κ����꣬���ǳ��ȺͼнǵĽ���ֵ��
            VBMatrix XA = new VBMatrix(intUnknownLengthAngle, 1);
            int intSumCountL = 0;
            for (int i = 0; i < intUnknownLength; i++) //���Ƚ���ֵ����
            {
                if (cptlt[intSumCountL].isCtrl == false || cptlt[intSumCountL + 1].isCtrl == false)
                {
                    XA[i, 0] = X0[intSumCountL, 0];
                }
                else
                {
                    i -= 1;
                }
                intSumCountL += 1;
            }
            int intSumCountA = 0;
            for (int i = intUnknownLength; i < intUnknownLengthAngle; i++) //�ǶȽ���ֵ����
            {
                if (cptlt[intSumCountA].isCtrl == false || cptlt[intSumCountA + 1].isCtrl == false || cptlt[intSumCountA + 2].isCtrl == false)
                {
                    XA[i, 0] = X0[intPtNum -1 + intSumCountA, 0];
                }
                else
                {
                    i -= 1;
                }
                intSumCountA += 1;
            }

            //����ϵ������ϵ��������Դ������Լ�����̣�1�����ȱ���2���Ƕȱ���3��X��Y�ıպϲ4����λ�Ǳպϲ�
            //�˴������������ȱ����͡��Ƕȱ�����ϵ�����йء� X��Y�ıպϲ�͡���λ�Ǳպϲ��ϵ������ѭ���и���
            VBMatrix A = new VBMatrix(intSumCst, intUnknownLengthAngle);
            for (int i = 0; i < intUnknownLengthAngle; i++)
            {
                A[i, i] = 1;
            }

            double dblJudge1 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            double dblJudge2 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            int intJudgeIndex = intUnknownLength  / 4;
            int intIterativeCount = 0;

            do
            {               
                VBMatrix matl = new VBMatrix(intSumCst, 1);

                //����matl���ڡ����ȱ����ⲿ�ֵ�ֵ
                int intSumCountL1 = 0;
                for (int i = 0; i < intUnknownLength; i++)
                {
                    if (cptlt[intSumCountL1].isCtrl == false || cptlt[intSumCountL1 + 1].isCtrl == false)
                    {
                        matl[i, 0] = XA[i, 0] - X0[intSumCountL1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountL1 += 1;
                }
                //����matl���ڡ��Ƕȱ����ⲿ�ֵ�ֵ
                int intSumCountA1 = 0;
                for (int i = intUnknownLength; i < intUnknownLengthAngle; i++)
                {
                    if (cptlt[intSumCountA1].isCtrl == false || cptlt[intSumCountA1 + 1].isCtrl == false || cptlt[intSumCountA1 + 2].isCtrl == false)
                    {
                        matl[i, 0] = XA[i, 0] - X0[intPtNum -1 + intSumCountA1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountA1 += 1;
                }

                //����ϵ������A��"intUnknownLengthAngle"�е�"intUnknownLengthAngle + intXYCst - 1"�еĸ�Ԫ�أ�����������Լ�������ĸ�ƫ��ֵ
                if (intKnownLocationLt .Count >=2)
                {                    
                    int intRow = intUnknownLengthAngle;
                    int intLastIsCtrl = 0;
                    for (int i = 0; i < cptlt.Count; i++)
                    {
                        if (cptlt[i].isCtrl == true)
                        {
                            intLastIsCtrl = i;
                            break;
                        }
                    }
                    int intKnownLength = 0;
                    int intKnownAngle = 0;
                    for (int i = intLastIsCtrl + 1; i < cptlt.Count; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i - 1].isCtrl != true)
                        {
                            double dblSumDerX = new double();
                            double dblSumDerY = new double();

                            for (int j = intLastIsCtrl; j < i; j++)  //ע�⣬�˴���j�����Ե���0����Ϊ����0��ʱ��ô������ڼнǡ���Ȼ������֮ǰ�涨ǰ�����̶������j>=1
                            {
                                dblSumDerX = 0;
                                dblSumDerY = 0;

                                //����X��Լ�����̣����ڷֱ���Դ������Ҳ����㣬���������Լ������
                                A[intRow, j - intKnownLength] = Math.Cos(adblAzimuth[j]);  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = j; k < i; k++)
                                {
                                    dblSumDerX -= (adblLength0[k] * Math.Sin(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow, j - 1 - intKnownAngle + intUnknownLength] = dblSumDerX;

                                   //����Y��Լ�����̣��м�1������ͬ
                                A[intRow + 1, j - intKnownLength] = Math.Sin(adblAzimuth[j]);  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = j; k < i; k++)
                                {
                                    dblSumDerY += (adblLength0[k] * Math.Cos(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow + 1, j - 1 - intKnownAngle + intUnknownLength] = dblSumDerY;
                               

                                if (j == intLastIsCtrl)
                                {
                                    matl[intRow + 0, 0] = dblSumDerY - (cptlt[i].X - cptlt[intLastIsCtrl].X);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�dblSumDerY������Ϊ��ֵ��������ȣ�
                                    matl[intRow + 1, 0] = -dblSumDerX - (cptlt[i].Y - cptlt[intLastIsCtrl].Y);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�-dblSumDerX������Ϊ��ֵ��������ȣ�
                                }
                            }

                            intRow += 2;
                            intLastIsCtrl = i;
                        }
                        else if (cptlt[i].isCtrl == true && cptlt[i - 1].isCtrl == true)  //��������������궼֪����������Լ������
                        {
                            intKnownLength += 1;
                            if (i >= 2)
                            {
                                if (cptlt[i - 2].isCtrl == true)
                                {
                                    intKnownAngle += 1;
                                }
                            }
                            intLastIsCtrl = i;
                        }
                    }
                }

                //����ϵ������A��"intUnknownLengthAngle + intXYCst"�е�"intUnknownLengthAngle + intXYCst + intAngleCst - 1 (��intSumCst - 1)"�еĸ�Ԫ�أ����нǱպϲ�����ĸ�ƫ��ֵ
                if (intKnownLengthLt.Count >= 2)
                {                    
                    int intRow = intUnknownLengthAngle + intXYCst;
                    int intLastIsCtrl = 0;
                    for (int i = 0; i < cptlt.Count-1; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true)
                        {
                            intLastIsCtrl = i;
                            break;
                        }
                    }
                    int intKnownAngle = 0;
                    for (int i = intLastIsCtrl + 1; i < cptlt.Count-1; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true && cptlt[i - 1].isCtrl != true)
                        {
                            double dblAngleSum = 0;
                            for (int j = intLastIsCtrl; j < i; j++)
                            {
                                //���ڼнǵ�Լ������
                                A[intRow, j - intKnownAngle + intUnknownLength] = 1;
                                //�н��ۼ�ֵ��Ϊ����matl��׼��
                                dblAngleSum += adblAngle0[j];
                            }

                            matl[intRow, 0] = dblAngleSum - (i - intLastIsCtrl) * Math.PI - (adblAzimuth[i] - adblAzimuth[intLastIsCtrl]);   //ͼ���㣬˳�����matl

                            double tt = (i - intLastIsCtrl) * Math.PI;
                            double ss = (adblAzimuth[i] - adblAzimuth[intLastIsCtrl]);

                            intRow += 1;
                            intLastIsCtrl = i;
                        }
                        else if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true && cptlt[i - 1].isCtrl == true)  //��������������궼֪����������Լ������
                        {
                            intKnownAngle += 1;
                            intLastIsCtrl = i;
                        }
                    }
                }
                
                //�����µĽ���ֵ
                //SaveFileDialog SFD = new SaveFileDialog();
                //SFD.ShowDialog();
                //CHelpFuncExcel.ExportDataToExcelA(A, "maxA", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcelP(P, "maxP", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcel2(matl, "maxmatl", _DataRecords.ParameterInitialize.strSavePath);

                //ƽ��
                VBMatrix x = _pCAL.InvAtPAAtPmatl(A, P, matl);
                XA -= x;

                //����Xmix
                int intSumCountL4 = 0;
                for (int i = 0; i < intUnknownLength; i++) //���Ƚ���ֵ����
                {
                    if (cptlt[intSumCountL4].isCtrl == false || cptlt[intSumCountL4 + 1].isCtrl == false)
                    {
                        Xmix[intSumCountL4, 0] = XA[i, 0];   //��ʵ�˴��ġ�intSumCountL4������"intPtNum-1"
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
                        Xmix[intPtNum - 1 + intSumCountA4, 0] = XA[ i, 0];   //��ʵ�˴��ġ�intSumCountL4������"intPtNum-1"
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountA4 += 1;
                }

                //����adblAzimuth��cptlt
                for (int i = 2; i < cptlt.Count; i++)
                {
                    if (cptlt[i].isCtrl == false)
                    {
                        adblAzimuth[i - 1] = adblAzimuth[i - 2] + Xmix[intPtNum -1+ i - 2,0] - Math.PI;
                        double dblnewX = cptlt[i - 1].X + Xmix[i - 1, 0] * Math.Cos(adblAzimuth[i - 1]);
                        double dblnewY = cptlt[i - 1].Y + Xmix[i - 1, 0] * Math.Sin(adblAzimuth[i - 1]);
                        CPoint newcpt = new CPoint(i, dblnewX, dblnewY);
                        //double dbloldX = cptlt[i].X;
                        //double dbloldY = cptlt[i].Y;
                        cptlt.RemoveAt (i);
                        cptlt.Insert(i, newcpt);
                    }
                }

                intIterativeCount += 1;
                if (intIterativeCount >= 1000)
                {
                    break;
                }
                dblJudge1 = Math.Abs(x[intJudgeIndex, 0]);
                dblJudge2 = Math.Abs(x[3 * intJudgeIndex, 0]);
            } while ((dblJudge1 > dblTX) || (dblJudge2 > dblTX));


            //����Ŀ���߶�
            CPolyline cpl = new CPolyline(0, cptlt);
            return cpl;
        }


        /// <summary>
        /// ��ȡ��״Ҫ�أ�ʵ����˵�����ò������õ���Լ���Ƕ���ģ�
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProp">��ֵ����</param>
        /// <returns>�ڴ�����״Ҫ��ʱ��������ԭ��״Ҫ�صı߽��п�������״Ҫ�ش����������������������״Ҫ��</returns>
        public CPolyline GetTargetcpl2(double dblProp)
        {
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //Read Datasets�󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            int intPtNum = pCorrCptsLt.Count;
            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double[] adblLength0 = new double[intPtNum - 1];
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);
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
            //for (int i = 2; i < pCorrCptsLt.Count ; i++)
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
            //        adblAzimuth[i - 1] = CGeoFunc.CalAxisAngle(cptlt[i-1], newcpt);
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
                    adblAzimuth[i - 1] = CGeoFunc.CalAxisAngle(cptlt[i - 1], newcpt);
                }
                cptlt.Add(newcpt);
            }
            //�������������
            double dblnewXlast1 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 2].FrCpt.X + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 2].ToCpt.X;
            double dblnewYlast1 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 2].FrCpt.Y + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 2].ToCpt.Y;
            CPoint newcptlast1 = new CPoint(pCorrCptsLt.Count - 2, dblnewXlast1, dblnewYlast1);
            newcptlast1.isCtrl = true;
            cptlt.Add(newcptlast1);
            adblAzimuth[pCorrCptsLt.Count - 3] = CGeoFunc.CalAxisAngle(cptlt[cptlt.Count - 2], cptlt[cptlt.Count - 1]);

            double dblnewXlast0 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 1].FrCpt.X + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 1].ToCpt.X;
            double dblnewYlast0 = (1 - dblProp) * pCorrCptsLt[pCorrCptsLt.Count - 1].FrCpt.Y + dblProp * pCorrCptsLt[pCorrCptsLt.Count - 1].ToCpt.Y;
            CPoint newcptlast0 = new CPoint(pCorrCptsLt.Count - 1, dblnewXlast0, dblnewYlast0);
            newcptlast0.isCtrl = true;
            cptlt.Add(newcptlast0);
            adblAzimuth[pCorrCptsLt.Count - 2] = CGeoFunc.CalAxisAngle(newcptlast1, newcptlast0);


            //ͳ�Ʋ�ֵ����
            int intKnownNum = 0;           //�̶������Ŀ
            int intUnknownNum = 0;         //�ǹ̶������Ŀ

            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < cptlt.Count; i++)
            {
                if (cptlt[i].isCtrl == true)
                {
                    intKnownLocationLt.Add(i);
                    intKnownNum += 1;
                }
                else
                {
                    intUnknownNum += 1;
                }
            }

            //�ҳ����ȹ̶���λ��(���һ���߶ε�ǰ�������㶼�̶�����ó��ȹ̶�)�����⣬���ȹ̶���ñߵķ�λ��Ҳ�̶�
            List<int> intKnownLengthLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 1; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1)
                {
                    intKnownLengthLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownLength = pCorrCptsLt.Count - 1 - intKnownLengthLt.Count;

            //�ҳ��Ƕȹ̶���λ��(���һ���̶������ǰ�������㶼�̶�����ýǶȹ̶�)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 2; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1 && (intKnownLocationLt[i + 2] - intKnownLocationLt[i + 1]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = pCorrCptsLt.Count - 2 - intKnownAngleLt.Count;

            //��δ֪��
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;

            //����Լ������
            int intXYCst = (intKnownLocationLt.Count - 1 - intKnownLengthLt.Count) * 4; //����������ڵ㶼�ǿ��Ƶ㣬�����������ڵ�֮�䲻��������Լ��
            //�н�Լ������
            int intAngleCst = intKnownLengthLt.Count - 1 - intKnownAngleLt.Count;  //ͬ����Լ��������������ڱ߶�Ϊ��֪�ߣ���֮�䲻���ڼн�Լ��
            //��Լ������
            int intSumCst = intUnknownLengthAngle + intXYCst + intAngleCst;

            //����Ȩ�ؾ���
            VBMatrix P = new VBMatrix(intSumCst, intSumCst);
            for (int i = 0; i < intUnknownLength; i++)
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                P[intUnknownLength + i, intUnknownLength + i] = 10;
            }
            for (int i = 0; i < intXYCst; i++)
            {
                P[intUnknownLengthAngle + i, intUnknownLengthAngle + i] = 1;
            }
            for (int i = 0; i < intAngleCst; i++)
            {
                P[intUnknownLengthAngle + intXYCst + i, intUnknownLengthAngle + intXYCst + i] = 1;
            }

            //�����ʼֵ����X0��ע�⣺�˴���X0�в�δ�����κ����꣬���ǳ��Ⱥͼнǵĳ�ֵ��
            VBMatrix X0 = new VBMatrix(intPtNum * 2 - 3, 1);
            for (int i = 0; i < (intPtNum - 1); i++)
            {
                X0[i, 0] = adblLength0[i];
            }
            for (int i = 0; i < (intPtNum - 2); i++)
            {
                X0[intPtNum - 1 + i, 0] = adblAngle0[i];
            }

            //Xmix��洢��XA��X0�����»��ֵ���˾����ڹ�ʽ�Ƶ��в������ڣ�ֻ��Ϊ�˷����д�����������
            VBMatrix Xmix = new VBMatrix(intPtNum * 2 - 3, 1);
            for (int i = 0; i < X0.Row; i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //�������ֵ����XA��ע�⣺ͬ�ϣ��˴���XA�в�δ�����κ����꣬���ǳ��ȺͼнǵĽ���ֵ��
            VBMatrix XA = new VBMatrix(intUnknownLengthAngle, 1);
            int intSumCountL = 0;
            for (int i = 0; i < intUnknownLength; i++) //���Ƚ���ֵ����
            {
                if (cptlt[intSumCountL].isCtrl == false || cptlt[intSumCountL + 1].isCtrl == false)
                {
                    XA[i, 0] = X0[intSumCountL, 0];
                }
                else
                {
                    i -= 1;
                }
                intSumCountL += 1;
            }
            int intSumCountA = 0;
            for (int i = intUnknownLength; i < intUnknownLengthAngle; i++) //�ǶȽ���ֵ����
            {
                if (cptlt[intSumCountA].isCtrl == false || cptlt[intSumCountA + 1].isCtrl == false || cptlt[intSumCountA + 2].isCtrl == false)
                {
                    XA[i, 0] = X0[intPtNum - 1 + intSumCountA, 0];
                }
                else
                {
                    i -= 1;
                }
                intSumCountA += 1;
            }

            //����ϵ������ϵ��������Դ������Լ�����̣�1�����ȱ���2���Ƕȱ���3��X��Y�ıպϲ4����λ�Ǳպϲ�
            //�˴������������ȱ����͡��Ƕȱ�����ϵ�����йء� X��Y�ıպϲ�͡���λ�Ǳպϲ��ϵ������ѭ���и���
            VBMatrix A = new VBMatrix(intSumCst, intUnknownLengthAngle);
            for (int i = 0; i < intUnknownLengthAngle; i++)
            {
                A[i, i] = 1;
            }

            double dblJudge = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            double dblOldJudege = 0;
            int intIterativeCount = 0;

            do
            {
                VBMatrix matl = new VBMatrix(intSumCst, 1);

                //����matl���ڡ����ȱ����ⲿ�ֵ�ֵ
                int intSumCountL1 = 0;
                for (int i = 0; i < intUnknownLength; i++)
                {
                    if (cptlt[intSumCountL1].isCtrl == false || cptlt[intSumCountL1 + 1].isCtrl == false)
                    {
                        matl[i, 0] = XA[i, 0] - X0[intSumCountL1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountL1 += 1;
                }
                //����matl���ڡ��Ƕȱ����ⲿ�ֵ�ֵ
                int intSumCountA1 = 0;
                for (int i = intUnknownLength; i < intUnknownLengthAngle; i++)
                {
                    if (cptlt[intSumCountA1].isCtrl == false || cptlt[intSumCountA1 + 1].isCtrl == false || cptlt[intSumCountA1 + 2].isCtrl == false)
                    {
                        matl[i, 0] = XA[i, 0] - X0[intPtNum - 1 + intSumCountA1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountA1 += 1;
                }

                //����ϵ������A��"intUnknownLengthAngle"�е�"intUnknownLengthAngle + intXYCst - 1"�еĸ�Ԫ�أ�����������Լ�������ĸ�ƫ��ֵ
                if (intKnownLocationLt.Count >= 2)
                {
                    int intRow = intUnknownLengthAngle;
                    int intLastIsCtrl = 0;
                    for (int i = 0; i < cptlt.Count; i++)
                    {
                        if (cptlt[i].isCtrl == true)
                        {
                            intLastIsCtrl = i;
                            break;
                        }
                    }
                    int intKnownLength = 0;
                    int intKnownAngle = 0;
                    for (int i = intLastIsCtrl + 1; i < cptlt.Count; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i - 1].isCtrl != true)
                        {
                            double dblSumDerX = new double();
                            double dblSumDerY = new double();
                            double dblSumDerX2 = new double();
                            double dblSumDerY2 = new double();

                            for (int j = intLastIsCtrl; j < i; j++)  //ע�⣬�˴���j�����Ե���0����Ϊ����0��ʱ��ô������ڼнǡ���Ȼ������֮ǰ�涨ǰ�����̶������j>=1
                            {
                                dblSumDerX = 0;
                                dblSumDerY = 0;
                                dblSumDerX2 = 0;
                                dblSumDerY2 = 0;

                                //����X��Լ�����̣����ڷֱ���Դ������Ҳ����㣬���������Լ������
                                A[intRow, j - intKnownLength] = Math.Cos(adblAzimuth[j]);  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = j; k < i; k++)
                                {
                                    dblSumDerX -= (adblLength0[k] * Math.Sin(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow, j - 1 - intKnownAngle + intUnknownLength] = dblSumDerX;

                                A[intRow + 1, j - intKnownLength] = A[intRow, j - intKnownLength];  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = intLastIsCtrl; k <= j; k++)
                                {
                                    dblSumDerX2 += (adblLength0[k] * Math.Sin(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow + 1, j - intKnownAngle + intUnknownLength] = dblSumDerX2;

                                //����Y��Լ�����̣��м�1������ͬ
                                A[intRow + 2, j - intKnownLength] = Math.Sin(adblAzimuth[j]);  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = j; k < i; k++)
                                {
                                    dblSumDerY += (adblLength0[k] * Math.Cos(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow + 2, j - 1 - intKnownAngle + intUnknownLength] = dblSumDerY;

                                //����Y��Լ�����̣��м�1������ͬ
                                A[intRow + 3, j - intKnownLength] = A[intRow + 2, j - intKnownLength];  //���ڳ��ȵ�ƫ��ֵ
                                for (int k = intLastIsCtrl; k <= j; k++)
                                {
                                    dblSumDerY2 -= (adblLength0[k] * Math.Cos(adblAzimuth[k]));  //���ڼнǵ�ƫ��ֵ
                                }
                                A[intRow + 3, j - intKnownAngle + intUnknownLength] = dblSumDerY2;

                                if (j == intLastIsCtrl)
                                {
                                    matl[intRow + 0, 0] = dblSumDerY - (cptlt[i].X - cptlt[intLastIsCtrl].X);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�dblSumDerY������Ϊ��ֵ��������ȣ�
                                    matl[intRow + 2, 0] = -dblSumDerX - (cptlt[i].Y - cptlt[intLastIsCtrl].Y);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�-dblSumDerX������Ϊ��ֵ��������ȣ�
                                }

                                if (j == (i - 1))
                                {
                                    matl[intRow + 1, 0] = -dblSumDerY2 - (cptlt[i].X - cptlt[intLastIsCtrl].X);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�dblSumDerY������Ϊ��ֵ��������ȣ�
                                    matl[intRow + 3, 0] = dblSumDerX2 - (cptlt[i].Y - cptlt[intLastIsCtrl].Y);   //ͼ���㣬˳�����matl���˴�֮��������֮ǰ�ĳɹ�-dblSumDerX������Ϊ��ֵ��������ȣ�
                                }
                            }

                            intRow += 4;
                            intLastIsCtrl = i;
                        }
                        else if (cptlt[i].isCtrl == true && cptlt[i - 1].isCtrl == true)  //��������������궼֪����������Լ������
                        {
                            intKnownLength += 1;
                            if (i >= 2)
                            {
                                if (cptlt[i - 2].isCtrl == true)
                                {
                                    intKnownAngle += 1;
                                }
                            }
                            intLastIsCtrl = i;
                        }
                    }
                }

                //����ϵ������A��"intUnknownLengthAngle + intXYCst"�е�"intUnknownLengthAngle + intXYCst + intAngleCst - 1 (��intSumCst - 1)"�еĸ�Ԫ�أ����нǱպϲ�����ĸ�ƫ��ֵ
                if (intKnownLengthLt.Count >= 2)
                {
                    int intRow = intUnknownLengthAngle + intXYCst;
                    int intLastIsCtrl = 0;
                    for (int i = 0; i < cptlt.Count - 1; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true)
                        {
                            intLastIsCtrl = i;
                            break;
                        }
                    }
                    int intKnownAngle = 0;
                    for (int i = intLastIsCtrl + 1; i < cptlt.Count - 1; i++)
                    {
                        if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true && cptlt[i - 1].isCtrl != true)
                        {
                            double dblAngleSum = 0;
                            for (int j = intLastIsCtrl; j < i; j++)
                            {
                                //���ڼнǵ�Լ������
                                A[intRow, j - intKnownAngle + intUnknownLength] = 1;
                                //�н��ۼ�ֵ��Ϊ����matl��׼��
                                dblAngleSum += adblAngle0[j];
                            }

                            matl[intRow, 0] = dblAngleSum - (i - intLastIsCtrl) * Math.PI - (adblAzimuth[i] - adblAzimuth[intLastIsCtrl]);   //ͼ���㣬˳�����matl

                            intRow += 1;
                            intLastIsCtrl = i;
                        }
                        else if (cptlt[i].isCtrl == true && cptlt[i + 1].isCtrl == true && cptlt[i - 1].isCtrl == true)  //��������������궼֪����������Լ������
                        {
                            intKnownAngle += 1;
                            intLastIsCtrl = i;
                        }
                    }
                }

                //��¼һ��ֵ��Э���ж��Ƿ�����˳�ѭ��
                double dblLast = XA[0, 0];

                //�����µĽ���ֵ
                //SaveFileDialog SFD = new SaveFileDialog();
                //SFD.ShowDialog();
                //CHelpFuncExcel.ExportDataToExcelA(A, "maxA", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcelP(P, "maxP", _DataRecords.ParameterInitialize.strSavePath);
                //CHelpFuncExcel.ExportDataToExcel2(matl, "maxmatl", _DataRecords.ParameterInitialize.strSavePath);

                //ƽ��
                XA -= _pCAL.InvAtPAAtPmatl(A, P, matl);

                //����Xmix
                int intSumCountL4 = 0;
                for (int i = 0; i < intUnknownLength; i++) //���Ƚ���ֵ����
                {
                    if (cptlt[intSumCountL4].isCtrl == false || cptlt[intSumCountL4 + 1].isCtrl == false)
                    {
                        Xmix[intSumCountL4, 0] = XA[i, 0];   //��ʵ�˴��ġ�intSumCountL4������"intPtNum-1"
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
                        Xmix[intPtNum - 1 + intSumCountA4, 0] = XA[i, 0];   //��ʵ�˴��ġ�intSumCountL4������"intPtNum-1"
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCountA4 += 1;
                }

                //����adblAzimuth��cptlt
                for (int i = 2; i < cptlt.Count; i++)
                {
                    if (cptlt[i].isCtrl == false)
                    {
                        adblAzimuth[i - 1] = adblAzimuth[i - 2] + Xmix[intPtNum - 1 + i - 2, 0] - Math.PI;
                        double dblnewX = cptlt[i - 1].X + Xmix[i - 1, 0] * Math.Cos(adblAzimuth[i - 1]);
                        double dblnewY = cptlt[i - 1].Y + Xmix[i - 1, 0] * Math.Sin(adblAzimuth[i - 1]);
                        CPoint newcpt = new CPoint(i, dblnewX, dblnewY);
                        double dbloldX = cptlt[i].X;
                        double dbloldY = cptlt[i].Y;
                        cptlt.RemoveAt(i);
                        cptlt.Insert(i, newcpt);
                    }
                }

                dblJudge = Math.Abs(dblLast - XA[0, 0]);
                intIterativeCount += 1;
                if (intIterativeCount >= 1000)
                {
                    break;
                }
                if (dblOldJudege == dblJudge)
                {
                    break;
                }

                dblOldJudege = dblJudge;
            } while (dblJudge > dblTX);


            //����Ŀ���߶�
            CPolyline cpl = new CPolyline(0, cptlt);
            return cpl;
        }
    }
}
