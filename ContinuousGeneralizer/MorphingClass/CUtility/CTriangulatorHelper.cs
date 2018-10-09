using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MorphingClass.CUtility;
using MorphingClass.CGeometry;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;


namespace MorphingClass.CUtility
{
    //public class CTriangulatorHelper
    //{
    //    /// <summary>
    //    /// ͳ��������Ϣ������ÿ��������ƽ����Ⱥ�������
    //    /// </summary>
    //    /// <param name="CTriangleLt">�������</param>
    //    /// <param name="CHiberarchyBend">����νṹ������</param> 
    //    /// <remarks>���������Ĳ�νṹɭ�ֹ�����ͬʱ��Ҫ�����������
    //    /// 1��ͳ���������ߵ�����ƽ����ȣ�ÿ�������ײ�����������������������ε�������ƽ��ֵ��
    //    /// 2��ͳ��ÿ������������ÿ��������������ȣ����Ӹ�������ʼ�������ײ��������������ε�������
    //    /// </remarks>
    //    public double StaticBendInfo(ref CBendForest FromBendForest, ref CBendForest ToBendForest)
    //    {
    //        //FromTriangleLt[i].a
    //        //����ÿ��������ƽ����Ⱥ�������
    //        FromBendForest.dblDeepBendDepthLt = new List<double>();
    //        for (int i = 0; i < FromBendForest.Count; i++)
    //            CalBendDepth(FromBendForest.ElementAt(i).Value);

    //        //����ÿ��������ƽ����Ⱥ�������
    //        ToBendForest.dblDeepBendDepthLt = new List<double>();
    //        for (int i = 0; i < ToBendForest.Count; i++)
    //            CalBendDepth(ToBendForest.ElementAt(i).Value);
            
    //        double dblDepthSumRatio = CalBendForestDepthSumRatio(FromBendForest, ToBendForest);
    //        return dblDepthSumRatio;
    //    }
















    //    /// <summary>
    //    /// ����ÿ�����������
    //    /// </summary>
    //    /// <param name="Bend">��ǰ����������</param>
    //    /// <remarks>��Ҷ�ӽ�㿪ʼ���㣨������ײ��������ʼ��������Ϊʵ����Ҫ��ͬʱ���������Ⱥ�ƽ�����</remarks>
    //    private void CalBendDepth(CBend Bend)
    //    {
    //        double dblCurrentDepth = 0;
    //        for (int i = 0; i < Bend.CTriangleLt.Count; i++)
    //        {
    //            dblCurrentDepth = dblCurrentDepth + Bend.CTriangleLt[i].Area;
    //        }

    //        if (Bend.CLeftBend != null)
    //        {
    //            CalBendDepth(Bend.CLeftBend);
    //            CalBendDepth(Bend.CRightBend);
    //        }
    //        else
    //        {                
    //            Bend.intPathCount = 1;
    //            Bend.dblBendDepthAverage = dblCurrentDepth;
    //            Bend.dblBendDepthMax = dblCurrentDepth;
    //            Bend.dblBendDepthSum = dblCurrentDepth;
    //            return;
    //        }

    //        //���������������
    //        if (Bend.CLeftBend.dblBendDepthMax >= Bend.CRightBend.dblBendDepthMax)
    //            Bend.dblBendDepthMax = Bend.CLeftBend.dblBendDepthMax + dblCurrentDepth;
    //        else
    //            Bend.dblBendDepthMax = Bend.CRightBend.dblBendDepthMax + dblCurrentDepth;

    //        //���������ƽ�����
    //        int intPathCountSum = Bend.CLeftBend.intPathCount + Bend.CRightBend.intPathCount;
    //        Bend.intPathCount = intPathCountSum;
    //        Bend.dblBendDepthAverage = dblCurrentDepth + (Bend.CLeftBend.dblBendDepthAverage * Bend.CLeftBend.intPathCount +
    //                                                      Bend.CRightBend.dblBendDepthAverage * Bend.CRightBend.intPathCount) / intPathCountSum;

    //        //���������������
    //        Bend.dblBendDepthSum =dblCurrentDepth + Bend.CLeftBend.dblBendDepthSum + Bend.CRightBend.dblBendDepthSum;
    //    }



    //    /// <summary>
    //    /// �������ߵ����
    //    /// </summary>
    //    /// <param name="pBendForest">����ĳ�������ɵ�����ɭ��</param>
    //    /// <remarks></remarks>
    //    private void CalBendForestDepth(ref CBendForest pBendForest)
    //    {
    //        int intPathCount = 0;
    //        double dblBendDepthSum = 0;
    //        double dblBendDepthMax = 0;
    //        for (int i = 0; i < pBendForest.Count; i++)
    //        {
    //            intPathCount = intPathCount + pBendForest.ElementAt(i).Value.intPathCount;
    //            dblBendDepthSum = dblBendDepthSum + pBendForest.ElementAt(i).Value.dblBendDepthAverage * pBendForest.ElementAt(i).Value.intPathCount;
    //            if (dblBendDepthMax < pBendForest.ElementAt(i).Value.dblBendDepthMax)
    //                dblBendDepthMax = pBendForest.ElementAt(i).Value.dblBendDepthMax;

