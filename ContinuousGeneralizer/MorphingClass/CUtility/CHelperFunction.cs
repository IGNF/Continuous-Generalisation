using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
//using System.Windows;

using SCG = System.Collections.Generic;
using C5;
using Microsoft.Office.Interop;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;

using MorphingClass.CCorrepondObjects;
using MorphingClass.CGeometry;
using MorphingClass.CGeometry.CGeometryBase;
using MorphingClass.CEntity;
using MorphingClass.CMorphingMethods.CMorphingMethodsBase;
using VBClass;
//using EasyMorphing.
//using MorphingClass.

namespace MorphingClass.CUtility
{



    public static class CHelperFunction
    {
        //private static string _strDataFolderName = "Jiangxi";
        //private static string _strDataFolderName = "CompatibleTriangulationTestVerySimple";
        //private static string _strDataFolderName = "JiangxiOneCounty";
        //private static string _strDataFolderName = "rail÷����Test";
        //private static string _strPath = "C:\\Study\\Data\\Morphing Data\\Rail Data\\" + _strDataFolderName + "\\";

        //private static string _strDataFolderName = "MainlandChina-Yunnan";
        //private static string _strDataFolderName = "MainlandChina-Guangxi";
        //private static string _strDataFolderName = "MainlandChina-Tianjin";
        //private static string _strDataFolderName = "MainlandChina-Animation";
        //private static string _strDataFolderName = "Tianjin-Animation"; 
        //private static string _strDataFolderName = "MainlandChina-Animation-complicated";
        //private static string _strDataFolderName = "MainlandChina-Shanghai";
        //private static string _strDataFolderName = "MainlandChina";
        //private static string _strDataFolderName = "CompatibleTriangulation";
        //private static string _strPath = "C:\\MyWork\\DailyWork\\ContinuousGeneralisation\\ContinuousGeneralisation_Data\\Administrative Boundary\\" + _strDataFolderName + "\\";
        //C:\MyWork\DailyWork\ContinuousGeneralisation\ContinuousGeneralisation_Data

        //private static string _strDataFolderName = "LinearMorphingBothInjected";
        //private static string _strDataFolderName = "CompatibleTriangulation3";
        //private static string _strPath = "C:\\Study\\Data\\Morphing Data\\Representation In Article\\ContinuousGeneralizationOfAdministrativeBoundariesBasedonMorphing\\" + _strDataFolderName + "\\";

        //private static string _strDataFolderName = "AreaAggregation";
        //private static string _strDataFolderName = "AreaAggregation_85_Hole";        
        //private static string _strDataFolderName = "AreaAggregation_543";
        //private static string _strDataFolderName = "AreaAggregation_544";
        //private static string _strDataFolderName = "AreaAggregation-Simulation";
        //private static string _strDataFolderName = "AreaAggregation-531";
        //private static string _strDataFolderName = "AreaAggregation-664-easy";
        //private static string _strDataFolderName = "AreaAggregation-590";
        //private static string _strDataFolderName = "AreaAggregation-Simplest";
        //private static string _strDataFolderName = "AreaAggregation_SmallInstances";
        //private static string _strDataFolderName = "AreaAggregation_Simplest_Three";
        //private static string _strDataFolderName = "AreaAggregation-Computable";
        //private static string _strDataFolderName = "AreaAggregation-Uncomputable";
        //private static string _strDataFolderName = "AreaAggregation-Mostpatches";
        //private static string _strPath = "C:\\MyWork\\DailyWork\\ContinuousGeneralisation\\ContinuousGeneralisation_Data\\AreaAggregation\\" + _strDataFolderName + "\\";

        //private static string _strDataFolderName = "France";
        //private static string _strDataFolderName = "France_Part";
        private static string _strDataFolderName = "France_Smallpart";
        //private static string _strDataFolderName = "France_Problematic";
        //private static string _strDataFolderName = "France_WithOneHole";
        //private static string _strDataFolderName = "France_OneSquare";
        //private static string _strDataFolderName = "France_TwoSquares_Faraway";
        //private static string _strDataFolderName = "France_TwoSquares_Overlap";
        private static string _strPath = "C:\\MyWork\\DailyWork\\ContinuousGeneralisation\\ContinuousGeneralisation_Data\\BuildingGrowing\\" + _strDataFolderName + "\\";


        public static IEnumerable<CPoint> GetTestCptEb()
        {
            yield return new CPoint(0, 0, 0);
            yield return new CPoint(1, 1, 0);
            yield return new CPoint(2, 0, 1);
            yield return new CPoint(3, 0, 0);
        }

        public static string strPath
        {
            get { return _strPath; }
            //set { _strPath = value; }
        }

        public static string strDataFolderName
        {
            get { return _strDataFolderName; }
            //set { _strDataFolderName = value; }
        }

        public static bool InBoundOrReport<T>(T value, T lowerbound, T upperbound, IComparer<T> cmp = null)
        {
            if (cmp == null) { cmp = SCG.Comparer<T>.Default; }
            //var tst = cmp.Compare(value, lowerbound);
            if (cmp.Compare(value, lowerbound) == -1 ||cmp.Compare(value, upperbound) == 1  )
            {
                throw new ArgumentException("incorrect value!");
            }

            return true;
        }

        public static void testmemory()
        {
            System.Collections.Generic.IList<System.Collections.Generic.IList<double>> intltlt = new List<System.Collections.Generic.IList<double>>();

            //while (intltlt.Count < 40)
            //{
            //    intltlt.Add(testttt());
            //}


            while (intltlt.Count < 1000000)
            {
                intltlt.Add(testttt());
            }

            while (intltlt.Count < 2000000)
            {
                intltlt.Add(testttt());
            }

            int st = 5;
            int sq = st;
        }

        public static System.Collections.Generic.IList<double> testttt()
        {
            int intLimit = 1000000;
            System.Collections.Generic.IList<double> intlt = new List<double>(intLimit);
            while (intlt.Count < intLimit)
            {
                intlt.Add(3.0);
            }
            return intlt;
        }

        public static double GetConsumedMemoryInMB(bool blnforceFullCollection, long lngStartMemoryInByte=0)
        {
            return Math.Round(Convert.ToDouble(GC.GetTotalMemory(blnforceFullCollection) - lngStartMemoryInByte) / 1048576, 3);
        }

        public static void SetSavePath(CParameterInitialize ParameterInitialize)
        {
            //if we have already set a path, then we simply use that path
            if (ParameterInitialize.strPath!=null)
            {
                return;
            }

            if (_strPath == null)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.ShowDialog();
                if (SFD.FileName == null || SFD.FileName == "") return;
                _strPath = SFD.FileName;
            }

            string strFileName = _strPath + GetTimeStamp();
            //string strFileName = CHelperFunction.strPath + "MorphingResults";

            ParameterInitialize.strPath = _strPath;
            ParameterInitialize.strSaveFolder = System.IO.Path.GetFileNameWithoutExtension(strFileName);
            ParameterInitialize.strSavePath = strFileName;
            ParameterInitialize.strSavePathBackSlash = strFileName + "\\";
            ParameterInitialize.pWorkspace = CHelperFunction.OpenWorkspace(strFileName);
        }

