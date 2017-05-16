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
    /// <remarks>�����ꡢ���ȺͽǶ�Ϊ��������ƽ��</remarks>
    public class CPAL
    {
        private CDataRecords _DataRecords;                    //���ݼ�¼
        private double _dblTX;
        
        

        public CPAL()
        {

        }

        public CPAL(CDataRecords pDataRecords,double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
        }

        public CPAL(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
            CPolyline FromCpl = pDataRecords.ParameterResult.FromCpl;
            _dblTX = FromCpl.pPolyline.Length / FromCpl.CptLt .Count  / 1000000;   //������ֵ����
        }


        /// <summary>
        /// ��ʾ�����ص�����ֵ��״Ҫ��
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��״Ҫ��</returns>
        public CPolyline DisplayInterpolation(double dblProportion)
        {
            if (dblProportion < 0 || dblProportion > 1)
            {
                MessageBox.Show("��������ȷ������");
                return null;
            }

            CPolyline cpl = GetTargetcpl(dblProportion);

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
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>�ڴ�����״Ҫ��ʱ��������ԭ��״Ҫ�صı߽��п�������״Ҫ�ش����������������������״Ҫ��</returns>
        public CPolyline GetTargetcpl(double dblProportion)
        {
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //��ȡ���ݺ󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            double dblTX = _dblTX;

            //ͳ�Ʋ�ֵ����
            int intKnownNum = 0;           //��̶�������ƽ��ĵ����Ŀ
            int intUnknownNum = 0;         //�����ƽ��ĵ����Ŀ
            
            List<int> intKnownLocationLt = new List<int>();  //��¼��֪������
            //ע�⣺���ڸ�ѭ������һ��Ĭ����������FromCpl�ĵ�һ������ֻ��һ����Ӧ��
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                if (pCorrCptsLt[i].FrCpt.isCtrl == true)
                {
                    intKnownLocationLt.Add(i);
                    intKnownNum += 1;
                }
                else
                {
                    intUnknownNum += 1;
                }
            }

            int intUnknownXY = intUnknownNum * 2;   //ÿ���㶼��X��Y����
            int intPtNum = pCorrCptsLt.Count;
            int intXYNum = 2 * intPtNum;


            //�ҳ����ȹ̶���λ��(���һ���߶ε�ǰ�������㶼�̶�����ó��ȹ̶�)
            List<int> intKnownLengthLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count-1 ; i++)
            {
                if ((intKnownLocationLt[i+1]-intKnownLocationLt[i])==1)
                {
                    intKnownLengthLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownLength = pCorrCptsLt.Count - 1 - intKnownLengthLt.Count;

            //�ҳ��Ƕȹ̶���λ��(���һ���̶������ǰ�������㶼�̶�����ýǶȹ̶�)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 1; i < intKnownLocationLt.Count - 1; i++)
            {
                if ((intKnownLocationLt[i] - intKnownLocationLt[i - 1]) == 1 && (intKnownLocationLt[i+1] - intKnownLocationLt[i ]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = pCorrCptsLt.Count - 2 - intKnownAngleLt.Count;

            int intUnknownXYLengthAngle = intUnknownXY + intUnknownLength + intUnknownAngle;

            //����Ȩ�ؾ���
            VBMatrix P = new VBMatrix(intUnknownXYLengthAngle, intUnknownXYLengthAngle);
            for (int i = 0; i < intUnknownXY; i++)
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intUnknownLength; i++)
            {
                P[intUnknownXY + i, intUnknownXY + i] = 100;
            }
            for (int i = 0; i < intUnknownAngle; i++)
            {
                P[intUnknownXY + intUnknownLength + i, intUnknownXY + intUnknownLength + i] = 10000;
            }
  
            //�����ʼֵ����X0
            VBMatrix X0 = new VBMatrix(intXYNum, 1);
            int intCount = 0;
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                X0[intCount, 0] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.X + dblProportion * pCorrCptsLt[i].ToCpt.X;
                X0[intCount + 1, 0] = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.Y + dblProportion * pCorrCptsLt[i].ToCpt.Y;
                intCount += 2;
            }

            //���㳤�ȳ�ʼֵ��ȫ�����㣩
            double[] adblLength0 = new double[intPtNum-1];
            for (int i = 0; i < pCorrCptsLt.Count - 1; i++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].FrCpt, pCorrCptsLt[i].FrCpt);
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[i + 1].ToCpt, pCorrCptsLt[i].ToCpt);
                adblLength0[i] = (1 - dblProportion) * dblfrsublength + dblProportion * dbltosublength;
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
                adblAngle0[i] = (1 - dblProportion) * dblfrAngle + dblProportion * dbltoAngle;
            }

            //Xmix��洢��XA��X0�����»��ֵ
            VBMatrix Xmix = new VBMatrix(intXYNum, 1);
            for (int i = 0; i < X0.Row; i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //�����������ֵ����XA
            VBMatrix XA = new VBMatrix(intUnknownXY, 1);
            int intSumCount0 = 0;
            for (int i = 0; i < intUnknownNum; i++)
            {
                if (pCorrCptsLt[intSumCount0].FrCpt.isCtrl == false)
                {
                    XA[i * 2, 0] = X0[intSumCount0 * 2, 0];
                    XA[i * 2 + 1, 0] = X0[intSumCount0 * 2 + 1, 0];
                }
                else
                {
                    i -= 1;
                }
                intSumCount0 += 1;
            }

            //����ϵ�������йس��ȺͽǶȵ�ֵ����ѭ���и�����
            VBMatrix A = new VBMatrix(intUnknownXYLengthAngle, intUnknownXY); 
            for (int i = 0; i < intUnknownXY; i++)
            {
                A[i, i] = 1;
            }

            double dblJudge1 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            double dblJudge2 = 0;   //��ֵ�����ж��Ƿ�Ӧ������ѭ��
            int intJudgeIndex = intUnknownXY  / 4;
            int intIterativeCount = 0;
            double[] adblSubDis = new double[intPtNum - 1];
            double[] adblAngle = new double[intPtNum - 2];
            double[] adblAzimuth = new double[intPtNum - 1];            
            VBMatrix matl = new VBMatrix(intUnknownXYLengthAngle, 1);
            do
            {
                
                int intSumCount1 = 0;
                for (int i = 0; i < intUnknownNum; i++)
                {
                    if (pCorrCptsLt[intSumCount1].FrCpt.isCtrl == false)
                    {
                        matl[2 * i,0] = XA[2 * i, 0] - X0[intSumCount1 * 2, 0];
                        matl[2 * i + 1,0] = XA[2 * i + 1, 0] - X0[intSumCount1 * 2 + 1, 0];
                    }
                    else
                    {
                        i -= 1;
                    }
                    intSumCount1 += 1;
                }

                //����ϵ������A��"intUnknownXY"�е�"intUnknownXY+intUnknownLength-1"�еĸ�Ԫ�أ����߶γ��ȶԸ�δ֪����ƫ����ֵ
                //�ȼ������ĸֵ��ע�⣺��ĸʵ��������ƫ�����һ����ֵ����ȴǡ�õ�������֮����룬�������㹫ʽ�������㹫ʽ��ͬ
                for (int i = 0; i < intPtNum-1; i++)
                {
                    adblSubDis[i] = Math.Pow((Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) * (Xmix[2 * i, 0] - Xmix[2 * i + 2, 0]) + (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]) * (Xmix[2 * i + 1, 0] - Xmix[2 * i + 3, 0]), 0.5);
                }
                //�����µļнǼ���λ��
                adblAzimuth[0] = CGeoFunc.CalAxisAngle(Xmix[0, 0], Xmix[1, 0], Xmix[2, 0], Xmix[3, 0]);
                for (int i = 1; i < intPtNum - 1; i++)
                {
                    adblAngle [i-1] = CGeoFunc.CalAngle_Counterclockwise(Xmix[i * 2 - 2, 0], Xmix[i * 2 - 1, 0], Xmix[i * 2, 0], Xmix[i * 2 + 1, 0], Xmix[i * 2 + 2, 0], Xmix[i * 2 + 3, 0]);
                    adblAzimuth[i] = adblAzimuth[i - 1] + adblAngle[i - 1] - Math.PI;
                }

                //��ʼ����ϵ�������"intUnknownXY"�е�"intUnknownXY+intUnknownLength-1"�еĸ�Ԫ��
                CalADevLength(pCorrCptsLt, intUnknownXY,intUnknownLength, ref A, ref matl, adblSubDis, adblAzimuth, adblLength0);                
         
                 //����ϵ������A��"intUnknownXY+intUnknownLength"�е�"intUnknownXY+intUnknownLength+intUnknownAngle"�еĸ�Ԫ�أ����ǶȶԸ�δ֪����ƫ����ֵ
                CalADevAngle(pCorrCptsLt, intUnknownXY + intUnknownLength, intUnknownAngle,Xmix , ref A, ref matl, adblSubDis,adblAngle , adblAngle0);                


                ////�����µĽ���ֵ
                //SaveFileDialog SFD = new SaveFileDialog();
                //SFD.ShowDialog();
                //     CHelpFuncExcel.ExportDataToExcelA(A, "maxA", SFD.FileName);
                //CHelpFuncExcel.ExportDataToExcelP(P, "maxP", SFD.FileName);
                //CHelpFuncExcel.ExportDataToExcel2(matl, "maxmatl", SFD.FileName);





                //VBMatrix Temp =A.Trans ()  * P * A;
                //Temp.InvertGaussJordan();
                //XA -= Temp * A.Transpose() * P * matl;

                //ƽ��
                VBMatrix x = InvAtPAAtPmatl(A, P, matl);
                XA -= x;

                //����Xmix
                int intSumCount4 = 0;
                for (int i = 0; i < intUnknownNum; i++)
                {
                    if (pCorrCptsLt[intSumCount4].FrCpt.isCtrl == false)
                    {
                        Xmix[intSumCount4 * 2, 0] = XA[i * 2, 0];
                        Xmix[intSumCount4 * 2 + 1, 0] = XA[i * 2 + 1, 0];
                   }
                    else
                    {
                        i -= 1;
                    }
                    intSumCount4 += 1;                 
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









        public void CalADevLength(List<CCorrCpts> pCorrCptsLt, int intBaseIndex, int intUnknownLength, ref VBMatrix A, ref VBMatrix matl, double[] dblSubDis, double[] adblAzimuth, double[] adblLength0)
        {
            //����ϵ�������й��ڳ���ֵ�ĵ������֣�ע�⣺�����ľ�����㹫ʽΪ��һ����������ǰһ��������꣩
            int intKnownCount = 0;
            int intUnKnownCount = 0;
            for (int i = 0; i < intUnknownLength; i++)
            {
                int intSumCount = intKnownCount + intUnKnownCount;
                if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == false)
                {
                    A[intBaseIndex + i, 2 * intUnKnownCount + 0] = -Math.Cos(adblAzimuth[intSumCount]);
                    A[intBaseIndex + i, 2 * intUnKnownCount + 1] = -Math.Sin(adblAzimuth[intSumCount]);
                    A[intBaseIndex + i, 2 * intUnKnownCount + 2] = -A[intBaseIndex + i, 2 * intUnKnownCount + 0];
                    A[intBaseIndex + i, 2 * intUnKnownCount + 3] = -A[intBaseIndex + i, 2 * intUnKnownCount + 1];

                    matl[intBaseIndex + i, 0] = adblLength0[intSumCount] - dblSubDis[intSumCount];   //ͼ���㣬˳�����matl

                    intUnKnownCount += 1;
                }
                else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true)
                {
                    A[intBaseIndex + i, 2 * intUnKnownCount + 0] = -Math.Cos(adblAzimuth[intSumCount]);
                    A[intBaseIndex + i, 2 * intUnKnownCount + 1] = -Math.Sin(adblAzimuth[intSumCount]);

                    matl[intBaseIndex + i, 0] = adblLength0[intSumCount] - dblSubDis[intSumCount];   //ͼ���㣬˳�����matl

                    intUnKnownCount += 1;
                }
                else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == false)
                {
                    //ע���������������"pCorrCptsLt[intSumCount].FrCpt.isCtrl == true"��ռλ�ӣ�����ռ�У�������������ȻΪ" 2 * intUnKnownCount + 0"��" 2 * intUnKnownCount + 1"��������+2,+3
                    A[intBaseIndex + i, 2 * intUnKnownCount + 0] = Math.Cos(adblAzimuth[intSumCount]);
                    A[intBaseIndex + i, 2 * intUnKnownCount + 1] = Math.Sin(adblAzimuth[intSumCount]);

                    matl[intBaseIndex + i, 0] = adblLength0[intSumCount] - dblSubDis[intSumCount];   //ͼ���㣬˳�����matl

                    intKnownCount += 1;
                }
                else
                {
                    intKnownCount += 1;
                    i -= 1;
                }
            }
        }

        public void CalADevAngle(List<CCorrCpts> pCorrCptsLt, int intBaseIndex, int intUnknownAngle, VBMatrix Xmix, ref VBMatrix A, ref VBMatrix matl, double[] adblSubDis, double[] adblAngle, double[] adblAngle0)
        {

            //����ϵ������A��"intUnknownXY+intUnknownLength"�е�"intUnknownXY+intUnknownLength+intUnknownAngle"�еĸ�Ԫ�أ����ǶȶԸ�δ֪����ƫ����ֵ
            int intKnownCount = 0;
            int intUnKnownCount = 0;
            for (int i = 0; i < intUnknownAngle; i++)
            {
                //����̫�����ˣ���Ȼ����������ʱ��н�ʱ��ֶ���������ۣ���������ĵ�����ʽȴ��һ�µģ���ʡ�˲��ٱ�̾�����������
                int intSumCount = intKnownCount + intUnKnownCount;

                //��������׼��
                double dblA2 = adblSubDis[intSumCount] * adblSubDis[intSumCount];
                double dblB2 = adblSubDis[intSumCount + 1] * adblSubDis[intSumCount + 1];

                //��ʼ����ϵ��ֵ�����ڽ������������������Ͻ��а����������˰����·�ʽ����
                if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 2].FrCpt.isCtrl == true)
                {
                    intKnownCount += 1;
                    i -= 1;
                }
                else
                {
                    matl[intBaseIndex + i, 0] = adblAngle0[intSumCount] - adblAngle[intSumCount];   //ͼ���㣬˳�����matl

                    int intPreTrueNum = 0;
                    int intUnKnownCountorginal = intUnKnownCount;
                    int intKnownCountorginal = intKnownCount;
                    if (pCorrCptsLt[intUnKnownCountorginal + intKnownCountorginal + 0].FrCpt.isCtrl == false)
                    {
                        //X1,Y1�ĵ���ֵ(ע�⣺�ò����Ǽ��������ֵΪ�����ĸ���)
                        A[intBaseIndex + i, 2 * intUnKnownCountorginal + 0] = -(Xmix[2 * intSumCount + 3, 0] - Xmix[2 * intSumCount + 1, 0]) / dblA2;
                        A[intBaseIndex + i, 2 * intUnKnownCountorginal + 1] = (Xmix[2 * intSumCount + 2, 0] - Xmix[2 * intSumCount + 0, 0]) / dblA2;

                        intUnKnownCount += 1;
                    }
                    else
                    {
                        intPreTrueNum += 1;
                        intKnownCount += 1;
                    }

                    if (pCorrCptsLt[intUnKnownCountorginal + intKnownCountorginal + 1].FrCpt.isCtrl == false)
                    {
                        //X2,Y2�ĵ���ֵ(ע�⣺��벿�����ɼ��������ĵ��������ֵΪ�����ĸ���)                       
                        A[intBaseIndex + i, 2 * (intUnKnownCountorginal - intPreTrueNum) + 2] = (Xmix[2 * intSumCount + 5, 0] - Xmix[2 * intSumCount + 3, 0]) / dblB2 + (Xmix[2 * intSumCount + 3, 0] - Xmix[2 * intSumCount + 1, 0]) / dblA2;
                        A[intBaseIndex + i, 2 * (intUnKnownCountorginal - intPreTrueNum) + 3] = -(Xmix[2 * intSumCount + 4, 0] - Xmix[2 * intSumCount + 2, 0]) / dblB2 - (Xmix[2 * intSumCount + 2, 0] - Xmix[2 * intSumCount + 0, 0]) / dblA2;
                    }
                    else
                    {
                        intPreTrueNum += 1;
                    }
                    if (pCorrCptsLt[intUnKnownCountorginal + intKnownCountorginal + 2].FrCpt.isCtrl == false)
                    {
                        //X3,Y3�ĵ���ֵ
                        A[intBaseIndex + i, 2 * (intUnKnownCountorginal - intPreTrueNum) + 4] = -(Xmix[2 * intSumCount + 5, 0] - Xmix[2 * intSumCount + 3, 0]) / dblB2;
                        A[intBaseIndex + i, 2 * (intUnKnownCountorginal - intPreTrueNum) + 5] = (Xmix[2 * intSumCount + 4, 0] - Xmix[2 * intSumCount + 2, 0]) / dblB2;
                    }
                }

            }


        }



        /// <summary>
        /// ����Inv(A'*P*A)*A'*P*matl��ʱ�临�Ӷȣ�2n��^2��nΪ�������
        /// </summary>
        /// <param name="A">ϵ�����󣺸þ���ӵ�0�е���i��Ϊ�ԽǾ���i��֮��Ϊȫ����</param>
        /// <param name="P">Ȩ�ؾ��󣺶ԽǾ���</param>
        /// <param name="matl">��ֵ����������</param>
        /// <returns>���ڱ�ʵ�����������ԣ�����ʹ�øú������м��㡣</returns>
        public VBMatrix InvAtPAAtPmatl(VBMatrix A, VBMatrix P, VBMatrix matl)
        {
            int intRowA = A.Row;
            int intColA = A.Col;
            int intDiffRowColA = intRowA - intColA;

            //����A'*P
            VBMatrix MatrixAtP = new VBMatrix(intColA, intRowA);
            for (int i = 0; i < intColA; i++)
            {
                //��i��i�е�ֵ
                MatrixAtP[i, i] = A[i, i] * P[i, i];
                //��i�����intDiffRowColA�е�ֵ
                for (int j = intColA; j < intRowA; j++)
                {
                    MatrixAtP[i, j] = A[j, i] * P[j, j];
                }
            }

            //����A'*P*A
            VBMatrix MatrixAtPA = new VBMatrix(intColA, intColA);
            for (int i = 0; i < intColA; i++)
            {
                //��i��i�е�ֵ(���Խ����ϵ�ֵ)
                double dblValue = MatrixAtP[i, i] * A[i, i];
                for (int j = intColA; j < intRowA; j++)
                {
                    dblValue += MatrixAtP[i, j] * A[j, i];
                }
                MatrixAtPA[i, i] = dblValue;

                //��i��j�к�j��i�е�ֵ(ע�⣺MatrixAtPA[i, j] == MatrixAtPA[j, i])
                for (int j = i + 1; j < intColA; j++)
                {
                    for (int k = 0; k < intDiffRowColA; k++)
                    {
                        MatrixAtPA[i, j] += MatrixAtP[i, intColA + k] * A[intColA + k, j];
                    }
                    MatrixAtPA[j, i] = MatrixAtPA[i, j];
                }
            }

            //����A'*P*matl
            VBMatrix MatrixAtPmatl = new VBMatrix(intColA, 1);
            for (int i = 0; i < intColA; i++)
            {
                double dblValue = MatrixAtP[i, i] * matl[i, 0];
                for (int j = intColA; j < intRowA; j++)
                {
                    dblValue += MatrixAtP[i, j] * matl[j, 0];
                }
                MatrixAtPmatl[i, 0] = dblValue;
            }

            //����Inv(A'*P*A)*A'*P*matl
            MatrixAtPA.Inv(MatrixAtPA);
            VBMatrix MatrixResult = MatrixAtPA * MatrixAtPmatl;

            return MatrixResult;
        }




        ///// <summary>���ԣ�������</summary>
        //public CParameterInitialize ParameterInitialize
        //{
        //    get { return _ParameterInitialize; }
        //    set { _ParameterInitialize = value; }
        //}

        ///// <summary>���ԣ�������</summary>
        //public CParameterResult ParameterResult
        //{
        //    get { return _ParameterResult; }
        //    set { _ParameterResult = value; }
        //}
    }
}
