using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;

using ESRI.ArcGIS.Display;

namespace MorphingClass.CUtility
{
    public class CFrmOperation
    {
        private CParameterInitialize _ParameterInitialize;  //�洢��ʼ���Ĳ���
        //private CSpatialHelperFunction _SpatialHelperFunction;

        //���ԣ���ʼ������
        public CParameterInitialize ParameterInitialize
        {
            get { return _ParameterInitialize; }
            set { _ParameterInitialize = value; }
        }

        /// <summary>���캯��</summary>
        public CFrmOperation(ref CParameterInitialize pParameterInitialize)
        {
            _ParameterInitialize = pParameterInitialize;
            GetAllLayers(pParameterInitialize);
            LoadTocbo(pParameterInitialize);

            //if (ParameterInitialize.cboMorphingMethod !=null )
            //{
            //    ParameterInitialize.cboMorphingMethod.SelectedIndex = 0;
            //}
            //_SpatialHelperFunction = new CSpatialHelperFunction();
        }

        private void GetAllLayers(CParameterInitialize pParameterInitialize)
        {
            pParameterInitialize.pMap = pParameterInitialize.m_mapControl.Map;
            IMap mapFeature=new MapClass (); 

            for (int i = pParameterInitialize.pMap.LayerCount - 1; i >= 0; i--) //��ȡ���е�Ҫ��ͼ�㣨����֮��ġ�AddLayer���������ǽ��µ�ͼ����ڵ�һ��λ�ã���������Ӻ��濪ʼ������
            {
                ILayer pLayer = pParameterInitialize.pMap.get_Layer(i);
                RecursivelyGetLayers(pLayer, ref mapFeature);
            }

            pParameterInitialize.m_mapFeature = mapFeature;
        }

        private void RecursivelyGetLayers(ILayer pLayer, ref IMap mapFeature)
        {
            if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
            {
                ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                for (int j = pComLayer.Count - 1; j >= 0; j--)
                {
                   ILayer psubLayer=  pComLayer.get_Layer(j);
                   psubLayer.Visible = (psubLayer.Visible && pLayer.Visible);    //we do this because it's helpful for exporting data to Ipe
                   RecursivelyGetLayers(psubLayer, ref mapFeature);
                }
            }
            else
            {
                if (pLayer is IFeatureLayer)  //�Ƿ�ΪҪ��ͼ��
                {
                    mapFeature.AddLayer(pLayer);
                }
            }
        }



        private void LoadTocbo(CParameterInitialize pParameterInitialize)
        {
            if (pParameterInitialize.cboLayerLt == null )
            {
                return;
            }

            IMap pm_mapFeature = pParameterInitialize.m_mapFeature;
            IEnumLayer pEnumlayer = pm_mapFeature.Layers;
            string[] astrLayerName = new string[pm_mapFeature.LayerCount];
            int intIndex = 0;
            while (intIndex<pm_mapFeature.LayerCount)
            {
              astrLayerName[intIndex]=  pEnumlayer.Next().Name;
              intIndex++;
            }

            int intSelect = 0;
            foreach (ComboBox  cboLayer in pParameterInitialize .cboLayerLt )
            {
                cboLayer.Items.AddRange(astrLayerName);
                cboLayer.SelectedIndex = intSelect;
                intSelect++;
            }
        }