        public static string GetTimeStamp()
        {
            var strMonth = JudgeAndAddZero(DateTime.Now.Month);
            var strDay = JudgeAndAddZero(DateTime.Now.Day);
            var strHour = JudgeAndAddZero(DateTime.Now.Hour);
            var strMinute = JudgeAndAddZero(DateTime.Now.Minute);
            var strSecond = JudgeAndAddZero(DateTime.Now.Second);
            var strMillisecond = JudgeAndAddZero(DateTime.Now.Millisecond,3);
            return "_" + DateTime.Now.Year.ToString() + strMonth + strDay + "_" + strHour + strMinute + strSecond + strMillisecond;
        }

        public static string JudgeAndAddZero(double dblNumber, int intDigits = 2)
        {
            var dblTest = Math.Pow(10, Convert.ToDouble(intDigits - 1));

            string strPrefix = "";
            while (dblTest > 1)
            {
                if (dblNumber < dblTest)
                {
                    strPrefix += "0";
                    dblTest = dblTest / 10;
                }
                else
                {
                    break;
                }
            }
            return strPrefix + dblNumber.ToString();
        }

        public static void Displaytspb(double dblValue, double dblTotal, ToolStripProgressBar tspb)
        {
            tspb.Value = Convert.ToInt32(dblValue * 100 / dblTotal);
        }



        public static List<object> GetObjLtByFeatureLayer(IFeatureLayer pFeatureLayer, out List<List<object>> pobjectValueLtLt, 
            string strSpecifiedFieldName = null, string strSpecifiedValue = null)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            int intFeatureCount = pFeatureClass.FeatureCount(null);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    //ע��˴��Ĳ���(****,false)������
            int intFieldCount = pFeatureClass.Fields.FieldCount;
            pobjectValueLtLt = new List<List<object>>(intFeatureCount);

            int intSpecifiedFieldIndex = -1;
            if (strSpecifiedFieldName!=null )
            {
               intSpecifiedFieldIndex = pFeatureClass.FindField(strSpecifiedFieldName);
               pobjectValueLtLt = new List<List<object>>(1);
            }
           
            var ObjShapeLt = new List<object>(intFeatureCount);            
            for (int i = 0; i < intFeatureCount; i++)
            {
                IFeature pFeature = pFeatureCursor.NextFeature();
                if (strSpecifiedFieldName!=null)  //to only get the specified feature
                {
                    if (pFeature.get_Value(intSpecifiedFieldIndex).ToString()!=strSpecifiedValue)
                    {
                        continue;
                    }
                }

                TestIGeoAccordingToInput(pFeature.Shape, i);
                ObjShapeLt.Add(pFeature.Shape);
                
                var ObjValueLt = new List<object>(intFieldCount-2);  //we don't need the first two values, i.e., Id and shape
                for (int j = 2; j < intFieldCount; j++)
                {
                    ObjValueLt.Add(pFeature.get_Value(j));
                }
                pobjectValueLtLt.Add(ObjValueLt);
            }

