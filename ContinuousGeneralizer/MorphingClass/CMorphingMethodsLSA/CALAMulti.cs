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
    /// 基于最小二乘原理的Morphing方法，以角度和边长为参数(Least Squares Alogrithm_Coordinate, Angle and Length)
    /// </summary>
    /// <remarks>顾及长度、角度、多线段间对应顶点间矢量方向，以坐标为平差参数进行平差，多结果间接平差</remarks>
    public class CALAMulti
    {
        private CDataRecords _DataRecords;                    //records of data
        private double _dblTX;
        
        

        public CALAMulti()
        {

        }

        public CALAMulti(CDataRecords pDataRecords, double dblTX)
        {
            _DataRecords = pDataRecords;
            _dblTX = dblTX;
        }

        public CALAMulti(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
            CPolyline FromCpl = pDataRecords.ParameterResult.FromCpl;
            _dblTX = FromCpl.pPolyline.Length / FromCpl.CptLt .Count  / 10000000000;   //计算阈值参数
        }


        /// <summary>
        /// 显示并返回单个插值面状要素
        /// </summary>
        /// <param name="intInterNum">Inter: Interpolation</param>
        /// <returns>面状要素</returns>
        public void ALAMultiMorphing()
        {
            List<CPolyline> cpllt = GetTargetcpllt();
            cpllt.Insert(0, _DataRecords.ParameterResult.FromCpl);
            cpllt.Add(_DataRecords.ParameterResult.ToCpl);
            _DataRecords.ParameterResult.CResultPlLt = cpllt;








            //// 清除绘画痕迹
            //IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            //IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            //pGra.DeleteAllElements();
            //CHelpFunc.ViewPolyline(m_mapControl, cpl);  //显示生成的线段
            //return cpl;
        }

        /// <summary>
        /// 获取线状要素
        /// </summary>
        /// <param name="intInterNum">Inter: Interpolation</param>
        /// <returns>在处理面状要素时，本程序将原面状要素的边界切开，按线状要素处理，处理完后再重新生成面状要素</returns>
        public List<CPolyline> GetTargetcpllt()
        {
            int intInterNum = Convert.ToInt16(_DataRecords.ParameterInitialize.txtInterpolationNum.Text);
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //读取数据后，此处ResultPtLt中的对应点为一一对应
            double dblTX = _dblTX;
            double dblInterval = 1 / Convert.ToDouble(intInterNum + 1);


            int intPtNum = pCorrCptsLt.Count;
            int intXYNum = intPtNum * 2;
            int intMultiXYNum = intInterNum * intXYNum;


            //计算长度初始值（全部计算）
            double[,] adblLength0 = new double[intInterNum, intPtNum - 1];
            double[,] adblLength = new double[intInterNum, intPtNum - 1];   //顺便定义中间值数组
            for (int j = 0; j < pCorrCptsLt.Count - 1; j++)
            {
                double dblfrsublength = CGeoFunc.CalDis(pCorrCptsLt[j + 1].FrCpt, pCorrCptsLt[j].FrCpt);
                double dbltosublength = CGeoFunc.CalDis(pCorrCptsLt[j + 1].ToCpt, pCorrCptsLt[j].ToCpt);
                for (int i = 0; i < intInterNum; i++)
                {
                    double dblProportion = (i+1) * dblInterval;
                    adblLength0[i, j] = (1 - dblProportion) * dblfrsublength + dblProportion * dbltosublength;
                }
                pCorrCptsLt[j].FrCpt.isCtrl = false;  //仅以最开始两对和最终两队对应点为固定点，故此先设置为false
            }

            //计算角度初始值（全部计算）
            double[,] adblAngle0 = new double[intInterNum, intPtNum - 2];
            for (int j = 0; j < pCorrCptsLt.Count - 2; j++)
            {
                //较大比例尺线状要素上的夹角
                double dblfrAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[j].FrCpt, pCorrCptsLt[j + 1].FrCpt, pCorrCptsLt[j + 2].FrCpt);
                //较小比例尺线状要素上的夹角
                double dbltoAngle = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[j].ToCpt, pCorrCptsLt[j + 1].ToCpt, pCorrCptsLt[j + 2].ToCpt);

                //角度初始值
                for (int i = 0; i < intInterNum; i++)
                {
                    double dblProportion = (i + 1) * dblInterval;
                    adblAngle0[i, j] = (1 - dblProportion) * dblfrAngle + dblProportion * dbltoAngle;
                }
            }

            //多线段间对应顶点间形成的夹角（全部计算），全部设置为pi
            //目标值也设置为0
            double[,] adblInterAngleDiff0 = new double[intInterNum, intPtNum];
            double[,] adblInterAngleDiff = new double[intInterNum, intPtNum];   //顺便定义中间值数组
            for (int j = 0; j < pCorrCptsLt.Count; j++)
            {
                //长度初始值
                for (int i = 0; i < intInterNum; i++)
                {
                    adblInterAngleDiff0[i, j] = 0;
                }
            }

            //计算坐标初始值，以及各线段方位角初始值
            //注意：默认固定第一条边
            pCorrCptsLt[0].FrCpt.isCtrl = true;
            pCorrCptsLt[1].FrCpt.isCtrl = true;
            VBMatrix X0 = new VBMatrix(intMultiXYNum, 1);
            double[,] adblAzimuth = new double[intInterNum, intPtNum - 1];
            for (int i = 0; i < intInterNum; i++)
            {
                double dblProportion = (i + 1) * dblInterval;
                double dblnewX0 = (1 - dblProportion) * pCorrCptsLt[0].FrCpt.X + dblProportion * pCorrCptsLt[0].ToCpt.X;
                double dblnewY0 = (1 - dblProportion) * pCorrCptsLt[0].FrCpt.Y + dblProportion * pCorrCptsLt[0].ToCpt.Y;
                double dblnewX1 = (1 - dblProportion) * pCorrCptsLt[1].FrCpt.X + dblProportion * pCorrCptsLt[1].ToCpt.X;
                double dblnewY1 = (1 - dblProportion) * pCorrCptsLt[1].FrCpt.Y + dblProportion * pCorrCptsLt[1].ToCpt.Y;
                adblAzimuth[i, 0] = CGeoFunc.CalAxisAngle(dblnewX0, dblnewY0, dblnewX1, dblnewY1);

                int intBasicIndex = i * intPtNum * 2;
                X0[intBasicIndex + 0, 0] = dblnewX0;
                X0[intBasicIndex + 1, 0] = dblnewY0;
                X0[intBasicIndex + 2, 0] = dblnewX1;
                X0[intBasicIndex + 3, 0] = dblnewY1;
            }

            //其它点
            //是否固定最后两个点
            pCorrCptsLt[pCorrCptsLt.Count - 2].FrCpt.isCtrl = true;
            pCorrCptsLt[pCorrCptsLt.Count - 1].FrCpt.isCtrl = true;

            for (int j = 2; j < intPtNum; j++)
            {

                if (pCorrCptsLt[j].FrCpt.isCtrl == false)
                {
                    for (int i = 0; i < intInterNum; i++)
                    {
                        int intBasicIndexI = i * intXYNum;
                        adblAzimuth[i, j - 1] = adblAzimuth[i, j - 2] + adblAngle0[i, j - 2] - Math.PI;
                        X0[intBasicIndexI + 2 * j + 0, 0] = X0[intBasicIndexI + 2 * (j - 1) + 0, 0] + adblLength0[i, j - 1] * Math.Cos(adblAzimuth[i, j - 1]);
                        X0[intBasicIndexI + 2 * j + 1, 0] = X0[intBasicIndexI + 2 * (j - 1) + 1, 0] + adblLength0[i, j - 1] * Math.Sin(adblAzimuth[i, j - 1]);
                    }
                }
                else
                {
                    for (int i = 0; i < intInterNum; i++)
                    {
                        int intBasicIndexIJ = i * intXYNum + 2 * j;
                        double dblProportion = (i + 1) * dblInterval;
                        X0[intBasicIndexIJ + 0, 0] = (1 - dblProportion) * pCorrCptsLt[j].FrCpt.X + dblProportion * pCorrCptsLt[j].ToCpt.X;
                        X0[intBasicIndexIJ + 1, 0] = (1 - dblProportion) * pCorrCptsLt[j].FrCpt.Y + dblProportion * pCorrCptsLt[j].ToCpt.Y;
                        double dblAngle = CGeoFunc.CalAngle_Counterclockwise(X0[intBasicIndexIJ - 4, 0], X0[intBasicIndexIJ - 3, 0], X0[intBasicIndexIJ - 2, 0], X0[intBasicIndexIJ - 1, 0], X0[intBasicIndexIJ - 0, 0], X0[intBasicIndexIJ + 1, 0]);  //计算实际夹角 
                        adblAzimuth[i, j - 1] = adblAzimuth[i, j - 2] + dblAngle - Math.PI;
                    }
                }
            }


            //统计插值点数
            int intKnownPt = 0;           //固定点的数目
            int intUnknownPt = 0;         //非固定点的数目

            List<int> intKnownLocationLt = new List<int>();  //记录已知点的序号
            //注意：对于该循环，有一个默认条件，即FromCpl的第一个顶点只有一个对应点
            for (int i = 0; i < intPtNum; i++)
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
            int intUnknownXY = intUnknownPt * 2;   //每个点都有X、Y坐标
            int intMultiUnknownXY = intInterNum * intUnknownXY;

            //找出长度固定的位置(如果一个线段的前后两个点都固定，则该长度固定)。另外，长度固定则该边的方位角也固定
            List<int> intKnownLengthLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 1; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1)
                {
                    intKnownLengthLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownLength = intPtNum - 1 - intKnownLengthLt.Count;

            //找出角度固定的位置(如果一个固定顶点的前后两个点都固定，则该角度固定)
            List<int> intKnownAngleLt = new List<int>();
            for (int i = 0; i < intKnownLocationLt.Count - 2; i++)
            {
                if ((intKnownLocationLt[i + 1] - intKnownLocationLt[i]) == 1 && (intKnownLocationLt[i + 2] - intKnownLocationLt[i + 1]) == 1)
                {
                    intKnownAngleLt.Add(intKnownLocationLt[i]);
                }
            }
            int intUnknownAngle = intPtNum - 2 - intKnownAngleLt.Count;

            //长度角度未知量
            int intUnknownLengthAngle = intUnknownLength + intUnknownAngle;

            //各未知量个数
            int intMultiUnknownLength = intInterNum * intUnknownLength;
            int intMultiUnknownAngle = intInterNum * intUnknownAngle;
            int intMultiUnknownLengthAngle = intMultiUnknownLength + intMultiUnknownAngle;
            int intMultiUnknownInterAngleDiff = intInterNum * intUnknownPt;
            int intSumConstraints = intMultiUnknownLength + intMultiUnknownAngle + intMultiUnknownInterAngleDiff;

            //定义权重矩阵***************************************************************************************定义权重矩阵************************************************************************************************定义权重矩阵//
            VBMatrix P = new VBMatrix(intSumConstraints, intSumConstraints);
            for (int i = 0; i < intMultiUnknownLength; i++)  //长度权重
            {
                P[i, i] = 1;
            }
            for (int i = 0; i < intMultiUnknownAngle; i++)   //角度权重
            {
                P[intMultiUnknownLength + i, intMultiUnknownLength + i] = 1;
            }
            for (int i = 0; i < intMultiUnknownInterAngleDiff; i++)   //角度权重
            {
                P[intMultiUnknownLengthAngle + i, intMultiUnknownLengthAngle + i] = 1;
            }



            //for (int i = 0; i < intInterNum; i++)
            //{
            //    int intBasicIndex2 = i * intUnknownLengthAngle;
            //    for (int j = 0; j < intUnknownLength; j++)
            //    {
            //        P[intBasicIndex2 + j, intBasicIndex2 + j] = 1;
            //    }
            //    for (int j = 0; j < intUnknownAngle; j++)
            //    {
            //        P[intBasicIndex2 + intUnknownLength + j, intBasicIndex2 + intUnknownLength + j] = 1;
            //    }
            //}

            //Xmix里存储了XA和X0的最新混合值（此矩阵在公式推导中并不存在，只是为了方便编写代码而建立）
            VBMatrix Xmix = new VBMatrix(intMultiXYNum, 1);
            for (int i = 0; i < intMultiXYNum; i++)
            {
                Xmix[i, 0] = X0[i, 0];
            }

            //定义坐标近似值矩阵XA
            VBMatrix XA = new VBMatrix(intMultiUnknownXY, 1);
            int intSumCount0 = 0;
            for (int j = 0; j < intUnknownPt; j++)
            {
                if (pCorrCptsLt[intSumCount0].FrCpt.isCtrl == false)
                {
                    for (int i = 0; i < intInterNum; i++)
                    {
                        XA[i * intUnknownXY + j * 2 + 0, 0] = X0[i * intXYNum + intSumCount0 * 2 + 0, 0];
                        XA[i * intUnknownXY + j * 2 + 1, 0] = X0[i * intXYNum + intSumCount0 * 2 + 1, 0];
                    }
                }
                else
                {
                    j -= 1;
                }
                intSumCount0 += 1;
            }

            //定义系数矩阵A(各方程对坐标的导数值)，A的导数值将在循环中给出
            VBMatrix A = new VBMatrix(intSumConstraints, intMultiUnknownXY);
            double dblJudge1 = 0;   //该值用于判断是否应该跳出循环
            double dblJudge2 = 0;   //该值用于判断是否应该跳出循环
            double dblJudge3 = 0;   //该值用于判断是否应该跳出循环
            int intJudgeIndex = intMultiUnknownXY / 4;
            int intIterativeCount = 0;

            for (int k = 0; k < 2; k++)
            {
                //break;
                do
                {

                    VBMatrix matl = new VBMatrix(intSumConstraints, 1);


                    //计算系数矩阵A第0行到"intUnknownLength"行的各元素，即线段长度对各未知数求偏导的值
                    //先计算各分母值（注意：分母实际上是求偏导后的一部分值，但却恰好等于两点之间距离，因此其计算公式与距离计算公式相同
                    for (int i = 0; i < intInterNum; i++)
                    {
                        int intBasicIndexS1 = i * intXYNum;
                        for (int j = 0; j < intPtNum - 1; j++)
                        {
                            int intBasicIndexIJS1 = intBasicIndexS1 + 2 * j;
                            adblLength[i, j] = Math.Pow((Xmix[intBasicIndexIJS1 + 0, 0] - Xmix[intBasicIndexIJS1 + 2, 0]) * (Xmix[intBasicIndexIJS1 + 0, 0] - Xmix[intBasicIndexIJS1 + 2, 0]) +
                                                        (Xmix[intBasicIndexIJS1 + 1, 0] - Xmix[intBasicIndexIJS1 + 3, 0]) * (Xmix[intBasicIndexIJS1 + 1, 0] - Xmix[intBasicIndexIJS1 + 3, 0]), 0.5);
                        }
                    }

                    //计算新的方位角
                    for (int i = 0; i < intInterNum; i++)
                    {
                        int intBasicIndexA1 = i * intXYNum;
                        //第一条线段的方位角
                        adblAzimuth[i, 0] = CGeoFunc.CalAxisAngle(Xmix[intBasicIndexA1 + 0, 0], Xmix[intBasicIndexA1 + 1, 0], Xmix[intBasicIndexA1 + 2, 0], Xmix[intBasicIndexA1 + 3, 0]);
                        //后面线段的方位角
                        for (int j = 1; j < intPtNum - 1; j++)
                        {
                            int intBasicIndexIJA1 = intBasicIndexA1 + 2 * j;
                            double dblAngle = CGeoFunc.CalAngle_Counterclockwise(Xmix[intBasicIndexIJA1 - 2, 0], Xmix[intBasicIndexIJA1 - 1, 0],
                                                                        Xmix[intBasicIndexIJA1 + 0, 0], Xmix[intBasicIndexIJA1 + 1, 0],
                                                                        Xmix[intBasicIndexIJA1 + 2, 0], Xmix[intBasicIndexIJA1 + 3, 0]);
                            adblAzimuth[i, j] = adblAzimuth[i, j - 1] + dblAngle - Math.PI;
                        }
                    }

                    //多线段间对应顶点距离初始值（全部计算，有重复计算）
                    double[,] adblIntervalDis = new double[intInterNum + 1, intPtNum];  //多线段间对应顶点距离
                    double[,] adbl2IntervalDis = new double[intInterNum, intPtNum];     //多线段间对应顶点距离（隔点）
                    for (int j = 0; j < pCorrCptsLt.Count; j++)
                    {
                        int int2J = 2 * j;
                        //多线段间对应顶点距离
                        //源线段与第一生成线段间对应点距离
                        adblIntervalDis[0, j] = Math.Pow((pCorrCptsLt[j].FrCpt.X - Xmix[int2J + 0, 0]) * (pCorrCptsLt[j].FrCpt.X - Xmix[int2J + 0, 0]) +
                                                         (pCorrCptsLt[j].FrCpt.Y - Xmix[int2J + 1, 0]) * (pCorrCptsLt[j].FrCpt.Y - Xmix[int2J + 1, 0]), 0.5);
                        //目标线段与最后生成线段间对应点距离
                        adblIntervalDis[intInterNum, j] = Math.Pow((pCorrCptsLt[j].ToCpt.X - Xmix[(intInterNum - 1) * intXYNum + int2J + 0, 0]) * (pCorrCptsLt[j].ToCpt.X - Xmix[(intInterNum - 1) * intXYNum + int2J + 0, 0]) +
                                                                   (pCorrCptsLt[j].ToCpt.Y - Xmix[(intInterNum - 1) * intXYNum + int2J + 1, 0]) * (pCorrCptsLt[j].ToCpt.Y - Xmix[(intInterNum - 1) * intXYNum + int2J + 1, 0]), 0.5);
                        //各生成线段间对应点距离
                        for (int i = 1; i < intInterNum; i++)
                        {
                            adblIntervalDis[i, j] = Math.Pow((Xmix[(i - 1) * intXYNum + int2J + 0, 0] - Xmix[i * intXYNum + int2J + 0, 0]) * (Xmix[(i - 1) * intXYNum + int2J + 0, 0] - Xmix[i * intXYNum + int2J + 0, 0]) +
                                                             (Xmix[(i - 1) * intXYNum + int2J + 1, 0] - Xmix[i * intXYNum + int2J + 1, 0]) * (Xmix[(i - 1) * intXYNum + int2J + 1, 0] - Xmix[i * intXYNum + int2J + 1, 0]), 0.5);
                        }


                        //多线段间对应顶点距离（隔点）
                        adbl2IntervalDis[0, j] = Math.Pow((pCorrCptsLt[j].FrCpt.X - Xmix[intXYNum + int2J + 0, 0]) * (pCorrCptsLt[j].FrCpt.X - Xmix[intXYNum + int2J + 0, 0]) +
                                                          (pCorrCptsLt[j].FrCpt.Y - Xmix[intXYNum + int2J + 1, 0]) * (pCorrCptsLt[j].FrCpt.Y - Xmix[intXYNum + int2J + 1, 0]), 0.5);
                        //目标线段与最后生成线段间对应点距离
                        adbl2IntervalDis[intInterNum - 1, j] = Math.Pow((pCorrCptsLt[j].ToCpt.X - Xmix[(intInterNum - 2) * intXYNum + int2J + 0, 0]) * (pCorrCptsLt[j].ToCpt.X - Xmix[(intInterNum - 2) * intXYNum + int2J + 0, 0]) +
                                                                        (pCorrCptsLt[j].ToCpt.Y - Xmix[(intInterNum - 2) * intXYNum + int2J + 1, 0]) * (pCorrCptsLt[j].ToCpt.Y - Xmix[(intInterNum - 2) * intXYNum + int2J + 1, 0]), 0.5);
                        //各生成线段间对应点距离
                        for (int i = 1; i < intInterNum - 1; i++)
                        {
                            adbl2IntervalDis[i, j] = Math.Pow((Xmix[(i - 1) * intXYNum + int2J + 0, 0] - Xmix[(i + 1) * intXYNum + int2J + 0, 0]) * (Xmix[(i - 1) * intXYNum + int2J + 0, 0] - Xmix[(i + 1) * intXYNum + int2J + 0, 0]) +
                                                              (Xmix[(i - 1) * intXYNum + int2J + 1, 0] - Xmix[(i + 1) * intXYNum + int2J + 1, 0]) * (Xmix[(i - 1) * intXYNum + int2J + 1, 0] - Xmix[(i + 1) * intXYNum + int2J + 1, 0]), 0.5);
                        }
                    }

                    //计算系数矩阵中关于长度值的导数部分（注意：隐含的距离计算公式为后一个点的坐标减前一个点的坐标）
                    int intKnownCount2 = 0;
                    int intUnKnownCount2 = 0;
                    for (int j = 0; j < intUnknownLength; j++)
                    {
                        int intSumCount = intKnownCount2 + intUnKnownCount2;
                        int intBasicIndexL2 = 2 * intUnKnownCount2;
                        if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == false)
                        {
                            for (int i = 0; i < intInterNum; i++)
                            {
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 0] = -Math.Cos(adblAzimuth[i, intSumCount]);
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 1] = -Math.Sin(adblAzimuth[i, intSumCount]);
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 2] = -A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 0];
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 3] = -A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 1];

                                matl[i * intUnknownLength + j, 0] = adblLength[i, intSumCount] - adblLength0[i, intSumCount];   //图方便，顺便计算matl                            
                            }
                            intUnKnownCount2 += 1;
                        }
                        else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == false && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == true)
                        {
                            for (int i = 0; i < intInterNum; i++)
                            {
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 0] = -Math.Cos(adblAzimuth[i, intSumCount]);
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 1] = -Math.Sin(adblAzimuth[i, intSumCount]);

                                matl[i * intUnknownLength + j, 0] = adblLength[i, intSumCount] - adblLength0[i, intSumCount];   //图方便，顺便计算matl                            
                            }
                            intUnKnownCount2 += 1;
                        }
                        else if (pCorrCptsLt[intSumCount].FrCpt.isCtrl == true && pCorrCptsLt[intSumCount + 1].FrCpt.isCtrl == false)
                        {
                            for (int i = 0; i < intInterNum; i++)
                            {
                                //注意这种情况，由于"pCorrCptsLt[intSumCount].FrCpt.isCtrl == true"不占位子（即不占列），因此列序号依然为" 2 * intUnKnownCount + 0"和" 2 * intUnKnownCount + 1"，而不是+2,+3
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 0] = Math.Cos(adblAzimuth[i, intSumCount]);
                                A[i * intUnknownLength + j, i * intUnknownXY + intBasicIndexL2 + 1] = Math.Sin(adblAzimuth[i, intSumCount]);

                                matl[i * intUnknownLength + j, 0] = adblLength[i, intSumCount] - adblLength0[i, intSumCount];   //图方便，顺便计算matl                            
                            }
                            intKnownCount2 += 1;
                        }
                        else
                        {
                            intKnownCount2 += 1;
                            j -= 1;
                        }
                    }

                    //计算系数矩阵中关于夹角值的导数部分
                    int intKnownCount3 = 0;
                    int intUnKnownCount3 = 0;
                    for (int j = 0; j < intUnknownAngle; j++)
                    {
                        //真是太幸运了，虽然求两向量逆时针夹角时需分多种情况讨论，但各情况的导数形式却是一致的，节省了不少编程精力啊，哈哈
                        int intSumCount = intKnownCount3 + intUnKnownCount3;


                        //常用数据准备
                        double[] adblA2 = new double[intInterNum];
                        double[] adblB2 = new double[intInterNum];
                        for (int i = 0; i < intInterNum; i++)
                        {
                            adblA2[i] = adblLength[i, intSumCount + 0] * adblLength[i, intSumCount + 0];
                            adblB2[i] = adblLength[i, intSumCount + 1] * adblLength[i, intSumCount + 1];
                        }

                        //开始计算系数值，由于将以下三个情况排列组合将有八种情况，因此按如下方式计算
                        if (pCorrCptsLt[intUnKnownCount3 + intKnownCount3].FrCpt.isCtrl == true && pCorrCptsLt[intUnKnownCount3 + intKnownCount3 + 1].FrCpt.isCtrl == true && pCorrCptsLt[intUnKnownCount3 + intKnownCount3 + 2].FrCpt.isCtrl == true)
                        {
                            intKnownCount3 += 1;
                            j -= 1;
                        }
                        else
                        {
                            for (int i = 0; i < intInterNum; i++)
                            {
                                int intBasicIndexFi3 = i * intXYNum + 2 * intSumCount;
                                double dblNewAngle = CGeoFunc.CalAngle_Counterclockwise(Xmix[intBasicIndexFi3 + 0, 0], Xmix[intBasicIndexFi3 + 1, 0],
                                                                               Xmix[intBasicIndexFi3 + 2, 0], Xmix[intBasicIndexFi3 + 3, 0],
                                                                               Xmix[intBasicIndexFi3 + 4, 0], Xmix[intBasicIndexFi3 + 5, 0]);

                                matl[intMultiUnknownLength + i * intUnknownAngle + j, 0] = dblNewAngle - adblAngle0[i, intSumCount];      //图方便，顺便计算matl
                            }

                            int intPreTrueNum = 0;
                            int intUnKnownCount3orginal = intUnKnownCount3;
                            int intKnownCount3orginal = intKnownCount3;
                            if (pCorrCptsLt[intUnKnownCount3orginal + intKnownCount3orginal + 0].FrCpt.isCtrl == false)
                            {
                                //X1,Y1的导数值(注意：该部分是减数，因此值为导数的负数)
                                for (int i = 0; i < intInterNum; i++)
                                {
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * intUnKnownCount3orginal + 0] = -(Xmix[i * intXYNum + 2 * intSumCount + 3, 0] - Xmix[i * intXYNum + 2 * intSumCount + 1, 0]) / adblA2[i];
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * intUnKnownCount3orginal + 1] = (Xmix[i * intXYNum + 2 * intSumCount + 2, 0] - Xmix[i * intXYNum + 2 * intSumCount + 0, 0]) / adblA2[i];
                                }

                                intUnKnownCount3 += 1;
                            }
                            else
                            {
                                intPreTrueNum += 1;
                                intKnownCount3 += 1;
                            }

                            if (pCorrCptsLt[intUnKnownCount3orginal + intKnownCount3orginal + 1].FrCpt.isCtrl == false)
                            {
                                //X2,Y2的导数值
                                for (int i = 0; i < intInterNum; i++)
                                {
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * (intUnKnownCount3orginal - intPreTrueNum) + 2] = (Xmix[i * intXYNum + 2 * intSumCount + 5, 0] - Xmix[i * intXYNum + 2 * intSumCount + 3, 0]) / adblB2[i]
                                                                                                                                                             + (Xmix[i * intXYNum + 2 * intSumCount + 3, 0] - Xmix[i * intXYNum + 2 * intSumCount + 1, 0]) / adblA2[i];
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * (intUnKnownCount3orginal - intPreTrueNum) + 3] = -(Xmix[i * intXYNum + 2 * intSumCount + 4, 0] - Xmix[i * intXYNum + 2 * intSumCount + 2, 0]) / adblB2[i]
                                                                                                                                                              - (Xmix[i * intXYNum + 2 * intSumCount + 2, 0] - Xmix[i * intXYNum + 2 * intSumCount + 0, 0]) / adblA2[i];
                                }
                            }
                            else
                            {
                                intPreTrueNum += 1;
                            }
                            if (pCorrCptsLt[intUnKnownCount3orginal + intKnownCount3orginal + 2].FrCpt.isCtrl == false)
                            {
                                //X3,Y3的导数值
                                for (int i = 0; i < intInterNum; i++)
                                {
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * (intUnKnownCount3orginal - intPreTrueNum) + 4] = -(Xmix[i * intXYNum + 2 * intSumCount + 5, 0] - Xmix[i * intXYNum + 2 * intSumCount + 3, 0]) / adblB2[i];
                                    A[intMultiUnknownLength + i * intUnknownAngle + j, i * intUnknownXY + 2 * (intUnKnownCount3orginal - intPreTrueNum) + 5] = (Xmix[i * intXYNum + 2 * intSumCount + 4, 0] - Xmix[i * intXYNum + 2 * intSumCount + 2, 0]) / adblB2[i];
                                }
                            }
                        }
                    }


                    //计算系数矩阵中关于多线段间对应顶点距离的导数部分（注意：隐含的距离计算公式为后一个点的坐标减前一个点的坐标）
                    int intKnownCount4 = 0;
                    int intUnKnownCount4 = 0;
                    for (int j = 0; j < intUnknownPt; j++)
                    {
                        int intSumCount4 = intKnownCount4 + intUnKnownCount4;
                        int intBasicIndexL4 = 2 * intUnKnownCount4;
                        if (pCorrCptsLt[intSumCount4].FrCpt.isCtrl == false)
                        {
                            int l=0;
                            //图方便，顺便计算matl
                            //第一行
                            double dblNewAnglek1 = CGeoFunc.CalAngle_Counterclockwise(pCorrCptsLt[intSumCount4].FrCpt.X - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                             pCorrCptsLt[intSumCount4].FrCpt.Y - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                             Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 0, 0] - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                             Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 1, 0] - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0]);
                            matl[intMultiUnknownLengthAngle + l * intUnknownPt + j, 0] = dblNewAnglek1 - adblInterAngleDiff0[k, intSumCount4];

                            //最后一行
                            l = intInterNum - 1;
                            double dblNewAnglek2 = CGeoFunc.CalAngle_Counterclockwise(Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 0, 0] - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                             Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 1, 0] - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                             pCorrCptsLt[intSumCount4].ToCpt.X - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                             pCorrCptsLt[intSumCount4].ToCpt.X - Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0]);
                            matl[intMultiUnknownLengthAngle + l * intUnknownPt + j, 0] = dblNewAnglek2 - adblInterAngleDiff0[k, intSumCount4];      //图方便，顺便计算matl
                            
                            //其它行
                            for (int i = 1; i < intInterNum-1; i++)
                            {
                                double dblNewAngle = CGeoFunc.CalAngle_Counterclockwise(Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 0, 0] - Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                               Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 1, 0] - Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                               Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 0, 0] - Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                               Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 1, 0] - Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0]);

                                matl[intMultiUnknownLengthAngle + i * intUnknownPt + j, 0] = dblNewAngle - adblInterAngleDiff0[i, intSumCount4];      //图方便，顺便计算matl
                            }





                            ////源线段、第一生成线段、第二生成线段间对应点向量夹角
                            //A[intMultiUnknownLengthAngle + j, intBasicIndexL4 + 0] = CGeoFunc.DerArctan(Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], Xmix[2 * intSumCount4 + intXYNum, 0], Xmix[2 * intSumCount4 + intXYNum + 1, 0], adblIntervalDis[0, intSumCount4], "x1")
                            //                                                       - CGeoFunc.DerArctan(pCorrCptsLt[intSumCount4].FrCpt.X, pCorrCptsLt[intSumCount4].FrCpt.Y, Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], adblIntervalDis[0, intSumCount4], "x2");
                            //A[intMultiUnknownLengthAngle + j, intBasicIndexL4 + 1] = CGeoFunc.DerArctan(Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], Xmix[2 * intSumCount4 + intXYNum, 0], Xmix[2 * intSumCount4 + intXYNum + 1, 0], adblIntervalDis[0, intSumCount4], "y1")
                            //                                                       - CGeoFunc.DerArctan(pCorrCptsLt[intSumCount4].FrCpt.X, pCorrCptsLt[intSumCount4].FrCpt.Y, Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], adblIntervalDis[0, intSumCount4], "y2");
                            //A[intMultiUnknownLengthAngle + j,intUnknownXY+ intBasicIndexL4 + 0] = CGeoFunc.DerArctan(Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], Xmix[2 * intSumCount4 + intXYNum, 0], Xmix[2 * intSumCount4 + intXYNum + 1, 0], adblIntervalDis[0, intSumCount4], "x2");
                            //A[intMultiUnknownLengthAngle + j, intUnknownXY + intBasicIndexL4 + 1] = CGeoFunc.DerArctan(Xmix[2 * intSumCount4, 0], Xmix[2 * intSumCount4 + 1, 0], Xmix[2 * intSumCount4 + intXYNum, 0], Xmix[2 * intSumCount4 + intXYNum + 1, 0], adblIntervalDis[0, intSumCount4], "y2");

                            //正式计算导数
                            //源线段、第一生成线段、第二生成线段间对应点向量夹角的导数
                            //第二个点
                            l = 0;
                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 0) * intUnknownXY + intBasicIndexL4 + 0] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 1, intSumCount4], "x1")
                                                                                                                            - CGeoFunc.DerArctan(pCorrCptsLt[intSumCount4].FrCpt.X,
                                                                                                                                                        pCorrCptsLt[intSumCount4].FrCpt.Y,
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "x2");

                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 0) * intUnknownXY + intBasicIndexL4 + 1] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 1, intSumCount4], "y1")
                                                                                                                            - CGeoFunc.DerArctan(pCorrCptsLt[intSumCount4].FrCpt.X,
                                                                                                                                                        pCorrCptsLt[intSumCount4].FrCpt.Y,
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "y2");
                           
                            //第三个点
                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l + 1) * intUnknownXY + intBasicIndexL4 + 0] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 1, intSumCount4], "x2");

                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l + 1) * intUnknownXY + intBasicIndexL4 + 1] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 1, intSumCount4], "y2");


                            //第一生成线段、第二生成线段、目标线段间对应点向量夹角的导数
                            //第一个点
                            l = intInterNum - 1;
                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 1) * intUnknownXY + intBasicIndexL4 + 0] = -CGeoFunc.DerArctan(Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "x1");

                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 1) * intUnknownXY + intBasicIndexL4 + 1] = -CGeoFunc.DerArctan(Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "y1");

                            //第二个点
                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 0) * intUnknownXY + intBasicIndexL4 + 0] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        pCorrCptsLt[intSumCount4].ToCpt.X,
                                                                                                                                                        pCorrCptsLt[intSumCount4].ToCpt.Y,          adblIntervalDis[l + 1, intSumCount4], "x1")
                                                                                                                            - CGeoFunc.DerArctan(Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "x2");

                            A[intMultiUnknownLengthAngle + l * intUnknownPt + j, (l - 0) * intUnknownXY + intBasicIndexL4 + 1] = +CGeoFunc.DerArctan(Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        pCorrCptsLt[intSumCount4].ToCpt.X,
                                                                                                                                                        pCorrCptsLt[intSumCount4].ToCpt.Y,          adblIntervalDis[l + 1, intSumCount4], "x1")
                                                                                                                            - CGeoFunc.DerArctan(Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                        Xmix[(l - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[l + 0, intSumCount4], "y2");
                            
                            //各生成线段间对应点距离导数
                            for (int i = 1; i < intInterNum -1; i++)
                            {
                                //第一个点
                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i - 1) * intUnknownXY + intBasicIndexL4 + 0] = -CGeoFunc.DerArctan(Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 0, intSumCount4], "x1");

                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i - 1) * intUnknownXY + intBasicIndexL4 + 1] = -CGeoFunc.DerArctan(Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 0, intSumCount4], "y1");

                                //第二个点
                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i - 0) * intUnknownXY + intBasicIndexL4 + 0] = +CGeoFunc.DerArctan(Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 1, intSumCount4], "x1")
                                                                                                                                - CGeoFunc.DerArctan(Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 0, intSumCount4], "x2");

                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i - 0) * intUnknownXY + intBasicIndexL4 + 1] = +CGeoFunc.DerArctan(Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 1, intSumCount4], "y1")
                                                                                                                                - CGeoFunc.DerArctan(Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 1) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 0, intSumCount4], "y2");

                                //第三个点
                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i + 1) * intUnknownXY + intBasicIndexL4 + 0] = +CGeoFunc.DerArctan(Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 1, intSumCount4], "x2");
                                A[intMultiUnknownLengthAngle + i * intUnknownPt + j, (i + 1) * intUnknownXY + intBasicIndexL4 + 1] = +CGeoFunc.DerArctan(Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i - 0) * intXYNum + 2 * intSumCount4 + 1, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 0, 0],
                                                                                                                                                            Xmix[(i + 1) * intXYNum + 2 * intSumCount4 + 1, 0], adblIntervalDis[i + 1, intSumCount4], "y2");
                            }

                            intUnKnownCount4 += 1;
                        }
                        else
                        {
                            intKnownCount4 += 1;
                            j -= 1;
                        }
                    }



                    int tt = 5;

                    //CHelpFuncExcel.ExportDataToExcel2(A, "maxA", _DataRecords.ParameterInitialize.strSavePath);
                    //CHelpFuncExcel.ExportDataToExcelP(P, "maxP", _DataRecords.ParameterInitialize.strSavePath);
                    //CHelpFuncExcel.ExportDataToExcel2(matl, "maxmatl", _DataRecords.ParameterInitialize.strSavePath);


                    //平差
                    VBMatrix Temp = A.Trans() * P * A;
                    VBMatrix InvTemp = Temp.Inv(Temp);
                    VBMatrix x = InvTemp * A.Trans() * P * matl;

                    //CHelpFuncExcel.ExportDataToExcel2(x, "maxX", _DataRecords.ParameterInitialize.strSavePath);
                    //CHelpFuncExcel.ExportDataToExcel2(XA, "maxXA", _DataRecords.ParameterInitialize.strSavePath);

                    XA -= x;

                    VBMatrix Negtivex = new VBMatrix(x.Row, x.Col);
                    Negtivex = Negtivex - x;

                    //更新Xmix
                    int intSumCount5 = 0;
                    for (int j = 0; j < intUnknownPt; j++)
                    {
                        if (pCorrCptsLt[intSumCount5].FrCpt.isCtrl == false)
                        {
                            for (int i = 0; i < intInterNum; i++)
                            {
                                Xmix[i * intXYNum + intSumCount5 * 2 + 0, 0] = XA[i * intUnknownXY + j * 2 + 0, 0];
                                Xmix[i * intXYNum + intSumCount5 * 2 + 1, 0] = XA[i * intUnknownXY + j * 2 + 1, 0];
                            }
                        }
                        else
                        {
                            j -= 1;
                        }
                        intSumCount5 += 1;
                    }


                    intIterativeCount += 1;
                    if (intIterativeCount >= 50)
                    {
                        break;
                    }

                    //这里只是随便取两个中间值以观测是否收敛
                    dblJudge1 = Math.Abs(x[1 * intJudgeIndex, 0]);
                    dblJudge2 = Math.Abs(x[2 * intJudgeIndex, 0]);
                    dblJudge3 = Math.Abs(x[3 * intJudgeIndex, 0]);

                } while ((dblJudge1 > dblTX) || (dblJudge2 > dblTX) || (dblJudge3 > dblTX));

                break;








                for (int i = 0; i < intMultiUnknownLength; i++)  //长度权重
                {
                    P[i, i] = 1;
                }
                for (int i = 0; i < intMultiUnknownAngle; i++)   //角度权重
                {
                    P[intMultiUnknownLength + i, intMultiUnknownLength + i] = 39.48;
                }
                for (int i = 0; i < intMultiUnknownInterAngleDiff; i++)   //角度权重
                {
                    P[intMultiUnknownLengthAngle + i, intMultiUnknownLengthAngle + i] = 1;
                }
            }
           

            //生成目标线段
            List<CPolyline> cpllt = new List<CPolyline>();
            for (int i = 0; i < intInterNum; i++)
            {
                List<CPoint> cptlt = new List<CPoint>();
                for (int j = 0; j < intPtNum; j++)
                {
                    CPoint cpt = new CPoint(j, Xmix[i * intXYNum + j * 2, 0], Xmix[i * intXYNum + j * 2 + 1, 0]);
                    cptlt.Add(cpt);
                }
                CPolyline cpl = new CPolyline(i, cptlt);
                cpllt.Add(cpl);
            }

            return cpllt;
        }


    }
}