    //        }
    //        pBendForest.dblDepthAverage = dblBendDepthSum / intPathCount;
    //        pBendForest.dblDepthMax = dblBendDepthMax;
    //        pBendForest.intPathCount = intPathCount;
    //    }


    //    /// <summary>
    //    /// ������������ɭ�ֵ�Э����׼��
    //    /// </summary>
    //    /// <param name="pBendForest">����ĳ�������ɵ�����ɭ��</param>
    //    /// <remarks></remarks>
    //    private double  CalBendForestCovariance(List<double> dblFromDeepBendDepthLt, List<double> dblToDeepBendDepthLt)
    //    {
    //        //����׼��
    //        double dblStep = Convert.ToDouble(dblFromDeepBendDepthLt.Count) / Convert.ToDouble(dblToDeepBendDepthLt.Count);
    //        double dblHalfStep = dblStep / 2;

    //        List<double> dblExtractiveDepthLt = new List<double>();
    //        for (int i = 0; i < dblToDeepBendDepthLt.Count ; i++)
    //        {
    //            int intIndex = Convert.ToInt32(i * dblStep + dblHalfStep);
    //            dblExtractiveDepthLt.Add(dblFromDeepBendDepthLt[intIndex]);
    //        }


    //        double dblFromSum = 0;
    //        for (int i = 0; i < dblExtractiveDepthLt.Count; i++)
    //            dblFromSum = dblFromSum + dblExtractiveDepthLt[i];
    //        double dblFromAverage = dblFromSum / dblExtractiveDepthLt.Count;

    //        double dblToSum = 0;
    //        for (int i = 0; i < dblToDeepBendDepthLt.Count; i++)
    //            dblToSum = dblToSum + dblToDeepBendDepthLt[i];
    //        double dblToAverage = dblToSum / dblExtractiveDepthLt.Count;

    //        double dblSum = 0;
    //        for (int i = 0; i < dblExtractiveDepthLt.Count; i++)
    //            dblSum = dblSum + (dblExtractiveDepthLt[i] - dblFromAverage) * (dblToDeepBendDepthLt[i] - dblToAverage);
    //        double dblAverage = dblSum / dblExtractiveDepthLt.Count;
    //        double dblCovariance = Math.Sqrt(dblAverage);

    //        return dblCovariance;










    //    }

    //    /// <summary>
    //    /// ������������ɭ��������Ȳ�ı�׼��
    //    /// </summary>
    //    /// <param name="pBendForest">����ĳ�������ɵ�����ɭ��</param>
    //    /// <remarks>SD : Standard Deviation </remarks>
    //    private double CalBendForestDepthDiffSD(List<double> dblFromDeepBendDepthLt, List<double> dblToDeepBendDepthLt)
    //    {
    //        int intCount = dblToDeepBendDepthLt.Count;

    //        //����From����࣬�ȴ�From������ȡ��To����������
    //        double dblStep = Convert.ToDouble(dblFromDeepBendDepthLt.Count) / Convert.ToDouble(dblToDeepBendDepthLt.Count);
    //        double dblHalfStep = dblStep / 2;

    //        List<double> dblExtractiveDepthLt = new List<double>();
    //        for (int i = 0; i < intCount; i++)
    //        {
    //            int intIndex = Convert.ToInt32(i * dblStep + dblHalfStep);
    //            dblExtractiveDepthLt.Add(dblFromDeepBendDepthLt[intIndex]);
    //        }


    //        List<double> dblDepthDiffLt = new List<double>();
    //        double dblDepthDiffSum = 0;
    //        for (int i = 0; i < intCount; i++)
    //        {
    //            double dblDepthDiff = dblExtractiveDepthLt[i] - dblToDeepBendDepthLt[i];
    //            dblDepthDiffLt.Add(dblDepthDiff);
    //            dblDepthDiffSum = dblDepthDiffSum + dblDepthDiff;
    //        }
    //        double dblDepthDiffAverage = dblDepthDiffSum / intCount;

    //        double dblSum = 0;
    //        for (int i = 0; i < intCount; i++)
    //            dblSum = dblSum + (dblDepthDiffLt[i] - dblDepthDiffAverage) * (dblDepthDiffLt[i] - dblDepthDiffAverage);
    //        double dblAverage = dblSum / (dblExtractiveDepthLt.Count-1);
    //        double dblDepthDiffSD = Math.Sqrt(dblAverage);


    //        return dblDepthDiffSD;

    //    }


    //    private double CalBendForestDepthSumRatio(CBendForest FromBendForest, CBendForest ToBendForest)
    //    {
    //        double dblFromSum = 0;
    //        for (int i = 0; i < FromBendForest.Count ; i++)
    //            dblFromSum = dblFromSum + FromBendForest.ElementAt(i).Value.dblBendDepthSum;

    //        double dblToSum = 0;
    //        for (int i = 0; i < ToBendForest.Count; i++)
    //            dblToSum = dblToSum + ToBendForest.ElementAt(i).Value.dblBendDepthSum;           

    //        double dblRatio = dblFromSum / dblToSum;
    //        return dblRatio;
    //    }












    //}
}