            return ObjShapeLt;
        }

        public static IEnumerable<object> GenerateCGeoEbAccordingToInputLt(List<object> pObjIGeoLt, double dblFactor = 1)
        {
            for (int i = 0; i < pObjIGeoLt.Count; i++)
            {
                yield return GenerateCGeoAccordingToInput((IGeometry)pObjIGeoLt[i], i, dblFactor);
            }
        }

        public static IEnumerable<T> GenerateCGeoEbAccordingToGeoEb<T>(IEnumerable<IGeometry> IGeoEb)
        {
            var IGeoEt = IGeoEb.GetEnumerator();
            int intCount=0;
            while (IGeoEt.MoveNext ())
            {
                yield return (T)GenerateCGeoAccordingToInput(IGeoEt.Current, intCount++);
            }
        }



        public static void TestIGeoAccordingToInput(IGeometry pGeo, int intIndex)
        {
            switch (pGeo.GeometryType)
            {
                case esriGeometryType.esriGeometryAny:
                    break;
                case esriGeometryType.esriGeometryBag:
                    break;
                case esriGeometryType.esriGeometryBezier3Curve:
                    break;
                case esriGeometryType.esriGeometryCircularArc:
                    break;
                case esriGeometryType.esriGeometryEllipticArc:
                    break;
                case esriGeometryType.esriGeometryEnvelope:
                    break;
                case esriGeometryType.esriGeometryLine:
                    break;
                case esriGeometryType.esriGeometryMultiPatch:
                    break;
                case esriGeometryType.esriGeometryMultipoint:
                    break;
                case esriGeometryType.esriGeometryNull:
                    break;
                case esriGeometryType.esriGeometryPath:
                    break;
                case esriGeometryType.esriGeometryPoint:               //point******************
                    break;
                case esriGeometryType.esriGeometryPolygon:             //polygon******************
                    var ipg = pGeo as IPolygon4;
                    if (ipg.Length <= 0)
                    {
                        throw new ArgumentNullException("Polygon "+ intIndex+ " has length 0!");
                    }

                    var pArea = ipg as IArea;
                    if (pArea.Area <= 0)
                    {
                        throw new ArgumentNullException("Polygon " + intIndex + " has area 0!");
                    }
                    break;
                case esriGeometryType.esriGeometryPolyline:            //polyline******************
                    var ipl = pGeo as IPolyline5;
                    if (ipl.Length <= 0)
                    {
                        throw new ArgumentNullException("Polyline " + intIndex + " has length 0!");
                    }
                    break;
                case esriGeometryType.esriGeometryRay:
                    break;
                case esriGeometryType.esriGeometryRing:
                    break;
                case esriGeometryType.esriGeometrySphere:
                    break;
                case esriGeometryType.esriGeometryTriangleFan:
                    break;
                case esriGeometryType.esriGeometryTriangleStrip:
                    break;
                case esriGeometryType.esriGeometryTriangles:
                    break;
                default:
                    break;
            }
        }

        public static object GenerateCGeoAccordingToInput(IGeometry pGeo, int intIndex = -2, double dblFactor = 1) 
        {
            object obj = null;
            switch (pGeo.GeometryType)
            {
                case esriGeometryType.esriGeometryAny:
                    break;
                case esriGeometryType.esriGeometryBag:
                    break;
                case esriGeometryType.esriGeometryBezier3Curve:
                    break;
                case esriGeometryType.esriGeometryCircularArc:
                    break;
                case esriGeometryType.esriGeometryEllipticArc:
                    break;
                case esriGeometryType.esriGeometryEnvelope:
                    break;
                case esriGeometryType.esriGeometryLine:
                    break;
                case esriGeometryType.esriGeometryMultiPatch:
                    break;
                case esriGeometryType.esriGeometryMultipoint:
                    break;
                case esriGeometryType.esriGeometryNull:
                    break;
                case esriGeometryType.esriGeometryPath:
                    break;
                case esriGeometryType.esriGeometryPoint:               //point******************
                    obj = new CPoint(intIndex, (IPoint)pGeo);
                    break;
                case esriGeometryType.esriGeometryPolygon:             //polygon******************
                    obj = new CPolygon(intIndex, (IPolygon4)pGeo, dblFactor);
                    break;
                case esriGeometryType.esriGeometryPolyline:            //polyline******************
                    obj = new CPolyline(intIndex, (IPolyline5)pGeo);
                    break;
                case esriGeometryType.esriGeometryRay:
                    break;
                case esriGeometryType.esriGeometryRing:
                    break;
                case esriGeometryType.esriGeometrySphere:
                    break;
                case esriGeometryType.esriGeometryTriangleFan:
                    break;
                case esriGeometryType.esriGeometryTriangleStrip:
                    break;
                case esriGeometryType.esriGeometryTriangles:
                    break;
                default:
                    break;
            }
            return obj;
        }




        /// <summary>
        /// ͨ����Ҫ��ͼ���ȡ������
        /// </summary>
        /// <param name="pFeatureLayer">��Ҫ��ͼ��</param>
        /// <returns>������</returns>
        public static List<CPolyline> GetCPlLtByFeatureLayer(IFeatureLayer pFeatureLayer)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            int intFeatureCount = pFeatureClass.FeatureCount(null);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    //ע��˴��Ĳ���(****,false)������
            List<CPolyline> CPlLt = new List<CPolyline>(intFeatureCount);    //�ȴ��������飬�Ի�ȡͼ���е��߶�
            for (int i = 0; i < intFeatureCount; i++)
            {
                IFeature pFeature = pFeatureCursor.NextFeature();
                IPolyline5 pPolyline = new PolylineClass();
                pPolyline = (IPolyline5)pFeature.Shape;
                SetZCoordinates(pPolyline as IPointCollection4);  //set the z coordinates, it may be used in Constructing TIN


                CPolyline cpl = new CPolyline(i, pPolyline);
                CPlLt.Add(cpl);
            }

            return CPlLt;
        }


        public static List<CPoint> GetCPtLtFromPointFeatureLayer(IFeatureLayer pFeatureLayer)
        {

            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            int intFeatureCount = pFeatureClass.FeatureCount(null);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    //ע��˴��Ĳ���(****,false)������
            List<CPoint> CPtLt = new List<CPoint>(intFeatureCount);
            for (int i = 0; i < intFeatureCount; i++)
            {
                IFeature pFeature = pFeatureCursor.NextFeature();
                IPoint ipt = (IPoint)pFeature.Shape;
                CPoint cpt = new CPoint(i, ipt);
                CPtLt.Add(cpt);
            }

            return CPtLt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCol"></param>
        /// <remarks>note that if we set z coordinates, then we may lose many information related to IPolygon (ConnectedComponentBag, InteriorRingBag, ExteriorRingBag, etc.) </remarks>
        public static void SetZCoordinates(IPointCollection4 pCol)
        {
            for (int i = 0; i < pCol.PointCount; i++)
            {
                IPoint copyipt = pCol.get_Point(i);
                copyipt.Z = 0;
                pCol.UpdatePoint(i, copyipt);
            }
        }
        //public static void PreviousWorkCplLt(ref List<CPolyline> cpllt, CEnumScale enumScale)
        //{
        //    foreach (CPolyline cpl in cpllt)
        //    {
        //        CPolyline cpli = cpl;
        //        PreviousWorkCpl(ref cpli, enumScale);
        //    }
        //}

        //public static void PreviousWorkCSeLt(ref C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk)
        //{
        //    foreach (CCorrespondSegment pCorrespondSegment in CorrespondSegmentLk)
        //    {
        //        CPolyline frcpl = pCorrespondSegment.CFrPolyline;
        //        CPolyline tocpl = pCorrespondSegment.CToPolyline;
        //        PreviousWorkCpl(ref frcpl, CEnumScale.Larger);
        //        PreviousWorkCpl(ref tocpl, CEnumScale.Smaller);
        //    }
        //}

        public static void SetAbsAndRatioLength(ref CPolyline cpl, CEnumScale enumScale)
        {
            //cpl.enumScale = enumScale;
            //cpl.SetCptBelongedPolyline();
            //cpl.SetEdgeLength();
            cpl.SetAbsLengthFromStart();
            cpl.SetRatioLengthFromStart();

        }


        //public static void SetCEdgeCEdgeTwinBelongedCpl(ref List<CPolyline> cpllt)
        //{
        //    foreach (CPolyline cpl in cpllt)
        //    {
        //        cpl.SetCEdgeBelongedPolyline();
        //        cpl.SetCEdgeTwinBelongedPolyline();
        //    }
        //}







        /// <summary>
        /// transform some items into a list
        /// </summary>
        /// <returns></returns>
        /// <remarks >we allow at most three items here. the number of items could be increased if necessary</remarks>
        public static List<T> MakeLt<T>(int intCount, T item1=null, T item2=null, T item3=null)
           where T:class
        {
            List<T> TLt = new List<T>(intCount);

            if (intCount >= 1)
            {
                TLt.Add(item1);
            }
            if (intCount >= 2)
            {
                TLt.Add(item2);
            }
            if (intCount >= 3)
            {
                TLt.Add(item3);
            }

            return TLt;
        }

        ///// <summary>
        ///// transform some items into a list
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks >we allow at most three items here. the number of items could be increased if necessary</remarks>
        //public static IEnumerable<T> MakeEb<T>(int intCount, T item1, T item2 = null, T item3 = null, T item4 = null, T item5 = null, T item6 = null)
        //   where T : class
        //{
            
        //    if (intCount >= 1)
        //    {
        //        yield return item1;
        //    }
        //    if (intCount >= 2)
        //    {
        //        yield return item2;
        //    }
        //    if (intCount >= 3)
        //    {
        //        yield return item3;
        //    }
        //    if (intCount >= 4)
        //    {
        //        yield return item4;
        //    }
        //    if (intCount >= 5)
        //    {
        //        yield return item5;
        //    }
        //    if (intCount >= 6)
        //    {
        //        yield return item6;
        //    }

        //}

        /// <summary>
        /// ͨ����Ҫ��(IPolyline5)��ȡ������
        /// </summary>
        /// <param name="ipl">��Ҫ��</param>
        /// <returns>������</returns>
        public static List<CPoint> GetCPtLtByIPl(IPolyline5 ipl)
        {
            IGeometryCollection pGeoCol = ipl as IGeometryCollection;
            if (pGeoCol.GeometryCount>1)
            {
                throw new ArgumentException("I didn't consider the problem of multiple polylines!");
            }

            return GetCptEbByICol((IPointCollection4)ipl).ToList();
        }

        //static int intCount = 0;

        /// <summary>
        /// ͨ����Ҫ��(IPolygon4)��ȡ������
        /// </summary>
        /// <param name="ipg">��Ҫ��</param>
        /// <returns>������</returns>
        /// <remarks>currently, we assume that there is only one exterior ring for ipg</remarks>
        public static List<List<CPoint>> GetCPtLtLtByIPG(IPolygon4 ipg, double dblFactor = 1)
        {
            //ipg.Close();
            if (ipg.ExteriorRingCount != 1)  //exterior ring: 127.0.0.1:47873/help/1-6544/ms.help?method=page&id=ESRIGEOMETRY-7B3BAA75-000594&product=VS&productVersion=100&topicVersion=&locale=EN-US&topicLocale=EN-US
            {
                throw new ArgumentException("I have not considered such a complicated case! This includes the case that a hole contains other holes! ");
            }

            IRing2 pExteriorRing = (ipg.ExteriorRingBag as IGeometryCollection).get_Geometry(0) as IRing2;
            IGeometryCollection pGeoColInteriorRing = (IGeometryCollection)ipg.get_InteriorRingBag(pExteriorRing);
            
            //the points list
            var cptltlt = new List<List<CPoint>>(pGeoColInteriorRing.GeometryCount + 1);
            cptltlt.Add(GetCptEbByICol(pExteriorRing as IPointCollection4, dblFactor).ToList());
            for (int i = 0; i < pGeoColInteriorRing.GeometryCount; i++)
            {
                cptltlt.Add(GetCptEbByICol(pGeoColInteriorRing.get_Geometry(i) as IPointCollection4, dblFactor).ToList());
            }

            return cptltlt;
        }

        public static IEnumerable<IEnumerable<CPoint>> GetCptEbEbByIColEb<T>(IEnumerable<T> TEb, int intFactor = 1)
        {
            foreach (var item in TEb)
            {
                yield return GetCptEbByICol(item as IPointCollection4, intFactor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCol">for a polygon, the fisrt point and the last point are identical</param>
        /// <param name="dblFactor"></param>
        /// <returns></returns>
        public static IEnumerable<CPoint> GetCptEbByICol(IPointCollection4 pCol, double dblFactor = 1)
        {
            for (int i = 0; i < pCol.PointCount; i++)
            {
                yield return new CPoint(i, pCol.get_Point(i), dblFactor);
            }
        }

        public static IPointCollection4 GetPointCollectionFromCptLt(List<CPoint> cptlt, esriGeometryType pesriGeometryType = esriGeometryType .esriGeometryPolyline)
        {
            IPointCollection4 pCol = null;
            switch (pesriGeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    pCol = new PolygonClass();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pCol = new PolylineClass();
                    break;
                default:
                    break;
            }

            foreach (var cpt in cptlt)
            {
                cpt.JudgeAndSetPoint();
                pCol.AddPoint(cpt.pPoint);
            }

            return pCol;
        }

        public static void ReverseCptLt(ref List<CPoint> cptlt)
        {
            cptlt.Reverse();
            for (int i = 0; i < cptlt.Count - 1; i++)
            {
                cptlt[i].ID = i;
                if (cptlt[i].pPoint != null)
                {
                    cptlt[i].pPoint.ID = i;
                }
            }
        }


        public static List<CPoint> CopyCptLt(List<CPoint> cptlt)
        {
            List<CPoint> copiedcptlt = new List<CPoint>(cptlt.Count );
            cptlt.ForEach(cpt => copiedcptlt.Add(cpt.Copy()));
            
            return copiedcptlt;
        }


        //��ɫ�� 
        public static void ViewPolyline(IMapControl4 pMapControl, CPolyline cpl)
        {
            //�����߶�����
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            ILineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Color = pColor;

            //�����߶�
            ILineElement pLineElement = new LineElementClass();
            pLineElement.Symbol = pSimpleLineSymbol;
            IElement pElement = pLineElement as IElement;
            pElement.Geometry = cpl.pPolyline;

            //��ʾ�߶�
            IGraphicsContainer pGra = pMapControl.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            pGra.AddElement(pElement, 0);
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);  //�����ز����٣�������ӵ�Ҫ�ز���ʵʱ��ʾ
        }

        //��ɫ�� 
        public static void ViewPolylines(IMapControl4 pMapControl, List<CPolyline> cpllt)
        {
            //�����߶�����
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            ILineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            pSimpleLineSymbol.Width = 1;
            pSimpleLineSymbol.Color = pColor;


            //�����߶�
            IElementCollection pEleCol = new ElementCollectionClass();

            for (int i = 0; i < cpllt.Count; i++)
            {
                ILineElement pLineElement = new LineElementClass();
                pLineElement.Symbol = pSimpleLineSymbol;
                IElement pElement = pLineElement as IElement;
                pElement.Geometry = cpllt[i].pPolyline;
                pEleCol.Add(pElement, 0);
            }

            //��ʾ�߶�
            IGraphicsContainer pGra = pMapControl.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            pGra.AddElements(pEleCol, 5);
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }


        /// <summary>
        /// Get the type and the index of this type for every Cgb 
        /// </summary>
        /// <typeparam name="CGeo"></typeparam>
        /// <param name="TEb"></param>
        /// <param name="objltlt"></param>
        /// <param name="intValueIndex"></param>
        /// <param name="intTypeIndexSD"></param>
        public static void GetCgbTypeAndTypeIndex(IEnumerable<CPolygon> TEb, List<List<object>> objltlt, int intValueIndex, CValMap_SD<int, int> TypePVSD)
            //where CGeo : class
        {
            IEnumerator<CPolygon> TEt = TEb.GetEnumerator();
            //IEnumerator<object> objEt = objEb.GetEnumerator();

            int intCount = 0;
            while (TEt.MoveNext())
            {
                CPolygon TCurrent = TEt.Current;

                //get intType
                TCurrent.intType = Convert.ToInt32(objltlt[intCount++][intValueIndex]);

                //get intTypeIndex
                int intTypeIndex;
                if (TypePVSD.SD.TryGetValue(TCurrent.intType, out intTypeIndex) == true)
                {
                    TCurrent.intTypeIndex = intTypeIndex;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("failed to get type index for a type!");
                    //MessageBox.Show("failed to get type index for a type!   In: " + "CHelperFunction.cs");
                }
            }
        }


        //public static void GetCgbTypeAndTypeIndex<CGeo>(IEnumerable<CGeometricBase<CGeo>> TEb, List<List<object>> objltlt, int intValueIndex, CValMap_SD<int, int> TypePVSD)
        //    where CGeo : class
        //{
        //    IEnumerator<CGeometricBase<CGeo>> TEt = TEb.GetEnumerator();
        //    //IEnumerator<object> objEt = objEb.GetEnumerator();

        //    int intCount = 0;
        //    while (TEt.MoveNext())
        //    {
        //        CGeometricBase<CGeo> TCurrent = TEt.Current;

        //        //get intType
        //        TCurrent.intType = Convert.ToInt32(objltlt[intCount++][intValueIndex]);

        //        //get intTypeIndex
        //        int intTypeIndex;
        //        if (TypePVSD.SD.TryGetValue(TCurrent.intType, out intTypeIndex) == true)
        //        {
        //            TCurrent.intTypeIndex = intTypeIndex;
        //        }
        //        else
        //        {
        //            throw new ArgumentOutOfRangeException("failed to get type index for a type!");
        //            //MessageBox.Show("failed to get type index for a type!   In: " + "CHelperFunction.cs");
        //        }
        //    }
        


        public static IEnumerable<object> JudgeAndSetAEGeometry<T>(IEnumerable<T> pCGeoEb)
            where T : CGeometricBase<T>
        {
            foreach (var pCGeo in pCGeoEb)
            {
                yield return (pCGeo.JudgeAndSetAEGeometry() as object);
            }
        }

        public static void SetAEGeometryNull<T>(IEnumerable<T> pCGeoEb)
            where T : CGeometricBase<T>
        {
            foreach (var pCGeo in pCGeoEb)
            {
                pCGeo.SetAEGeometryNull();
            }
        }


        /// <summary>
        /// �����Ӧ��
        /// </summary>
        /// <param name="CResultPtLt">�����飨������Ӧ����Ϣ��</param>
        /// <param name="strFileName">�ļ���</param>
        /// <param name="strPath">����·��</param>
        /// <param name="m_mapControl">��ͼ�ؼ�</param>
        public static void SaveCtrlLine(List<List<CCorrCpts>> CtrlCptLtLt, string strFileName, double dblStandardLength, IWorkspace pWorkspace, IMapControl4 m_mapControl)
        {
            List<CPolyline> CtrlCplLt = GenerateCplLt(CtrlCptLtLt);
            int intCount = CtrlCptLtLt.GetCountItem();
            List<CPolyline> cpllt1 = new List<CPolyline>(intCount);
            List<CPolyline> cpllt2 = new List<CPolyline>(intCount);

            foreach (CPolyline CtrlCpl in CtrlCplLt)
            {
                CtrlCpl.SetPolyline();

                if (CCompareMethods.CompareDbl_VerySmall(CtrlCpl.pPolyline.Length, dblStandardLength) == 0)
                {
                    cpllt1.Add(CtrlCpl);
                }
                else
                {
                    cpllt2.Add(CtrlCpl);
                }
            }
            
            SaveCPlLt(cpllt2, strFileName + "_UnPrecise", pWorkspace, m_mapControl, intRed: 255);
            SaveCPlLt(cpllt1, strFileName + "_Precise", pWorkspace, m_mapControl, intRed: 255);
        }

        /// <summary>
        /// �����Ӧ��
        /// </summary>
        /// <param name="CResultPtLt">�����飨������Ӧ����Ϣ��</param>
        /// <param name="strFileName">�ļ���</param>
        /// <param name="strPath">����·��</param>
        /// <param name="m_mapControl">��ͼ�ؼ�</param>
        public static void SaveCorrLine(List<List<CCorrCpts>> CtrlCptLtLt, string strFileName, IWorkspace pWorkspace, IMapControl4 m_mapControl)
        {
            List<CPolyline> CtrlCplLt = GenerateCplLt(CtrlCptLtLt);
            SaveCPlLt(CtrlCplLt, strFileName, pWorkspace, m_mapControl, intRed: 255);
        }


        public static List<CPolyline> GenerateCplLt(List<List<CCorrCpts>> CorrCptsLtLt)
        {
            List<CPolyline> cpllt = new List<CPolyline>(CorrCptsLtLt.GetCountItem());
            foreach (List<CCorrCpts> CorrCptsLt in CorrCptsLtLt)
            {
                foreach (CCorrCpts CorrCpts in CorrCptsLt)
                {
                    cpllt.Add(new CPolyline(CorrCpts));
                }
            }

            return cpllt;
        }

        

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="cpllt">������</param>
        /// <param name="strFileName">������ļ���</param>
        /// <param name="strPath">����·��</param>
        /// <param name="m_mapControl">��ͼ�ؼ�</param>
        public static IFeatureLayer SaveCPlLt(List<CPolyline> cpllt, string strFileName, IWorkspace pWorkspace, IMapControl4 m_mapControl, int intRed = 0, int intGreen = 0, int intBlue = 0, double dblWidth = 1)
        {
            return CSaveFeature.SaveCGeoEb(cpllt, esriGeometryType.esriGeometryPolyline, strFileName, pWorkspace, m_mapControl,null ,null ,null, intRed, intGreen, intBlue, dblWidth);
        }
        

        public static void PrintStart(string strName)
        {
            string str = "\n-----------------------Start to print " + strName + "-----------------------";
            Console.WriteLine(str);
        }

        public static void PrintEnd(string strName)
        {
            string str = "************************End of printing " + strName + "************************\n\n";
            Console.WriteLine(str);
        }

        

        /// <summary>
        /// SaveToIpe
        /// </summary>
        /// <param name="CPlLt"></param>
        /// <param name="strWidth">normal, heavier, fat, ultrafat</param>
        public static void SaveToIpe(List<CPolyline> CPlLt, string strFileName, IEnvelope pEnvelopeLayer, CEnvelope pEnvelopeIpe, CParameterInitialize pParameterInitialize, int fintRed = 0, int fintGreen = 0, int fintBlue = 0, string strWidth = "normal", bool blnGroup = true)
        {
            double dblFactor = (pEnvelopeIpe.XMax - pEnvelopeIpe.XMin) / pEnvelopeLayer.Width;
            SCG.LinkedList<string> strLkText = new SCG.LinkedList<string>();

            double dblIpeRed = Convert.ToDouble(fintRed) / 255;
            double dblIpeGreen = Convert.ToDouble(fintGreen) / 255;
            double dblIpeBlue = Convert.ToDouble(fintBlue) / 255;

            int intcount = 0;
            foreach (CPolyline cpl in CPlLt)
            {
                List<CPoint> cptlt = cpl.CptLt;
                string str = "<path stroke=\"" + dblIpeRed + " " + dblIpeGreen + " " + dblIpeBlue + "\" pen=\"" + strWidth + "\">\n";

                double dblX0 = CGeometricMethods.CoordinateTransform(cptlt[0].X, pEnvelopeLayer.XMin, pEnvelopeIpe.XMin, dblFactor);
                double dblY0 = CGeometricMethods.CoordinateTransform(cptlt[0].Y, pEnvelopeLayer.YMin, pEnvelopeIpe.YMin, dblFactor);

                //str += GetStrOfEnglishNumber(dblX0) + " " + GetStrOfEnglishNumber(dblY0) + " " + "m" + "\n";
                str += dblX0 + " " + dblY0 + " " + "m" + "\n";

                for (int j = 1; j < cptlt.Count; j++)
                {
                    double dblX = CGeometricMethods.CoordinateTransform(cptlt[j].X, pEnvelopeLayer.XMin, pEnvelopeIpe.XMin, dblFactor);
                    double dblY = CGeometricMethods.CoordinateTransform(cptlt[j].Y, pEnvelopeLayer.YMin, pEnvelopeIpe.YMin, dblFactor);

                    //str += GetStrOfEnglishNumber(dblX) + " " + GetStrOfEnglishNumber(dblY) + " " + "l" + "\n";
                    str += dblX + " " + dblY + " " + "l" + "\n";
                }
                str += "</path>\n";
                strLkText.AddLast(str);

                intcount++;
            }


            using (var writer = new System.IO.StreamWriter(pParameterInitialize.strSavePath + "\\" + strFileName + ".ipe", true))
            {
                if (blnGroup == true)
                {
                    writer.Write("<group>\n");
                }

                foreach (string str in strLkText)
                {
                    writer.Write(str);
                }

                if (blnGroup == true)
                {
                    writer.Write("</group>\n");
                }
            }
        }





        /// <summary>
        /// SaveToIpe
        /// </summary>
        /// <param name="CPlLt"></param>
        /// <param name="strWidth">normal, heavier, fat, ultrafat</param>
        public static void SaveToIpe(List<CPoint> CptLt, string strFileName, IEnvelope pEnvelopeLayer, CEnvelope pEnvelopeIpe, CParameterInitialize pParameterInitialize, int fintRed = 0, int fintGreen = 0, int fintBlue = 0, string strWidth = "normal", bool blnGroup = true)
        {
            double dblFactor = (pEnvelopeIpe.XMax - pEnvelopeIpe.XMin) / pEnvelopeLayer.Width;
            SCG.LinkedList<string> strLkText = new SCG.LinkedList<string>();

            double dblIpeRed = Convert.ToDouble(fintRed) / 255;
            double dblIpeGreen = Convert.ToDouble(fintGreen) / 255;
            double dblIpeBlue = Convert.ToDouble(fintBlue) / 255;

            int intcount = 0;
            foreach (CPoint cpt in CptLt)
            {

                double dblX = CGeometricMethods.CoordinateTransform(cpt.X, pEnvelopeLayer.XMin, pEnvelopeIpe.XMin, dblFactor);
                double dblY = CGeometricMethods.CoordinateTransform(cpt.Y, pEnvelopeLayer.YMin, pEnvelopeIpe.YMin, dblFactor);

                //string str = "<use name=\"mark/disk(sx)\" pos=\"" + GetStrOfEnglishNumber(dblX) + " " + GetStrOfEnglishNumber(dblY) + "\" size=\"" + strWidth + "\" stroke=\"" + dblIpeRed + " " + dblIpeGreen + " " + dblIpeBlue + "\"/>\n";

                string str = "<use name=\"mark/disk(sx)\" pos=\"" + dblX + " " + dblY + "\" size=\"" + strWidth + "\" stroke=\"" + dblIpeRed + " " + dblIpeGreen + " " + dblIpeBlue + "\"/>\n";

                strLkText.AddLast(str);

                intcount++;
            }


            using (var writer = new System.IO.StreamWriter(pParameterInitialize.strSavePath + "\\" + strFileName + ".txt", true))
            {
                if (blnGroup == true)
                {
                    writer.Write("<group>\n");
                }

                foreach (string str in strLkText)
                {
                    writer.Write(str);
                }

                if (blnGroup == true)
                {
                    writer.Write("</group>\n");
                }
            }
        }



        /// <summary>
        /// SaveToIpe
        /// </summary>
        /// <param name="CPlLt"></param>
        /// <param name="strWidth">normal, heavier, fat, ultrafat</param>
        public static string SaveToIpeIpl(IFeatureLayer pFeatureLayer, string strFileName, IEnvelope pEnvelopeLayer, CEnvelope pEnvelopeIpe, CParameterInitialize pParameterInitialize)
        {
            string str = "";
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            int intFeatureCount = pFeatureClass.FeatureCount(null);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    //ע��˴��Ĳ���(****,false)������            

            //IGeoFeatureLayer pGeoFeaturelayer = pFeatureLayer as IGeoFeatureLayer;
            //IClassBreaksRenderer pClassBreaksRenderer = pGeoFeaturelayer.Renderer as IClassBreaksRenderer;

            IFeature pFeature = pFeatureCursor.NextFeature();
            //ILookupSymbol pLookupSymbol = pClassBreaksRenderer as ILookupSymbol;
            //pLookupSymbol.LookupSymbol(true, pFeature);

            for (int i = 0; i < intFeatureCount; i++)
            {
                //get the colors
                //ILineSymbol pLinseSymbol = pLookupSymbol.LookupSymbol(true, pFeature) as ILineSymbol;








                ////get the colors
                //ISimpleFillSymbol pSimpleFillSymbol = pLookupSymbol.LookupSymbol(true, pFeature) as ISimpleFillSymbol;
                //IRgbColor pOutlineRgbColor = pSimpleFillSymbol.Outline.Color as IRgbColor;
                //IColor pFillSymbolColor = new RgbColorClass();
                //pFillSymbolColor.RGB = pSimpleFillSymbol.Color.RGB;
                //IRgbColor pFillSymbolRgbColor = pFillSymbolColor as IRgbColor;

                //get the feature
                IPolyline5 ipl = pFeature.Shape as IPolyline5;
                SetZCoordinates(ipl as IPointCollection4);  //set the z coordinates, it may be used in Constructing TIN
                CPolyline cpl = new CPolyline(i, ipl);

                //append the string
                str += CIpeDraw.DrawCpl(cpl, pEnvelopeLayer, pEnvelopeIpe, new CColor(255,0,0));

                pFeature = pFeatureCursor.NextFeature();  //at the last round of this loop, pFeatureCursor.NextFeature() will return null
            }

            return str;
        }

        /// <summary>
        /// SaveToIpe
        /// </summary>
        /// <param name="CPlLt"></param>
        /// <param name="strWidth">normal, heavier, fat, ultrafat</param>
        public static string SaveToIpeIpg(IFeatureLayer pFeatureLayer, string strFileName, IEnvelope pEnvelopeLayer, CEnvelope pEnvelopeIpe, CParameterInitialize pParameterInitialize)
        {
            string str = "";
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            int intFeatureCount = pFeatureClass.FeatureCount(null);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    //ע��˴��Ĳ���(****,false)������            

            IGeoFeatureLayer pGeoFeaturelayer = pFeatureLayer as IGeoFeatureLayer;
            IClassBreaksRenderer pClassBreaksRenderer = pGeoFeaturelayer.Renderer as IClassBreaksRenderer;

            IFeature pFeature = pFeatureCursor.NextFeature();
            ILookupSymbol pLookupSymbol = pClassBreaksRenderer as ILookupSymbol;
            pLookupSymbol.LookupSymbol(true, pFeature);

            for (int i = 0; i < intFeatureCount; i++)
            {
                //get the colors
                ISimpleFillSymbol pSimpleFillSymbol = pLookupSymbol.LookupSymbol(true, pFeature) as ISimpleFillSymbol;
                IRgbColor pOutlineRgbColor = pSimpleFillSymbol.Outline.Color as IRgbColor;                
                IColor pFillSymbolColor = new RgbColorClass();
                pFillSymbolColor.RGB = pSimpleFillSymbol.Color.RGB;
                IRgbColor pFillSymbolRgbColor = pFillSymbolColor as IRgbColor;

                //get the feature
                IPolygon4 ipg = pFeature.Shape as IPolygon4;
                //SetZCoordinates(ipg as IPointCollection4);  //set the z coordinates, it may be used in Constructing TIN
                CPolygon cpg = new CPolygon(i, ipg as IPolygon4);

                //append the string
                str += CIpeDraw.DrawCpg(cpg, pEnvelopeLayer, pEnvelopeIpe, new CColor(pOutlineRgbColor), new CColor(pFillSymbolRgbColor), pSimpleFillSymbol.Outline.Width.ToString());

                pFeature = pFeatureCursor.NextFeature();  //at the last round of this loop, pFeatureCursor.NextFeature() will return null
            }

            return str;            
        }








        /// <summary>
        /// Coder:   ��  ˬ
        /// Date:    2008-10-16
        /// Content: �ú������ݴ����·��������һ�������ռ䣬��������·�������ڣ��򴴽�һ��·��
        /// </summary>
        /// <param name="path">�������빤���ռ��·��</param>
        /// <returns></returns>
        /// <remarks>please consider using CHelperFunction.SetSavePath(ParameterInitialize)</remarks>
        public static IWorkspace OpenWorkspace(string path)
        {
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspace pWorkspace;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0);
            return pWorkspace;
        }


        public static long GetDirectoryLength(string dirPath)
        {
            //�жϸ�����·���Ƿ����,������������˳�
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;

            //����һ��DirectoryInfo����
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //ͨ��GetFiles����,��ȡdiĿ¼�е������ļ��Ĵ�С
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            //��ȡdi�����е��ļ���,���浽һ���µĶ���������,�Խ��еݹ�
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }


        public static IEnumerable<T> GetTEbFromIGeoCol<T>(IGeometryCollection pGeoCol)
        {
            for (int i = 0; i < pGeoCol.GeometryCount; i++)
            {
                yield return (T)pGeoCol.get_Geometry(i);
            }
        }


        /// <summary>
        /// ��ʾ�����ص�����ֵ�߶�
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��ֵ�߶�</returns>
        public static CPolyline DisplayInterpolation(CDataRecords pDataRecords, double dblProportion)
        {
            if (dblProportion < 0 || dblProportion > 1)
            {
                MessageBox.Show("��������ȷ������");
                return null;
            }
            List<CPoint> CResultPtLt = pDataRecords.ParameterResult.CResultPtLt;
            CPolyline cpl = CGeometricMethods.GetTargetcpl(CResultPtLt, dblProportion);
            cpl.SetPolyline();
            // ����滭�ۼ�
            IMapControl4 m_mapControl = pDataRecords.ParameterInitialize.m_mapControl;
            IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            //m_mapControl.ActiveView.Refresh();   //��������һ����ViewPolyline������ˢ�µ����������ʡ��
            ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�
            return cpl;
        }

        /// <summary>
        /// ��ʾ�����ض����ֵ�߶�
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��ֵ�߶�</returns>
        public static List<CPolyline> DisplayInterpolations(CDataRecords pDataRecords, double dblProportion)
        {
            if (dblProportion < 0 || dblProportion > 1)
            {
                MessageBox.Show("��������ȷ������");
                return null;
            }
            List<List<CPoint>> CResultPtLtLt = pDataRecords.ParameterResult.CResultPtLtLt;

            List<CPolyline> cpllt = new List<CPolyline>();
            for (int i = 0; i < CResultPtLtLt.Count; i++)
            {
                CPolyline cpl = CGeometricMethods.GetTargetcpl(CResultPtLtLt[i], dblProportion);
                cpllt.Add(cpl);
            }


            // ����滭�ۼ�
            IMapControl4 m_mapControl = pDataRecords.ParameterInitialize.m_mapControl;
            IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            //m_mapControl.ActiveView.Refresh();   //��������һ����ViewPolyline������ˢ�µ����������ʡ��
            ViewPolylines(m_mapControl, cpllt);  //��ʾ���ɵ��߶�
            return cpllt;
        }

        /// <summary>
        /// we set move vector so that we can easily get intermediate results and save to excel
        /// </summary>
        public static void SetMoveVectorForCorrCptsLtLt(List<List<CCorrCpts>> CorrCptsLtLt)
        {
            CorrCptsLtLt.ForEach(CorrCptsLt => CorrCptsLt.ForEach(CorrCpts => CorrCpts.SetMoveVector()));
        }

        public static void SetMoveVectorForCorrCptsLt(List<CCorrCpts> CorrCptsLt)
        {
            CorrCptsLt.ForEach(CorrCpts => CorrCpts.SetMoveVector());
        }

        /// <summary>
        /// ��ʾ�����ص�����ֵ�߶�
        /// </summary>
        /// <param name="pDataRecords">���ݼ�¼</param>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��ֵ�߶�</returns>
        public static List<CPolyline> GetAndSaveInterpolation(CDataRecords pDataRecords, double dblProportion)
        {
            if (dblProportion < 0 || dblProportion > 1)
            {
                MessageBox.Show("the parameter t is not acceptable!");
                return null;
            }

            CMorphingBaseCpl pMorphingBaseCpl = pDataRecords.ParameterResult.pMorphingBaseCpl;
            List<CPolyline> pInterpolatedCPlLt = pMorphingBaseCpl.GenerateInterpolatedLt(dblProportion);

            CParameterInitialize pParameterInitialize = pDataRecords.ParameterInitialize;
            CHelperFunction.SaveCPlLt(pInterpolatedCPlLt, pParameterInitialize.strSaveFolder + "____" + dblProportion.ToString(), pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);

            return pInterpolatedCPlLt;
        }


        ///// <summary>
        ///// ��ʾ�����ص�����ֵ�߶�
        ///// </summary>
        ///// <param name="pDataRecords">���ݼ�¼</param>
        ///// <param name="dblProportion">��ֵ����</param>
        ///// <returns>��ֵ�߶�</returns>
        //public static List<CRiver> DisplayRiverLt(CDataRecords pDataRecords, double dblProportion)
        //{
        //    if (dblProportion < 0 || dblProportion > 1)
        //    {
        //        MessageBox.Show("��������ȷ������");
        //        return null;
        //    }
        //    List<CRiver> CResultRiverLt = pDataRecords.ParameterResult.CResultRiverLt;
        //    List<CRiver> CDisplayRiverLt = new List<CRiver>();
        //    List<CPolyline> CDisplayPlLt = new List<CPolyline>();
        //    for (int i = 0; i < CResultRiverLt.Count; i++)
        //    {
        //        if (CResultRiverLt[i].dblWeightinessUnitary > dblProportion)
        //        {
        //            CDisplayRiverLt.Add(CResultRiverLt[i]);
        //            CPolyline cpl = new CPolyline(CResultRiverLt[i]);
        //            CDisplayPlLt.Add(cpl);
        //        }
        //    }

        //    // ����滭�ۼ�
        //    IMapControl4 m_mapControl = pDataRecords.ParameterInitialize.m_mapControl;
        //    IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
        //    pGra.DeleteAllElements();
        //    m_mapControl.ActiveView.Refresh();
        //    ViewPolylines(m_mapControl, CDisplayPlLt);  //��ʾ���ɵ��߶�
        //    return CDisplayRiverLt;
        //}




        /// <summary>
        /// ��ȡ��Ӧ�߶�
        /// </summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param>
        /// <param name="pCorrespondBendLt">��Ӧ�����б�</param>
        /// <returns>��Ӧ�߶�</returns>
        /// <remarks></remarks>
        public static SCG.LinkedList<CCorrespondSegment> DetectCorrespondSegment(CPolyline frcpl, CPolyline tocpl, List<CCorrespondBend> pCorrespondBendLt)
        {
            //��ȡ��Ӧ��������β��Ϊ��Ӧ������
            SortedList<double, CCorrCpts> pCorrespondingCptSlt = new SortedList<double, CCorrCpts>(new CCompareDbl());

            CCorrCpts pStartCorrespondingCpt0 = new CCorrCpts(frcpl.CptLt[0], tocpl.CptLt[0]);//��һ�Զ�Ӧ������
            CCorrCpts pEndCorrespondingCpt0 = new CCorrCpts(frcpl.CptLt[frcpl.CptLt.Count - 1], tocpl.CptLt[tocpl.CptLt.Count - 1]);//�ڶ��Զ�Ӧ������
            pCorrespondingCptSlt.Add(0, pStartCorrespondingCpt0);
            pCorrespondingCptSlt.Add(1, pEndCorrespondingCpt0);

            //������Ӧ������
            for (int i = 0; i < pCorrespondBendLt.Count; i++)
            {
                CCorrCpts pStartCorrespondingCpt = new CCorrCpts(pCorrespondBendLt[i].CFromBend.CptLt[0], pCorrespondBendLt[i].CToBend.CptLt[0]);
                CCorrCpts pEndCorrespondingCpt = new CCorrCpts(pCorrespondBendLt[i].CFromBend.CptLt[pCorrespondBendLt[i].CFromBend.CptLt.Count - 1], pCorrespondBendLt[i].CToBend.CptLt[pCorrespondBendLt[i].CToBend.CptLt.Count - 1]);

                pCorrespondingCptSlt.Add(pCorrespondBendLt[i].CFromBend.dblStartRL, pStartCorrespondingCpt);
                pCorrespondingCptSlt.Add(pCorrespondBendLt[i].CFromBend.dblEndRL, pEndCorrespondingCpt);
            }


            //���Ҳ�ɾ���ظ���Ӧ������
            for (int i = pCorrespondingCptSlt.Count - 1; i > 0; i--)
            {
                CPoint frcpt2 = pCorrespondingCptSlt.Values[i].FrCpt;
                CPoint tocpt2 = pCorrespondingCptSlt.Values[i].ToCpt;
                CPoint frcpt1 = pCorrespondingCptSlt.Values[i - 1].FrCpt;
                CPoint tocpt1 = pCorrespondingCptSlt.Values[i - 1].ToCpt;
                if (frcpt1.Equals2D(frcpt2) && tocpt1.Equals2D(tocpt2))
                {
                    pCorrespondingCptSlt.RemoveAt(i);
                }
            }

            //�Զ�Ӧ������Ϊ�ϵ�ָ�ԭʼ�߶Σ��õ���Ӧ�߶�
            SCG.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = new SCG.LinkedList<CCorrespondSegment>();

            //�м�Ķ�Ӧ�߶�
            for (int i = 0; i < pCorrespondingCptSlt.Count - 1; i++)
            {
                CPolyline frSegment = frcpl.GetSubPolyline(pCorrespondingCptSlt.Values[i].FrCpt, pCorrespondingCptSlt.Values[i + 1].FrCpt);
                CPolyline toSegment = tocpl.GetSubPolyline(pCorrespondingCptSlt.Values[i].ToCpt, pCorrespondingCptSlt.Values[i + 1].ToCpt);
                pCorrespondSegmentLk.AddLast(new CCorrespondSegment(frSegment, toSegment));
            }

            return pCorrespondSegmentLk;
        }

        /// <summary>
        /// ��ResultPtlt��ʽ�Ľ��ת��ΪCorrespondPtlt��ʽ�Ľ��
        /// </summary>
        /// <param name="cresultptlt">ResultPtlt��ʽ�Ľ��</param>
        /// <returns>CorrespondPtlt��ʽ�Ľ��</returns>
        /// <remarks></remarks>
        public static List<CCorrCpts> TransferResultptltToCorrCptsLt(List<CPoint> cresultptlt)
        {
            List<CCorrCpts> pCorrCptslt = new List<CCorrCpts>(cresultptlt.Count);
            for (int i = 0; i < cresultptlt.Count; i++)
            {
                CPoint frcpt = cresultptlt[i];
                for (int j = 0; j < cresultptlt[i].CorrespondingPtLt.Count; j++)
                {
                    CPoint tocpt = cresultptlt[i].CorrespondingPtLt[j];

                    //Ϊ�˸�ԭ���ĵ�Ͼ���ϵ���Բ�������Ҫ���鷳���˴��������ɵ�
                    CPoint newfrcpt = new CPoint(frcpt.ID, frcpt.X, frcpt.Y, frcpt.Z);
                    CPoint newtocpt = new CPoint(tocpt.ID, tocpt.X, tocpt.Y, frcpt.Z);
                    CCorrCpts pCorrCpts = new CCorrCpts(newfrcpt, newtocpt);
                    pCorrCptslt.Add(pCorrCpts);
                }
            }
            return pCorrCptslt;
        }




        public static void CompareAndOrder<T, TOrder>(T T1, T T2, Func<T, TOrder> orderFunc, out T minT, out T maxT, IComparer<TOrder> cmp = null)
        {
            if (cmp == null) { cmp = SCG.Comparer<TOrder>.Default; }
            if (cmp.Compare(orderFunc(T1), orderFunc(T2)) <= 0)
            {
                minT = T1;
                maxT = T2;
            }
            else
            {
                minT = T2;
                maxT = T1;
            }
        }

        //public static void Swap<T>(ref T T1, ref T T2)
        //{
        //    T TempT = T1;
        //    T1 = T2;
        //    T2 = TempT;
        //}

        /// <summary>
        /// compare two variables according to a specified attribute and return the smaller one 
        /// </summary>
        /// <typeparam name="T">the type of the variables</typeparam>
        /// <typeparam name="TOrder">the type of the specified attribute</typeparam>
        /// <param name="T1">variable 1</param>
        /// <param name="T2">variable 2</param>
        /// <param name="orderFunc">the specified attribute of type T</param>
        /// <param name="cmp">compare function</param>
        /// <returns>the smaller varaible according to the specified attribute</returns>
        public static T Min<T, TOrder>(T T1, T T2, Func<T, TOrder> orderFunc, IComparer<TOrder> cmp = null)
        {
            if (cmp == null) { cmp = SCG.Comparer<TOrder>.Default; }

            if (cmp.Compare(orderFunc(T1), orderFunc(T2)) < 0)
            {
                return T1;
            }
            else
            {
                return T2;
            }            
        }



        
    }
}
