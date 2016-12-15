using System;
using System.Collections.Generic;
using System.Text;

using MorphingClass.CGeometry ;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace MorphingClass.CUtility
{
    public class CParameterVariable
    {
        private int _intIndex;
        private string _strPolyline;
        private CPolyline _CPolyline;
        private IFeatureLayer _pFeatureLayer;   //Ҫ��ͼ��(һ��Ϊ��Ҫ�ؼ����������Σ���������CDT)
        private string _strName;
        private double _dblVerySmall;


        /// <summary>���ԣ����</summary>
        public int intIndex
        {
            get { return _intIndex; }
            set { _intIndex = value; }
        }

        /// <summary>���ԣ���Сֵ</summary>
        public double dblVerySmall
        {
            get { return _dblVerySmall; }
            set { _dblVerySmall = value; }
        }

        /// <summary>���ԣ�����</summary>
        public string strName
        {
            get { return _strName; }
            set { _strName = value; }
        }

        /// <summary>���ԣ��߶�����</summary>
        public string strPolyline
        {
            get { return _strPolyline; }
            set { _strPolyline = value; }
        }

        /// <summary>���ԣ���Ҫ��</summary>
        public CPolyline CPolyline
        {
            get { return _CPolyline; }
            set { _CPolyline = value; }
        }

        /// <summary>���ԣ�Ҫ��ͼ��(һ��Ϊ��Ҫ�ؼ����������Σ���������CDT)</summary>
        public IFeatureLayer pFeatureLayer
        {
            get { return _pFeatureLayer; }
            set { _pFeatureLayer = value; }
        }



        public CParameterVariable(int vintIndex, string vstrPolyline)
        {
            _intIndex = vintIndex;
            _strPolyline = vstrPolyline;
        }

        public CParameterVariable(CPolyline vCPolyline, string vstrName, IFeatureLayer vpFeatureLayer,double vdblVerySmall)
        {
            _CPolyline = vCPolyline;
            _strName = vstrName;
            _pFeatureLayer = vpFeatureLayer;
            _dblVerySmall = vdblVerySmall;
        }

        public CParameterVariable(CPolyline vCPolyline, string vstrName, double vdblVerySmall)
        {
            _CPolyline = vCPolyline;
            _strName = vstrName;
            _dblVerySmall = vdblVerySmall;
        }

        public void CreateLayerByCpl()
        {


        }
    }
}