        /// <summary>ʵ��Frm_Load�ķ���</summary>
        /// <remarks>��һ���Frm_Load�ķ���</remarks>
        public void FrmLoad()
        {
            _ParameterInitialize.cboLayer.Items.Clear();//ͼ���б��

            for (int i = _ParameterInitialize.pMap.LayerCount - 1; i >= 0; i--) //��ȡ���е�Ҫ��ͼ�㣨����֮��ġ�AddLayer���������ǽ��µ�ͼ����ڵ�һ��λ�ã���������Ӻ��濪ʼ������
            {
                ILayer pLayer = _ParameterInitialize.pMap.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    bool isVisible = pLayer.Visible;
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = pComLayer.Count - 1; j >= 0; j--)
                    {
                        if (isVisible==false)
                        {
                            pComLayer.get_Layer(j).Visible = false;
                        }
                        
                        if (pComLayer.get_Layer(j) is IFeatureLayer)  //�Ƿ�ΪҪ��ͼ��
                        {
                            _ParameterInitialize.m_mapFeature.AddLayer(pComLayer.get_Layer(j));
                            _ParameterInitialize.cboLayer.Items.Insert(0, pComLayer.get_Layer(j).Name);
                        }
                    }
                }
                else
                {
                    if (pLayer is IFeatureLayer)  //�Ƿ�ΪҪ��ͼ��
                    {
                        _ParameterInitialize.m_mapFeature.AddLayer(pLayer);
                        _ParameterInitialize.cboLayer.Items.Insert(0, pLayer.Name);
                    }
                }
            }
            if (_ParameterInitialize.cboLayer.Items.Count > 0)
                _ParameterInitialize.cboLayer.SelectedIndex = 0;//Ĭ��ѡȡ��һ��Ҫ��ͼ��
        }

        /// <summary>ʵ��Frm_Load�ķ���</summary>
        /// <remarks>����ͼ��ѡ���</remarks>
        public void FrmLoadMulticbo()
        {
            _ParameterInitialize.cboLargerScaleLayer.Items.Clear();//ͼ���б��
            _ParameterInitialize.cboSmallerScaleLayer.Items.Clear();//ͼ���б��

            for (int i = _ParameterInitialize.pMap.LayerCount - 1; i >= 0; i--) //��ȡ���е�Ҫ��ͼ�㣨����֮��ġ�AddLayer���������ǽ��µ�ͼ����ڵ�һ��λ�ã���������Ӻ��濪ʼ������
            {
                ILayer pLayer = _ParameterInitialize.pMap.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    bool isVisible = pLayer.Visible;
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = pComLayer.Count - 1; j >= 0; j--)
                    {
                        if (isVisible == false)
                        {
                            pComLayer.get_Layer(j).Visible = false;
                        }
                        //_ParameterInitialize.m_mapAll.AddLayer(pComLayer.get_Layer(j));
                        if (pComLayer.get_Layer(j) is IFeatureLayer)  //�Ƿ�ΪҪ��ͼ��
                        {
                            IFeatureLayer pFeaturelayer = (IFeatureLayer)pComLayer.get_Layer(j);
                            _ParameterInitialize.cboLargerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                            _ParameterInitialize.cboSmallerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                            _ParameterInitialize.m_mapFeature.AddLayer(pFeaturelayer);
                        }
                    }
                }
                else
                {
                    if (pLayer is IFeatureLayer)
                    {
                        _ParameterInitialize.m_mapFeature.AddLayer(pLayer);
                        _ParameterInitialize.cboLargerScaleLayer.Items.Insert(0, pLayer.Name);
                        _ParameterInitialize.cboSmallerScaleLayer.Items.Insert(0, pLayer.Name);

                        //IFeatureLayer pFeaturelayer = (IFeatureLayer)pLayer;
                        //if (pFeaturelayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                        //{
                        //    _ParameterInitialize.cboLargerScaleLayer.Items.Add(pFeaturelayer.Name);
                        //    _ParameterInitialize.cboSmallerScaleLayer.Items.Add(pFeaturelayer.Name);
                        //    _ParameterInitialize.m_mapPolyline.AddLayer(pFeaturelayer);
                        //}
                    }
                }
            }

            if (_ParameterInitialize.cboLargerScaleLayer.Items.Count > 1 && _ParameterInitialize.cboSmallerScaleLayer.Items.Count > 1)
            {
                _ParameterInitialize.cboLargerScaleLayer.SelectedIndex = 0;//Ĭ��ѡȡ��һ����Ҫ��ͼ��
                _ParameterInitialize.cboSmallerScaleLayer.SelectedIndex = 1;//Ĭ��ѡȡ��һ����Ҫ��ͼ��
            }


        }

        /// <summary>ʵ��Frm_Load�ķ���</summary>
        /// <remarks>����ͼ��ѡ���</remarks>
        public void FrmLoadThreecbo()
        {
            _ParameterInitialize.cboLargerScaleLayer.Items.Clear();//ͼ���б��
            _ParameterInitialize.cboSmallerScaleLayer.Items.Clear();//ͼ���б��
            _ParameterInitialize.cboLayer.Items.Clear();

            for (int i = _ParameterInitialize.pMap.LayerCount - 1; i >= 0; i--) //��ȡ���е�Ҫ��ͼ��
            {
                ILayer pLayer = _ParameterInitialize.pMap.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    bool isVisible = pLayer.Visible;
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = pComLayer.Count - 1; j >= 0; j--)
                    {
                        if (isVisible == false)
                        {
                            pComLayer.get_Layer(j).Visible = false;
                        }
                        if (pComLayer.get_Layer(j) is IFeatureLayer)  //�Ƿ�ΪҪ��ͼ��
                        {
                            IFeatureLayer pFeaturelayer = (IFeatureLayer)pComLayer.get_Layer(j);
                            _ParameterInitialize.m_mapFeature.AddLayer(pFeaturelayer);

                            _ParameterInitialize.cboLargerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                            _ParameterInitialize.cboSmallerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                            _ParameterInitialize.cboLayer.Items.Insert(0, pFeaturelayer.Name);
                        }
                    }
                }
                else
                {
                    if (pLayer is IFeatureLayer)
                    {
                        IFeatureLayer pFeaturelayer = (IFeatureLayer)pLayer;
                        _ParameterInitialize.m_mapFeature.AddLayer(pFeaturelayer);

                        _ParameterInitialize.cboLargerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                        _ParameterInitialize.cboSmallerScaleLayer.Items.Insert(0, pFeaturelayer.Name);
                        _ParameterInitialize.cboLayer.Items.Insert(0, pFeaturelayer.Name);
                    }
                }
            }

            if (_ParameterInitialize.cboLargerScaleLayer.Items.Count > 1 && _ParameterInitialize.cboSmallerScaleLayer.Items.Count > 1)
            {
                _ParameterInitialize.cboLargerScaleLayer.SelectedIndex = 0;//Ĭ��ѡȡ��һ����Ҫ��ͼ��
                _ParameterInitialize.cboSmallerScaleLayer.SelectedIndex = 1;//Ĭ��ѡȡ��һ����Ҫ��ͼ��
                _ParameterInitialize.cboLayer.SelectedIndex = 2;
            }

        }
    }
}
